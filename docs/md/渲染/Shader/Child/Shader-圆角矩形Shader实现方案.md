# Shader-圆角矩形Shader实现方案

## 背景

> 在优化项目的过程中，我发现许多圆角矩形图片和遮罩虽然颜色，圆角大小和图片尺寸不同以外，其余方面基本相同。此外，这些类似的图片素材大量存在导致包体变得较大。因此，我们编写了一个Shader来解决这个问题,这样就可以将多余的圆角矩形素材删除，使用此Shader创建的材质赋予图片圆角效果。

## 概要

编写一个圆角矩形的Shader是一个很好的练习，它可以使我们更好地理解在Unity中如何编写自定义着色器。在本教程中，我们将讨论如何编写一个简单的圆角矩形着色器，该着色器将为矩形添加圆角，并控制圆角的大小。

# 一、定义变量参数、着色器的渲染方式和队列信息

首先，在Unity创建一个新的Unlit Shader，并将其命名为”RoundedRect”。然后在属性块中定义需要的变量参数：

```auto
Properties
{
    _MainTex ("Texture", 2D) = "white" {}
    _Radius ("Radius",Range(0,0.3)) = 0
    _Ratio("Height/Width",Range(0,3) )= 1
    _Color("AddColor",Color)=(1,1,1,1)
    _Reverse("Reverse",float )= 1

}
```

这里定义了5个变量参数：

\_MainTex：矩形的纹理贴图。  
\_Radius：圆角的半径。  
\_Ratio：高度与宽度之比。  
\_Color：颜色叠加。  
\_Reverse：当值为1时，剔除的是圆角外框；当值为其他数值时，则剔除的是圆角内部。  
其中有两个变量 \_Radius 和 \_Ratio 都使用了 Range 属性来限制它们的值域范围。

接下来，在SubShader块中定义着色器的渲染方式和队列信息：

```auto
SubShader
{
    Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
    LOD 100
    Blend SrcAlpha OneMinusSrcAlpha

    Pass
    {
        CGPROGRAM
        ...
        ENDCG
    }
}
```

# 二、编写着色器

在pass块中，我们开始编写着色器的顶点和片元着色器代码。

首先，我们需要定义输入结构体 appdata 和输出结构体 v2f。其中，appdata 结构体包含顶点位置信息和纹理坐标信息，v2f 结构体则包含纹理坐标和变换后的顶点位置信息。

```auto
struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};
```

接下来，我们需要将纹理坐标从原始坐标系转换到纹理坐标系，并将其传递给输出结构体：

```auto
v2f vert (appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    return o;
}
```

其中，UnityObjectToClipPos() 函数将顶点位置从物体坐标系变换到剪裁空间坐标系，TRANSFORM\_TEX() 函数将纹理坐标从原始坐标系转换到纹理坐标系。

最后，我们需要编写片元着色器代码。在这里，我们首先将坐标等效为左下角的位置，进行圆角矩形边界的判断（后文有详细介绍），最后在获取当前像素的颜色值并叠加：

```auto
fixed4 frag (v2f i) : SV_Target
{
    //将坐标等效为左下角的位置。
    float2 p = abs(step(0.5,i.uv) - i.uv);

    //如果三个条件同时满足，则结果为0；否则结果为1。
    float temp=(step(_Radius,p.x) ||step( _Radius  ,p.y*_Ratio) || step(length(float2(p.x-_Radius,p.y*_Ratio-_Radius)),_Radius));
   
    if(_Reverse==1)temp=temp>0?0:1;

    // 获取当前像素的颜色值并叠加
    fixed4 col =  tex2D(_MainTex, i.uv)* temp;
    return col*_Color;
}
```

在代码中，我们首先计算当前像素的位置`p`，这里采用了前面提到过的 `abs()` 和 `step()` 函数。接着，使用 `length()` 函数来计算圆角边缘处的斜线是否需要进行渲染。最后，使用颜色值和透明度信息来对当前像素进行着色，并在最后将其乘以指定的颜色叠加值。

# 三、关键代码

## 1、像素点距离计算

```auto
float2 p = abs(step(0.5, i.uv) - i.uv);
```

我们需要将纹理坐标系转化为以左下角为原点的坐标系。为了实现这个功能，我们使用 step 函数将纹理坐标系转化为分段函数的形式。

例如，当纹理坐标 i.uv 的值小于等于 0.5 时，step(0.5, i.uv) 返回 1，否则返回 0。这样，通过 abs(step(0.5, i.uv) – i.uv) 就可以将取值范围从 0 到 1 转换为 0 到 0.5 和 0.5 到 1 两个区间。

## 2、实现圆角的渲染效果

```auto
float temp = (step(_Radius, p.x) || step(_Radius, p.y * _Ratio) || step(length(float2(p.x - _Radius, p.y * _Ratio - _Radius)), _Radius));
```

这一行代码实现了一个圆角的渲染效果。

其中，step 函数被用于判断当前像素点是否在圆角范围内。第一个 step 函数 `step(_Radius, p.x)` 用于判断像素点距离圆心的横向距离是否小于等于半径 `_Radius`。如果是，则返回 1；否则返回 0。

第二个 step 函数 `step(_Radius, p.y * _Ratio)` 则用于判断当前像素点距离圆心的纵向距离是否小于等于半径 `_Radius`。由于在 Shader 中，坐标系默认以左下角为原点，因此我们需要将纵坐标乘上 `_Ratio` 来调整宽高比。

第三个 step 函数 `step(length(float2(p.x - _Radius, p.y * _Ratio - _Radius)), _Radius)` 则用于判断像素点到圆心的距离是否小于等于半径 `_Radius`。具体来说，这个函数先计算了当前像素点和圆心之间的距离，然后使用 step 函数将距离转化为 0 或 1。

最终，这三个 step 函数通过逻辑运算符（||）进行组合，得到了一个 0 或 1 的值，表示当前像素点是否在圆形范围内。如果这个值为 1，则将该像素点设置为不透明，否则设置为透明，从而实现了一个圆角的渲染效果。

总之，这一行代码是 Shader 中非常常见的一种实现圆角渲染效果的方式

# 三、完整Shader代码

最后，在完整的Shader代码中，要注意添加CG程序的头文件，并在Subshader块的末尾添加 `ENDCG` 来结束CG程序。

下面是完整的代码：

```auto
Shader "Unlit/RoundedRect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius",Range(0,0.3)) = 0
        _Ratio("Height/Width",Range(0,3) )= 1
        _Color("AddColor",Color)=(1,1,1,1)
        _Reverse("Reverse",float )= 1

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;
            float _Ratio;
            fixed4 _Color;
            float _Reverse;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {  
                float2 p = abs(step(0.5,i.uv) - i.uv);
                float temp=(step(_Radius,p.x) ||step( _Radius  ,p.y*_Ratio) || step(length(float2(p.x-_Radius,p.y*_Ratio-_Radius)),_Radius));
                if(_Reverse==1)temp=temp>0?0:1;
                fixed4 col =  tex2D(_MainTex, i.uv) *temp;
                return col*_Color;
            }

            ENDCG
        }
    }
}
```

使用上述Shader，我们就可以成功地为矩形添加圆角，并控制圆角的大小和透明度。这也是编写自定义着色器非常有用的一种方法，它可以让开发者实现更多复杂的效果。

结果示例：

![1](\../Image/Shader-圆角矩形Shader实现方案/1.jpg)
