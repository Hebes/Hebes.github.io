# ET6.0实操-创建游戏实体

![1](\../Image/ET6.0实操-创建游戏实体/1.png)

![5](\../Image/ET6.0实操-创建游戏实体/5.png)

```C#
namespace ET
{
    public enum UnitType: byte
    {
        Player = 1,
        Monster = 2,
        NPC = 3,
        //新添加的代码
        DropIten=4,//掉落物
        Box,//宝箱
    }
}
```

![2](\../Image/ET6.0实操-创建游戏实体/2.png)

```C#
public UnitType UnitType { get; set; } 
```

![3](\../Image/ET6.0实操-创建游戏实体/3.png)

```C#
using UnityEngine;

namespace ET
{
    public static class UnitFactory
    {
        public static Unit Create(Scene currentScene, UnitInfo unitInfo)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(unitInfo.UnitId, unitInfo.ConfigId);
            unitComponent.Add(unit);

            unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
            unit.Forward = new Vector3(unitInfo.ForwardX, unitInfo.ForwardY, unitInfo.ForwardZ);

            NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
            for (int i = 0; i < unitInfo.Ks.Count; ++i)
                numericComponent.Set(unitInfo.Ks[i], unitInfo.Vs[i]);

            unit.AddComponent<MoveComponent>();
            if (unitInfo.MoveInfo != null)
            {
                if (unitInfo.MoveInfo.X.Count > 0)
                {
                    using (ListComponent<Vector3> list = ListComponent<Vector3>.Create())
                    {
                        list.Add(unit.Position);
                        for (int i = 0; i < unitInfo.MoveInfo.X.Count; ++i)
                            list.Add(new Vector3(unitInfo.MoveInfo.X[i], unitInfo.MoveInfo.Y[i], unitInfo.MoveInfo.Z[i]));
                        unit.MoveToAsync(list).Coroutine();
                    }
                }
            }   
            unit.AddComponent<ObjectWait>();    
            unit.AddComponent<XunLuoPathComponent>();

            Game.EventSystem.Publish(new EventType.AfterUnitCreate() {Unit = unit});
               return unit;
        }
        
        //**************************************************************************
        //下面是自己添加的代码
        /// <summary> 创建玩家 </summary>
        public static Unit CreatePlayer(Scene currentScene, UnitInfo unitInfo)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(unitInfo.UnitId, unitInfo.ConfigId);
            unitComponent.Add(unit);

            unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
            unit.Forward = new Vector3(unitInfo.ForwardX, unitInfo.ForwardY, unitInfo.ForwardZ);

            unit.UnitType=UnitType.Player;//类型是玩家
            //todo 这里可以添加组件
            //如果玩家想走的话
            unit.AddComponent<MoveComponent>();

            unit.AddComponent<ObjectWait>();
            //这里因为不能调用unityEngine相关接口的API所以使用Publish抛出事件
            Game.EventSystem.Publish(new EventType.AfterUnitCreate() { Unit = unit });
            return unit;
        }
        /// <summary> 创建NPC </summary>
        public static Unit CreateNPC(Scene currentScene, UnitInfo unitInfo)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(unitInfo.UnitId, unitInfo.ConfigId);
            unitComponent.Add(unit);

            unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
            unit.Forward = new Vector3(unitInfo.ForwardX, unitInfo.ForwardY, unitInfo.ForwardZ);

            unit.UnitType = UnitType.NPC;//类型是NPC

            unit.AddComponent<ObjectWait>();

            Game.EventSystem.Publish(new EventType.AfterUnitCreate() { Unit = unit });
            return unit;
        }
        /// <summary> 创建怪物 </summary>
        public static Unit CreateMonster(Scene currentScene, UnitInfo unitInfo)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(unitInfo.UnitId, unitInfo.ConfigId);
            unitComponent.Add(unit);

            unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
            unit.Forward = new Vector3(unitInfo.ForwardX, unitInfo.ForwardY, unitInfo.ForwardZ);

            unit.UnitType = UnitType.Monster;//类型是NPC

            unit.AddComponent<ObjectWait>();
            //如果怪物想走和巡逻的话
            unit.AddComponent<MoveComponent>();
            unit.AddComponent<XunLuoPathComponent>();

            Game.EventSystem.Publish(new EventType.AfterUnitCreate() { Unit = unit });
            return unit;
        }
    }
}
```

![4](\../Image/ET6.0实操-创建游戏实体/4.png)

```C#
using UnityEngine;

namespace ET
{
    public class AfterUnitCreate_CreateUnitView: AEvent<EventType.AfterUnitCreate>
    {
        protected override void Run(EventType.AfterUnitCreate args)
        {
            //创建不同的物体
            switch (args.Unit.UnitType)
            {
                case UnitType.Player:
                    // Unit View层
                    // 这里可以改成异步加载，demo就不搞了
                    GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset("Unit.unity3d", "Unit");
                    GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");

                    GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
                    go.transform.position = args.Unit.Position;
                    args.Unit.AddComponent<GameObjectComponent>().GameObject = go;
                    args.Unit.AddComponent<AnimatorComponent>();
                    break;
                case UnitType.Monster:
                    break;
                case UnitType.NPC:
                    // Unit View层
                    // 这里可以改成异步加载，demo就不搞了
                    GameObject bundleGameObject1 = (GameObject)ResourcesComponent.Instance.GetAsset("Unit.unity3d", "Unit");
                    GameObject prefab1 = bundleGameObject1.Get<GameObject>("Skeleton");

                    GameObject go1 = UnityEngine.Object.Instantiate(prefab1, GlobalComponent.Instance.Unit, true);
                    go1.transform.position = args.Unit.Position;
                    args.Unit.AddComponent<GameObjectComponent>().GameObject = go1;
                    args.Unit.AddComponent<AnimatorComponent>();
                    break;
                case UnitType.DropIten:
                    break;
                case UnitType.Box:
                    break;
                default:
                    break;
            }
        }
    }
}
```
