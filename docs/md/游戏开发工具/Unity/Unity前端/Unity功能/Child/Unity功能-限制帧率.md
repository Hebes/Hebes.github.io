# Unity功能-限制帧率

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
