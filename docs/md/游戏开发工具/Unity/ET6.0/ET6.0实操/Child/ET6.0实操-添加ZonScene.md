# ET6.0实操-添加ZonScene

1. 修改SceneType.cs

    ![1](\../Image/ET6.0实操-添加ZonScene/1.png)

2. 修改SceneFactory.cs

    ![2](\../Image/ET6.0实操-添加ZonScene/2.png)

    ```C#
    case SceneType.Account://添加账号登录服务器 示例
        scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.OuterIPPSessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
        Log.Error("Account Scene 创建!!");//这里是取巧打印日志
        break;
    ```

3. 修改Excel

    ![3](\../Image/ET6.0实操-添加ZonScene/3.png)

4. 启动

    运行Server.App
    ![4](\../Image/ET6.0实操-添加ZonScene/4.png)
