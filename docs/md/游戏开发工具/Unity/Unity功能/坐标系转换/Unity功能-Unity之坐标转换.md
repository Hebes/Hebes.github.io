# Unity功能-Unity之坐标转换

**[Unity之坐标转换](<https://blog.51cto.com/u_15719772/5476655>)**

1. 世界坐标→屏幕坐标：
   > camera.WorldToScreenPoint(transform.position);这样可以将世界坐标转换为屏幕坐标。其中camera为场景中的camera对象。
2. 屏幕坐标→视口坐标：
   > camera.ScreenToViewportPoint(Input.GetTouch(0).position);这样可以将屏幕坐标转换为视口坐标。其中camera为场景中的camera对象。
3. 视口坐标→屏幕坐标：
   > camera.ViewportToScreenPoint();
4. 视口坐标→世界坐标：
   > camera.ViewportToWorldPoint()
