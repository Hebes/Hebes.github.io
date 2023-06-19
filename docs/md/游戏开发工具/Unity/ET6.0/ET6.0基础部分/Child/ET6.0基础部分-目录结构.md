# ET6.0基础部分-目录结构

## **简单版:**

1. `Unity.Mono：`所有冷更层代码
2. `Unity.Model：`热更层的Model，纯数据, 继承Entity IAwake初始化
3. `Unity.ModelView：`热更层的ModelView，涉及到Unity交互的都可以放在这里，例如相机类，UI类等，依旧是纯数据,Unity相关的数据
4. `Unity.Hotfix：`对应Unity.Model的纯逻辑,行为,静态类静态方法=>行为 可以控制组件
5. `Unity.HotfixView：`对应Unity.ModeView的纯逻辑,Unity相关的行为

## 详解版

**1. Model**
**Model层负责定义实体和组件**,在这里要定义实体Unit,以及一些组件(MoveComponent,CombatComponent等)以及还需定义UnitType的枚举,以便后续逻辑的分发处理
**注意：**
**Model下的实体不能调用UnityAPI,与Unity交互的组件实体只能放在ModelView下**
**Model中只能存在数据,例如position,UnitType不能有对数据的操作**

**2. ModelView**
**负责定义与Unity交互的实体和组件**,有一些诸如动画组件,GameObject组件用到的数据由Unity提供,就需要将这些实体组件放到这个下面。

**3. Hotfix**
**Hotfix负责System行为的定义**,提供创建实体和为实体挂载组件的功能。此时定义的UnitFactory工厂就应为不同的实体提供不同的创建方法,例如CreatePlayer,就应先将UnitType置为Player,为其添加MoveComponent组件等待操作。再如CreateNPC,UnitType设置为NPC后,由于npc一般不会移动则无需添加移动组件,可以添加对话组件等待。
**注意：**
**Hotfix下只能定义行为,不能包含数据状态,只能对Model提供的数据状态进行相关操作**
**Hotfix下也不能使用Unity Api 对于一些需要Unity才能挂载的组件,需要放到view中执行**

**4. HotfixView**
HotfixView负责需要与Unity交互的System行为,
例如：
加载模型prefab,实例化游戏对象GameObject,若实体中需要用到GameObject对象,则还应为Unit实体添加GameObject组件,里面存有gameObject。

若Unit实体还需要在场景中播放动画,还需要为其添加使用了UnityApi的AnimatorComponent。

同理,在处理与Unity交互的System行为时,不同类型的Unit也可能行为有所不同,在此需要针对UnitType提供不同的行为。
