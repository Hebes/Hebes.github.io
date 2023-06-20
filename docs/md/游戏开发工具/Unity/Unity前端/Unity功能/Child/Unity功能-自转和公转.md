# Unity功能-自转和公转

自转代码

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotation : MonoBehaviour
{
    public float RotateSpeed =1.0f;//定义一个的旋转速度

    void Update()
    {
        //沿着Y轴正方向以一定的速度旋转，Space.World意思是在世界坐标系里
        this.transform.Rotate(Vector3.up*RotateSpeed ,Space.World);
    }
}
```

公转代码

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundRotation : MonoBehaviour
{
    public Transform Target;//环绕旋转的目标对象 
    public float AroundSpeed=1.0f;//公转的速度

    void Update()
    {
        //  RotateAround是系统定义的环绕旋转方法，它对应的参数分别是围绕目标的位置、围绕的轴  和旋转速度
        this.transform.RotateAround(Target.position,Vector3.up,AroundSpeed);
    }
}
```
