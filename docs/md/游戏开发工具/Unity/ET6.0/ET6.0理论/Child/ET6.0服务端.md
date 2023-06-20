# ET6.0服务端

**[07-ET框架的数据库连接](<https://blog.csdn.net/qq_46179975/article/details/128414611>)**

**[ET6.0服务器框架学习笔记（二、一条登录协议）](<https://blog.csdn.net/kylinok/article/details/120106628?spm=1001.2014.3001.5502>)**

**[基于ET6.0框架登录系统全图文流程详解](<https://blog.csdn.net/Q540670228/article/details/123592622>)**

**[05-ET框架的前后端通信1](<https://blog.csdn.net/qq_46179975/article/details/128139684>)**

**[06-ET框架的前后端通信2](<https://blog.csdn.net/qq_46179975/article/details/128156432>)**

**[07-ET框架的数据库连接](<https://blog.csdn.net/qq_46179975/article/details/128414611>)**

框架有一个Sesion对象，提供了Send()，Call()方法，玩家与服务器之间的通信就是通过Session来发起的。

Session对象就是用发送目标的IP地址与端口构建的对象，所以我们前面发送消息给服务端的例子中，就是用配置文件中配置的服务器地址与端口构建了Session实例，再发送消息给服务器的。

**命名规范：**

消息类型名+Handler，所有消息都需添加[MessageHandler]特性

**重点：**

1. Request消息，实现AMRpcHandler<Request消息类型，Response消息类型>接口 实现的函数为异步函数
2. Message消息，需要实现AMHandler<Message消息类型>接口 Run函数仍为异步函数

`IMessage:` 不需要返回结果的消息，就是一个单向传输的消息。
`IRequest:` 需要返回结果的消息，此类消息，是发送请求，与
`IResponse` 配对，实现一个Rpc调用过程，此类消息，是接受请求，与IRequest配对，实现一个Rpc返回过程
`IResponse:` 用于回复的消息
`IRequest/IResponse消息对` 让使用者可以把发送和接受写在一个函数之中（这个函数本身必须是一个协程），这样使用者在写代码的时候，思路比较连贯，代码容易看懂，这就是【RPC(远程过程调用)】。
`IActorMessage、IActorLocationMessage`: 不需要返回结果的Actor消息
`IActorRequest` 请求,需要实体回复
`IActorLocationRequest`: 需要返回结果的Actor消息,定位服务器(目标即使切换服务器了也能收到请求)
`IActorResponse` 实体回复
`IActorLocationResponse:` 用于回复的Actor消息

**发送消息：**

1. Run函数的session是连接发送方客户端的session
2. reply委托调用时就会将response消息发回客户端
3. Run函数的session参数是连接发送放客户端的session
4. 是Call请求，都是RPC请求，所以消息中都要有RpcID的，而返回消息中还要有Error

`普通消息：`如果只是发送一条消息，不需要对方作响应（返回消息），那就是一条普通消息。用Session对象的Send方法。

`RPC消息：`如果发送一条消息，还需要对方响应，这叫RPC消息，我们把RPC叫远程调用，对方肯定接收你的消息后，要调用一系列方法进行处理，再返回你一个结果。用Session对象的Call方法。

**proto文件的作用：**

1. InnerMessage.proto：不可以传递Entity数据,用于服务器之间的通讯
2. MongoMessage.proto：可以传递Entity数据,用于服务器之间的通讯带Entity实体。
3. OuterMessage.proto：服务器和客户端之间的通信,不属于服务器内部协议里面存放的都是客户端与服务器通信定义的协议数据。

**其他：**

1. request和response参数为接收到的消息和回复的消息
2. message参数为接收到的消息

**杂项：**

1. int32是表示int类型数据，int64是long类型，string跟我们熟悉的一样。
我们把接口定义的字段 = 90 开始，后面自定义的字段+1就可以
2. 服务器内部全部使用actor发送消息，比如realm发给gate，其实是发个actor消息到gate scene
