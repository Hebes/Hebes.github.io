# ET6.0实操-UI界面

1. 添加Component->ModelView

    ```C#
    namespace ET
    {
        [ComponentOf(typeof(UI))]
        [ChildType]
        public class UIMateLoginComponent : Entity, IAwake, IUpdate, IDestroy
        {

        }
    }
    ```

2. 添加UIType -> Unity.ModelView

    ```C#
    namespace ET
    {
        public static class UIType
        {
            public const string UIMateLogin = "UIMateLogin";
        }
    }
    ```

3. 编写Event -> Unity.HotfixView

    ```C#
    namespace ET
    {

        [UIEvent(UIType.UIMateLogin)]
        public class UIMateLoginEvent : AUIEvent
        {
            public override async ETTask<UI> OnCreateAsync(UIComponent uiComponent, UILayer uiLayer)
            {
                //加载AB包
                await uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadAsync(UIType.UIMateLogin.StringToAB());
                //加载登录注册界面预设并生成实例
                GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(UIType.UIMateLogin.StringToAB(), UIType.UIMateLogin);
                //设置UI层级，只有UI摄像机可以渲染
                GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, UIEventComponent.Instance.UILayers[(int)uiLayer]);
                UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIMateLogin, gameObject);
                ui.AddComponent<UIMateLoginComponent>();
                return ui;
            }

            public override void OnRemove(UIComponent uiComponent)
            {
                ResourcesComponent.Instance.UnloadBundle(UIType.UIMateLogin.StringToAB());
            }
        }
    }
    ```

4. 编写System -> Unity.HotfixView

    ```C#
    namespace ET
    {
        public class ChaatViewItemComponentAwakeSystem : AwakeSystem<ChaatViewItemComponent>
        {
            public override void Awake(ChaatViewItemComponent self)
            {
                ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

                #region 左边
                self.T_Left = rc.Get<GameObject>("T_Left");
                self.T_LeftChatView_Text = rc.Get<GameObject>("T_LeftChatView_Text");
                self.T_LeftIcon = rc.Get<GameObject>("T_LeftIcon");
                #endregion

            }
        }

        [FriendClass(typeof(ChaatViewItemComponent))]
        public static class ChaatViewItemComponentSystem
        {
        
        }
    }
    ```
