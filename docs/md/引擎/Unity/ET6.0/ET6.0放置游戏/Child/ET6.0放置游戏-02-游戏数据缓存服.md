# ET6.0放置游戏-02-游戏数据缓存服

## 准备工作

MongoMessage.proto

```proto
message UnitCache2Other_GetUnit // IActorResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;

    repeated Entity EntityList = 4;
    repeated string ComponentNameList = 5;
}
```

InnerMessage.proto

```proto
//----------------玩家缓存相关,增删改查消息定义----------------

//增加或者更新Unit缓存
//ResponseType UnitCache2Other_AddOrUpdateUnit
message Other2UnitCache_AddOrUpdateUnit // IActorRequest
{
    int32 RpcId = 90;

    int64 UnitId  = 1;  // 需要缓存的UnitId
    repeated string EntityTypes = 2; // 实体类型的名称
    repeated bytes EntityBytes = 3; // 实体序列化后的bytes
}

message UnitCache2Other_AddOrUpdateUnit // IActorResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;
}

//获取Unit缓存
//这条回复消息需要传送一个实体,所以定义在了MongoMessage.protoli里面
//ResponseType UnitCache2Other_GetUnit
message Other2UnitCache_GetUnit // IActorRequest
{
    int32 RpcId = 90;

    int64 UnitId  = 1;  // 需要缓存的UnitId
    repeated string ComponentNameList = 2; // 需要获取的组件名
}

//删除Unit缓存
//ResponseType UnitCache2Other_DeleteUnit
message Other2UnitCache_DeleteUnit // IActorRequest
{
    int32 RpcId = 90;

    int64 UnitId  = 1;  // 需要缓存的UnitId
}

message UnitCache2Other_DeleteUnit // IActorResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;
}
```

SceneType.cs

```c#
//数据缓存服务器
UnitCache = 9,
```

Server.Model -> Demo -> UnitCache -> UnitCache.cs

```C#
using System.Collections.Generic;

namespace ET
{
    public interface IUnitCache
    {

    }

    public class UnitCache : Entity, IAwake, IDestroy
    {
        public string key { get; set; }

        public Dictionary<long, Entity> CacheComponentDic { get; set; } = new Dictionary<long, Entity>();
    }
}
```

Server.Model -> Demo -> UnitCache -> UnitCacheComponent.cs

```C#
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType]
    public class UnitCacheComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<string, UnitCache> UnitCacheDic = new Dictionary<string, UnitCache>();

        public List<string> UnitCacheKeyList = new List<string>();
    }
}
```

Server.Hotfix\Demo\Scene\SceneFactory.cs

```C#
case SceneType.LoginCenter://账号中心服务器
    scene.AddComponent<LoginInfoRecordComponent>();
    break;
case SceneType.UnitCache://新增缓存数据服务器
    scene.AddComponent<UnitCacheComponent>();
    break;
```

Server.Model\Generate\ConfigPartial\StartSceneConfig.cs

```C#
public List<StartSceneConfig> Robots = new List<StartSceneConfig>();
//这个是新添加的
public Dictionary<int, StartSceneConfig> UnitCaches = new Dictionary<int, StartSceneConfig>();
```

```C#
//写在StartSceneConfigCategory里面
public StartSceneConfig GetUnitCacheConfig(long unitId)
{
    int zone = UnitIdStruct.GetUnitZone(unitId);
    return UnitCaches[zone];
}
```

```C#
case SceneType.LoginCenter:
    this.loginCenterConfig = startSceneConfig;
    break;
case SceneType.UnitCache://这个是新添加的
    UnitCaches.Add(startSceneConfig.Zone, startSceneConfig);
    break;
```

Server.Hotfix\Demo\UnitCache\UnitCacheHelper.cs

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public static class UnitCacheHelper
    {
        /// <summary> 保存或者更新玩家缓存 </summary>
        public static async ETTask AddUpdateUnitCache<T>(this T self) where T : Entity, IUnitCache
        {
            Other2UnitCache_AddOrUpdateUnit message = new Other2UnitCache_AddOrUpdateUnit() { UnitId = self.Id };
            message.EntityTypes.Add(typeof(T).FullName);
            message.EntityBytes.Add(MongoHelper.ToBson(self));
            await MessageHelper.CallActor(StartSceneConfigCategory.Instance.GetUnitCacheConfig(self.Id).InstanceId, message);
        }

        /// <summary> 获取玩家缓存 </summary>
        public static async ETTask<Unit> GetUnitCche(Scene scene, long unitId)
        {
            //long instanceId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).InstanceId;
            //Other2UnitCache_GetUnit message = new Other2UnitCache_GetUnit() { UnitId = unitId };
            //UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit)await MessageHelper.CallActor(instanceId, message);
            //if (queryUnit.Error != ErrorCode.ERR_Success || queryUnit.EntityList.Count <= 0)
            //    return null;
        }

        /// <summary> 获取玩家组件缓存 </summary>
        public static async ETTask<T> GetUnitComponentCache<T>(long unitId) where T : Entity, IUnitCache
        {
            Other2UnitCache_GetUnit message = new Other2UnitCache_GetUnit() { UnitId = unitId };
            message.ComponentNameList.Add(typeof(T).Name);
            long instanceId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).InstanceId;//获取区服缓存服的具体的地址
            UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit)await MessageHelper.CallActor(instanceId, message);
            if (queryUnit.Error == ErrorCode.ERR_Success && queryUnit.EntityList.Count > 0)
                return queryUnit.EntityList[0] as T;
            return null;
        }

        /// <summary> 删除玩家缓存 </summary>
        public static async ETTask DeleteUnitCache(long unitId)
        {
            Other2UnitCache_DeleteUnit message = new Other2UnitCache_DeleteUnit() { UnitId = unitId };
            long instanceId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).InstanceId;
            UnitCache2Other_DeleteUnit queryUnit = (UnitCache2Other_DeleteUnit)await MessageHelper.CallActor(instanceId, message);
        }
    }
}
```

