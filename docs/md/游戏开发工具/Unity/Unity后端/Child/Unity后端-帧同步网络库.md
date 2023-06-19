# Unity后端-帧同步网络库

## 简介

KCP是一个快速可靠协议，能以比 TCP 浪费 10%-20% 的带宽的代价，换取平均延迟降低 30%-40%，且最大延迟降低三倍的传输效果。纯算法实现，并不负责底层协议（如UDP）的收发，需要使用者自己定义下层数据包的发送方式，以 callback的方式提供给 KCP。 连时钟都需要外部传递进来，内部不会有任何一次系统调用。

整个协议只有 ikcp.h, ikcp.c两个源文件，可以方便的集成到用户自己的协议栈中。也许你实现了一个P2P，或者某个基于 UDP的协议，而缺乏一套完善的ARQ可靠协议实现，那么简单的拷贝这两个文件到现有项目中，稍微编写两行代码，即可使用。

**[什么是KCP，怎么使用呢！](<https://zhuanlan.zhihu.com/p/339780809>)**

**KCP介绍:**

- [KCP开源地址](<https://github.com/skywind3000/kcp>)
- C#语言实现版本
- 一套高效`确认算法`，可以让数据传输变得`可靠有序`。
- 讲解KCP的处理流程及原理
- 演示KCP丢包`重发机制`
  - kcp包下载及安装
  - 开发演示程序

**基于KCP封装KCPNet实现可靠UDP传输:**

- 会话ID必须由服务器统一分配， 不可重复。
- 实现模拟连接以便识别不同客户端数据。
- 双端复用代码，抽象网络会话类，进行数据收发。

KCPNet工作流程

![1](\../Image/Unity后端-帧同步网络库/1.png)

## 技术特性

TCP是为流量设计的（每秒内可以传输多少KB的数据），讲究的是充分利用带宽。而 KCP是为流速设计的（单个数据包从一端发送到一端需要多少时间），以10%-20%带宽浪费的代价换取了比 TCP快30%-40%的传输速度。TCP信道是一条流速很慢，但每秒流量很大的大运河，而KCP是水流湍急的小激流。KCP有正常模式和快速模式两种，通过以下策略达到提高流速的结果：

RTO翻倍vs不翻倍：
TCP超时计算是RTOx2，这样连续丢三次包就变成RTOx8了，十分恐怖，而KCP启动快速模式后不x2，只是x1.5（实验证明1.5这个值相对比较好），提高了传输速度。

选择性重传 vs 全部重传：
TCP丢包时会全部重传从丢的那个包开始以后的数据，KCP是选择性重传，只重传真正丢失的数据包。

快速重传：
发送端发送了1,2,3,4,5几个包，然后收到远端的ACK: 1, 3, 4, 5，当收到ACK3时，KCP知道2被跳过1次，收到ACK4时，知道2被跳过了2次，此时可以认为2号丢失，不用等超时，直接重传2号包，大大改善了丢包时的传输速度。

延迟ACK vs 非延迟ACK：
TCP为了充分利用带宽，延迟发送ACK（NODELAY都没用），这样超时计算会算出较大 RTT时间，延长了丢包时的判断过程。KCP的ACK是否延迟发送可以调节。

UNA vs ACK+UNA：
ARQ模型响应有两种，UNA（此编号前所有包已收到，如TCP）和ACK（该编号包已收到），光用UNA将导致全部重传，光用ACK则丢失成本太高，以往协议都是二选其一，而 KCP协议中，除去单独的 ACK包外，所有包都有UNA信息。

非退让流控：
KCP正常模式同TCP一样使用公平退让法则，即发送窗口大小由：发送缓存大小、接收端剩余接收缓存大小、丢包退让及慢启动这四要素决定。但传送及时性要求很高的小数据时，可选择通过配置跳过后两步，仅用前两项来控制发送频率。以牺牲部分公平性及带宽利用率之代价，换取了开着BT都能流畅传输的效果。

## 网络基础,简单的TCP.UDP通信

服务端

```C#

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ServerStart
    {
        private static int port = 17333;

        static void Main(string[] arge)
        {
            //CreatTcpSercver();
            CreatUDPSercver();
            Console.ReadKey();
        }

        private static void CreatUDPSercver()
        {
            UdpClient Listener = new UdpClient(port);
            IPEndPoint remoteIP = new IPEndPoint(IPAddress.Any, port);
            try
            {
                while (true)
                {
                    Console.WriteLine("等待消息 ...");
                    byte[] bytes = Listener.Receive(ref remoteIP);
                    Console.WriteLine($"Recived msg from {remoteIP}:");
                    Console.WriteLine($"{Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Listener.Close();
            }

        }

        /// <summary>
        /// Tcp基础连接
        /// </summary>
        private static void CreatTcpSercver()
        {
            TcpListener Listener = new TcpListener(IPAddress.Any, port);
            Listener.Start();
            Console.WriteLine("等待连接... ");
            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();
                Console.WriteLine("接受连接.");
                NetworkStream ns = client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes("server send TCP message");
                try
                {
                    ns.Write(data, 0, data.Length);
                    ns.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}
```

客户端

```C#
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class ClientStart
    {
        private static int port = 17333;
        private static string IP = "192.168.1.100";

        public static void Main(string[] arge)
        {
            //CreatTcpClient();
            CreatUDPClient();
            Console.ReadKey();
        }

        private static void CreatUDPClient()
        {
            UdpClient client = new UdpClient();
            byte[] data = Encoding.ASCII.GetBytes("UDPClient sned message :hello");
            IPEndPoint remoteIP = new IPEndPoint(IPAddress.Parse(IP), port);
            client.Send(data, data.Length, remoteIP);
            Console.WriteLine("Message send to remote address");

        }

        private static void CreatTcpClient()
        {
            try
            {
                var client = new TcpClient(IP, port);
                NetworkStream ns = client.GetStream();
                byte[] data = new byte[1024];
                int Len = ns.Read(data, 0, data.Length);
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, Len));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
```

使用方法:

服务端设置启动项,窗口下面选项直接点击启动

客户端右击->调试->启动新实力

![2](\../Image/Unity后端-帧同步网络库/2.png)

---

## KCP丢包重传算法演示

![3](\../Image/Unity后端-帧同步网络库/3.png)

网络不好请开梯子

创建新项目名称是KCPTest并设置启动项,其中文件如下:

KCPHandle.cs

```C#
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets.Kcp;
using System.Text;
using System.Threading.Tasks;

namespace KCPTest
{
    /// <summary>
    /// KCP的数据处理器
    /// </summary>
    public class KCPHandle : IKcpCallback
    {
        public Action<Memory<byte>> Out;

        /// <summary>
        /// kcp发送出去的数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="avalidLength"></param>
        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            using (buffer)
            {
                Out(buffer.Memory.Slice(0, avalidLength));
            }
        }
    }
}
```

KCPItem.cs

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets.Kcp;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace KCPTest
{
    public class KCPItem
    {
        public string itemName;
        public KCPHandle handle;
        public Kcp kcp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conv"></param>
        /// <param name="itemName">识别客户端和服务器哪个发送过来的数据</param>
        public KCPItem(uint conv, string itemName)
        {
            handle = new KCPHandle();
            kcp = new Kcp(conv, handle);

            kcp.NoDelay(1, 10, 2, 1);
            kcp.WndSize(64, 64);
            kcp.SetMtu(512);

            this.itemName = itemName;
        }

        //2.准备发送的数据进入这边也就是KCP这个部分请结合图片
        /// <summary>
        /// 需要发送的消息放入这里
        /// </summary>
        /// <param name="data"></param>
        public void InputData(Span<byte> data)
        {
            kcp.Input(data);
        }

        public void Set0utCallback(Action<Memory<byte>> itemSender)
        {
            handle.Out = itemSender;
        }


        /// <summary>
        /// 把二进制的数据放入,然后发送数据
        /// </summary>
        /// <param name="data"></param>
        public void SendMsg(byte[] data)
        {
            Console.WriteLine($" {itemName}输入数据: {GetByteString(data)}");
            kcp.Send(data.AsSpan());
        }

        /// <summary>
        /// 驱动KCP
        /// </summary>
        public void Updata()
        {
            kcp.Update(DateTime.UtcNow);
            int len;
            while ((len = kcp.PeekSize()) > 0)
            {
                var buffer = new byte[len];
                if (kcp.Recv(buffer)>=0) {
                    Console.WriteLine($"{itemName}收到数据: {GetByteString(buffer)}");
                }
            }
        }

        static string GetByteString(byte[] bytes)
        {
            string str = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                str += $"\n   [{i}]:{bytes[i]}";
            }
            return str;
        }
    }
}
```

KCPTestStart.cs

```C#
using System.Text;
using System;

namespace KCPTest
{
    public class KCPTestStart
    {
        static string GetByteString(byte[] bytes)
        {
            string str = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                str += $"\n   [{i}]:{bytes[i]}";
            }
            return str;
        }

        public static void Main(string[] arge)
        {
            const uint conv = 123;

            Random rd = new Random();

            KCPItem kcpServer = new KCPItem(conv, "server");
            KCPItem kcpClient = new KCPItem(conv, "client");

            kcpServer.Set0utCallback((Memory<byte> buffer) =>
            {
                kcpClient.InputData(buffer.Span);
            });

            kcpClient.Set0utCallback((Memory<byte> buffer) =>
            {
                int next = rd.Next(100);
                if (next >= 95)//演示丢包率  现在是百分之95丢包
                {
                    Console.WriteLine($"Send Pkg Succ : {GetByteString(buffer.ToArray())}");
                    kcpServer.InputData(buffer.Span);
                }
                else
                {
                    Console. WriteLine("Send Pkg Miss");
                }
            });

            //1.发送data 这条数据
            byte[] data = Encoding.ASCII.GetBytes("www . qiqiker. com");
            kcpClient.SendMsg(data);

            while (true)
            {
                kcpServer.Updata();
                kcpClient.Updata();
                Thread.Sleep(10);
            }
        }
    }
}
```

## KCPNet

创建C#类库 Standard

![4](\../Image/Unity后端-帧同步网络库/4.png)


## 参考

来自
> 齐齐课
