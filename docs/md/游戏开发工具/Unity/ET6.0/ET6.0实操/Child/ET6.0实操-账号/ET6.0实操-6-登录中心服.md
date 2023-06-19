# ET6.0实操-6-登录中心服

## 准备工作

登录中心服务器的作用

前面只编写了账号服务器的顶号代码,但是如果玩家A在Gate服务器,玩家B在账号服务器(PS:都是同一个账号登录),那么两个玩家就不会进行顶号操作,所以添加中心服务器,控制正在登录和已经登录在Gate网关的玩家.

![15](\../../Image/ET6.0实操-账号/15.png)

![16](\../../Image/ET6.0实操-账号/16.png)

准备工作

InnerMessage.proto

```C#
// 服务器之间的相互通讯
// 账号服务器发往登录中心服
//ResponseType L2A_LoginAccountResponse
message A2L_LoginAccountRequest // IActorRequest
{
    int32 RpcId = 90;
    int64 AccountId = 1;
}

message L2A_LoginAccountResponse // IActorResponse
{
    int32 RpcId = 1;
    int32 Error = 2;
    string Message = 3;
}
```

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

        //登录中心服务器
        LoginCenter = 8,

        // 客户端Model层
        Client = 30,
        Zone = 31,
        Login = 32,
        Robot = 33,
        Current = 34,
    }
}
```

Server.Model->Demo->Account创建

```C#
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    /// <summary> 登录中心服的组件 </summary>
    public  class LoginInfoRecordComponent:Entity,IAwake ,IDestroy
    {
        public Dictionary<long, int> AccountLoginInfoDic { get; set; } = new Dictionary<long, int>();
    }
}
```

Server.Hotfix->Demo->Account创建

```C#
namespace ET
{
    public class LoginInfoRecordComponentAwakeSystem : AwakeSystem<LoginInfoRecordComponent>
    {
        public override void Awake(LoginInfoRecordComponent self)
        {
        }
    }
    public class LoginInfoRecordComponentDestroySystem : DestroySystem<LoginInfoRecordComponent>
    {
        public override void Destroy(LoginInfoRecordComponent self)
        {
            self.AccountLoginInfoDic.Clear();
        }
    }

    public static class LoginInfoRecordComponentSystem
    {
        public static void Add(this LoginInfoRecordComponent self, long key, int value)
        {
            if (self.AccountLoginInfoDic.ContainsKey(key))
            {
                self.AccountLoginInfoDic[key] = value;
                return;
            }
            self.AccountLoginInfoDic.Add(key, value);
        }

        public static void Remove(this LoginInfoRecordComponent self, long key)
        {
            if (self.AccountLoginInfoDic.ContainsKey(key))
                self.AccountLoginInfoDic.Remove(key);
        }

        public static int Get(this LoginInfoRecordComponent self, long key)
        {
            if (!self.AccountLoginInfoDic.TryGetValue(key, out int value))
                return -1;
            return value;
        }

        public static bool IsExit(this LoginInfoRecordComponent self,long key)
        {
            return self.AccountLoginInfoDic.ContainsKey(key);
        }
    }
}
```

添加组件

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
                    //新添加的顶号
                    scene.AddComponent<AccountSessionsComponent>();
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

## 实际使用

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ET
{

    [MessageHandler]
    /// <summary> 后端游戏账号登录 </summary>
    public class C2A_LoginAccountHandler : AMRpcHandler<C2A_LoginAccount, A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误,当前的Scene为:{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            //防止多次点击登录
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedlyError;
                reply();//用于发送response这条消息
                session.Disconnect().Coroutine();
                return;
            }

            if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginInfoError;
                reply();//用于发送response这条消息
                session.Disconnect().Coroutine();
                return;
            }

            //验证账号
            if (!Regex.IsMatch(request.Account.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_AccountNamFormError;
                reply();//用于发送response这条消息
                session.Disconnect().Coroutine();
                return;
            }
            //验证密码
            if (!Regex.IsMatch(request.Password.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_LoginPasswordError;
                reply();//用于发送response这条消息
                session.Disconnect().Coroutine();
                return;
            }

            //using包裹语句块里面的代码逻辑执行完毕 才会释放SessionLockingComponent 防止多次点击登录
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.Account.Trim().GetHashCode()))//锁,防止用户同时点击注册
                {
                    //查询数据库
                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<Account>(d =>
                            d.AccountName.Trim().Equals(request.Account.Trim()));
                    Account account = null;
                    if (accountInfoList != null && accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        session.AddChild(account);
                        if (account.AccountType == (int)AccountType.BlackList)
                        {
                            response.Error = ErrorCode.ERR_AccountInBlackListError;
                            reply();//用于发送response这条消息
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }

                        //判断账号的登录密码
                        if (!account.Password.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_LoginPasswordError;
                            reply();//用于发送response这条消息
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }

                    }
                    else
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.Account.Trim();
                        account.Password = request.Password;
                        account.CreateTimer = TimeHelper.ServerNow();
                        account.AccountType = (int)AccountType.General;

                        //DomainZone() 代表区服 比如1服 2服 3服等
                        //GetZoneDB() 获得1服数据库 2服数据库 等
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save(account);
                    }

                    //账号中心服务器
                    StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "LoginCenter");
                    long loginCenterInstanceId = startSceneConfig.InstanceId;
                    L2A_LoginAccountResponse l2A_LoginAccountResponse = (L2A_LoginAccountResponse)await ActorMessageSenderComponent.Instance.Call(loginCenterInstanceId,new A2L_LoginAccountRequest() 
                    {
                        AccountId = account.Id,
                    });
                    if (l2A_LoginAccountResponse.Error!=ErrorCode.ERR_Success)
                    {
                        response.Error = l2A_LoginAccountResponse.Error;
                        reply();
                        session?.Disconnect().Coroutine();
                        account?.Dispose();
                        return;
                    }

                    //顶号操作
                    long accountSessionInstanceId = session.DomainScene().GetComponent<AccountSessionsComponent>().Get(account.Id);
                    Session otherSession = Game.EventSystem.Get(accountSessionInstanceId) as Session;
                    //如果otherSession不为空,则说明有人已经在用这个账号了
                    otherSession?.Send(new A2C_Disconnect()
                    {
                        Error = 0,
                    });
                    otherSession?.Disconnect().Coroutine();
                    session.DomainScene().GetComponent<AccountSessionsComponent>().Add(account.Id, session.InstanceId);
                    session.AddComponent<AccountCheckOutTimerComponent, long>(account.Id);

                    // 记录每个登录的令牌令牌
                    string Token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString();
                    session.DomainScene().GetComponent<TokenComponent>().Remove(account.Id);
                    session.DomainScene().GetComponent<TokenComponent>().Add(account.Id, Token);

                    //返回给服务器的
                    response.AccountId = account.Id;
                    response.Token = Token;
                    reply();
                    account?.Dispose();
                }
            }
        }
    }
}
```
