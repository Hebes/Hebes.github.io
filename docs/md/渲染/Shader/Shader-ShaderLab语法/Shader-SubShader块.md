# Shader-SubShader块

## Unity Shader SubShader块

> 一个Shader可以有多个子着色器（SubShader），这些子着色器互不干扰，且只有一个会运行，编写多个子着色器的目的是为了解决平台兼容性问题，Unity会自己选择兼容终端平台的Shader运行。

```csharp
Shader "Example" 
{ 
    Properties{ }
    // Unity加载Shader扫描所有SubShader块，使用第一个能在目标平台使用的SubShader块，当所有SubShader都不支持目标平台时会执行Fallback语义块
    SubShader{ }
    Fallback Off // 当上面Shader运行不了的时候会使用下面的Fallback Shader渲染
}
```

### SurfaceShader

## Fallback

> 回滚，相当于备胎。
