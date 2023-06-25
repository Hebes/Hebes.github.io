# ET6.0实操-Actor消息

![6](\../Image/ET6.0实操-Actor消息/6.png)

![1](\../Image/ET6.0实操-Actor消息/1.png)

协议消息的定义,客户端对服务器的定义在OuterMessage.proto

```proto
//Actor消息
//会被ET框架认为是从
//客户端经过Gate网关服务区转发到Map游戏服务器进程上
//的unit对象进行消息的处理和回复
//这条是请求消息
//ResponseType M2C_TestActorLoactionResponse
message C2M_TestActorLoactionRequest // IActorLocationRequest
{
    int32 RpcId = 90; // 必须90开始
    //具体回复内容从1开始
    string Content = 1;
}

//回复消息
message M2C_TestActorLoactionResponse // IActorLocationResponse
{
    //回复的消息前面的这三个变量必须存在
    int32 RpcId = 90; // 必须90开始
    int32 Error = 91; // 必须91开始
    string Message = 92; // 必须92开始
    //具体回复内容从1开始
    string Content = 1;
}

//游戏客户端经过Gate发送到Map服务器的unit对象的不需要回复的消息
message C2M_TestActorLoactionMessage // IActorLocationMessage
{
    //IActorLocationMessage 必须要有RpcId
    int32 RpcId = 90; // 必须90开始
    //具体回复内容从1开始
    string Content = 1;
}

//Map服务器的unit对象经过Gate网关服务器主动发送消息到客户端的消息
message M2C_TestActorMessage // IActorMessage
{
    //具体回复内容从1开始
    string Content = 1;
}
```

UILoginComponentSystem.cs 还原代码

```C#
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class UILoginComponentAwakeSystem : AwakeSystem<UILoginComponent>
    {
        public override void Awake(UILoginComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.loginBtn = rc.Get<GameObject>("LoginBtn");

            self.loginBtn.GetComponent<Button>().onClick.AddListener(()=> { self.OnLogin(); });
            self.account = rc.Get<GameObject>("Account");
            self.password = rc.Get<GameObject>("Password");
        }
    }

    [FriendClass(typeof(UILoginComponent))]
    public static class UILoginComponentSystem
    {
        public static void OnLogin(this UILoginComponent self)
        {
            //*************************
            //还原代码
            LoginHelper.Login(
                self.DomainScene(),
                ConstValue.LoginAddress,
                self.account.GetComponent<InputField>().text,
                self.password.GetComponent<InputField>().text).Coroutine(); 
            //LoginHelper.Logintest(
            //    self.DomainScene(),
            //    ConstValue.LoginAddress
            //    ).Coroutine();
        }
    }
}
```

M2M_UnitTransferRequestHandler.cs

里面的代码有创建unit

M2C_CreateMyUnitHandler.cs

通知场景继续走  Wait_CreateMyUnit找到等待的地方

SceneChangeHelper.cs

 发送 ActorLoactionRequest 消息 这个消息是需要回复的 过程是 客户端->经过Gate->Map

例子是下面的  实际操作继续往下看

```C#
 Session session = zoneScene.GetComponent<SessionComponent>().Session;
var c2M_TestActorLoactionRequest = (M2C_TestActorLoactionResponse)await session.CallC2M_TestActorLoactionRequest() { Content = "Actor 客户端经过Gate网关转发到了Map服务器" });
Log.Warning(c2M_TestActorLoactionRequest.Content);
```

发送 ActorLoactionMessage 消息 不需要回复消息

```C#
session.Send(new C2M_TestActorLoactionMessage() { Content = "Actor 客户端经过Gate网关转发到了Map服务器 不用回复消息" });
```

实际操作

```C#
namespace ET
{
    public static class SceneChangeHelper
    {
        // 场景切换协程
        public static async ETTask SceneChangeTo(Scene zoneScene, string sceneName, long sceneInstanceId)
        {
            zoneScene.RemoveComponent<AIComponent>();

            CurrentScenesComponent currentScenesComponent = zoneScene.GetComponent<CurrentScenesComponent>();
            currentScenesComponent.Scene?.Dispose(); // 删除之前的CurrentScene，创建新的
            Scene currentScene = SceneFactory.CreateCurrentScene(sceneInstanceId, zoneScene.Zone, sceneName, currentScenesComponent);
            UnitComponent unitComponent = currentScene.AddComponent<UnitComponent>();

            // 可以订阅这个事件中创建Loading界面
            Game.EventSystem.Publish(new EventType.SceneChangeStart() { ZoneScene = zoneScene });

            // 等待CreateMyUnit的消息
            WaitType.Wait_CreateMyUnit waitCreateMyUnit = await zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_CreateMyUnit>();
            M2C_CreateMyUnit m2CCreateMyUnit = waitCreateMyUnit.Message;
            Unit unit = UnitFactory.Create(currentScene, m2CCreateMyUnit.Unit);
            unitComponent.Add(unit);

            zoneScene.RemoveComponent<AIComponent>();

            Game.EventSystem.Publish(new EventType.SceneChangeFinish() { ZoneScene = zoneScene, CurrentScene = currentScene });


            //*************************这里是新添加测试Acotr代码
            try
            {
                Session session = zoneScene.GetComponent<SessionComponent>().Session;
                var c2M_TestActorLoactionRequest = (M2C_TestActorLoactionResponse)await session.Call(new C2M_TestActorLoactionRequest() { Content = "Actor 客户端经过Gate网关转发到了Map服务器" });
                Log.Warning(c2M_TestActorLoactionRequest.Content);

                session.Send(new C2M_TestActorLoactionMessage() { Content = "Actor 客户端经过Gate网关转发到了Map服务器 不用回复消息" });
            }
            catch (System.Exception)
            {

                throw;
            }
            //*************************

            // 通知等待场景切换的协程
            zoneScene.GetComponent<ObjectWait>().Notify(new WaitType.Wait_SceneChangeFinish());
        }
    }
}
```

