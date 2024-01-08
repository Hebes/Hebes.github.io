# ET6.0放置游戏-03-游戏数据缓存服

## 准备工作

Server.Hotfix -> Demo -> UnitCache -> UnitCacheSystem.cs

```C#
namespace ET
{
    public class UnitCacheDestroySystem : DestroySystem<UnitCache>
    {
        public override void Destroy(UnitCache self)
        {
            foreach (Entity entity in self.CacheComponentDic.Values)
                entity.Dispose();
            self.CacheComponentDic.Clear();
            self.key = null;
        }
    }

    [FriendClass(typeof(UnitCache))]
    public static class UnitCacheSystem
    {
        /// <summary> 添加或者更新 </summary>
        public static void AddOrUpdate(this UnitCache self, Entity entity)
        {
            if (entity == null || entity.IsDisposed) return;

            if (self.CacheComponentDic.TryGetValue(entity.Id, out Entity oldEntity))
            {
                if (entity != oldEntity)
                    oldEntity.Dispose();
                self.CacheComponentDic.Remove(entity.Id);
            }
            self.CacheComponentDic.Add(entity.Id, entity);
        }

        /// <summary> 获取 </summary>
        public static async ETTask<Entity> Get(this UnitCache self, long unitId)
        {
            Entity entity = null;
            if (!self.CacheComponentDic.TryGetValue(unitId, out entity))
            {
                entity = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<Entity>(unitId, self.key);
                if (entity != null)
                    self.AddOrUpdate(entity);
            }
            return entity;
        }

        /// <summary> 删除 </summary>
        public static void Delete(this UnitCache self, long id)
        {
            if (self.CacheComponentDic.TryGetValue(id, out Entity entity))
            {
                entity.Dispose();
                self.CacheComponentDic.Remove(id);
            }
        }
    }
}
```

Server.Hotfix -> Demo -> UnitCache -> UnitCacheComponentSystem.cs

```C#
namespace ET
{
    public class UnitCacheComponentAwakeSystem : AwakeSystem<UnitCacheComponent>
    {
        public override void Awake(UnitCacheComponent self)
        {
            self.UnitCacheKeyList.Clear();
            foreach (System.Type type in Game.EventSystem.GetTypes().Values)
            {
                if (type != typeof(IUnitCache) && typeof(IUnitCache).IsAssignableFrom(type))
                    self.UnitCacheKeyList.Add(type.Name);
            }

            foreach (var key in self.UnitCacheKeyList)
            {
                UnitCache unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCacheDic.Add(key, unitCache);
            }
        }
    }

    public class UnitCacheComponentDestroySystem : DestroySystem<UnitCacheComponent>
    {
        public override void Destroy(UnitCacheComponent self)
        {
            foreach (var unitCache in self.UnitCacheDic.Values)
                unitCache?.Dispose();
            self.UnitCacheDic.Clear();
        }
    }

    [FriendClass(typeof(UnitCacheComponent))]
    public static class UnitCacheComponentSystem
    {
        /// <summary> 增加或者更新 </summary>
        public static async ETTask AddOrUpdate(this UnitCacheComponent self, long id, ListComponent<Entity> entityList)
        {
            using (ListComponent<Entity> list = ListComponent<Entity>.Create())
            {
                foreach (Entity entiry in entityList)
                {
                    string key = entiry.GetType().Name;
                    if (!self.UnitCacheDic.TryGetValue(key, out UnitCache unitCache))
                    {
                        unitCache = self.AddChild<UnitCache>();
                        unitCache.key = key;
                        self.UnitCacheDic.Add(key, unitCache);
                    }
                    unitCache.AddOrUpdate(entiry);
                    list.Add(entiry);
                }
                if (list.Count > 0)
                {
                    await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(id, list);
                }
            }
        }

        /// <summary> 获取 </summary>
        public static async ETTask<Entity> Get(this UnitCacheComponent self, long unitId, string key)
        {
            if (!self.UnitCacheDic.TryGetValue(key, out UnitCache unitCache))
            {
                unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCacheDic[key] = unitCache;
            }
            return await unitCache.Get(unitId);
        }

        /// <summary> 删除 </summary>
        public static void Delete(this UnitCacheComponent self, long unitId)
        {
            foreach (UnitCache cache in self.UnitCacheDic.Values)
                cache.Delete(unitId);
        }
    }
}
```

