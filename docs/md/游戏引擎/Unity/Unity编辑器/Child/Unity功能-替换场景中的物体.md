# Unity功能-替换场景中的物体

实现场景中批量替换预制体的编辑器功能。
**关键:**
PrefabUtility.InstantiatePrefab 将预设实例化到场景中，与GameObject.Instantiate不同，它是以预设实例的。
![1](\../Image/Unity功能-映射3D坐标到UI/2.png)
**使用方法:**
在Project中拖拽预设到工具中
打开需要操作的场景，可以多选需要替换的物体，或者输入需要替换的名字
修改完成后保存场景

``` C#
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
 
public class ObjectReplaceEditorWindow : EditorWindow
{
    [MenuItem("Tools/替换场景中的物体")]
    public static void Open()
    {
        EditorWindow.GetWindow(typeof(ObjectReplaceEditorWindow));
    }
    public GameObject newPrefab;
    static GameObject tonewPrefab;
    private string replaceName = "";
    private bool isChangeName = false;
    public class ReplaceData
    {
        public GameObject old;
        public GameObject replace;
        public int index = 0;
    }
    void OnGUI()
    {
        EditorGUILayout.LabelField("选择一个新的物体");
        newPrefab = (GameObject) EditorGUILayout.ObjectField(newPrefab, typeof(GameObject),true, GUILayout.MinWidth(100f));
        tonewPrefab = newPrefab;
        //isChangeName = EditorGUILayout.ToggleLeft("不改变场景中物体的名字", isChangeName);
        if (GUILayout.Button("替换选中的物体"))
        {
            ReplaceObjects();
        }
        EditorGUILayout.LabelField("----------------------");
        replaceName = EditorGUILayout.TextField("需要替换的物体名字", replaceName);
        if (GUILayout.Button("替换相同名字的"))
        {
            ReplaceObjectsByName(replaceName, false);
        }
        if (GUILayout.Button("替换包含名字的 慎用"))
        {
            ReplaceObjectsByName(replaceName, true);
        }
        EditorGUILayout.LabelField("----------------------");
        if (GUILayout.Button("保存修改"))
        {
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }
    void ReplaceObjects()
    {
        if (tonewPrefab == null) return;
        Object[] objects = Selection.objects;
        List<ReplaceData> replaceDatas = new List<ReplaceData>();
        foreach (Object item in objects)
        {
            GameObject temp = (GameObject)item;
            ReplaceData replaceData = new ReplaceData();
            replaceData.old = temp;
            ReplaceOne(replaceData);
            replaceDatas.Add(replaceData);
        }
        HandleReplaceData(replaceDatas);
    }
    void HandleReplaceData(List<ReplaceData> replaceDatas)
    {
        foreach (var replaceData in replaceDatas)
        {
            replaceData.replace.transform.SetSiblingIndex(replaceData.index);
            if(null != replaceData.old && null != replaceData.old.gameObject)
                DestroyImmediate(replaceData.old.gameObject);
        }
    }
    void ReplaceObjectsByName(string name, bool isContain)
    {
        if (string.IsNullOrEmpty(name)) return;
        List<ReplaceData> replaceDatas = new List<ReplaceData>();
        Transform[] all = Object.FindObjectsOfType<Transform>();
        foreach (var item in all)
        {
            //Debug.LogError(item.name);
            ReplaceData replaceData = new ReplaceData();
            replaceData.old = item.gameObject;
            if (!isContain && item.gameObject.name == name)
            {
                ReplaceOne(replaceData);
                replaceDatas.Add(replaceData);
            }
            else if (isContain && item.gameObject.name.Contains(name))
            {
                ReplaceOne(replaceData);
                replaceDatas.Add(replaceData);
            }
        }
        HandleReplaceData(replaceDatas);
    }
 
    public void ReplaceOne(ReplaceData replaceData)
    {
        GameObject replace = (GameObject)PrefabUtility.InstantiatePrefab(tonewPrefab);
        replace.transform.SetParent(replaceData.old.transform.parent);
        replace.transform.localPosition = replaceData.old.transform.localPosition;
        replace.transform.localRotation = replaceData.old.transform.localRotation;
        replace.transform.localScale = replaceData.old.transform.localScale;
        replaceData.replace = replace;
        replaceData.index = replaceData.old.transform.GetSiblingIndex();
    }
}
```
