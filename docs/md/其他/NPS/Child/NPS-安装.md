# NPS-安装

内网穿透搭建 **[nps](<https://github.com/ehang-io/nps/releases>)**

查看linux架构

我的是x86_64

下载服务器对应的包

![1](\../Image/1.png)

1. 切换到root权限，命令是sudo -i

2. 下载

    ```Linux
    #下载安装包
    wget https://github.com/ehang-io/nps/releases/download/v0.26.10/linux_amd64_server.tar.gz
    #解压
    tar -zxvf linux_amd64_server.tar.gz
    ```

3. 安装nps

    ```C#
    #执行安装命令
    ./nps install
    #启动
    nps start
    ```

4. 配置服务器信息（IP地址，账号密码等）

    ```C#
    //如果要修改配置，切记去 /etc/nps下进行修改，在解压包文件夹下修改不生效
    vi /etc/nps/conf/nps.conf
    ```

    找到#web 修改密码web_passwed = 123  这里的123可以改成自己的密码

    编辑完成后，按esc，然后输入:wq就可以退出编辑了

    web_port = 8080 服务器防火墙开启8080端口

5. 重启下nps服

    ```C#
    nps restart
    ```

## 来源网站

**[nps内网端口映射，含（p2p配置方法）](<https://zhuanlan.zhihu.com/p/492549992>)**

**[Linux平台搭建NPS内网穿透平台搭建](<https://blog.csdn.net/leaf541568990/article/details/124401349>)**
