# ET6.0实操-13-角色上线

ErrorCode.cs

```C#
/// <summary> 连接Gate令牌错误 </summary>
public const int ERR_ConnectGateKeyError = 200013;
/// <summary> 请求的场景错误 </summary>
public const int ERR_RequestSceneTypeError = 200014;
```

Server.Hotfix\Demo\Account\Handler\A2R_GetRealmKeyHandler.cs

```C#
using System;

namespace ET
{
    public class A2R_GetRealmKeyHandler : AMActorRpcHandler<Scene, A2R_GetRealmKey, R2A_GetRealmKey>
    {
        //Scene 代表A2R_GetRealmKey消息交给Scene实体处理
        protected override async ETTask Run(Scene scene, A2R_GetRealmKey request, R2A_GetRealmKey response, Action reply)
        {
            if (scene.SceneType != SceneType.Realm)
            {
                Log.Error($"请求的Sceen错误,当前Scene为:{scene.SceneType}");
                response.Error = ErrorCode.ERR_RequestSceneTypeError;
                reply();
                return;
            }

            //scene的类型是Realm
            string key = TimeHelper.ServerNow().ToString() + RandomHelper.RandInt64().ToString();
            scene.GetComponent<TokenComponent>().Remove(request.AccountId);
            scene.GetComponent<TokenComponent>().Add(request.AccountId, key);
            response.RealmKey = key.ToString();
            reply();
            await ETTask.CompletedTask;
        }
    }
}
```

Server.Hotfix\Demo\Scene\SceneFactory.cs

```C#
 case SceneType.Realm:
    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.OuterIPPort,SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
    //***********************************
    scene.AddComponent<TokenComponent>();
    break;
```

OuterMessage.proto

```proto
//连接网关服务器
//ResponseType R2C_LoginRealm
message C2R_LoginRealm // IRequest
{
    int32 RpcId = 90;
    int64 AccountId = 1;
    string RealmTokenKey = 2;
}

message R2C_LoginRealm // IResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;

    string GateSessionKey = 1; //令牌Key
    string GateAddress = 2; //Gate地址
}
```

CoroutineLockType.cs

```C#
public const int LoginRealm = 13;//登录Realm锁
public const int LoginGate = 14;//登录Gate锁
```

InnerMessage.proto

```proto
// 同Gate请求一个key,客户端可以拿着这个Key请求gate
//ResponseType G2R_GetLoginGateKey
message R2G_GetLoginGateKey // IActorRequest
{
    int32 RpcId = 90;
    int64 AccountId = 1;
}

message G2R_GetLoginGateKey // IActorResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;

    string GateSessionKey = 1;
}
```

Server.Hotfix\Demo\Account\Handler\R2G_GetLoginGateKeyHandler.cs

```C#
using System;

namespace ET
{
    public class R2G_GetLoginGateKeyHandler : AMActorRpcHandler<Scene, R2G_GetLoginGateKey, G2R_GetLoginGateKey>
    {
        protected override async ETTask Run(Scene scene, R2G_GetLoginGateKey request, G2R_GetLoginGateKey response, Action reply)
        {
            if (scene.SceneType != SceneType.Gate)
            {
                Log.Error($"请求的Sceen错误,当前Scene为:{scene.SceneType}");
                response.Error = ErrorCode.ERR_RequestSceneTypeError;
                reply();
                return;
            }

            //scene的类型是Realm
            string key =  RandomHelper.RandInt64().ToString()+TimeHelper.ServerNow().ToString();
            scene.GetComponent<GateSessionKeyComponent>().Remove(request.AccountId);
            scene.GetComponent<GateSessionKeyComponent>().Add(request.AccountId, key);
            response.GateSessionKey = key;
            reply();
            await ETTask.CompletedTask;
        }
    }
}
```

Server.Hotfix\Demo\Account\Handler\C2R_LoginRealmHandler.cs

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class C2R_LoginRealmHandler : AMRpcHandler<C2R_LoginRealm, R2C_LoginRealm>
    {
        protected override async  ETTask Run(Session session, C2R_LoginRealm request, R2C_LoginRealm response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Realm)
            {
                Log.Error($"请求的Scene错误,当前的Scene为:{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }
            Scene domainScene =session.DomainScene();

            //防止多次点击登录
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedlyError;
                reply();//用于发送response这条消息
                session?.Disconnect().Coroutine();
                return;
            }

            string Token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);

            if (Token == null || Token != request.RealmTokenKey)
            {
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session?.Disconnect().Coroutine();
                return;
            }

            domainScene.GetComponent<TokenComponent>().Remove(request.AccountId);//之后不会在连接Realm服务器

            //防止频繁创建游戏角色
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginRealm, request.AccountId))
                {
                    //取模固定分配一个Gate
                    //domainScene.Zone 一服 看配置表都是1
                    StartSceneConfig config = RealmGateAddressHelper.GetGate(domainScene.Zone,request.AccountId);

                    //进程之间消息发送可以用MessageHelper
                    //同Gate请求一个key,客户端可以拿着这个Key请求gate
                    G2R_GetLoginGateKey g2R_GetLoginGateKey = (G2R_GetLoginGateKey)await MessageHelper.CallActor(config.InstanceId,
                        new R2G_GetLoginGateKey()
                        {
                            AccountId = request.AccountId,
                        });

                    //失败了
                    if (g2R_GetLoginGateKey.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = g2R_GetLoginGateKey.Error;
                        reply();
                        return;
                    }
                    //成功 给游戏客户端返回消息
                    response.GateSessionKey = g2R_GetLoginGateKey.GateSessionKey;
                    response.GateAddress = config.OuterIPPort.ToString();
                    reply();
                    session?.Disconnect().Coroutine();
                }
            }
        }
    }
}
```

未完成的 仅供观看,后面会补全

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
}
```
