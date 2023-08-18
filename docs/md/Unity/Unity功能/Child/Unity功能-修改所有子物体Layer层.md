# Unity功能-修改所有子物体Layer层

```c#
void Start () {
    foreach(Transform tran in GetComponentsInChildren<Transform>()){//遍历当前物体及其所有子物体
        tran.gameObject.layer = 30;//更改物体的Layer层
    }
}
```

当需要隐藏一部分物体时，可以使用Layer过滤。

在Camera中设置Culling Mask选择要观察的Layer
