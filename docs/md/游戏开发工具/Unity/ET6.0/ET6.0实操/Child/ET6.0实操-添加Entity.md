# ET6.0实操-添加Entity

## 客户端

![1]( \../Image/ET6.0实操-添加Entity/1.png)

Unity.Model

添加脚本 Computer等

```C#
namespace ET
{
    public class Computer :Entity,IAwake
    {

    }
}
```

```C#
namespace ET
{
    [ComponentOf(typeof(Computer))]
    public class PCSaseComponent: Entity,IAwake
    {

    }
}
```

```C#
namespace ET
{
    [ComponentOf(typeof(Computer))]
    public class MonitorsComponent :Entity,IAwake
    {

    }
}
```

Scene脚本添加[ChildType]

![2]( \../Image/ET6.0实操-添加Entity/2.png)

编写System

![3]( \../Image/ET6.0实操-添加Entity/3.png)

```C#
namespace ET
{
    public class ComputerAwakeSystem : AwakeSystem<Computer>
    {
        public override void Awake(Computer self)
        {
            Log.Debug("ComputerAwakeSystem");
        }
    }

    public class ComputerUpdateSystem : UpdateSystem<Computer>
    {
        public override void Update(Computer self)
        {
            Log.Debug("ComputerUpdateSystem");
        }
    }

    public class ComputerDestroySystem : DestroySystem<Computer>
    {
        public override void Destroy(Computer self)
        {
            Log.Debug("ComputerDestroySystem");
        }
    }

    public static class ComputerSystem
    {
        public static void Star(this Computer self)
        {
            Log.Debug("电脑开始...");

            self.GetComponent<PCSaseComponent>().StartPower();
        }
    }
}
```

```C#
namespace ET
{
    public static class PCSaseComponentSystem
    {
        public static void StartPower(this PCSaseComponent self)
        {
            Log.Debug("电源开启");
        }
    }
}
```

```C#
namespace ET
{
    public static class MonitorsComponentSystem
    {
        public static void StartPower(this MonitorsComponent self)
        {
            Log.Debug("显示器开启");
        }
    }
}
```

启动

修改AppStartInitFinish_CreateLoginUI中的代码

```C#
namespace ET
{
    public class AppStartInitFinish_CreateLoginUI : AEvent<EventType.AppStartInitFinish>
    {
        protected override void Run(EventType.AppStartInitFinish args)
        {
            UIHelper.Create(args.ZoneScene, UIType.UILogin, UILayer.Mid).Coroutine();

            test(args.ZoneScene).Coroutine();
        }

        public async ETTask test(Scene ZoneScene)
        {
            Computer computer = ZoneScene.AddChild<Computer>();
            computer.AddComponent<PCSaseComponent>();
            computer.AddComponent<MonitorsComponent>();

            computer.Star();
            await TimerComponent.Instance.WaitAsync(3000);//等待3秒
            computer.Dispose();//销毁
        }
    }
}
```

![5]( \../Image/ET6.0实操-添加Entity/5.png)

## 服务端

共用客户端的代码

![6]( \../Image/ET6.0实操-添加Entity/6.png)

选中Computer右击->添加->现有项

进入Client(PS:客户端)的Unity.Hotfix的Computer文件夹添加下面的代码文件

![7]( \../Image/ET6.0实操-添加Entity/7.png)

选中Computer右击->添加->现有项

进入Client(PS:客户端)的Unity.Model的Computer文件夹添加下面的代码文件

重新编译客户端或者客户端设为启动项目

然后启动

进入ET的后端Log文件夹查看日志文件

![8]( \../Image/ET6.0实操-添加Entity/8.png)
