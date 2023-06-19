# ET6.0基础部分-NumericComponent

## 说明

数值组件

Excel配表最好使用万分比,可以减少精度损失

**Excel填写float要用万分比**,打个比方配置表最大HP100,如果调用GetAsFloat(HP100),那么获取最后就是得到0.001HP了

再比如要得到100HP,可以通过GetAsInt(),或者配置表配置100*10000=1000000然后使用GetAsFloat()方法

**[计算机二进制和浮点数](<https://et-framework.cn/d/95>)**

## 代码

```C#
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    namespace EventType
    {
        public class NumbericChange: DisposeObject
        {
            public static readonly NumbericChange Instance = new    NumbericChange();
            
            public Entity Parent;
            public int NumericType;
            public long Old;
            public long New;
        }
    }

    [FriendClass(typeof(NumericComponent))]
    public static class NumericComponentSystem
    {
        public static float GetAsFloat(this NumericComponent self, int  numericType)
        {
            return (float)self.GetByKey(numericType) / 10000;
        }

        public static int GetAsInt(this NumericComponent self, int  numericType)
        {
            return (int)self.GetByKey(numericType);
        }
        
        public static long GetAsLong(this NumericComponent self, int    numericType)
        {
            return self.GetByKey(numericType);
        }

        public static void Set(this NumericComponent self, int nt, float value)
        {
            self[nt] = (int) (value * 10000);
        }

        public static void Set(this NumericComponent self, int nt, int value)
        {
            self[nt] = value;
        }
        
        public static void Set(this NumericComponent self, int nt, long value)
        {
            self[nt] = value;
        }

        /// <summary>
        /// 更新存储数值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="numericType">数值类型</param>
        /// <param name="value">值</param>
        public static void SetNoEvent(this NumericComponent self, int numericType, long value)
        {
            self.Insert(numericType,value,false);
        }
        
        public static void Insert(this NumericComponent self, int numericType, long value,bool isPublicEvent = true)
        {
            long oldValue = self.GetByKey(numericType);
            if (oldValue == value) return;

            self.NumericDic[numericType] = value;

            if (numericType >= NumericType.Max)
            {
                //更新存储数值
                self.Update(numericType,isPublicEvent);
                return;
            }

            if (isPublicEvent)
            {
                EventType.NumbericChange args = EventType.NumbericChange.Instance;
                args.Parent = self.Parent;
                args.NumericType = numericType;
                args.Old = oldValue;
                args.New = value;
                Game.EventSystem.PublishClass(args);
            }
        }
        
        public static long GetByKey(this NumericComponent self, int key)
        {
            long value = 0;
            self.NumericDic.TryGetValue(key, out value);
            return value;
        }

        public static void Update(this NumericComponent self, int numericType,bool isPublicEvent)
        {
            int final = (int) numericType / 10;
            int bas = final * 10 + 1; 
            int add = final * 10 + 2;
            int pct = final * 10 + 3;
            int finalAdd = final * 10 + 4;
            int finalPct = final * 10 + 5;

            // 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值  100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
            // final = (((base + add) * (100 + pct) / 100) + finalAdd) *    (100 + finalPct) / 100;
            long result = (long)(((self.GetByKey(bas) + self.GetByKey   (add)) * (100 + self.GetAsFloat(pct)) / 100f + self.GetByKey   (finalAdd)) * (100 + self.GetAsFloat(finalPct)) / 100f);
            self.Insert(final,result,isPublicEvent);
        }
    }
        

    [ComponentOf(typeof(Unit))]
    public class NumericComponent: Entity, IAwake, ITransfer
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> NumericDic = new Dictionary<int, long> ();

        public long this[int numericType]
        {
            get
            {
                return this.GetByKey(numericType);
            }
            set
            {
                this.Insert(numericType,value);
            }
        }
    }
}
```

## 使用

先编写NumericType.cs文件

```C#
namespace ET
{
    // 这个可弄个配置表生成
    public static class NumericType
    {
        public const int Max = 10000;

        public const int Speed = 1000;
        public const int SpeedBase = Speed * 10 + 1;
        public const int SpeedAdd = Speed * 10 + 2;
        public const int SpeedPct = Speed * 10 + 3;
        public const int SpeedFinalAdd = Speed * 10 + 4;
        public const int SpeedFinalPct = Speed * 10 + 5;

        public const int MaxHp = 1002;
        public const int MaxHpBase = MaxHp * 10 + 1;
        public const int MaxHpAdd = MaxHp * 10 + 2;
        public const int MaxHpPct = MaxHp * 10 + 3;
        public const int MaxHpFinalAdd = MaxHp * 10 + 4;
        public const int MaxHpFinalPct = MaxHp * 10 + 5;

        public const int AOI = 1003;
        public const int AOIBase = AOI * 10 + 1;
        public const int AOIAdd = AOI * 10 + 2;
        public const int AOIPct = AOI * 10 + 3;
        public const int AOIFinalAdd = AOI * 10 + 4;
        public const int AOIFinalPct = AOI * 10 + 5;

        public const int Position = 3018;
        public const int Height = 3019;
        public const int Weight = 3020;
    }
}
```

F:\Yet\Project\ET-EUI\Server\Hotfix\Demo\Unit\UnitFactory.cs

这个是初始化

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
                        //从配置表读取数据并设置进NumericComponent组件的字典里面
                        UnitConfig unitConfig = UnitConfigCategory.Instance.Get(1001);
                        numericComponent.SetNoEvent(NumericType.Position, unitConfig.Position);
                        numericComponent.SetNoEvent(NumericType.Height, unitConfig.Height);
                        numericComponent.SetNoEvent(NumericType.Weight, unitConfig.Weight);
                        numericComponent.SetNoEvent(NumericType.IsAlive, 1);
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

设置属性,可以参考源码查找

```C#
NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
int newGold = numericComponent.GetAsInt(NumericType.Gold) + 100;
long newExp = numericComponent.GetAsLong(NumericType.Exp) + 50;
long level = numericComponent.GetAsLong(NumericType.Level) + 1;
numericComponent.Set(NumericType.Gold, newGold);
numericComponent.Set(NumericType.Exp, newExp);
numericComponent.Set(NumericType.Level, level);
```
