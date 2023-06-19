# Unity功能-鼠标显示隐藏

```c#
//设置鼠标坐标
[System.Runtime.InteropServices.DllImport("user32.dll")]
public static extern int SetCursorPos(int x, int y);

Cursor.Visible = false;//隐藏鼠标
Cursor.lockState = CursorLockMode.Locked;//锁定鼠标至屏幕中间
Cursor.lockState = CursorLockMode.None;//取消锁定
```

```C#
/// <summary>
/// 是否显示鼠标
/// </summary>
/// <param name="self"></param>
public static void SetMouse(this MouseManagerComponent self)
{
    self.isShow = !self.isShow;
    switch (self.isShow)
    {
        default: Cursor.lockState = CursorLockMode.Locked; break;
        case false: Cursor.lockState = CursorLockMode.None; break;
    }
}
```
