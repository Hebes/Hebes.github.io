# Unity功能-帧率

Unity提供了API Application.targetFrameRate=60来设置帧率。
但需要注意的是Project Setting中开启Vsync时会使该设置失效，实际运行帧率将和硬件的刷新率挂钩。
考虑到移动端项目的特性，一般锁30帧即可，在高端机或性能压力较低的场景可以选择动态设置60甚至更高的帧率。

```C#
using UnityEngine;
public enum LimitType
{
    NoLimit = -1,
    Limit30 = 30,
    Limit60 = 60,
    Limit120 = 120
}

public class FramerateLimitSampLe: MonoBehaviour
{
    public LimitType LimitType;

    private void Awake()
    {
        Application.targetFrameRate = (int)LimitType;
    }
}
```
