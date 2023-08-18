# ET6.0服务器命名以及消息类的端到端的命名方式

**[ET6.0服务器框架学习笔记（二、一条登录协议）](<https://blog.csdn.net/kylinok/article/details/120106628>)**

**[ET6.0服务器框架学习笔记（三、一条内网actor消息）](<https://blog.csdn.net/kylinok/article/details/120246339>)**

**[服务器命名以及消息类的端到端的命名方式](<https://et-framework.cn/d/51>)**

**[参考1：泰课学院（搜Et）课时3](<https://www.taikr.com/goods/show/554?targetId=1053&preview=0>)**

**服务器名称:**

- Manager : 对服务器进程进行管理
- Realm : 登录服务器 ( 验证账号密码 相当于LoginServer 祖传叫法,- 你想叫什么随你)
- Gate : 网关服务器
- DB : 数据库服务器
- Location : 位置服务器
- Map : 地图服务器
- Client : 客户端
- All Server: 所有服务器集合
**消息命名( 端到端的命名方式 )**
- C2R_Ping = Client to Realm
- R2G_GetLoginKey = Realm to Gate

```text
M一般情况下代表map, 特殊代表Manager ,具体代表什么看消息协议内容
```

- G2M_CreateUnit = Gate to Map
- C2M_TestRequest = Client to Map
- M2C_CreateUnits = Map to Client
- M2A_Reload = Manager to AllServer 通知全部服务器热更新

**各服务器的作用(摘录自文档ET框架笔记):**

**1、Manager：**

连接客户端的外网和连接内部服务器的内网，对服务器进程进行管理，自动检测和启动服务器进程。加载有内网组件NetInnerComponent，外网组件NetOuterComponent，服务器进程管理组件。自动启动突然停止运行的服务器，保证此服务器管理的其它服务器崩溃后能及时自动启动运行。

**2、Realm：** 对Actor消息进行管理（添加、移除、分发等），连接内网和外网，对内网服务器进程进行操作，随机分配Gate服务器地址。内网组件NetInnerComponent，外网组件NetOuterComponent，Gate服务器随机分发组件。客户端登录时连接的第一个服务器，也可称为登录服务器。

**3、Gate：** 对玩家进行管理，对Actor消息进行管理（添加、移除、分发等），连接内网和外网，对内网服务器进程进行操作，随机分配Gate服务器地址，对Actor消息进程进行管理，对玩家ID登录后的Key进行管理。加载有玩家管理组件PlayerComponent，管理登陆时联网的Key组件GateSessionKeyComponent。

**4、Location：** 连接内网，服务器进程状态集中管理（Actor消息IP管理服务器）。加载有内网组件NetInnerComponent，服务器消息处理状态存储组件LocationComponent。对客户端的登录信息进行验证和客户端登录后连接的服务器，登录后通过此服务器进行消息互动，也可称为验证服务器。

**5、Map：** 连接内网，对ActorMessage消息进行管理（添加、移除、分发等），对场景内现在活动物体存储管理，对内网服务器进程进行操作，对Actor消息进程进行管理，对Actor消息进行管理（添加、移除、分发等），服务器帧率管理。服务器帧率管理组件ServerFrameComponent。

**6、AllServer：** 将以上服务器功能集中合并成一个服务器。另外增加DB连接组件DBComponent

**Benchmark：** 连接内网和测试服务器承受力。加载有内网组件NetInnerComponent，服务器承受力测试组件BenchmarkComponent。