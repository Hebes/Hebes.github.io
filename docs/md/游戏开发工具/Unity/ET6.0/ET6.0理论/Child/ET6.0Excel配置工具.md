# ET6.0Excel配置工具

**@c:** 表示只导出到客户端
**@s:** 表示只导出到服务端

- **StartMachineConfig：**
  
    启动的物理机器配置，每一条代表一台物理机，包括id，内外网地址，以及一个守护进程端口
    我们增加一个进程配置：

    ![StartMachineConfig：](\../Image/ET4.png)

    然后把所有StartSceneConfig表里面的配置改成2，这样所有服务都在2号进程启动。

    ![StartMachineConfig：](\../Image/ET5.png)

- **StartZoneConfig：**
  
    相关游戏区的配置，现在里面已经写好的配置，每一条代表一个区连接的数据库配置
- **StartProcessConfig：**
  
    启动的进程配置，当启动一个ET6.0的服务器时，会根据传入的数据读取这个配置确定服务器应该以什么样的配置运行当前进程
- **StartSceneConfig：**
  
    启动服务器时，这个表里面的配置，决定了给这个服务器添加什么类型的服务功能（暂且理解为，其中的一个场景就是一种服务，对应有：Realm服（用于认证玩家），Gate服（经过认证后所有与客户端通信），Location服（用于提供查找Actor定位服），Map服（用于管理玩家游戏实体，互相通信））。注意Process字段表示所属的进程，Zone字段表示游戏区的概念。如果这里的Process填的进程id与StartProcessConfig对不上，那么没有任何服务功能会在启动的服务器里。

**路径：ET/Excel**
配置excel规则：第二行第二列的#代表忽略c代表客户端生成，s代表服务端生成，默认代表两端都生成，同一个excel的sheet分页可以组合数据，sheet的名字前#则忽略
![配置excel规则](<\../Image/8.png>)

## 添加Vector3类型

Vector3必须引用为System.Numerics

```c#
UnitConfig config = UnitConfigCategory.Instance.Get(id);
var allUnitConfig = UnitConfigCategory.Instance.GetAll();

namespace ET
{
    //为表单添加数据类型
    public partial class UnitConfig
    {
        public System.Numerics.Vector3 TestValue;
    }
    public class TestVector3
    {
        public Vector3 TestValue;
    }
    public partial class UnitConfigCategory
    {
        public List<TestVector3> LiVector3 = new List<TestVector3();
        public override void AfterEndInit()
        {
            base.AfterEndInit();
            foreach(var config in this.dict.Value)
            {
                config.TestValue = new Vector3(config.Position, config.Height,config.Weight);
                this.LiVector3.Add(new TestVector3(){TestValue = config.TestValue});
            }
        }
        public UnitConfig GetUnitConfigByHeight(int height)
        {
            UnitConfig unitConfig = null;
            ...
        }
    }
}
```

## 添加数组类型

在excel的类型一行改为Int[],下面参数改为1,2,3各int间用逗号隔开,代码部分同上
扩展UnitConfig类添加新参数
扩展UnitConfigCategory添加读取方式

### ET6.0 读取配置档解析

### 表的读取方法

```c# {.line-numbers}
await ResourcesComponent.Instance.LoadBundleAsync("conunity3d");
Game.Scene.AddComponent<ConfigComponent>();
ConfigComponent.Instance.Load();
ResourcesComponent.Instance.UnloadBundle("config.unity3d");
//使用
Log.Error(AIConfigCategory.Instance.Get(201).Order.ToSt() + "====================================");
```

## 扩展读表的方法

**[ET7-ExcelExporter工具使用](<https://www.yuque.com/et-xd/docs/wcwbl01gr9wgvmaq>)**

**[解析ET6框架的配置表的原理和使用](https://blog.csdn.net/m0_46712616/article/details/121773072)**

**[读取配置档解析](https://www.jianshu.com/p/1bfeecd2d506)**

**[ET6.0 配置档增加List和Dictionary配置支持](https://www.jianshu.com/p/f4214c66e73c)**

**[ET框架Excel配置持久化数据类（自动生成源码）](https://blog.csdn.net/Q540670228/article/details/123429437)**

```c#
public partial class AIConfigCategory : ProtoObject
{
       //扩展方法
｝ 
```

## 备注

```C#
public static async ETTask<Scene> Create(Entity parent, long id, long instanceId, int zone, string name, SceneType sceneType, StartSceneConfig startSceneConfig = null)
        {
            await ETTask.CompletedTask;
            Scene scene = EntitySceneFactory.CreateScene(id, instanceId, zone, sceneType, name, parent);

            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);

            switch (scene.SceneType)
            {
                case SceneType.Realm:
                    scene.AddComponent<NetKcpComponent, IPEndPoint>(startSceneConfig.OuterIPPort);
                    break;
                case SceneType.Gate:
                    scene.AddComponent<NetKcpComponent, IPEndPoint>(startSceneConfig.OuterIPPort);
                    scene.AddComponent<PlayerComponent>();
                    scene.AddComponent<GateSessionKeyComponent>();
                    break;
                case SceneType.Map:
                    scene.AddComponent<UnitComponent>();
                    scene.AddComponent<RecastPathComponent>();
                    break;
                case SceneType.Location:
                    scene.AddComponent<LocationComponent>();
                    break;
            }

            return scene;
        }
```

根据传入的服务类型，startScene配置ID，InstanceId（这个很重要，它是由生成对应StartSceneConfig单条配置时，由进程号与配置ID组合成的实体ID，通过这个实体ID可以找到对应的进程IP与端口用于通信），服务区号，服务器类型等信息，创建一个服务Scene。

对Scene挂载MailBoxComponent组件，用于处理Actor请求，并且区分是否需要按顺序处理收到的请求。（备注：挂在了MailBoxComponent的Entity类就可以处理Actor消息，且可以有顺序处理的功能，Scene也是Entity的一种）

当所有服配置在一个进程中时，就相当于5.0的All Server模式了

### ET Computer生命周期

[ET Computer生命周期(Ctrl+F 生命周期)](<https://blog.csdn.net/Q540670228/article/details/123416617>)(PS:IAwake,IUpdate,IDestroy)
