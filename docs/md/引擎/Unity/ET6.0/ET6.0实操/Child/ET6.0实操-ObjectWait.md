# ET6.0实操-ObjectWait

AfterUnitCreate_CreateUnitView中如果角色是自己添加的刚体和碰撞体，会遇到场景没有加载完毕，角色会先创建出来,然后会掉下去,通过ObjectWait解决

WaitType.cs

```C#
public struct Wait_CreateMyUnitFinish : IWaitType
{
    public int Error
    {
        get;
        set;
    }
}
```

SceneChangeStart_AddComponent.cs

```C#
public class SceneChangeStart_AddComponent : AEvent<EventType.SceneChangeStart>
{
    protected override void Run(EventType.SceneChangeStart args)
    {
        RunAsync(args).Coroutine();
    }

    private async ETTask RunAsync(EventType.SceneChangeStart args)
    {
        Scene currentScene = args.zoneScene.CurrentScene();
        //创建Loading
        UIHelper.Create(args.zoneScene, UIType.UIMateLoading, UILayer.High).Coroutine();
        ResourcesComponent.Instance.LoadBundle("loading.unity3d");
        SceneManager.LoadSceneAsync("loading");
        // 加载场景资源
        await ResourcesComponent.Instance.LoadBundleAsync($"{currentScene.Name}.unity3d");
        // 切换到map场景
        SceneChangeComponent sceneChangeComponent = null;
        try
        {
            sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>();
            {
                await sceneChangeComponent.ChangeSceneAsync(args);
            }
        }
        finally
        {
            sceneChangeComponent?.Dispose();
        }
        //************************************************************************************
        //这里添加了通知 意思是上面的代码，场景加载完毕后在通知这个等待类型的awit方法可以继续下面的代码
        //Notify==通知
        args.zoneScene.GetComponent<ObjectWait>().Notify(new WaitType.Wait_CreateMyUnitFinish());
        //************************************************************************************
    }
}
```

AfterUnitCreate_CreateUnitView.cs

```C#
public class AfterUnitCreate_CreateUnitView : AEvent<EventType.AfterUnitCreate>
{
    protected override async void Run(EventType.AfterUnitCreate args)
    {
        //************************************************************************************
        //这里就是等待Notify的通知，有了Notify的通知才会继续执行下面的代码
        //Wait==等待
        await args.CurrentScene.Parent.ZoneScene().GetComponent<ObjectWait>().Wait<WaitType.Wait_CreateMyUnitFinish>();
        //************************************************************************************
        //这里就是下面的代码
        //等场景加载完毕再去查找物体
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        //Todo 其他代码,比如生成玩家
        await ETTask.CompletedTask;
    }
}
```

场景切换的主要地方

SceneChangeHelper.cs

```C#
ublic static class SceneChangeHelper
{
    // 场景切换
    public static async ETTask SceneChangeTo(Scene zoneScene, string sceneName, long sceneInstanceId)
    {
        CurrentScenesComponent currentScenesComponent = zoneScene.GetComponent<CurrentScenesComponent>();
        currentScenesComponent.Scene?.Dispose(); // 删除之前的CurrentScene，创建新的
        //创建一个当前场景
        Scene currentScene = SceneFactory.CreateCurrentScene(sceneInstanceId, zoneScene.Zone, sceneName, currentScenesComponent);
        UnitComponent unitComponent = currentScene.AddComponent<UnitComponent>();//添加了单元组件
        //自己添加的组件
        currentScene.AddComponent<UnitCreatObjManageComponent>();//currentScene添加组件
        // 可以订阅这个事件中创建Loading界面
        Game.EventSystem.Publish(new EventType.SceneChangeStart() { zoneScene = zoneScene, unitComponent = unitComponent });//顺便载AB包
        // 等待CreateMyUnit的消息
        WaitType.Wait_CreateMyUnit waitCreateMyUnit = await zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_CreateMyUnit>();
        //创建我的我Unity单元，IActorMessage消息不需要返回结果的Actor消息，内网通信
        M2C_CreateMyUnit m2CCreateMyUnit = waitCreateMyUnit.Message;
        Log.Debug("当前场景名称是" + currentScene.Name);
        Unit unit = UnitFactory.Create( currentScene, m2CCreateMyUnit.Unit);
        unitComponent.Add(unit);
        //在SceneChangeFinish里面创建UI界面
        Game.EventSystem.Publish(new EventType.SceneChangeFinish() 
        { 
            zoneScene = zoneScene, 
        });
        // 通知等待场景切换的协程
        zoneScene.GetComponent<ObjectWait>().Notify(new WaitType.Wait_SceneChangeFinish());
        await ETTask.CompletedTask;
    }
}
```
