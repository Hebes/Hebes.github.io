# ET6.0实操-16-角色上线

OuterMessage.proto

```C#
//角色正式请求进入游戏逻辑服
//ResponseType G2C_EnterGame
message C2G_EnterGame // IRequest
{
    int32 RpcId = 90;
}

message G2C_EnterGame // IResponse
{
    int32 RpcId = 1;
    int32 Error = 2;
    string Message = 3;
    // 自己的unitId
    int64 MyId = 4; 
}
```

InnerMessage.proto

```C#
// 玩家请求进入游戏状态进入游戏逻辑服
//ResponseType M2G_RequestEnterGameState
message G2M_RequestEnterGameState // IActorLocationRequest
{
    int32 RpcId = 90;
}

message M2G_RequestEnterGameState // IActorLocationResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;
}
```

ErrorCode.cs

```C#
/// <summary> 会话玩家错误 </summary>
public const int ERR_SessionPlayerError = 200016;
/// <summary> 没有玩家错误 </summary>
public const int ERR_NonePlayerError = 200017;
/// <summary> 玩家会话错误 </summary>
public const int ERR_PlayerSessionError = 200018;
/// <summary> 会话状态错误 </summary>
public const int ERR_SessionStateError = 200019;
/// <summary> 进入游戏错误 </summary>
public const int ERR_EnterGameError = 200020;
/// <summary> 重新进入游戏失败 </summary>
public const int ERR_ReEnterGameError = 200021;
/// <summary> 重新进入游戏失败2 </summary>
public const int ERR_ReEnterGameError2 = 200022;
```

Server.Hotfix\Demo\Account\Handler\G2M_RequestEnterGameStateHandler.cs

```C#
using System;

namespace ET
{
    public class G2M_RequestEnterGameStateHandler : AMActorLocationRpcHandler<Unit, G2M_RequestEnterGameState, M2G_RequestEnterGameState>
    {
        protected override async ETTask Run(Unit unit, G2M_RequestEnterGameState request, M2G_RequestEnterGameState response, Action reply)
        {
            reply();

            await ETTask.CompletedTask;
        }
    }
}
```

Server.Model\Demo\Account\SessionStateComponent.cs

```C#
namespace ET
{
    public enum SessionState
    {
        Normal,
        Game,
    }

    [ComponentOf(typeof(Session))]
    public class SessionStateComponent:Entity,IAwake
    {
        public SessionState State { get; set; }
    }
}
```

## 更新代码

LoginHelper.cs 拓展 这次是完整版

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
    //3. 角色正式请求进入游戏逻辑服
    G2C_EnterGame g2C_EnterGame = null;
    try
    {
        g2C_EnterGame = (G2C_EnterGame)await gateSession.Call(new C2G_EnterGame() { });
    }
    catch (Exception e)
    {
        Log.Error(e.ToString());
        zoneScene.GetComponent<SessionComponent>().Session?.Dispose();
        return ErrorCode.ERR_NetWorkError;
    }

    if (g2C_EnterGame.Error != ErrorCode.ERR_Success)
    {
        Log.Error(g2C_EnterGame.Error.ToString());
        return g2C_EnterGame.Error;
    }
    Log.Debug("角色进入游戏成功!!");

    return ErrorCode.ERR_Success;
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
                    //*******************新增
                    SessionStateComponent sessionStateComponent = session.GetComponent<SessionStateComponent>();
                    if (sessionStateComponent == null)
                    {
                        sessionStateComponent = session.AddComponent<SessionStateComponent>();
                    }
                    sessionStateComponent.State = SessionState.Normal;

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

C2G_EnterGameHandler.cs

