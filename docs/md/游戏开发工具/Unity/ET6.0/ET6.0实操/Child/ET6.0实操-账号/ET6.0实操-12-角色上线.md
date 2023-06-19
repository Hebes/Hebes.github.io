# ET6.0实操-12-角色上线

## 准备工作

OuterMessage.proto

```proto
//获取网关服务器
//ResponseType A2C_GetRealmKey
message C2A_GetRealmKey // IRequest
{
    int32 RpcId = 90;
    string Token = 1;
    int32 ServerId = 2;
    int64 AccountId = 3;
}

message A2C_GetRealmKey // IResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;
    
    string RealmKey = 1;
    string RealmAddress = 2;
}
```

进程之间发送消息

InnerMessage.proto

```C#
// 获取网关服务器
//ResponseType R2A_GetRealmKey
message A2R_GetRealmKey // IActorRequest
{
    int32 RpcId = 90;
    int64 AccountId = 1;
}

message R2A_GetRealmKey // IActorResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;

    string RealmKey = 1;
}
```

客户端 Unity.Model -> Codes -> Model -> Demo -> Account -> AccountInfoComponent.cs

```C#
namespace ET
{
    [ComponentOf(typeof(Scene))]
    /// <summary> 保存登录令牌等 </summary>
    public class AccountInfoComponent : Entity, IAwake, IDestroy
    {
        public string Token { get; set; }
        public long AccountId { get; set; }

        /// <summary> Realm网关负载均衡服务器令牌 </summary>
        public string RealmKey { get; set; }
        /// <summary> Realm网关负载均衡服务地址 </summary>
        public string RealmAddress { get; set; }
    }
}
```

服务端 Server.Model -> Generate -> ConfigPartial -> StartSceneConfig.cs

```C#
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace ET
{
    public partial class StartSceneConfigCategory
    {
        public MultiMap<int, StartSceneConfig> Gates = new MultiMap<int, StartSceneConfig>();

        //************************************
        public Dictionary<int, StartSceneConfig> Realms = new Dictionary<int, StartSceneConfig>();

        public MultiMap<int, StartSceneConfig> ProcessScenes = new MultiMap<int, StartSceneConfig>();

        public Dictionary<long, Dictionary<string, StartSceneConfig>> ZoneScenesByName = new Dictionary<long, Dictionary<string, StartSceneConfig>>();

        public StartSceneConfig LocationConfig;

        public List<StartSceneConfig> Robots = new List<StartSceneConfig>();

        public List<StartSceneConfig> GetByProcess(int process)
        {
            return this.ProcessScenes[process];
        }

        public StartSceneConfig GetBySceneName(int zone, string name)
        {
            return this.ZoneScenesByName[zone][name];
        }

        public override void AfterEndInit()
        {
            foreach (StartSceneConfig startSceneConfig in this.GetAll().Values)
            {
                this.ProcessScenes.Add(startSceneConfig.Process, startSceneConfig);

                if (!this.ZoneScenesByName.ContainsKey(startSceneConfig.Zone))
                {
                    this.ZoneScenesByName.Add(startSceneConfig.Zone, new Dictionary<string, StartSceneConfig>());
                }
                this.ZoneScenesByName[startSceneConfig.Zone].Add(startSceneConfig.Name, startSceneConfig);

                switch (startSceneConfig.Type)
                {
                    case SceneType.Gate:
                        this.Gates.Add(startSceneConfig.Zone, startSceneConfig);
                        break;
                    case SceneType.Location:
                        this.LocationConfig = startSceneConfig;
                        break;
                    case SceneType.Robot:
                        this.Robots.Add(startSceneConfig);
                        break;
                        //************************************
                    case SceneType.Realm:
                        this.Realms.Add(startSceneConfig.Zone, startSceneConfig);
                        break;
                }
            }
        }
    }

    public partial class StartSceneConfig : ISupportInitialize
    {
        public long InstanceId;

        public SceneType Type;

        public StartProcessConfig StartProcessConfig
        {
            get
            {
                return StartProcessConfigCategory.Instance.Get(this.Process);
            }
        }

        public StartZoneConfig StartZoneConfig
        {
            get
            {
                return StartZoneConfigCategory.Instance.Get(this.Zone);
            }
        }

        // 内网地址外网端口，通过防火墙映射端口过来
        private IPEndPoint innerIPOutPort;

        public IPEndPoint InnerIPOutPort
        {
            get
            {
                if (innerIPOutPort == null)
                {
                    this.innerIPOutPort = NetworkHelper.ToIPEndPoint($"{this.StartProcessConfig.InnerIP}:{this.OuterPort}");
                }

                return this.innerIPOutPort;
            }
        }

        private IPEndPoint outerIPPort;

        // 外网地址外网端口
        public IPEndPoint OuterIPPort
        {
            get
            {
                if (this.outerIPPort == null)
                {
                    this.outerIPPort = NetworkHelper.ToIPEndPoint($"{this.StartProcessConfig.OuterIP}:{this.OuterPort}");
                }

                return this.outerIPPort;
            }
        }

        public override void BeginInit()
        {
        }

        public override void EndInit()
        {
            this.Type = EnumHelper.FromString<SceneType>(this.SceneType);
            InstanceIdStruct instanceIdStruct = new InstanceIdStruct(this.Process, (uint)this.Id);
            this.InstanceId = instanceIdStruct.ToLong();
        }
    }
}
```

