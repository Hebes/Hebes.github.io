# ET6.0实操-账号

下面的路径是按照编辑器打开后解决方案资源管理器看到的路径

账号密码:Uw123456

## 准备流程

![1](\../Image/ET6.0实操-账号/1.png)

```C#
namespace ET
{
    public enum AccountType
    {
        /// <summary> 普通账号 </summary>
        General = 0,
        /// <summary> 黑名单 </summary>
        BlackList=1,
    }

    /// <summary> 账号结构 </summary>
    public class Account : Entity, IAwake
    {
         /// <summary> 账号名 </summary>
        public string AccountName { get; set; }
        /// <summary> 密码 </summary>
        public string Password { get; set; }
        /// <summary> 账号创建时间 </summary>
        public long CreateTimer { get; set; }
        /// <summary> 账号类型 </summary>
        public int AccountType { get; set; }
    }
}
```

![2](\../Image/ET6.0实操-账号/2.png)

```C#
namespace ET
{
    public enum SceneType
    {
        Process = 0,
        Manager = 1,
        Realm = 2,
        Gate = 3,
        Http = 4,
        Location = 5,
        Map = 6,

        //添加账号服务器
        Account = 7,

        // 客户端Model层
        Client = 30,
        Zone = 31,
        Login = 32,
        Robot = 33,
        Current = 34,
    }
}
```

DBManagerComponent添加[ComponentOf(typeof(Scene))]

![3](\../Image/ET6.0实操-账号/3.png)

```C#
namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType(typeof(DBComponent))]
    public class DBManagerComponent: Entity, IAwake, IDestroy
    {
        public static DBManagerComponent Instance;
        
        public DBComponent[] DBComponents = new DBComponent[IdGenerater.MaxZone];
    }
}
```

![7](\../Image/ET6.0实操-账号/7.png)

```C#
case SceneType.Account://账号服务器
    scAddComponent<NetKcpComponeIPEndPoint, (startSceneConfig.OuterIPPoSessionStreamDispatcherTSessionStreamDispatcherServeer);
    break;
```


![4](\../Image/ET6.0实操-账号/4.png)

```C#
//数据库组件
Game.Scene.AddComponent<DBManagerCompone();//添加[ComponentOf(typeof(Scene))]就错了
```

![5](\../Image/ET6.0实操-账号/5.png)

启动服务端

![6](\../Image/ET6.0实操-账号/6.png)

编写proto

OuterMessage.proto

```proto
//自己定义的
//ResponseType A2C_LoginAccount
message C2A_LoginAccount // IRequest
{
    int32 RpcId = 90; 

    string Account = 1;
    string Password = 2;
}

message A2C_LoginAccount // IResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;    

    string Token = 1; // 登录令牌 可以理解为登录验证码
    int64 AccountId = 2; //账号的id 相当于身份证号码
}
```

导出proto

正式开始

## 前端部分

定义错误码 Unity\Codes\Model\Module\Message\ErrorCode.cs

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
    }
}
```

保存令牌等 \Unity\Codes\Model\Demo\Account\AccountInofComponent.cs

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    /// <summary> 保存登录令牌等 </summary>
    public class AccountInofComponent : Entity, IAwake, IDestroy
    {
        public string Token { get; set; }
        public long AccountId { get; set; }
    }
}
```

Unity\Codes\Hotfix\Demo\Account\AccountInfoComponentSystem.cs

```C#
namespace ET
{
    public class AccountInfoComponentAwakeSystem : AwakeSystem<AccountInofComponent>
    {
        public override void Awake(AccountInofComponent self)
        {
            self.Token  =string.Empty;
            self.AccountId = 0;
        }
    }

    public class AccountInfoComponentDestroySystem : DestroySystem<AccountInofComponent>
    {
        public override void Destroy(AccountInofComponent self)
        {

        }
    }

    public static  class AccountInfoComponentSystem
    {
    }
}
```

Unity\Codes\Hotfix\Demo\Scene\SceneFactory.cs

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
            zoneScene.AddComponent<AccountInofComponent>();
            
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

重新修改Login代码逻辑,以前登录代码逻辑全都删除修改成 \Unity\Codes\Hotfix\Demo\Login\LoginHelper.cs

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

            //保存令牌等
            zoneScene.GetComponent<AccountInofComponent>().Token = a2C_LoginAccount.Token;
            zoneScene.GetComponent<AccountInofComponent>().AccountId = a2C_LoginAccount.AccountId;

            return ErrorCode.ERR_Success;
        }
    }
}
```

## 后端部分

![8](\../Image/ET6.0实操-账号/8.png)

```C#
session.AddChild<Account>();//报错
```

修复方法

Session类添加[ChildType]
