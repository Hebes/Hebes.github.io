# Unity功能-映射3D坐标到UI

[【UGUI】映射3D坐标到UI上（血条、人物状态）](https://blog.csdn.net/cyf649669121/article/details/111801841)

![【UGUI】映射3D坐标到UI上（血条、人物状态）](\../Image/Unity功能-映射3D坐标到UI/1.png)

``` C#
    /// <summary>
    /// 通过世界坐标设置UI的位置
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="worldPos"></param>
    public static void Do_SetUIPositionByWorldPos(this RectTransform rectTransform, Vector3 worldPos)
    {
        if (rectTransform == null)
            return;
 
        if (uiCamera == null)
            return;
 
        Vector3 screenPos = worldCamera.WorldToScreenPoint(worldPos);
        // Z小于0，代表在相机后面，此时X、Y反向；
        if (screenPos.z < 0)
        {
            screenPos.x *= -1;
            screenPos.y *= -1;
        }
 
        Vector3 uiWorldPos = uiCamera.ScreenToWorldPoint(screenPos);
        rectTransform.position = uiWorldPos;
        rectTransform.Do_SetLocalPosZ(0);
    }
 
    public static void Do_SetLocalPosZ(this Transform t, float z)
    {
        Vector3 localPos = t.localPosition;
        localPos.z = z;
        t.localPosition = localPos;
    }
```

## 参考

**[【UGUI】3D世界坐标转2DUI坐标并自适应的做法（血条，气泡）](<https://blog.csdn.net/sinat_34870723/article/details/89682782>)**
