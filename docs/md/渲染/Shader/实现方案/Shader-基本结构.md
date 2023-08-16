# Shader-基本结构

## shader的基本结构

```auto
Shader "ShaderName"{
   Properties{
    ...
   }
   SubShader{
    ...
   }
   
   Fallback "VertexLit"
}
```

+ ShaderName是自己定义的shader名字，可以类似“Custom/MyShader”这样来定义目录结构，不需要和文件名相同。
+ Properties是用来自定义属性
+ SubShader是用了编写具体的代码片段
+ Fallback是在出现SubShader的代码片段在目标机器上编译失败时会采用的默认shader。

## Properties 基础属性

| 类型 | 示例语法 | 注释 |
| --- | --- | --- |
| Int | \_ExampleName (“Int display name”, Int) = 1 | 注意：尽管名称如此，但是此类型实际上受浮点数支持。 |
| Float | \_ExampleName (“Float display name”, Float) = 0.5\_ExampleName (“Float with range”, Range(0.0, 1.0)) = 0.5 | 范围滑动条的最大值和最小值包含在内。 |
| Texture2D | \_ExampleName (“Texture2D display name”, 2D) = “red” {} \_ExampleName (“Texture2D display name”, 2D) = “” {} | 将以下值置于默认值字符串中可使用 Unity 的内置纹理之一：“white”（RGBA：1,1,1,1）、“black”（RGBA：0,0,0,1）、“gray”（RGBA：0.5,0.5,0.5,1）、“bump”（RGBA：0.5,0.5,1,0.5）或“red”（RGBA：1,0,0,1）。 如果将该字符串留空或输入无效值，则它默认为 “gray”。 注意：这些默认纹理在 Inspector 中不可见。 |
| Texture2DArray | \_ExampleName (“Texture2DArray display name”, 2DArray) = “” {} | 纹理数组。 |
| Texture3D | \_ExampleName (“Texture3D”, 3D) = “” {} | 默认值为 “gray”（RGBA：0.5,0.5,0.5,1）纹理。 |
| Cubemap | \_ExampleName (“Cubemap”, Cube) = “” {} | 默认值为 “gray”（RGBA：0.5,0.5,0.5,1）纹理。 |
| CubemapArray | \_ExampleName (“CubemapArray”, CubeArray) = “” {} | 立方体贴图数组。 |
| Color | \_ExampleName(“Example color”, Color) = (.25, .5, .5, 1) | 这会在着色器代码中映射到 float4。 材质 Inspector 会显示一个拾色器。如果更愿意将值作为四个单独的浮点数进行编辑，请使用 Vector 类型。 |
| Vector | \_ExampleName (“Example vector”, Vector) = (.25, .5, .5, 1) | 这会在着色器代码中映射到 float4。 材质 Inspector 会显示四个单独的浮点数字段。如果更愿意使用拾色器编辑值，请使用 Color 类型。 |

## SubShader 自定义子着色器

+ SubShader结构
  SubShader
  {
  <optional: LOD>
  <optional: tags>
  <optional: commands>
  
  }
  下面是一个简单示例。
  一个 顶点/片断着色器。这也是我们以后所有示例采用的着色器编写方式。
  
```auto
Shader "Mark/Unlit" {
    Properties {
        MainTex ("Base (RGB)", 2D) = "white" {}
        _Color("Main Color", Color) = (1,1,1,1)
    }   
    SubShader {
        Tags    {"RenderPipeline"="UniversalRenderPipel ne" "RenderType"="Opaque" }  
        Pass {
            HLSLPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma target 2.0
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    struct appdata_t {
      float4 vertex : POSITION;
      float2 texcoord : TEXCOORD0;
    };

    struct v2f {
      float4 vertex : SV_POSITION;
      float2 texcoord : TEXCOORD0;
    };

    sampler2D _MainTex;
    half4 _Color;
    float4 _MainTex_ST;

    v2f vert (appdata_t v)
    {
      v2f o;
      o.vertex = TransformObjectToHClip(v.vertex.xyz);
      o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
      return o;
    }

    half4 frag (v2f i) : SV_Target
    {
      half4 col = tex2D(_MainTex,i.texcoord);
      return col*_Color;
    }
            ENDHLSL
        }
    }
}

```

+ 使用HLSL 访问着色器属性
  着色器在 Properties 代码块中声明材质属性。如果要在着色器程序中访问其中一些属性，则需要声明具有相同名称和匹配类型的 HLSL 变量。
  例如上面示例：
  属性中定义了\_Color,那么在代码片段我们就需要定义一个

```auto
half4 _Color;
```

来在代码中进行使用。

## HLSL 数据类型

float、int、sampler2D、 samplerCUBE

## ShaderLab 中的属性类型以如下方式映射到 Cg/HLSL 变量类型

+ Color 和 Vector 属性映射到 float4、half4 变量。
+ Range 和 Float 属性映射到 float、half 变量。
+ 对于普通 (2D) 纹理，Texture 属性映射到 sampler2D 变量；立方体贴图 (Cubemap) 映射到 samplerCUBE\_\_；3D 纹理映射到 sampler3D\_\_。

## 复合矢量/矩阵类型

HLSL 具有从基本类型创建的内置矢量和矩阵类型。例如，float3 是一个 3D 矢量，具有分量 .x、.y 和 .z，而 half4 是一个中等精度 4D 矢量，具有分量 .x、.y、.z 和 .w。或者，可使用 .r、.g、.b 和 .a 分量来对矢量编制索引，这在处理颜色时很有用。

矩阵类型以类似的方式构建；例如 float4x4 是一个 4x4 变换矩阵。请注意，某些平台仅支持方形矩阵，最主要的是 OpenGL ES 2.0。

## 纹理/采样器类型

通常按照如下方式在 HLSL 代码中声明纹理：

```auto
sampler2D _MainTex;
samplerCUBE _Cubemap;
```

## Fallback 分配回退

如果找不到兼容的 SubShader，则使用指定的 Shader 对象。
如果不需要shader使用回退功能，也可以使用

```auto
Fallback Off
```

使用这个属性，在出现自定义shader不匹配的时候，会显示默认的 error shader，也就是渲染对象会显示成紫色。

## 来源网站

**[木匠沙](<https://blog.csdn.net/king_mumu/article/details/124656087>)**
