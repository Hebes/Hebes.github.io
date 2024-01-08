# DOTS技术

DOTS全称是Data-Oriented Tech Stack，翻译过来就是多线程式数据导向型技术堆栈（DOTS），它由任务系统（Job System）、实体组件系统（ECS）、Burst Compiler编译器三部分组成。

ECS + JobSystem + BurstCompile = 高性能 + 多线程 +  编译层面优化

## 类似原则

ECS规定，以下行为都不能在Job中处理(如需,请看EntityCommandBufferSystem)：

1. 创建实体（Create Entities）
1. 销毁实体（Destroy Entities）
1. 给实体添加组件（Add Components）
1. 删除实体的组件（Remove Components）

**可以在实体中创建非托管或托管系统:**
要定义Class托管系统，请创建一个继承自SystemBase.更简单，运行在主线程上，不能使用Burst
要定义Struct非托管系统，请创建一个继承自ISystem.可以使用Burst，非常快，但稍复杂

**Entity Random Access**
如果你希望在Job之外访问Entity组件，可以考虑使用
EntityManager.GetComponentData\<T>
EntityManager.SetComponentData\<T>
做随机访问，避免创建ComponentLookup的开销

## 逻辑表图

![1](\..\Image\DOTS\DOTS3.png)
![1](\..\Image\DOTS\DOTS4.png)

## 参考

**[Scripting API(重点)](<https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/>)**

