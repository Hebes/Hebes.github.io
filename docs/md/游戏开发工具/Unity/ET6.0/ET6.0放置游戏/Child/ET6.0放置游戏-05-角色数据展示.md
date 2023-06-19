# ET6.0放置游戏-05-角色数据展示

## 更新

Server.Hotfix\Demo\Session\SessionStreamDispatcherServerInner.cs

```C#
// 内网收到外网消息，有可能是gateUnit消息，还有可能是gate广播消息
if (OpcodeTypeComponent.Instance.IsOutrActorMessage(opcode))
{
    InstanceIdStruct instanceIdStruct = new InstanceIdStruct(actorId);
    instanceIdStruct.Process = Game.Options.Process;
    long realActorId = instanceIdStruct.ToLong();
    
    
    Entity entity = Game.EventSystem.Get(realActorId);
    if (entity == null)
    {
        type = OpcodeTypeComponent.Instance.GetType(opcode);
        message = MessageSerializeHelper.DeserializeFrom(opcode, type, memoryStream);
        Log.Error($"not found actor: {session.DomainScene().Name}  {opcode} {realActorId} {message}");
        return;
    }
    
    if (entity is Session gateSession)
    {
        // 发送给客户端
        memoryStream.Seek(Packet.OpcodeIndex, SeekOrigin.Begin);
        gateSession.Send(0, memoryStream);
        return;
    }

    if (entity is Player player)
    {
        //发送给客户端
        if (player==null|| player.IsDisposed)
            return;

        if (player.ClientSession==null||player.ClientSession.IsDisposed)
            return;

        memoryStream.Seek(Packet.OpcodeIndex, SeekOrigin.Begin);
        player.ClientSession.Send(0, memoryStream);
        return;
    }
}
```

F:\Yet\Project\ET-EUI\Server\Model\Demo\Player.cs

```C#
public long AccountId { get; set; }
public long UnitId { get; set; }
public PlayerState PlayerState { get; set; }
public Session ClientSession { get; set; }
```

F:\Yet\Project\ET-EUI\Server\Hotfix\Demo\Account\Handler\L2G_DisconnectGateUnitRequestHandler.cs

```C#
scene.GetComponent<GateSessionKeyComponent>().Remove(accountId);
//Session gateSession = Game.EventSystem.Get(playr.SessionInstanceId) as Session;
Session gateSession = playr.ClientSession;
if (gateSession != null && !gateSession.IsDisposed)
{
    if (gateSession.GetComponent<SessionPlayerComponent>() != null)
    {
        gateSession.GetComponent<SessionPlayerComponent>().isLoginAgain = true;
    }
    gateSession.Send(new A2C_Disconnect()
    {
        Error = ErrorCode.ERR_OtherAccountLoginError
    });
    gateSession?.Disconnect().Coroutine();
}
playr.AddComponent<PlayerOfflineOutTimeComponent>();
```

F:\Yet\Project\ET-EUI\Server\Hotfix\Demo\Account\Handler\C2G_LoginGameGateHandler.cs

```C#
session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;
session.GetComponent<SessionPlayerComponent>().PlayerInstanceId = player.InstanceId;
session.GetComponent<SessionPlayerComponent>().AccountId = request.Account;
//player.SessionInstanceId = session.InstanceId;
player.ClientSession = session;
```

F:\Yet\Project\ET-EUI\Server\Hotfix\Demo\Unit\UnitHelper.cs

把不需要的都注释了,放置类游戏不需要

```C#
//MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
//if (moveComponent != null)
//{
//    if (!moveComponent.IsArrived())
//    {
//        unitInfo.MoveInfo = new MoveInfo();
//        for (int i = moveComponent.N; i < moveComponent.Targets.Count; ++i)
//        {
//            Vector3 pos = moveComponent.Targets[i];
//            unitInfo.MoveInfo.X.Add(pos.x);
//            unitInfo.MoveInfo.Y.Add(pos.y);
//            unitInfo.MoveInfo.Z.Add(pos.z);
//        }
//    }
//}
```

F:\Yet\Project\ET-EUI\Server\Hotfix\Demo\M2M_UnitTransferRequestHandler.cs

```C#
//unit.AddComponent<MoveComponent>();
//unit.AddComponent<PathfindingComponent, string>(scene.Name);
//unit.Position = new Vector3(-10, 0, -10);
```

```C#
// 加入aoi
//unit.AddComponent<AOIEntity, int, Vector3>(9 * 1000, unit.Position);
```

F:\Yet\Project\ET-EUI\Unity\Codes\Hotfix\Demo\Unit\UnitFactory.cs

```C#
//unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
//unit.Forward = new Vector3(unitInfo.ForwardX, unitInfo.ForwardY, unitInfo.ForwardZ);
```

```C#
//unit.AddComponent<MoveComponent>();
//if (unitInfo.MoveInfo != null)
//{
    // if (unitInfo.MoveInfo.X.Count > 0)
    // {
      //  using (ListComponent<Vector3> list = ListComponent<Vector3>.Create())
      //  {
        //   list.Add(unit.Position);
        //   for (int i = 0; i < unitInfo.MoveInfo.X.Count; ++i)
        //   {
           //    list.Add(new Vector3(unitInfo.MoveInfo.X[i], unitInfo.MoveInfo.Y[i], unitInfo.MoveInfo.Z[i]));
        //   }
        //   unit.MoveToAsync(list).Coroutine();
       //  }
    // }
//}
```

```C#
//unit.AddComponent<XunLuoPathComponent>();
```

F:\Yet\Project\ET-EUI\Unity\Codes\HotfixView\Demo\Unit\AfterUnitCreate_CreateUnitView.cs

```C#
using UnityEngine;

namespace ET
{

    [FriendClass(typeof(GlobalComponent))]
    public class AfterUnitCreate_CreateUnitView: AEventAsync<EventType.AfterUnitCreate>
    {
        protected override async ETTask Run(EventType.AfterUnitCreate args)
        {
            // Unit View层
            // 这里可以改成异步加载，demo就不搞了
            await ResourcesComponent.Instance.LoadBundleAsync("Knight.unity3d");
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset("Knight.unity3d", "Unit");
            GameObject go = UnityEngine.Object.Instantiate(bundleGameObject, GlobalComponent.Instance.Unit, true);
            args.Unit.AddComponent<GameObjectComponent>().GameObject = go;
            args.Unit.AddComponent<AnimatorComponent>();
            args.Unit.Position = Vector3.zero;
            await ETTask.CompletedTask;
        }
    }
}
```

F:\Yet\Project\ET-EUI\Unity\Codes\HotfixView\Demo\UI\UIHelp\SceneChangeFinishEvent_CreateUIHelp.cs

```C#
protected override async ETTask Run(EventType.SceneChangeFinish args)
{
    args.CurrentScene.GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Loading);
    await ETTask.CompletedTask;
}
```

F:\Yet\Project\ET-EUI\Unity\Codes\Hotfix\Demo\Login\LoginHelper.cs

```C#
Log.Debug("角色进入游戏成功!!");
zoneScene.GetComponent<PlayerComponent>().MyId = g2C_EnterGame.MyId;
// 等待场景切换完成
await zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_SceneChangeFinish>();
return ErrorCode.ERR_Success;
```
