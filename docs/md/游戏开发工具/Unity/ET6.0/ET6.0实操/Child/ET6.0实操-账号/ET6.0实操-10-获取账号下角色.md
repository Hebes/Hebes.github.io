# ET6.0实操-10-获取账号下角色

## 准备工作

OuterMessage.proto

```C#
//请求角色信息消息
//ResponseType A2C_GetRoles
message C2A_GetRoles // IRequest
{
    int32 RpcId = 90;
    string Token = 1;
    int64 AccountId = 2;
    int32 ServerId = 3;
}

message A2C_GetRoles // IResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;

    repeated RoleInfoProto RloeInfo = 1;
}
```

客户端 Unity.Hotfix\Codes\Hotfix\Demo\Login\LoginHelper.cs 添加拓展方法

```C#
 /// <summary> 获取角色信息 </summary>
public static async ETTask<int> GetRoles(Scene zoneScene)
{
    A2C_GetRoles a2C_GetRoles = null;
    try
    {
        a2C_GetRoles = (A2C_GetRoles)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetRoles()
        {
            AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
            Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
            ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId,
        });
    }
    catch (Exception e)
    {
        Log.Error(e.ToString());
        return ErrorCode.ERR_NetWorkError;
    }

    if (a2C_GetRoles.Error!= ErrorCode.ERR_Success)
    {
        Log.Error(a2C_GetRoles.Error.ToString());
        return a2C_GetRoles.Error;
    }

    zoneScene.GetComponent<RoleInfosComponent>().roleInfosList.Clear();
    foreach (var roleInfoProto in a2C_GetRoles.RloeInfo)
    {
        RoleInfo  roleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
        roleInfo.FromMessage(roleInfoProto);
        zoneScene.GetComponent<RoleInfosComponent>().roleInfosList.Add(roleInfo);
    }
    return ErrorCode.ERR_Success;
}
```

服务端 Server.Hotfix\Demo\Role\Handler\C2A_GetRolesHandler.cs

```C#
using System;

namespace ET
{
    public class C2A_GetRolesHandler : AMRpcHandler<C2A_GetRoles, A2C_GetRoles>
    {
        protected override async ETTask Run(Session session, C2A_GetRoles request, A2C_GetRoles response, Action reply)
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
                //协程锁一样的CreatRoleLock  保证不会进行角色查询
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreatRoleLock, request.AccountId))
                {
                     var roleInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).
                        Query<RoleInfo>(d => d.AccountId == request.AccountId //账号ID
                        && d.State == (int)RoleInfoState.Normal//角色的不是删除或者冻结的
                        && d.ServerId == request.ServerId);//游戏的区服

                    //没有角色
                    if (roleInfos == null || roleInfos.Count == 0)
                    {
                        reply();
                        return;
                    }

                    foreach (var roleInfo in roleInfos)
                    {
                        response.RloeInfo.Add(roleInfo.ToMessage());
                        roleInfo?.Dispose();//释放,可以不释放,这个不释放系统会自动释放
                    }

                    roleInfos.Clear();
                    reply();
                }
            }
        }
    }
}
```

## 实际使用

DlgServerSystem.cs 选择服务器进入的按钮的监听

```C#
 /// <summary> 点击按钮事件 </summary> 
public static async void OnEnterMapCilckHandler(this DlgServer self)
{
    if (self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId == 0)
    {
        Log.Error("请选择服务器");
        return;
    }

    try
    {
        //获取角色列表
        int errorCode = await LoginHelper.GetRoles(self.ZoneScene());
        if (errorCode != ErrorCode.ERR_Success)
        {
            Log.Error(errorCode.ToString());
            return;
        }

        //跳转场景
        //关闭选择服务器界面
        self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Server);
        //打开选择角色界面
        self.DomainScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Role);
    }
    catch (Exception e)
    {
        Log.Error(e.ToString());
    }
}
```
