# UnityWebRequest

# 一、前言

1\. UnityWebRequest

> 官方描述：
>
> UnityWebRequest 提供了一个模块化系统，用于构成 HTTP 请求和处理 HTTP 响应。UnityWebRequest 系统的主要目标是让 Unity 游戏与 Web 浏览器后端进行交互。该系统还支持高需求功能，例如分块 HTTP 请求、流式 POST/PUT 操作以及对 HTTP 标头和动词的完全控制。
>
> 从官方描述来看，对于UnityWebRequest类的升级更大程度代表了Unity对于Webgl网页浏览器支持的优化。

2.WWW类

> 其实5.4版本的时候就出了新的API UnityWebRequest用于替代WWW，有些较大的文件下载需要断点续传的功能（即下载了一部分突然中断下载后，再次下载直接从上次下载的地方继续下载，而不是重新下载）就需要使用HttpWebRequest或UnityWebRequest，在2017版本中WWW是还没有被弃用的，2018版本我没有试过，2019是已经被弃用的，使用的时候可以看到这个类被画上了绿色波浪。

这里大体的描述一下，后面还可能会持续深入解剖。下面讲述一下这个类的一些常用接口和使用

# 二、常用方法Get、Post、Put和Head

**UnityWebRequest**

> 架构：
>
> UnityWebRequest 生态系统将 HTTP 事务分解为三个不同的操作：
>
> + 向服务器提供数据
> + 从服务器接收数据
> + HTTP 流量控制（例如，重定向和错误处理）
>
> UnityWebRequest由三个元素组成：  
> 1 UpLoadHandler处理数据将数据上传到服务器的对象；  
> 2 DownLoadHandler从服务器下载数据的对象；  
> 3 UnityWebRequest负责与HTTP通信并管理上面两个对象。还处理 HTTP 流量控制。此对象是定义自定义标头和 URL 的位置，也是存储错误和重定向信息的位置。

更多描述请看官网手册：[https://docs.unity.cn/cn/2019.4/Manual/UnityWebRequest.html](https://docs.unity.cn/cn/2019.4/Manual/UnityWebRequest.html "https://docs.unity.cn/cn/2019.4/Manual/UnityWebRequest.html")

**（1）常用方法：**

<table border="1" cellpadding="1" cellspacing="1" style="width:500px;"><tbody><tr><td><strong>方法</strong></td><td><strong>作用</strong></td></tr><tr><td>SendWebRequest()</td><td>开始与远程服务器通信。在调用此方法之后，有必要的话UnityWebRequest将执行DNS解析，将HTTP请求发送到目标URL的远程服务器并处理服务器的响应。</td></tr><tr><td>Get(url)</td><td>创建一个HTTP为传入URL的UnityWebRequest对象</td></tr><tr><td>Post（url)</td><td>向Web服务器发送表单信息</td></tr><tr><td>Put(url)</td><td>将数据上传到Web服务器</td></tr><tr><td>Abort()</td><td>直接结束联网</td></tr><tr><td>Head()</td><td>创建一个为传输HTTP头请求的UnityWebRequest对象</td></tr><tr><td>GetResponseHeader()</td><td>返回一个字典，内容为在最新的HTTP响应中收到的所有响应头</td></tr></tbody></table>

**（2）构造函数**

```cs
public UnityWebRequest（）; 
public UnityWebRequest（Uri uri）;
public UnityWebRequest（Uri uri，string method）;
public UnityWebRequest（Uri uri，string method,Networking.DownloadHandler downloadHandler， 
      Networking.UploadHandler uploadHandler）;
```

<table border="1" cellpadding="1" cellspacing="1" style="width:500px;"><tbody><tr><td><strong>参数</strong></td><td><strong>含义</strong></td></tr><tr><td>URL</td><td>url网址信息或本地文件路径信息</td></tr><tr><td>method</td><td>相当于方法名，只有<code>GET, POST, PUT, HEAD</code>四种，默认为<code>GET</code>，一旦调用<code>SendWebRequest()</code>，就无法更改</td></tr><tr><td>downloadHandler</td><td>下载数据的委托方法</td></tr><tr><td>uploadHandler</td><td>上传数据的委托方法</td></tr></tbody></table>

