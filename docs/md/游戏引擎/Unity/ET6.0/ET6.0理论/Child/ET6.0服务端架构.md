# ET6.0服务端架构

**[ET5.0服务端架构(重点需要看的)](<https://www.cnblogs.com/cj8988/p/14485518.html>)**

![图片](..\Image/ET/ET1.png)

![01ET6.0服务端](..\Image/ET/ET2.png)

**Manager管理服务器-- AppManagerComponent:**

主要功能：读取配置文件，每隔5秒检测所有的服务器是否健在，如果不健在，则重启该服务器。

**Realm登录服务器:**

**【RealmGateAddressComponent】:**
主要功能：在收到客户端发来的C2R_LoginHandler消息以后，随机挑选一个Gate，让其加入。

**Gate网关服务器，用户长链接的服务器:**

**【PlayerComponent】:**
主要功能：保存玩家信息（目前只有账号和UnitId）。
**【NetInnerComponent】:**
主要功能：与Realm和Map服务器通讯。
**【GateSessionKeyComponent】:**
主要功能：保存所有Gate里的玩家的Session的Key
**【ActorLocationSenderComponent】:**
主要功能：向Map内的指定玩家发送消息，如果发送失败，则向Location服务器索要新的地址。

**Location地址服务器:**

**【LocationComponent】:**
主要功能：保存了所有玩家的地址（Key是玩家的Id，Value是玩家的InstanceId），如果玩家在切换Map的时候，要把这里锁住。

**Map场景服务器:**

**【NetInnerComponent】:**
与Gate通信。注意，Map并不与玩家直接通讯，全都由Gate转发。
**【ActorMessageSenderComponent】:**
与Gate通讯。这里可以获得ActorId，而ActorId是找到对应Map的关键信息：IdGenerater.AppId。
对于开房间的游戏来说，一个Map服务器可能会有很多个房间。
