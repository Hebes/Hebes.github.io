# Unity方法-协程

## 协程

Unity协程的调用中止及yield return的使用
来自:<http://www.javashuo.com/article/p-rleninkz-ge.html>

在动态加载资源或建立对象的时候，因为数量较多，在同一帧完成大量计算可能形成程序卡顿现象，这里就要用到协程，将大量计算分散到多帧里面完成。或在程序执行到某一步，须要等待一段时间，而后在继续执行，也须要用到协程。还有其余一些状况下，充分利用协程，能够更好地实现功能。html

分享一下我的对协程的一些使用理解：

```javaScript {.line-numbers}
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

```c#
//?IENumerator:执行时会影响挂载的物体(例如WaitForSeconds时间截至前物体会保持不动,2019以后版本不影响)
public IENumerator MethodName()
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
