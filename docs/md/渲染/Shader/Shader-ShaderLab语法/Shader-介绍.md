# Shader-介绍

## Unity Shader与Shader

> **Unity Shader ！= 真正的Shader**
> Unity Shader实际上是指的就是一个ShaderLab文件。以.Shader作为后缀的一种文件。在Unity Shader里面，我们可以做的事情远多于一个传统意义上的Shader。

### Unity Shader优点

> 1.在传统的Shader中，我们仅可以编写特定类型的Shader，例如顶点着色器、片元着色器等。而在Unity Shader中，我们可以在同一个文件里同时包含需要的顶点着色器和片元着色器代码。  
> 2.在传统的Shader中，我们无法设置一些渲染设置，例如是否开启混合、深度测试等，这些是开发者在另外的代码中自行设置的。而在Unity Shader中，我们通过一行特定的指令就可以完成这些设置。  
> 3.在传统的Shader中，我们需要编写冗长的代码来设置着色器的输入输出，要小心地处理这些输入输出的位置对应关系等。而在Unity Shader中，我们只需要在特定语句块中声明一些属性，就可以依靠材质来方便地改变这些属性。而且对于模型自带的数据（如顶点位置、纹理坐标、法线等），Unity Shader也提供了直接访问的方法，不需要开发者自行编码来传给着色器。

### Unity Shader缺点

> 由于Unity Shader的高度封装性，我们可以编写的Shader类型和语法都被限制了。例如：Unity对曲面细分着色器、几何着色器等的支持性就差一些。  
> 可以说，Unity Shader提供了一种让开发者同时控制渲染流水线中多个阶段的方式，不仅仅是提供Shader代码。作为开发者而言，我们绝大部分时候只需要和Unity Shader打交道，而不需要关心渲染引擎底层的实现细节。

## CG/HLSL与ShaderLab

> Unity Shader是使用ShaderLab语言编写的，但对于表面着色器和顶点/片元着色器，我们可以在ShaderLab内部嵌套CG/HLSL语言来编写这些着色器代码。CG/HLSL代码是区别于ShaderLab的另一个世界。
> 从本质上讲，Unity Shader只有两种形式：顶点/片元着色器（Unity会把表面着色器转化为包含多Pass的顶点/片元着色器）和固定函数着色器（Unity5.2版本以后，固定函数着色器会被转化成顶点/片元着色器，因此本质上说UnityShader只有顶点/片元着色器一种形式）。

## Unity Shader模板

> Unity一共提供了4 种Unity Shader 模板供我们选择——  
> Standard Surface Shader：包含了标准光照模型基于物理的渲染方法表面着色器模板 （PBR）  
> Unlit Shader：一个不包含光照（但包含雾效）的基本的顶点／片元着色器  
> Image Effect Shader： 实现各种屏幕后处理效果  
> Compute Shader：特殊Shader，利用GPU 的并行性来进行一些与常规渲染流水线无关的计算