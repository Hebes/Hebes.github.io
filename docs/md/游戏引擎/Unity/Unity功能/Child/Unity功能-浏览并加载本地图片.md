# Unity功能-浏览并加载本地图片

**[Unity实用功能之浏览并加载本地图片](<https://juejin.cn/post/6992212153897844766>)**

## 概述

现在很多软件，网站都有更换头像，上传图片等功能，本篇文章主要讲解一下在Unity中，如何浏览本地图片文件，并将其加载到untiy中并显示出来。大体上分为两个步骤：浏览本地文件、加载本地文件（其他格式文件原理相同）

## 思路分析

整体的思路是：

1.  调用系统的文件窗口（打开资源管理器，进行图片资源的浏览）
2.  通过资源管理器获取到的图片路径，将图片加载出 加载本地图片的方式：

+   使用WWW进行图片加载（此方法适用旧版Unity）
+   使用UnityWebRequest进行图片加载（此方法使用于高版本Unity）
+   以IO方式进行加载图片

## 功能实现

### 调用系统文件窗口

引用Comdlg32.dll系统类库，调用GetOpenFileName方法实现窗口打开功能。具体实现代码如下：

```arduino
public class WindowDll
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOpenFileName1([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }
}
```

光有这一段代码还不足够，还需要声明原型，设置参数，创建OpenFileName类，并为每个结构元素声明一个类成员。具体可参照[微软官方的.NET 框架高级开发文档](https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/w5tyztk9%28v=vs.100%29?redirectedfrom=MSDN#declaring-prototypes "https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/w5tyztk9%28v=vs.100%29?redirectedfrom=MSDN#declaring-prototypes")，本文中使用到的参数如下：

```ini
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}
```

到此，调用系统文件窗口文件就实现了。

### 打开窗口获取文件路径

上面打开窗口方法已经写好了，接下来要编写一个按钮点击事件，通过按钮，调用上述方法，打开要加载的图片并获取图片位置：

```ini
OpenFileName ofn = new OpenFileName();
ofn.structSize = Marshal.SizeOf(ofn);
ofn.filter = "图片文件(*.jpg*.png)\0*.jpg;*.png";
ofn.file = new string(new char[256]);
ofn.maxFile = ofn.file.Length;
ofn.fileTitle = new string(new char[64]);
ofn.maxFileTitle = ofn.fileTitle.Length;
//默认路径
string path = Application.streamingAssetsPath;
path = path.Replace('/', '\\');
ofn.initialDir = path;
ofn.title = "Open Project";
ofn.defExt = "JPG";
//注意 一下项目不一定要全选 但是0x00000008项不要缺少
ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
//上一行代码对应如下
//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
//点击Windows窗口时开始加载选中的图片
if (WindowDll.GetOpenFileName(ofn))
{
    Debug.Log("Selected file with full path: " + ofn.file);
}
```

上述代码中，有几个主要参数：

+   ofn.initialDir:是打开的窗口的默认路径
+   ofn.defExt:打开窗口中要显示的文件的类型
+   ofn.file：这个是主要的，这个是选中的图片返回的路径参数，包括路径+文件名全部在一起。 其他的参数很好理解，只要设置成对应的参数即可。

### 加载图片方法一---使用WWW加载

此方法使用于低版本Unity使用，高版本Unity虽然可以使用，但是会报过时的警告。www加载方式主要是将图片加载为Texture2D。而我们要将图片显示在Image上，Image使用的是Sprite,所以我们需要将图片转换为sprite在赋值给图片。以下方法是将图片加载为Texture2D。

```csharp
IEnumerator Load(string path)
{
    WWW www = new WWW("file:///" + path);
    yield return www;
    if (www != null && string.IsNullOrEmpty(www.error))
    {
        //获取Texture
        Texture2D texture = www.texture;
    }
}
```

在获取到texture后，根据获取的Texture创建一个sprite

```arduino
Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
```

接着就可以将图片赋值到Image身上，有事图片会比Image大或小，这是可能有的人需要使用原始图片大小，这是就需要将图片尺寸设置为原始尺寸

`image.SetNativeSize();`

### 加载图片方法二---使用UnityWebRequest加载

方法一加载本地图片文件是通过www加载，但是高版本Unity已经不支持这个API了，采用了更高级的API。根据官方文档介绍，采用UnityWebRequest代替。命名空间同样是NetWorking，总体没有太大变化，和WWW加载几乎相同，只是代码写法略有差别，具体方法如下代码

```scss
IEnumerator Load(string path)
{
    UnityWebRequest request = UnityWebRequestTexture.GetTexture(path);
    yield return request.SendWebRequest();
    if (request.isHttpError || request.isNetworkError)
    {
        Debug.Log(request.error);
    }
    else
    {
        Texture tt = DownloadHandlerTexture.GetContent(request);
        //根据获取的Texture创建一个sprite
        Sprite sprite = Sprite.Create((Texture2D)tt, new Rect(0, 0, tt.width, tt.height), new Vector2(0.5f, 0.5f));
        //将sprite显示在图片上
        image.sprite = sprite;
        //图片设置为原始尺寸
        image.SetNativeSize();
    }  
}  
```

### 加载图片方法三---使用IO方式读取文件

流：  
流的定义：流是在内存中开辟内存地址，然后将文件中的数据存入流，在内存中（流中）对文本内容进行更改，再保存回文件。  
流的关键字：FileStream，应用到FileStream就是使用流  
首先需要引用命名空间using System.IO;using System.IO.Compression; 具体代码为：

```arduino
    private void LoadByIo(string url)
    {
        //创建文件读取流
        FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);

        //释放文件读取流
        fileStream.Close();
        //释放本机屏幕资源
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        int width = 300;
        int height = 372;
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //将sprite显示在图片上
        image.sprite = sprite;
        //图片设置为原始尺寸
        image.SetNativeSize();
    }
```

注意：通过Read获取文件的内容赋值给syte数组的时候同时会保存到流内，再通过write写入相同的内容到流中就会有双倍的原文件内容，所以通过Read读取流内数据后只需要write新的数据进入流中就能将文件自身的数据和新的数据同时从流写入到文件中。

## 效果展示

三种加载方法效果是相同的

![动画.gif](https://p6-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/bc24e1f972b84a2d94533c076c39fa2a~tplv-k3u1fbpfcp-zoom-in-crop-mark:3024:0:0:0.image)

## 写在最后

所有分享的内容均为作者在日常开发过程中使用过的各种小功能点，分享出来也变相的回顾一下，如有写的不好的地方还请多多指教。Demo源码会在之后整理好之后分享给大家。