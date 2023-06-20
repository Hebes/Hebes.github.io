# Shader-渲染设置

## Unity Shader 渲染设置

```csharp
Shader "Example" 
{ 
    Properties{ }
    SubShader
    { 
        Tags { }

        // 渲染设置
        // 裁剪
        Cull off
        //深度测试 
        ZTest Always
        //深度写入
        Zwrite of
        // 混合
        Blend SrcFactor DstFactor
        // 不同情况下使用不同的LOD达到性能提升
        Lod 100 
        Pass
        {
            Tags { }
            // 渲染设置
        }
    }
    Fallback Off
}
```

**Cull:**

> **Cull Back（默认）**：把图片的背面剪裁，相当于背面不进行渲染  
> **Cull Front**：把图片的正面剪裁，相当于正面不进行渲染  
> **Cull Off**：关闭剔除，正反面都渲染

**ZWrite:**

> **ZWrite On（默认）**： 深度写入开启  
> **ZWrite Off**：深度写入关闭  
> 深度写入开启后决定片元的深度值是否写入深度缓冲。不建议关闭深度写入，除非有特殊需求，关闭深度写入会有很多意想不到的现象产生。

**ZTest:**

> **ZTest LEqual（默认）**：深度小于等于当前缓存则通过  
> **ZTest GEqual**：深度大于等于当前缓存则通过  
> **ZTest Equal**：深度等于当前缓存则通过  
> **ZTest NotEqual**：深度不等于当前缓存则通过  
> **ZTest Less**：深度小于当前缓存则通过  
> **ZTest Greater**：深度大于当前缓存则通过  
> **ZTest Always**：不论如何都通过  
> **ZTest Off**：效果跟ZTest Always一样  
> 这里的缓存可以理解成，相机视角此时所保存的某一点的深度值，离相机越近，深度值越大。

**Blend:**

> ShaderLab提供了相应的Blend混合命令，用于指定后渲染产生的颜色如何与颜色缓存中的颜色进行混合。混合命令由Blend关键字，操作和因子组成，操作默认是加操作
> **Blend Off**：关闭混合  
> **Blend SrcFactor DstFactor**：基本的配置并启动混合操作。对产生的颜色乘以SrcFactor。对已存在于屏幕的颜色乘以DstFactor，并且两者将被叠加在一起。  
> **Blend SrcFactor DstFactor,SrcFactorA DstFactorA**：同上，但是使用不同的要素来混合alpha通道，也就是有了4个操作对象
> 以下所有属性都可以作为SrcFactor或DstFactor。其中Source指的是被计算过的颜色，Destination是已经在屏幕上的颜色  
> **One**：值为1，使用此因子来让帧缓冲区源颜色或是目标颜色完全的通过  
> **Zero**：值为0，使用此因子来删除帧缓冲区源颜色或目标颜色的值。  
> **SrcColor**：使用此因子为将当前值乘以帧缓冲区源颜色的值  
> **SrcAlpha**：使用此因子为将当前值乘以帧缓冲区源颜色的Alpha的值  
> **DstColor**：使用此因子为将当前值乘以帧缓冲区目标颜色的值。  
> **DstAlpha**：使用此因子为将当前值乘以帧缓冲区目标颜色Alpha分量的值  
> **OneMinusSrcColor**：使用此因子为将当前值乘以(1-帧缓冲区源颜色值)  
> **OneMinusSrcAlpha**：使用此因子为将当前值乘以(1-帧缓冲区源颜色Alpha分量的值)  
> **OneMinusDstColor**：使用此因子为将当前值乘以(1-目标颜色值)  
> **OneMinusDstAlpha**：使用此因子为将当前值乘以(1-目标颜色Alpha分量的值)

```csharp
Shader "Example" 
{ 
    Properties{ }
    SubShader
    { 
        Tags { }
        
        //正常（Normal）
        Blend SrcAlpha OneMinusSrcAlpha
        //柔和相加（Soft Addtive）
        Blend OneMinusDstAlpha One
        //正片叠底（Multiply），即相乘
        Blend DstColor Zero
        //两倍相乘（2x Multiply）
        Blend DstColor SrcColor
        //滤色（Screen）
        Blend OneMinusDstColor One
        //线性减淡（Linear Dodge）
        Blend One One

        Pass { }
    }
    Fallback Off
}
```

**BlendOp**  
是混合操作，下面是一些常用的混合操作：

> **BlendOp Add（默认）**：将源颜色和目的颜色相加  
> **BlendOp Sub**：用源颜色减去目的颜色  
> **BlendOp RevSub**：用目的颜色减去源颜色  
> **BlendOp Min**：取源颜色和目的颜色中的较小值，是逐分量比较  
> **BlendOp Max**：取源颜色和目的颜色中的较大值，是逐分量比较  
> **BlendOp LogicalClear**：全部赋值为0，就是透明  
> **BlendOp LogicalSet**：全部赋值为1，就是黑色  
> **BlendOp LogicalCopy**：复制原颜色的值，相当于前面的Blend One Zero  
> **BlendOp LogicalCopyInverted**：把源颜色的值按位取反后渲染  
> **BlendOp LogicalNoop**：渲染目的颜色中的像素，相当于前面的Blend Zero One  
> **BlendOp LogicalInvert**：把目的颜色中的值按位取反后渲染  
> **BlendOp LogicalAnd**：源颜色 & 目的颜色  
> **BlendOp LogicalNand**：! (源颜色 & 目的颜色)  
> **BlendOp LogicalOr**：源颜色 | 目的颜色  
> **BlendOp LogicalNor**：! (源颜色 | 目的颜色)  
> **BlendOp LogicalXor**：源颜色 ^ 目的颜色  
> **BlendOp LogicalEquiv**：! ( 源颜色 ^ 目的颜色)  
> **BlendOp LogicalAndReverse**：源颜色 & !目的颜色  
> **BlendOp LogicalAndInverted**：!源颜色 & 目的颜色  
> **BlendOp LogicalOrReverse**：源颜色 | !目的颜色  
> **BlendOp LogicalOrInverted**：!源颜色 | 目的颜色

```csharp
Shader "Example" 
{ 
    Properties{ }
    SubShader
    { 
        Tags { }
        
        //变暗（Darken）
        BlendOp Min
        Blend One One
        //变亮（Lighten）
        BlendOp Max
        Blend One One

        Pass { }
    }
    Fallback Off
}
```
