# ET6.0代码示例

**Run的参数UIEventType.LandInitSceneStart是如何关联到我们定义的事件方法的呢？**

``` text {.line-numbers}
using System;
using System.Collections.Generic;

namespace ETModel
{

    public static partial class LandUIType
    {
        public const string LandLogin = "LandLogin";
    }

    public static partial class UIEventType
    {
        public const string LandInitSceneStart = "LandInitSceneStart";
    }
    
    [Event(UIEventType.LandInitSceneStart)]
    public class InitSceneStart_CreateLandLogin : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Create(LandUIType.LandLogin);
        } 
    }
}
```

**添加登录注册界面的工厂类LandLoginFactory**
上面我们使用 Game.Scene.GetComponent\<UIComponent>().Create(LandUIType.LandLogin) 即是调用LandLoginFactory创建了登录注册界面。
这是如何做到的呢？
ETCore帮我们实现了对程序集中有定义了[UIFactory(LandUIType.LandLogin)]特性的类对象的Create方法的调用。

\Assets\Model\Landlords\LandUI\LandLogin\LandLoginFactory.cs

``` text {.line-numbers}
using System;
using UnityEngine;

namespace ETModel
{
    [UIFactory(LandUIType.LandLogin)]
    public class LandLoginFactory:IUIFactory
    {
        public UI Create(Scene scene, string type, GameObject parent)
        {
            try
            {
                //加载AB包
                ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");

                //加载登录注册界面预设并生成实例
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject landLogin = UnityEngine.Object.Instantiate(bundleGameObject);

                //设置UI层级，只有UI摄像机可以渲染
                landLogin.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(landLogin);

                ui.AddComponent<LandLoginComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
    }
}
```

**添加LandLoginComponent组件:**

> 上面在LandLogin工厂中创建了登录界面：
> UI ui = ComponentFactory.Create<UI, GameObject>(landLogin);
> ui.AddComponent\<LandLoginComponent>();

**这就是ETCore的UI组件的用法：**

- 传入UI实例创建UI实体
- UI实体添加UI组件
- 在UI组件中获取UI的绑定元件和添加事件方法等
**借助ReferenceCollector 组件，获取绑定引用**
ReferenceCollector rc = this.GetParent\<UI>().GameObject.GetComponent\<ReferenceCollector>()

例如LandLoginComponent，UI组件的父对象是UI实体，UI实体持有UI实例，再获取实例上的ReferenceCollector脚本组件。

\Assets\Model\Landlords\LandUI\LandLogin\LandLoginComponent.cs

``` text {.line-numbers}
using UnityEngine;
using UnityEngine.UI;
namespace ETModel
{
    [ObjectSystem]
    public class LandLoginComponentAwakeSystem : AwakeSystem<LandLoginComponent>
    {
        public override void Awake(LandLoginComponent self)
        {
            self.Awake();
        }
    }

    public class LandLoginComponent : Component
    {
        //提示文本
        public Text prompt;

        public InputField account;
        public InputField password;

        //是否正在登录中（避免登录请求还没响应时连续点击登录）
        public bool isLogining;
        //是否正在注册中（避免登录请求还没响应时连续点击注册）
        public bool isRegistering;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            //初始化数据
            account = rc.Get<GameObject>("Account").GetComponent<InputField>();
            password = rc.Get<GameObject>("Password").GetComponent<InputField>();
            prompt = rc.Get<GameObject>("Prompt").GetComponent<Text>();
            this.isLogining = false;
            this.isRegistering = false;

            //添加事件
            rc.Get<GameObject>("LoginButton").GetComponent<Button>().onClick.Add(() => LoginBtnOnClick());
            rc.Get<GameObject>("RegisterButton").GetComponent<Button>().onClick.Add(() => RegisterBtnOnClick());
        }

        public void LoginBtnOnClick()
        {
            if (this.isLogining || this.IsDisposed)
            {
                return;
            }
            this.isLogining = true; 
        }

        public void RegisterBtnOnClick()
        {
            if (this.isRegistering || this.IsDisposed)
            {
                return;
            }
            this.isRegistering = true;
        }
    }
}
```

**生命周期**：

``` text {.line-numbers}
[ObjectSystem]
   public class UILoginComponentAwakeSystem : AwakeSystem<UILoginComponent>
   {
      public override void Awake(UILoginComponent self)
      {
         ReferenceCollector rc = self.GetParent<UI>().GameObject  GetComponent<ReferenceCollector>();
         self.loginBtn = rc.Get<GameObject>("LoginBtn");

         self.loginBtn.GetComponent<Button>().onClick.AddListene  (()=> { self.OnLogin(); });
         self.account = rc.Get<GameObject>("Account");
         self.password = rc.Get<GameObject>("Password");
      }
   }

   [FriendClass(typeof(UILoginComponent))]
   public static class UILoginComponentSystem
   {
      public static void OnLogin(this UILoginComponent self)
      {
         LoginHelper.Login(
            self.DomainScene(), 
            ConstValue.LoginAddress, 
            self.account.GetComponent<InputField>().text, 
            self.password.GetComponent<InputField>().text)  Coroutine();
      }
   }
```

**界面离开的事件**：

```text {.line-numbers}
public class LoginFinish_RemoveLoginUI: AEvent<EventType.LoginFinish>
    {
        protected override void Ru  (EventType   LoginFinish args)
        {
            UIHelper.Remove(args.ZoneScene, UIType.UILogin).Coroutine();
        }
    }
```
