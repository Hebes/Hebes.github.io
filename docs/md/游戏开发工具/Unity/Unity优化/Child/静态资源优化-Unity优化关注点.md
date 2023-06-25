# 静态资源优化-Unity优化关注点

## Unity优化关注点

### cpu占用：

+ 一般考虑两种档位-超过33ms的帧和超过50ms的帧，来进行量化
+ 这种就需要找到cpu占用比较多的帧，然后分析消耗cpu的具体函数，再针对业务需求进行优化

### 内存占用：

+ 检查内存的走势，是否存在异常，比如持续上升或者快速上升，对于低端机，建议中体内存在150M左右
+ 对于堆内存（Reserved Mono Memory）检查峰值（因为堆内存不会释放，所以峰值既是堆内存的占用量），然后考虑峰值的触发条件，进行优化
+ 堆内存需要排查是否存在内存泄漏的问题，检查缓冲池是否size过大，然后具体分析分配较大堆内存的函数进一步优化

### 资源分布：

+ 主要是三个部分：资源加载，Instantiate实例化，资源释放（Resource.UnloadUnusedAssets）
+ 资源加载主要是检查一些比较复杂的UI打开时、跨场景时的情况，优化主要是
+ 1）简化资源：主要是对资源进行压缩等，资源的大头是Texture和Mesh，我们需要学习了解一下MeshBaker、SimpleLOD等处理mesh的工具、纹理贴图的压缩规格以及当前的主流标准等
+ 2）部分UI资源常驻：可能需要对不同的机型使用不同的方案，考虑如何划分标准（资源标准、机型标准等）
+ 实例化非常消耗CPU
+ 检查Instantiate的调用情况，看看能否通过缓冲池来复用GameObject来保证流畅性
+ 资源释放（GC和Resources.UnloadUnusedAssets），cpu耗时很长，集中在200ms~300ms，一般会再跨场景时调用，应该谨慎使用，每一次调用都很卡，所以需要注意api调用的情况和频率
+ shader的解析也存在很高的开销，一般是把所有shader打包到一个AB文件中，同时加载初始化，shader本身所占内存很小，可以常驻内存中。
+ log输出:输出log会占用cpu并由于字符串的处理会产生内存，因此考虑仅控制必要的输出，我们的log有三档，会在环境配置时失效不同的档位
+ drawcall尽量控制在60左右吧，可以遍历场景和ui检查drawcall，然后进行分析优化
+ 同屏三角形面数正常范围峰值在40000左右，平时25000左右，具体优化也是使用工具简化mesh的面数

### UI性能：

**相关开销函数**

+ EventSystem.Update：该函数有较高CPU通常是调用了其他的较为耗时的函数，因此需要通过添加Profile.BeginSample/EndSample来对触摸所触发的逻辑进行进一步的检测
+ Canvas.SendWillRenderCavases：该api为UI元素自身发生变化时（比如Enable或者缩放，移动不算），其中CanvasUpdateRegistry.GraphicRebuild为构建渲染网格和材质信息，CanvasUpdateRegistry.LayoutRebuild为构建布局信息
+ Canvas.BuildBatch：UI同层的元素会合并Drawcall（Mesh和材质），因此Canvas.SendWillRenderCanvases和UI元素的移动都会触发该api的调用
+ PutGeometryJobFence && WaitingForJob：该API为Canvas.BuildBatch在子线程中操作过慢，导致主线程等待锁产生的调用
+ 检查从游戏登录到游戏内容，切换一些复杂UI（打开和关闭）等一系列操作检查上述API的调用情况（cpu的占用）

**优化建议：**

1. **切换界面时，SetActive比Instantiate更高效，对于UI元素，OnEnable和OnDisable都会进行较多的操作，对于频率较高的UI界面更好的做法是：通过改变UI的位置来实现UI的隐藏和显示（位置移动不消耗cpu）**
2. **同时同地出现的Sprite应该尽可能打在一个Atlas中，以减少drawcall**
3. **根据UGUI的机制，移除渲染范围的UI元素并不会减少Drawcall，建议通过修改Layer来“隐藏”面板，避免Active/Deactive带来的卡顿**
4. **Mask组件会造成2个drawcall，因此建议少用，或者尝试改为RectMask2D组件进行替换**

### 物理系统：

+ 物理系统的两个指标：Rigidbody数量和Collider数量
+ 正常应该控制在30-50
+ cpu占用主要注意两个函数：Physics.Simulate和Physics.Processing

### 游戏发布指标：

+ OptimizeMeshData：该选项位于PlayerSetting的OtherSetting中，勾选后，引擎会在发布时遍历所有的网格数据，将多余数据进行去除，从而降低数据量大小。
+ “多余数据”：指Mesh数据中心包含了渲染时Shader中所不需要的数据，比如（Mesh数据含有position、uv、normal、color、tangent等顶点数据，但其渲染所用shader只需要position和uv、normal，因此color和tangent则为多余数据
+ ShaderLab占用内存较高时，建议尝试去掉StandardShader，该shader比较复杂，会造成较大的ShaderLab占用

### 资源使用：

+ 低端机纹理内存占用应控制在50MB以下，尽可能使用移动设备硬件支持的纹理格式（Android：ETC1，iOS：PVRTC）
+ Mipmap，将不必要的UI的Mipmap关闭，（Mipmap会生成几种较小尺寸的图片，因此内存会增大几倍）
+ 检查资源引用中，是否存在同一个资源，加载了多次AssetBundle的问题
+ Mesh资源使用，建议控制在20MB以下，尝试通过插件优化减少Mesh网格数，去掉多余的Mesh数据
+ shader的使用，建议剥离shader的引用关系，所有shader统一打包
+ AnimationClip资源，检查是否存在冗余情况，建议控制在10MB以下（冗余是指，同一个资源，加载多次，占用多份内存，正常情况下同一个资源只需要一份，然后需要该资源的对其进行引用）
+ Material资源，Material的每次修改都会生成一份新的Material，因此使用时，需要注意。
+ RenderTexture，
+ Font，字体资源占用内偶才能一般还挺多，可以考虑自己控制字库的文字个数等

### 代码CPU占用：

1. Camera.Render
 1. 检查Graphics.PresetAndSync，该API反映的是GPU上的渲染压力，渲染压力越大，API耗时越长，建议严格控制和简化模型降低GPU开销，控制多层纹理渲染，或者将纹理融合为一张较大纹理进行渲染
 2. 蒙皮网格渲染，控制在1ms左右，注意shaer不要使用不必要的AlphaTest等、Mesh进行简化
 3. 粒子系统渲染，建议对于不同机型适配不同的粒子效果
 4. 半透明模型的渲染，对半透明模型进一步简化，
2. BehaviourUpdate
3. Animator.Update & MeshSkinning.Update
 1. 对于简单的模型，考虑将其蒙皮网格（SkinnedMesh）转化为普通的Mesh，该方法核心在于将蒙皮模型按照一定的帧率将其动画文件抽取成序列帧网格数据，并将这些序列帧网格合并成一个较大的Mesh数据，这样，当角色需要占时某个动作时，只需根据所要播放的动画及播放时间，即可在网格中获取相对应的网格数据进行渲染。该方法极大降低MeshSkinning.Render的CPU耗时，同时还可以直接降低SkinnedMesh.Update和Animator.Update的CPU占用。但会使得内存占用上升，不建议对Mesh较大的模型使用。
4. 其他上边提到的UI相关API

### 代码堆内存：

根据测试结果，具体按照上述几个核心api进行分析
