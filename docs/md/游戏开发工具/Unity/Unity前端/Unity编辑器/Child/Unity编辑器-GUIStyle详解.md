# Unity编辑器-GUIStyle详解

GUIStyle可以new一个全新的实例，这样，需要自己处理所有自己需要的效果。

GUIStyle还可以基于已经存在的实例new一个新的实例，这样，只需对原有的效果中不符合自己需求的进行修改（省事省力，这类情况是我们最常用的），譬如：

```c#
GUIStyle btnStyle = new GUIStyle("Command");
btnStyle.fontSize = 12;
btnStyle.alignment = TextAnchor.MiddleCenter;
btnStyle.imagePosition = ImagePosition.ImageAbove;
btnStyle.fontStyle = FontStyle.Normal;
btnStyle.fixedWidth = 60;
 
//等同于：
GUIStyle btnStyle_1 = new GUIStyle("Command")
{
    fontSize = 12,
    alignment = TextAnchor.MiddleCenter,
    imagePosition = ImagePosition.ImageAbove,
    fontStyle = FontStyle.Normal,
    fixedWidth = 60
};
```

遍历 GUI.skin.customStyles 可以取到所有的内置GUIStyle

```c#
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public sealed class GUIStyle1 : EditorWindow
    {
        [MenuItem("Tools/EditorGUILayout模板/参考样例3")]
        private static void OpenGUIStyle()
        {
            GetWindow<GUIStyle1>().Show();
        }

        private UnityEngine.GUIStyle[] styles;
        private Vector2 scroll = Vector2.zero;
        private string searchContent = "";

        private void OnGUI()
        {
            if (styles == null)
            {
                styles = GUI.skin.customStyles;
            }
            GUILayout.BeginHorizontal("Toolbar");
            {
                GUILayout.Label("Search:", GUILayout.Width(50));
                searchContent = GUILayout.TextField(searchContent, "SearchTextField");
            }
            GUILayout.EndHorizontal();

            scroll = GUILayout.BeginScrollView(scroll);
            {
                for (int i = 0; i < styles.Length; i++)
                {
                    if (styles[i].name.ToLower().Contains(searchContent.ToLower()))
                    {
                        GUILayout.BeginHorizontal("Badge");
                        {
                            if (GUILayout.Button("拷贝", "LargeButton", GUILayout.Width(40f)))
                            {
                                EditorGUIUtility.systemCopyBuffer = styles[i].name;
                                UnityEngine.Debug.Log($"拷贝名称: {styles[i].name}");
                            }
                            EditorGUILayout.SelectableLabel(styles[i].name, GUILayout.Width((position.width - 40f) * 0.3f));
                            GUILayout.Button(string.Empty, styles[i], GUILayout.Width((position.width - 40f) * 0.6f));
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUILayout.EndScrollView();
        }
    }
}
```
