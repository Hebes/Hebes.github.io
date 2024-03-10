# TimeSpan

https://www.cnblogs.com/jack-jiang0/p/17865052.html

# [C#中TimeSpan和DateTime的用法详解](https://www.cnblogs.com/jack-jiang0/p/17865052.html)

在C#编程中，`TimeSpan`和`DateTime`是常用的日期和时间处理类。它们提供了丰富的方法和属性，方便我们对日期和时间进行操作和格式化。本篇博客将详细介绍`TimeSpan`和`DateTime`的用法。

## TimeSpan

`TimeSpan`类用于表示一段时间间隔，可以表示从几天到几个纳秒的时间。下面是`TimeSpan`类的常用属性和方法：

### 属性

+   `Days`：获取或设置时间间隔中的天数部分。
+   `Hours`：获取或设置时间间隔中的小时部分。
+   `Minutes`：获取或设置时间间隔中的分钟部分。
+   `Seconds`：获取或设置时间间隔中的秒部分。
+   `Milliseconds`：获取或设置时间间隔中的毫秒部分。
+   `TotalDays`：获取时间间隔的总天数。
+   `TotalHours`：获取时间间隔的总小时数。
+   `TotalMinutes`：获取时间间隔的总分钟数。
+   `TotalSeconds`：获取时间间隔的总秒数。
+   `TotalMilliseconds`：获取时间间隔的总毫秒数。

### 构造函数

+   `TimeSpan()`：初始化一个时间间隔为零的`TimeSpan`对象。
+   `TimeSpan(int hours, int minutes, int seconds)`：使用指定的小时、分钟和秒初始化一个`TimeSpan`对象。
+   `TimeSpan(int days, int hours, int minutes, int seconds)`：使用指定的天数、小时、分钟和秒初始化一个`TimeSpan`对象。

### 方法

+   `Add(TimeSpan ts)`：将指定的时间间隔添加到当前时间间隔。
+   `Subtract(TimeSpan ts)`：从当前时间间隔中减去指定的时间间隔。
+   `ToString()`：将时间间隔转换为字符串表示形式。

下面是一些示例代码：

```csharp
// 创建一个时间间隔为2天的TimeSpan对象
TimeSpan ts1 = new TimeSpan(2, 0, 0, 0);
Console.WriteLine(ts1.TotalDays);  // 输出：2

// 创建一个时间间隔为1小时30分钟的TimeSpan对象
TimeSpan ts2 = new TimeSpan(0, 1, 30, 0);
Console.WriteLine(ts2.TotalMinutes);  // 输出：90

// 将两个时间间隔相加
TimeSpan sum = ts1.Add(ts2);
Console.WriteLine(sum.TotalHours);  // 输出：50

// 将一个时间间隔从另一个时间间隔中减去
TimeSpan diff = ts1.Subtract(ts2);
Console.WriteLine(diff.TotalMinutes);  // 输出：1410

// 将时间间隔转换为字符串表示
Console.WriteLine(ts1.ToString());  // 输出：2.00:00:00
```

## DateTime

`DateTime`类用于表示日期和时间，并提供了各种方法和属性用于日期和时间的计算、格式化和比较。下面是`DateTime`类的常用属性和方法：

### 属性

+   `Date`：获取日期部分。
+   `TimeOfDay`：获取时间部分。
+   `DayOfWeek`：获取星期几。
+   `Day`：获取当前日期的天数。
+   `Month`：获取当前日期的月份。
+   `Year`：获取当前日期的年份。
+   `Hour`：获取当前时间的小时数。
+   `Minute`：获取当前时间的分钟数。
+   `Second`：获取当前时间的秒数。
+   `Millisecond`：获取当前时间的毫秒数。
+   `Now`：获取当前日期和时间。
+   `Today`：获取当前日期。

### 构造函数

+   `DateTime()`：初始化一个`DateTime`对象，表示当前日期和时间。
+   `DateTime(int year, int month, int day)`：使用指定的年、月、日初始化一个`DateTime`对象。
+   `DateTime(int year, int month, int day, int hour, int minute, int second)`：使用指定的年、月、日、小时、分钟和秒初始化一个`DateTime`对象。

### 方法

+   `Add(TimeSpan value)`：将指定的时间间隔加到当前`DateTime`对象。
+   `Subtract(TimeSpan value)`：从当前`DateTime`对象减去指定的时间间隔。
+   `ToString()`：将`DateTime`对象转换为字符串表示形式。

下面是一些示例代码：

```csharp
// 获取当前日期和时间
DateTime now = DateTime.Now;
Console.WriteLine(now);  // 输出：2022/10/10 09:30:00

// 获取当前日期
DateTime today = DateTime.Today;
Console.WriteLine(today);  // 输出：2022/10/10 00:00:00

// 创建一个指定日期和时间的DateTime对象
DateTime dt1 = new DateTime(2022, 10, 10, 8, 30, 0);
Console.WriteLine(dt1.DayOfWeek);  // 输出：Monday

// 将时间间隔加到当前日期和时间
TimeSpan ts = new TimeSpan(1, 0, 0, 0);
DateTime dt2 = now.Add(ts);
Console.WriteLine(dt2);  // 输出：2022/10/11 09:30:00

// 从当前日期和时间减去时间间隔
DateTime dt3 = now.Subtract(ts);
Console.WriteLine(dt3);  // 输出：2022/10/09 09:30:00

// 将DateTime对象转换为字符串表示
Console.WriteLine(now.ToString("yyyy-MM-dd HH:mm:ss"));  // 输出：2022-10-10 09:30:00
```

以上就是`TimeSpan`和`DateTime`类的用法详解。通过使用这两个类，我们可以方便地处理日期和时间，并进行各种操作和格式化。

参考资料：

+   [Microsoft Docs: TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan?view=net-6.0)
+   [Microsoft Docs: DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime?view=net-6.0)

posted @ 2023-11-29 15:49  [Jack-sparrow](https://www.cnblogs.com/jack-jiang0)  阅读(291)  评论(0)  [编辑](https://i.cnblogs.com/EditPosts.aspx?postid=17865052)  [收藏](javascript:void(0))  [举报](javascript:void(0))
