# Unity功能-控制鼠标

**[Unity实用功能之控制鼠标（隐藏，锁定及更改样式）](<https://juejin.cn/post/6995338757331238943>)**

## 概述

平时在开发过程中，为了程序美化，经常会需要隐藏鼠标，当需要点击的时候在显示出来，比如在游戏中的时候隐藏鼠标，打开背包后显示鼠标。或者是在程序中更改一个鼠标样式，是玩家能够更好的融入到游戏中。本片文章主要介绍一下如何隐藏与显示鼠标和在程序中更换鼠标样式。

## 鼠标显示与隐藏

在Unity中，想要控制鼠标，我们需要使用到`Cursor`。我们直接通过设置`Cursor.visible`属性，即可达到鼠标的显示与隐藏  
状态：true显示，false隐藏

```ini
//隐藏鼠标
Cursor.visible = false;
```

## 鼠标锁定

通常隐藏鼠标之后，有的时候不知道鼠标在哪里，这就导致当需要显示鼠标的时候我们还要满屏幕寻找显示的鼠标。这时候就需要`Cursor.lockState`属性的配合,`Cursor.lockState`属性的作用是锁住鼠标，使其一直保持在屏幕中心。防止即使隐藏了鼠标，依然还会把鼠标移到游戏外面，和显示鼠标时还需要到处寻找鼠标的问题。让我们来看一下`Cursor.lockState`的属性都有哪些

1. 鼠标锁定并消失

    ```ini
    Cursor.lockState = CursorLockMode.Locked;
    ```

2. 鼠标解锁并显示

    ```ini
    Cursor.lockState = CursorLockMode.None;
    ```

3. 鼠标限定在Game视图中

    ```ini
    Cursor.lockState = CursorLockMode.Confined;
    ```

最终实现结果如下：

![1](../Image/Unity%E5%8A%9F%E8%83%BD-%E6%8E%A7%E5%88%B6%E9%BC%A0%E6%A0%87/1.png)

![2](../Image/Unity%E5%8A%9F%E8%83%BD-%E6%8E%A7%E5%88%B6%E9%BC%A0%E6%A0%87/2.gif)

## 更改鼠标样式

在Unity中修改鼠标样式方法有多中，一种是在Unity中设置，还有一种是通过代码修改。前提是都需要准备一张鼠标样式图片

### 方法一、通过设置修改鼠标样式

此方法操作非常简单，而且修改完立刻生效，在Unity的Game视图中不论是否运行状态，都能够直接显示更改后的鼠标样式。  
首先导入需要使用的鼠标图片，每张图片导入进来后都会默认有一个Texture Type，一般为`Default`格式或`Sprite(2D and UI)` 格式，第一步需要做的是将图片格式修改为`Cursor`，切记修改完后要点击`Apply`. ![image.png](https://p9-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/66af26aac1a2474489473b8a69d4a0d0~tplv-k3u1fbpfcp-zoom-in-crop-mark:3024:0:0:0.image) 如果不修改图片格式，修改完鼠标样式就有可能出现如下情况，显示不出来鼠标图片 ![454534242.gif](https://p1-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/2fc2a67afd1a4968867257a74be6baef~tplv-k3u1fbpfcp-zoom-in-crop-mark:3024:0:0:0.image) 修改完鼠标鼠标图片格式，接下来就是修改鼠标样式了，在`Edit->Project Setting->Player->Default Cursor`中设置，将更改好的图片拖拽赋值到`Default Cursor`中即可，具体位置如图

![image.png](https://p3-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/668fa53e56c64dfb88a9536f17391c20~tplv-k3u1fbpfcp-zoom-in-crop-mark:3024:0:0:0.image) 效果如下，此方法只会在Game视图发生变化，其他地方还是原来的样式。而且只需要更改这一个地方，很方便！

![45453.gif](https://p6-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/bb41c8344af94dffbba5f95965896c48~tplv-k3u1fbpfcp-zoom-in-crop-mark:3024:0:0:0.image)

### 方法二、通过代码修改鼠标样式

上面的方法修改起来非常简单，但是实用性不太好，上述方法修改完后只能一直保持一种状态，无法实现多种状态切换，多以，接下来的方法正好能够完美的解决。 其核心代码只有一句话，就是调用`Cursor.SetCursor`,如下图

![image.png](https://p1-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/5375add6626e44bea1c12f213e58bec1~tplv-k3u1fbpfcp-zoom-in-crop-mark:3024:0:0:0.image) 让我们来看一下此句代码的具体参数。

```arduino
public static void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode);
```

1.  Texture2D texture：要替换的光标图片
2.  Vector2 hotspot： 响应区域 (vector2.zero)
3.  CursorMode cursorMode：渲染形式  
    渲染形式共有两种:
    1.  ```makefile
        Auto:平台自适应显示
        ```
        
    2.  ```makefile
        ForceSoftware:强制使用软件游标
        ```
        

只要在想要修改鼠标样式的时候调用这一行代码，将里面的第一个参数赋值成我们想要的图片样式就好了！可以是鼠标进入一个状态，鼠标点击一个状态等等，就等着大家自由发挥了。。。

## 写在最后

所有分享的内容均为作者在日常开发过程中使用过的各种小功能点，分享出来也变相的回顾一下，如有写的不好的地方还请多多指教。Demo源码会在之后整理好之后分享给大家。欢迎大家相互学习进步。