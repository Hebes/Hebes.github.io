# WPS修改注册表跳过登录

## 参考网站

<https://www.zhihu.com/question/525681203>

## 方法

    导入以下注册表，禁用 WPS Office 强制登录功能：

``` C#
Windows Registry Editor Version 5.00

[HKEY_CURRENT_USER\Software\kingsoft\Office\6.0\plugins\officespace\flogin]
"enableForceLoginForFirstInstallDevice"="false"
```


新建一个文本文件，将上面的代码复制到里面，保存后将文本文件的后缀名从 txt 改为 reg，然后双击导入。怕麻烦的话，可以直接下载下面的文件，解压缩后双击文件导入即可。
