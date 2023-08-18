# ET6.0实操-14-角色上线

OuterMessage.proto

```C#
//连接Gate服务器
//ResponseType G2C_LoginGameGate
message C2G_LoginGameGate // IRequest
{
    int32 RpcId = 90;
    string Key = 1;
    int64 RoleId = 2;
    int64 Account = 3;
}

message G2C_LoginGameGate // IResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;

    int64 PlayerId = 1; 
}
```

InnerMessage.proto

```C#
// 登录中心服,添加登录信息
//ResponseType L2G_AddLoginRecord
message G2L_AddLoginRecord // IActorRequest
{
    int32 RpcId = 90;
    int64 AccountId = 1;
    int32 ServerId = 2;
}

message L2G_AddLoginRecord // IActorResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;
}
```

CoroutineLockType.cs

```C#
public const int LoginCenterLock = 15;//登录中心服锁
```

StartSceneConfigCategory.cs

```C#
public StartSceneConfig LocationConfig;

//************************************登录中心服配置
public StartSceneConfig loginCenterConfig;

public List<StartSceneConfig> Robots = new List<StartSceneConfig>();
```

```C#
 case SceneType.Robot:
     this.Robots.Add(startSceneConfig);
     break;
     //************************************
 case SceneType.Realm:
     this.Realms.Add(startSceneConfig.Zone, startSceneConfig);
     break;
 //************************************登录中心服
 case SceneType.LoginCenter:
     this.loginCenterConfig = startSceneConfig;
     break;
```

Server.Hotfix -> Demo -> Account -> Handler -> G2L_AddLoginRecordHandler.cs

```C#
using System;

namespace ET
{
    public class G2L_AddLoginRecordHandler : AMActorRpcHandler<Scene, G2L_AddLoginRecord, L2G_AddLoginRecord>
    {
        protected override async ETTask Run(Scene scene, G2L_AddLoginRecord request, L2G_AddLoginRecord response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterLock,accountId.GetHashCode()))
            {
                scene.GetComponent<LoginInfoRecordComponent>().Remove(accountId);
                scene.GetComponent<LoginInfoRecordComponent>().Add(accountId,request.ServerId);
            }
            reply();
        }
    }
}
```

Server.Model -> Demo -> Player.cs

```C#
namespace ET
{
    public enum PlayerState
    {
        /// <summary> 断开状态 </summary>
        Disconnect,
        /// <summary> 连接Gate网关的装填 </summary>
        Gate,
        /// <summary> 进入游戏的状态 </summary>
        Game,
    }

    public sealed class Player : Entity, IAwake<string>, IAwake<long, long>
    {
        //public string Account { get; set; }

        public long UnitId { get; set; }

        public long AccountId { get; set; }

        public long SessionInstanceId { get; set; }

        public PlayerState PlayerState { get; set; }
    }
}
```

PlayerSystem.cs

```C#
namespace ET
{
    [FriendClass(typeof(Player))]
    public static class PlayerSystem
    {
        public class PlayerAwakeSystem : AwakeSystem<Player, long, long>
        {
            public override void Awake(Player self, long a, long roleId)
            {
                self.AccountId = a;
                self.UnitId = roleId;
            }
        }
    }
}
```

Server.Model -> Demo -> SessionPlayerComponent.cs

```C#
namespace ET
{
    [ComponentOf(typeof(Session))]
    public class SessionPlayerComponent : Entity, IAwake, IDestroy
    {
        public long PlayerId;
        public long PlayerInstanceId;
    }
}
```

## 更新内容

LoginHelper.cs 拓展

```C#
 /// <summary> 登录游戏 </summary>
public static async ETTask<int> EnterGame(Scene zoneScene)
{
    string realmAddress = zoneScene.GetComponent<AccountInfoComponent>().RealmAddress;
    //1. 连接Realm,获取分配的Gate
    R2C_LoginRealm r2C_LoginRealm;
    Session session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(realmAddress));
    try
    {
        r2C_LoginRealm = (R2C_LoginRealm)await session.Call(new C2R_LoginRealm()
        {
            AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
            RealmTokenKey = zoneScene.GetComponent<AccountInfoComponent>().RealmKey,
        });
    }
    catch (Exception e)
    {
        Log.Error(e.ToString());
        session?.Dispose();
        return ErrorCode.ERR_NetWorkError;
    }
    session?.Dispose();
    if (r2C_LoginRealm.Error != ErrorCode.ERR_Success)
    {
        Log.Error(r2C_LoginRealm.Error.ToString());
        return r2C_LoginRealm.Error;
    }
    Log.Warning($"GateAddress : {r2C_LoginRealm.GateAddress}");
    Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(r2C_LoginRealm.GateAddress));
    gateSession.AddComponent<PingComponent>();
    zoneScene.GetComponent<SessionComponent>().Session = gateSession;

    //2. 开始连接Gate
    G2C_LoginGameGate g2C_LoginGameGate = null;
    try
    {
        g2C_LoginGameGate = (G2C_LoginGameGate)await gateSession.Call(new C2G_LoginGameGate()
        {
            Key = r2C_LoginRealm.GateSessionKey,
            Account = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
            RoleId = zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId,
        });
    }
    catch (Exception e)
    {
        Log.Error(e.ToString());
        zoneScene.GetComponent<SessionComponent>().Session?.Dispose();
        return ErrorCode.ERR_NetWorkError;
    }
    
    if (g2C_LoginGameGate.Error != ErrorCode.ERR_Success)
    {
        zoneScene.GetComponent<SessionComponent>().Session?.Dispose();
        return g2C_LoginGameGate.Error;
    }
    Log.Debug("登录Gate成功!");
    return ErrorCode.ERR_Success;
}
```

Server.Hotfix -> Demo -> Account -> Handler -> C2G_LoginGameGateHandler.cs

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

                    Player player = scene.GetComponent<PlayerComponent>().Get(request.RoleId);

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
                        //player.RemoveComponent<PlayerOfflineOutTimeComponent>();
                    }

                    session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;
                    session.GetComponent<SessionPlayerComponent>().PlayerInstanceId = player.InstanceId;
                    player.SessionInstanceId = session.InstanceId;
                }
                reply();
            }
        }
    }
}
```
