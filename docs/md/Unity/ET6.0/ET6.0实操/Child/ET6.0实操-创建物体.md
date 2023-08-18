# ET6.0实操-创建物体

1. 方法一

    ```C#
    //加载AB包
    await uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadAsync(UITUIMateMain.StringToAB());
    //加载登录注册界面预设并生成实例
    GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(UITUIMateMain.StringToAB(), UIType.UIMateMain);
    //设置UI层级，只有UI摄像机可以渲染
    GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObjeUIEventComponent.Instance.UILayers[(int)uiLayer]);
    UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIMateMain, gameObject);
    ui.AddComponent<UIMateMainComponent>();
    ```

2. 方法二

    ```C#
    GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset("Unit.unity3d", "Unit");
    GameObject prefab = bundleGameObject.Get<GameObject>("PlayerArmature");
    GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponenstance.Unit, true);
    go.transform.position = new Vector3(26f, 20f, 0f);
    ```
