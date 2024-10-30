# Shader-Pass块

[ShaderLab 命令：UsePass](https://docs.unity.cn/cn/2021.1/Manual/SL-UsePass.html)
[ShaderLab：内置渲染管线中的预定义通道标签](https://docs.unity.cn/cn/2021.1/Manual/shader-predefined-pass-tags-built-in.html)
[ShaderLab 命令：GrabPass](https://docs.unity.cn/cn/2021.1/Manual/SL-GrabPass.html)
[ShaderLab：分配回退](https://docs.unity.cn/cn/2021.1/Manual/SL-Fallback.html)
[着色器编译：pragma 指令](https://docs.unity.cn/cn/2021.1/Manual/SL-PragmaDirectives.html)
[CG/hlsl 内置函数](https://huailiang.github.io/blog/2019/cg/)
[CG标准函数库](https://blog.csdn.net/jingmengshenaaa/article/details/52809879)
[N卡 CG标准函数库](https://developer.download.nvidia.cn/cg/index_stdlib.html)

## SubShader语义块Pass

> 一个Pass就是一次绘制，可以看成一个DC（Draw Call），对于表面着色器而言，只有一个Pass，但是顶点片元着色器可以有多个Pass，多个Pass可以实现一些特殊效果。Pass的意义在于多次渲染，如果你有一个Pass，那么着色器只会被调用一次，如果你有多个Pass的话，那么就相当于执行多次SubShader了，这就叫双通道或者多通道。在编写Shader的时候，Pass要尽量的少，每多一个Pass，那么DC就会多一次！而且我们可以在一个Shader中使用另一个Shader的某一个Pass块，Pass块是可以命名的，使用use "passname"这样的方式可调用，[具体方式](https://docs.unity3d.com/Manual/SL-UsePass.html)。
> Pass { \[Name and Tags\] \[RenderSetup\] \[TextureSetup\] }
> Name and tags：一个Pass 可以定义它的名字和任意数量的标签-name/value 字符串 将Pass的含义告诉给渲染引擎。
> Render Setup 渲染设置： Pass设置了一系列的显卡状态，比如说alpha混合是否开启，是否允许雾化等等。
> Texture Setup 纹理设置：当渲染状态设置以后，你可以用SetTexture命令指定这些纹理和他们的结合方式。

```csharp

Shader "Example" 
{ 
    Properties{ }
    SubShader
    {
	UsePass "Example/name"

	// 将对象后面的屏幕抓取到 _BackgroundTexture中  
	GrabPass
        {
            "_BackgroundTexture"
        }

        Tags{ }
        // 渲染设置

        Pass 
        {
            	// Pass通道名称，可以当成一个函数进行使用，使用时需要全名称大小调用
            	Name "name"
            	// Pass标签
            	Tags{ }
            	// Render Setup 渲染设置
            	// Texture Setup 纹理设置
  
            	// CG语言所写的代码，主要是顶点/片元着色器
            	CGPROGRAM// 插入Cg代码开始

		//定义实现顶点/片元着色器代码的函数名称
		//#pragma vertex name（实现顶点着色器的函数名）
		//#pragma fragment name（实现片元着色器的函数名）

		//uint	32为无符号整形
		//int	32位整形

		//float	32位浮点数符号：
		//half	16位浮点数符号：
		//fixed	12位浮点数

		//bool布尔类型
		//string字符串

		//sampler 纹理对象句柄
			//sampler:通用的纹理采样器，可以用于处理各种不同维度和类型的纹理
			//sampler1D:用于一维纹理，通常用于对一维纹理进行采样，例如从左到右的渐变色门
			//sampler2D:用于二维纹理，最常见的纹理类型之一。它用于处理二维图像纹理，例如贴图
			//sampler3D:用于三维纹理，通常用于体积纹理，例如体积渲染
			//samplercUBE：用于立方体纹理，通常用于处理环境映射等需要立方体贴图的情况
			//samplerRECT：用于处理矩形纹理，通常用于一些非标准的纹理映射需求
			//他们都是用于处理纹理（Texture）数据的数据类型，他们的主要区别是纹理的维度和类型

		//数组：和C#中类似
		//一维 int a[4]={1,2,3,4} 长度 a.length
		//二维 int b[2][3]={{1,2,3},{4,5,6}} 长度 b.length为2 b[o].length为3
		//结构体 和c#基本-样 没有访问修饰符 结构体声明结束加分号 一般在函数外声明


		//2维向量fixed2 f2=fixed2（1.2,2.5);
		//3维向量float3 f3=f1oat3(2,3,4);
		//4维向量int4 i4=int4（1,2,3,4)


		//矩阵声明
		//int2x3 mInt2x3={1,2,3， 4,5,6};
		//float3x3 mF1oat3x3 ={1,2,3, 	4,5,6, 	7,8,9}；
		//fixed4x4mFixed4x4={1,2,3,4， 4,5,6,7， 7,8,9,10, 11,12,13,14}


		//boo1类型同样可以用于如同向量一样声明
		//它可以用于存储一些逻辑判断结果
		//比如
		//f1oat3 a=f1oat3(0.5,0.0,1.0);
		//f1oat3b=f1oat3（0.6,-0.1,0.9);
		//bool3c=a<b;
		//运算后向量c的结果为bool3（true，false，faLse)


		//Swizzle操作符通常以点号（.）的形式使用，后面跟着所需的分量顺序
		//对于四维向量来说
		//我们可以通过
		//向量.xyzw1
		//或
		//向量.rgba
		//两种分量的写法来表示向量中的四个值
		//其中xyzw和rgba分别代表四维向量中的四个元素
		//在此的意义就是向量一般可以用来表示坐标和颜色


		//基本结构
		//void name（in参数类型参数名，out 参数类型参数名）
		//{
			//函数体
		//}
		//void：以void开头，表示没有返回值
		//name：函数的名称
		//in：表示是输入参数，表示由函数外部传递给函数内部，内部不会修改该参数，只会使用该参数进行计算，允许有多个
		//out：表示是输出参数，表示由函数内部传递给函数的调用者，在函数内部必须对该参数值进行初始化或修改，允许有多个
		//注意：
			//in和out都可以省略，省略后就没有了in和out相关的限制
			//虽然可以省略，但是建议大家在编写shader时不要省略int和out
			//因为他们可以明确参数的传递方式，提高代码的可读性和可维护性
			//可以让我们更容易的理解函数是如何与参数交互的，减少潜在的误解可能

		//基本结构
		//typename（in 参数类型 参数名）
		//{
			//函数体
			//return 返回值；
		//}
		//type：返回值类型
		//return：返回指定类型的数据
		//注意：//虽然可以在有返回值的函数中使用out参数//但是这并不是常见做法，除非是一些自定义逻辑函数V对于顶点/片元着色器函数只会使用单返回值的方式进行处理


		//顶点着色器基本结构
		//CGPROGRAM
		//#pragma vertex myVert
		顶点看色器回调函数//POSITION和 SV POSITION是CG语言的语义//POSITION:把模型的顶点坐标填充到输入的参数v当中
		//float4 myVert(float4 v:POSITION):SV_POSITIO
		//{
			return mul(UNITY_MATRIX_MVP,v);
		//}
		//ENDCG

		//ShaderLab属性类型CG变量类型
		//Color,Vector 				float3,half4,fixed4
		//Range,Float,Int 			float,half,fixed
		//2D 					sampler2D
		//Cube					samplerCube
		//3D					sampler3D
		//2DArray				sampler2DArray

            	ENDCG // 插入Cg代码结束
        }
    }
    Fallback Off
}
```

### SurfaceShader

## Fallback

> 回滚，相当于备胎。
