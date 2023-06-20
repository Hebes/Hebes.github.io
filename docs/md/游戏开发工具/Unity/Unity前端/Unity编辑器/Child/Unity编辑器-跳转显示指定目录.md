# Unity编辑器-跳转显示指定目录

EditorUtility.RevealInFinder(string outputPath);

![1](\../Image/Unity编辑器-跳转显示指定目录/1.gif)

```C#
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ShowSystemFinder {

   [MenuItem("UnityAsk/DoTest")]
   private static void DoTest()
   {
      var outputPath = "configfiles";
      if (!Directory.Exists(outputPath))
      {
         Directory.CreateDirectory(outputPath);
      }

      File.WriteAllText(Path.Combine(outputPath,"level1.txt"),"this is level one");
      
      
      if (EditorUtility.DisplayDialog("UnityAsk的Unity3D小技巧","我完成了","确定"))
      {
         EditorUtility.RevealInFinder(outputPath);
      }
   }
}
```
