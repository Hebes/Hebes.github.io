# 网络基础,简单的TCP.UDP通信

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

![2](\../Image/UDP2/2.png)

---