下面介绍一下一些经常用的接口。介绍之前先简单聊聊GET、POST和PUT这三种方式的区别

> 1.Get：一般用于向服务器获取信息，举例：后台服务器有一个接口<http://127.0.0.1/Get/?studentName=张三，这个接口负责返回学生名字为张三的数据，我们在提交get的时候，服务器会接收studentName下的值，通过这个值来进行逻辑处理，使用Get我们要访问得值是暴露在浏览器中的，如果是用户名密码这样的重要信息被暴露后果将不堪设想，所以像网页中搜索栏需要条件来获取信息的功能，就可以使用Get的方法来实现。>
>
> 2.Post：这种方式就是为了解决Get访问时信息暴露的危险，使用Post访问时表单中的内容不会暴露，安全性更高，一般用于网页用户登录等重要信息上
>
> 3.Put：这种方式用于将数据发送到远程的服务器。比如文件上传。

### 1.Get方法

(1) 使用静态类创建UnityWebRequest获取Txt文本信息

```cs
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System;

public class AAA : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("GetDataB", "Test.txt");
    }

    #region 创建persistentDataPath文件夹
    IEnumerator GetDataB(string fileName)
    {
        //1.url地址
        string fromPath = Application.streamingAssetsPath + "/" + fileName;
        //2.创建一个UnityWebRequest类 method属性为Get
        UnityWebRequest request = UnityWebRequest.Get(fromPath);
        //3.等待响应时间，超过5秒结束
        request.timeout = 5;
        //4.发送请求信息
        yield return request.SendWebRequest(); 

        //5.判断是否下载完成
        if (request.isDone)
        {
            //6.判断是否下载错误
            if (request.isHttpError || request.isNetworkError)
                Debug.Log(request.error);
            else
                Debug.Log(request.downloadHandler.text);
        }
    }

    #endregion
}
```

(2) 使用构造来创建

> 注意：这里为什么要用两种方式呢？因为静态类创建(UnityWebRequest.Get)的Request是自带DownloadHandler和UploadHandler的，而构造创建(new UnityWebRequest )是没有的，需要自己手动创建赋值，注意不要踩坑哦！！

```cs
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System;

public class AAA : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("GetDataB", "Test.txt");
    }

    #region 创建persistentDataPath文件夹
    IEnumerator GetDataB(string fileName)
    {

        string fromPath = Application.streamingAssetsPath + "/" + fileName;
        Uri uri = new Uri(fromPath);
        //UnityWebRequest request = UnityWebRequest.Get(fromPath);
        UnityWebRequest request =new UnityWebRequest(uri); //使用构造
        request.timeout = 5;//等待响应时间，超过5秒结束

        /*使用构造没有DownloadHandler和UploadHandler，所以要创建赋值，这里只用到了下载，所以可以不用创建UploadHandler
         * 下面罗列了三种不同类型的DownloadHandler
         1.DownloadHandlerBuffer 读取文件存储
         2.DownloadHandlerTexture 读取图片
         3.DownloadHandlerFile 下载文件到本地
         */
        DownloadHandlerBuffer Download = new DownloadHandlerBuffer();
        request.downloadHandler = Download;

        yield return request.SendWebRequest();

        if (request.isDone)
        {
            if (request.isHttpError || request.isNetworkError)
                Debug.Log(request.error);
            else 
                Debug.Log(request.downloadHandler.text);
        }
        
    }

    #endregion

}
```

### 2.Post方法

> Post方法将一个表上传到远程的服务器，一般来说我们登陆某个网站的时候会用到这个方法，我们的账号密码会以一个表单的形式传过去。

```cs
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 网络请求测试
/// </summary>
public class ChinarWebRequest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Post());
    }
    /// <summary>
    /// 开启一个协程，发送请求
    /// </summary>
    /// <returns></returns>
    IEnumerator Post()
    {
        WWWForm form = new WWWForm();
        //键值对
        form.AddField("key",  "value");
        form.AddField("name", "Chinar");
        //请求链接，并将form对象发送到远程服务器
        UnityWebRequest webRequest = UnityWebRequest.Post("http://www.baidu.com", form);

        yield return webRequest.SendWebRequest();
        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("发送成功"); 
        }
    }
}
```

### 3.Put方法

> Put方法将数据发送到远程的服务器。例如：文件上传

