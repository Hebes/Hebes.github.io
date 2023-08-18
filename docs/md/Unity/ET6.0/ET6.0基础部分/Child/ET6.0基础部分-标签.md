# ET6.0基础部分-标签

**[ET分析器介绍](<https://www.yuque.com/u28961999/yms0nt>)**

## [ET0001] AddChildTypeAnalyzer

**介绍：**在ET中使用AddChild、AddChildWithId 方法对父实体添加子实体时，要求父实体必须含有ChildType标签。

**ChildType有两种标记方式：**

1. 无类型:  **[ChildType]**
此种方式适合Scene Session等需要添加多种子实体类型的特殊情况，可以对父实体添加任意类型的子实体.。
1. 唯一类型：**[ChildType(typeof(typeA))]**
此种方式适合只拥有唯一子实体类型的实体类，只能添加标记类型的子实体。建议没有多种子实体需求的情况都使用唯一类型标记。

**约束目的：**

1. 帮助开发者快速了解特定实体的子实体类型，提升代码可读性。
2. 约束开发者滥用添加子实体功能，尽量保证一个父实体只拥有一类子实体，提升代码可维护性。

## [ET0002] EntityFiledAccessAnalyzer

介绍：ET中实体类的字段成员只能由该实体的生命周期System类(Awake Update Destroy等) 或添加了[FriendClass(ET7为FriendOf)]标签的静态类中访问。
对于不想限制访问权限的字段，建议将字段改为get set属性，该约束不会限制属性成员的访问。
对于外部模块对实体字段的修改，应该在实体System中用方法封装。

## [FriendClass(FriendOf)]标签添加方式

在所有静态类中引用任何实体类EntityType的字段成员,都需要添加
**[FriendClasstypeof(EntityType))]。**
目前提供了FriendClass自动补全功能，在ET0002报错红线处按下Alt+Enter,IDE会自动帮你填写。

**约束目的：**
ET基于ECS思想设计，实体的字段成员需要使用public 关键字，这导致实体成员可以被任意访问。
FriendClass标签则相当于ET定义的实体字段访问权限，能够帮助开发者维护不同组件间的字段访问规范。

## [ET0003] EntityClassDeclarationAnalyzer

介绍：该约束限制了实体类只能继承自ET.Entity， 禁止多层继承。

约束目的：
ET架构的设计理念中，一切面向对象的多态需求都可以用ET中的分发逻辑来代替, 分发逻辑可以参考AI模块 机器人模块 网络消息 等等，ET中大量使用了分发设计。
相比扩展的便利，多重继承带来了更严重的问题。
具体OOP相关参考链接：
<https://www.yuque.com/et-xd/docs/num22w>

## [ET0004] HotfixProjectFieldDeclarationAnalyzer

介绍：Hotfix程序集中不允许声明非Const字段。

约束目的：
Hotfix程序集中的类只有非静态的实体生命周期类和静态类，而实体生命周期类的实例是进程内全局唯一的，同种实体类共享同一套生命周期类对象。
因此除了少数常量可以定义在hotfix程序集，所有字段都应在Model程序集中定义。

## [ET0005] ClassDeclarationInHotfixAnalyzer

介绍：该约束限制了Hofix程序集中只能声明含有BaseAttribute子类特性的类(包括实体的生命周期类，分发逻辑类)或静态类。
可以理解为只能声明实体的生命周期类，分发逻辑类, 静态辅助类三种。

约束目的：
1.ET的热重载机制是替换只包含了方法的Hotfix程序集的dll, 不改变包含数据的Model程序集。如果在Hotfix程序集定义数据类，会导致热重载出问题。
2.ET的数据层在Model程序集，如果在Hotfix中定义一个数据类，Model层获取不到这个类。

### [ET0006] EntityMethodDeclarationAnalyzer

介绍：
实体类禁止声明方法。
框架内的实体类若需要特殊处理绕过该约束可以添加 EnableMethod 标签。

约束目的：

1. ET的ECS设计理念中。实体和方法分离，实体类定义在Model程序集，方法定义在Hotfix程序集。
2. ET的热重载只能改变Hotfix程序集的方法，若定义在Model程序集，则无法热重载。
3. 方法和数据成员定义在一起会降低代码可读性、可维护性。

## [ET0007] EntityComponentAnalyzer

介绍：
实体类添加或获取组件需要对组件添加ComponentOf标签,表明该组件的父实体类型。

ComponentOf有两种标记方式：

1. 无类型:  [ComponentOf]
此种方式适合组件需要添加到多种父实体类型的情况。
2. 唯一类型：[ComponentOf(typeof(typeA))]
此种方式适合只拥有唯一父实体类型的组件，大多数业务组件都建议使用此种标记。

约束目的：

1. 帮助开发者快速了解特定实体和组件的关系，提升代码可读性。
2. 约束开发者滥用添加组件功能，尽量保证一个组件只拥有一类父实体，提升代码可维护性。

## [ET0008] [ET0009] ETTaskAnalyzer

介绍：
在非异步方法内使用ETTask时，必须添加.Coroutine()后缀。
在异步方法体内使用ETTask时需要添加await前缀 或 .Coroutine()后缀。

约束目的：

1. 使用ETTask对象池时，必须await或者调用Coroutine才能回收重用ETTaskCompletionSource。
2. 某个会抛出异常的ETTask调用了Coroutine()，异常就不会往上抛了，会被ETVoid抓住，然后打印，可以看看ETTask的代码，里面有个Log.Error(exception)
