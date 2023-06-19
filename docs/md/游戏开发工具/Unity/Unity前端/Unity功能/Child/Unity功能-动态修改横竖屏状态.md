# Unity功能-动态修改横竖屏状态

**[unity 动态修改当前横竖屏状态](<https://blog.csdn.net/yu1368072332/article/details/82901312>)**

```C#
void OnGUI()
{
    if (GUI.Button(new Rect(10, 10, 100, 40), "Horizontal"))
    {
        Screen.orientation = ScreenOrientation.Landscape;
    }

    if (GUI.Button(new Rect(10, 60, 100, 40), "Vertial"))
    {
        Screen.orientation = ScreenOrientation.PortraitUpsideDown;
    }
}
```
