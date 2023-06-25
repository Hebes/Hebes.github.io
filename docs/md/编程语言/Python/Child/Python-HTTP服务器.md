

# Python-HTTP服务器

## 引言

`http.server` 是 `socketserver.TCPServer` 的子类，它在 HTTP 套接字上创建和监听，并将请求分派给处理程序。本文是关于如何使用 Python 的 `http.server` 模块快速地搭建一个简易 HTTP 服务器的教程。

## 安装

Python3 内置标准模块，无需安装。（在之前的 Python2 版本名称是 `SimpleHTTPServer`）

## 教程

### 用命令行创建

> http.server 支持以 Python 解释器的 -m 参数直接调用。

通过执行如下命令创建一个最简单的 HTTP 服务器：

```bash
python -m http.server
```

服务器默认监听端口是 8000，支持自定义端口号：

```bash
python -m http.server 9000
```

服务器默认绑定到所有接口，可以通过 `-b/--bind` 指定地址，如本地主机：

```bash
python -m http.server --bind 127.0.0.1
或者
python -m http.server --bind 0.0.0.0
```

服务器默认工作目录为当前目录，可通过 `-d/--directory` 参数指定工作目录：

```bash
python -m http.server --directory /tmp/
```

此外，可以通过传递参数 `--cgi` 启用 [CGI](https://baike.baidu.com/item/CGI/607810) 请求处理程序：

```bash
python -m http.server --cgi
```

### 编写代码创建

> http.server 也支持在代码中调用，导入对应的类和函数即可。

```python
from http.server import SimpleHTTPRequestHandler
from http.server import CGIHTTPRequestHandler
from http.server import ThreadingHTTPServer
from functools import partial
import contextlib
import sys
import os


class DualStackServer(ThreadingHTTPServer):
    def server_bind(self):
        # suppress exception when protocol is IPv4
        with contextlib.suppress(Exception):
            self.socket.setsockopt(socket.IPPROTO_IPV6, socket.IPV6_V6ONLY, 0)
        return super().server_bind()


def run(server_class=DualStackServer,
        handler_class=SimpleHTTPRequestHandler,
        port=8000,
        bind='127.0.0.1',
        cgi=False,
        directory=os.getcwd()):
    """Run an HTTP server on port 8000 (or the port argument).

    Args:
        server_class (_type_, optional): Class of server. Defaults to DualStackServer.
        handler_class (_type_, optional): Class of handler. Defaults to SimpleHTTPRequestHandler.
        port (int, optional): Specify alternate port. Defaults to 8000.
        bind (str, optional): Specify alternate bind address. Defaults to '127.0.0.1'.
        cgi (bool, optional): Run as CGI Server. Defaults to False.
        directory (_type_, optional): Specify alternative directory. Defaults to os.getcwd().
    """

    if cgi:
        handler_class = partial(CGIHTTPRequestHandler, directory=directory)
    else:
        handler_class = partial(SimpleHTTPRequestHandler, directory=directory)

    with server_class((bind, port), handler_class) as httpd:
        print(
            f"Serving HTTP on {bind} port {port} "
            f"(http://{bind}:{port}/) ..."
        )
        try:
            httpd.serve_forever()
        except KeyboardInterrupt:
            print("\nKeyboard interrupt received, exiting.")
            sys.exit(0)


if __name__ == '__main__':
    run(port=8000, bind='127.0.0.1')
```

+ `server_class`：服务器类
+ `handler_class`：请求处理类
+ `port`：端口
+ `bind`：IP
+ `cgi`：是否启用 CGI 请求处理程序
+ `directory`：工作目录

## 实例

既然我们已经知道了 http.server 能够快速地创建一个 HTTP 服务器，那么它能应用到哪些项目场景？

1. 小型 web 项目在局域网内的预览
   + 项目目录

   ```bash
   web:.
   ├─index.html
   ```

   + index.html

   ```html
   <!DOCTYPE html>
   <html lang="en">
   <head>
       <meta charset="UTF-8">
       <meta http-equiv="X-UA-Compatible" content="IE=edge">
       <meta name="viewport" content="width=device-width, initial-scale=1.0">
       <title>Document</title>
   </head>
   <body>
       hello world
   </body>
   </html>
   ```

   + 切换到目录 `cd web`，执行命令 `python -m http.server`，浏览器地址栏输入 `localhost:8000`，显示：

   ```bash
   hello world
   ```

   > 对于局域网的其他用户，可通过你的主机IP+端口号访问，如你的主机IP是192.168.0.1，那么将网址 192.168.0.1:8000 发送给你的同事或同学，他们也可以看到 index.html 文件渲染的内容。

2. 在本地浏览器访问远程服务器的端口映射

    如果通过 VSCode 连接远程服务器，使用 http.server 开启一个端口后，会自动映射到本地，这样在本地浏览器就能查看和下载远程服务器资源。（除 VSCode 外，其他工具应该也可以实现远程与本地的端口映射）

## 注意

http.server 只实现了最基本的安全审查，请不要用于生产环境。

## 参考

[http.server 官方文档](https://docs.python.org/3/library/http.server.html)

https://docs.python.org/3/library/http.server.html

https://docs.python.org/zh-cn/3.12/library/http.server.html