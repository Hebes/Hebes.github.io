# Linux-动态扩容Linux根目录

## 操作步骤

1. 首先，必须确保其他分区有足够的空间用来分给根目录/。可以使用以下命令查看:

   ```linux
    sudo df -h
   ```

    ![1Linux-动态扩容Linux根目录](\../Image/Linux-动态扩容Linux根目录/1Linux-动态扩容Linux根目录.png)

    可以看到，这里home目录空闲的空间还很大，因此，我们将home的空间分给根目录一些。

2. 扩容根目录的思路如下:

    将/home文件夹备份，删除/home文件系统所在的逻辑卷，增大/文件系统所在的逻辑卷，增大/文件系统大小，最后新建/home目录，并恢复/home文件夹下的内容。

3. 备份/home分区内容

    这里需要选一个能够容纳下/home文件夹大小的分区，可以看到/run剩余空间为32G，因此，我们将/home备份到/run下面。

    ```linux
    sudo tar cvf /run/home.tar /home
    ```

4. 卸载/home

    要先终止所有使用/home文件系统的进程，这里要注意不要在/home目录下执行下面的操作：

    ```linux
    sudo fuser -km /home
    ```

    然后，卸载：

    ```liunx
    sudo umount /home
    ```

5. 删除/home所在的逻辑卷lv：

    ```linux
    sudo lvremove /dev/mapper/centos-home
    ```

    选择y。

    当执行这一步的时候，有可能会一直提示的是Logical volume centos/home contains a filesystem in use.，网上搜的解决办法太麻烦，尝试重复执行了命令导致服务器被重启后，再次执行上面的操作就出现了输入y的提示

6. 扩大根目录所在的逻辑卷，这里增大1T

    ```linux
    sudo lvextend -L +1T /dev/mapper/centos-root
    ```

7. 扩大/文件系统：

    ```linux
    sudo xfs_growfs /dev/mapper/centos-root
    ```

8. 重建/home文件系统所需要的逻辑卷：

    由于刚才分出去1.0T，因此这里创建的逻辑卷大小为2.5T.

    ```linux
    sudo lvcreate -L 2.5T -n/dev/mapper/centos-home
    ```

9. 创建文件系统：

    ```linux
    sudo mkfs.xfs  /dev/mapper/centos-home
    ```

10. 将新建的文件系统挂载到/home目录下：

    ```linux
    sudo mount /dev/mapper/centos-home
    ```

11. 恢复/home目录的内容：

    ```linux
    sudo tar xvf /run/home.tar -C /
    ```

12. 删除/run下面的备份：

    ```linux
    sudo rm -rf /run/home.tar
    ```

13. 再次查看磁盘存储df -h

## 参考链接

**[动态扩容Linux根目录](<https://www.cnblogs.com/mihoutao/p/13826374.html>)**

**[动态扩容Linux根目录](<https://blog.csdn.net/u013431916/article/details/80548069>)**
