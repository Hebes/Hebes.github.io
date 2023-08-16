# Shder-pragma指令

**[着色器编译：pragma 指令](<https://docs.unity.cn/cn/2021.1/Manual/SL-PragmaDirectives.html>)**

+ [Unity User Manual 2021.1](https://docs.unity.cn/cn/2021.1/Manual/UnityManual.html)
+ [图形](https://docs.unity.cn/cn/2021.1/Manual/Graphics.html)
+ [着色器](https://docs.unity.cn/cn/2021.1/Manual/Shaders.html)
+ [编写着色器](https://docs.unity.cn/cn/2021.1/Manual/shader-writing.html)
+ [Unity 中的 HLSL](https://docs.unity.cn/cn/2021.1/Manual/SL-ShaderPrograms.html)
+ 着色器编译：pragma 指令

## 着色器编译：pragma 指令

在 HLSL 中，您可以使用预处理器指令来告诉编译器如何编译着色器程序。Pragma 指令是一种预处理器指令。本页面包含有关可在着色器源代码中使用的 pragma 指令的信息。

有关 HLSL 中预处理器指令的一般信息，请参阅 Microsoft 文档：[预处理器指令 (HLSL)](https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-appendix-preprocessor) 。

## 使用 pragma 指令

将 pragma 指令放在着色器源文件中的 HLSL 代码块中。

默认情况下，Unity 不支持 include 文件中的 pragma 指令。如果在编辑器设置中启用缓存预处理器，则可以使用 `#include_with_pragmas` 预处理器指令。该指令允许您将 pragma 指令放在 include 文件中。这对于启用或禁用多个文件的着色器调试符号特别有用。

## 支持的 pragma 指令

### 着色器阶段

使用这些 pragma 指令告诉编译器将着色器代码的哪些部分编译为不同的着色器阶段。

HLSL 代码片段必须至少包含一个顶点程序和一个片元程序，因此需要使用 `#pragma vertex` 和 `#pragma fragment` 指令。

 
| **语句** | **功能** |
| --- | --- |
| `#pragma vertex name` | 作为顶点着色器来编译函数 `name`。 |
| `#pragma fragment name` | 作为片元着色器来编译函数 `name`。 |
| `#pragma geometry name` | 作为 DX10 几何着色器来编译函数 `name`。如下表中所述，此选项会自动开启 `#pragma target 4.0`。 |
| `#pragma hull name` | 作为 DX11 外壳着色器来编译函数 `name`。如下表中所述，此选项会自动开启 `#pragma target 5.0`。 |
| `#pragma domain name` | 作为 DX11 域着色器来编译函数 `name`。如下表中所述，此选项会自动开启 `#pragma target 5.0`。 |

### 着色器变体和关键字

使用这些 pragma 指令告诉着色器编译器如何处理着色器变体和关键字。

 
| **语句** | **功能** |
| --- | --- |
| `#pragma multi_compile ...` | 为给定的关键字创建着色器变体。`multi_compile` 着色器的未使用变体会包含在游戏构建中。有关更多信息，请参阅[着色器关键字和变体](https://docs.unity.cn/cn/2021.1/Manual/SL-MultipleProgramVariants.html)。 |
| `#pragma multi_compile_local ...` | 此语句类似于 `multi_compile`，但是枚举的关键字为本地关键字。有关更多信息，请参阅[着色器关键字和变体](https://docs.unity.cn/cn/2021.1/Manual/SL-MultipleProgramVariants.html)。 |
| `#pragma shader_feature ...` | 为给定的关键字创建着色器变体。`shader_feature` 着色器的未使用变体不会包含在游戏构建中。有关更多信息，请参阅[着色器关键字和变体](https://docs.unity.cn/cn/2021.1/Manual/SL-MultipleProgramVariants.html)。 |
| `#pragma shader_feature_local ...` | 此语句类似于 `shader_feature`，但是枚举的关键字为本地关键字。有关更多信息，请参阅[着色器关键字和变体](https://docs.unity.cn/cn/2021.1/Manual/SL-MultipleProgramVariants.html)。 |

### 着色器模型和 GPU 功能

使用这些 pragma 指令告诉编译器您的着色器针对特定的着色器模型，或需要特定的 GPU 功能。

 
| **语句** | **功能** |
| --- | --- |
| `#pragma target name` | 为哪个着色器模型编译此着色器程序。有关更多信息，请参阅[着色器编译：针对着色器模型和 GPU 功能](https://docs.unity.cn/cn/2021.1/Manual/SL-ShaderCompileTargets.html)。 |
| `#pragma require feature ...` | 着色器程序需要哪些 GPU 功能。有关更多信息，请参阅[着色器编译：针对着色器模型和 GPU 功能](https://docs.unity.cn/cn/2021.1/Manual/SL-ShaderCompileTargets.html)。 |

### 图形 API

使用这些 pragma 指令告诉编译器为特定的图形 API 编译着色器。

| **语句** | **功能** |
| --- | --- |
| `#pragma only_renderers space separated names` | 仅为给定的图形 API 编译此着色器程序。有关更多信息，请参阅[着色器编译：针对图形 API](https://docs.unity.cn/cn/2021.1/Manual/SL-ShaderCompilationAPIs.html)文档。 |
| `#pragma exclude_renderers space separated names` | 不为给定的图形 API 编译此着色器程序。有关更多信息，请参阅[着色器编译：针对图形 API](https://docs.unity.cn/cn/2021.1/Manual/SL-ShaderCompilationAPIs.html)文档。 |

### 其他 pragma 指令

| **语句** | **功能** |
| --- | --- |
| `#pragma enable_d3d11_debug_symbols` | 生成着色器调试符号和/或禁用优化。使用此指令在外部工具中调试着色器代码。
对于 Vulkan、DirectX 11 和 12 以及支持的游戏主机平台，Unity 会生成调试符号并禁用优化。

对于 Metal 和 OpenGL，默认情况下您已经可以调试着色器。当您使用此 pragma 指令时，Unity 会禁用优化。

**警告：**使用此指令会导致文件大小增加并降低着色器性能。当您完成对着色器的调试并准备好对应用程序进行最终构建时，请从着色器源代码中删除此行并重新编译着色器。

 |
| `#pragma hardware_tier_variants renderer name` | 针对每个可运行所选渲染器的硬件层，生成每个由系统编译的着色器的[多个着色器硬件变体](https://docs.unity.cn/cn/2021.1/Manual/SL-MultipleProgramVariants.html)。  
仅在[内置渲染管线](https://docs.unity.cn/cn/2021.1/Manual/built-in-render-pipeline.html)中支持此语句。 |
| `#pragma hlslcc_bytecode_disassembly` | 将反汇编的 HLSLcc 字节码嵌入到转换的着色器中。 |
| `#pragma disable_fastmath` | 启用涉及 NaN 处理的精确 IEEE 754 规则。当前这仅影响 Metal 平台。 |
| `#pragma glsl_es2` | 在 GLSL 着色器中进行设置以生成 GLSL ES 1.0(OpenGL ES 2.0)，即使着色器目标为 OpenGL ES 3 也是如此。 |
| `#pragma editor_sync_compilation` | 强制进行同步编译。这仅影响 Unity 编辑器。有关更多信息，请参阅[异步着色器编译](https://docs.unity.cn/cn/2021.1/Manual/AsynchronousShaderCompilation.html)。 |
| `#pragma enable_cbuffer` | 使用 HLSLSupport 的 `CBUFFER_START(name)` 和 `CBUFFER_END` 宏时，即使当前平台不支持常量缓冲区，也要发出 `cbuffer(name)`。 |

## 未使用的 pragma 指令

以下编译指令不执行任何操作，因此可以安全删除：

+ `#pragma glsl`
+ `#pragma glsl_no_auto_normalization`
+ `#pragma profileoption`
+ `#pragma fragmentoption`
