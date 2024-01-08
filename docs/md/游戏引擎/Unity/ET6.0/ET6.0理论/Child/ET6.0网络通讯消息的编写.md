# ET6.0网络通讯消息的编写

Protobuf中

```protobuf
//ResponseType R2C_LoginTest //定义回复类型
message C2R_LoginTest // IRequest //该注释会让该程序需要服务器回复
{
   int32 RpcId = 90;
   string Account = 1;
   string Password = 2;
}
message R2C_LoginTest // IResponse
{
   //前三条必须有
   int32 RpcId = 90;
   int32 Error = 91;
   string Message = 92;
   
   string Gateddress = 1;
   string key = 2;
}
message C2R_SayHello // IMessage //不需要回复的就不需要RpcId
{
   string Hello = 1;
}

message R2C_SayGoodBye // IMessage
{
   string GoodBye = 1;
}
```
