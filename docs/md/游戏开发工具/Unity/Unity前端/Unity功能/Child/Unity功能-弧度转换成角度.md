# Unity功能-弧度转换成角度

## 判断鼠标弧度

```C#
/// <summary>
/// 判断鼠标是左边还是右边
/// </summary>
/// <returns></returns>
private bool MouseRightOrLeft(out float vector)
{
    //鼠标在屏幕的位置 GetMouseWorldPosition请Ctrl + f
    Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
    //偏移量
    Vector3 aimDir = (mousePosition - transform.position).normalized;
    //返回Tan为y/x的角度(以弧度为单位)。弧度到角度转换常数(只读)。
    float angle = vector = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
    return (angle > 90 || angle < -90) ? true : false;
}
```

```C#
//使用方法
playerComponent.T_GunTransform.eulerAngles = new Vector3(0, 0, angle);
```

## 鼠标的弧度

```C#
public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) 
{
    Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
    return worldPosition;
}
```

```C#
/// <summary>
/// Get Mouse Position in World with Z = 0f
/// 获得鼠标在世界中的位置 Z = 0f
/// </summary>
/// <returns></returns
public static Vector3 GetMouseWorldPosition() 
{
    Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    vec.z = 0f;
    return vec;
}
```

```C#
/// <summary>
/// 判断鼠标是左边还是右边
/// </summary>
/// <returns></returns>
private bool MouseRightOrLeft(out floatvector)
{
    //鼠标在屏幕的位置
    Vector3 mousePosition = UtilsClassGetMouseWorldPosition();
    //偏移量 transform是人物
    Vector3 aimDir = (mousePosition -transform.position).normalized;
    //返回Tan为y/x的角度(以弧度为单位)。弧度角度转换常数(只读)。
    float angle = vector = Mathf.Atan(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
    return (angle > 90 || angle < -90) ?true : false;
}
```

```C#

/// <summary>
/// 处理瞄准目标的方法
/// </summary>
private void HandleAiming()
{
    //单个图片的变换方向
    //设置枪械变换朝向
    if (MouseRightOrLeft(out float angle))
    {
        playerComponent.T_GunTransformlocalScale = new Vector2(-1, -1);/枪旋转
        transform.localScale = new Vector(-1, 1);//玩家旋转
    }
    else
    {
        playerComponent.T_GunTransformlocalScale = Vector2.one;//枪旋转
        transform.localScale = Vector2one; //玩家旋转
    }
    playerComponent.T_GunTransformeulerAngles = new Vector3(0, 0, angle);
}
```

```C#
private bool InteractwithUI()
{
    return EventSystem.current != null &&EventSystem.currentIsPointerOverGameObject();
}
```

```C#
public void Update()
{
    HandleAiming();
    if (!InteractwithUI())
    {
        HandleShooting();//修正穿透UI点进
    }
}
```
