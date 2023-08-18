# Statistics窗口

**[Unity 5 Statistics 窗口](<https://blog.csdn.net/ldy597321444/article/details/77982774>)**

**[Unity 5 Stats窗口](<https://www.cnblogs.com/zhaoqingqing/p/6288154.html>)**

Statistics 窗口包含以下信息：

|  |  |
| --- | --- |
| **Time per frame and FPS** | 处理和渲染一个游戏帧所花费的时间（及其倒数，即每秒帧数）。请注意，此数字仅包括进行帧更新和渲染 Game 视图所用的时间；不包括在 Editor 中绘制 Scene 视图、检视面板处理和其他仅限于 Editor 的处理所花费的时间。 |
| **Batches** | “批处理 (Batching)”可让引擎尝试将多个对象的渲染组合到一个内存块中以便减少由于资源切换而导致的 CPU 开销。 |
| **Saved by batching** | 合并的批次数。为确保良好的批处理，应尽可能在不同对象之间共享材质。更改渲染状态会将批次分成具有相同状态的组。 |
| **Tris** 和 **Verts** | 绘制的三角形和顶点的数量。在[针对低端硬件进行优化](https://docs.unity.cn/cn/2019.4/Manual/OptimizingGraphicsPerformance.html)时，这一点非常重要 |
| **Screen** | 屏幕大小以及抗锯齿级别和内存使用情况。 |
| **SetPass** | 渲染 pass 的数量。每个 pass 都需要 Unity 运行时绑定一个新的着色器，这可能会带来 CPU 开销。 |
| **Visible Skinned Meshes** | 渲染的蒙皮网格的数量。 |
| **Animations** | 播放的动画的数量。 |

另请参阅 [Profiler 窗口的 Rendering 部分](https://docs.unity.cn/cn/2019.4/Manual/ProfilerRendering.html)，了解这些统计信息更详细和完整的版本。

## FPS Fragments Per Second

自然是Unity每秒渲染的帧数，是一个关键的性能指标，其能维持在一个正常的范围决定了整个项目的流畅度，指标严重低于目标范围的情况被称为掉帧，会带来严重的卡顿感。

## CPU Main

是cpu处理一帧所消耗的总时间，单位一般为毫秒，这个时间不仅仅包含项目中更新每一帧所需要的各种操作耗时，也包含对应用外editor下的其他窗口和工具更新的耗时。

## CPU Render 或 Render thread

渲染每帧所需要的时间，为应用或者游戏中所需要的时间，包含在main的时间之内。

GPU渲染线程处理图像所花费的时间，具体数值由GPU性能来决定。

## Batches

为Unity中处理每帧中draw call batches（绘制图元的指令批处理）的总数，包括动态批次、静态批次以及GPUInstancing的批次总和。

即Batched Draw Calls,是Unity内置的Draw Call Batching技术

首先解释下什么叫做“Draw call”，CPU每次通知GPU发出一个glDrawElements（OpenGl中的图元渲染函数)或者 DrawIndexedPrimitive（DirectX中的顶点绘制方法）的过程称为一次Draw call,一般来说，引擎每对一个物体进行一次DrawCall，就会产生一个Batch,这个Batch里包含着该物体所有的网格和顶点数据，当渲染另一个相同的物体时，引擎会直接调用Batch里的信息，将相关顶点数据直接送到GPU,从而让渲染过程更加高效，即Batching技术是将所有材质相近的物体进行合并渲染。

对于含有多个不同Shader和Material的物体，渲染的过程比较耗时，因为会产生多个Batches。每次对物体的材质或者贴图进行修改，都会影响Batches里数据集的构成。因此，如果场景中有大量材质不同的物体，会很明显的影响到GPU的渲染效率。这里说几点关于Batches优化相关的方案

- 虽然Unity引擎自带Draw Call Batching技术，我们也可以通过手动的方式合并材质接近的物体；
- 尽量不要修改Batches里物体的Scale，因为这样会生成新的Batch。
- 为了提升GPU的渲染效率，应当尽可能的在一个物体上使用较少的材质，减少Batches过多的开销；
- 对于场景中不会运动的物体，考虑设置Static属性,Static声明的物体会自动进行内部批处理优化。

## Saved by batching

Unity将draw call 合并后所节省的drawcall数量，每个Batche group的渲染状态都是相同的，所以有相同渲染状态的draw call可以被合并到一个批次之中。

## Tris

Triangles的缩写，每帧中Unity要处理的三角形总数，优化中的重点数据。

摄像机视野(field of view)内渲染的顶点总数

## Verts

Vertices的缩写vertex的复数，每帧中Unity要处理的顶点数据总数，优化中的重点数据。

摄像机视野(field of view)内渲染的的三角面总数量。

## Screen

屏幕的显示分辨率，后面是跟着的是其所占用的存储空间。

## SetPass Calls

每帧中Unity为渲染GameObject而切换Shader Pass 的次数，一个Shder可能包含多个Pass，例如卡通着色中一个Pass用来渲染颜色、一个用来描边另一个用来画顶点阴影，每一个Pass都需要Unity去重新绑定一个新的Shader。可以看出Batches是要大于等于SetPass Calls的，而且最耗费性能的也是SetPass Calls操作，所以更是优化的核心数据指标。

## Shadow Casters

每帧投射阴影的GameObject的总数量。

## Visible skinned meshes

每帧中SkinnedMeshRenderer组件出现的总数量。

## Animation components playing

每帧中正在播放的Animation组件。

## Animator components playing

每帧中正在播放的Animator组件。