服务端 Server.Hotfix -> Demo -> RealmGateAddressHelper.cs 拓展

```C#
/// <summary> 获取网关负载均衡服务器 </summary>
public static StartSceneConfig GetRealm(int zone)
{
    StartSceneConfig zoneRealm = StartSceneConfigCategory.Instance.Realms[zone];
    return zoneRealm;
}
```

客户端 Unity.Hotfix -> Codes -> Hotfix- > Demo -> Login -> LoginHelper.cs 添加拓展

```C#
/// <summary> 获取网关服务器连接的令牌等 </summary>
public static async ETTask<int> GetRelamKey(Scene zoneScene)
{
    A2C_GetRealmKey a2C_GetRealmKey =  null;
    try
    {
        a2C_GetRealmKey = (A2C_GetRealmKey)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetRealmKey()
        {
            Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
            AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
            ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId,
        });
    }
    catch (Exception e)
    {
        Log.Error(e.ToString());
        return ErrorCode.ERR_NetWorkError;
    }

    if (a2C_GetRealmKey.Error != ErrorCode.ERR_Success)
    {
        Log.Error(a2C_GetRealmKey.Error.ToString());
        return a2C_GetRealmKey.Error;
    }

    //保存网关服务器的信息
    zoneScene.GetComponent<AccountInfoComponent>().RealmKey = a2C_GetRealmKey.RealmKey;
    zoneScene.GetComponent<AccountInfoComponent>().RealmAddress = a2C_GetRealmKey.RealmAddress;
    zoneScene.GetComponent<SessionComponent>().Session.Dispose();

    return ErrorCode.ERR_Success;
}
```

服务端 Server.Hotfix -> Demo -> Account -> Handler -> C2A_GetRealmKeyHandler.cs

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class C2A_GetRealmKeyHandler : AMRpcHandler<C2A_GetRealmKey, A2C_GetRealmKey>
    {
        protected override async ETTask Run(Session session, C2A_GetRealmKey request, A2C_GetRealmKey response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误,当前的Scene为:{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            //防止多次点击登录
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedlyError;
                reply();//用于发送response这条消息
                session.Disconnect().Coroutine();
                return;
            }

            string Token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);

            if (Token == null || Token != request.Token)
            {
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            //防止频繁创建游戏角色
            using (session.AddComponent<SessionLockingComponent>())
            {
                //在C2A_LoginAccountHandler这里也有用到
                //为什么用到LoginAccount:假设账号服务器正在和Ralem服务器发送消息中另外一个玩家登录这个账号把原先的玩家踢下线,
                //那么这个请求有效性就会收到质疑,帐号服务器虽然收到Realm服务器的回复消息但是玩家被踢下线,那么消息应该发给谁.
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountId))
                {
                    StartSceneConfig realmStartSceneConfig = RealmGateAddressHelper.GetRealm(request.ServerId);

                    //进程之间消息发送可以用MessageHelper
                    //realmStartSceneConfig.InstanceId包含Realm服务器的地址
                    R2A_GetRealmKey r2A_GetRealmKey = (R2A_GetRealmKey)await MessageHelper.CallActor(realmStartSceneConfig.InstanceId,
                        new A2R_GetRealmKey()
                        {
                            AccountId = request.AccountId,
                        });

                    //失败了
                    if (r2A_GetRealmKey.Error!= ErrorCode.ERR_Success)
                    {
                        response.Error = r2A_GetRealmKey.Error;
                        reply();
                        session.Disconnect().Coroutine();
                    }
                    //成功 给游戏客户端返回消息
                    response.RealmKey = r2A_GetRealmKey.RealmKey;
                    response.RealmAddress = realmStartSceneConfig.OuterIPPort.ToString();
                    reply();
                    session.Disconnect().Coroutine();
                }
            }
        }
    }
}
```
