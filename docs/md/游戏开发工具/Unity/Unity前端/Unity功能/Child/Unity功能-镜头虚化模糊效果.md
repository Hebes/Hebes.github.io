# Unity功能-镜头虚化模糊效果

【Unity】实现镜头虚化模糊效果（Blur）：camera模糊，UGUI模糊，FairyGUI模糊效果

## 前言

起因是在一个FairyGUI项目里，有个项目需求要求弹出框的背景是虚化模糊效果，包括背景后的UI模糊和场景模糊，于是有了这一次unity实现模糊效果的测试记录。

先贴一下测试使用的unity工程：  
使用FairyGUI建了一个UI界面，建造了一个Cube作为场景里的物体。  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200628223055544.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzQxNDY4MjE5,size_16,color_FFFFFF,t_70)

### 1.FairyGUI自带的模糊滤镜

FairyGUI可以使一个显示对象进入绘画模式，简单的说就是将目标对象整体画到一张纹理上，然后就可以操作这个纹理实现一些特殊的效果。这里简单使用到的就是**对组件使用任意滤镜**和**对任意对象使用模糊滤镜**。  
对任意对象使用模糊滤镜的测试：  
代码：

```csharp
using UnityEngine;
using FairyGUI;

public class MainBlur : MonoBehaviour
{
    GComponent _mainView;

    void Start()
    {
        //加载UI包
        UIPackage.AddPackage("UI/Main");
        UIPanel panel = this.GetComponent<UIPanel>();
        //获取主容器组件
        _mainView = panel.ui;
        CreateBlur();
    }

    void CreateBlur()
    {
        BlurFilter filter = new BlurFilter();
        //找到背景，添加模糊滤镜，背景图片在FairyGUI工程里名称是n0
        _mainView.GetChild("n0").filter = filter;
        //设置模糊程度
        filter.blurSize = 0.2f;
    }
}

```

运行结果如下图，可以看到只有背景图片模糊了  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200629195825823.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzQxNDY4MjE5,size_16,color_FFFFFF,t_70)  
在测试一遍对主容器组件使用模糊滤镜，只需把上述代码改动一点：

```csharp
        //_mainView.GetChild("n0").filter = filter;
        _mainView.filter = filter;
```

这回能看到整个界面上的UI都变模糊了，但是对组件使用模糊滤镜有一个限制，就是组件内**只有实体UI才会被模糊，空白处是不会被模糊**的，所以FairyGUI自带的模糊滤镜在这里是没法满足项目的需求的。  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200629200436816.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzQxNDY4MjE5,size_16,color_FFFFFF,t_70)

### 2.Camera模糊

之前有一款ImageEffect插件可以轻松的实现这种效果，但是目前AssetStore这款插件好像已经下架了。这里只找到了BlurOptimized组件来做测试:  
将BlurOptimized组件挂在需要模糊的camera下，选中shader，就能看到该摄像机下看到的物体都已经模糊。  
这种效果适合对一整个摄像机下物体模糊使用，但是上面提到的需求，是对整个场景，和部分UI界面进行模糊，游戏场景可以直接使用这个，但是部分UI界面就没法使用这种方法了。  
当前的项目需求也考虑过使用UI相机和场景相机同时截屏模糊，再作为背景图片放到UI上，但是这样过程有些繁琐，暂时不采用。  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200629201624451.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzQxNDY4MjE5,size_16,color_FFFFFF,t_70)

### 3.UGUI部分模糊

这里找到一个shader，引用[链接](https://blog.csdn.net/qq_39162826/article/details/102978580)，测试一遍：  
照着链接里的说法，先把image的透明度设置为0，然后把加有shader的material赋给image就可以了。这种方法好像是目前最好用的了。  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200629203205422.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzQxNDY4MjE5,size_16,color_FFFFFF,t_70)  
但是项目需求是在FairyGUI基础上，如何实现（场景+部分UI模糊 部分UI清晰）的区别呢  
在这个测试例子里，图片和颜色动画同在一个UI界面上，要实现的效果就是场景里的cube和UI里的图片要模糊，但是颜色动画不要模糊。  
测试一下：  
代码：  
代码中使用的材质就是上面UGUI模糊使用的材质。

```csharp
using UnityEngine;
using FairyGUI;

public class MainBlur : MonoBehaviour
{
    GComponent _mainView;

    public Material material;

    void Start()
    {
        //加载UI包
        UIPackage.AddPackage("UI/Main");
        UIPanel panel = this.GetComponent<UIPanel>();
        //获取主容器组件
        _mainView = panel.ui;
        CreateBlur();
    }

    void CreateBlur()
    {
        //新建一个图形遮罩
        GGraph graph = new GGraph();
        //将图形遮罩添加到UI界面上，并且层级在图片背景前，颜色动画后
        _mainView.AddChildAt(graph, 1);
        //绘制一个矩形的全屏遮罩
        graph.SetPosition(0, 0, 0);
        graph.DrawRect(1080, 1920, 1, Color.black, Color.white);
        //给图形赋材质
        graph.displayObject.material = material;
    }
}
```

运行效果：  
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200629205513379.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzQxNDY4MjE5,size_16,color_FFFFFF,t_70)  
利用UI的层级关系，实现了场景和部分UI的模糊效果。

## 文章来源

**[【Unity】实现镜头虚化模糊效果（Blur）：camera模糊，UGUI模糊，FairyGUI模糊效果](<https://blog.csdn.net/qq_41468219/article/details/107008910>)**
