# ET6.0放置游戏-06-角色数据展示

## 新增

```C#
// 发送测试unit数值的消息
//ResponseType M2C_TestUnitNumeric
message C2M_TestUnitNumeric // IActorLocationRequest
{
    int32 RpcId = 1;
}

message M2C_TestUnitNumeric // IActorLocationResponse
{
    int32 RpcId = 90;
    int32 Error = 91;
    string Message = 92;
}
```

Unity.Hotfix\Demo\Numeric\NumericHelper.cs

```C#
using System;

namespace ET
{
    public static class NumericHelper
    {
        public static async ETTask<int> TestUpdateNumeric(Scene zoneScene)
        {
            M2C_TestUnitNumeric m2CTestUnitNumeric = null;
            try
            {
                m2CTestUnitNumeric = (M2C_TestUnitNumeric)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2M_TestUnitNumeric() { });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (m2CTestUnitNumeric.Error != ErrorCode.ERR_Success)
            {
                Log.Error(m2CTestUnitNumeric.Error.ToString());
                return m2CTestUnitNumeric.Error;
            }
            return ErrorCode.ERR_Success;
        }
    }
}
```

F:\Yet\Project\ET-EUI\Server\Hotfix\Demo\Numeric\Handler\M2C_TestUnitNumericHandler.cs

```C#
using System;

namespace ET
{
    public class C2M_TestUnitNumericHandler : AMActorLocationRpcHandler<Unit, C2M_TestUnitNumeric, M2C_TestUnitNumeric>
    {
        protected override async ETTask Run(Unit unit, C2M_TestUnitNumeric request, M2C_TestUnitNumeric response, Action reply)
        {
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

            int newGold = numericComponent.GetAsInt(NumericType.Gold) + 100;
            long newExp = numericComponent.GetAsLong(NumericType.Exp) + 50;
            long level = numericComponent.GetAsLong(NumericType.Level) + 1;
            numericComponent.Set(NumericType.Gold, newGold);
            numericComponent.Set(NumericType.Exp, newExp);
            numericComponent.Set(NumericType.Level, level);

            //numericComponent.AddOrUpdateUnitCache().Coroutine();


            reply();
            await ETTask.CompletedTask;
        }
    }
}
```

## 更新

F:\Yet\Project\ET-EUI\Unity\Codes\HotfixView\Demo\UI\DlgRole\DlgRoleSystem.cs

```C#
//self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Main);
self.ZoneScene().GetComponent<UIComponent>().CloseWindow(WindowID.WindowID_Role);
```

F:\Yet\Project\ET-EUI\Unity\Codes\HotfixView\Demo\UI\DlgHelper\Event\SceneChangeFinish_ShowCurrentSceneUI.cs

```C#
protected override async ETTask Run(EventType.SceneChangeFinish args)
{
    args.ZoneScene.GetComponent<UIComponent>().CloseWindow(WindowID.WindowID_Loading);
    args.ZoneScene.GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Main);
    await ETTask.CompletedTask;
}
```

F:\Yet\Project\ET-EUI\Unity\Codes\HotfixView\Demo\Scene\SceneChangeStart_AddComponent.cs

```C#
Scene currentScene = args.ZoneScene.CurrentScene();
args.ZoneScene.GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Loading);
// 加载场景资源
await ResourcesComponent.Instance.LoadBundleAsync($"{currentScene.Name}.unity3d");
// 切换到map场景
```

F:\Yet\Project\ET-EUI\Server\Hotfix\Demo\Account\Handler\C2G_EnterGameHandler.cs

```C#
//玩家Unit上线后初始化操作,传送之前先回复消息保证顺序
await UnitHelper.InitUnit(unit,isNewPlayer);
response.MyId = unit.Id;
reply();
```

