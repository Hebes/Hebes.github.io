# Shader-Tags

## Unity Shader Tags

> Unity ShaderLab中，经常会看到各种Tag(标签)。这里大致分为两类Tag，SubShader Tag 和 Pass Tag。
> 里面写了走里面，外面写了走外面，如果里外定义相同key使用外面的

```csharp
Shader "Example" 
{ 
    Properties{ }
    SubShader
    { 
        //标签 可选 key = value
        Tags 
        {
            // 渲染顺序：使用 Queue标签，分别放入不同的渲染队列中
            "Queue" = "Transparent"
            // 着色器替换功能
            "RenderType" = "Opaque"
            // 是否进行合批
            "DisableBatching" = "True"
            // 是否投射阴影
            "ForceNoShadowCasting" = "True"
            // 是否受Projector的影响，通常用于透明物体
            "IgnoreProjector" = "True"
            // 是否用于图片的shader，通常用于UI
            "CanUseSpriteAltas" = "False"
            // 用作shader面板预览的类型
            "PreviewType" = "Plane"
        }

        Pass
        {
            // 在pass通道中特有的Tag设置
            Tags 
            {
                // 定义该Pass通道在Unity渲染流水中的角色
                "LightMode" = "ForwardBase"
                // 当满足某些条件时才渲染该Pass通道
                "RequireOptions" = "SoftVegetation"
            }
        }
    }
    Fallback Off
}

```

### SubShader Tag

**Rendering Order - Queue tag:**

> 在3D引擎中，我们经常要对透明和不透明物体进行排序。先渲染不透明再渲染透明物体，Unity使用 Queue标签，分别放入不同的渲染队列中。
> **Background**：背景，一般天空盒之类的使用这个标签，最早被渲染  
> **Geometry**（default）：适用于大部分不透明的物体  
> **AlphaTest**：如果Shader要使用AlphaTest功能，使用这个队列性能更高  
> **Transparent**：这个渲染队列在AlphaTest之后，Shader中有用到Alpha Blend的，或者深入不写入的都应该放在这个队列。  
> **Overlay**：最后渲染的队列，全屏幕后期的，都应该使用这个  
> this render queue is meant for overlay effects. Anything rendered last should go here (e.g. lens flares).

**RenderType tag:**

> RenderType tag可以用自定义的字符串，在使用ShadeReplacement，全局替换渲染的时候有用。
> **Opaque**：用于大多数着色器（法线着色器、自发光着色器、反射着色器以及地形的着色器）。  
> **Transparent**：用于半透明着色器（透明着色器、粒子着色器、字体着色器、地形额外通道的着色器）。  
> **TransparentCutout**：蒙皮透明着色器（Transparent Cutout，两个通道的植被着色器）。  
> **Background**：Skybox shaders. 天空盒着色器。  
> **Overlay**：GUITexture, Halo, Flare shaders. 光晕着色器、闪光着色器。  
> **TreeOpaque**：terrain engine tree bark. 地形引擎中的树皮。  
> **TreeTransparentCutout**：terrain engine tree leaves. 地形引擎中的树叶。  
> **TreeBillboard**：terrain engine billboarded trees. 地形引擎中的广告牌树。  
> **Grass**：terrain engine grass. 地形引擎中的草。  
> **GrassBillboard**：terrain engine billboarded grass. 地形引擎何中的广告牌草。

**DisableBatching tag:**

> 很多着色器中，如果使用的Batching技术，物体的模型空间中的位置信息都没了。如果要设计的模型空间坐标系的操作的Shader，就得禁用 Batching，保存模型空间的信息。 我们可以使用 "DisableBatching " = “True”，默认都是“False”

**ForceNoShadowCasting tag:**

> 如果要替换一个半透明的物体的Shader，如果想要这个物体不需要产生阴影，就用这个。半透明物体渲染如果不需要阴影就加上标签。

**IgnoreProjector tag:**

> Unity有种Projector投影效果，如果加上这个Tag，那么就不会受到投影影响。（贴花，Projector阴影效果）

**CanUseSpriteAtlas tag:**

> 如果着色器用于精灵，则将CanUseSpriteAtlas标记设置为“ False”，并且在将它们打包到地图集中时不起作用（请参见Sprite Packer）。

**PreviewType tag:**

> PreviewType 正常我们都是用一个球体预览材质，设置标签”Plane“ 或者 “Skybox”

**Pass Tag:**

> Pass Tag 一般控制灯光相关的信息，这些跟SubShader Tag一样，只能在Pass块里面起效果。

**LightMode tag:**

> 渲染路径标签， 一般现在渲染分为了 三类，顶点渲染路径，向前的渲染，对于的延迟渲染路径。
> **Always**：始终渲染，没有照明  
> **ForwardBase**：在前向渲染中，应用了环境光、主方向光、顶点/SH光和光照贴图（只受到环境光，主要（强度最大那个）的方向光，球协光照和lightMap影响）  
> **ForwardAdd**：用于正向渲染，应用每个像素的附加光，每个光通过一次。（如果灯光类型是 NO-IMPORT 或者其他类型光源 就会用到这个）  
> **Deferred**：延迟渲染的，渲染到Gbuffer  
> **ShadowCaster**：渲染物体深度到阴影贴图或深度纹理。（生成阴影要用深度图Shader）  
> **MotionVectors**： 渲染物体深度到阴影贴图或深度纹理。用于计算每个物体的运动矢量。（计算物件移动向量）  
> **PrepassBase**：用于传统的延迟照明，渲染正常和高光指数。  
> **PrepassFinal**：用于传统的延迟照明，渲染最终颜色的纹理，照明和发射。  
> **Vertex**：当对象没有光照映射时，在传统顶点光照渲染中使用；所有顶点光照都被应用。  
> **VertexLMRGBM**：当对象被光照映射时，在传统顶点光照渲染中使用；在lightMap编码为RGBM (PC和控制台)的平台上。  
> **VertexLM**：当对象被光照映射时，在传统顶点光照渲染中使用；在lightMap是double-LDR编码的平台(移动平台)上。

**PassFlags tag:**

> 通过Pass可以指示更改渲染管道将数据传递给它的方式的标志。 这是通过使用PassFlags标记完成的，该标记的值是用空格分隔标志名称。 当前支持的标志是：
> **OnlyDirectional**： 在ForwardBase传递类型中使用时，使用此标志可以将仅主要定向光和环境/光照探针数据传递到着色器中。 这意味着不重要的光的数据不会传递到顶点光或球谐函数着色器变量中。 有关详细信息，请参见正向渲染。

**RequireOptions tag:**

> 通过Pass可以表明仅应在满足某些外部条件时才进行渲染。 这是通过使用RequireOptions标记完成的，该标记的值是一串用空格分隔的选项。 当前，Unity支持的选项包括：
> **SoftVegetation**：仅当Quality Settings中设置SoftVegetation处于启用状态时，才渲染此通道。
