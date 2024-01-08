# Unity后端-定点数运算数学库

![1](\../Image/Unity后端-定点数运算数学库/1.png)

![]()

## 定点数原理

```c#
0.1+0.2=0.3//这个是经过浮点数计算的,会有浮点运算的问题
//下面是定点数计算的,不会有浮点运算的问题,先放大1000倍,然后缩小1000倍
0.1 *1000=100;
0.2 *1000=200;
100+200=300
300/1000=0.3
```

## 正文

**[左移和右移运算符（<< 和 >>）](<https://learn.microsoft.com/zh-cn/cpp/cpp/left-shift-and-right-shift-operators-input-and-output?view=msvc-170>)**

**[位运算符和移位运算符（C# 参考）](<https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators>)**

为什么定义1024-> 放大1000倍基本够用 在2进制里面最相近的是2^10=1024

## 参考网站

**[什么是定点数？](<https://zhuanlan.zhihu.com/p/338588296>)**

**[C# 的固定点类型](<https://blog.csdn.net/jikefzz1095377498/article/details/89001756>)**

**[《帧同步教程一》定点数原理和无损精度的实现方式](<https://blog.csdn.net/qq_42461824/article/details/125609431>)** 我没看

**[帧同步之：定点数(fixedpoint num)原理、运算、实现](<https://blog.csdn.net/ak47007tiger/article/details/106610914>)**
