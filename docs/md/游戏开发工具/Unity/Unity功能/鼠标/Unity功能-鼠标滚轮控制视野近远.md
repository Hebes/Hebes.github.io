# Unity功能-鼠标滚轮控制视野近远

## 方案一

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FllowPlayer : MonoBehaviour {

    public float scrollSpeed = 10;//滑轮滚动速度
    private Transform player;//主角的位置变量
    private Vector3 offsetPosition; //位置偏移
    public float distance = 0;//位置偏移的向量长度
    
    void Start () 
    {
        //找到主角的位置
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        //主角与摄像机之间的偏移
        offsetPosition = transform.position - player.position;
    }

    void Update () 
    {
        //调用处理视野的拉近和拉远方法
        ScrollView();
    }

    private  void ScrollView()
    {
        //向后滑动返回负值  向前滑动返回正值
        distance = offsetPosition.magnitude;//位置偏移的向量长度
        distance -= Input.GetAxis("Mouse ScrollWheel") *scrollSpeed;//获取滚轮值的改变
        distance = Mathf.Clamp(distance, 2, 15);//限制滚轮距离的范围，此数值可根不同需求设置相应的值
        offsetPosition = offsetPosition.normalized * distance;  //单位向量  方向不变 距离改变
        //镜头带缓冲
        transform.position = Vector3.Lerp(transform.position,  player .transform.position + offsetPos, Time.deltaTime * 4);
        //镜头不带缓冲
        //transform.position = player .transform.position + offsetPos;
    }
}
```

## 方案二

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 
public class Follw_Camera : MonoBehaviour {
 
 
    public GameObject Player;  //声明需要跟随的玩家
    private Vector3 offset;   //差值
    private Transform playerTransform;  //声明玩家的Transform组件 
    private Transform cameraTransform;  //声明相机的Transform组件 
    public float distance = 0;
    public float scrollSpeed = 10;
    
    
    // Use this for initialization
    void Start () {
        playerTransform = Player.GetComponent<Transform> (); //得到玩家的   Transform组件
        cameraTransform = this.GetComponent<Transform> ();   //得到相机的   Transform组件
        offset = cameraTransform.position - playerTransform.position;  //得到相机和玩家位置的差值
    }
        
    // Update is called once per frame
    void Update () {
        this.transform.position = playerTransform.position + offset;  //玩家的  位置加上差值赋值给相机的位置
        ScrollView();
    }
    void ScrollView(){
        //print (Input.GetAxis ("Mouse ScrollWheel"));
        distance = offset.magnitude;
        distance -= Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed;  //往前  滑动是正值
        if(distance > 26){   //如果距离大于26，就返回26
            distance = 26;
        }
        if(distance < 5){    //如果距离小于5，就返回5
            distance = 5;
        }
        offset = offset.normalized * distance;
    }
}
```

## 其他参考网站

**[推拉变焦（也称为“伸缩变焦”效果）](<https://docs.unity.cn/cn/2018.4/Manual/DollyZoom.html>)**

**[【Unity】战斗摄像机效果：视野拉近，屏幕抖动](<https://juejin.cn/post/7090496062309269534>)**

**[Unity相机的跟随、拉进拉远、 旋转的效果实现](<https://www.cnblogs.com/Damon-3707/p/11640476.html>)**
