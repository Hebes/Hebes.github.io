# Unity编辑器-宏

**[如何通过代码在Unity中设置自定义宏信息](<https://blog.csdn.net/qq_42898629/article/details/86632116>)**

## 打包尽量别输出Log,别拼接字符串

Unity EDITOR_LOG.输出会产生GC.拼接字符串也会产生.开发时用宏log.打包时去掉宏

![1](\../Image/Unity编辑器-宏/1.png)

```C#
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class ModifyScriptEditor : EditorWindow
{
    [MenuItem("Tools/使用宏Log")]
    public static void ShowGUIMacro()
    {
        EditorWindow.GetWindow<ModifyScriptGUI>(false);
    }
}
public class ModifyScriptGUI : EditorWindow
{
    private string mModuleStr = "";
    void OnGUI()
    {
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Box");
        {
            mModuleStr = EditorGUILayout.TextField("模块路径:", mModuleStr);
        }
        ShowExecute();
    }
    private void GetPathForDebugs(string pPath)
    {
        mDicTxts.Clear();mCates.Clear();
        string mProject = Application.dataPath.Replace("/Assets", "");
        if (string.IsNullOrEmpty(pPath)) pPath = "Assets/Scripts";
        var guids = AssetDatabase.FindAssets("t:Script", new string[] { pPath });
        foreach (var item in guids)
        {
            var tPath = mProject + "/" + AssetDatabase.GUIDToAssetPath(item);
            var tTxt = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(item)) as TextAsset;
            if (tTxt.text.Contains("Debug"))
            {
                mDicTxts[tPath] = tTxt.text;
                mCates.Add(tPath);
            }
        }
    }
    private Vector2 mScrollViewCommon;
    private Dictionary<string, string> mDicTxts=new Dictionary<string, string>();
    private List<string> mCates = new List<string>();
    void ShowNotifyLabel(string label)
    {
        this.ShowNotification(new GUIContent(label));
    }
    private void ShowExecute()
    {
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Box");
        {
            GUILayout.Label(@"脚本代码的使用,默认路径(Assets/Script),执行自己跟的模块Log输出,拼接字符串也会产生GC.开发时用宏打印log.打包时去掉宏");
            if (GUILayout.Button("执行", GUILayout.Height(27)))
            {
                GetPathForDebugs(mModuleStr);
            }
            if (mCates != null && mCates.Count > 0)
            {
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical("HelpBox", GUILayout.Height(250));
                {
                    mScrollViewCommon = EditorGUILayout.BeginScrollView(mScrollViewCommon);
                    for (int i = 0; i < mCates.Count; i++)
                    {
                        var tPathKey = mCates[i];
                        if (mDicTxts.ContainsKey(tPathKey) == false) continue;
                        var tValueContent = mDicTxts[tPathKey];
                        var tName = tPathKey.Split('/')[tPathKey.Split('/').Length - 1];
                        if (GUILayout.Button("-点击给引脚本加上宏-," + tName, GUILayout.Height(20)))
                        {
                            var tLines = tValueContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            var sb = new StringBuilder();
                            for (int j = 0;j < tLines.Length; j++)
                            {
                                var tLine = tLines[j];
                                if (tLine.Contains("Debug"))
                                {
                                    sb.AppendLine("#if EDITOR_LOG");
                                    sb.AppendLine(tLine);
                                    sb.AppendLine("#endif");
                                }
                                else
                                {
                                    sb.AppendLine(tLine);
                                }
                            }
                            StreamWriter tReader = new StreamWriter(tPathKey, false);
                            tReader.Write(sb.ToString());
                            tReader.Close();
                            AssetDatabase.SaveAssets();
                            mCates.Remove(tPathKey);
                            ShowNotifyLabel("执行了此脚本");
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                if (GUILayout.Button("若太多,一口气此模块全部加宏", GUILayout.Height(27)))
                {
                    for (int i = 0; i < mCates.Count; i++)
                    { 
                        var tPathKey = mCates[i];
                        if(mDicTxts.ContainsKey(tPathKey)==false)continue;
                        var tValueContent = mDicTxts[tPathKey];
                        var tLines = tValueContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        var sb = new StringBuilder();
                        for (int j = 0; j < tLines.Length; j++)
                        {
                            var tLine = tLines[j];
                            if (tLine.Contains("Debug"))
                            {
                                sb.AppendLine("#if EDITOR_LOG");
                                sb.AppendLine(tLine);
                                sb.AppendLine("#endif");
                            }
                            else
                            {
                                sb.AppendLine(tLine);
                            }
                            StreamWriter tReader = new StreamWriter(tPathKey, false);
                            tReader.Write(sb.ToString());
                            tReader.Close();
                            AssetDatabase.SaveAssets();
                            mDicTxts[tPathKey] = "";
                        }
                    }
                    ShowNotifyLabel("执行了此模块");
                }
            }
        }
    }
}
```

## Unity 代码改宏定义

```C#
PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup); //所有宏定义 ; 分割
PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, sz); //写入全部宏,相当于改配置
```

需要考虑一组宏定义的互斥问题,要保持不改动的宏不变,示例代码如下

```C#
public static void SetDebugerLevle(string logType)
{
    BuildTargetGroup targetGroup = BuildTargetGroup.Android;
    string ori = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
    string debugType = "Debuger_" + logType;
 
    List<string> defineSymbols = new List<string>(ori.Split(';'));
    for (int i = 0; i < defineSymbols.Count; ++i)
    {
        if (defineSymbols[i] == debugType)
        {
            Debug.LogFormat("========== debuglog {0}", logType);
            return;
        }
 
        if (defineSymbols[i].StartsWith("Debuger_"))
        {
            defineSymbols[i] = debugType;
            debugType = null;
            break;
        }
    }
 
    if (debugType != null)
    {
        defineSymbols.Add(debugType);
    }
 
    PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, string.Join(";", defineSymbols.ToArray()));
    Debug.LogFormat("========== debuglog {0}", logType);
}
```
