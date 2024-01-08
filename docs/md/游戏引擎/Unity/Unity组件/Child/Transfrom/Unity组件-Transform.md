# Unity组件-Transform

## [Unity]旋转的三种实现方式

### Transform.Rotate 欧拉角旋转

### Transform.Rotate(float xAngle, float yAngle, float zAngle)

「xAngle」以x轴为旋转轴中心每帧旋转的角度

「yAngle」以y轴为旋转轴中心每帧旋转的角度

「zAngle」以z轴为旋转轴中心每帧旋转的角度

```csharp
void Update ()
{
    // 每一帧，以y轴为旋转轴旋转1°
    transform.Rotate(0f, 1f, 0f); 
    // 如果是个人物模型，第一人称看到的效果就是一直向右转
    // 每帧旋转1°
    // 左手点赞手势，大拇指指向轴正方向，手指蜷曲方向为旋转正方向
}
```

### Transform.Rotate(Vector3 eulerAngles)

用 Vector3 eulerAngles 代替 三个旋转角

```csharp
void Update () 
{
    transform.Rotate(new Vector3(0f,1f,0f));
    // 每一帧，以y轴为旋转轴旋转1°
}
```

### Transform.Rotate(Vector3 axis, float angle, Space relativeTo)

「Vector3 axis」 旋转轴，new Vector3(0f,1f,0f) 就代表以Y轴为旋转轴，值大小不影响旋转速度

「angle」 每一帧的旋转角度

「Space relativeTo」相对于本地坐标还是世界坐标

```csharp
void Update () 
{
    transform.Rotate(new Vector3(0f, 1f, 0f), 1f, Space.Self);
    // 每一帧，以本地坐标系（自身）y轴为旋转轴，旋转1°
}
```

其它解释按方法注解类推。

### Transform.RotateAround  一个物体围绕另一个物体旋转

### Transform.RotateAround(Vector3 point, Vector3 axis, float angle)

「Vector3 point」围绕哪个点旋转

「Vector3 axis」自身哪个轴围绕这个点旋转

「angle」每一帧旋转的角度

```csharp
void Update ()
{
    transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0f, 1f, 0f), 1f);
    // new Vector3(0, 0, 0) 改成自己的position，就可以实现和上面一样的效果
}
```

个人理解为，空间中一点 point 向 Vector3 axis延长线做追垂线，然后垂线抓住axis，进行旋转。 打个比方，空间中有一根棍子，记作y轴，然后某一点，向y做垂线，垂线记作z轴，那么以这一点为中心，向x方向做圆周运动。

### 直接修改旋转角度

### 修改欧拉角 eulerAngles和localEulerAngles

「eulerAngles」世界坐标系中以欧拉角表示的旋转

「localEulerAngle」本地坐标中以欧拉角表示的旋转

```csharp
void Update ()
{
    // 改变y轴旋转角度30°
    transform.eulerAngles = new Vector3(0, 30f, 0);
    // 旋转至某一角度（有过程）
    float speed = 5.0f;
    transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0, 30f, 0), Time.deltaTime * speed);
}
```

### rotation和localRotation

四元数用于表示旋转，所有的 rotation都为Quaternion类型，没有Vector3类型的 eulerAngles 直观，但其不受万向锁影响，可以轻松插值运算。

「rotation」世界坐标系中以四元数表示的旋转

「localRotation」本地坐标中以四元数表示的旋转

```csharp
void Update () 
{   
    // 改变y轴旋转角度30°
    transform.rotation = Quaternion.Euler(new Vector3(0, 30f, 0));
    // 旋转至某一角度（有过程）
    float speed=10f;        
    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 30f, 0)), Time.deltaTime * speed);
}
```

## API笔记Transform详解及使用方法

### 二、常用变量与属性（位置）

#### 位置

position：世界坐标

localposition：相对坐标（父物体就是世界，相对于父物体的坐标）

