# Linux

## VM虚拟机拓展磁盘

**不重要的参考：**

[VMware虚拟机解决空间不足，增加磁盘空间(磁盘扩容)](<https://blog.csdn.net/qq_44297579/article/details/107318096>)

[使用VMware创建的虚拟机磁盘空间不够了，如何扩充磁盘并生效](<https://blog.csdn.net/qq_42448606/article/details/111647757>)

[Vmware Linux磁盘空间扩容（超简单）](<https://blog.csdn.net/Flying_Ape/article/details/118548972>)

**重要参考:**

[VMware虚拟机扩展内存和磁盘](<https://www.cnblogs.com/beyondhd/p/15222482.html>)

**内容：**

关闭虚拟机，设置扩展磁盘容量
![1Linux](<../Image/1Linux.png>)

注： 虚拟机关机后修改硬盘容量才有效，另外如果有快照的话也要先删除快照才能设置扩展磁盘容量
![2Linux](<../Image/2Linux.png>)

扩展磁盘空间：打开VMware，关闭虚拟机，菜单 虚拟机-->设置，选择硬盘，点击扩展，填写扩展磁盘大小，确认。（这里我们从40GB扩展到60GB）
![4Linux](<../Image/4Linux.png>)

启动虚拟机，df -h查看磁盘空间，可以看到目前还是40G没有扩展
![3Linux](<../Image/3Linux.png>)

fdisk -l 查看新磁盘的分区信息
![5Linux](<../Image/5Linux.png>)

fdisk /dev/sda 对新加的磁盘进行分区操作，输入“p”回车，查看已分区的数量：可知目前有两个分区sda1和sda2
![6Linux](<../Image/6Linux.png>)

输入n回车，新增一个分区

输入p回车，在打开的分区号命中中使用默认的分区号并回车（此处是3）

在显示的起始扇区first sector直接回车，在last sector直接回车

输入p回车，查看当前分区的信息：可以看到多了一个/dev/sda3的新分区

输入w回车，写入磁盘信息并保存
![7Linux](<../Image/7Linux.png>)

输入reboot回车，重启虚拟机格式化新建分区。

虚拟机重启后，输入vgdisplay，查看磁盘卷组名，如图为centos
![8Linux](<../Image/8Linux.png>)

输入pvcreate /dev/sda3回车，初始化刚建立的分区

输入vgextend centos /dev/sda3回车，把刚初始化的分区加入到虚拟卷组名中（命令：vgextend 虚拟卷组名 新增的分区 ）

输入vgdisplay回车，查看卷组的详细信息，可以看到刚增加的20G空间处于空闲状态
![9Linux](<../Image/9Linux.png>)

输入df -h回车，查看需要扩展的文件系统名，此处是/dev/mapper/centos-root
![10Linux](<../Image/10Linux.png>)

输入lvextend -L +19G /dev/mapper/centos-root回车，扩充已有的卷组容量（注意：如果扩容的是20G，这里的20G就不能全部扩展，只能扩展比20G小的容量，不然系统会报错导致扩容失败。命令：lvextend -L +需要扩展的容量 需要扩展的文件系统名，需要注意命令中区分字母的大小写） 
![11Linux](<../Image/11Linux.png>)

输入pvdisplay回车，查看当前的卷组，在显示信息中可见卷组已经扩容成功。接下来还需要将文件系统也扩容。
![12Linux](<../Image/12Linux.png>)

 输入cat /etc/fstab |grep centos-root回车，查看文件系统的格式，此处是xfs
![13Linux](<../Image/13Linux.png>)

 输入xfs_growfs /dev/mapper/centos-root回车（命令：xfs_growfs 文件系统名，注意不同的文件系统要用不同的命令，否则会报错）
![14Linux](<../Image/14Linux.png>)

等待系统自动扩容完成后，输入df -h回车，查看磁盘大小是否扩容成功，在显示的磁盘信息中，可看到磁盘扩容成功了。
![15Linux](<../Image/15Linux.png>)
