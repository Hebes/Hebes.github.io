# ET6.0实操-8-获取服务器列表

## 准备工作

mongoDB创建集合SeerverInfo

![27](\../../Image/ET6.0实操-账号/27.png)

<a href="\md\ET6.0\ET6.0实操\Resources\ServerInfoConfig.xlsx" target="_blank">ServerInfoConfig表格</a>

OuterMessage.proto

```C#
//ResponseType A2C_GetServerInfos
message C2A_GetServerInfos // IRequest
{
    int32 RpcId = 90;
    string Token = 1;
    int64 AccountId = 2;
}

//得到一个列表repeated
message A2C_GetServerInfos // IResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;

    repeated ServerInfoProto ServerInfoList = 1;
}

//服务器结构体
message ServerInfoProto
{
    int32 Id = 1;
    int32 Status = 2;
    string ServerName = 3;
}
```

创建文件夹

![22](\../../Image/ET6.0实操-账号/22.png)

![21](\../../Image/ET6.0实操-账号/21.png)

后端的这个是通过添加现有项->前端的那个

![23](\../../Image/ET6.0实操-账号/23.png)

```C#
namespace ET
{
    public enum ServerStatus
    {
        NorMal = 0,
        Stop = 1,
    }

    public class ServerInfo : Entity, IAwake
    {
         public int Status { get; set; }
        public string ServerName { get; set; }
    }
}
```

```C#
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType]
    public class ServerInfoManagerComponent : Entity, IAwake,IDestroy,ILoad
    {
        public List<ServerInfo> ServerInfos { get; set; } = new List<ServerInfo>();
    }
}
```

```C#
namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误

        // 110000以下的错误请看ErrorCore.cs

        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常

        //自定义错误码

        /// <summary> 网络错误 </summary>
        public const int ERR_NetWorkError = 200002;
        /// <summary> 登录信息错误 </summary>
        public const int ERR_LoginInfoError = 200003;
        /// <summary> 账号格式错误 </summary>
        public const int ERR_AccountNamFormError = 200004;
        /// <summary> 登录密码格式错误 </summary>
        public const int ERR_PasswrodFromError = 200005;
        /// <summary> 账号处于黑名单 </summary>
        public const int ERR_AccountInBlackListError = 200006;
        /// <summary> 登录密码错误 </summary>
        public const int ERR_LoginPasswordError = 200007;
        /// <summary> 多次发送错误 </summary>
        public const int ERR_RequestRepeatedlyError = 200007;
        /// <summary> 令牌Token错误 </summary>
        public const int ERR_TokenError = 200008;//新添加的
    }
}
```

```C#
using System.Net;

namespace ET
{
    public static class SceneFactory
    {
        public static async ETTask<Scene> Create(Entity parent, string name, SceneType sceneType)
        {
            long instanceId = IdGenerater.Instance.GenerateInstanceId();
            return await Create(parent, instanceId, instanceId, parent.DomainZone(), name, sceneType);
        }
        
        public static async ETTask<Scene> Create(Entity parent, long id, long instanceId, int zone, string name, SceneType sceneType, StartSceneConfig startSceneConfig = null)
        {
            await ETTask.CompletedTask;
            Scene scene = EntitySceneFactory.CreateScene(id, instanceId, zone, sceneType, name, parent);

            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);

            switch (scene.SceneType)
            {
                case SceneType.Realm:
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.OuterIPPort, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    break;
                case SceneType.Gate:
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.OuterIPPort, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    scene.AddComponent<PlayerComponent>();
                    scene.AddComponent<GateSessionKeyComponent>();
                    break;
                case SceneType.Map:
                    scene.AddComponent<UnitComponent>();
                    scene.AddComponent<AOIManagerComponent>();
                    break;
                case SceneType.Location:
                    scene.AddComponent<LocationComponent>();
                    break;
                case SceneType.Account://账号服务器
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.OuterIPPort, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    scene.AddComponent<TokenComponent>();
                    scene.AddComponent<AccountSessionsComponent>();//新添加的顶号
                    scene.AddComponent<ServerInfoManagerComponent>();//服务器列表//新添加的
                    break;
                case SceneType.LoginCenter://账号中心服务器
                    scene.AddComponent<LoginInfoRecordComponent>();
                    break;
            }
            return scene;
        }
    }
}
```

