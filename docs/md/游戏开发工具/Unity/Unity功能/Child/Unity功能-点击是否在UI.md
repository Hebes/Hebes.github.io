# Unity功能-点击是否在UI

## 代码

这几天在做捕鱼达人游戏时发现，当鼠标点击UI时，炮台的子弹也会发射子弹，这样会影响用户体验。

然后网上百度了一波，发现在UGUI系统上，的EventSystem提供了一些方法。

那就是EventSystem.current.IsPointerOverGameObject（）方法，作用：判断鼠标是否点击在UI上。

因此，我们可以在开火前做一个判断

在窗口端进行判断时使用：

if（Input.GetMouseButtonDown（0）&&！EventSystem.current.IsPointerOverGameObject（））

{

//生成子弹

}

在Android的段运行时使用：

if（Input.touchCount> 0 && Input.GetTouch（0）.phase == TouchPhase.Began）//手指按下并且触点大于0
 {
     if（！EventSystem.current.IsPointerOverGameObject（Input.GetTouch（0）.fingerId ））//没点到UI
        {
         }

}

 

一： 下面先说经常用的三个事件  手指按下、手指移动、手指松开
1.  手指按下
if(input.touchCount==1)
{
   if(input.touches[0].phase==TouchPhase.Beagn)
   {
          // 手指按下时，要触发的代码
   }

2.  手指在屏幕上滑动
if(input.touchCount==1)
{  
     if(input.touches[0].phase==TouchPhase.Move)  
     {         
           // 手指滑动时，要触发的代码 
          float s01=Input.getAxis("Mouse X");    //手指水平移动的距离
          float s02=Input.getAxis("Mouse Y");    //手指垂直移动的距离
     }​​​​​​​

3.  手指在屏幕上松开时
​   if(input.touches[0].phase==TouchPhase.Ended)&&                        Input.touches[0].phase!=TouchPhase.Canceled  ​​

二： 上面介绍的是单手指触发事件，下面介绍的是多手指触发事件
if(touchCount==2)   //代表有两个手指
{
   if(Input.getTouch(0).phase==TouchPhase.Moved&&    //第一个手指                  Input.getTouch(1).phase==TouchPhase.Moved)            //第二个手指
    {
          vecter3 s1=input.getTouch(0).position;         //第一个手指屏幕坐标
          vecter3 s2=input.getTouch(1).position;         //第二个手指屏幕坐标
          newdis=Vecter2.distance(s1,s2);
          if(newdis>olddis)             //手势外拉
          { 
                distance+=Time.deltaTime*50f;
          }
          if(newdis
          {
                distance-=Time.deltaTime*50f;
          }
         olddis=newdis;
    }
}
​​​​​​​​​​​​​​
总结：
1.  不管是触屏事件还是PC端的事件，世界转屏幕还是屏幕转世界以及射线检测都是管用的
2.  安卓端的手指坐标（Input.touches[0].position）等同于PC端的鼠标屏幕坐标
(Input.mousePosition)

这样就行了。但是！但是！结果运行起来，点击UI是无法生成子弹，这是实现了，可是点击没有UI的地方却无法生成子弹，这就令人很崩溃了！

原来在我们的背景图上，它也是一个UI Image，那这样这个判断是否点击UI上的方法岂不是实现不了？

但是EventSystem貌似早就为我们想好了。在图像组件上的有一个Raycast Type的复选框，只要我们把勾选去掉，就行了。

因此我推测EventSystem.current.IsPointerOverGameObject（）方法的原理是，是根据UI上的Raycast Target的勾选来遍历，那些UI需要鼠标点击判断，那些不需要。

## 参考

[EventSystem.IsPointerOverGameObject](<https://docs.unity.cn/cn/2018.2/ScriptReference/EventSystems.EventSystem.IsPointerOverGameObject.html>)
