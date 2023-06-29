# Unity资源详解-prefab详解

<https://blog.csdn.net/qq_37776196/article/details/116006274>

<https://www.cnblogs.com/rollingyouandme/p/14674276.html>

<https://blog.csdn.net/nikoong/article/details/115145059>

**[prefab内容分析](<https://www.cnblogs.com/blueberryzzz/p/9097391.html>)**

<https://blog.csdn.net/keneyr/article/details/111060396>

## 写在前面

> + 当前使用的unity版本：5.3.7p4。
> + 如果打开prefab文件是乱码：  
>     ![](https://images2018.cnblogs.com/blog/1362861/201805/1362861-20180527210631386-815712050.png)  
>     把editer的asset Srialization改为Force Text即可。

## 一、什么是Prefab

Prefab是unity3d中的一种资源类型，用于存储可重复使用的游戏对象，来方便快捷的创建实例。  
通过prefab在场景中创建的所有实例，都会链接到原始的prefab，所以当修改原始的prefab时，所有场景中的所有prefab实例都会被修改。

## 二、Prefab文件的内容

Prefab文件的内容是通过YAML语言序列化的一个GameObject的对象，包含了这个GameObject的所有描述信息（GameObject信息、Compent实例和属性等）。

## 三、从一个Cube的Prefab说起

### prefab文件的内容

```yaml
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!1 &101978
GameObject:
  m_ObjectHideFlags: 0
  m_PrefabParentObject: {fileID: 0}
  m_PrefabInternal: {fileID: 100100000}
  serializedVersion: 4
  m_Component:
  - 4: {fileID: 481870}
  - 33: {fileID: 3343092}
  - 65: {fileID: 6596578}
  - 23: {fileID: 2346436}
  m_Layer: 0
  m_Name: aCube
  m_TagString: Untagged
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!4 &481870
Transform:
  m_ObjectHideFlags: 1
  m_PrefabParentObject: {fileID: 0}
  m_PrefabInternal: {fileID: 100100000}
  m_GameObject: {fileID: 101978}
  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}
  m_LocalPosition: {x: 384.3385, y: 224.03131, z: 116.32626}
  m_LocalScale: {x: 1, y: 1, z: 1}
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
  m_Children: []
  m_Father: {fileID: 0}
  m_RootOrder: 0
--- !u!23 &2346436
MeshRenderer:
  m_ObjectHideFlags: 1
  m_PrefabParentObject: {fileID: 0}
  m_PrefabInternal: {fileID: 100100000}
  m_GameObject: {fileID: 101978}
  m_Enabled: 1
  m_CastShadows: 1
  m_ReceiveShadows: 1
  m_Materials:
  - {fileID: 10303, guid: 0000000000000000f000000000000000, type: 0}
  m_SubsetIndices: 
  m_StaticBatchRoot: {fileID: 0}
  m_UseLightProbes: 1
  m_ReflectionProbeUsage: 1
  m_ProbeAnchor: {fileID: 0}
  m_ScaleInLightmap: 1
  m_PreserveUVs: 1
  m_IgnoreNormalsForChartDetection: 0
  m_ImportantGI: 0
  m_MinimumChartSize: 4
  m_AutoUVMaxDistance: 0.5
  m_AutoUVMaxAngle: 89
  m_LightmapParameters: {fileID: 0}
  m_SortingLayerID: 0
  m_SortingOrder: 0
--- !u!33 &3343092
MeshFilter:
  m_ObjectHideFlags: 1
  m_PrefabParentObject: {fileID: 0}
  m_PrefabInternal: {fileID: 100100000}
  m_GameObject: {fileID: 101978}
  m_Mesh: {fileID: 10202, guid: 0000000000000000e000000000000000, type: 0}
--- !u!65 &6596578
BoxCollider:
  m_ObjectHideFlags: 1
  m_PrefabParentObject: {fileID: 0}
  m_PrefabInternal: {fileID: 100100000}
  m_GameObject: {fileID: 101978}
  m_Material: {fileID: 0}
  m_IsTrigger: 0
  m_Enabled: 1
  serializedVersion: 2
  m_Size: {x: 1, y: 1, z: 1}
  m_Center: {x: 0, y: 0, z: 0}
--- !u!1001 &100100000
Prefab:
  m_ObjectHideFlags: 1
  serializedVersion: 2
  m_Modification:
    m_TransformParent: {fileID: 0}
    m_Modifications: []
    m_RemovedComponents: []
  m_ParentPrefab: {fileID: 0}
  m_RootGameObject: {fileID: 101978}
  m_IsPrefabParent: 1
```

### 分析其中内容

#### 1.前两行内容是yaml语言的注释

#### 2.接下来是prefab中所有对象的描述信息

每个元素的描述内容以--- !u!n1 & n2开头，其中n1代表元素的类型ID（每个ID的具体含义参考[YAML Class ID Reference](https://docs.unity3d.com/Manual/ClassIDReference.html)），n2代表这个元素的本地ID（fileID，在prefab文件中唯一）。  
可以看到这个prefab中有GameObject，Transform，MeshRenderer，MeshFilter，BoxCollider，Prefab共6个对象。  
除了Prefab对象代表prefab本身的信息，其他5个对象正是一个Cube的构成。

#### 3.第一个GameObject对象的详细说明

**<1>--- !u!1 &101978**  
1代表这是一个GameObject类型的对象，101978是这个GameObject对象在Prefab资源中的fileID。  
**<2>m\_ObjectHideFlags：0**  
这个元素在Project视图中是否被隐藏，因为prefab中只有这一个GameObject作为根GameObject，所以没被隐藏，值为0。  
**<3>m\_PrefabParentObject: {fileID: 0}**  
代表场景中的prefab实例与原始prefab资源的链接，当一个prefab实例链接被破坏（删除prefab实例的一个子节点，或修改一个子节点父子关系），或其中的compent被剥离到场景文件中（stripped，比如prefab实例在场景中不是根节点，而是一个GameObject的子节点，那么prefab实例的transtrom就会被剥离到场景文件中，赋予一个fileID，用来描述prefab实例的父子关系）时，生成的对象的m\_PrefabParentObject会对应到原始prefab资源中的对象。

在原始prefab资源中，所有对象的m\_PrefabParentObject都是0，代表空。  
**<4>m\_PrefabInternal: {fileID: 100100000}**  
该属性表示这个对象属于哪个prefab对象。  
在场景中当一个compent被剥离时，会指向场景中的prefab实例。  
在原始prefab资源中，因为prefab中只有一个prefab对象，fileID就是1000100000。  
所以在原始prefab资源中，所有对象的m\_PrefabInternal值都是{fileID: 100100000}。  
**Stripped举例：**  
新建一个叫testStripped的prefab，用一个空的GameObject给它赋值，然后将这个prefab拖到Main Camera下，保存场景文件后查看场景文件内容。

```yaml
Transform:
  m_ObjectHideFlags: 0
  m_PrefabParentObject: {fileID: 0}
  m_PrefabInternal: {fileID: 0}
  m_GameObject: {fileID: 280358070}
  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}
  m_LocalPosition: {x: 0, y: 1, z: -10}
  m_LocalScale: {x: 1, y: 1, z: 1}
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
  m_Children:
  - {fileID: 619871928}
  m_Father: {fileID: 0}
  m_RootOrder: 0
--- !u!1001 &619871927
Prefab:
  m_ObjectHideFlags: 0
  serializedVersion: 2
  m_Modification:
    m_TransformParent: {fileID: 280358075}
    m_Modifications:
    - target: {fileID: 488654, guid: 617cd7fcc638efd44b4beab357086bc0, type: 2}
      propertyPath: m_LocalPosition.x
      value: 384.3385
      objectReference: {fileID: 0}
    - target: {fileID: 488654, guid: 617cd7fcc638efd44b4beab357086bc0, type: 2}
      propertyPath: m_LocalPosition.y
      value: 223.03131
      objectReference: {fileID: 0}
    - target: {fileID: 488654, guid: 617cd7fcc638efd44b4beab357086bc0, type: 2}
      propertyPath: m_LocalPosition.z
      value: 126.32626
      objectReference: {fileID: 0}
    - target: {fileID: 488654, guid: 617cd7fcc638efd44b4beab357086bc0, type: 2}
      propertyPath: m_LocalRotation.x
      value: 0
      objectReference: {fileID: 0}
    - target: {fileID: 488654, guid: 617cd7fcc638efd44b4beab357086bc0, type: 2}
      propertyPath: m_LocalRotation.y
      value: 0
      objectReference: {fileID: 0}
    - target: {fileID: 488654, guid: 617cd7fcc638efd44b4beab357086bc0, type: 2}
      propertyPath: m_LocalRotation.z
      value: 0
      objectReference: {fileID: 0}
    - target: {fileID: 488654, guid: 617cd7fcc638efd44b4beab357086bc0, type: 2}
      propertyPath: m_LocalRotation.w
      value: 1
      objectReference: {fileID: 0}
    - target: {fileID: 488654, guid: 617cd7fcc638efd44b4beab357086bc0, type: 2}
      propertyPath: m_RootOrder
      value: 0
      objectReference: {fileID: 0}
    m_RemovedComponents: []
  m_ParentPrefab: {fileID: 100100000, guid: 617cd7fcc638efd44b4beab357086bc0, type: 2}
  m_IsPrefabParent: 0
--- !u!4 &619871928 stripped
Transform:
  m_PrefabParentObject: {fileID: 488654, guid: 617cd7fcc638efd44b4beab357086bc0, type: 2}
  m_PrefabInternal: {fileID: 619871927}
```

查看其中Main Camera的transform，prefab实例和stripped的transform的信息。  
发现Main Camera的transform的m\_Children属性指向了sttripped的transform，代表Prefab实例是Main Camera的子节点。  
sttripped的transform的m\_ParentPrefab指向了Prefab资源中对应的transform属性，m\_IsPrefabParent指向了场景文件中的Prefab实例。

通过对Preafab的transform剥离到场景文件中，实现了场景中对Prefab实例父子关系的记录。  
**<5>serializedVersion: 4**  
序列化的版本，跟unity版本有关，不太确定。  
**<6>m\_Component:**  
该GameObject包含哪些component，包含的对象用fileID表示。这个GameObject包含了4个Component，分别是Transform，MeshRenderer，MeshFilter，BoxCollider。  
**<7>m\_Layer: 0**  
GameObject所在的层级。  
**<8>m\_Name: aCube**  
GameObject的名称。  
**<9>m\_TagString: Untagged**  
Tag。  
**<10>m\_Icon: {fileID: 0}**  
图标，fileID为0代表没有图标。  
图标会显示在场景中，比如选择这个红色的icon，在场景中就可以看到prefab上出现了一个红色的标签。0代表没有标签。  
![](https://images2018.cnblogs.com/blog/1362861/201805/1362861-20180527210700716-1421433329.png)  
![](https://images2018.cnblogs.com/blog/1362861/201805/1362861-20180527210714220-974373632.png)  
**<11>m\_NavMeshLayer: 0**  
![](https://images2018.cnblogs.com/blog/1362861/201805/1362861-20180527210732033-1844992393.png)  
GameObject的NavigationArea属性，与自动寻路有关。  
**<12>m\_StaticEditorFlags: 0**  
GameObject的Static属性。  
**<13>m\_IsActive: 1**  
GameObject是否是active状态。

#### 4.接下来四个对象描述了这个GameObject上的4个Component，选择比较有代表性的Transform和MeshFilter进行分析

#### Transform

**<1>前三行跟GameObject的含义一样**  
**<2>m\_GameObject: {fileID: 101978}**  
Transoform所属GameObject的fileID。  
**<3>m\_LocalRotation: {x: 0, y: 0, z: 0, w: 1}**  
记录的Transform的LocalRotation信息，这个是通过四元数表示的，可在debug模式下查看。  
![](https://images2018.cnblogs.com/blog/1362861/201805/1362861-20180527210750331-107527912.png)  
**<4>m\_LocalPosition: {x: 88.257, y: 90.200806, z: 90.15849}**  
Transform的LocalPosition属性。  
**<5>m\_LocalScale: {x: 1, y: 1, z: 1}**  
Transform的LocalScale属性。  
**<6>m\_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}**  
Transform的LocalEulerAnglesHint属性，在Normal模式下可见。

#### MeshFilter

**<1>前4行的含义上面已经讲过了**  
**<2>m\_Mesh: {fileID: 10202, guid: 0000000000000000e000000000000000, type: 0}**  
这代表这个Mesh属性的值是一个外部资源，通过唯一的外部资源guid（每个外部资源的guid保存在同名的.meta文件下）和资源的本地fileID确定了具体的资源对象。  
通过对项目中各个引用的外部资源的查看，猜测type的含义为：  
0代表unity的内置资源。  
2代表Yaml描述的复合类型的外部资源（prefab，animControoller等）。  
3代表基础类型的外部资源（image，script等）。

#### 5.prefab对象

**<1>前两个说过了**  
**<2>m\_Modification:**  
当一个prefab实例化到场景中的时候，代表场景prefab实例被修改的值，这些值不随原始prefab资源的修改而修改。  
在原始prefab资源中是空的。  
**<3>m\_ParentPrefab: {fileID: 0}**  
在场景中的一个prefab实例中代表这个prefab对应的原始perfab资源中的prefab对象。  
在prefab实例中是0。  
**<4>m\_RootGameObject: {fileID: 152762}**  
prefab资源对应的GameObject。  
**<5>m\_IsPrefabParent: 1**  
可能是Prefab是资源还是实例的标记，不太确定。

### 简单总结一下一个prefab的内容

prefab文件并不存储具体资源，而是使用了yaml语言描述了一个被序列化的GameObject对象中包含的所有GameObject，Component的信息。包括相互之间的关系、存储的数据和引用信息。

## 四、prefab在场景中的使用

### 1.简单介绍场景文件的内容

一个场景文件也是通过yaml语言序列化的场景内容，一个场景文件不仅包含显示在hierarchy中的GameObject、prefab信息，还包含了场景的设置信息。比prefab稍微复杂一些。

### 2.prefab文件在场景中的内容

新建一个test场景，将test拖入场景中，然后保存，查看场景中的Prefab实例信息。

```yaml
--- !u!1001 &131130024
Prefab:
  m_ObjectHideFlags: 0
  serializedVersion: 2
  m_Modification:
    m_TransformParent: {fileID: 0}
    m_Modifications:
    - target: {fileID: 481870, guid: fda6bd048b136f44286b537a5526a83c, type: 2}
      propertyPath: m_LocalPosition.x
      value: 384.3385
      objectReference: {fileID: 0}
    - target: {fileID: 481870, guid: fda6bd048b136f44286b537a5526a83c, type: 2}
      propertyPath: m_LocalPosition.y
      value: 224.03131
      objectReference: {fileID: 0}
    - target: {fileID: 481870, guid: fda6bd048b136f44286b537a5526a83c, type: 2}
      propertyPath: m_LocalPosition.z
      value: 116.32626
      objectReference: {fileID: 0}
    - target: {fileID: 481870, guid: fda6bd048b136f44286b537a5526a83c, type: 2}
      propertyPath: m_LocalRotation.x
      value: 0
      objectReference: {fileID: 0}
    - target: {fileID: 481870, guid: fda6bd048b136f44286b537a5526a83c, type: 2}
      propertyPath: m_LocalRotation.y
      value: 0
      objectReference: {fileID: 0}
    - target: {fileID: 481870, guid: fda6bd048b136f44286b537a5526a83c, type: 2}
      propertyPath: m_LocalRotation.z
      value: 0
      objectReference: {fileID: 0}
    - target: {fileID: 481870, guid: fda6bd048b136f44286b537a5526a83c, type: 2}
      propertyPath: m_LocalRotation.w
      value: 1
      objectReference: {fileID: 0}
    - target: {fileID: 481870, guid: fda6bd048b136f44286b537a5526a83c, type: 2}
      propertyPath: m_RootOrder
      value: 2
      objectReference: {fileID: 0}
    m_RemovedComponents: []
  m_ParentPrefab: {fileID: 100100000, guid: fda6bd048b136f44286b537a5526a83c, type: 2}
  m_IsPrefabParent: 0
```

可以看到一个prefab被实例化到场景中时，没有将prefab资源中内容直接复制到场景中，而是生成了一个prefab对象。  
m\_ParentPrefab代表这个prefab对应的prefab资源。  
m\_Modification代表这个prefab实例中属性的修改信息，场景在读取一个prefab实例时，会查看prefab资源中的属性是否在m\_Modification中存在，如果存在，则使用m\_Modification中保存的值。这样就保证了修改了场景中的prefab实例的属性，不会被prefab资源的修改而覆盖。  
正是这两个属性让prefab实例具有了与prefab资源同步修改，但会保存自己特性的功能。

### 3.Break Prefab Instance

如果我们希望一个prefab实例完全不随prefab的修改而修改，那么我们可以通过Break Prefab Instance选项实现。  
![](https://images2018.cnblogs.com/blog/1362861/201805/1362861-20180527210814206-1113435869.png)  
将一个prefab实例Break之后，这个prefab就会像一个普通的GameObject存在场景中

```yaml
--- !u!1 &765230757
GameObject:
  m_ObjectHideFlags: 0
  m_PrefabParentObject: {fileID: 101978, guid: fda6bd048b136f44286b537a5526a83c, type: 2}
  m_PrefabInternal: {fileID: 0}
  serializedVersion: 4
  m_Component:
  - 4: {fileID: 765230761}
  - 33: {fileID: 765230760}
  - 65: {fileID: 765230759}
  - 23: {fileID: 765230758}
  m_Layer: 0
  m_Name: aCube
  m_TagString: Untagged
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!23 &765230758
MeshRenderer:
  m_ObjectHideFlags: 0
  m_PrefabParentObject: {fileID: 2346436, guid: fda6bd048b136f44286b537a5526a83c,
    type: 2}
  m_PrefabInternal: {fileID: 0}
  m_GameObject: {fileID: 765230757}
  m_Enabled: 1
  m_CastShadows: 1
  m_ReceiveShadows: 1
  m_Materials:
  - {fileID: 10303, guid: 0000000000000000f000000000000000, type: 0}
  m_SubsetIndices: 
  m_StaticBatchRoot: {fileID: 0}
  m_UseLightProbes: 1
  m_ReflectionProbeUsage: 1
  m_ProbeAnchor: {fileID: 0}
  m_ScaleInLightmap: 1
  m_PreserveUVs: 1
  m_IgnoreNormalsForChartDetection: 0
  m_ImportantGI: 0
  m_MinimumChartSize: 4
  m_AutoUVMaxDistance: 0.5
  m_AutoUVMaxAngle: 89
  m_LightmapParameters: {fileID: 0}
  m_SortingLayerID: 0
  m_SortingOrder: 0
--- !u!65 &765230759
BoxCollider:
  m_ObjectHideFlags: 0
  m_PrefabParentObject: {fileID: 6596578, guid: fda6bd048b136f44286b537a5526a83c,
    type: 2}
  m_PrefabInternal: {fileID: 0}
  m_GameObject: {fileID: 765230757}
  m_Material: {fileID: 0}
  m_IsTrigger: 0
  m_Enabled: 1
  serializedVersion: 2
  m_Size: {x: 1, y: 1, z: 1}
  m_Center: {x: 0, y: 0, z: 0}
--- !u!33 &765230760
MeshFilter:
  m_ObjectHideFlags: 0
  m_PrefabParentObject: {fileID: 3343092, guid: fda6bd048b136f44286b537a5526a83c,
    type: 2}
  m_PrefabInternal: {fileID: 0}
  m_GameObject: {fileID: 765230757}
  m_Mesh: {fileID: 10202, guid: 0000000000000000e000000000000000, type: 0}
--- !u!4 &765230761
Transform:
  m_ObjectHideFlags: 0
  m_PrefabParentObject: {fileID: 481870, guid: fda6bd048b136f44286b537a5526a83c, type: 2}
  m_PrefabInternal: {fileID: 0}
  m_GameObject: {fileID: 765230757}
  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}
  m_LocalPosition: {x: 384.3385, y: 224.03131, z: 116.32626}
  m_LocalScale: {x: 1, y: 1, z: 1}
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
  m_Children: []
  m_Father: {fileID: 0}
  m_RootOrder: 2
```

每个元素的m\_PrefabParentObject都指向了原始prefab资源中对应的对象。  
![](https://images2018.cnblogs.com/blog/1362861/201805/1362861-20180527210827033-1039301790.png)  
右边的prefab表示也会变为黄色，这代表这个prefab虽然被break了，但是还可以通过与原始prefab资源的链接去select，revert或apply，来变回或修改原来的prefab实例。
