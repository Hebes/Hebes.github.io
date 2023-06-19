# ET6.0机器人

通过直接调用游戏客户端逻辑层的接口实现完完全全模拟玩家的游戏操作
每一个RobotZoneScene代表一个玩家
RobotZoneScene->RobotManagerComponent->RobotScene->Game.Scene

## 例:添加新服务器

SceneType枚举中添加服务器类型,Account = 7
ET/Excel/StartSceneConfig中添加 6,1,1,Acount,Account,1007
编译Cilent-server后打开ET/win_startExcelExport.bat

服务器

```c#
//Hotfix/AppStart_init.cs
switch(scene.SceneType)
{
    case SceneType.Account:
        scene.AddComponent<NetKcpComponent,IPEndPoint,int>(...);
        break;
}
```

## ET6.0 事件系统

[ET6.0使用手册-事件系统(搜 事件系统)](<https://www.lfzxb.top/et6-manual/#awaitasync>)
ET对部分UnityEngine API进行了兼容,让用户不在View层也能引用API
兼容部分在ThirdParty/UnityEngine.Vector3中
只能扩展纯逻辑,与显示无关的

**客户端**
事件类型位置 Model/Codes/Model/Demo/EventType.cs
如果要添加unity api的,则在ModelView中对应位置创建EventType.cs

```c#
namespace ET
{
    public class InstallComputer_AddComponent:AEvent<EventType.InstallComputer>
    {
        protected override void Run(InstallComputer arg)
        {
            Computer computer = arg.Computer;
            computer.AddComponent<PCCaseComponent>();            
        }
    }   
    public class InstallComputerAsync_AddComponent:AEventAsync<EventType.InstallComputerAsync>
    {
        protected async override ETTask Run(InstallComputersync a)
        {
            Computer computer = a.Computer;
            computer.AddComponent<PCCaseComponent>();
            
            await TimerComponent
        }
    }
}
```

AppStartInitFinish中调用

```c#
//HotfixView/Codes/HotfixView/Demo/UI/UILogin
namespace ET
{
    public class AppStartInitFinish_CreateLoginUI:AEvent<EventType.AppStartInitFinish>
    {
        protected override void Run(EventType.AppStartInitFInish args)
        {
            Computer computer = args.ZoneScene.ADdChild<Computer>();
            //事件发布(类似于广播)
            Game.EventSystem.Publish(new EventType.InstallComputer(){Computer = computer});
            //异步事件发布
            Game.EventSystem.PublishAsync(new EventType.InstallComputer(){Computer = computer}).Coroutine();
        }
    }
}
```
