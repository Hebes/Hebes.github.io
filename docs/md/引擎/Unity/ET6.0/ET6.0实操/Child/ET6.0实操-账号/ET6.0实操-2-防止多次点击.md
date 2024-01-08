# ET6.0实操-账号防止多次点击2

## 准备工作

错误码

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
    }
}
```

锁

```C#
namespace ET
{
    public static class CoroutineLockType
    {
        public const int None = 0;
        public const int Location = 1;                  // location进程上使用
        public const int ActorLocationSender = 2;       // ActorLocationSender中队列消息 
        public const int Mailbox = 3;                   // Mailbox中队列
        public const int UnitId = 4;                    // Map服务器上线下线时使用
        public const int DB = 5;
        public const int Resources = 6;
        public const int ResourcesLoader = 7;
        public const int LoadUIBaseWindows = 8;

        //新增
        public const int LoginAccount = 9;

        public const int Max = 100; // 这个必须最大
    }
}
```

限制玩家频繁点击的按钮监听方法

```C#
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class UIEventComponent : Entity,IAwake,IDestroy
    {
        public static UIEventComponent Instance { get; set; }
        public readonly Dictionary<WindowID, IAUIEventHandler> UIEventHandlers = new Dictionary<WindowID, IAUIEventHandler>();
        //IsClicked这个没有的话就添加,这个框架里面已经有了
        public bool IsClicked { get; set; }
    }
}
```

```C#
//自带就有
public static void AddListenerAsync(this Button button, Func<ETTask> action)
{ 
    button.onClick.RemoveAllListeners();
    async ETTask clickActionAsync()
    {
        UIEventComponent.Instance?.SetUIClicked(true);
        await action();
        UIEventComponent.Instance?.SetUIClicked(false);
    }
         
    button.onClick.AddListener(() =>
    {
        if ( UIEventComponent.Instance == null) return;
        if (UIEventComponent.Instance.IsClicked) return;
             
        clickActionAsync().Coroutine();
    });
}
```

## 实际使用

```C#
按钮..AddListenAsync(() => { return self.OnLoginClickHandler(); });
```

```C#
public static async ETTask OnLoginClickHandler(this DlgLogin self)
{
    try
    {
        int errorCode = await LoginHelper.Login(
                       self.DomainScene(),
                       ConstValue.LoginAddress,
                       self.View.E_AccountInputField.GetComponent<InputField>().text,
                       self.View.E_PasswordInputField.GetComponent<InputField>().text);
        if (errorCode != ErrorCode.ERR_Success) 
        {
            Log.Error(errorCode.ToString());
            return;
        }
        //TODO 显示登录之后的页面逻辑比如登录后的显示页面
    }
    catch (Exception e)
    {
        Log.Error(e.ToString());
    }
}
```

Session锁

![9](\../../Image/ET6.0实操-账号/9.png)

```C#
namespace ET
{
    [ComponentOf(typeof(Session))]
    public class SessionLockingComponent:Entity,IAwake
    {

    }
}
```

防止消息没发完就关闭连接了

![10](\../../Image/ET6.0实操-账号/10.png)

![11](\../../Image/ET6.0实操-账号/11.png)

```C#
namespace ET
{
    /// <summary> 帮助类 </summary>
    public static class DisconnectHelper
    {
        /// <summary> 防止消息没有发送完成就关闭了链接 </summary>
        public static async ETTask Disconnect(this Session self )
        {
            if (self == null || self.IsDisposed) return;

            long instanceId=self.InstanceId;

            await TimerComponent.Instance.WaitAsync(1000);

            if (self.InstanceId != instanceId) return;

            self.Dispose();
        }
    }
}
```

```C#
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class TokenComponent : Entity, IAwake
    {
        public readonly Dictionary<long, string> TokenDictionary = new Dictionary<long, string>();
    }
}
```

```C#
namespace ET
{
    [FriendClass(typeof(ET.TokenComponent))]
    public static class TokenComponentSystem
    {
        /// <summary> 令牌添加 </summary>
        public static void Add(this TokenComponent self, long key, string token)
        {
            self.TokenDictionary.Add(key, token);
            self.TimeOutRemoveKey(key, token).Coroutine();//启动一个协程
        }
        /// <summary> 令牌获取 </summary>
        public static string Get(this TokenComponent self, long key)
        {
            string value = null;
            self.TokenDictionary.TryGetValue(key, out value);
            return value;
        }
        /// <summary> 令牌移除 </summary>
        public static void Remove(this TokenComponent self, long key)
        {
            if (self.TokenDictionary.ContainsKey(key))
                self.TokenDictionary.Remove(key);
        }
        /// <summary> 令牌过期移除 </summary>
        private static async ETTask TimeOutRemoveKey(this TokenComponent self, long key, string tokenKey)
        {
            //等待10分钟
            await TimerComponent.Instance.WaitAsync(600000);
            //获取令牌
            string onlineToken = self.Get(key);
            //令牌不能为空并且和以前的令牌保持一致
            if (!string.IsNullOrEmpty(onlineToken) && onlineToken == tokenKey)
                self.Remove(key);
        }
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
                    break;
            }

            return scene;
        }
    }
}
```

后端的消息接受和处理

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
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount,request.Account.Trim().GetHashCode()))//锁,防止用户同时点击注册
                {
                    //查询数据库
                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<Account>(d =>
                            d.AccountName.Equals(request.Account.Trim()));
                    Account account = null;
                    if (accountInfoList !=null && accountInfoList.Count > 0)
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
