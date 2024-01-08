# ET6.0实操-添加场景

1. 可以自己定义一个类在Model文件夹保存场景名称，例如：

    ```C#
    namespace ET
    {
        /// <summary>
        /// 从ab包里面加载的名称
        /// </summary>
        public static class ABName
        {
            //**************场景名称*****************
            public const string MainHallScene1 = "MainHallScene1";
        }
    }
    ```

2. 在ET的Excel的StartSceneConfig@s填写场景
3. Unity中的场景因该都放在Scenes文件夹下
4. 新的场景请在File->Build Settings中添加
5. 选中场景，查看inspector的AssetLabels，创建的新的并命名：场景的名称+.unity3d
6. 点击Tools->Build Tools 打包
7. 场景使用：
    - 第一次使用场景，在示例DEmo中vsStudio使用全局搜索"EnterMapHelper.EnterMapAsync",看代码
    - 多次场景切换使用，在第一次使用完毕后，可以使用C2M_TransferMap发送消息，在示例DEmo中vsStudio使用全局搜索"C2M_TransferMap",看代码

    ```C#
    /// <summary>
    /// 切换场景
    /// </summary>
    public static void OnChangeScene(this UIMateJumpComponent self, string SceneName)
    {
        C2M_TransferMap c2MTransferMap = new C2M_TransferMap();
        c2MTransferMap.SceneName = SceneName;
        SessionComponent sessionComponent = self.ZoneScene(GetComponent<SessionComponent>();
        sessionComponent.Session.Call(c2MTransferMap).Coroutine();
    }
   ```