```cs
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 网络请求测试
/// </summary>
public class ChinarWebRequest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Upload());
    }
    
    /// <summary>
    /// 开启协程
    /// </summary>
    /// <returns></returns>
    IEnumerator Upload()
    {
        byte[] myData = System.Text.Encoding.UTF8.GetBytes("Chinar的测试数据");
        using (UnityWebRequest uwr = UnityWebRequest.Put("http://www.baidu.com", myData))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Debug.Log("上传成功!");
            }
        }
    }
}
```

### 4.Abort方法

> Abort方法会尽快结束联网，可以随时调用此方法。  
> 如果 UnityWebRequest尚未完成，那么 UnityWebRequest将尽快停止上传或下载数据。
>
> 中止的 UnityWebRequests被认为遇到了系统错误。isNetworkError或isHttpError属性将返回true，error属性将为“User Aborted”。

### 5.Head方法

> Head方法与Get方法用法一致，都是传入一个URL。
>
> 关于head这个方法解释推荐文章：[https://www.jianshu.com/p/49ebc4a78474](https://www.jianshu.com/p/49ebc4a78474 "https://www.jianshu.com/p/49ebc4a78474")

![](https://img-blog.csdnimg.cn/bde39bb3924a4e80afcd47ec509212ad.png?x-oss-process=image/watermark,type_d3F5LXplbmhlaQ,shadow_50,text_Q1NETiBA6KKr5Luj56CB5oqY56Oo55qE54uX5a2Q,size_19,color_FFFFFF,t_70,g_se,x_16)

根据这个文章特点的截图 我简单聊聊Head作用 :

1.第一条就是只请求资源头部，网页的body主体是不显示的。大家可以先用get请求一个www.baidu.com查看

![](https://img-blog.csdnimg.cn/71ef2ce228914a9e9e7694f499d9117b.png?x-oss-process=image/watermark,type_d3F5LXplbmhlaQ,shadow_50,text_Q1NETiBA6KKr5Luj56CB5oqY56Oo55qE54uX5a2Q,size_20,color_FFFFFF,t_70,g_se,x_16)

接下来我们使用Head来请求

![](https://img-blog.csdnimg.cn/b4f6d15e484145bfbcd01409c4d4b6e8.png?x-oss-process=image/watermark,type_d3F5LXplbmhlaQ,shadow_50,text_Q1NETiBA6KKr5Luj56CB5oqY56Oo55qE54uX5a2Q,size_20,color_FFFFFF,t_70,g_se,x_16) 2.检查超链接有效性，当链接出现问题时会返回一个错误码，上方链接文章有对应错误码的中文描述。使用get、post也可以测试有效性，但是这些方式访问成功会返回body主体，所以使用head访问可以节省网络资源

3.网页是否被修改，举个例子，我们将一个静态网页使用MD5加密后存入数据库，在head中加入这个html被加密后的数据进行数据库访问判断，如果这个静态网页被修改访问就会出现问题。不过这是很久之前的版本用的方法，现在基本使用的时token进行验证

4.第四点就是头包含的这些信息了，例如我们需要获取安全验证信息来进心判断操作，如果使用GET、PUST等方式访问，就会连带body主体一起获取，大大浪费资源。

下面是UnityWebRequest.Head的请求方式：一般与下面的GetResponseHeader方法配合使用获取文件大小的，后面断电续传中会有用到。

```cs
    /// <summary>
    /// 开启一个协程，发送请求
    /// </summary>
    /// <returns></returns>
    IEnumerator SendRequest()
    {
        UnityWebRequest uwr = UnityWebRequest.Head("www.baidu.com");       //创建UnityWebRequest对象
        yield return uwr.SendWebRequest();                                 //等待返回请求的信息
        if (uwr.isHttpError || uwr.isNetworkError)                         //如果请求失败，或是 网络错误
        {
            Debug.Log(uwr.error); //打印错误原因
        }
        else //请求成功
        {
            Debug.Log("Head:请求成功");
        }
    }
```

### 6.GetResponseHeader方法

GetResponseHeader方法可以用来获取请求文件的长度 传入参数 "Content-Length"字符串，表示获取文件内容长度。

```cs
 IEnumerator SendRequest()
    {
        UnityWebRequest uwr = UnityWebRequest.Head("www.baidu.com"); //创建UnityWebRequest对象
        yield return uwr.SendWebRequest();                                 //等待返回请求的信息
        if (uwr.isHttpError || uwr.isNetworkError)                         //如果其 请求失败，或是 网络错误
        {
            Debug.Log(uwr.error); //打印错误原因
        }
        else //请求成功
        {
            long totalLength = long.Parse(uwr.GetResponseHeader("Content-Length")); //首先拿到文件的全部长度
            Debug.Log($"totalLength:{totalLength}" );//打印文件长度
        }
    }
```

# 三、常用属性

<table border="1" cellpadding="1" cellspacing="1" style="width:600px;"><tbody><tr><td><strong>属性</strong></td><td><strong>类型</strong></td><td><strong>含义</strong></td></tr><tr><td>timeout</td><td>int</td><td>等待时间(秒)超过此数值是 <code>UnityWebReqest</code>的尝试连接将终止</td></tr><tr><td>isHttpError</td><td>bool</td><td><code>HTTP</code>响应出现出现错误</td></tr><tr><td>isNetworkError</td><td>bool</td><td>系统出现错误</td></tr><tr><td>error</td><td>string</td><td>描述 <code>UnityWebRequest</code>对象在处理<code>HTTP</code>请求或响应时遇到的任何系统错误</td></tr><tr><td>downloadProgress</td><td>float</td><td>表示从服务器下载数据的进度</td></tr><tr><td>uploadProgress</td><td>float</td><td>表示从服务器上传数据的进度</td></tr><tr><td>isDone</td><td>bool</td><td>是否完成与远程服务器的通信</td></tr><tr><td>SendWebRequest</td><td>UnityWebRequestAsyncOperation</td><td>发送信息访问</td></tr></tbody></table>

# 四、案例

### 一、断点续传

> 记录已经下载到的本地文件大小，向资源服务器发送请求时，通过请求头实现拿到剩下需要下载的内容，然后接着下载  
> 确保对同一个资源文件的下载操作，就不存在资源会下载错误的情况，如果你在断点续传的阶段发现资源服务器上的资源已经更新，那就得删除之前下载的文件然后重新下载。

```cs
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChinarBreakpointRenewal : MonoBehaviour
{
    private bool _isStop;           //是否暂停

    public Slider ProgressBar;      //进度条
    public Text SliderValue;        //滑动条值
    public Button startBtn;        //开始按钮
    public Button pauseBtn;        //暂停按钮
    string Url = "https://downsc.chinaz.net/Files/DownLoad/sound1/201808/10447.wav";

    /// <summary>
    /// 初始化UI界面及给按钮绑定方法
    /// </summary>
    void Start()
    {
        //初始化进度条和文本框
        ProgressBar.value = 0;
        SliderValue.text = "0.0%";

        //开始、暂停按钮事件监听
        startBtn.onClick.AddListener(OnClickStartDownload);
        pauseBtn.onClick.AddListener(OnClickStop);
    }


    //开始下载按钮监听事件
    public void OnClickStartDownload()
    {
        //开启协程 *注意真机上要用Application.persistentDataPath路径*
        StartCoroutine(DownloadFile(Url, Application.streamingAssetsPath + "/MP4/test.mp4", CallBack));
    }


    /// <summary>
    /// 协程：下载文件
    /// </summary>
    /// <param name="url">请求的Web地址</param>
    /// <param name="filePath">文件保存路径</param>
    /// <param name="callBack">下载完成的回调函数</param>
    /// <returns></returns>
    IEnumerator DownloadFile(string url, string filePath, Action callBack)
    {
        UnityWebRequest huwr = UnityWebRequest.Head(url); //使用Head方法可以获取到文件的全部长度
        yield return huwr.SendWebRequest();//发送信息请求
        //判断请求或系统是否出错
        if (huwr.isNetworkError || huwr.isHttpError) 
        {
            Debug.Log(huwr.error); //出现错误 输出错误信息
        }
        else
        {
            long totalLength = long.Parse(huwr.GetResponseHeader("Content-Length")); //首先拿到文件的全部长度
            string dirPath = Path.GetDirectoryName(filePath);//获取文件的上一级目录
            if (!Directory.Exists(dirPath)) //判断路径是否存在
            {
                Directory.CreateDirectory(dirPath);//不存在创建
            }

            /*作用：创建一个文件流，指定路径为filePath,模式为打开或创建，访问为写入
             * 使用using(){}方法原因： 当同一个cs引用了不同的命名空间，但这些命名控件都包括了一个相同名字的类型的时候,可以使用using关键字来创建别名，这样会使代码更简洁。注意：并不是说两个名字重复，给其中一个用了别名，另外一个就不需要用别名了，如果两个都要使用，则两个都需要用using来定义别名的
             * using(类){} 括号中的类必须是继承了IDisposable接口才能使用否则报错
             * 这里没有出现不同命名空间出现相同名字的类属性可以不用using(){}
             */
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                long nowFileLength = fs.Length; //当前文件长度,断点前已经下载的文件长度。
                Debug.Log(fs.Length);
                //判断当前文件是否小于要下载文件的长度，即文件是否下载完成
                if (nowFileLength < totalLength)
                {
                    Debug.Log("还没下载完成");

                    /*使用Seek方法 可以随机读写文件
                     * Seek()  ----------有两个参数 第一参数规定文件指针以字节为单位移动的距离。第二个参数规定开始计算的位置
                     * 第二个参数SeekOrigin 有三个值：Begin  Current   End
                     * fs.Seek(8,SeekOrigin.Begin);表示 将文件指针从开头位置移动到文件的第8个字节
                     * fs.Seek(8,SeekOrigin.Current);表示 将文件指针从当前位置移动到文件的第8个字节
                     * fs.Seek(8,SeekOrigin.End);表示 将文件指针从最后位置移动到文件的第8个字节
                     */
                    fs.Seek(nowFileLength, SeekOrigin.Begin);  //从开头位置，移动到当前已下载的子节位置

                    UnityWebRequest uwr = UnityWebRequest.Get(url); //创建UnityWebRequest对象，将Url传入
                    uwr.SetRequestHeader("Range", "bytes=" + nowFileLength + "-" + totalLength);//修改请求头从n-m之间
                    uwr.SendWebRequest();                      //开始请求
                    if (uwr.isNetworkError || uwr.isHttpError) //如果出错
                    {
                        Debug.Log(uwr.error); //输出 错误信息
                    }
                    else
                    {
                        long index = 0;     //从该索引处继续下载
                        while (nowFileLength < totalLength) //只要下载没有完成，一直执行此循环
                        {
                            if (_isStop) break;//如果停止跳出循环
                            yield return null;
                            byte[] data = uwr.downloadHandler.data;
                            if (data != null)
                            {
                                long length = data.Length - index;
                                fs.Write(data, (int)index, (int)length); //写入文件
                                index += length;
                                nowFileLength += length;
                                ProgressBar.value = (float)nowFileLength / totalLength;
                                SliderValue.text = Math.Floor((float)nowFileLength / totalLength * 100) + "%";
                                if (nowFileLength >= totalLength) //如果下载完成了
                                {
                                    ProgressBar.value = 1; //改变Slider的值
                                    SliderValue.text = 100 + "%";
                                    /*这句话的作用是：如果callBack方法不为空则执行Invoke
                                     * 注意：
                                     * 1.这里的Invoke可不是Unity的Invoke延迟调用的用法，参考文章：https://blog.csdn.net/liujiejieliu1234/article/details/45312141 从文章中我们可以看到，C#中的Invoke是为了防止winform中子主线程刚开始创建对象时，子线程与主线程并发修改主线程尚未创建的对象属性。
                                     * 因为unity这里只有主线程没有用到子线程可以直接写callBack();
                                     */
                                    callBack?.Invoke();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 下载完成后的回调函数
    /// </summary>
    void CallBack()
    {
        Debug.Log("下载完成");
    }

    /// <summary>
    /// 暂停下载
    /// </summary>
    public void OnClickStop()
    {
        if (_isStop)
        {
            pauseBtn.GetComponentInChildren<Text>().text = "暂停下载";
            Debug.Log("继续下载");
            _isStop = !_isStop;
            OnClickStartDownload();
        }
        else
        {
            pauseBtn.GetComponentInChildren<Text>().text = "继续下载";
            Debug.Log("暂停下载");
            _isStop = !_isStop;
        }
    }
}
```
