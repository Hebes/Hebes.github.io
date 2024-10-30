# Shader-Properties块

**[ShaderLab：Properties](https://docs.unity.cn/cn/2018.4/Manual/SL-Properties.html)**
**[Shader属性的常用特性](https://blog.csdn.net/weixin_43834106/article/details/105654071)**

![alt text](1.png)
Type类型

* Color 颜色
* Int 整数
* Float 浮点数
* Vector 四维向量
* 2D 纹理
* 3D 纹理
* Cube 立方体纹理

```csharp
Shader "Example" //路径和名称
{ 
    //属性块：定义了一些属性参数，可在Unity编辑器的“Inspector”面板中编辑和调整。
    Properties
    { 
        _MainTex("Texture",2D) = "White" {} //_MainTex是变量名 ("Texture"是在“Inspector”中的名字, 2D是变量类型) = "White" {}是默认值
        _Int("Int",Int) = 2// 整数：创建一个让用户填入整数的栏位
        _Float("Float",float) = 1.5 // 浮点数：创建一个让用户填入浮点数的栏位
        _Range("Range",range(0.0,2.0)) = 1.0// 在（min，max）范围内的浮点数：创建一个滑动条，可以让用户选择在min和max之间的浮点数值
        _Color("Color",color) = (1,1,1,1)// 颜色 RGBA：创建一个调色板选择器，可以让用户选择颜色
        _Vector("Vector",Vector) = (1,2,3,4)// 四维向量：创建4个栏位，让用户可以填入相应的浮点数，代表一个Vecto4
        _2D("Texture",2D) = "White" {}// 创建一个图片选择框，可以让用户选择贴图
        _Rect("Rect",Rect) = "White" {}// 矩形纹理：创建一个non-power-of-2贴图选择框，功能基本跟2D想同
        _Cube("Cube",Cube) = "White" {}// 立方体贴图纹理：创建一个选择Cubmap的框
        _3D("3D",3D) = "black" {} // 3D纹理
    } 
    SubShader{ }
}
```
