# ET6.0实操-17-对象传送

InnerMessage.proto

```C#
//退出游戏
//ResponseType M2G_RequestExitGame
message G2M_RequestExitGame // IActorLocationRequest
{
    int32 RpcId = 90;
}


message M2G_RequestExitGame // IActorLocationResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;
}
```

```C#
//删除登录
//ResponseType L2G_RemoveLoginRecord
message G2L_RemoveLoginRecord // IActorRequest
{
    int32 RpcId       = 90;
    int64 AccountId  = 1;
    int ServerId = 2;
}

message L2G_RemoveLoginRecord // IActorResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;
}
```

Server.Hotfix\Demo\Account\Handler\G2M_RequestExitGameHandler.cs

```C#
using System;

namespace ET
{
    public class G2M_RequestExitGameHandler : AMActorLocationRpcHandler<Unit, G2M_RequestExitGame, M2G_RequestExitGame>
    {
        protected override async ETTask Run(Unit unit, G2M_RequestExitGame request, M2G_RequestExitGame response, Action reply)
        {
            //TODD 保存玩家数据到数据库，执行相关下线操作
            Log.Debug("开始下线保存玩家数据");

            reply();

            //正式释放Unit
            await unit.RemoveLocation();//通知定位游戏服务器将unit位置移除
            UnitComponent unitComponent = unit.DomainScene().GetComponent<UnitComponent>();
            unitComponent.Remove(unit.Id);

            await ETTask.CompletedTask;
        }
    }
}
```

Server.Hotfix\Demo\Account\Handler\G2L_RemoveLoginRecordHandler.cs

```C#
using System;

namespace ET
{
    public class G2L_RemoveLoginRecordHandler : AMActorRpcHandler<Scene, G2L_RemoveLoginRecord, L2G_RemoveLoginRecord>
    {
        protected override async ETTask Run(Scene scene, G2L_RemoveLoginRecord request, L2G_RemoveLoginRecord response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterLock, accountId.GetHashCode()))
            {
                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(accountId);
                if (request.ServerId == zone)
                {
                    scene.GetComponent<LoginInfoRecordComponent>().Remove(accountId);
                }
            }
            reply();
        }
    }
}
```

## 更新代码

SessionPlayerComponent.cs

```C#
namespace ET
{
    [ComponentOf(typeof(Session))]
    public class SessionPlayerComponent : Entity, IAwake, IDestroy
    {
        public long PlayerId;
        public long PlayerInstanceId;

        public long AccoutnId;
        /// <summary> 是否是再次登录 </summary>
        public bool isLoginAgain = false;
    }
}
```

SessionPlayerComponentSystem.cs

```C#


namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    public static class SessionPlayerComponentSystem
    {
        public class SessionPlayerComponentDestroySystem: DestroySystem<SessionPlayerComponent>
        {
            public override void Destroy(SessionPlayerComponent self)
            {
                //// 发送断线消息
                //ActorLocationSenderComponent.Instance.Send(self.PlayerId, new G2M_SessionDisconnect());
                //self.Domain.GetComponent<PlayerComponent>()?.Remove(self.AccoutnId);

                // 发送断线消息
                if (!self.isLoginAgain && self.PlayerInstanceId != 0)
                {
                    Player player = Game.EventSystem.Get(self.PlayerInstanceId) as Player;
                    DisconnectHelper.KickPlayer(player).Coroutine();
                }

                self.AccountId = 0;
                self.PlayerId = 0;
                self.PlayerInstanceId = 0;
                self.isLoginAgain = false;
            }
        }

        public static Player GetMyPlayer(this SessionPlayerComponent self)
        {
            return self.Domain.GetComponent<PlayerComponent>().Get(self.AccoutnId);
        }
    }
}
```

L2G_DisconnectGateUnitRequestHandler.cs

```C#
using System;

namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    public class L2G_DisconnectGateUnitRequestHandler : AMActorRpcHandler<Scene, L2G_DisconnectGateUnitRequest, G2L_DisconnectGateUnitResponse>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateUnitRequest request, G2L_DisconnectGateUnitResponse response, Action reply)
        {
            long accountId = request.AccountId;

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, accountId.GetHashCode()))
            {
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player playr = playerComponent.Get(accountId);
                if (playr == null)
                {
                    reply();
                    return;
                }
                playerComponent.Remove(accountId);

                scene.GetComponent<GateSessionKeyComponent>().Remove(accountId);
                Session gateSession = Game.EventSystem.Get(playr.SessionInstanceId) as Session;
                if (gateSession != null && !gateSession.IsDisposed)
                {
                    //新增
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
                playr.SessionInstanceId = 0;
                playr.AddComponent<PlayerOfflineOutTimeComponent>();
            }
            reply();
        }
    }
}
```

DisconnectHelper.cs 拓展

```C#
/// <summary> 玩家下线 </summary>
/// <param name="player"></param>
/// <param name="isExcption">是否是正常下线</param>
/// <returns></returns>
public static async ETTask KickPlayer(Player player, bool isExcption = false)
{
    if (player == null || player.IsDisposed) return;
    long instanceId = player.InstanceId;
    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, player.AccountId.GetHashCode()))
    {
        if (player.IsDisposed || instanceId != player.InstanceId) return;
        if (isExcption == false)
        {
            switch (player.PlayerState)
            {
                case PlayerState.Disconnect:
                    break;
                case PlayerState.Gate:
                    break;
                case PlayerState.Game:
                    //通知游戏逻辑服下线Unit角色逻辑，并将数据存入数据库
                    var m2GRequestExitGame = (M2G_RequestExitGame)await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestExitGame());
                    //通知移除账号角色登录信息
                    long LoginCenterConfigSceneId = StartSceneConfigCategory.Instance.loginCenterConfig.InstanceId;
                    var L2G_RemoveLoginRecord = (L2G_RemoveLoginRecord)await MessageHelper.CallActor(LoginCenterConfigSceneId, new G2L_RemoveLoginRecord()
                    {
                        AccountId = player.AccountId,
                        ServerId = player.DomainZone()
                    });
                    break;
                default:
                    break;
            }
        }
        player.PlayerState = PlayerState.Disconnect;
        player.DomainScene().GetComponent<PlayerComponent>()?.Remove(player.AccountId);
        player?.Dispose();
        await TimerComponent.Instance.WaitAsync(300);
    }
}
```

整个账号登录流程

![30](\../../Image/ET6.0实操-账号/30.png)

**客户端 -> 账号服务器 -> 登录中心服务器 -> Gate服务器**
客户端 -> 账号服务器:登录
账号服务器 -> 登录中心服务器:查询都是已经有账号登录
登录中心服务器 -> Gate服务器:有的话就踢玩家下线

**客户端 -> 账号服务器 -> Realm -> 账号服务器 -> 客户端 -> Realm -> 客户端 -> Gate-> Map**
客户端 -> 账号服务器:创建角色,获取区服信息,进入游戏
账号服务器 -> Realm:发送消息获取Realm令牌
Realm -> 账号服务器:返回Realm令牌
账号服务器 -> 客户端:返回Realm令牌
客户端 -> Realm:请求分配gate服务器地址,Realm也会和Gate请求地址和令牌
Realm -> 客户端:返回Gate请求地址和令牌
客户端 -> Gate:通过Gate请求地址和令牌连接Gate,创建映射Player对象,创建Unit对象
Gate-> Map:将实体传送到Map逻辑服当中,并且通知Location服务器

后续都是客户端发送消息给Map,只不过中途还是要经过Gate自动转发ActorLoaction消息
