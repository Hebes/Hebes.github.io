# c#使用UDP进行聊天通信

**[文章来源](<https://blog.csdn.net/u012563853/article/details/126307114>)**

UDP和TCP都是网络通信中不可缺少的部分，两者在不同的环境中，应用的场景不一样，UDP在网络不好的情况下，传输会丢包，也就是会丢数据，而TCP不会这样，所以重要的数据使用TCP传输，但是TCP对网络的资源消耗非常的大，例如视频，音频等大量的数据，这个时候就选择UDP，因为UDP占用网络资源比较低，就算丢一帧二帧的图像的数据，也不会有影响的。UDP只管发送，不管你有没有接收到信息，比较主动，同理，也会一直接收，只要在线，就能接受对方的信息。UDP比TCP的使用更加的简单。

UDP通信可以分为使用UDP和UdpClient。UdpClient是Socket的一种封装。其实UDP通信没有绝对的服务端和客户端分别，因为都是连接上，就可以发送和接收。

使用UDP方式，需要注意发送消息的IP地址和端口与接收消息的IP地址和端口。

服务端代码

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //udp 服务端
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6001));//绑定端口号和IP
            Console.WriteLine("服务端已经开启");
            Thread t = new Thread(()=>ReciveMsg(server));//开启接收消息线程
            t.Start();
            Thread t2 = new Thread(()=>sendMsg(server));//开启发送消息线程
            t2.Start();


        }
        static void sendMsg(Socket server)
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);    //向指定的IP和端口发送消息
            while (true)
            {
                string msg = Console.ReadLine();
                server.SendTo(Encoding.UTF8.GetBytes(msg), point);
            }
        }
        static void ReciveMsg(Socket server)
        {
            while (true)
            {
                //EndPoint point = new IPEndPoint(IPAddress.Any, 0);                   //向所有的IP和端口接收消息
                EndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);   //向指定的IP和端口接收消息
                byte[] buffer = new byte[1024];
                int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine("收到了消息：" + message);
            }
        }
 
    }
}
```

客户端代码

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            client.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000));
            Thread t = new Thread(() => sendMsg(client));
            t.Start();
            Thread t2 = new Thread(() => ReciveMsg(client));
            t2.Start();
            Console.WriteLine("客户端已经开启");
        }

        static void sendMsg(Socket client)
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6001);    //向指定的IP和端口发送消息
            while (true)
            {
                string msg = Console.ReadLine();
                client.SendTo(Encoding.UTF8.GetBytes(msg), point);
            }
        }

        static void ReciveMsg(Socket client)
        {
            while (true)
            {
                //EndPoint point = new IPEndPoint(IPAddress.Any, 0);                        //向所有的IP和端口接收消息
                EndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6001);        //向指定的IP和端口接收消息
                byte[] buffer = new byte[1024];
                int length = client.ReceiveFrom(buffer, ref point);//接收数据报
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine("收到了消息：" + message);
            }
        }

    }
}
```

效果预览

![](https://img-blog.csdnimg.cn/d1ae196282b94a3c955b5f8f6199a279.gif)

使用UdpClient方式，需要注意发送消息的IP地址和端口与接收消息的IP地址和端口

服务端代码

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建udpclient 绑定ip跟端口号
            UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8090));
            Console.WriteLine("UdpClient服务端已经开启");

            Thread t = new Thread(() => ReciveMsg(udpClient));//开启接收消息线程
            t.Start();
            Thread t2 = new Thread(() => sendMsg(udpClient));//开启发送消息线程
            t2.Start();

        }
        static void sendMsg(UdpClient udpClient)
        {
            while (true)
            {
                string message = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(message);
                udpClient.Send(data, data.Length, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8091));   //发送到指定的IP地址和端口信息
            }
        }
        static void ReciveMsg(UdpClient udpClient)
        {
            while (true)
            {
                //接收数据
                //IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);                     //接收所有的IP地址和端口信息
                IPEndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8091);     //接收指定的IP地址和端口信息
                byte[] data = udpClient.Receive(ref point);// 
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine("收到了消息：" + message);
            }
        }

    }
}
```

客户端代码

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建udpclient对象
            UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8091));
            Console.WriteLine("UdpClient客户端已经开启");

            Thread t = new Thread(() => sendMsg(client));
            t.Start();
            Thread t2 = new Thread(() => ReciveMsg(client));
            t2.Start();
        }

        static void sendMsg(UdpClient client)
        {
            while (true)
            {
                string message = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(message);
                client.Send(data, data.Length, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8090));
            }
        }

        static void ReciveMsg(UdpClient udpClient)
        {
            while (true)
            {
                //接收数据
                //IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);                      //接收所有的IP地址和端口信息
                IPEndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8090);      //接收指定的IP地址和端口信息
                byte[] data = udpClient.Receive(ref point); 
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine("收到了消息：" + message);
            }
        }

    }
}
```

