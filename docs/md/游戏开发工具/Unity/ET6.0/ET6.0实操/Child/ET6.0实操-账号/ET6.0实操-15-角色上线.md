# ET6.0实操-15-角色上线

ErrorCode.cs

```C#
/// <summary> 其他账号登录错误 </summary>
public const int ERR_OtherAccountLoginError = 200015;
```

Server.Model\Demo\Account\PlayerOfflineOutTimeComponent.cs

```C#
namespace ET
{
    [ComponentOf(typeof(Player))]
    public class PlayerOfflineOutTimeComponent:Entity,IAwake,IDestroy
    {
        public long Timer;
    }
}
```

TimerType.cs

```C#
/// <summary> 玩家离线超时时间 </summary>
public const int PlayerOfflineOutTime = 4;
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
                    //TODO 通知游戏逻辑服下线Unit角色逻辑,并将数据存入数据库
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

Server.Hotfix\Demo\Account\PlayerOfflineOutTimeComponentSystem.cs

```C#
using System;
using System.Threading;

namespace ET
{
    [Timer(TimerType.PlayerOfflineOutTime)]
    public class PlayerOfflinwOutTime : ATimer<PlayerOfflineOutTimeComponent>
    {
        public override void Run(PlayerOfflineOutTimeComponent self)
        {
            try
            {
                self.KickPlayer();
            }
            catch (Exception e )
            {
                Log.Error($"玩家超时错误: {self.Id}\n{e}");
            }
        }
    }

    public class GateUnitDeleteComponentDestroySystem : DestroySystem<PlayerOfflineOutTimeComponent>
    {
        public override void Destroy(PlayerOfflineOutTimeComponent self)
        {
            //取消定时器的任务
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }

    public class GateUnitDeleteComponentAwakeSystem : AwakeSystem<PlayerOfflineOutTimeComponent>
    {
        public override void Awake(PlayerOfflineOutTimeComponent self)
        {
            //在TimeHelper.ServerNow() + 10000后启动定时器任务就是上方的PlayerOfflinwOutTime 10秒之后启动
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 10000, TimerType.PlayerOfflineOutTime, self);
        }
    }

    public static  class PlayerOfflineOutTimeComponentSystem
    {
        public static void KickPlayer(this PlayerOfflineOutTimeComponent self)
        {
            DisconnectHelper.KickPkayer(self.GetParent<Player>()).Coroutine();
        }
    }
}
```


## 更新

L2G_DisconnectGateUnitRequestHandler.cs

```C#
using System;

namespace ET
{
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

PlayerComponentSystem.cs 修改 原来player.id -> player.Account

```C#
public static void Add(this PlayerComponent self, Player player)
{
    self.idPlayers.Add(player.AccountId, player);
}
```

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
    }
}
```

C2G_LoginGameGateHandler.cs

```C#
using System;

namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    public class C2G_LoginGameGateHandler : AMRpcHandler<C2G_LoginGameGate, G2C_LoginGameGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGameGate request, G2C_LoginGameGate response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Gate)
            {
                Log.Error($"请求的Scene错误,当前的Scene为:{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            //为保持长时间连接必须移除SessionAcceptTimeoutComponent组件
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            //防止多次点击登录
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedlyError;
                reply();//用于发送response这条消息
                return;
            }

            Scene scene = session.DomainScene();
            string tokenKey = scene.GetComponent<GateSessionKeyComponent>().Get(request.Account);
            if (tokenKey == null || !tokenKey.Equals(request.Key))
            {
                response.Error = ErrorCode.ERR_ConnectGateKeyError;
                response.Message = "Gate Key验证失败!";
                reply();
                session?.Disconnect().Coroutine();
                return;
            }

            scene.GetComponent<GateSessionKeyComponent>().Remove(request.Account);
            long instanceId = session.InstanceId;
            //防止频繁创建游戏角色
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, request.Account.GetHashCode()))
                {

                    if (instanceId != session.InstanceId) return;

                    //通知登录中心服,记录本次登录的服务器Zone
                    StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.loginCenterConfig;
                    L2G_AddLoginRecord l2ARoleLogin = (L2G_AddLoginRecord)await MessageHelper.CallActor(loginCenterConfig.InstanceId, new G2L_AddLoginRecord()
                    {
                        AccountId = request.Account,
                        ServerId = scene.Zone,
                    });


                    if (l2ARoleLogin.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = l2ARoleLogin.Error;
                        reply();
                        session?.Disconnect().Coroutine();
                        return;
                    }
                    //**********************更新
                    Player player = scene.GetComponent<PlayerComponent>().Get(request.Account);

                    if (player == null)
                    {
                        //添加一个新的GateUnit
                        player = scene.GetComponent<PlayerComponent>().AddChildWithId<Player, long, long>(request.RoleId, request.Account, request.RoleId);
                        player.PlayerState = PlayerState.Gate;
                        scene.GetComponent<PlayerComponent>().Add(player);
                        //添加MailBoxComponent组件后session这个实体就有处理Actor消息的能力
                        session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
                    }
                    else
                    {
                        //TODO角色下线
                        player.RemoveComponent<PlayerOfflineOutTimeComponent>();
                    }

                    session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;
                    session.GetComponent<SessionPlayerComponent>().PlayerInstanceId = player.InstanceId;
                    //*********这里新添加
                    session.GetComponent<SessionPlayerComponent>().AccoutnId = request.Account;
                    player.SessionInstanceId = session.InstanceId;
                }
                reply();
            }
        }
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
                // 发送断线消息
                ActorLocationSenderComponent.Instance.Send(self.PlayerId, new G2M_SessionDisconnect());
                self.Domain.GetComponent<PlayerComponent>()?.Remove(self.AccoutnId);
            }
        }

        public static Player GetMyPlayer(this SessionPlayerComponent self)
        {
            return self.Domain.GetComponent<PlayerComponent>().Get(self.AccoutnId);
        }
    }
}
```