```C#
using System;

namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {
            A2C_LoginAccount a2C_LoginAccount = null;
            Session accountSession = null;

            try
            {
                accountSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                password = MD5Helper.StringMD5(password);//密码加密
                a2C_LoginAccount = (A2C_LoginAccount)await accountSession.Call(new C2A_LoginAccount() { Account = account, Password = password });
            }
            catch (Exception e)
            {
                accountSession?.Dispose();
                Log.Error("(PS:服务器可能没开?)连接错误:" + e);
                return ErrorCode.ERR_NetWorkError;//网络错误
            }

            //为什么写这个代码 因为这边是逻辑层  不能显示编写UI界面显示代码 所以返回int型,在外面编写显示登录错误的UI界面
            if (a2C_LoginAccount.Error != ErrorCode.ERR_Success)
            {
                accountSession?.Dispose();
                return a2C_LoginAccount.Error;
            }

            zoneScene.AddComponent<SessionComponent>().Session = accountSession;//保存Session链接
            zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();//心跳包检测

            //保存令牌等
            zoneScene.GetComponent<AccountInfoComponent>().Token = a2C_LoginAccount.Token;
            zoneScene.GetComponent<AccountInfoComponent>().AccountId = a2C_LoginAccount.AccountId;

            return ErrorCode.ERR_Success;
        }

        //新增方法
        /// <summary> 获取服务器列表 </summary>
        public static async ETTask<int> GetServerInfos(Scene zoneScene)
        {
            A2C_GetServerInfos a2C_GetServerInfos = null;

            try
            {
                a2C_GetServerInfos = (A2C_GetServerInfos)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetServerInfos()
                {
                    AccountId=zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token= zoneScene.GetComponent<AccountInfoComponent>().Token,
                });
            }
            catch (Exception e)
            {
                Log.Debug(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2C_GetServerInfos.Error!= ErrorCode.ERR_Success)
            {
                return a2C_GetServerInfos.Error;
            }

            foreach (var serverInfoProto in a2C_GetServerInfos.ServerInfoList)
            {
                ServerInfo serverInfo = zoneScene.GetComponent<ServerInfosComponent>().AddChild<ServerInfo>();
                serverInfo.FromMessage(serverInfoProto);
                zoneScene.GetComponent<ServerInfosComponent>().Add(serverInfo);
            }

            return ErrorCode.ERR_Success;
        }
    }
}
```

```C#
namespace ET
{
    public static class SceneFactory
    {
        public static Scene CreateZoneScene(int zone, string name, Entity parent)
        {
            Scene zoneScene = EntitySceneFactory.CreateScene(Game.IdGenerater.GenerateInstanceId(), zone, SceneType.Zone, name, parent);
            zoneScene.AddComponent<ZoneSceneFlagComponent>();
            zoneScene.AddComponent<NetKcpComponent, int>(SessionStreamDispatcherType.SessionStreamDispatcherClientOuter);
            zoneScene.AddComponent<CurrentScenesComponent>();
            zoneScene.AddComponent<ObjectWait>();
            zoneScene.AddComponent<PlayerComponent>();

            //******************************
            zoneScene.AddComponent<AccountInfoComponent>();
            zoneScene.AddComponent<ServerInfosComponent>();//新添加的
            
            Game.EventSystem.Publish(new EventType.AfterCreateZoneScene() {ZoneScene = zoneScene});
            return zoneScene;
        }
        
        public static Scene CreateCurrentScene(long id, int zone, string name, CurrentScenesComponent currentScenesComponent)
        {
            Scene currentScene = EntitySceneFactory.CreateScene(id, IdGenerater.Instance.GenerateInstanceId(), zone, SceneType.Current, name, currentScenesComponent);
            currentScenesComponent.Scene = currentScene;
            
            Game.EventSystem.Publish(new EventType.AfterCreateCurrentScene() {CurrentScene = currentScene});
            return currentScene;
        }
    }
}
```

![24](\../Image/ET6.0实操-账号/24.png)

