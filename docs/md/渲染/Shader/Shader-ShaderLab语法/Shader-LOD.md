# Shader-LOD

**LOD:**

> Shader Level of Detail (LOD)，翻译过来是什么“着色器的细节层次效果”。Shader LOD其实就是根据设备性能的不同编译不同版本的Shader。这个是另外一种控制细节级别的技术，在一个Shader当中，可以给不同的SubShader指定不同的LOD属性。
> 根据这个特性，我们就可以在一个Shader里写一出组SubShader，分别设置不同的LOD，LOD越大的对应性能越好，越低的性能越差。然后我们就可以用设置LOD的方法来控制游戏画面的渲染质量。

```csharp
Shader "Example" 
{ 
    Properties{ }
    SubShader
    {
        Tags{ }
        LOD 100
    }
    SubShader
    {
        Tags{ }
        LOD 200
    }
    Fallback Off
}
```

> 在Edit->Project Setting->QualitySettings中的Maximum LOD Level可以设置最大LOD等级，Shader的LOD值是小于所设定的LOD值，才会被编译使用。Maximum LOD Level的等级可以设置7个级别，例如设置为1，则表示把最大LOD值设置为100，等级2，则最大LOD值为200，以此类推，若设置为0，则表示不进行LOD判断，任何LOD值的Shader都会被使用。
