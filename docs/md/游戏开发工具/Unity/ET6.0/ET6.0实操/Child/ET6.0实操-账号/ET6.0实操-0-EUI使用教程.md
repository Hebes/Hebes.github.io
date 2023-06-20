# ET6.0实操-0-EUI使用教程

GitHub:**[账号登录](<https://github.com/Hebes/ETLogin>)**

数据库搭建: 宝塔

配置

![31](\../../Image/ET6.0实操-账号/31.png)

**创建UI界面:**

**[EUI](<https://www.bilibili.com/video/BV12F411e7bP>)**

## 创建UI

复制粘贴DlgLobby界面的预制体,并重命名DlgServer

请放在同一目录下

修改AssetBundle标签为dlgserver.unity3d

生成代码

![28](\../../Image/ET6.0实操-账号/28.png)

打开VS Studio

(PS:如果没有找到如下图所示的脚本,请参考VisualStudio 下的 请参考 VisualStudio-添加文件引用)

![29](\../../Image/ET6.0实操-账号/29.png)

```C#
namespace ET
{
    [FriendClass(typeof(DlgServer))]
    public static  class DlgServerSystem
    {

        public static void RegisterUIEvent  (this DlgServer self)
        {
            //添加的测试监听
            self.View.E_EnterMapButton.AddListener(self.OnEnterMapCilckHandler);
        }

        public static void ShowWindow(this  DlgServer self, Entity contextData =     null)
        {
            
        }

        /// <summary> 点击按钮事件 </summary>
        public static void  OnEnterMapCilckHandler(this DlgServer    self)
        {
            Log.Debug("点击了按钮");
        }
    }
}
```

```C#
namespace ET
{
    //可以在这边测试
    //获取在点击登录后打开界面测试
    public class AfterCreateZoneScene_AddComponent: AEvent<EventType.AfterCreateZoneScene>
    {
        protected override  void Run(EventType.AfterCreateZoneScene args)
        {
            Scene zoneScene = args.ZoneScene;
            zoneScene.AddComponent<UIComponent>();
            zoneScene.AddComponent<UIPathComponent>();
            zoneScene.AddComponent<UIEventComponent>();
            zoneScene.AddComponent<RedDotComponent>();
            zoneScene.AddComponent<ResourcesLoaderComponent>();
        
            zoneScene.GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Login);//WindowID.WindowID_Server
        }
    }
}
```

```C#
using System;
using UnityEngine.UI;

namespace ET
{
    public static class DlgLoginSystem
    {

        public static void RegisterUIEvent(this DlgLogin self)
        {
            self.View.E_LoginButton.AddListenerAsync(() => { return self.OnLoginClickHandler(); });
        }

        public static void ShowWindow(this DlgLogin self, Entity contextData = null)
        {

        }

        public static async ETTask OnLoginClickHandler(this DlgLogin self)
        {
            try
            {
                int errorCode = await LoginHelper.Login(
                               self.DomainScene(),
                               ConstValue.LoginAddress,
                               self.View.E_AccountInputField.GetComponent<InputField>().text,
                               self.View.E_PasswordInputField.GetComponent<InputField>().text);

                if (errorCode != ErrorCode.ERR_Success) 
                {
                    Log.Error(errorCode.ToString());
                    return;
                }
                Log.Debug("登录成功");
                //TODO 显示登录之后的页面逻辑比如登录后的显示页面
                self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Login);
                //显示服务器的界面
                self.DomainScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Server);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public static void HideWindow(this DlgLogin self)
        {

        }

    }
}
```

## 使用UGUI自带的按钮

组件前面添加E,然后右键->SpawnEUICode
