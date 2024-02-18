# 是否点击到UI

<https://blog.csdn.net/yzx5452830/article/details/82012990>
https://blog.csdn.net/qq_52786854/article/details/124677388
https://blog.csdn.net/qq_33994566/article/details/106069521
https://blog.csdn.net/u013012420/article/details/106999229

```C#
private bool IsTouchedUI()
{
    bool touchedUI = false;
    //TODO 移动端
    if (Application.isMobilePlatform)  
    {
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            touchedUI = true;
        }
    }
    //TODO PC端
    else if (EventSystem.current.IsPointerOverGameObject())
    {
        touchedUI = true;
    }
    return touchedUI;
}
```

