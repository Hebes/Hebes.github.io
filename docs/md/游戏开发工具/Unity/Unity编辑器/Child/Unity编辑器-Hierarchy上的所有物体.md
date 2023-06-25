# Unity编辑器-Hierarchy上的所有物体

Unity遍历Hierarchy上的所有物体，包括隐藏物体

代码如下

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyEditorUtils 
{
    [MenuItem("GameTools/遍历Hierarchy")]
    static void GetAllSceneObjectsWithInactive()
    {
        var allGos = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        var previousSelection = Selection.objects;
        Selection.objects = allGos;
        var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
        Selection.objects = previousSelection;
        foreach(var trans in selectedTransforms)
        {
            Debug.Log(trans.name);
        }
    }
}
```

![1](\../Image/Unity编辑器-Hierarchy上的所有物体/1.png)

如果想把预设的资源路径打印出来，可以这样

```C#
var go = PrefabUtility.GetPrefabParent(trans.gameObject);
string path = AssetDatabase.GetAssetPath(go);
Debug.Log(path);
```
