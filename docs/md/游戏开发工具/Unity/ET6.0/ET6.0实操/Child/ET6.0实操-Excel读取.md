# ET6.0实操-Excel读取


```C#
UnitConfig unitConfig = UnitConfigCategory.Instance.Get(1001);
```

## 特殊符号

1. 特殊符号
    ![1]( \../Image/ET6.0实操-Excel读取/1.png)

    - 默认不写:客户端和服务端都会生成
    - #:忽略了对应的列
    - c:仅生成客户端
    - s:仅生成服务端

2. 左上角

    左上角添加C只会在客户端生成

    ![4]( \../Image/ET6.0实操-Excel读取/4.png)

3. 空行

    空行用于给策划,数据行的分类

    ![5]( \../Image/ET6.0实操-Excel读取/5.png)

4. 表单前面加#

    不导出表单

    ![6]( \../Image/ET6.0实操-Excel读取/6.png)

5. 为什么前面空两行

   作用:可以使用vba宏->给策划用

## 生成的对应的文件

仅参考,没有加特殊符号前的

**客户端:**

![2]( \../Image/ET6.0实操-Excel读取/2.png)

**服务端:**

![3]( \../Image/ET6.0实操-Excel读取/3.png)

## Excel拓展

### 基础拓展

![9]( \../Image/ET6.0实操-Excel读取/9.png)

查找UnitConfigCategory类,可以发现有个partial,用于拓展

UnitConfigCategory类在 UnitConfig.cs的第10行,拉到顶就是

![7](\../Image/ET6.0实操-Excel读取/7.png)

拓展通过身高获取

![11](\../Image/ET6.0实操-Excel读取/11.png)

```C#
namespace ET
{
    //UnitConfigCategory这个类名要和UnitConfig的UnitConfigCategory一样
    public partial class UnitConfigCategory
    {
        public UnitConfig GetUnitConfigByHeight(int height)
        {
            UnitConfig unitConfig = null;

            foreach (var info in this.dict.Values)
            {
                if (info.Height == height)
                {
                    unitConfig = info;
                    break;
                }
            }

            return unitConfig;
        }
    }
}
```

使用代码

![8]( \../Image/ET6.0实操-Excel读取/8.png)

```C#
namespace ET
{
    public class AppStartInitFinish_CreateLoginUI : AEvent<EventType.AppStartInitFinish>
    {
        protected override void Run(EventType.AppStartInitFinish args)
        {
            UIHelper.Create(args.ZoneScene, UIType.UILogin, UILayer.Mid).Coroutine();

            test(args.ZoneScene).Coroutine();
        }

        public async ETTask test(Scene ZoneScene)
        {
            var info =  UnitConfigCategory.Instance.Get(1001);
            Log.Debug(info.Name);
            //***********自己拓展的excel解析
            var info1 = UnitConfigCategory.Instance.GetUnitConfigByHeight(174);
            Log.Debug(info1.Name);
        }
    }
}
```

![10]( \../Image/ET6.0实操-Excel读取/10.png)

### 拓展类型复杂

```C#
namespace ET
{
    //拓展类型 UnitConfig这个和这个类名要和UnitConfig的一样
    public partial class UnitConfig
    {
        public UnityEngine.Vector3 TestValue;
    }

    //UnitConfigCategory这个类名要和UnitConfig的UnitConfigCategory一样
    public partial class UnitConfigCategory
    {
        //复杂类型拓展
        public override void AfterEndInit()
        {
            base.AfterEndInit();

            foreach (var config in this.dict.Values)
                config.TestValue = new UnityEngine.Vector3(config.Position, config.Height, config.Weight);
        }

        public UnitConfig GetUnitConfigByHeight(int height)
        {
            UnitConfig unitConfig = null;

            foreach (var info in this.dict.Values)
            {
                if (info.Height == height)
                {
                    unitConfig = info;
                    break;
                }
            }

            return unitConfig;
        }
    }
}
```

使用代码

```C#
namespace ET
{
    public class AppStartInitFinish_CreateLoginUI : AEvent<EventType.AppStartInitFinish>
    {
        protected override void Run(EventType.AppStartInitFinish args)
        {
            UIHelper.Create(args.ZoneScene, UIType.UILogin, UILayer.Mid).Coroutine();

            test(args.ZoneScene).Coroutine();
        }

        public async ETTask test(Scene ZoneScene)
        {
            var info =  UnitConfigCategory.Instance.Get(1001);
            Log.Debug(info.Name);
            //***********自己拓展的excel解析
            var info1 = UnitConfigCategory.Instance.GetUnitConfigByHeight(174);
            Log.Debug(info1.Name);
            //***********自己拓展的v3类型解析
            var info2 = UnitConfigCategory.Instance.GetUnitConfigByHeight(174);
            Log.Debug(info2.Name+"v3是:"+info2.TestValue);
        }
    }
}
```

![12]( \../Image/ET6.0实操-Excel读取/12.png)

### 拓展List

```C#
namespace ET
{
    //拓展类型 UnitConfig这个和这个类名要和UnitConfig的一样
    public partial class UnitConfig
    {
        public UnityEngine.Vector3 TestValue;
    }

    public partial class TestValue3
    {
        public UnityEngine.Vector3 TestValue;
    }


    //UnitConfigCategory这个类名要和UnitConfig的UnitConfigCategory一样
    public partial class UnitConfigCategory
    {
        //这里拓展列表
        public List<TestValue3> testValue3List=new List<TestValue3>();

        //复杂类型拓展
        public override void AfterEndInit()
        {
            base.AfterEndInit();

            foreach (var config in this.dict.Values)
            {
                config.TestValue = new UnityEngine.Vector3(config.Position, config.Height, config.Weight);
                this.testValue3List.Add(new TestValue3() { TestValue= config.TestValue });
            }
        }

        public UnitConfig GetUnitConfigByHeight(int height)
        {
            UnitConfig unitConfig = null;

            foreach (var info in this.dict.Values)
            {
                if (info.Height == height)
                {
                    unitConfig = info;
                    break;
                }
            }

            return unitConfig;
        }
    }
}
```

```C#


namespace ET
{
    public class AppStartInitFinish_CreateLoginUI : AEvent<EventType.AppStartInitFinish>
    {
        protected override void Run(EventType.AppStartInitFinish args)
        {
            UIHelper.Create(args.ZoneScene, UIType.UILogin, UILayer.Mid).Coroutine();

            test(args.ZoneScene).Coroutine();
        }

        public async ETTask test(Scene ZoneScene)
        {
            var info =  UnitConfigCategory.Instance.Get(1001);
            Log.Debug(info.Name);
            //***********自己拓展的excel解析
            var info1 = UnitConfigCategory.Instance.GetUnitConfigByHeight(174);
            Log.Debug(info1.Name);
            //***********自己拓展的v3类型解析
            var info2 = UnitConfigCategory.Instance.GetUnitConfigByHeight(174);
            Log.Debug(info2.Name+"v3是:"+info2.TestValue);
            //***********list
            var info3 = UnitConfigCategory.Instance.testValue3List;
            foreach (var item in info3)
            {
                Log.Debug("v3是:" + item.TestValue);
            }
        }
    }
}
```

![13]( \../Image/ET6.0实操-Excel读取/13.png)

## 其他参考

**[解析ET6框架的配置表的原理和使用](<https://blog.csdn.net/m0_46712616/article/details/121773072>)**
**[ET框架-09 Excel配置工具](<https://blog.csdn.net/m0_48781656/article/details/123483915>)**
