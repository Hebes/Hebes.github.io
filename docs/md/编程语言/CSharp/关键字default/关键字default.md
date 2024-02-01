# 关键字default

## C# 关键字default（默认值表达式）用法简介

https://blog.csdn.net/wcc27857285/article/details/96700760

最新推荐文章于 2023-09-26 04:26:27 发布

![](https://csdnimg.cn/release/blogv2/dist/pc/img/original.png)

[Bird鸟人](https://blog.csdn.net/wcc27857285 "Bird鸟人") ![](https://csdnimg.cn/release/blogv2/dist/pc/img/newCurrentTime2.png) 最新推荐文章于 2023-09-26 04:26:27 发布

版权声明：本文为博主原创文章，遵循 [CC 4.0 BY-SA](http://creativecommons.org/licenses/by-sa/4.0/) 版权协议，转载请附上原文出处链接和本声明。

#### 第一种，用于switch语句中

```cs
switch (num)
{
    case 1:
        Console.WriteLine("1");
        break;
    case 2:
        Console.WriteLine("2");
        break;
    default:
        Console.WriteLine("no match");
    break;
}
```

作用是:不满足所有case的情况下，会进default, 若不写则直接跳出switch

#### 重点是第二种，即各种类型的默认值

如下表：

| 
**类型**

 | 

**默认值**

 |
| --- | --- |
| 

任何引用类型

 | 

`null`

 |
| 

数值类型

 | 

零

 |
| 

bool

 | 

**false**

 |
| 

enum

 | 

表达式 `(E)0` 生成的值，其中 `E` 是枚举标识符。

 |
| 

struct

 | 

通过如下设置生成的值：将所有值类型的字段设置为其默认值，将所有引用类型的字段设置为`null`。

 |
| 

可以为 null 的类型

 | 

HasValue 属性为 `false` 且 Value 属性未定义的实例。

 |

#### 1.主要是用于泛型中

```cs
public T GetData<T>()
{
    return default;
}
```

#### 2.default 在C#7.1中得到了改进，不再需要default（T）了：

#### 变量赋值

```cs
C#7.0
var s = "字符串"; 
s = default(string); 


C#7.1
var s = "字符串"; 
s = default;
```

#### 初始化赋值

```cs
C#7.0
var dstring = default(string); 
var dint = default(int); 
var dintNull = default(int?); 
var d = default(dynamic); 
var dt = default(DateTime); 
var dt1 = default(DateTime?);


C#7.1
string s = default; 
int i = default; 
DateTime? dt = default; 
dynamic d = default;
```

#### 可选参数

```cs
C#7.0
void Test(int a, string b = default(string)) 
{
}


C#7.1
void Test(int a, string b = default) 
{
}
```

在VS中选择了C#7.1版本后 ，会提示优化代码，如下图：

![](https://img-blog.csdnimg.cn/20190721103850527.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3djYzI3ODU3Mjg1,size_16,color_FFFFFF,t_70)