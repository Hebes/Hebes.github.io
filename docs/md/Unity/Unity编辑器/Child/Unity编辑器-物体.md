# Unity编辑器-物体

## 获取资源文件夹下所有物体

```C#
/// <summary>
/// 获取所有物体
/// </summary>
/// <returns></returns>
public static List<UnityEngine.Object>ACGetResourcesAllObject()
{
    return Resources.FindObjectsOfTypeAll<UnityEngine.Object>().ToList();
}
```

![1](\../Image/Unity编辑器-物体/1.png)

## 打印预设的资源路径

```C#
var go = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);//来源对应对象
string path = AssetDatabase.GetAssetPath(go);
Debug.Log(path);
```

## Unity遍历Hierarchy上的所有物体，包括隐藏物体

```C#
private void OnGUI()
{
    if (GUILayout.Button("测试"))
    {
        var allGos = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        var previousSelection = Selection.objects;
        Selection.objects = allGos;
        var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
        Selection.objects = previousSelection;
        foreach (var trans in selectedTransforms)
        {
            Debug.Log(trans.name);
        }
    }
}
```

![2](\../Image/Unity编辑器-物体/2.png)

## 获取场景的中的带有Button组件物体

```C#
//获取场景中所有物体，不包括unity默认依赖的，但并没有显示在Hierarchy面板内的物体。
var all = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
foreach (var item in all)
{
   if (item.scene.isLoaded && item.GetComponent<Button>())
       Debug.Log(item.name);
}
```

## Unity 遍历场景所有物体（包括隐藏及被禁用的物体）

```C#
//用于获取所有Hierarchy中的物体，包括被禁用的物体
private List<GameObject> GetAllSceneObjectsWithInactive()
{
    var allTransforms = Resources.FindObjectsOfTypeAll(typeof(Transform));
    var previousSelection = Selection.objects;
    Selection.objects = allTransforms.Cast<Transform>()
        .Where(x => x != null)
        .Select(x => x.gameObject)
        //如果你只想获取所有在Hierarchy中被禁用的物体，反注释下面代码
        //.Where(x => x != null && !x.activeInHierarchy)
        .Cast<UnityEngine.Object>().ToArray();

    var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
    Selection.objects = previousSelection;

    return selectedTransforms.Select(tr => tr.gameObject).ToList();
}
```
