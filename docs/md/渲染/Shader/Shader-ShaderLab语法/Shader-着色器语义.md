# Shader-着色器语义

**[着色器语义](<https://docs.unity3d.com/cn/current/Manual/SL-ShaderSemantics.html>)**

**[语义](<https://learn.microsoft.com/zh-cn/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics?redirectedfrom=MSDN>)**

**[SV_POSITION与SV_Target](<https://www.jianshu.com/p/ea63577a08c0>)**

在unity shader的学习和工作中经常看到 SV_POSITION和SV_Target，之前也看了一些文章，一直模模糊糊的概念，现在差不多清楚了，记录一下。
        SV_POSITION和SV_Target都是语义绑定(semantics binding) ，可以理解为关键字吧，图形渲染是按照固定的流程一步一步走的，所以也叫管线，应该是这个意思吧，在这个过程中，前面流程处理完的数据是需要传到下一个流程继续处理的，因为gpu和cpu的架构不同，这又是个更大的知识点了，有兴趣的同学可以自己去了解一下，这里不多说了，所以呢gpu并不能像cpu一样有内存堆栈可以用来存取变量和值，只有通过语义绑定(semantics binding) 将处理好的值存到一个物理位置，方便下一个流程去取，一般的可编程管线主要处理vertext(顶点)函数和fragment(片段)函数，当然也有叫片元函数的，一个意思吧。
        SV_前缀的变量代表system value的意思，在DX10+的语义绑定中被使用代表特殊的意义，SV_POSITION在用法上和POSITION是一样的，区别是 SV_POSTION一旦被作为vertex函数的输出语义，那么这个最终的顶点位置就被固定了，不得改变。DX10+推荐使用SV_POSITION作为vertex函数的输出和fragment函数的输入，而vertex函数的输入还是使用POSITION。不过DX10以后的代码依旧兼容POSITION作为全程表达，估计编译器会自动判断并替换的吧。
        SV_Target是DX10+用于fragment函数着色器颜色输出的语义。DX9使用COLOR作为fragment函数输出语义，但是也有一些着色器语言使用COLOR来表示网格数据和顶点输出语义，效果和功能是一样的，没有什么区别，同时使用COLOR的话DX10+也会兼容。