收到需要回复的Actor消息 返回ActorLoactionResponse消息

![2](\../Image/ET6.0实操-Actor消息/2.png)

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ActorMessageHandler]
    public class C2M_TestActorLoactionRequestHandler : AMActorLocationRpcHandler<Unit, C2M_TestActorLoactionRequest, M2C_TestActorLoactionResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_TestActorLoactionRequest request, M2C_TestActorLoactionResponse response, Action reply)
        {
            Log.Debug("接受服务端的消息,内容为:  " + request.Content);
            response.Content = " 这条是 Actor 服务端经过Gate发往客户端的消息";
            reply();
            await ETTask.CompletedTask;
        }
    }
}
```

收到不需要回复的ActorMessage消息

游戏客户端经过Gate发送到Map服务器的unit对象的不需要回复的消息

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ActorMessageHandler]
    public class C2M_TestActorLoactionMessageHandler : AMActorLocationHandler<Unit, C2M_TestActorLoactionMessage>
    {
        protected override async ETTask Run(Unit unit, C2M_TestActorLoactionMessage message)
        {
            Log.Debug("从客户端经过Gate网关发送到Map接受到的消息是: " + message.Content);

            MessageHelper.SendToClient(unit,new M2C_TestActorMessage() {Content="Map经过网关发送给服务端的消息,不用回复" });

            await ETTask.CompletedTask;
        }
    }
}
```

Map服务器的unit对象经过Gate网关服务器主动发送消息到客户端的消息

发送代码

```C#
 MessageHelper.SendToClient(unit,new M2C_TestActorMessage() {Content="Map经过网关发送给服务端的消息,不用回复" });
```

接受代码在

![3](\../Image/ET6.0实操-Actor消息/3.png)

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [MessageHandler]
    public class M2C_TestActorMessageHandler : AMHandler<M2C_TestActorMessage>
    {
        protected override void Run(Session session, M2C_TestActorMessage message)
        {
            Log.Debug("Actor 接受消息 Map->gate->客户端,内容是: " + message.Content);
        }
    }
}
```

![4](\../Image/ET6.0实操-Actor消息/4.png)

![5](\../Image/ET6.0实操-Actor消息/5.png)

解决上面图片报错方法

修改

```C#
using UnityEngine;

namespace ET
{
    public class AfterUnitCreate_CreateUnitView: AEvent<EventType.AfterUnitCreate>
    {
        protected override void Run(EventType.AfterUnitCreate args)
        {
            //**************************
            args.Unit.UnitType = UnitType.Player;//修改这里.可以生成小骷髅
            //**************************
            
            //创建不同的物体
            switch (args.Unit.UnitType)
            {
                case UnitType.Player:
                    // Unit View层
                    // 这里可以改成异步加载，demo就不搞了
                    GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset("Unit.unity3d", "Unit");
                    GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");

                    GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
                    go.transform.position = args.Unit.Position;
                    args.Unit.AddComponent<GameObjectComponent>().GameObject = go;
                    args.Unit.AddComponent<AnimatorComponent>();
                    break;
                case UnitType.Monster:
                    break;
                case UnitType.NPC:
                    // Unit View层
                    // 这里可以改成异步加载，demo就不搞了
                    GameObject bundleGameObject1 = (GameObject)ResourcesComponent.Instance.GetAsset("Unit.unity3d", "Unit");
                    GameObject prefab1 = bundleGameObject1.Get<GameObject>("Skeleton");

                    GameObject go1 = UnityEngine.Object.Instantiate(prefab1, GlobalComponent.Instance.Unit, true);
                    go1.transform.position = args.Unit.Position;
                    args.Unit.AddComponent<GameObjectComponent>().GameObject = go1;
                    args.Unit.AddComponent<AnimatorComponent>();
                    break;
                case UnitType.DropIten:
                    break;
                case UnitType.Box:
                    break;
                default:
                    break;
            }
        }
    }
}
```
