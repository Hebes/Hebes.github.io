# Shader-LOD

[https://docs.unity.cn/cn/2020.3/Manual/shader-shaderlab-commands.html](https://docs.unity.cn/cn/2020.3/Manual/shader-shaderlab-commands.html%E2%80%B8) ShaderLab：命令

```csharp
Shader "Example" 
{ 
    Properties{ }
    SubShader
    {
        Tags{ }

	//在Edit->Project Setting->QualitySettings中的Maximum LOD Level可以设置最大LOD等级，Shader的LOD值是小于所设定的LOD值，才会被编译使用。
	//Maximum LOD Level的等级可以设置7个级别，例如设置为1，则表示把最大LOD值设置为100，等级2，则最大LOD值为200，以此类推
	//若设置为0，则表示不进行LOD判断，任何LOD值的Shader都会被使用。
	//https://docs.unity.cn/cn/2021.1/Manual/SL-ShaderLOD.html
        LOD 100

	//设置多边形的剔除方式，有背面剔除、正面剔除、不剔除
	//所谓的剔除，就是不染，背面剔除就是背面不渲染，正面剔除就是正面不染，不剔除就是都渲染
	//Cull Back背面剔除
	//Cull Front正面剔除
	//cul1off不剔除
	//不设置的话，默认为背面剔除
	//一般情况下，我们需要两面渲染时，会设置为不剔除
	Cull Off

	//深度缓冲是一个与屏幕像素对应的缓冲区，用于存储每个像素的深度值（距离相机的距离）。
	//在渲染场景之前，深度缓冲被初始化为最大深度值，表示所有像素都在相机之外。
	//最后留在深度缓冲中的信息会被渲染
	//zwrite On写入深度缓冲
	//zWrite Off不写入深度缓冲
	//不设置的话，默认为写入
	//一般情况下，我们在做透明等特殊效果时，会设置为不写入
	ZWrite Off

	//设置深度测试的对比方式
	//深度测试的主要目的是确保在渲染时，像素按照正确的深度（距离相机的距离）顺序进行绘制）
	//从而创建正确的遮挡关系和透视效果//在渲染场景之前，深度缓冲被初始化为最大深度值，表示所有像素都在相机之外。
	//在渲染过程中，对于每个像素，深度测试会将当前像素的深度值与深度缓冲中对应位置的值进行比较。
	//一般情况下
	//1.如果当前像素的深度值小于深度缓冲中的值，说明当前像素在其他物体之前，它会被绘制，并更新深度缓冲。
	//2.如果当前像素的深度值大于等于深度缓冲中的值，说明当前像素在其他物体之后，它会被丢弃，不会被绘制，并保持深度缓冲不变。

	//ZTest Less小于当前深度缓冲中的值，就通过测试，写入到深度缓冲中
	//ZTest Greater大于当前深度缓冲中的值，就通过测试，写入到深度缓冲中
	//ZTest LEqual小于等于当前深度缓冲中的值，就通过测试，写入到深度缓冲中
	//ZTest GEqual大于等于当前深度缓冲中的值，就通过测试，写入到深度缓冲中
	//ZTest Equal等于当前深度缓冲中的值，就通过测试，写入到深度缓冲中
	//ZTest NotEqual不等于当前深度缓冲中的值，就通过测试，写入到深度缓冲中
	//ZTest Always始终通过深度测试写入深度缓冲中
	//不设置的话，默认为LEqual小于等于
	//一般情况下，我们只有在实现一些特殊效果时才会区修改深度测试方式，比如透明物体渲染会修改为Less，描边效果会修改为Greater等
	ZTest Less

	//设置渲染图像的混合方式（多种颜色叠加混合，比如透明、半透明效果和遮挡的物体进行颜色混合）
	//Blend One One线性减淡
	//Blend SrcAlpha OneMinusSrcAlpha正常透明混合
	//Blend OneMinusDstColor One滤色
	//Blend Dstcolor Zero正片叠底
	//Blend DstColor SrcColorX光片效果
	//Blend One OneMinusSrcAlpha透明度混合
	//等等
	//不设置的话，默认不会进行混合
	//一般情况下，我们需要多种颜色叠加渲染时，就就需要设置混合方式，具体情况具体处理
	Blend One OneMinusSrcAlpha

	//ColorMask设置颜色通道的写入蒙版，默认蒙版为RGBA
	ColorMask RGBA
    }
    SubShader
    {
        Tags{ }
        LOD 200
    }
    Fallback Off
}
```
