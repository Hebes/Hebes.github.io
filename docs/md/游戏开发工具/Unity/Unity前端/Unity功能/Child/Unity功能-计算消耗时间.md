# Unity功能-计算消耗时间

## 计算消耗时间

用来计算消耗的时间

``` C#
DateTime beforeDT = DateTime.Now;
//todo这里填写过程
DateTime afterDT = DateTime.Now;
TimeSpan ts = afterDT.Subtract(beforeDT);
print($"消耗的时间: {ts.TotalMilliseconds}ms");
```
