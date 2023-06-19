# ET6.0Component和实体Entity的理解

[斗地主](<https://www.taikr.com/my/course/1053>)
ET中的实体与组件，实现了基础的给实体添加组件，给组件添加事件，组件的扩展类可以作为System系统。

**实体Entity**
可以创建一个实体类，继承Entity，通过实体类的属性可以持有需要的ID号，实体状态属性，关联的GameObject对象，或者UI对象。

//unity中的实体对象，这个实体持有玩家或者npc的可见角色模型动画物体

``` text {.line-numbers}
public sealed class Unit: Entity
{
       public int ConfigId;
       //带有角色模型动画的物体
       public GameObject GameUnit { get; set; }
}
```

``` text {.line-numbers}
//登录服务器每个用户的实体叫User，进入地图（斗地主叫进入房间）叫Player或Gamer
public sealed class Player : Entity
{
        //实体对象的唯一ID，比如斗地主这个案例叫UserID
        //可以用这个ID查询实体的数据库信息，可以通过这个ID找到实体实例
        public long UnitId { get; set; }
}
```

``` text {.line-numbers}
public sealed class User : Entity
{
        /// \<summary\>
        /// 读取自数据库中的永久ID
        /// 本程序中出现的UserID字样变量均代表此意
        /// </summary>
        public long UserID { get; private set; }
 
        /// <summary>
        /// 玩家所在的Gate服务器的AppID
        /// </summary>    
        public int GateAppID { get; set; }

        /// <summary>
        /// 玩家所绑定的Seesion.Id 用于给客户端发送消息
        /// </summary>
        public long GateSessionID { get; set; }
}
```

``` text {.line-numbers}
public sealed class UI: Entity
{
       //UI资源的GameObject
       public GameObject UIGObject;

       public string Name { get; private set; }

       public new Dictionary<string, UI> children = new Dictionary<string, UI>();
}
```

**ET的特殊全局实体 Game.Scene**
比如在服务端 Program类中添加这些组件

``` text {.line-numbers}
Game.Scene.AddComponent<UserComponent>();
Game.Scene.AddComponent<SessionKeyComponent>();

Game.Scene.AddComponent<OnlineComponent>();
Game.Scene.AddComponent<LandMatchComponent>();
```

那就可以在本服务器上的各Handler中，System类中，各组件中直接这样获取到添加给Game.Scene的组件实例

``` text {.line-numbers}
SessionKeyComponent SessionKeyComponent = Game.Scene.GetComponent<SessionKeyComponent>();

Game.Scene.GetComponent<UserComponent>().Remove(self.User.UserID);
```

**组件Component**
**给组件添加事件**，AddComponent方法默认给组件添加到awake事件系统了。
![图片](<..\ET\ET6.png>)
当然这样事件并没有任何效果，因为还没有对应的awake事件方法体。
**给组件添加扩展类，扩展方法**
给组件加一个扩展类，重写awake扩展方法，这样当组件创建时就会执行awake事件方法。
扩展类要继承AwakeSystem\<T>，当然还有其它事件StartSystem\<T>，UpdateSystem\<T>等，定义在这些接口中。

System扩展类要加[ObjectSystem]特性，这样程序集加载时就会扫描到扩展的类与事件方法
![图片](<.\ET\ET7.png>)

``` text {.line-numbers}
    [ObjectSystem]
public class UnitComponentSystem : AwakeSystem<UnitComponent>
{
    public override void Awake(UnitComponent self)
    {
        self.Awake();
    }
}
```

**同样的也可以给实体类添加扩展类、扩展方法。不举例了。**

- 组件、实体的扩展类可以与原组件、实体定义在同一个类文件。
- 也可以脱离原组件、实体单独定义扩展类，这样就可以放在运行时加载的其它程序集，比如在Hotfix程序集。

**这样就实现了：**
1 实体Entity状态与组件Componet方法的分离，可以实现同一个ET框架，部署到不同服务器上，而Program中添加不同的组件，加载不同的配置设定即可以分别作为Map，Gate，Realm服务器，发挥不同的功能作用。

2 实体Entity、组件Componet与系统事件System的分离，可以实现将组件的扩展方法放在Hotfix程序集，游戏即便在运行中，也可以热重载Hotfix.dll，在服务器上更新新的方法逻辑。

**请求处理handler，实体与组件的扩展方法，新的实体，组件都可以热重载。**
