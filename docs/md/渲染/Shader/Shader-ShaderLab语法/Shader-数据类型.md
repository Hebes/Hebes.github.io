# Shader-数据类型

**[Unity Shader（四） 数据类型](https://blog.csdn.net/qq_39091751/article/details/105035654)**

## Cg/HLSL中的数据类型

几种常见的数据类型：

1. float、half、fixed（三个都是浮点型  只是精度不一样）
2. integer（整型）
3. sampler2D（2D纹理）
4. samplerCUBE（3D纹理）

+ float 高精度类型，32位，通常用于世界坐标下的位置，纹理UV，或涉及复杂函数的标量计算，如三角函数、幂运算等。
+ half 中精度类型，16位，数值范围为[-60000,+60000]，通常用于本地坐标下的位置、方向向量、HDR颜色等。
+ fixed 低精度类型，11位，数值范围为[-2,+2],通常用于常规的颜色与贴图，以及低精度间的一些运算变量等。

> 在PC平台不管你Shader中写的是half还是fixed，统统都会被当作float来处理。half与fixed仅在一些移动设备上有效。
> 比较常用的一个规则是，除了位置和坐标用float以外，其余的全部用half。主要原因也是因为大部分的现代GPU只支持32位与16位，也就是说只支持float和half，不支持fixed。

+ interger 整型类型，通常用于循环与数组的索引。

> 在 Direct3D 9 和 OpenGL ES 2.0平台上整型可能会被直接用浮点数来处理，在Direct3D 11、OpenGL ES 3等现代GPU上可以正确的以整型类型来处理。

+ sampler2D、sampler3D与samplerCUBE 纹理，默认情况下在移动平台纹理会被自动转换成低精度的纹理类型，如果你需要中精度的或者高精度的需要用以下方式来声明：

sampler2D\_half(中精度2D纹理)

sampler2D*\_*float(高精度2D纹理)

sampler3D\_half(中精度3D纹理)

sampler3D*\_*float(高精度3D纹理)

samplerCUBE\_halft(中精度立方体纹理)

samplerCUBE\_float(高精度立方体纹理)

## 类型对应

好了，现在我们已经了解了Cg/HLSL中的数据类型，那么Properties中的与Cg/HLSL中的是如何对应的呢？

+ Int/float/Range用浮点值表示，也就是float、half或者fixed，根据自己需要的精度来定义。
+ Vector/Color用float4､half4或者fixed4表示。
+ 2D类型用sampler2D表示。
+ 3D类型sampler3D表示。
+ CUBE类型用samplerCUBE表示。

单个浮点数值比较好理解，像Vector与Color的float4要如何理解呢？

其实不管是Vector还是Color，都是由四个同样精度的浮点数值组成的，所以我们在定义的时候才会写成float4､half4或者fixed4.

比如，我们在Properties中声明了如下的颜色：

```auto
_Color("Color", Color) = (1,1,1,1)
```

在Cg/HLSL中我们需要同样再声明一次：

```auto
fixed4 _Color;
```

颜色的四个分量：

+ Red（红）
+ Green（绿）
+ Blue（蓝）
+ Alpha（透明）

在Cg/HLSL中我们可以通过\_Color来访问颜色，也可以通过\_Color.rgba来访问，这里的.rgba就是表示颜色的四个分量，如果只想获得颜色的红通道就是\_Color.r*，*又如果只想获取绿通道和透明通道就是\_Color.ga，以此类推~

> 表示分量除了可以用.rgba，我们还可以使用.xyzw，它们的意义是一样的，你可以使用Vector.rgba，也可以使用Color.xyzw，这两者本身并没有什么区别，只是我们通常在颜色上用rgba,在向量上用xyzw,这样比较直观方便理解。

再说下矩阵，在Shader中，矩阵是一个按照长方形阵列排列的浮点数集合。

> 你可以想像成是一队站列整齐的士兵,横向有M人，竖向有N人。就可以用floatMxN来表示。如果是4x4矩阵，就是float4x4（同样支持其它精度），不过有一点要注意，在某些平台上是不支持非方矩阵的（比如float3x2），特别是OpenGL ES 2.0平台。

语义：

## 应用阶段传入顶点着色器的数据

```auto
struct appdata
{
	float4 vertex : POSITION;		//顶点
	float4 tangent : TANGENT;		//切线
	float3 normal : NORMAL;			//法线
	float4 texcoord : TEXCOORD0;	   //UV1
	float4 texcoord1 : TEXCOORD1;	   //UV2
	float4 texcoord2 : TEXCOORD2;	   //UV3
	float4 texcoord3 : TEXCOORD3;	   //UV4
	fixed4 color : COLOR;			//顶点色
};
```