**[Manual(重点)](<https://docs.unity3d.com/Packages/com.unity.entities@1.0/>)**

**[ECS 三 访问Entity数据](<https://blog.csdn.net/qq_37672438/article/details/104598734>)**

**[Metaverse大衍神君(推荐去看看)](<https://www.bilibili.com/video/BV19B4y177hY/?spm_id_from=333.788>)**

**[UnityECS(推荐去看看)](<https://blog.csdn.net/qq_36382054/category_9596750.html>)**

**[DOTS介绍+Unity DOTS-MAN小游戏项目实战(PS:自己需要看的,有碰撞)](<https://blog.csdn.net/qq_51773145/article/details/123358623>)**

**[weixin_40124181的博客](<https://blog.csdn.net/weixin_40124181/category_9197340.html>)**

**[DOTS ECS](<https://blog.csdn.net/qq_42461824/category_10773154.html>)**

**[Unity DOTS：入门简介（ECS，Burst Complier，JobSystem）](<https://zhuanlan.zhihu.com/p/138029194>)**

**[【Unity】DOTS 1.0 初体验 坦克尿尿教程）](<https://zhuanlan.zhihu.com/p/575792897>)**

**[【Unity】Entities 1.0 学习（一）：Aspect](<https://blog.csdn.net/cyf649669121/article/details/127814468>)**

[Unity Entities 1.0来啦！讲师达哥带你入门DOTS框架](<https://www.bilibili.com/video/BV1i14y177Ny>)

## ECS

ECS即实体（Entity），组件（Component），系统（System），其中Entity，Component皆为纯数据向的类，System负责操控他们，这种模式会一定程度上优化我们的代码速度。

- Entities：游戏中的事物，但在ECS中他只作为一个Id
- Components：与Entity相关的数据，但是这些数据应该由Component本身而不是Entity来组织。（这种组织上的差异正是面向对象和面向数据的设计之间的关键差异之一）。
- Systems：Systems是把Components的数据从当前状态转换为下一个状态的逻辑，但System本身应当是无状态的。例如，一个system可能会通过他们的速度乘以从前一帧到这一帧的时间间隔来更新所有的移动中的entities的位置。

## Burst Complier

**概观:**
Burst是一个编译器，它使用LLVM将IL / .NET字节码转换为高度优化的本机代码。它作为Unity包发布，并使用Unity Package Manager集成到Unity中。

## Jobs多线程

可以将主线程逻辑转移到Jobs线程中并行执行

**参考链接:**
**[使用Unity的Job System性能为什么没有明显提升？](<https://www.zhihu.com/question/443162990>)**
**[Unity学习—JobSystem](<https://zhuanlan.zhihu.com/p/148160780>)**

**Job Types:**
[Job Types](<https://docs.unity3d.com/Manual/job-system-jobs.html>)
*IJobParallelForTransform：*并行运行任务。每个并行运行的工作线程都有一个来自转换层次结构的独占转换来进行操作。
`IJob：`在作业线程上运行单个任务。
`IJobFor：`与 相同IJobParallelFor，但允许您安排作业，使其不会并行运行。
`IJobParallelFor：`并行运行任务。每个并行运行的工作线程都有一个独占索引来安全地访问工作线程之间的共享数据。
`IJobBurstSchedulable`
`IJobParallelForBatch`
`IJobParalleForDefer`
`IJobParallelForFilter`

**三种控制流:**

**Run (main thread)**->主线程立即顺序执行
**Schedule (single thread, async)**->单个工作线程或主线程，每个Job顺序执行
**ScheduleParallel (multiple threads, async)**->在多个工作线程.上同时执行,性能最好,但多个工作线程访问一数据时可能会发生冲突

本身名字带有Parallel的Job类型,仅提供Schedule ,不提供Run和ScheduleParallel的调度方式。

**Job 执行时长决定使用哪种 Allocator:**

**Allocator.Temp** 最快的分配方法，适用于一帧或几帧的生命时长，不能将该类型分配的数据传给 Job，在方法 Return 前执行Dispose
**Allocator.TempJob** 分配速度比 Temp 慢比 Persistent 快，4帧的生命时长且线程安全。若四帧内没有调用Dispose，控制台会打印原生代码生成的警告。大部分小任务都使用该类型分配NativeContainer
**Allocator.Persistent** 是对malloc的包装，能够维持尽可能地生命时长，必要的话可以贯穿整个应用的生命周期,性能不足的情况下不应使用

Job 同时拥有NativeContainer的读写权限，但 C# Job System 不允许多个 Job 同时拥有对一个NativeContainer的写权限，因此对不需要写权限的NativeContainer加上[ReadOnly]特性，以减少性能影响

``` C#
[ReadOnly]
public NativeArray<int> input;
```

性能测试

``` C#
//请尝试把size的数量慢慢调高
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class test : MonoBehaviour
{

    public NativeArray<Vector3> data_a;
    public NativeArray<float> result_a;

    public NativeArray<Vector3> data_b;
    public NativeArray<float> result_b;

    public int size;

    private void Awake()
    {
        data_a = new NativeArray<Vector3>(size,Allocator.Persistent);
        result_a = new NativeArray<float>(size, Allocator.Persistent);
        data_b = new NativeArray<Vector3>(size, Allocator.Persistent);
        result_b = new NativeArray<float>(size, Allocator.Persistent);
        CalaDataA();
        CalaDataB();
        CalaDataC();
    }

    public void CalaDataA()
    {
        DateTime beforeDT = DateTime.Now; 
        for (var i = 0; i < size; i++)
        {
            var item = data_a[i];
            result_a[i] = Mathf.Sqrt(item.x * item.x + item.y * item.y + item.z * item.z);
        }
        DateTime afterDT = DateTime.Now;
        TimeSpan ts = afterDT.Subtract(beforeDT);
        print($"单线程计算时间: {ts.TotalMilliseconds}ms");
    }


    private struct SingleJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> data;
        public NativeArray<float> result;
        public void Execute(int i)
        {
            Vector3 item = data[i];
            result[i] = Mathf.Sqrt(item.x * item.x + item.y * item.y + item.z * item.z);
        }
    }



    public void CalaDataB()
    {
        DateTime beforeDT = DateTime.Now;
        SingleJob jobs = new SingleJob
        {
            data = data_b,
            result = result_b
        };
        var handler = jobs.Schedule(size, 64);
        handler.Complete();
        DateTime afterDT = DateTime.Now;
        TimeSpan ts = afterDT.Subtract(beforeDT);
        print($"多线程计算时间: {ts.TotalMilliseconds}ms");
    }

    [BurstCompile]//Burst添加后
    private struct SingleJob1: IJobParallelFor 
    {
        [ReadOnly]
        public NativeArray<Vector3> data;
        public NativeArray<float> result;
        public void Execute(int i)
        {
            Vector3 item = data[i];
            result[i] = Mathf.Sqrt(item.x * item.x + item.y * item.y + item.z * item.z);
        }
    }

    public void CalaDataC()
    {
        DateTime beforeDT = DateTime.Now;
        SingleJob1 jobs = new SingleJob1
        {
            data = data_b,
            result = result_b
        };
        var handler = jobs.Schedule(size, 64);
        handler.Complete();
        DateTime afterDT = DateTime.Now;
        TimeSpan ts = afterDT.Subtract(beforeDT);
        print($"Burst多线程计算时间: {ts.TotalMilliseconds}ms");
    }
}

```

![DOTS](\..\Image\DOTS\DOTS1.png)

## Component

是结构体 Strucl
ECS的组件是纯组件，仅包含数据结构，不包含任何其他功能
![1](<https://img-blog.csdnimg.cn/20191226145232572.png>)
继承了IComponentData接口

按内存类型划分:

1. 非托管Compnent
2. 托管Compnent

按功能类型划分

1. 般的Component
2. Shared Component:消除实体间重复值
3. Tag Component:表示entity方便查找
4. Enableable Component:运行时方便禁用或启用
5. Cleanup Component:错误状态下防止销毁的
6. Singleton Componet:一个world唯一存在的

按数据访问类型划分

1. 按Entity访问
2. 按Chunk访问
3. 按Element访问

按接口类型划分:

1. IEnableableComponent
2. ISystemStateComponentData .
3. ISystemStateSharedComponentData
4. IComponentData
5. ISharedComponentData
6. IBufferElementData

按定义类型划分:

1. IEquatabeType - System.IEquatable{T}
2. BakingOnlyType - BakingTypeAttribute
3. TemporaryBakingType - TemporaryBakingTypeAttribute
4. IRefCountedComponent - IRefCounted
5. EnableableComponent - IEnablea bleComponent
6. HasNoEntityReferences -不包含任何Entity引用
7. CleanupComponentType - ISystemStateComponentData
8. BufferComponentType - IBufferElementData
9. SharedComponentType - ISharedComponentData
10. ManagedComponentType - IComponentData
11. hunkComponentType -被转换成Chunk Components .
12. ZeroSizeInChunkType - Chunk只存储了0个字节的component类型.
13. CleanupSharedComponentType - ISystemStateSharedComponentData
14. ManagedSharedComponentType - ISharedComponentData

## Entity

ECS里没有实体，只有实体ID

**Entities包中的数据大类:**

1. ComponentData
2. BufferData
3. ISharedComponentData
4. EntityData
5. UnityEngineObject

遍历查询Entity数据的5种方式

1. SystemAPI.Query + ForEach
2. IJobEntity .
3. IJobChunk
4. Entities.ForEach
5. Manually

IJobEntity查询

1. 与Entities.ForEach功能类似,为每个Entity调用一次Execute
2. 可在多个System中重用
3. 底层时IJobChunk实现的
4. 查询方式WithAll、WithAny、WithNone、WithChangeFilter、
WithOptions
5. EntityIndexInQuery

```C#
[BurstCompile]
partial struct CopyPositionsJob : IJobEntity
{
    public NativeArray <float3> copyPositions;
    public void Execute([EntityIndexInQuery] int entityIndexInQuery, in LocalToWorld localToWorld)
    {
        copyPositions[entityIndexInQuery] = localToWorld.Position;
    }
}
```

IJobChunk

1. 遍历ArcheType Chunk
2. 为每个Chunk调用一次Execute
3. 一般用在不需要遍历每个Chunk中Entity的情况或者对Chunk内的Entity执行多
次遍历或以不寻常顺序遍历的情况
4. useEnabledMask与ChunkEnableMask来辅助过滤Enableable Component
未激活的Entity

Entities. ForEach

1. 只用于继承SystemBase创建的System
2. 定义是一个Lambda表达式
3. 有太多的使用限制

手动遍历

1. entityManager.GetAllEntities()
2. entity Manager.GetAllChunks()

## Aspect

Aspect中可包含数据类型

1. Entity
2. RefRW< T>和RefRO\<T>
3. Ena bled RefRW和EnabledRefRO
4. DynamicBuffer\<T>
5. 其他Aspect类型.

```C#
readonly partial struct RotateAndMoveAspect : IAspect
{
    readonly RefRW<LocalTransform> localTransform;
    readonly RefRO<RotateAndMoveSpeed> speed;

    public void Move (doubLe eLapsedTime)
    {
        LocalTransform . ValueRW. Position.y = (float)math. sin( x: elapsedTime *speed . ValueRO . moveSpeed) ;
    }

    public void Rotate(float deltaTime)
    {
        LocalTransform . VaLueRW = LocalTransform. ValueRO . RotateY(speed. ValueRO. rotateSpeed* deltaTime);
    }
    
    public void RotateAndMove (doubLe elapsedTime, float deltaTime)
    {
        LocalTransform. ValueRW. Position.y = (float)math. sin( x: elapsedTime *speed. ValueRO. moveSpeed);
        LocalTransform. ValueRW = LocalTransform. ValueRO . RotateY(speed. ValueRO. rotateSpeed* deltaTime);
    }
}
```

## System

[Unity ECS学习笔记（1）ComponentSystem和ForEach](<https://blog.csdn.net/weixin_40124181/article/details/103713240>)

ECS的System是用于处理逻辑的，并且只处理逻辑，它不应该包含组件或其他东西，它是纯的，整个游戏中，可能会存在很多很多的System。

> PS:可能继承对不上,不过不影响理解,新版有ISystem和SystemBase,可能以后会修改

``` c#
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

// This system updates all entities in the scene with both a RotationSpeed_ForEach and Rotation component.
public class RotationSpeedSystem_ForEach : ComponentSystem
{
    protected override void OnUpdate ()
    {
        Entities.ForEach((ref RotationSpeed_ForEach rotationSpeed, ref Rotation rotation) =>
        {
            var deltaTime = Time.DeltaTime;
            /* 旋转Cube，代码逻辑不用管，这里可以是其他任何逻辑 */
            rotation.Value = math.mul(math.normalize(rotation.Value),
            quaternion.AxisAngle(math.up(), rotationSpeed.RadiansPerSecond * deltaTime));
        });
    }
}
```

ForEach的参数是一个Action，我们这个Action有两个参数：RotationSpeed_ForEach rotationSpeed和Rotation rotation。

那么，ForEach就会把`所有同时包含RotationSpeed_ForEach组件和Rotation组件的实体查出来`，遍历一次。（Rotation是系统自带的组件，我们的Cube在转换为实体时，也会自动附加Rotation组件）

然后，我们就可以在Action里写我们的逻辑，比如这里的逻辑是修改实体的旋转值。

ForEach里传递的参数是一个Action，Action可以带有1到6个参数，都是组件类型（用于筛选实体）
Entities的ForEach是用来筛选实体的,通过组件筛选——把包含指定组件的实体筛选出来

System逻辑的先后顺序---生命周期
ComponentSystem的OnUpdate：每帧都会被调用。
JobComponentSystem的OnUpdate：取决于Job里筛选出来的实体数量是否大于0，如果是，则每帧都会调用OnUpdate；如果筛选出来的实体数量是0，那么，OnUpdate不会被调用。

**系统分组（ComponentSystem Group）**
在了解系统如果控制先后顺序之前，需要先了解系统的分组。
唔，其实很简单，就是分组…
比如，SystemA和SystemB都属于一个叫做SystemGroupHello的分组（名字无所谓），SystemC和SystemD属于SystemGroupOther分组。
就是这么简单，就是我们所认知的普通的分组概念。

**System的执行顺序（System Update Order）**
InitializationSystemGroup（负责初始化工作的系统分组）
SimulationSystemGroup（负责逻辑运算的系统分组）
PresentationSystemGroup（负责图形与渲染工作的系统分组）
以上三个是ECS默认的系统分组，很重要，这三个分组里的System从上到下按照顺序被执行。（实际上SystemGroup本身也属于System，也会被调用OnUpdate函数）。

**InitializationSystemGroup**
    BeginInitializationEntityCommandBufferSystem
    CopyInitialTransformFromGameObjectSystem
    SubSceneLiveLinkSystem
    SubSceneStreamingSystem
    EndInitializationEntityCommandBufferSystem

**SimulationSystemGroup**
    BeginSimulationEntityCommandBufferSystem
    TransformSystemGroup
        EndFrameParentSystem
        CopyTransformFromGameObjectSystem
        EndFrameTRSToLocalToWorldSystem
        EndFrameTRSToLocalToParentSystem
        EndFrameLocalToParentSystem
        CopyTransformToGameObjectSystem
    LateSimulationSystemGroup
    EndSimulationEntityCommandBufferSystem

**PresentationSystemGroup**
    BeginPresentationEntityCommandBufferSystem
    CreateMissingRenderBoundsFromMeshRenderer
    RenderingSystemBootstrap
    RenderBoundsUpdateSystem
    RenderMeshSystem
    LODGroupSystemV1
    LodRequirementsUpdateSystem
    EndPresentationEntityCommandBufferSystem

以上全部都是ECS自带的System，这是基于0.3.0版本，以后的版本可能会发生变化。
注意粗体部分的内容：InitializationSystemGroup，SimulationSystemGroup，PresentationSystemGroup。

以上贴出的所有System，都会按照罗列的顺序执行OnUpdate函数，如果System又包含了子System，那么就子System也会执行OnUpdate函数。

如果大家之前有看过Entity Debugger（Window->Analysis->Entity Debugger）的话，应该就会发现，上面列出的部分System就在Debugger里：
Debugger里的System也是按照执行顺序逐一罗列的，未来我们新增的System也会出现在这里。

控制System的执行顺序

1. 我们来看看三个特性（Attribute）
1. UpdateInGroup：指定当前System在哪个分组下
1. UpdateBefore：指定当前System在哪个System之前执行
1. UpdateAfter：指定当前System在哪个System之后执行

![修改System的执行顺序](\..\Image\DOTS\DOTS2.png)

## ISystem和SystemBase

- ISystem:提供对非托管内存访问
- SystemBase:用于访问存储托管数据

![ISystem和SystemBase](../Image/DOTS/DOTS5.png)

## IJobEntity

Execute函数里做的事情和之前类似可以把例如物体旋转的逻辑卸载里面,`每一个IJobEntity，只会处理一个实体的组件对象`

1. System的OnUpdate函数的主要逻辑移到了IJobEntity中处理。
2. Job是可以并行执行的（多线程），因此能提升性能。
3. Execute函数只能对单个实体的组件（组件可以多个）进行操作。
4. Execute的参数可以带有ReadOnly特性，代表获取的这个类型的组件是只读的，不能进行写操作。
5. 如果某个Job对某个组件对象进行了写操作，则其他需要读取这个组件对象的Job无法并行执行，相当于锁住了
6. IJobEntity接口需要实现Execute函数，函数参数指定了需要筛选的组件类型（可多个），以此来获得所需实体的组件对象，然后进行读写操作。指定的组件类型需要和接口声明的泛型对应，比如我们的接口声明的泛型是： IJobForEach<Rotation, RotationSpeed_ForEach>，和Execute函数的参数类型是对应的。

**要注意的地方，重要！** 利用IJobEntity时，系统会自动并行处理，但是，它只会按照块（Chunk）来并行处理。

## NativeContainers

Unity提供了一个NativeContainer叫做NativeArray，你可以通过NativeSlice从特定位置开始操作固定长度操作NativeArray的数据子集。

- NativeArray
- NativeSIice-NativeArray的子集
- TransformAccess
- TransformAccessArray
- Unity Containers Package
  - NativeList-可变大小的NativeArray
  - NativeHashMap-key/value pairs
  - NativeMu|tiHashMap单key多值的Pairs
  - NativeQueue:一个FIFO的queue
  - 多线程同时写入版本NativeHashMap\NativeMuItiHashMap\NativeQueue
  - 自定义的NativeContainers

注意：UnityECS系统拓展了Unity.Collection命名空间下的NativeContainer类型：

1. NativeList-可变长度的NativeArra
2. NativeHashMap-键值对
3. NativeMultiHashMap-一键多值的哈希表
4. NativeQueue-先入先出列表

一个Job在默认情况下拥有NativeContainer的读写权限，这有可能会拖累性能表现，C# 的SF不允许一个Job在另一个Job写入数据的时候拥有写入权限，如果不需要写入权限，可以在NativeContainer上添加[ReadOnly]标签，比如：

```C#
[ReadOnly]
public NativeArray<int> input;
```

这样其他Job就可以在当前job执行的时候，获得NativeArray的只读权限

NativeContainer 的分配:

**Allocator.Temp** 最快的分配方法，适用于一帧或几帧的生命时长，不能将该类型分配的数据传给 Job，在方法 Return 前执行Dispose

**Allocator.TempJob** 分配速度比 Temp 慢比 Persistent 快，4帧的生命时长且线程安全。若四帧内没有调用Dispose，控制台会打印原生代码生成的警告。大部分小任务都使用该类型分配NativeContainer

**Allocator.Persistent** 是对malloc的包装，能够维持尽可能地生命时长，必要的话可以贯穿整个应用的生命周期,性能不足的情况下不应使用

## IJobParallelFor

[UnityECS学习日记八：Unity Job System 之 IJobParallelFor 多线程并行化](https://blog.csdn.net/qq_36382054/article/details/103658922)

[IJobParallelFor](https://docs.unity.cn/cn/2019.4/ScriptReference/Unity.Jobs.IJobParallelFor.html)

想让线程人物真正的并行，那么可以采用IJobParallelFor。

## 块（Chunk）和原型（Archetype）

### 块（Chunk）

![块（Chunk）和原型（Archetype）](<https://imgconvert.csdnimg.cn/aHR0cDovL3d3dy5iZW5tdXRvdS5jb20vd3AtY29udGVudC91cGxvYWRzLzIwMTkvMTIvMTIwNTE5XzAyNDFfVW5pdHlFQ1M1MS5wbmc?x-oss-process=image/format,png>)

我们有三个实体：
EntityA：包含了Translation、Rotation、LocalToWorld、Render四个组件。
EntityB：包含了Translation、Rotation、LocalToWorld、Render四个组件。
EntityC：包含了Translation、Rotation、LocalToWorld三个组件。

很明显的，EntityA和EntityB拥有的组件是一模一样的。
在ECS中，底层会把这些拥有相同组件的实体放在一起，也就是我们所说的内存块（Chunk）。
于是，EntityA和EntityB是放在一个块（Chunk）里的，而EntityC则放在另一个块（Chunk）里。

优点：与同样使用Jobs系统的上一篇案例不同的是，在循环遍历的时候，前者迭代的是实体，而这个案例迭代的是块。内存块，这个块是被一开始基于相同原型分配好了的，也就是说，如果这20个实体有相同的组件，那么他们会被紧密的安排在一、块内存中，从而方便处理器进行操作。这是相对于面向对象的散列内存而言的，在面向对象中要操作内存中的某个对象时，你不得不在整个内存中寻找它，这样会降低读取速度，也不方便批量操作。ECS则会非常紧密地分配实体在内存中的位置，相同的组件会被统一放在块中，读取速度快，也方便批量操作。

### 原型（Archetype）

![1](<https://imgconvert.csdnimg.cn/aHR0cDovL3d3dy5iZW5tdXRvdS5jb20vd3AtY29udGVudC91cGxvYWRzLzIwMTkvMTIvMTIwNTE5XzAyNDFfVW5pdHlFQ1M1Mi5wbmc?x-oss-process=image/format,png>)

比如上面那张图，一共有三个块，其实这三个块里存放的都是拥有相同组件的实体，只不过数量太多，所以分开存放到不同的块里了。

而这是存放相同组件的实体的所有块，都属于同一个类型，也就是官方所说的原型（Archetype）。

某个原型（Archetype）下的所有块（Chunk），它们都存放了相同组件的实体。

实际上，原型（Archetype）更多的是一种概念，实际在写代码的过程中，我们可能没法直接对原型进行操作。

对于某种原型下的所有块，它们的大小是一样的。

相当于预先申请了内存空间，不管塞没塞满，都占用一样的内存空间，一个块塞满之后，就又申请一个新的空的块。

## 特性

[API](<https://docs.unity.cn/Packages/com.unity.entities@0.17/api/>)

RequiresEntityConversion

Entity Component System API reference
Entity types	
Entity	The fundamental identifier in ECS
EntityArchetype	A unique combination of component types
EntityQuery	Use to select entities with specific characteristics
EntityQueryDesc	Use to create EntityQuery objects
EntityManager	Manages entities and provides utility methods
World	An isolated collection of entities
Component types	
IComponentData	A marker interface for general purpose components
ISharedComponentData	A marker interface for components shared by many entities
ISystemStateComponentData	A marker interface for specialized system components
IBufferElementData	A marker interface for buffer elements
DynamicBuffer	The API to access buffer elements
[BlobAssetReference]	A reference to a blob asset in a component
System types	
ComponentSystemBase	Defines a set of basic functionality for systems
SystemBase	The base class to extend when writing an ECS system
GameObjectConversionSystem	The base class to extend when writing GameObject conversion systems
ComponentSystemGroup	A group of systems that update as a unit
ECS job types	
Entities.ForEach	An implicitly created job that iterates over a set of entities
Job.WithCode	An implicitly created single job
IJobEntityBatch	An interface to implement to explicitly create a job that iterates over the entities returned by an entity query in batches
Other important types	
ArchetypeChunk	The storage unit for entity components
EntityCommandBuffer	A buffer for recording entity modifications used to reduce structural changes
ComponentType	Use to define types when creating entity queries
BlobBuilder	A utility class for creating blob assets, which are immutable data structures that can be safely shared between jobs using [BlobAssetReference] instances
ICustomBootstrap	An interface to implement to create your own system loop
Attributes	
UpdateInGroup	Defines the ComponentSystemGroup to which a system should be added
UpdateBefore	Specifies that one system must update before another
UpdateAfter	Specifies that one system must update after another
DisableAutoCreation	Prevents a system from being automatically discovered and run when your application starts up
ExecuteAlways	Specifies that a system's update function must be invoked every frame, even when no entities are returned by the system's entity query
GenerateAuthoringComponent	Generates a MonoBehaviour-based Component for an ECS IComponentData struct, allowing you to add it directly to a GameObject in the Unity Editor
ConverterVersion	Use to ensure that serialized data is up to date with conversion code

## Entity Command Buffer System

**[Unity ECS学习笔记（8）EntityCommandBufferSystem](<https://blog.csdn.net/weixin_40124181/article/details/103736819>)**

ECS规定，以下行为都不能在Job中处理:
创建实体（Create Entities）
销毁实体（Destroy Entities）
给实体添加组件（Add Components）
删除实体的组件（Remove Components）
但是，很多情况下，只有在Job中才能决定要不要创建实体、添加组件等，这种时候应该怎么办？
EntityCommandBufferSystem可以让我们在Job里添加一些任务队列，然后在主线程中执行这些任务。

- RenderCommandBuffer是同步统一执行 渲染状态的改变
- EntityCommandBuffer是同步统一执行Entity相关数据的改变

- Job中不能直接创建和销毁Entity
- Job中不能添加和删除Entity组件
- 只能使用EntityCommandBuffer来处理这些操作。
  - EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
- 在Parallel Job中我们需要使用ECB的ParallelWriter将命令并行记录到ECB中.
  - EntityCommandBuffer.ParallelWriter ecbParallel = ecb.AsParallelWriter();
- 为ECB回放创建专门的System
  - ecb.Playback(this. EntityManager);
  - ecb.Dispose();

- 在Job中使用时，ECB必须通过Allocator.TempJob与Allocator.Persistent来分配
- ECB的Playback方法必须在主线程上调用,记录ECB命令的Job,必须在Playback方
法调用前完成
- ECB需要在PlayBack后销毁
    Dependency.Complete();
    ecb.Playback(EntityManager);
    ecb.Dispose();

- 强烈不建议在多个Job中共享使用EntityCommandBuffer，最好为每个Job创建和使用一个ECB
- 如果多次调用ECB的Playback方法，可能会发生异常，如果你需要多次调用，请使用PlaybackPolicy.MultiPlayBack选项来创建EntityCommandBuffer实例。
    EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob, PlaybackPolicy.MultiPlayback);
    ecb. Playback(this. EntityManager);
    //这样额外调用Playback是没问题的
    ecb.Playback(this. EntityManager);
    ecb.Dispose();

- 创建自定义ECBSystem
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(FooSystem)]
    public class MyECBSystem : EntityCo mmand BufferSystem
    {}
- 获取已经存在的ECBSystem并通过存在的ECBSystem创建ECB
    EntityCommandBufferSystem sys = this.World.GetExistingSystemManaged\<FooECBSystem> ();
    EntityCommandBuffer ecb = sys.CreateCommand Buffer();
    sys.AddJobHandleForProducer(this. Dependency);

- 通常不需要创建自定义ECBSystem，因为DOTS系统会默认创建一.些ECB System，我们只需要
获取对应阶段这些ECBSystem并创建ECB实例

    ```C#
    var ecbSingleton =
        SystemAPI.GetSingleton< BeginSimulationEntityCommand BufferSystem.Singleton> ();
        var ecb = ecbSingleton.CreateCommand Buffer(state.WorldUnmanaged);
    ```

- 系统默认创建的ECBSystem包括:
  - BeginInitializationEntityCommandBufferSystem
  - EndInitializationEntityCommandBufferSystem
  - BeginSimulationEntityCommandBufferSystem
  - EndSimulationEntityCommandBufferSystem
  - BeginPresentationEntityCommandBufferSystem
  - EndP resentationEntityCommandBufferSystem

## BlobAsset

固定数据可以使用,节省大量内存

## Unity.Mathematics

一个C＃数学库提供矢量类型和数学函数（类似Shader里的语法）。由Burst编译器用来将C＃/IL编译为高效的本机代码。

这个库的主要目标是提供一个友好的数学API（对于熟悉SIMD和图形/着色器的开发者们来说），使用常说的float4，float3类型...等等。带有由静态类math提供的所有内在函数，可以使用轻松将其导入到C＃程序中然后using static Unity.Mathematics.math来使用它。

除此之外，Burst编译器还可以识别这些类型，并为所有受支持的平台（x64，ARMv7a ...等）上为正在运行的CPU提供优化的SIMD类型。

## Unity官方文档注解

### 坦克生成模块

``` C#
using Unity.Entities;

/// <summary>
/// 坦克生成
/// </summary>
class ConfigAuthoring : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject TankPrefab;//坦克物体
    public int TankCount;//坦克数量
    public float SafeZoneRadius;//安全区域半径,不能发射子弹

    /// <summary>
    /// 烘焙
    /// </summary>
    class ConfigBaker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            AddComponent(new Config
            {
                TankPrefab = GetEntity(authoring.TankPrefab),
                TankCount = authoring.TankCount,
                SafeZoneRadius = authoring.SafeZoneRadius
            });
        }
    }
}

/// <summary>
/// 类似标签的功能
/// </summary>
struct Config : IComponentData
{
    public Entity TankPrefab;
    public int TankCount;
    public float SafeZoneRadius;
}
```

``` c#
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

/// <summary>
/// 坦克出生System
/// </summary>
[BurstCompile]
partial struct TankSpawningSystem : ISystem
{
    //查询不应该在OnUpdate中当场创建，因此它们被缓存在字段中。
    // Queries should not be created on the spot in OnUpdate, so they are cached in fields.
    EntityQuery m_BaseColorQuery;//基础颜色

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="state"></param>
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        //在加载Config单例之前，该系统不应该运行。
        // This system should not run before the Config singleton has been loaded.
        state.RequireForUpdate<Config>();//需要更新

        //获取实体查询
        m_BaseColorQuery = state.GetEntityQuery(ComponentType.ReadOnly<URPMaterialPropertyBaseColor>());
    }

    /// <summary>
    /// 销毁
    /// </summary>
    /// <param name="state"></param>
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="state"></param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //获取单例
        var config = SystemAPI.GetSingleton<Config>();


        //该系统将只运行一次，所以随机种子可以硬编码。
        //使用任意常数种子使行为确定。
        // This system will only run once, so the random seed can be hard-coded.
        // Using an arbitrary constant seed makes the behavior deterministic.
        var random = Random.CreateFromIndex(1234);
        var hue = random.NextFloat();

        //帮助创建任意数量的颜色，尽可能彼此不同。
        //这种方法背后的逻辑详细如下:
        // Helper to create any amount of colors as distinct from each other as possible.
        // The logic behind this approach is detailed at the following address:
        // https://martin.ankerl.com/2009/12/09/how-to-create-random-colors-programmatically/
        URPMaterialPropertyBaseColor RandomColor()//颜色渲染
        {
            //注意:如果你不熟悉这个概念，这是一个“局部函数”。
            //你可以在网上搜索这个词以获取更多信息。
            // Note: if you are not familiar with this concept, this is a "local function".
            // You can search for that term on the internet for more information.

            // 0.618034005f == 2 / (math.sqrt(5) + 1) == inverse of the golden ratio
            hue = (hue + 0.618034005f) % 1;
            var color = UnityEngine.Color.HSVToRGB(hue, 1.0f, 1.0f);
            return new URPMaterialPropertyBaseColor { Value = (UnityEngine.Vector4)color };
        }

        //ecb单例 BeginSimulationEntityCommandBufferSystem创建 销毁等操作
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        //NativeArray 会向托管代码显示本机内存缓冲区，从而可以在托管数组和本机数组之间共享数据，无需任何编组成本。 类似数组
        var vehicles = CollectionHelper.CreateNativeArray<Entity>(config.TankCount, Allocator.Temp);
        ecb.Instantiate(config.TankPrefab, vehicles);//创建

        //一个EntityQueryMask提供了一个有效的测试特定实体是否会
        //被一个EntityQuery选中。
        // An EntityQueryMask provides an efficient test of whether a specific entity would
        // be selected by an EntityQuery.
        var queryMask = m_BaseColorQuery.GetEntityQueryMask();

        foreach (var vehicle in vehicles)
        {
            //每个预制根都包含一个LinkedEntityGroup，这是它的所有实体的列表。
            // Every prefab root contains a LinkedEntityGroup, a list of all of its entities.
            ecb.SetComponentForLinkedEntityGroup(vehicle, queryMask, RandomColor());
        }

        state.Enabled = false;
    }
}
```

## Unity ecs physics collision

[Unity ecs physics collision](<https://blog.csdn.net/RocketJ/article/details/119172286>)

Unity DOTS中的物理系统即Unity.Physics，这里使用一个简单的demo（小球相撞效果），来说明一下 碰撞 Collision。

1. 创建两个sphere（注意，要去掉ShpereCollider），添加Physics Shape，ShapeType 选择 Sphere，PhysicsShape 选择Collide Raise Collision Events。新建CollisionFilter类型Sphere和Plane，然后设置 BelongsTo = Sphere，设置CollidesWith = Sphere, Plane；添加 Physics Body。
![1](<https://img-blog.csdnimg.cn/e9b5ea9bb50c463f85312e0a6a20a8b2.png>)
![2](<https://img-blog.csdnimg.cn/6ec267c3a66c4571a4cd7ba805fda2f4.png>)
1. 小球碰撞，触发逻辑

``` C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Collections;
using Unity.Physics.Systems;

public class PhysicalSystem : SystemBase
{
    private BuildPhysicsWorld buildPhysicsWorld;//物理系统的准备工作，将TriggerCollisionEvents传递给Job
    private StepPhysicsWorld stepPhysicsWorld;//物理系统的准备工作，将TriggerCollisionEvents传递给Job

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        ColliderJob colliderJob = new ColliderJob()
        {
            velocitys = GetComponentDataFromEntity<PhysicsVelocity>()
        };
        Dependency = colliderJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);
    }
}


public struct ColliderJob : ICollisionEventsJob
{
    public ComponentDataFromEntity<PhysicsVelocity> velocitys;

    public void Execute(CollisionEvent collisionEvent)
    {
        if (velocitys.HasComponent(collisionEvent.EntityA) && IsSphere(collisionEvent.EntityA, "Sphere"))
        {
            if (velocitys.HasComponent(collisionEvent.EntityB) && IsSphere(collisionEvent.EntityB, "Sphere"))
            {
                var velocityA = velocitys[collisionEvent.EntityA];
                var dirA = -math.sign(velocityA.Linear.x);
                //Debug.Log("dirA = " + dirA);
                velocityA.Linear.x = dirA * 20;
                velocitys[collisionEvent.EntityA] = velocityA;

                var velocityB = velocitys[collisionEvent.EntityB];
                var dirB = -math.sign(velocityB.Linear.x);
                //Debug.Log("dirB = " + dirB);
                velocityB.Linear.x = dirB * 20;
                velocitys[collisionEvent.EntityB] = velocityB;
            }
        }
    }

    public bool IsSphere(Entity entity, string name)
    {
        return World.DefaultGameObjectInjectionWorld.EntityManager.GetName(entity) == name;
    }
}
```
