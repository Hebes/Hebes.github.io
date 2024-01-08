# Unity方法-协程

**[Unity协程的调用中止及yield return的使用](<http://www.javashuo.com/article/p-rleninkz-ge.html>)**

**[Unity 之 关于停止协程的五种方式解析](<https://blog.csdn.net/Czhenya/article/details/115936459>)**

**[Unity 协程(Coroutine)原理与用法详解](<https://blog.csdn.net/xinzhilinger/article/details/116240688>)**

在动态加载资源或建立对象的时候，因为数量较多，在同一帧完成大量计算可能形成程序卡顿现象，这里就要用到协程，将大量计算分散到多帧里面完成。或在程序执行到某一步，须要等待一段时间，而后在继续执行，也须要用到协程。还有其余一些状况下，充分利用协程，能够更好地实现功能。

```CSharp {.line-numbers}
public void Start()
{
    //开启协程
    Coroutine testCoroutine = StartCoroutine(Test());
    //中止指定协程
    StopCoroutine(testCoroutine)
    //协程能够同时开启多个
    StartCoroutine("Test");
    //经实测，StopCoroutine("Test")只能中止StartCoroutine("Test")开启的协程，对StartCorouti(Test())开启的协程无效
    StopCoroutine("Test")
    //中止本脚本内全部协程
    StopAllCoroutines();

    IEnumerator Test()
    {
        //等待下一帧Update以后，继续执行后续代码
        yield return null
        //等待在全部相机和GUI渲染以后，直到帧结束，继续执行后续代码
        yield return new WaitForEndOfFrame()
        //等待下一个FixedUpdate以后，继续执行后续代码
        yield return new WaitForFixedUpdate()
        //等待3秒以后，继续执行后续代码，使用缩放时间暂停协程执行达到给定的秒数
        yield return new WaitForSeconds(3.0f)
        //等待3秒以后，继续执行后续代码，使用未缩放的时间暂停协程执行达到给定的秒数
        yield return new WaitForSecondsRealtime(3.0f)
        //等待直到Func返回true，继续执行后续代码
        //yield return new WaitUntil(System.Func<bool>);
        yield return new WaitUntil(() => true)
        //等待直到Func返回false，继续执行后续代码
        //yield return new WaitWhile(System.Func<bool>);
        yield return new WaitWhile(() => false)
        //等待新开启的协程完成后，继续执行后续代码，能够利用这一点，实现递归
        yield return StartCoroutine(Test())
        //for循环
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1);
        
        //while循环，while(true)：若是循环体内有yield return···语句，不会由于死循环卡死
        int j = 0;
        while (j < 10)
        {
            j++;
            Debug.Log(j);
            yield return new WaitForSeconds(1);
        
        //终止协程
        yield break;
    }
```

```CSharp {.line-numbers}
//?IEnumerator :执行时会影响挂载的物体(例如WaitForSeconds时间截至前物体会保持不动,2019以后版本不影响)
public IEnumerator  MethodName()
{
    yield return new WaitForSeconds(float time);
    //Do Something
}
//执行协程
StartCorountine(MethodName);

yield return null;//下一帧再执行
yield return int;//下一帧再执行
yield break;//跳出协程
yield return asyncOperation;//等待异步操作结束后再执行后续代码
yield return StartCoroutinue(method());//等待method执行完后再执行后续代码
yield return WWW();//等待WWW操作完成后再执行后续代码
yield return new WaitForEndOfFrame();//等待帧结束,等待直到所有的摄像机和GUI被渲染完成后，在该帧显示在屏幕之前执行
yield return new WaitForSeconds(0.3f);//等待0.3秒之后执行后续代码(不受Time.TimeScale影响)
yield return new WaitFOrSecondsRealtime(0.3f);//等待0.3秒之后执行后续代码(不受Time.TimeScale影响)
yield return WaitForFixedUpdate();//等待下一次fixedUpdate开始时执行后续代码
yield return new WaitUntil();//直到为true时执行后续代码
yield return new WaitWhile();//直到为false时执行后续代码
```

**协程的不同用法：**

- yield 在下一帧上调用所有 Update 函数后，协程将继续。
- yield WaitForSeconds 在为帧调用所有 Update 函数后，在指定的时间延迟后继续协程
- yield WaitForFixedUpdate 在所有脚本上调用所有 FixedUpdate 后继续协程
- yield WWW 在 WWW 下载完成后继续。
- yield StartCoroutine 将协程链接起来，并会等待 MyFunc 协程先完成。

## 方式一：函数的方式

使用传递函数的方式来 开启协程:

```csharp
StartCoroutine(Cor_1());
```

停止协程：(❎ 错误的使用方式1)

```csharp
StopCoroutine(Cor_1());
```

**最初学习的时候就这么干的，但是不知道为什么就是不好用。后来才明白：虽然传递的是一样的函数名，但是停止时传递进去的并不是开始时传递的函数的地址啊~。**

停止协程：(❎ 错误的使用方式2)

```csharp
StopCoroutine(”Cor_1“);
```

新手的错误用法：使用**传递函数**的方式开启协程，使用**传递字符串**的形式停止协程。

那么使用`StartCoroutine(Cor_1());`这种方式开启协程，要如何才能手动停掉它呢？请继续往下看…

* * *

## 方式二：函数名的方式

使用传递函数名的方式 开启协程：

```csharp
StartCoroutine("Cor_1");
```

停止协程：

```csharp
StopCoroutine(”Cor_1“);
```

这样使用是没问题的（我猜测是内部是实现是通过<Key, Value>的形式保存了一下）。

缺点：**只支持传递一个参数**。

由一,二得出结论，只有通过函数名的形式开启和关闭是可行的，但是这并没有解决我们方式一中留下的问题，请继续往下看吧…

* * *

## 方式三：接收返回值

不管使用下面哪种方式启动协程，都可以结束其返回值用以停止对应协程；

```csharp
private Coroutine stopCor_1;
private Coroutine stopCor_2;

stopCor_1 = StartCoroutine("Cor_1");
stopCor_2 = StartCoroutine(Cor_2());
```

停止协程：

```csharp
StopCoroutine(stopCor_1);
StopCoroutine(stopCor_2); 
```

使用这种接收返回值的方式就可以根据我们的需求来停止协程了；  
这就解决了方式一,二中留下的问题。

* * *

## 方式四：StopAllCoroutines

任意一种方式开始协程

```csharp
StartCoroutine("Cor_1");
StartCoroutine(Cor_2());
```

都可以使用`StopAllCoroutines`去停止

```csharp
StopAllCoroutines();
```

**StopAllCoroutines() 可以停止当前脚本中所有协程。**

注意事项：

- 建议谨慎使用，因为可能后续修改逻辑时新建协程，在不需要被停止的情况下停止（别问我怎么知道的）
- 需要确定调用脚本的全部协程都需要被终止（比如：断线重连需要重置所有状态）

* * *

## 方式五：禁用/销毁游戏对象

注意是：

```csharp
gameObject.SetActive(false); 
//通过销毁游戏对象方式和禁用同效果
//Destroy(gameobject)
```

不是这个：

```csharp
script.enabled = false; 
```

也就是隐藏脚本所挂载的游戏物体（其父物体被隐藏时也是一样），如下图：  
![5.1.1](https://img-blog.csdnimg.cn/20210508085806965.png)  
**当物体被再次激活时，协程不会继续执行**

* * *

## 本文小结

1. 使用 StartCoroutine(函数()); 形式开启的，只能用接收返回值的形式去停止；【不限制参数个数】
2. 使用 StartCoroutine(“函数名”); 形式开启的，可以使用 StopCoroutine(“函数名”); 形式停止， 也可使用 接收返回值的形式去停止。【缺点：只可以传递一个参数】
3. 两种开启形式均受到 StopAllCoroutines() 控制。StopAllCoroutines() 可以停止当前脚本中所有协程。
4. gameObject.SetActive(false); 可停掉所有此GameObject上的所有协程，且再次激活时协程不会继续。
5. StopCoroutine(函数()); 脚本.enabled = false; 不可停掉协程。