```C#
using System;

namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    [FriendClass(typeof(GateMapComponent))]
    public class C2G_EnterGameHandler : AMRpcHandler<C2G_EnterGame, G2C_EnterGame>
    {
        protected override async  ETTask Run(Session session, C2G_EnterGame request, G2C_EnterGame response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Gate)
            {
                Log.Error($"请求的Scene错误，当前Scene为：{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedlyError;
                reply();
                return;
            }

            SessionPlayerComponent sessionPlayerComponent = session.GetComponent<SessionPlayerComponent>();
            if (null == sessionPlayerComponent)
            {
                response.Error = ErrorCode.ERR_SessionPlayerError;
                reply();
                return;
            }

            Player player = Game.EventSystem.Get(sessionPlayerComponent.PlayerInstanceId) as Player;

            if (player == null || player.IsDisposed)
            {
                response.Error = ErrorCode.ERR_NonePlayerError;
                reply();
                return;
            }

            long instanceId = session.InstanceId;

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, player.AccountId.GetHashCode()))
                {

                    if (instanceId != session.InstanceId || player.IsDisposed)
                    {
                        response.Error = ErrorCode.ERR_PlayerSessionError;
                        reply();
                        return;
                    }

                    //这个是针对Session做的判定
                    if (session.GetComponent<SessionStateComponent>() != null && session.GetComponent<SessionStateComponent>().State == SessionState.Game)
                    {
                        response.Error = ErrorCode.ERR_SessionStateError;
                        reply();
                        return;
                    }

                    //对玩家做的判定
                    if (player.PlayerState == PlayerState.Game)
                    {
                        try
                        {
                            IActorResponse reqEnter = await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestEnterGameState());
                            if (reqEnter.Error == ErrorCode.ERR_Success)
                            {
                                reply();
                                return;
                            }
                            Log.Error("二次登录失败  " + reqEnter.Error + " | " + reqEnter.Message);
                            response.Error = ErrorCode.ERR_ReEnterGameError;
                            await DisconnectHelper.KickPlayer(player, true);
                            reply();
                            session?.Disconnect().Coroutine();
                        }
                        catch (Exception e)
                        {
                            Log.Error("二次登录失败  " + e.ToString());
                            response.Error = ErrorCode.ERR_ReEnterGameError2;
                            await DisconnectHelper.KickPlayer(player, true);
                            reply();
                            session?.Disconnect().Coroutine();
                            throw;
                        }
                        return;
                    }

                    try
                    {

                        GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
                        gateMapComponent.Scene = await SceneFactory.Create(gateMapComponent, "GateMap", SceneType.Map);

                        //unit简单理解为抽象的游戏角色实体
                        Unit unit = UnitFactory.Create(gateMapComponent.Scene, player.Id, UnitType.Player);
                        unit.AddComponent<UnitGateComponent, long>(session.InstanceId);
                        long unitId = unit.Id;

                        //区服IDsession.DomainZone()
                        StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "Map1");
                        //将角色传送到游戏逻辑服当中,unit对象将不在Gate服当中而是在游戏逻辑服务器之中
                        await TransferHelper.Transfer(unit, startSceneConfig.InstanceId, startSceneConfig.Name);


                        player.UnitId = unitId;
                        response.MyId = unitId;

                        reply();

                        SessionStateComponent SessionStateComponent = session.GetComponent<SessionStateComponent>();
                        if (SessionStateComponent == null)
                        {
                            SessionStateComponent = session.AddComponent<SessionStateComponent>();
                        }
                        SessionStateComponent.State = SessionState.Game;

                        player.PlayerState = PlayerState.Game;
                    }
                    catch (Exception e)
                    {

                        Log.Error($"角色进入游戏逻辑服出现问题 账号Id: {player.AccountId}  角色Id: {player.Id}   异常信息： {e.ToString()}");
                        response.Error = ErrorCode.ERR_EnterGameError;
                        reply();
                        await DisconnectHelper.KickPlayer(player, true);
                        session.Disconnect().Coroutine();

                    }
                }
            }


        }
    }
}
```

DlgRoleSystem.cs

```C#
/// <summary> 开始游戏 </summary>
public static async ETTask OnStartGameButtonAsync(this DlgRole self)
{
    if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
    {
        Log.Error("请选择需要删除的角色");
        return;
    }
    try
    {
        //请求网关地址
        int errorCode = await LoginHelper.GetRelamKey(self.ZoneScene());
        if (errorCode != ErrorCode.ERR_Success)
        {
            Log.Debug(errorCode.ToString());
            return;
        }
        //登录游戏
        errorCode = await LoginHelper.EnterGame(self.ZoneScene());
        if (errorCode != ErrorCode.ERR_Success)
        {
            Log.Error(errorCode.ToString());
            return;
        }
    }
    catch (Exception e)
    {
        Log.Debug(e.ToString());
    }
}
```
