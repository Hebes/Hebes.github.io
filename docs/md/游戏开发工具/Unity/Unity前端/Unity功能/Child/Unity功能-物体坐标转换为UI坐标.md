# Unity功能-物体坐标转换为UI坐标

## 方法1

```c#
//方法1 根据视口坐标转换
public static Vector3 GetScreenPosition(Transform target, Canvas can, Camera cam)
{
    Vector3 viewportPos = cam.WorldToViewportPoint(target.transform.position);
    RectTransform canvasRtm = can.GetComponent<RectTransform>();
    Vector2 uguiPos = Vector2.zero;
    uguiPos.x = (viewportPos.x - 0.5f) * canvasRtm.sizeDelta.x;
    uguiPos.y = (viewportPos.y - 0.5f) * canvasRtm.sizeDelta.y;
    return uguiPos;
}
```

## 方法2

```C#
//方法2 根据屏幕坐标转换
public static Vector3 GetScreenPosition(Transform target, Canvas can, Camera cam)
{
    RectTransform canvasRtm = can.GetComponent<RectTransform>();
    float width = canvasRtm.sizeDelta.x;
    float height = canvasRtm.sizeDelta.y;
    Vector3 pos = cam.WorldToScreenPoint(target.transform.position);
    pos.x *= width / Screen.width;
    pos.y *= height / Screen.height;
    pos.x -= width * 0.5f;
    pos.y -= height * 0.5f;
    return pos;
}
```

## 跟随鼠标移动

```c#
//RectTransform
//Canvas的Render Mode为Screen Space-Camera时
//UI跟随鼠标
RectTransformUtility.ScreenPointToWorldPointInRectangle(RectTransform:Canvas,Vector3:ScreenPosition, Camera, out Vector3);
//该canvas下的UI
RectTransform _ui;
_ui.position = newPos;
//or
RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform:Canvas,Vector3:ScreenPosition, Camera, out Vector2);
_ui.localPosition = newPos;

//Transform跟随鼠标移动 2D
Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
GameObject worldObj;
worldObj.transform.position = new Vector3(newPos.x, newPos.y,0);
```