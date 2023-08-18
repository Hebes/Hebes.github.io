# ET6.0实操-事件系统

可以参考demo里面的AppStartInitFinish_CreateLoginUI.cs等

## 同步事件

```C#
// 定义一个事件结构体
public struct AppStart
{
    public string Type;
}

// 定义一个类，用于处理指定事件，泛型类型为我们想要订阅的事件
public class AppStart_Init: AEvent<EventType.AppStart>
{
    protected override async ETTask Run(EventType.AppStart args)
    {
        Log.Info($"AppStart事件触发了 : {args.Type}");
        //这个操作也很常用，比如虽然这个函数是async的，但是我们内部没有异步操作，就可以这样调用，相当于直接return
        await ETTask.CompletedTask;
    }
}

// 抛出事件
Game.EventSystem.Publish(new EventType.AppStart(){Type = "游戏开始"});

------------------------------
AppStart事件触发了
```

## 异步事件

AEvent -> AEventAsync
