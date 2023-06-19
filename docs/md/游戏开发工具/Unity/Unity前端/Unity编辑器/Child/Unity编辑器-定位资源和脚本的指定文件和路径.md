# Unity编辑器-定位资源和脚本的指定文件和路径

```C#
public class EditorTool
{
    //Alt+R打开资源路径
    [MenuItem("HSJ/快捷方式/打开UI预制路径 &R")]
    static void OpenResourcesUIPanel()
    {
        Selection.activeObject  = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefab/Panel/LoginPanel.prefab");
    }
    //Alt+S打开脚本路径
    [MenuItem("HSJ/快捷方式/打开Panel脚本路径 &S")]
    static void OpenScript()
    {
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/MessageBoxPanel.cs");
    }
    //Alt+S打开指定文件夹路径
    [MenuItem("HSJ/快捷方式/打开工程目录 &O")]
    private static void OpenProjectFolder()
    {
        EditorUtility.RevealInFinder(Application.dataPath);
    }
}
```
