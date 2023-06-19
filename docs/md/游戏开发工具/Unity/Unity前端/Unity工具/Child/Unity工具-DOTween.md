# Unity工具-DOTween

## 参考

**[Dotween常用方法详解](<https://blog.csdn.net/zcaixzy5211314/article/details/84886663>)**

**[Dotween Path 路径动画使用方法详解](<https://blog.csdn.net/zcaixzy5211314/article/details/84988535>)**

**[Dotween常见问题及使用方式](<https://blog.csdn.net/zcaixzy5211314/article/details/85749734>)**

**[运动曲线参考网站](<https://robertpenner.com/easing/easing_demo.html>)**

```c#
Using DG.Tweening;
public class Name:MonoBehaviour
{
    //looptype.Incremental 增量形式
    material.DOColor(Color.red, 属性名, time).SetLoops(10, LoopType.Yoyo);
    // arg1:颜色;arg2:要改变颜色的属性名;arg3:时间;
    material.DOFade(0, "_TintColor", 2);
    material.DoGradientColor(gradient, 2);
    //混合颜色
    material.DoBlendableColor(Color.red, 2);
    //移动
    //arg1:目标位置;arg2:使用时间
    transform.DoMove(position, 2);
    transform.DOMove(position, 2);//两条相同作用的代码只会执行第二条    
    //旋转
    transform.DORotate(new Vector3(0,0,90), 2);
    //缩放
    transform.DOScale(Vector3.one*2, 2);
    //从一个值到另一个值
    DoTween.To;
    DoTween.To(()=>CurrentValue, x=>CurrentValue=x, targetValue, time));
}
```
