# Linux-架构

以下命令几乎在所有 Linux 发行版都可用

1. uname 命令
    uname -m 直接显示Linux 系统架构
    uname -a 命令也可以显示Linux 系统架构，但是还有和其他信息
2. dpkg 命令
    dpkg的命令可用于查看 Debian/ Ubuntu 操作系统是 32 位还是 64 位
    dpkg --print-architecture
    如果当前 Linux 是 64 位则输出 amd64，是 32 位则会输出 i386。
3. getconf 命令
    getconf命令主要用于显示系统变量配置
    getconf LONG_BIT
4. arch 命令
    arch命令主要用于显示操作系统架构类型。如果输出x86_64 则表示为 64 位系统，
    如果输出 i686 或 i386 则表示为 32 位系统。
5. file 命令
    file命令可以配合 /sbin/init或者/bin/bash来查看系统架构类型，与系统架构相同：
    file /sbin/init
