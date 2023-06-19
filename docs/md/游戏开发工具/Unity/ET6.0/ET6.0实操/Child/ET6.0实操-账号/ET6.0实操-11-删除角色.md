# ET6.0实操-11-删除角色

## 准备工作

OuterMessage.proto

```C#
//删除角色
//ResponseType A2C_GetRoles
message C2A_DeleteRoles // IRequest
{
    int32 RpcId = 90;
    string Token = 1;
    int64 AccountId = 2;
    int64 RoleInfoId = 3;
    int32 ServerId = 4;
}

message A2C_DeleteRoles // IResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;

    int64 DeleteRloeInfoId = 1;
}
```

 错误码 ErrorCode.cs

```C#
/// <summary> 游戏角色不存在 </summary>
public const int ERR_RoleNotExistError = 200012;
```

客户端 Unity.Hotfix -> Codes -> Hotfix- > Demo -> Login -> LoginHelper.cs 添加拓展

```C#
/// <summary> 删除角色 </summary>
public static async ETTask<int> DeleteRole(Scene zoneScene)
{
    A2C_DeleteRoles a2C_DeleteRoles = null;
    try
    {
        a2C_DeleteRoles = (A2C_DeleteRoles)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_DeleteRoles()
        {
            AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
            Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
            RoleInfoId = zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId,
            ServerId= zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId,
        });
    }
    catch (Exception e)
    {
        Log.Error(e.ToString());
        return ErrorCode.ERR_NetWorkError;
    }

    if (a2C_DeleteRoles.Error != ErrorCode.ERR_Success)
    {
        Log.Error(a2C_DeleteRoles.Error.ToString());
        return a2C_DeleteRoles.Error;
    }

    //删除本地角色信息
    int index = zoneScene.GetComponent<RoleInfosComponent>().roleInfosList.FindIndex((index) =>
     {
         return index.Id == a2C_DeleteRoles.DeleteRloeInfoId;
     });

    zoneScene.GetComponent<RoleInfosComponent>().roleInfosList.RemoveAt(index);

    return ErrorCode.ERR_Success;
}
```

服务端 Server.Hotfix -> Demo -> Role -> Handler -> C2A_DeleteRolesHandler.cs

```C#

发现没有的代码请参考github的ETLogin项目

using System;

namespace ET
{
    /// <summary> 客户端法网服务端的消息的处理 </summary>
    public class C2A_DeleteRolesHandler : AMRpcHandler<C2A_DeleteRoles, A2C_DeleteRoles>
    {
        protected override async ETTask Run(Session session, C2A_DeleteRoles request, A2C_DeleteRoles response, Action reply)
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
                //创建和删除都是异步的过程,所以都用CreatRoleLock这个锁住.保证逻辑顺序的正确性
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreatRoleLock, request.AccountId))
                {
                    var roleInfos = await DBManagerComponent.Instance.GetZoneDB(request.ServerId).
                        Query<RoleInfo>(d => d.Id == request.RoleInfoId && d.ServerId == request.ServerId);

                    //有角色了
                    if (roleInfos == null || roleInfos.Count <= 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNotExistError;
                        reply();
                        return;
                    }

                    var roleInfo = roleInfos[0];
                    session.AddChild(roleInfo);
                    roleInfo.State = (int)RoleInfoState.Freeze;//不能直接删除,用冻结状态
                    await DBManagerComponent.Instance.GetZoneDB(request.ServerId).Save<RoleInfo>(roleInfo);
                    response.DeleteRloeInfoId = roleInfo.Id;
                    roleInfo?.Dispose();//销毁
                    reply();
                }
            }
        }
    }
}
```

## 实际使用

```C#
 /// <summary> 删除角色 </summary>
public static async ETTask OnDelectRoleButton(this DlgRole self)
{
    if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
    {
        Log.Error("请选择需要删除的角色");
        return;
    }

    try
    {
        int errorCode = await LoginHelper.DeleteRole(self.ZoneScene());
        if (errorCode != ErrorCode.ERR_Success)
        {
            Log.Debug(errorCode.ToString());
            return;
        }
        self.OnShowRole();
        //清理不需要的赋值
        self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId = 0;
        self.roleInfo = null;
        self.View.ERoleNameText.text = string.Empty;
    }
    catch (Exception e)
    {
        Log.Debug(e.ToString());
    }
}
```
