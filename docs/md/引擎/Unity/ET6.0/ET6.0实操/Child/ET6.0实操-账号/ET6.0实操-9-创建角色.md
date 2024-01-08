# ET6.0实操-9-创建角色

## 准备工作

proto 文件 OuterMessage.proto

```C#
//请求创建游戏角色
//ResponseType A2C_CreatRole
message C2A_CreatRole // IRequest
{
    int32 RpcId = 90;
    string Token = 1;
    int64 AccountId = 2;
    string Name = 3;
    int32 ServerId = 4;
}

//请求创建游戏角色
message A2C_CreatRole // IResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;

    RoleInfoProto RloeInfo = 1;
}

message RoleInfoProto
{
    int64 Id = 1;
    string Name = 2;
    int32 State = 3;
    int64 AccountId = 4;
    int64 LastLoginTime = 5;
    int64 CreateTime = 6;
    int32 ServerId = 7;
}
```

错误代码 server Unity.Model\Module\Message\ErrorCode.cs 新增

```C#
/// <summary> 角色名称是空的 </summary>
public const int ERR_RoleNameIsNullError = 200009;
/// <summary> 角色名称重复 </summary>
public const int ERR_RoleNameSameError = 200010;
```

锁的类型 CoroutineLockType.cs

```C#
public const int CreatRoleLock = 12;//玩家创建携程锁
```

消息协议的转换 Unity.Hotfix\Demo\Role\RoleInfoSystem.cs

```C#
namespace ET
{
    public static  class RoleInfoSystem
    {
        public static void FromMessage(this RoleInfo self,RoleInfoProto roleInfoProto)
        {
            self.Id = roleInfoProto.Id;
            self.Name = roleInfoProto.Name;
            self.State = roleInfoProto.State;
            self.ServerId = roleInfoProto.ServerId;
            self.CreatTiemr = roleInfoProto.CreateTime;
            self.AccountId = roleInfoProto.AccountId;
            self.LastLoginTime=roleInfoProto.LastLoginTime;
        }

        public static RoleInfoProto ToMessage(this RoleInfo self)
        {
            return new RoleInfoProto()
            {
                Id = self.Id,
                Name = self.Name,
                State = self.State,
                ServerId = self.ServerId,
                AccountId = self.AccountId,
                LastLoginTime = self.LastLoginTime,
                CreateTime = self.CreatTiemr,
            };
        }
    }
}
```

Unity.Model\Codes\Model\Demo\Role\RoleInfo.cs

```C#
namespace ET
{
    public enum RoleInfoState
    {
        /// <summary> 正常状态 </summary>
        Normal = 0,
        /// <summary> 冻结状态 </summary>
        Freeze = 1,
    }
    
    /// <summary> 游戏角色 </summary>
    public class RoleInfo : Entity, IAwake
    {
        /// <summary> 角色名称 </summary>
        public string Name { get; set; }
        /// <summary> 区服 </summary>
        public int ServerId { get; set; }
        /// <summary> 角色当前的状态 </summary>
        public int State { get; set; }
        /// <summary> 角色所属的账号ID </summary>
        public long AccountId { get; set; }
        /// <summary> 最后的登录时间 </summary>
        public long LastLoginTime { get; set; }
        /// <summary> 创建的时间 </summary>
        public long CreatTiemr { get; set; }
    }
}
```

Unity.Hotfix\Codes\Hotfix\Demo\Login\LoginHelper.cs 添加新的拓展方法

```C#
 /// <summary> 创建角色 </summary>
/// <param name="zoneScene"></param>
/// <param name="name">角色名称</param>
/// <returns></returns>
public static async ETTask<int> CreatRole(Scene zoneScene, string name)
{
    A2C_CreatRole a2C_CreatRole = null;

    try
    {
        a2C_CreatRole = (A2C_CreatRole)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_CreatRole()
        {
            AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
            Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
            Name = name,
            ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId,
        });
    }
    catch (Exception e)
    {
        Log.Error(e.ToString());
        return ErrorCode.ERR_NetWorkError;
    }

    if (a2C_CreatRole.Error != ErrorCode.ERR_Success)
    {
        Log.Error(a2C_CreatRole.Error.ToString());
        return a2C_CreatRole.Error;
    }

    //消息类转实体类
    Log.Debug("进入到了实体转化类");
    RoleInfo newRoleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
    newRoleInfo.FromMessage(a2C_CreatRole.RloeInfo);
    zoneScene.GetComponent<RoleInfosComponent>().roleInfosList.Add(newRoleInfo);
    return ErrorCode.ERR_Success;
}
```

