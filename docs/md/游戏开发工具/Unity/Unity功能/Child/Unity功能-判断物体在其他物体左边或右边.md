# 判断物体在其他物体左边或右边

float result = Vector3.Cross(Boss.transform.forward, 人物到Boss的向量).y

如果结果是正，则表示在Boss的右边，结果为负，表示在Boss的左边，具体代码：

```C#
public void V_GetDirectionLeftOrRight(GameObject Boss,GameObject Player)
{
    //设置目标点与Boss的y值相同
    Vector3 targetPos = new  Vector3(Player.transform.position.x,Boss.transform.position.y,Player.transform.position.z);
    //目标点与Boss连线的向量
    Vector3 vec = targetPos - Boss.transform.position;
    //求结果
    float result = Vector3.Cross(Boss.transform.forward,vec).y;
    if(y>0)
    {
        Debug.Log("在Boss左边");
    }
    else
    {
        Debug.Log("在Boss右边");
    }
}
```