```auto
 private void OnGUI()
    {
        //世界坐标
        GUILayout.Label(string.Format("Transform's position:{0}", transform.position));
        //相对坐标
        GUILayout.Label(string.Format("Transform's localPosition:{0}", transform.localPosition));
    }
```

#### 角度

enlerAnles，localEnlerAnles欧拉角；rotation，localRotation四元数旋转角度（自然数）。

```auto
private void OnGUI()
    {
        //相对世界的欧拉角
        GUILayout.Label(string.Format("Transform's eulerAngles:{0}", transform.eulerAngles));
        //相对父物体的欧拉角
        GUILayout.Label(string.Format("Transform's localEulerAngles:{0}", transform.localEulerAngles));
        //相对世界的rotation
        GUILayout.Label(string.Format("Transform's rotation:{0}", 
transform.rotation));
        //相对父物体的rotation
        GUILayout.Label(string.Format("Transform's localRotation:{0}", transform.localRotation));
        //欧拉角转换成自然数方式
        transform.rotation = Quaternion.EulerAngles(270, 0, 0);
    }
```

#### 缩放

localScale相对父级缩放比例；lossyScale相对世界缩放比例。

```auto
 private void OnGUI()
    {
        //相对父级缩放
        GUILayout.Label(string.Format("Transform's localScale:{0}", transform.localScale));
        //相对世界的缩放
        GUILayout.Label(string.Format("Transform's lossyScale:{0}", transform.lossyScale));
    }
```

#### 其他

right（向左），up（向上），forward（向前）方向向量。

```auto
 private void OnGUI()
    {
        //向右
        GUILayout.Label(string.Format("Transform's right:{0}", transform.right));
        //向上
        GUILayout.Label(string.Format("Transform's up:{0}", transform.up));
        //向前
        GUILayout.Label(string.Format("Transform's forward:{0}", transform.forward));
    }
```

parent（父级），root（根物体），childCount（多少个子级）子父级相对。

