# Unity编辑器-创建脚本头部注释方法

在项目中创建名字为 NewScriptTemplates 的新脚本,将如下代码拷贝进去就ok了。然后再去创建新的脚本查看是否有头部注释了。

```C#
using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEditor;
public class NewScriptTemplates : UnityEditor.AssetModificationProcessor
{
    // 添加脚本注释模板
    private static string noteStr =
        "// ==========================================================\n"
        + "// Description：\n"
        + "// Autor：#Autor# \n"
        + "// CreateTime：#CreateTime# \n"
        + "// 版 本：v #Version# \n"
        + "// ========================================================\n";

    // 创建资源调用
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string allText = noteStr ;
            allText += File.ReadAllText(path);
            // 替换字符串为系统时间
            allText = allText.Replace("#CreateTime#",DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            //替换版本为当前项目版本
            allText = allText.Replace("#Version#", Application.version);
            //替换用户为当前系统用户
            allText = allText.Replace("#Autor#", Environment.UserName);
            File.WriteAllText(path, allText);
            AssetDatabase.Refresh();
        }
    }
}
```

## 来源

**[Unity简单粗暴式——创建脚本头部注释方法](<https://blog.csdn.net/weixin_43159569/article/details/118676543>)**
