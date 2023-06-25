# 第100天：UDP 编程

## UDP 协议

我们来看 UDP 的定义：

> UDP 协议（User Datagram Protocol），中文名是用户数据报协议，是 OSI（Open System Interconnection，开放式系统互联） 参考模型中一种无连接的传输层协议，提供面向事务的简单不可靠信息传送服务。

从这个定义中，我们可以总结出 UDP 的几个特点以及其与 TCP 的区别：

+ UDP 是用户数据报协议，传输模式是数据报，而 TCP 是基于字节流的传输协议。
+ UDP 是无连接的协议，每个数据报都是一个独立的信息，包括完整的源地址或目的地址，它在网络上以任何可能的路径传往目的地，因此到达目的地，到达目的地的时间以及内容的正确性都是不能被保证的。
+ UDP 是简单不可靠的协议，它不提供可靠性，只是把数据包发送出去，并不保证能够到达目的地。由于它不需要在客户端和服务端之间建立连接，也没有超时重发机制，所以传输速度很快。

从以上特点，我们可以看到 UDP 适合应用在每次传输数据量小、对数据完整性要求不高、对传输速度要求高的领域。这里面最典型的就是即时通信的场景，微信是一个很常见的例子。相信大家在使用微信的时候都遇到过先发的消息后收到，或者有些发送的消息对方没有收到的情况吧，这就是 UDP 协议典型的特点，不保证传输数据的完整性和顺序性。除此之外， UDP 还应用在在线视频、网络电话等场景。

## UDP 传输过程

我们在讲 TCP 的时候，我们说 TCP 客户端和服务端必须先连接才可以传输数据：客户端先请求连接服务器，服务器接受连接请求，然后双方才可以通信。在 UDP 协议里，客户端只需要知道服务器的地址和端口号，就可以直接发送数据了。

我们来看下 UDP 传输的流程图：

![1](../Image/Python-UDP%E7%BC%96%E7%A8%8B/1.png)

TCP服务器的建立可以归纳这几步：

+ 创建 socket（套接字）
+ 绑定 socket 的 IP 地址和端口号
+ 接收客户端数据
+ 关闭连接

TCP客户端的创建可总结为这几步：

+ 创建 socket（套接字）
+ 向服务器发送数据
+ 关闭连接

这里需要注意的是 UDP 客户端连接到服务器的 IP 和端口号必须是 UDP 服务器的 IP 和监听的端口号，服务器服务器只需要绑定 IP 和端口号，就可以时刻准备接收客户端发送的数据，此时服务器处于阻塞状态，直到接收到数据为止。

## UDP 客户端

创建 socket，可以这样做：

```auto
# 导入socket库
import socket

# 创建一个socket
s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
```

创建 socket 时，第一个参数 socket.AF_INET 表示指定使用 IPv4 协议，如果要使用 IPv6 协议，就指定为 socket.AF_INET6。SOCK_DGRAM 指定基于 UDP 的数据报式 Socket 通信。

创建了 socket 之后，我们就可以向目标地址发送数据报了：

```auto
# 发送数据
s.sendto(b'Hello Server', ('127.0.0.1', 6000))
```

第一个参数是需要发送的数据报内容，第二个参数是 IP 地址和端口号的二元组。

如果是接收数据的话，我们可以这样写：

```auto
# 接收数据
data, addr = s.recv(1024)
# 解码接收到的数据
data = data.decode('utf-8')
```

接收信息的时候，第一个 data 表示接收到的数据， addr 是对方的 IP 地址和端口号的二元组。

想要关闭 socket，直接调用 close() 方法即可：

```auto
# 关闭 socket
socket.close()
```

## UDP 服务器

相比于客户端，服务器端只是多了一个步骤，在创建 socket 之后，需要绑定一个 IP 地址和端口号，以便接收客户端随时可能发送过来的数据。绑定的方法为：

```auto
# 绑定 IP 和端口
s.bind(('127.0.0.1', 6000))
```

## UDP 简单实例

我们通过一个简单的实例来体会下 UDP 的客户端和服务器的通信流程。

服务器代码为：

```auto
import socket

# 创建 socket
sk = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
# 绑定 IP 和端口号
sk.bind(('127.0.0.1', 6000))
while True:
    # 接收数据报
    msg, addr = sk.recvfrom(1024)
    # 打印
    print('来自[%s:%s]的消息: %s' % (addr[0], addr[1], msg.decode('utf-8')))

    # 等待输入
    inp = input('>>>')
    # 发送数据报
    sk.sendto(inp.encode('utf-8'), addr)

# 关闭 socket
sk.close()
```

这里，我们先创建 socket，然后绑定本机的6000端口，然后等待接收客户端发送的数据报，接收到数据后将数据内容打印在控制台。然后可以在控制台输入回复内容，发送给客户端。

客户端代码：

```auto
import socket

# 创建 socket
sk = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
addr = ('127.0.0.1', 6000)
while True:
    # 等待输入
    msg = input('>>>')
    # 发送数据报
    sk.sendto(msg.encode('utf-8'), addr)
    # 接收数据报
    msg_recv, addr = sk.recvfrom(1024)
    # 打印
    print(msg_recv.decode('utf-8'))

# 关闭 socket
sk.close()
```

在客户端代码中，我们就只是创建 socket，然后在控制台输入需要向服务器发送的内容，通过 sentto() 方法发送给服务器，然后接收服务器返回的内容，将接收的内容打印到控制台。

分别运行客户端和服务器代码，然后我们在客户端的控制台输入 “hello server”，我们可以看到服务器的控制台打印了客户端发送的内容，然后我们在服务器控制台输入 “hello client”，同样在客户端控制台可以看你到内容。

下面是客户端的控制台内容：

```auto
>>>hello server
hello client
>>>
```

下面是服务器的控制台内容：

```auto
来自[127.0.0.1:61207]的消息: hello server
>>>hello client
```

这个实例其实就是一个简单的聊天模型，客户端和服务器就像两个人一样可以发送和接收对方的信息。

那么多人群聊怎么实现呢？简单来说，我们需要设置一台中心服务器，我们每个人发送的内容都先发送到中心服务器，然后中心服务器再转发到每个群聊的人。

## 总结

本文为大家介绍了 UDP 编程的基本原理以及通过 Python 实现一个最简单的聊天程序来模拟 UDP 通信的过程。通过本文的学习，我们需要对 UDP 协议有基本的认识，以及对 UDP 的通信过程比较熟悉。

> 文中示例代码：<https://github.com/JustDoPython/python-100-day/tree/master/day-100>

## 参考

<https://www.kancloud.cn/smilesb101/python3_x/29888>