![1](https://img-blog.csdnimg.cn/20200524225309632.png)

```auto
 private void OnGUI()
    {
        //向右
        GUILayout.Label(string.Format("Transform's right:{0}", transform.parent.name));
        //向上
        GUILayout.Label(string.Format("Transform's up:{0}", transform.root.name));
        //向前
        GUILayout.Label(string.Format("Transform's forward:{0}", transform.childCount));
    }
```

### 三、移动

#### 1.直接修改位置

小技巧：优化，缓存和不缓存。

```auto
public Transform trans;
    public int Count = 1000;     //缓存次数
    private long NoCache = 0;    //不缓存
    private long Cache = 0;    //缓存
    // Start is called before the first frame update
    void Start()
    {
        trans = this.transform; //获得trans,能提高效率,缓存

        Stopwatch sw = new Stopwatch();
        sw.Start();       //启动Stopwatch
        for (int i = 0; i < Count; i++) {     //循环次数
            Transform t = this.transform;
        }
        sw.Stop();
        NoCache = sw.ElapsedTicks;

        sw.Reset();

        sw.Start();
        for (int i = 0; i < Count; i++) {
            Transform t = trans;      //直接获取trans
        }
        sw.Stop();
        Cache = sw.ElapsedTicks;
    }

    private void OnGUI()
    {
        //向右
        GUILayout.Label(string.Format("Transform's NoCache:{0}", NoCache));
        //向上
        GUILayout.Label(string.Format("Transform's Cache:{0}", Cache));
    }
```

![1](https://img-blog.csdnimg.cn/20200524231548237.png)

移动

```auto
    public Transform trans;
    private bool moveToLeft = true;  //控制左右移动
    private float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        trans = this.transform; //获得trans,能提高效率,缓存

    }
      
    // Update is called once per frame
    void Update()
    {
      Move();
      //  trans.position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f)); //直接移动，随机移动，随机数-3*3
      //  trans.position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f)); //直接移动，随机移动，随机数-3*3
    }

    private void Move()
    {
        if (trans.position.x <= -3 && moveToLeft) {     //在x轴-3到3移动，向左移动为true
            moveToLeft = false;    //向右移动
        } else if(trans.position.x >= 3 && !moveToLeft){
            moveToLeft = true;      //向左移动
        }
       trans.position += (moveToLeft?Vector3.left : Vector3.right) * Time.deltaTime * speed;  //判断向左向右，后面的话是指每秒；如果向左移动，每秒钟向左移动两个单位。。。
    }
```

#### 2.Translate函数

1. 1 依据自身坐标移动

```auto
 private void Translate()
    {
        if (transform.position.x < -3 && moveToLeft)
        {     //在x轴-3到3移动，向左移动为true
            moveToLeft = false;    //向右移动
        }
        else if (transform.position.x >= 3 && !moveToLeft)
        {
            moveToLeft = true;      //向左移
        }
        transform.Translate((moveToLeft ? Vector3.left : Vector3.right) * Time.deltaTime * speed, Space.Self);
    }
```

1. 2 space可以控制是沿自身方向（space.Self）移动还是沿世界方向（space.World）移动。

下面这段代码是沿着世界向前移动。

```auto
public Transform trans;
 void Start()
    {
        trans = this.transform;
    }
 void Update()
    {
        Translate(Space.World);
}
 private void Translate(Space space) {
        trans.Translate(Vector3.forward *Time.deltaTime *2, space);
    }
```

### 四、旋转

1. 直接修改欧拉角和旋转角

    修改eulerAngles欧拉角和rotation定义旋转角度代码如下：

    ```auto
    public Transform trans;
        public float speed = 90;

        private void Start() {
            trans = this.transform;
                }
        private void Update()
        {
            //  trans.eulerAngles = new Vector3(0, 0, 45);      //在z轴旋转45°
            trans.eulerAngles = new Vector3(0, 0, 45);      //相对父级
            trans.rotation = Quaternion.Euler(0,0,45);       // 
            trans.localRotation = Quaternion.Euler(0, 0, 45);     //相对父级

                    // trans.eulerAngles += Time.deltaTime * speed * Vector3.forward;    //沿着z轴旋转
            trans.rotation = Quaternion.Euler(trans.eulerAngles.x, trans.eulerAngles.y, trans.eulerAngles.z + Time.deltaTime *speed); //沿着z轴旋转
        }
    ```

2. Rotate，RotateAround和LookAt函数

    1. Rotate函数实现旋转功能跟修改欧拉角和旋转角进行旋转有一样的效果，只需要填写一个方向和旋转的速度。

        ```auto
        private void Rotate() {
                trans.Rotate(Vector3.forward * Time.deltaTime * speed);     //跟修改欧拉角和旋转角进行旋转有一样的效果，只需要填写一个方向和旋转的速度
            }
        ```

    2. RotateAround函数实现sphere围绕cube旋转，代码如下：

        ```auto
        public Transform target;    //围绕的点

            private void Rotate() {
                trans.RotateAround(target.position, target.up, Time.deltaTime *speed); //围绕旋转的点、轴向、速度
            }
        ```

    ![1](https://img-blog.csdnimg.cn/20200525131435447.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0FuZ2xlcG9wcHk=,size_16,color_FFFFFF,t_70)

3. LookAt函数，z轴看向，比如把sphere移动一下，cube也是一直始终看向sphere。

    ```auto
    public Transform target;    //围绕的点
        private void LookAt() {
            trans.LookAt(target);
        }
    ```

    3.利用right，up，forward改变方向

    ```auto
    private void ChangeDir() {
            Vector3 dir = target.position - trans.position;    //trans指向target
            trans.up = dir;      
        }
    ```

### 五、缩放

1. 直接修改缩放比例

    ```auto
    public Transform trans;
        public float speed = 1;
        public bool ZoomInt;    //放大还是缩小

        private void Start()
        {
            trans = this.transform;
        }
        private void Update()
        {
            Zoom();
        }

        public void Zoom() {
            if (trans.localScale.x <= 0 && !ZoomInt) {     //放大
                ZoomInt = true;
            } else if(trans.localScale.x <=3 && ZoomInt)       //缩小
            {
                ZoomInt = false;
            }
            trans.localScale += (ZoomInt ? Vector3.right : Vector3.left) * Time.deltaTime*speed;      //是否放大，left相当于-right
        }
    ```

### 五、其他常用函数

1. TransformDirection和InverseTransformDirection

    1. TransformDirection可以实现人物可以朝着摄像机的方向移动而不一定是世界坐标，把希望的方向转给trans，代码如下。

        ```auto
        public Transform trans;

            private void Start()
            {
                trans = this.transform;
            }
            private void Update()
            {
                Debug.DrawRay(trans.position, trans.TransformDirection(Camera.main.transform.forward) *10, Color.red);     // Debug.DrawRay()画一根线，从trans的位置
            }
        ```

    2. InverseTransformDirection，处理镜像运动。

        Debug.DrawRay(trans.position, trans.InverseTransformDirection(Camera.main.transform.forward) \* 10, Color.yellow);

2. DetachChilder，Find，GetChild和IsChildOf

    ```auto
    private Transform trans;

        public Transform Child;

        private void Start()
        {
            trans = this.transform;
        }
        private void Update()
        {
        }
        private string _IsChild;   //记录
        private void OnGUI()
        {
            if (GUILayout.Button("Detch Children")) {         //将当前transform下的子物体分离出来，取消他们的一个关系
                trans.DetachChildren();
            }
            if (GUILayout.Button("Find Children"))           //查找"Child"
            {
                trans.Find("Child").position = Vector3.one * 3;
            }
            if (GUILayout.Button("GetChildren"))         //获得子物体的方式，是根据索引值来获取的
            {
                trans.GetChild(0).position = Vector3.one * 5;
            }
            if (GUILayout.Button("IsChildOf"))            //检测当前transform是不是我们参数里填的transform的子物体
            {
                _IsChild = Child.IsChildOf(trans).ToString();
            }
            GUILayout.Label(_IsChild);
        }
    ```

![1](https://img-blog.csdnimg.cn/20200525141245422.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0FuZ2xlcG9wcHk=,size_16,color_FFFFFF,t_70)

## 只修改一个坐标系的值

Unity在修改物体位置、大小、旋转角度时，一般采用的都是赋值的方式如：

```C#
transform.localEulerAngles = newVector3(0, 0, 0);
transform.localPosition = newVector3(0, 0, 0);
transform.localRotation = newQuaternion(0, 0, 0, 0);
transform.localScale = new Vector(1, 1, 1);
```

当有的时候只需要修改某一个轴的值时，可如下方式：

```C#
transform.localScale += new Vector3(0, 0, 50);
```

但这只是方式在Update()等方法中就会出现累加的情况，如何做到只修改一次：

```C#
var v = transform.localPosition;
v.z = 50;
transform.localPosition = v;
```

先获取到当前值，并赋值给另一个变量，然后对变量就行修改，最有赋给自己！

## 参考网站

**[重要的类 - Transform](<https://docs.unity.cn/cn/2021.2/Manual/class-Transform.html>)**

**[Unity之脚本API笔记一（Transform详解及使用方法）](<https://blog.csdn.net/anglepoppy/article/details/106322296>)**

**[Unity设置物体旋转角度误区](<https://gwb.tencent.com/community/detail/121547>)**

**[[Unity]旋转的三种实现方式](<https://juejin.cn/post/7050784202207264805>)**

**[[Unity]旋转的三种实现方式](<https://juejin.cn/post/7050784202207264805>)**
