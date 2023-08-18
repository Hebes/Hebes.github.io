# ET6.0实操-7-账号中心服

## 准备工作

```proto
// 登录中心服务器发送到Gate服务器
//ResponseType G2L_DisconnectGateUnitResponse
message L2G_DisconnectGateUnitRequest // IActorRequest
{
    int32 RpcId = 90;
    int64 AccountId = 1;
}

message G2L_DisconnectGateUnitResponse // IActorResponse
{
    int32 RpcId = 1;
    int32 Error = 2;
    string Message = 3;
}
```

```C#
namespace ET
{
    public static class CoroutineLockType
    {
        //添加部分
        public const int LogoutCenterLock = 10;
        public const int GateLoginLock = 11;
    }
}
```

```C#
using System.Collections.Generic;


namespace ET
{
    public static class RealmGateAddressHelper
    {
        //添加部分
        /// <summary> 新增获取固定的区服 </summary>
        public static StartSceneConfig GetGate(int zone, long accountId)
        {
            List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[zone];
            int n = accountId.GetHashCode() % zoneGates.Count;
            return zoneGates[n];
        }
    }
}
```

```C#
using System;
using System.Net;

namespace ET
{
    [MessageHandler]
    public class C2R_LoginHandler : AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response, Action reply)
        {
            // 随机分配一个Gate
            StartSceneConfig config = RealmGateAddressHelper.GetGate(session.DomainZone(),1);
            Log.Debug($"gate address: {MongoHelper.ToJson(config)}");

            // 向gate请求一个key,客户端可以拿着这个key连接gate
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey) await ActorMessageSenderComponent.Instance.Call(
                config.InstanceId, new R2G_GetLoginKey() {Account = request.Account});  
            response.Address = config.OuterIPPort.ToString();
            response.Key = g2RGetLoginKey.Key;
            response.GateId = g2RGetLoginKey.GateId;
            reply();
        }
    }
}
```

![17](\../../Image/ET6.0实操-账号/17.png)

```C#
using System;

namespace ET
{
    public class L2G_DisconnectGateUnitRequestHandler : AMActorRpcHandler<Scene, L2G_DisconnectGateUnitRequest, G2L_DisconnectGateUnitResponse>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateUnitRequest request, G2L_DisconnectGateUnitResponse response, Action reply)
        {
            long accountId = request.AccountId;

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.GateLoginLock, accountId.GetHashCode()))
            {
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player gateUnit = playerComponent.Get(accountId);
                if (gateUnit == null)
                {
                    reply();
                    return;
                }
                playerComponent.Remove(accountId);

                //tudo 临时处理
                gateUnit.Dispose();
            }
            reply();
        }
    }
}
```

```C#
using System;

namespace ET
{
    [ActorMessageHandler]
    public class A2L_LoginAccountRequestHandler : AMActorRpcHandler<Scene, A2L_LoginAccountRequest, L2A_LoginAccountResponse>
    {
        protected override async ETTask Run(Scene scene, A2L_LoginAccountRequest request, L2A_LoginAccountResponse response, Action reply)
        {
            long accountId = request.AccountId;

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LogoutCenterLock, accountId.GetHashCode()))
            {
                if (!scene.GetComponent<LoginInfoRecordComponent>().IsExit(accountId))
                {
                    reply();
                    return;
                }

                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(accountId);//获取区服ID
                StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(zone, accountId);//获取区服信息

                //为啥使用MessageHelper.CallActor而不用ActorMessageSenderComponent.Instance.Call
                //主要原因:好看
                var g2LDisconnectGateUnitResponse = (G2L_DisconnectGateUnitResponse)await MessageHelper.CallActor(gateConfig.InstanceId, new L2G_DisconnectGateUnitRequest()
                {
                    AccountId = accountId,
                });

                response.Error = g2LDisconnectGateUnitResponse.Error;
                reply();
            }
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
            if (!Regex.IsMatch(request.Password.Trim(), @"^[A-Za-z0-9]+$"))//@" ^ (?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"
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
                    Log.Debug("连接数据库的是:  "+ session.DomainZone().ToString());
                    //查询数据库
                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<Account>(d =>
                            d.AccountName.Equals(request.Account.Trim()));
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

登录成功后的界面显示 账号密码:Uw123456

```C#
using System;
using UnityEngine.UI;

namespace ET
{
    public static class DlgLoginSystem
    {

        public static void RegisterUIEvent(this DlgLogin self)
        {
            self.View.E_LoginButton.AddListenerAsync(() => { return self.OnLoginClickHandler(); });
        }

        public static void ShowWindow(this DlgLogin self, Entity contextData = null)
        {

        }

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
                Log.Debug("登录成功");
                //TODO 显示登录之后的页面逻辑比如登录后的显示页面
                self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Login);
                self.DomainScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Lobby);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public static void HideWindow(this DlgLogin self)
        {

        }

    }
}
```

![20](\../../Image/ET6.0实操-账号/20.png)

![18](\../../Image/ET6.0实操-账号/18.png)

![19](\../../Image/ET6.0实操-账号/19.png)
