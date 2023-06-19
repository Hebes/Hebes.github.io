# Gitlab

宝塔面板搭建GitLab小坑
https://www.bt.cn/bbs/thread-92200-1-1.html

https://www.cnblogs.com/mihoutao/p/13826374.html

## gitlab修改项目克隆地址

修改项目的clone地址

```text
vi /opt/gitlab/embedded/service/gitlab-rails/config/gitlab.yml
```

修改该文件

```text
production: &base
   #
   # 1. GitLab app settings
   # ==========================
 
   ## GitLab settings
   gitlab:
    ## Web server settings (note: host is the FQDN, do not include http://)¬
     host: xxxxxxx.cn // 原域名,或者IP
     port: 8099
     https: false
```

将host改为绑定的域名或IP

之后重启

可能会出现nginx不能启动,执行：

```text
cp /opt/gitlab/embedded/sbin/gitlab-web /opt/gitlab/embedded/sbin/nginx
```

## 宝塔搭建的Gitlab空间太小解决办法

文档里面点击的:[Linux-动态扩容Linux根目录](../../Linux/Child/Linux-动态扩容Linux根目录.md#操作步骤)

网站点击的:[Linux-动态扩容Linux根目录](md\Linux\Child\Linux-动态扩容Linux根目录.md#操作步骤)

## 参考链接

**[gitlab如何修改项目clone克隆地址](<https://www.yuanchengzhushou.cn/article/8229.html>)**

**[GitLab中文社区版 8.8.5修改代码等存储位置](<https://www.bt.cn/bbs/thread-92200-1-1.html>)**