回复的消息编写

Server.Hotfix -> Demo -> UnitCache -> Handler -> Other2UnitCache_DeleteUnitHandler.cs

```C#
using System;

namespace ET
{
    public class Other2UnitCache_DeleteUnitHandler : AMActorRpcHandler<Scene, Other2UnitCache_DeleteUnit, UnitCache2Other_DeleteUnit>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene">缓存服的Scene</param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="reply"></param>
        /// <returns></returns>
        protected override async ETTask Run(Scene scene, Other2UnitCache_DeleteUnit request, UnitCache2Other_DeleteUnit response, Action reply)
        {
            UnitCacheComponent unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            unitCacheComponent.Delete(request.UnitId);
            reply();
            await ETTask.CompletedTask;
        }
    }
}
```

Server.Hotfix -> Demo -> UnitCache -> Handler -> Other2UnitCache_GetUnitHandler.cs

```C#
using System;
using System.Collections.Generic;

namespace ET
{
    [FriendClass(typeof(UnitCacheComponent))]
    public class Other2UnitCache_GetUnitHandler : AMActorRpcHandler<Scene, Other2UnitCache_GetUnit, UnitCache2Other_GetUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_GetUnit request, UnitCache2Other_GetUnit response, Action reply)
        {
            UnitCacheComponent unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            Dictionary<string, Entity> dic = MonoPool.Instance.Fetch(typeof(Dictionary<string, Entity>)) as Dictionary<string, Entity>;

            try
            {
                if (request.ComponentNameList.Count == 0)
                {
                    dic.Add(nameof(Unit), null);
                    foreach (string s in unitCacheComponent.UnitCacheKeyList)
                        dic.Add(s, null);
                }
                else
                {
                    foreach (string s in request.ComponentNameList)
                        dic.Add(s, null);
                }

                foreach (var key in dic.Keys)
                {
                    Entity entity = await unitCacheComponent.Get(request.UnitId, key);
                    dic[key] = entity;
                }

                response.ComponentNameList.AddRange(dic.Keys);
                response.EntityList.AddRange(dic.Values);
            }
            finally
            {

                dic.Clear();
                MonoPool.Instance.Recycle(dic);
            }

            reply();
        }
    }
}
```

Server.Hotfix -> Demo -> UnitCache -> Handler -> Other2UnitCache_AddOrUpdateUnitHandler.cs

```C#
using System;

namespace ET
{
    //Scene 内存缓存服的Scene
    public class Other2UnitCache_AddOrUpdateUnitHandler : AMActorRpcHandler<Scene, Other2UnitCache_AddOrUpdateUnit, UnitCache2Other_AddOrUpdateUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_AddOrUpdateUnit request, UnitCache2Other_AddOrUpdateUnit response, Action reply)
        {
            UPdateUnitCacheAsync(scene, request, response).Coroutine();
            reply();
            await ETTask.CompletedTask;
        }

        /// <summary> 更新缓存服 </summary>
        public async ETTask UPdateUnitCacheAsync(Scene scene, Other2UnitCache_AddOrUpdateUnit request, UnitCache2Other_AddOrUpdateUnit response)
        {
            UnitCacheComponent unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            using (ListComponent<Entity> entityList = ListComponent<Entity>.Create())//ListComponent复用列表组件,创建列表
            {
                for (int index = 0; index < request.EntityTypes.Count; index++)//EntityTypes 实体列表的名称
                {
                    Type type = Game.EventSystem.GetType(request.EntityTypes[index]);//获取类型
                    Entity entity = (Entity)MongoHelper.FromBson(type, request.EntityBytes[index]);//反序列化
                    entityList.Add(entity);
                }
                await unitCacheComponent.AddOrUpdate(request.UnitId, entityList);
            }
        }
    }
}
```
