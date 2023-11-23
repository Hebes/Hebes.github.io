# Unity功能-计算消耗时间

## 计算消耗时间

用来计算消耗的时间

``` C#
DateTime beforeDT = DateTime.Now;
//todo这里填写过程
DateTime afterDT = DateTime.Now;
TimeSpan ts = afterDT.Subtract(beforeDT);
print($"消耗的时间: {ts.TotalMilliseconds}毫秒");
```

``` C#
Stopwatch sw = new Stopwatch();
sw.Start();
//todo这里填写过程
DateTime afterDT = DateTime.Now;
TimeSpan ts = afterDT.Subtract(beforeDT);
sw.Stop();
UnityEngine.Debug.Log($"当前消耗的时间是：{sw.ElapsedMilliseconds}毫秒等于{sw.ElapsedMilliseconds / 1000}秒");
```
