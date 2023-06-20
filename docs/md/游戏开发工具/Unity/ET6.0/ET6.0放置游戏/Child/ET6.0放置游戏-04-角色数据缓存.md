# ET6.0放置游戏-04-角色数据缓存

![1](\../Image/ET6.0放置游戏-04-角色数据缓存/1.png)

![2](\../Image/ET6.0放置游戏-04-角色数据缓存/2.png)

Game.scene 添加game.unity3d标签,ab包的标签

## 新增

Unity.HotfixView -> Codes -> HotfixView -> Demo -> UI -> DlgMain -> DlgMainSystem.cs

```C#
namespace ET
{
     [FriendClass(typeof(DlgMain))]
    public static  class DlgMainSystem
    {

        public static void RegisterUIEvent(this DlgMain self)
        {
         
        }

        public static void ShowWindow(this DlgMain self, Entity contextData = null)
        {
            self.Refresh().Coroutine();
        }
        /// <summary> 刷新 </summary>
        public static async ETTask Refresh(this DlgMain self)
        {
            Unit unit = UnitHelper.GetMyUnitFromCurrentScene(self.ZoneScene().CurrentScene());
            NumericComponent numericComponent=unit.GetComponent<NumericComponent>();
            //这里肯定报错,看下个文章
            self.View.E_RoleLevelText.SetText($"Lv.{numericComponent.GetAsInt((int)NumericType.Level)}");
            self.View.E_GoldText.SetText(numericComponent.GetAsInt((int)NumericType.Gold).ToString());
            self.View.E_ExpText.SetText(numericComponent.GetAsInt((int)NumericType.Exp).ToString());

            await ETTask.CompletedTask;
        }
    }
}
```

## 更新

Server.Hotfix -> Demo -> UnitCache -> UnitCacheHelper.cs

```C#
/// <summary> 保存Unit及Unit身上组件到缓存服及数据库中 </summary>
public static void AddOrUpdateUnitAllCache(Unit unit)
{
    Other2UnitCache_AddOrUpdateUnit message = new Other2UnitCache_AddOrUpdateUnit() { UnitId = unit.Id };

    message.EntityTypes.Add(unit.GetType().FullName);
    message.EntityBytes.Add(MongoHelper.ToBson(unit));

    foreach ((Type key, Entity entity) in unit.Components)//循环所有的组件
    {
        if (!typeof(IUnitCache).IsAssignableFrom(key))
            continue;
        message.EntityTypes.Add(key.FullName);
        message.EntityBytes.Add(MongoHelper.ToBson(entity));
    }

    MessageHelper.CallActor(StartSceneConfigCategory.Instance.GetUnitCacheConfig(unit.Id).InstanceId, message).Coroutine();
}
/// <summary> 获取玩家缓存 </summary>
public static async ETTask<Unit> GatUnitCache(Scene scene, long unitId)
{
    //获取缓存服地址
    long instanceId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).InstanceId;
    //查村缓存服的消息
    Other2UnitCache_GetUnit messahe = new Other2UnitCache_GetUnit() { UnitId = unitId };
    UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit)await MessageHelper.CallActor(instanceId, messahe);
    if (queryUnit.Error != ErrorCode.ERR_Success || queryUnit.EntityList.Count <= 0)
        return null;

    int indexOf = queryUnit.ComponentNameList.IndexOf(nameof(Unit));
    Unit unit = queryUnit.EntityList[indexOf] as Unit;
    if (unit == null) return null;
    scene.AddChild(unit);
    foreach (Entity entity in queryUnit.EntityList)
    {
        if (entity == null || entity is Unit)
            continue;
        unit.AddComponent(entity);
    }
    return unit;
}
```

Server.Hotfix -> Demo -> Account -> Handler -> C2G_EnterGameHandler.cs

```C#
//GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
//gateMapComponent.Scene = await SceneFactory.Create(gateMapComponent, "GateMap",SceneType.Map);

////unit简单理解为抽象的游戏角色实体
//Unit unit = UnitFactory.Create(gateMapComponent.Scene, player.Id, UnitTypePlayer);
//unit.AddComponent<UnitGateComponent, long>(session.InstanceId);

//从数据库或者缓存中加载出Unit实体及其相关组件
(bool isNewPlayer, Unit unit) = await UnitHelper.LoadUnit(player);
unit.AddComponent<UnitGateComponent, long>(player.InstanceId);

//玩家Unit上线后初始化操作
await UnitHelper.InitUnit(unit,isNewPlayer);

long unitId = unit.Id;
```

Server.Hotfix -> Demo -> Unit -> UnitHelper.cs

```C#
public static async ETTask InitUnit(Unit unit, bool isNewPlayer)
{
    await ETTask.CompletedTask;
}

/// <summary> 获取Unit缓存数据 </summary>
public static async ETTask<(bool, Unit)> LoadUnit(Player player)
{
    GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
    gateMapComponent.Scene = await SceneFactory.Create(gateMapComponent, "GateMap", SceneType.Map);

    //从缓存服务中查询
    Unit unit = await UnitCacheHelper.GatUnitCache(gateMapComponent.Scene, player.UnitId);
    bool isNewUnit = unit == null;

    if (isNewUnit)
    {
        //创建新的
        unit = UnitFactory.Create(gateMapComponent.Scene, player.Id, UnitType.Player);// player.Id或者player.UnitId都行
        UnitCacheHelper.AddOrUpdateUnitAllCache(unit);
    }
    return (isNewUnit, unit);
}
```

Server.Hotfix -> Demo -> Unit -> UnitFactory.cs

```C#
using System;

namespace ET
{
    public static class UnitFactory
    {
        public static Unit Create(Scene scene, long id, UnitType unitType)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            switch (unitType)
            {
                case UnitType.Player:
                {
                    Unit unit = unitComponent.AddChildWithId<Unit, int>(id, 1001);
                    //ChildType测试代码 取消注释 编译Server.hotfix 可发现报错
                    //unitComponent.AddChild<Player, string>("Player");
                    //unit.AddComponent<MoveComponent>();
                    //unit.Position = new Vector3(-10, 0, -10);

                    NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
                    //numericComponent.Set(NumericType.Speed, 6f); // 速度是6米每秒
                    //numericComponent.Set(NumericType.AOI, 15000); // 视野15米
                    
                    unitComponent.Add(unit);
                    // 加入aoi
                    //unit.AddComponent<AOIEntity, int, Vector3>(9 * 1000, unit.Position);
                    return unit;
                }
                default:
                    throw new Exception($"not such unit type: {unitType}");
            }
        }
    }
}
```

Unity.Hotfix -> Codes -> Hotfix -> Demo -> Login -> LoginHelper.cs

```C#
if (g2C_EnterGame.Error != ErrorCode.ERR_Success)
{
    Log.Error(g2C_EnterGame.Error.ToString());
    return g2C_EnterGame.Error;
}
//新增部分
Log.Debug("角色进入游戏成功!!");zoneScene.GetComponent<PlayerComponent>().MyId = g2C_EnterGame.MyId;
```

Unity.HotfixView -> Codes -> HotfixView -> Demo -> UI -> DlgRole -> DlgRoleSystem.cs

```C#
 //登录游戏
errorCode = await LoginHelper.EnterGame(self.ZoneScene());
if (errorCode != ErrorCode.ERR_Success)
{
    Log.Error(errorCode.ToString());
    return;
}

self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Main);
self.ZoneScene().GetComponent<UIComponent>().CloseWindow(WindowID.WindowID_Role);
```
