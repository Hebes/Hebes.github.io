# Unity编辑器-丢失脚本

## unity 代码批量修改Remove Missing Script和批量修改指定组件的内容

昨天在项目开发中，发现一个问题，就是一些脚本已经被废弃，但是这些废弃脚本还是被绑定在某些预制体中，这时候运行就会产生很多Missing Script的警告信息，这些警告虽不影响代码的实际运行，但是一个大项目肯定不能出现的N多的警告信息，并且这里通过手动去找肯定不现实，所以这里我们就同一个脚本去实现自动去遍历所有的prefab然后移除Missing的组件，下面是具体的代码信息。

1.首先在工程中创建一个Editro文件夹，将脚本放在Editor文件夹下

2.通过选中文件，通过编辑器实现遍历该文件夹下的所有prefab:

```cs
    [MenuItem("工具/对象移除丢失脚本")]
    static void GetAllGo()
    {
        //寻找你选中文件夹下的所有文件
        object[] obj = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
        for (int i = 0; i < obj.Length; i++)
        {
            //获取文件的后缀信息
            string ext = System.IO.Path.GetExtension(obj[i].ToString());
            //这里筛选出不是预制体的物体
            if (!ext.Contains(".GameObject"))
            {
                continue;
            }

            GameObject go = (GameObject)obj[i];

            // 移除子物体下的脚本丢失
            foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
            {
                CleanMissingScript(trans.gameObject, go);
            }
        }
    }
```

3.找到prefab,去遍历她的所有子物体，并实现直接移除相应的空组件：

```cs
    static void CleanMissingScript(GameObject go, GameObject parentGO)
    {
        var components = go.GetComponents<Component>();
        var serializedObject = new SerializedObject(go);
        var prop = serializedObject.FindProperty("m_Component");
        int r = 0;
        for (int j = 0; j < components.Length; j++)
        {
            if (components[j] == null)
            {
                prop.DeleteArrayElementAtIndex(j - r);
                MyDebug.Log("成功移除丢失脚本，gameObject name: " + go.name + " ---父类prefab name：" + parentGO.name);
                r++;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
```

4.如果还要做其他操作，比如我们写了一个扩展的button脚本，这时候我们不可能通过手动去把所有之前的button换成新的butonPlus，所以这时候就需要去代码自动实现：

```cs
    static void CleanMissingScript(GameObject go, GameObject parentGO)
    {
        var components = go.GetComponents<Component>();

        //如果当前组件有按钮
        if (go.GetComponent<UnityEngine.UI.Button>() != null)
        {
            Button btn = go.GetAddComponent<Button>();
            //copy之前的组件信息
            UnityEditorInternal.ComponentUtility.CopyComponent(btn);
            //移除组件信息
            GameObject.DestroyImmediate(btn);
            //TODO:添加新的组件信息

        }       
        serializedObject.ApplyModifiedProperties();
    }
```

5.下面是完整脚本：

```cs
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 移除项目中go上丢失的脚本
/// </summary>
public class RemoveMissingScript
{
    [MenuItem("SlgTool/程序工具/对象移除丢失脚本")]
    static void GetAllGo()
    {
        object[] obj = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
        for (int i = 0; i < obj.Length; i++)
        {
            string ext = System.IO.Path.GetExtension(obj[i].ToString());
            Debug.Log("name:" + ext);
            if (!ext.Contains(".GameObject"))
            {
                continue;
            }

            GameObject go = (GameObject)obj[i];

            // 移除子物体下的脚本丢失
            foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
            {
                CleanMissingScript(trans.gameObject, go);
            }
        }

    }

    static void CleanMissingScript(GameObject go, GameObject parentGO)
    {
        var components = go.GetComponents<Component>();   
        var serializedObject = new SerializedObject(go);
        var prop = serializedObject.FindProperty("m_Component");
        int r = 0;
        for (int j = 0; j < components.Length; j++)
        {
            if (components[j] == null)
            {
                prop.DeleteArrayElementAtIndex(j - r);
                MyDebug.Log("成功移除丢失脚本，gameObject name: " + go.name + " ---父类prefab name：" + parentGO.name);
                r++;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }

}
```

这里就是一个完整的代码自动查找prefab下的空组件，然后移除的代码。
