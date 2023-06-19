# Unity功能-LayerMask

## LayerMask

```c#
//位运算
/*
假设Player为第8层
LayerMask.GetMask("Player")效果等于(1<<8)
LayerMask.NameToLayer("Player")效果等于8
*/
//只碰撞第10层
LayerMask mask = 1<<10;
//碰撞除第10层以外的所有层(忽略第10层)
LayerMask mask = ~(1<<10);
LayerMask mask = ~LayerMask.GetMask(string maskName);
LayerMask mask = ~(1<<LayerMask.NameToLayer(string maskName));
//忽略第8层和第10层
LayerMask mask = ~((1<<8)|(1<<10));
```

## unity中的LayerMask用法

layerMask参数：`Raycast (ray : Ray, out hitInfo : RaycastHit, distance : float = Mathf.Infinity, layerMask : int = kDefaultRaycastLayers)`

```C#
RaycastHit hit;
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
if (Physics.Raycast(ray, out hit, 1000, 1<<LayerMask.NameToLayer("Ground")))
    Log.Debug(" hit :" + hit.point );
else
    Log.Debug("ray cast null!");
//画线，使其射线可视化。 
Debug.DrawLine(RayStartPoint.position, hit.point, Color.red,100);
```

1. 打开第10的层
    > 1 << 10 打开第10的层。
    > 等价于[1 << LayerMask.NameToLayer(“Ground”);]
    > 也等价于[LayerMask.GetMask(“Ground”);]
2. 打开除了第10之外的层
    > ~(1 << 10) 打开除了第10之外的层。
3. 打开所有的层
    > ~(1 << 0) 打开所有的层。
4. 打开第10和第8的层
    > (1 << 10) | (1 << 8) 打开第10和第8的层。
    > 等价于[LayerMask.GetMask((“Ground”, “Wall”);]

在代码中使用时如何开启某个Layers？
LayerMask mask = 1 << 你需要开启的Layers层。
LayerMask mask = 0 << 你需要关闭的Layers层。

> LayerMask mask = 1 << 2; 表示开启Layer2。
> LayerMask mask = 0 << 5;表示关闭Layer5。
> LayerMask mask = 1<<2|1<<8;表示开启Layer2和Layer8。
> LayerMask mask = 0<<3|0<<7;表示关闭Layer3和Layer7。
上面也可以写成：
> LayerMask mask = ~（1<<3|1<<7）;表示关闭Layer3和Layer7。
> LayerMask mask = 1<<2|0<<4;表示开启Layer2并且同时关闭Layer4.

## 参考

[Unity 基础 之 Layer(层layer) 、LayerMask (遮罩层) 的 总结 和 使用（CullingMask、Ray 射线的使用等](<https://blog.csdn.net/u014361280/article/details/112671632>)