```C#
namespace ET
{

    public class ServerInfosComponentAwakeSystem : AwakeSystem<ServerInfosComponent>
    {
        public override void Awake(ServerInfosComponent self)
        {
            foreach (var serverInfo in self.serverInfoList)
            {
                serverInfo?.Dispose();
            }
            self.serverInfoList.Clear();
        }
    }

    public class ServerInfosComponentDestroySystem : DestroySystem<ServerInfosComponent>
    {
        public override void Destroy(ServerInfosComponent self)
        {
        }
    }

    [FriendClass(typeof(ServerInfosComponent))]
    public static class ServerInfosComponentSystem
    {
        public static void Add(this ServerInfosComponent self, ServerInfo serverInfo)
        {
            self.serverInfoList.Add(serverInfo);
        }
    }
}
```

```C#
namespace ET
{
    [FriendClass(typeof(ServerInfo))]
    public static  class ServerInfoSystem
    {
        public static void FromMessage(this ServerInfo self,ServerInfoProto serverInfoProto)
        {
            self.Id = serverInfoProto.Id;
            self.Status = serverInfoProto.Status;
            self.ServerName = serverInfoProto.ServerName;
        }

        public static ServerInfoProto ToMassage(this ServerInfo self)
        {
            return new ServerInfoProto() 
            {
                Id= (int)self.Id,
                Status=self.Status,
                ServerName=self.ServerName,
            };
        }
    }
}
```

后端引用前端

![25](\../../Image/ET6.0实操-账号/25.png)

---

![26](\../../Image/ET6.0实操-账号/26.png)

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ServerInfoManagerComponentAwakeSystem : AwakeSystem<ServerInfoManagerComponent>
    {
        public override void Awake(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }

    public class ServerInfoManagerComponentDestroySystem : DestroySystem<ServerInfoManagerComponent>
    {
        public override void Destroy(ServerInfoManagerComponent self)
        {
            foreach (var serverInfo in self.ServerInfos)
                serverInfo?.Dispose();
            self.ServerInfos.Clear();
        }
    }

    /// <summary> 热重载会用到的 </summary>
    public class ServerInfoManagerComponentLoadSystem : LoadSystem<ServerInfoManagerComponent>
    {
        public override void Load(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }

    [FriendClass(typeof(ServerInfoManagerComponent))]
    public static class ServerInfoManagerComponentSystem
    {
        public static async ETTask Awake(this ServerInfoManagerComponent self)
        {
            //从数据库中获取游戏服务器信息
            var ServerInfoList = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<ServerInfo>(d => true);
            if (ServerInfoList == null || ServerInfoList.Count <= 0)
            {
                Log.Error("sercverInfo count is zero");
                self.ServerInfos.Clear();
                var serverInfoConfigs = ServerInfoConfigCategory.Instance.GetAll();

                foreach (var info in serverInfoConfigs.Values)
                {
                    ServerInfo newServerInfo = self.AddChildWithId<ServerInfo>(info.Id);
                    newServerInfo.ServerName = info.ServerName;
                    newServerInfo.Status = (int)ServerStatus.NorMal;
                    self.ServerInfos.Add(newServerInfo);

                    await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(newServerInfo);
                }
                return;
            }

            self.ServerInfos.Clear();

            foreach (var serverInfo in ServerInfoList)
            {
                self.AddChild(serverInfo);
                self.ServerInfos.Add(serverInfo);
            }
        }
    }
}
```

## 实际使用

```C#
using System;

namespace ET
{
    /// <summary> 获取游戏服务器端下发游戏区服信息 </summary>
    public class C2A_GetServerInfosHandler : AMRpcHandler<C2A_GetServerInfos, A2C_GetServerInfos>
    {
        protected override async ETTask Run(Session session, C2A_GetServerInfos request, A2C_GetServerInfos response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Sceen错误,当前Sceen为:{session.DomainScene().SceneType}");
                session.Dispose();
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

            foreach (var sererInfo in session.DomainScene().GetComponent<ServerInfoManagerComponent>().ServerInfos)
                response.ServerInfoList.Add(sererInfo.ToMassage());

            reply();

            await ETTask.CompletedTask;
        }
    }
}
```

运行后(PS:数据库没有数据的话肯定会报错,下次再次启动就好了)发现数据库就有两个数据
