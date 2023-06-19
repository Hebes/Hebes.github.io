# Unity功能-防止鼠标点透UI

防止UI穿透

```c#
using UnityEngine.EventSystems;
class
{
    //检测到鼠标按下或触摸
    if(Input.GetMouseButtonDown(0)||(Input.touchCount>0&&Input.GetTouch(0).phase == TouchPhase.Began))
    {
        if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            //pc检测
            if(EventSystem.current.IsPointerOverGameobject())
                Debug.Log("当前触摸在UI上");
            else
                Debug.Log("当前没有触摸在UI上");
        }
    }
}

```

判断点是否在UI内

```c#
//以下方法有问题优先检查相机
//判断鼠标在UI内
RectTransformUtility.RectangleContainsScreenPoint(RectTransform, Input.mousePosition,Camera.main);

//判断鼠标是否进入该collider
Collider.OverLapPoint(mousePosition);

//判断UI是否在另一个UI内
public virtual bool ContainsUI(RectTransform container)
{
    RectTransform rt = transform.GetComponent<RectTransform>();
    //获取容器的四个顶点
    Vector3[] containerCorners = new Vector3[4];
    container.GetWorldCorners(containerCorners);
    float width = Mathf.Abs(containerCorners[2].x-containerCorners[0].x);
    float height = Mathf.Abs(containerCorners[2].y-containerCorners[0].y);
    Rect rect = new Rect(containerCorners[0].x, containerCorners[0].y, width,height);
    //获取需要判断的UI的四个顶点
    Vector3[] rtCorners = new Vector3[4];
    rt.GetWorldCorners(rtCorners);
    //依次判断四个顶点是否都在矩形中
    foreach(var corner in rtCorners)
    {
        if(!rect.Contains(corner))
            return false;            
    }
    Vector3 pos = Camera.main.ScreenToWorldPoint(transfrom.position);
    //?判断鼠标是否在UI内
    RectTransformUtility.ScreenPointToWorldPointInRectangle(container,Input.mousePosition, Camera.main, out pos);
    if(!rect.Contains(pos)) return false;
    return true;
}

//UI是否相交
public virtual bool IntersectUI(RectTransform container)
{
    RectTransform rt = transform.GetComponent<RectTransform>();
    //获取容器的四个顶点
    Vector3p[] ontainerCorners = new Vector3[4];
    container.GetWorldCorners(containerCorners);
    float width = Mathf.Abs(containerCorners[2].x - containerCorners[0].x);
    float height = Mathf.Abs(containerCorners[2].y - containerCorners[0].y);
    Rect rect = new Rect(containerCorners[0].x, containerCorners[0].y, width, height);
    //获取需要判断的UI的顶点
    Vector3[] rtCorners = new Vector3[4];
    rt.GetWorldCorners(rtCorners);
    //依次判断四个定点是否都在矩形中
    foreach(var corner int rtCorners)
    {
        if(rect.Contains(corner))
            return true;
    }
    return false;
}
```

Unity防止鼠标点透UI影响到被UI遮挡的游戏物体

```C#
private IEnumerator OnMouseDown()
{
    if (!EventSystem.current.IsPointerOverGameObject())
    {
    //选中被点击的物体
    }
}
```

就是加上EventSystem.current.IsPointerOverGameObject()这个判断   这段代码是  具有给定ID的指针是否位于EventSystem对象上   也就是说判断鼠标指针是否在UI上
