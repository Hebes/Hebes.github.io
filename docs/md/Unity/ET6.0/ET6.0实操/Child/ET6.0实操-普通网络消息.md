# ET6.0实操-普通网络消息

协议消息的定义,客户端对服务器的定义在OuterMessage.proto

打开文件,往下添加下面代码

```proto
//需要回复消息的定义
//ResponseType R2C_LoginTest
message C2R_LoginTest // IRequest
{
    int32 RpcId = 90;
    string Account = 1;
    string Password = 2;
}

message R2C_LoginTest // IResponse
{
    //回复的消息前面的这三个变量必须存在
    int32 RpcId = 90; // 必须90开始
    int32 Error = 91; // 必须91开始
    string Message = 92; // 必须92开始
    //具体回复内容从1开始
    string GateAddress = 1;
    string Key = 2;
}

//不需要回复消息的定义 发送给网关的
message C2R_SayHello // IMessage
{
    //具体回复内容从1开始
    string Hello = 1;
}

//游戏服务器主动下推消息给客户端
message R2C_SayGoodBye // IMessage
{
    //具体回复内容从1开始
    string GoodBye = 1;
}
```

修改UILoginComponentSystem.cs中的登录代码

```C#
[FriendClass(typeof(UILoginComponent))]
public static class UILoginComponentSystem
{
    public static void OnLogin(this UILoginComponent self)
    {
       //LoginHelper.Login(
       // self.DomainScene(), 
       // ConstValue.LoginAddress, 
       // self.account.GetComponent<InputField>().text, 
       // self.password.GetComponent<InputField>().text).Coroutine();
       LoginHelper.Logintest(
           self.DomainScene(),
           ConstValue.LoginAddress
           ).Coroutine();
    }
}
```

修改LoginHelper.cs

```C#
using System;

namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask Login(Scene zoneScene, string address, string account, string password)
        {
            try
            {
                // 创建一个ETModel层的Session
                R2C_Login r2CLogin;
                Session session = null;
                try
                {
                    session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                    {
                        r2CLogin = (R2C_Login)await session.Call(new C2R_Login() { Account = account, Password = password });
                    }
                }
                finally
                {
                    session?.Dispose();
                }

                // 创建一个gate Session,并且保存到SessionComponent中
                Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(r2CLogin.Address));
                gateSession.AddComponent<PingComponent>();
                zoneScene.AddComponent<SessionComponent>().Session = gateSession;

                G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(
                    new C2G_LoginGate() { Key = r2CLogin.Key, GateId = r2CLogin.GateId });//

                Log.Debug("登陆gate成功!");

                Game.EventSystem.Publish(new EventType.LoginFinish() { ZoneScene = zoneScene });
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        //新添加的方法
        public static async ETTask Logintest(Scene zoneScene, string address)
        {
            try
            {
                Session session = null;
                R2C_LoginTest r2C_LoginTest = null;
                try
                {
                    session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                    {
                        //需要回复的消息
                        r2C_LoginTest = (R2C_LoginTest)await session.Call(new C2R_LoginTest()
                        {
                            Account = "123",
                            Password = "123",
                        });
                        Log.Debug(r2C_LoginTest.Key.ToString());

                        ///客户端发送不用接受返回消息
                        session.Send(new C2R_SayHello(){Hello="我是从客户端发往服务端的消息 IMessage类型!" });
                        await TimerComponent.Instance.WaitAsync(2000);
                    }
                    
                }
                finally
                {
                    session?.Dispose();
                }
            }
            catch (Exception e)
            {

                Log.Error(e);
            }
        }
    }
}
```

**服务端:**

![1](\../Image/ET6.0实操-普通网络消息/1.png)

需要回复的消息类型编写

命名规则为XXX+Handler,继承AMRpcHandler

```C#
using System;

namespace ET
{
    [MessageHandler]
    public class R2C_LoginTestHandler : AMRpcHandler<C2R_LoginTest, R2C_LoginTest>
    {
        protected override async ETTask Run(Session session, C2R_LoginTest request, R2C_LoginTest response, Action reply)
        {
            response.Key = "服务端收到消息,这个消息是返回给客户端的!!";
            reply();
            await ETTask.CompletedTask;
        }
    }
}
```

不要回复消息的类型的编写

命名规则为XXX+Handler,继承AMHandler

```C#
namespace ET
{
    [MessageHandler]
    public class C2R_SayHelloHandler : AMHandler<C2R_SayHello>
    {
        protected override void Run(Session session, C2R_SayHello message)
        {
            Log.Debug("收到从客户端发过来的消息,消息为:  " + message.Hello);
            session.Send(new R2C_SayGoodBye() { GoodBye = "这条是从服务端发送的消息!!" });
        }
    }
}
```

**客户端:**

![2](\../Image/ET6.0实操-普通网络消息/2.png)

```C#
namespace ET
{
    [MessageHandler]
    public class R2C_SayHelloHandler : AMHandler<R2C_SayGoodBye>
    {
        protected override void Run(Session session, R2C_SayGoodBye message)
        {
            Log.Debug("从服务端收到的消息,内容为:   " + message.GoodBye);
        }
    }
}
```

编译客户端和服务端的代码

运行

![3](\../Image/ET6.0实操-普通网络消息/3.png)
