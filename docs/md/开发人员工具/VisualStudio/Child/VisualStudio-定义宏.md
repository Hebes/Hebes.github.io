# VisualStudio-定义宏

```C#
#define ACTool //定义宏
#if ACTool //使用宏定义  File -> Build Settings -> Player Settings -> Player -> Script Compilation 添加ACTool
#endif
```

```C#
#define COREDUBG

[Conditional("COREDUBG")]
public static void Log(string msg, params object[] args)
{
    //TODO 逻辑
}
```

https://blog.csdn.net/farcor_cn/article/details/113620418
https://www.jianshu.com/p/db11d544cd92