添加现有项目,没有的文件夹自己需要创建

Server.Model\Demo\Role\RoleInfo.cs 代码同上,直接添加现有项

Server.Hotfix\Demo\Role\RoleInfoSystem.cs 代码同上,直接添加现有项

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
            zoneScene.AddComponent<ServerInfosComponent>();
            
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

Hotfix.Server\Demo\ServerInfo\ServerInfoManagerComponentSystem.cs

```C#
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

通过LoginHelper的CreatRole发送消息给服务区创建新的角色

Server.Hotfix\Demo\ServerInfo\Handler\C2A_GetServerInfosHandler.cs

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

Hotfix.Server\Demo\Role\Handler\C2A_CreatRoleSystem.cs

```C#
using System;

namespace ET
{
    public class C2A_CreatRoleSystem : AMRpcHandler<C2A_CreatRole, A2C_CreatRole>
    {
        protected override async ETTask Run(Session session, C2A_CreatRole request, A2C_CreatRole response, Action reply)
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

            //TODO 游戏角色名称的敏感词判定、长度的判定
            if (string.IsNullOrEmpty(request.Name))
            {
                response.Error = ErrorCode.ERR_RoleNameIsNullError;
                reply();
                return;
            }

            //防止频繁创建游戏角色
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreatRoleLock, request.AccountId))
                {
                    var roleInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<RoleInfo>(d =>
                    d.Name == request.Name && d.ServerId == request.ServerId);

                    //有角色
                    if (roleInfos == null || roleInfos.Count > 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNameSameError;
                        reply();
                        return;
                    }

                    //没有角色的话,就创建出来一个
                    RoleInfo newRoleInfo = session.AddChildWithId<RoleInfo>(IdGenerater.Instance.GenerateUnitId(request.ServerId));
                    newRoleInfo.Name = request.Name;
                    newRoleInfo.State = (int)RoleInfoState.Normal;
                    newRoleInfo.ServerId = request.ServerId;
                    newRoleInfo.AccountId = request.AccountId;
                    newRoleInfo.CreatTiemr = TimeHelper.ServerNow();
                    newRoleInfo.LastLoginTime = 0;

                    await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<RoleInfo>(newRoleInfo);

                    response.RloeInfo = newRoleInfo.ToMessage();
                    reply();

                    newRoleInfo?.Dispose();//销毁
                }
            }
        }
    }
}
```

## 实际使用

DlgRoleSystem.cs 本人的写法

```C#
/// <summary> 创建角色 </summary>
public static async ETTask OnCreatRoleButton(this DlgRole self)
{
    //名字
    string InputName = self.View.EInputFieldNameInputField.text;
    if (string.IsNullOrEmpty(InputName))
    {
        Log.Error("名字是空的");
        return;
    }
    
    try
    {
        //创建角色
        int errorCode = await LoginHelper.CreatRole(self.ZoneScene(), InputName);
        if (errorCode != ErrorCode.ERR_Success)
        {
            Log.Debug(errorCode.ToString());
            return;
        }
        //显示角色
        foreach (RoleInfo item in self.ZoneScene().GetComponent<RoleInfosComponent>().roleInfosList)
        {
            //克隆
            GameObject roleItemGO = GameObject.Instantiate(self.View.ERoleItemImage.gameObject, self.View.EContentImage.transform);
            roleItemGO.SetActive(true);
            roleItemGO.transform.Find("Name").GetComponent<Text>().text = item.Name;
            roleItemGO.GetComponent<Button>().AddListener(() =>
            {
                self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId = item.Id;
            });
        }
    }
    catch (Exception e)
    {
        Log.Debug(e.ToString());
    }
}
```
