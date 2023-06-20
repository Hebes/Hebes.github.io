# Unity功能-获取分辨率

获取屏幕分辨率

```c#
//实际获取的是系统记录的分辨率,不是物理分辨率,若屏幕2560*1600,缩放为200%,则获取到的为1920*1080
[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
private static extern int GetSystemMetrics(int index);
private static int SM_CXSCREEN = 0; //主屏幕分辨率宽度
private static int SM_CYSCREEN = 1; //主屏幕分辨率高度
private static int SM_CYCAPTION = 4; //标题栏高度
private static int SM_CXFULLSCREEN = 16; //最大化窗口宽度(减去任务栏)
private static int SM_CYFULLSCREEN = 17; //最大化窗口高度(减去任务栏)
private void Test()
{
    //屏幕分辨率
    int x = GetSystemMetrics(SM_CXSCREEN);//x=1920
    int y = GetSystemMetrics(SM_CYSCREEN);//y=1080
    
    //屏幕工作区域
    int x1 = GetSystemMetrics(SM_CXFULLSCREEN);
    int y1 = GetSystemMetrics(SM_CYFULLSCREEN);
    
    //标题栏高度
    int title = GetSystemMetrics(SM_CYCAPTION);
    
    //不最大化,不全屏的最大窗口高度
    int maxHeight = y1 - title;
}
```
