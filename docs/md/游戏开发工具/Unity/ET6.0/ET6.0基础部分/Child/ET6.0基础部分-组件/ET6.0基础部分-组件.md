# ET6.0基础部分-组件

**初始化第一个服务器Scene,Game.Scene,然后给这个Scene添加各种组件：**

- TimerComponent：定时器
- OpcodeTypeComponent：协议管理
- MessageDispatcherComponent：普通消息派发处理
- CoroutineLockComponent：协程锁
- ActorMessageSenderComponent：普通Actor消息发送
- ActorLocationSenderComponent：带定位的Location Actor消息发送
- LocationProxyComponent：定位服处理
- ActorMessageDispatcherComponent：Actor消息派发处理
- NumericWatcherComponent：数值变化订阅处理
- NetThreadComponent：网络服务管理（注意不是处理只是管理）

**针对服务器类型，给上面生成的服务Scene挂载各类功能组件：**

- NetKcpComponent：网外通信组件，用于监听来自客户端的通信处理
- PlayerComponent：用于管理生成的Player实体
- GateSessionKeyComponent：用于管理玩家认证的凭据（主要用于Gate认证玩家连接是否被允许）
- UnitComponent：用于管理Map中的Unit实体
- RecastPathComponent：寻路组件
- LocationComponent：用于管理实体对象ID与InstanceID之间的关系，提供了通过ID查询InstanceID的功能。

**其他:**

- NumericComponent：数值组件，请查看EBOOK
- ObjectWait: 用于等待，锁住方法继续执行直到收到返回通知。
