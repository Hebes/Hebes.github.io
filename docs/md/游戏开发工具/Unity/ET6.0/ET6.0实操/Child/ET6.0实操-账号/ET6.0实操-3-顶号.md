# ET6.0实操-3-顶号

## 准备工作

```C#
namespace ET
{
    public static partial class TimerType
    {
        // 框架层0-1000，逻辑层的timer type 1000-9999
        public const int MoveTimer = 1001;
        public const int AITimer = 1002;
        public const int SessionAcceptTimeout = 1003;

        //新增
        public const int AccountSessionCheckOutTime = 1004;
        // 不能超过10000
    }
}
```

```C#
namespace ET
{
    public static class ConstValue
    {
        //请查看Excel中的配置
        public const string LoginAddress = "127.0.0.1:10005";//登录账号的服务器
    }
}
```

![12](\../../Image/ET6.0实操-账号/12.png)

![13](\../../Image/ET6.0实操-账号/13.png)

```C#
namespace ET
{
    [ComponentOf(typeof(Session))]
    public class AccountCheckOutTimerComponent : Entity, IAwake<long>, IDestroy
    {
        public long Timer = 0;
        public long AccountId = 0;
    }
}
```

```C#
namespace ET
{

    [Timer(TimerType.AccountSessionCheckOutTime)]
    public class AccountSessionCheckOutTimer : ATimer<AccountCheckOutTimerComponent>
    {
        public override void Run(AccountCheckOutTimerComponent self)
        {
            try
            {
                self.DelectSession();
            }
            catch (System.Exception e)
            {
                Log.Debug(e.ToString());
            }
        }
    }

    public class AccountCheckOutTimerComponentAwakeSystem : AwakeSystem<AccountCheckOutTimerComponent, long>
    {
        public override void Awake(AccountCheckOutTimerComponent self, long a)
        {
            self.AccountId = a;
            TimerComponent.Instance.Remove(ref self.Timer);
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 600000, TimerType.AccountSessionCheckOutTime, self);
        }
    }
    public class AccountCheckOutTimerComponentDestroySystem : DestroySystem<AccountCheckOutTimerComponent>
    {
        public override void Destroy(AccountCheckOutTimerComponent self)
        {
            self.AccountId = 0;
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }

    [FriendClass(typeof(AccountCheckOutTimerComponent))]
    public static class AccountCheckOutTimerComponentSystem
    {
        public static void DelectSession(this AccountCheckOutTimerComponent self)
        {
            Session session = self.GetParent<Session>();
            long sessionInstanceId = session.DomainScene().GetComponent<AccountSessionsComponent>().Get(self.AccountId);
            if (session.InstanceId == sessionInstanceId)
                session.DomainScene().GetComponent<AccountSessionsComponent>().Remove(self.AccountId);
            session?.Send(new A2C_Disconnect() 
            {
                Error =1,
            });
            session?.Disconnect().Coroutine();
        }
    }
}
```

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    //管理Session 用于顶号操作
    [ComponentOf(typeof(Scene))]
    public class AccountSessionsComponent :Entity,IAwake ,IDestroy
    {
        public Dictionary<long, long> AccountSessionDic { get; set; } = new Dictionary<long, long>(); 
    }
}
```

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class AccountSessionsComponentAwakeSystem : AwakeSystem<AccountSessionsComponent>
    {
        public override void Awake(AccountSessionsComponent self)
        {
        }
    }
    public class AccountSessionsComponentDestroySystem : DestroySystem<AccountSessionsComponent>
    {
        public override void Destroy(AccountSessionsComponent self)
        {
            self.AccountSessionDic.Clear();
        }
    }

    public static class AccountSessionsComponentSystem
    {
        public static long Get(this AccountSessionsComponent self,long accountId)
        {
            if (!self.AccountSessionDic.TryGetValue(accountId,out long instanceId)) return 0;
            return instanceId;
        }
        public static void  Add(this AccountSessionsComponent self, long accountId, long sessionInstanceId)
        {
            if (self.AccountSessionDic.ContainsKey(accountId))
            {
                self.AccountSessionDic[accountId] = sessionInstanceId;
                return;
            }
            self.AccountSessionDic.Add(accountId, sessionInstanceId);
        }

        public static void Remove(this AccountSessionsComponent self,long accountId)
        {
            if (self.AccountSessionDic.ContainsKey(accountId))
                self.AccountSessionDic.Remove(accountId);
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
                    session.AddComponent<AccountCheckOutTimerComponent,long>(account.Id);

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
