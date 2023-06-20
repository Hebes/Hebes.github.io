# ET6.0实操-AwaitAsync

async ETVoid
async ETTask

await Task.Delay(500);//等待500毫秒

```C#
//虽然无名氏是先被制造的，但是是李逍遥先可以自由活动的
public static async ETVoid StartAsync()
{
    HumanEntity human1 = EntityFactory.Create<HumanEntity>(Game.Scene, true);
    human1.AddComponent<HeadComponent>();
    human1.AddComponent<BodyComponent>();
    //等待1000ms，但是并不会阻塞在这里，而是立即执行外面将要执行的函数，也就是StartAsync2
    await TimerComponent.Instance.WaitAsync(1000);
    Log.Info("无名氏可以自由活动啦");
}
public static void Start()
{
    HumanEntity human2 = EntityFactory.Create<HumanEntity, string>(Game.Scene, "Li XiaoYao", true);
    human2.AddComponent<HeadComponent>();
    human2.AddComponent<BodyComponent>();
    Log.Info("李逍遥可以自由活动啦");
}

public static void WorldStart()
{
    StartAsync().Coroutine();
    Start();
}

调用堆栈为
-----------------------------------------------
WorldStart->StartAsync->await TimerComponent.Instance.WaitAsync->Start --(等待1000ms)--> Log.Info("无名氏可以自由活动啦");
--------------------------------------------------
李逍遥可以自由活动啦
无名氏可以自由活动啦
```

```C#
public static async ETTask StartAsync()
{
    HumanEntity human1 = EntityFactory.Create<HumanEntity>(Game.Scene, true);
    human1.AddComponent<HeadComponent>();
    human1.AddComponent<BodyComponent>();
    await TimerComponent.Instance.WaitAsync(1000);
    Log.Info("无名氏可以自由活动啦");
}
public static void Start()
{
    HumanEntity human2 = EntityFactory.Create<HumanEntity, string>(Game.Scene, "Li XiaoYao", true);
    human2.AddComponent<HeadComponent>();
    human2.AddComponent<BodyComponent>();
    Log.Info("李逍遥可以自由活动啦");
}

public static async ETVoid WorldStart()
{
    await StartAsync();
    Start();
}
调用堆栈为
-----------------------------------------------
WorldStart->StartAsync->await TimerComponent.Instance.WaitAsync--(等待1000ms)-->Log.Info("无名氏可以自由活动啦")->Start->Log.Info("李逍遥可以自由活动啦");
--------------------------------------------------
无名氏可以自由活动啦
李逍遥可以自由活动啦
```

到这里就可以总结一下了，如果一个异步方法是async ETVoid的，那么他就是不可被等待的，会直接执行外部后面的方法，如果一个异步方法是async ETTask的，那么他就是可以被等待的，只有执行完它才会执行外部后面的方法
但是同样需要记住，ET是单线程的，这里的等待其实就是一个计时器。
在第一个例子里，我们把这个时间戳加入计时器组件后，才跳出的StartAsync堆栈，并没有新开辟线程，也没用什么魔法
在第二个例子里，我们把这个时间戳加入计时器组件后，由于是async ETTask，所以跳不出去，只能老实等待1000ms

返回带有结果的

```C#
 public async ETTask<int> testAsync()
{
    await TimerComponent.Instance.WaitAsync(2000);
    await ETTask.CompletedTask;
    return 1;
}
```