效果预览

![](https://img-blog.csdnimg.cn/5519551353da4b5a835aafa0d58cdfdd.gif)

从以上的案例来看，UDP和UdpClient的区别基本上都是一样的，一个使用了Socket 类，一个使用了UdpClient类，然后都是绑定对应的IP地址和端口，然后就是分别调用Socket类的方法，进行发送消息和接收消息，调用UdpClient类的方法，进行发送消息和接收信息。其实在新建类的时候，可以不用先进行连接，可以把IP地址和端口，以及消息一次性发出去。

拓展

单播，广播，多播三者的区别

单播

用于两个主机之间的端对端通信，指定了固定的IP地址和端口，就是一对一的对话，其他人听不到你们说的话，类似私聊。

代码：就是上面的代码，指定了固定的IP地址和端口号，发送和接收都互相对应。

广播

用于一个主机对整个局域网上所有主机上的数据通信，就是一个人大声说话，所有的人都能听到，类似群聊。

代码：修改成下面2句即可，发送端和接收端都要修改。

UDP服务端

![](https://img-blog.csdnimg.cn/e8c1d39e538c40f8b8cb20b387735495.png)

```cs
 server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);    //Broadcast开启广播
 EndPoint point = new IPEndPoint(IPAddress.Broadcast, 4567);    //开启广播，端口是4567
```

UDP客户端

![](https://img-blog.csdnimg.cn/0c0249109960455280e7216c9675545f.png)

```cs
 EndPoint iep = new IPEndPoint(IPAddress.Any, 4567);      //任意ip，端口就是服务端的4567
 EndPoint point = new IPEndPoint(IPAddress.Any, 4567);        //广播，端口是4567
```

UdpClient服务端

![](https://img-blog.csdnimg.cn/3c58eed3860148d3a081061d3cf71d2e.png)

```cs
            UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
            IPEndPoint point = new IPEndPoint(IPAddress.Broadcast, 4567);    //开启广播，端口是4567
```

UdpClient客户端

![](https://img-blog.csdnimg.cn/e317f273361f496d99d2a6bdeddc7e23.png)

```cs
            UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 4567));
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
```

多播

介于单播和广播之间，也叫组播，从名字上面就能知道，建立一个组，然后向组内的人员发送消息，就类似，微信临时拉一个群，指定群内的人，向群内通知信息。

代码：修改成下面2句即可，发送端和接收端都要修改。

加入多播

```cs
                udpClient.JoinMulticastGroup(IPAddress.Parse("224.0.0.0"));//将 UdpClient 添加到多播组;IPAddress.Parse将IP地址字符串转换为IPAddress 实例
                IPEndPoint multicast = new IPEndPoint(IPAddress.Parse("224.0.0.0"), 7788); //将网络终结点表示为 IP 地址和端口号  7788是目的端口
```

退出多播

```cs
udpClient.DropMulticastGroup(IPAddress.Parse("224.0.0.0"));//将 UdpClient 从多播组中移除;IPAddress.Parse将IP地址字符串转换为IPAddress 实例
                IPEndPoint multicast = new IPEndPoint(IPAddress.Parse("224.0.0.0"), 7788); //将网络终结点表示为 IP 地址和端口号  7788是目的端口
```

带界面操作

![](https://img-blog.csdnimg.cn/09bbb93be95e4fa695fc05d1af5e3f90.png)

代码

```cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;//在其他线程中可以调用主窗体控件
        }
        UdpClient udpClient;
        UdpClient client;
        private void Form1_Load(object sender, EventArgs e)
        {
            Thread js = new Thread(() =>
            {
                client = new UdpClient(new IPEndPoint(IPAddress.Any, 4567));
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                while (true)
                {
                    try
                    {
                        if (client.Available <= 0) continue;
                        if (client.Client == null) return;
                        byte[] bytes = client.Receive(ref iPEndPoint);
                        string str = Encoding.Default.GetString(bytes);
                        //Invoke(new Action(() => textBox2.Text = str));
                        textBox2.Text = str;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    Thread.Sleep(1000);
                }
            });
            js.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
            IPEndPoint point = new IPEndPoint(IPAddress.Broadcast, 4567);    //开启广播，端口是4567
            if (textBox1.Text != null)
            {
                Byte[] sendBytes = Encoding.Default.GetBytes(textBox1.Text);
                udpClient.Send(sendBytes, sendBytes.Length, point);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            udpClient.Dispose();
            udpClient.Close();

            client.Dispose();
            client.Close();
        }
    }
}
```

多网卡广播问题，让本机先指定对应的IP。即发送端，指定对应的IP。

```C#
UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse("192.168.1.100"), 0));
```