F:\Yet\Project\ET-EUI\Unity\Codes\Model\Module\Numeric\NumericType.cs

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


        public const int MaxMp = 1004;
        public const int MaxMpBase = MaxMp * 10 + 1;
        public const int MaxMpAdd = MaxMp * 10 + 2;
        public const int MaxMpPct = MaxMp * 10 + 3;
        public const int MaxMpFinalAdd = MaxMp * 10 + 4;
        public const int MaxMpFinalPct = MaxMp * 10 + 5;


        public const int DamageValue = 1011;         //伤害
        public const int DamageValueBase = DamageValue * 10 + 1;
        public const int DamageValueAdd = DamageValue * 10 + 2;
        public const int DamageValuePct = DamageValue * 10 + 3;
        public const int DamageValueFinalAdd = DamageValue * 10 + 4;
        public const int DamageValueFinalPct = DamageValue * 10 + 5;

        public const int AdditionalDdamage = 1012;         //伤害追加


        public const int Hp = 1013;  // 生命值
        public const int HpBase = Hp * 10 + 1;
        public const int HpAdd = Hp * 10 + 2;
        public const int HpPct = Hp * 10 + 3;
        public const int HpFinalAdd = Hp * 10 + 4;
        public const int HpFinalPct = Hp * 10 + 5;



        public const int MP = 1014; //法力值
        public const int MPBase = MP * 10 + 1;
        public const int MPAdd = MP * 10 + 2;
        public const int MPPct = MP * 10 + 3;
        public const int MPFinalAdd = MP * 10 + 4;
        public const int MPFinalPct = MP * 10 + 5;


        public const int Armor = 1015; //护甲
        public const int ArmorBase = Armor * 10 + 1;
        public const int ArmorAdd = Armor * 10 + 2;
        public const int ArmorPct = Armor * 10 + 3;
        public const int ArmorFinalAdd = Armor * 10 + 4;
        public const int ArmorFinalPct = Armor * 10 + 5;

        public const int ArmorAddition = 1015; //护甲追加

        public const int Dodge = 1017;           //闪避
        public const int DodgeBase = Dodge * 10 + 1;
        public const int DodgeAdd = Dodge * 10 + 2;
        public const int DodgePct = Dodge * 10 + 3;
        public const int DodgeFinalAdd = Dodge * 10 + 4;
        public const int DodgeFinalPct = Dodge * 10 + 5;

        public const int DodgeAddition = 1018;   // 闪避追加

        public const int CriticalHitRate = 1019; //暴击率
        public const int CriticalHitRateBase = CriticalHitRate * 10 + 1;
        public const int CriticalHitRateAdd = CriticalHitRate * 10 + 2;
        public const int CriticalHitRatePct = CriticalHitRate * 10 + 3;
        public const int CriticalHitRateFinalAdd = CriticalHitRate * 10 + 4;
        public const int CriticalHitRateFinalPct = CriticalHitRate * 10 + 5;

        public const int Power = 3001; //力量

        public const int PhysicalStrength = 3002; //体力

        public const int Agile = 3003; //敏捷值

        public const int Spirit = 3004; //精神

        public const int AttributePoint = 3005; //属性点

        public const int CombatEffectiveness = 3006; //战力值

        public const int Level = 3007;

        public const int Gold = 3008;

        public const int Exp = 3009;

        public const int AdventureState = 3010;   //关卡冒险状态

        public const int DyingState = 3011;      //垂死状态

        public const int AdventureStartTime = 3012;   //关卡开始冒险的时间

        public const int IsAlive = 3013;    //存活状态  0为死亡 1为活着


        public const int BattleRandomSeed = 3014;    //战斗随机数种子

        public const int MaxBagCapacity = 3015;   //背包最大负重


        public const int IronStone = 3016; //铁矿石

        public const int Fur = 3017; //皮毛

        public const int Position = 3018;
        public const int Height = 3019;
        public const int Weight = 3020;
    }
}
```

## 使用

F:\Yet\Project\ET-EUI\Unity\Codes\HotfixView\Demo\UI\DlgMain\DlgMainSystem.cs

```C#
using System;

namespace ET
{
    [FriendClass(typeof(DlgMain))]
    public static class DlgMainSystem
    {

        public static void RegisterUIEvent(this DlgMain self)
        {
            self.View.E_RoleButton.AddListenAsync(self.OnRoleButtonClickHandler);
        }

        public static void ShowWindow(this DlgMain self, Entity contextData = null)
        {
            self.Refresh().Coroutine();
        }

        /// <summary> 刷新 </summary>
        public static async ETTask Refresh(this DlgMain self)
        {
            Unit unit = UnitHelper.GetMyUnitFromCurrentScene(self.ZoneScene().CurrentScene());
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

            self.View.E_RoleLevelText.SetText($"Lv.{numericComponent.GetAsInt((int)NumericType.Level)}");
            self.View.E_GoldText.SetText(numericComponent.GetAsInt((int)NumericType.Gold).ToString());
            self.View.E_ExpText.SetText(numericComponent.GetAsInt((int)NumericType.Exp).ToString());

            await ETTask.CompletedTask;
        }

        public static async ETTask OnRoleButtonClickHandler(this DlgMain self)
        {
            try
            {
                int error = await NumericHelper.TestUpdateNumeric(self.ZoneScene());
                if (error != ErrorCode.ERR_Success)
                    return;
                Log.Error("发送更新属性测试消息成功");
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }
}
```
