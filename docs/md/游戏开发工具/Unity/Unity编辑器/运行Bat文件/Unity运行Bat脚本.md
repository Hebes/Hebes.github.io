# Unity运行Bat脚本

**[unity C# 调用 bat](<https://zhuanlan.zhihu.com/p/389593020>)**

**[Unity C#执行bat脚本的操作](<https://blog.csdn.net/chinaherolts2008/article/details/122616841>)**

```CSharp
public class TestRunBat
 {
  [MenuItem("xue/测试TestBat")]
  public static void runTestBat() {
   string cmd = "/c test.bat /path:\"{0}\" /closeonend 2";
   var path = Application.dataPath + "/../";
   cmd = string.Format(cmd, path);
   UnityEngine.Debug.LogError(cmd);
   ProcessStartInfo proc = new ProcessStartInfo("cmd.exe", cmd);
   proc.WindowStyle = ProcessWindowStyle.Normal;
   Process.Start(proc);
  }
 }
```
