# Unity功能-读写Json文件

**[Unity实用功能之读写Json文件](<https://juejin.cn/post/6992593189685166110>)**

**[【Unity3D读取数据】—读取Json文件](<https://juejin.cn/post/7094531251800899598>)**

## 概述

Json是一种轻量级的文本数据格式，在项目中使用非常广泛。在Unity开发过程中通常用于通讯时的数据交互，以及配置文件的读写等操作。本篇文章主要介绍一下什么是Json以及其在unity中的简单应用。

## Json介绍

JSON是JavaScript Object Notation的简称，它是一种数据交换的文本格式。它基于 ECMAScript (w3c制定的js规范)的一个子集，采用完全独立于编程语言的文本格式来存储和表示数据。简洁和清晰的层次结构使得 JSON 成为理想的数据交换语言。易于人阅读和编写，同时也易于机器解析和生成，并有效地提升网络传输效率。

## Json的特点

Json 主要具有以下特性，这些特性使它成为理想的数据交换语言：

+   Json 是轻量级的文本数据交换格式
+   Json 具有自我描述性，更易理解
+   Json 采用完全独立于语言的文本格式：Json 使用 JavaScript 语法来描述数据对象，但是 Json 仍然独立于语言和平台。Json 解析器和 Json 库支持许多不同的编程语言。 目前常见的动态编程语言（PHP，JSP，.NET）都支持Json。 Json 是存储和交换文本信息的一种语法，它与XML具有相同的特性，是一种数据存储格式，却比 XML 更小、更快、 更易于人编写和阅读、更易于生成和解析。

## 在Unity中使用JSON

在Unity中，Json是最常用的一种文件格式，不过Unity引擎和C#语言本身并没有针对Json提供太方便的使用接口，常被人们所熟知`LitJson`就是一个流行的Unity插件，可以方便、快速地进行Json和对象之间的转换。

`LitJson`插件是一个Dll文件，在工程Assets下新建一个Plugins目录，把`LitJson.dll`导入其中，然后在调用代码处引入它的命名空间既可以使用了

`using LitJson;;`

### 将string类型的Json串转成Json数据并解析

在开发过程中经常会使用Json串发送数据，而这些数据发送完之后可能会变成string类型，这时，我们需要将其转换为Json数据并进行解析。比如现在有这么一串数据

`string data = "{\"姓名\":\"张三\",\"学号\":\"20210101\",\"班级\":\"三年一班\",\"年龄\":\"22\"}";`"\\"为转义字符

现在要在Unity中将其转换为Json数据并解析。 首先定义一个类来存储信息

```csharp
public class StudentInfo
{
    public string Name { get; set; }//姓名
    public string Number { get; set; }//学号
    public string Class { get; set; }//班级
    public int Age { get; set; }//年龄

    public void Info(string name, string number, string _class, string age)
    {
        this.Name = name;
        this.Number = number;
        this.Class = _class;
        this.Age = int.Parse(age);
    }
}

```

接下来我们就需要引用LitJson解析数据了

```vbscript
JsonData jsonData = JsonMapper.ToObject(data);//将string转成Json形式
StudentInfo studentInfo = new StudentInfo();//new 一个学生信息类
studentInfo.Info(jsonData["姓名"].ToString(), jsonData["学号"].ToString(), jsonData["班级"].ToString(), jsonData["年龄"].ToString());//传入参数
Debug.Log(studentInfo.Name + "," + studentInfo.Number + "," + studentInfo.Class + "," + studentInfo.Age);
```

这样就可以将数据解析出来了 在Unity中运行结果为

![image.png](https://p3-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/f20b904df50845739b912b1f346c560d~tplv-k3u1fbpfcp-zoom-in-crop-mark:3024:0:0:0.image)

### 读取json文件

这个其实和上述方法差不太多，只是先读取json数据，转换成数据流，然后再转换成json数据，然后在解析就OK了。  
首选判断文件是否存在，然后在进行读取

```ini
string path = Application.streamingAssetsPath +"/Config/JsonModel.json";
if(!File.Exists(path))
{
    return;
}
StreamReader streamreader =new StreamReader(Application.streamingAssetsPath +"/Config/JsonModel.json");//读取数据，转换成数据流
```

在之后将数据转换然后向上述方法一样解析

```ini
JsonReader js =new JsonReader(streamreader);//再转换成json数据
studentInfo = JsonMapper.ToObject<StudentInfo>(js);//读取
//释放资源
streamreader.Close();
streamreader.Dispose();
```

### 写入Json（新建或更新）

首先读取本地文件，查看是否存在，然后在进行操作 先定义一个class类

```csharp
public class StudentList
{ 
    public Dictionary<string, string> dictionary = new Dictionary<string, string>(); 
}
```

然后获取路径

```ini
string path = Application.streamingAssetsPath +"/Config/JsonModel.json";
```

通过`File.Exists(filePath)`判断文件是否存在。  
若存在更新键值对，不存在则创建键值对

```ini
public StudentList studentList = new StudentList();
```

```less
if (!File.Exists(filePath)) //不存在就创建键值对 
{ 
    studentList.dictionary.Add("Name", studentInfo.Name); 
    studentList.dictionary.Add("Number", studentInfo.Number); 
    studentList.dictionary.Add("Class", studentInfo.Class); 
    studentList.dictionary.Add("Age", studentInfo.Age.ToString()); 
} 
else //若存在就更新值 
{ 
    studentList.dictionary["Name"] = studentInfo.Name; 
    studentList.dictionary["Number"] = studentInfo.Number; 
    studentList.dictionary["Class"] = studentInfo.Class; 
    studentList.dictionary["Age"] = studentInfo.Age.ToString(); 
}
```

然后进行数据保存

```scss
//找到当前路径 
FileInfo file = new FileInfo(filePath); 
//判断有没有文件，有则打开文件，没有创建后打开文件 
StreamWriter sw = file.CreateText(); 
//ToJson接口将你的列表类传进去，并自动转换为string类型 
string json = JsonMapper.ToJson(studentList.dictionary); 
//将转换好的字符串存进文件 
sw.WriteLine(json); 
//注意释放资源 
sw.Close(); 
sw.Dispose(); 
//刷新资源
AssetDatabase.Refresh();
```

到这里Json文件也就写入完毕。

## 写在最后

所有分享的内容均为作者在日常开发过程中使用过的各种小功能点，分享出来也变相的回顾一下，如有写的不好的地方还请多多指教。Demo源码会在之后整理好之后分享给大家。