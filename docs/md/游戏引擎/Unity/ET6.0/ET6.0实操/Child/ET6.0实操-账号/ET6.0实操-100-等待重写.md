# ET6.0实操-10-删除角色

**[EUI](<https://www.bilibili.com/video/BV12F411e7bP>)**

## 准备工作

Unity.Model\Codes\Demo\Role\RoleInfosComponent.cs

```C#
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType]
    /// <summary> 存储游戏服务器创建出来的角色信息 </summary>
    public class RoleInfosComponent : Entity, IAwake, IDestroy
    {
        /// <summary> 角色信息列表 </summary>
        public List<RoleInfo> roleInfosList { get; set; } = new List<RoleInfo>();
        /// <summary> 当前的角色ID </summary>
        public long CurrentRoleId { get; set; } = 0;
    }
}
```

Unity.Hotfix\Codes\Hotfix\Demo\Role\RoleInfosComponentSystem.cs

```C#
namespace ET
{
    public class RoleInfosComponentSystemAwakeSystem : AwakeSystem<RoleInfosComponent>
    {
        public override void Awake(RoleInfosComponent self)
        {
        }
    }

    public class RoleInfosComponentSystemDestroySystem : DestroySystem<RoleInfosComponent>
    {
        public override void Destroy(RoleInfosComponent self)
        {
            foreach (var roleInfo in self.roleInfosList)
                roleInfo?.Dispose();
            self.roleInfosList.Clear();
            self.CurrentRoleId = 0;
        }
    }

    /// <summary> 角色信息系统 </summary>
    public static  class RoleInfosComponentSystem
    {
    }
}
```

Unity.Model\Codes\Model\Demo\ServerInfo\ServerInfosComponent.cs

```C#
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType]
    /// <summary> 服务器信息列表 </summary>
    public class ServerInfosComponent : Entity, IAwake, IDestroy
    {
        /// <summary> 服务器列表 </summary>
        public List<ServerInfo> serverInfoList = new List<ServerInfo>();
        /// <summary> 需要进入服务器的ID </summary>
        public int CurrentServerId { get; set; } = 0;
    }
}
```

Unity.Hotfix\Codes\Hotfix\Demo\Login\LoginHelper.cs 拓展方法

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

        /// <summary> 获取服务器列表 </summary>
        public static async ETTask<int> GetServerInfos(Scene zoneScene)
        {
            A2C_GetServerInfos a2C_GetServerInfos = null;

            try
            {
                a2C_GetServerInfos = (A2C_GetServerInfos)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetServerInfos()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                });
            }
            catch (Exception e)
            {
                Log.Debug(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2C_GetServerInfos.Error != ErrorCode.ERR_Success)
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

        /// <summary> 创建角色 </summary>
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
            //新添加
            //消息类转实体类
            RoleInfo newRoleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
            newRoleInfo.FromMessage(a2C_CreatRole.RloeInfo);
            zoneScene.GetComponent<RoleInfosComponent>().roleInfosList.Add(newRoleInfo);

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

            //账号信息添加
            zoneScene.AddComponent<AccountInfoComponent>();
            //服务器信息添加
            zoneScene.AddComponent<ServerInfosComponent>();
            //角色信息添加
            zoneScene.AddComponent<RoleInfosComponent>();
            
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

## 实际使用
