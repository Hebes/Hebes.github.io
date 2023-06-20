# Unity编辑器-Inspector中OnValidate

编辑器模式下OnValidate 仅在下面两种情况下被调用：

- 脚本被加载时
- Inspector 中的任何值被修改时
  
## 演示1

![1](\../Image/Unity编辑器-Inspector中OnValidate/1.gif)

```C#
using UnityEngine;
using System.Collections;

public class OnValidateExample : MonoBehaviour {
    public int size;

    void OnValidate() {
        Debug.Log("OnValidate");
    }
}
```

## 角度简化

使用场景 - 我们需要将设计人员输入的角度限定在-359到359之间，因为360 相当于 0度。

![2](\../Image/Unity编辑器-Inspector中OnValidate/2.gif)

```C#
using UnityEngine;
using System.Collections;
 
public class OnValidateExample : MonoBehaviour {
    public float objectRotation;
 
    void OnValidate() {
        // objectRotation
        objectRotation = objectRotation % 360;
    }
}
```

## 二次方

使用场景 - 当需要设计人员输入 16 到 4096 之间 2的整数次幂时
Unity提供了ClosestPowerOfTwo函数，方便我们取得最接近的值。同时我们使用RangeAttribute 属性来限定一下输入数值的区间，同时能更好的看出来处理后的值跟原始输入值的区别。

![3](\../Image/Unity编辑器-Inspector中OnValidate/3.gif)

```C#
using UnityEngine;
using System.Collections;
 
public class OnValidateExample : MonoBehaviour {
    [RangeAttribute(16, 4096)]
    public int textureSize;

    void OnValidate() {
        // textureSize
        textureSize = Mathf.ClosestPowerOfTwo(textureSize);
    }
}
```

## 关联值

使用场景 -需要“Nitro”车的速度比其他车的速度大至少20mph.

![4](\../Image/Unity编辑器-Inspector中OnValidate/4.gif)

```C#
using UnityEngine;
using System.Collections;
 
public class OnValidateExample : MonoBehaviour {
    [RangeAttribute(10, 300)] [Tooltip("mph")]
    public int maxCarSpeed;
    [RangeAttribute(10, 300)] [Tooltip("mph")]
    public int maxNitroSpeed;

    const int minNitroSpeedExtra = 20;

    void OnValidate() {
        // speed check
        if (maxNitroSpeed < maxCarSpeed + minNitroSpeedExtra)
            maxNitroSpeed = maxCarSpeed + minNitroSpeedExtra;
    }
}
```

## 参考

![Unity3D 编辑器扩展 强大的OnValidate](<https://blog.csdn.net/piai9568/article/details/96645500>)
