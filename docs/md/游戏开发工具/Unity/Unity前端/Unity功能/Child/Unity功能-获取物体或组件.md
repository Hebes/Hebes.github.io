# Unity功能-获取物体或组件

## 获取不活动组件

```C#
Transform[] gos = go.transform.GetComponentsInChildren<Transform>(true);
```

## 查询

```c#
//不激活也可查找到
transform.GetChild(0);
transform.find("Cube/Cube2");

//只能找到激活的项
GameObject.Find("Cube");
GameObject.FindGameObjectWithTag("Cube");
GameObject.FindGameObjectsWithTag("Cube");
FindObjectOfType<T>();
FindObjectsOfType<T>();

//根据Scene查找场景中所有物体(包括非激活项但是不包括子物体)
using UnityEngine.SceneManagement;
//Scene scene = SceneManager.GetSceneByName(SceneName);//获取对应名称的场景
//Scene scene = SceneManager.GetSceneAt(index);//根据索引获取场景
Scene scene = SceneManager.GetActiveScene();
GameObject[] objs = scene.GetRootGameObjects();
//根据查找到的RootObject遍历子类
foreach(GameObject obj in objs)
{
    //该方法可找到非激活物体
    foreach(Transform child in obj.transform)
    {
        
    }
}

//获取当前场景中所有物体(包括非激活项和子物体,但是效率低,会查找Resources文件夹中所有项)
using system.Linq;
using UnityEditor;

private List<GameObject> GetAllSceneObjectsWithInactive()
{
    var allTransforms = Resources.FindObjectsOfTypeAll(typeof(Transform));
    var previousSelection = Selection.objects;
    Selection.objects = allTransforms.Cast<Transform>()
        .Where(x => x != null)
        .Select(x => x.gameObject)
        .Cast<Object>().ToArray();
    var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
    Selection.objects = previousSelection;
    return selectedTransforms.Select(tr => tr.gameObject).ToList();
}
```
