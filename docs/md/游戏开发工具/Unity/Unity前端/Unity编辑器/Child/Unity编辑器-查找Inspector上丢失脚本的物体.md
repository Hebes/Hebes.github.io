# Unity编辑器-查找Inspector上丢失脚本的物体

有时因为各种原因，某些物体的Inspector上显示脚本丢失。如下图：

![1](\../Image/Unity编辑器-Inspector上丢失脚本的物体/1.png)

通过下面的脚本，我们可以找出场景中哪些物体丢失了脚本。

```C#
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
 
public class SelectGameObjectsWithMissingScripts : Editor
{
    [MenuItem("Tools/Select GameObjects With Missing Scripts")]
    static void SelectGameObjects()
    {
        //Get the current scene and all top-level GameObjects in the scene hierarchy
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();
 
        List<Object> objectsWithDeadLinks = new List<Object>();
        foreach (GameObject g in rootObjects)
        {
            var trans = g.GetComponentsInChildren<Transform>();
            foreach (Transform tran in trans)
            {
                Component[] components = tran.GetComponents<Component>();
                for (int i = 0; i < components.Length; i++)
                {
                    Component currentComponent = components[i]          
                    //If the component is null, that means it's a missing script!
                    if (currentComponent == null)
                    {
                        //Add the sinner to our naughty-list
                        objectsWithDeadLinks.Add(tran.gameObject);
                        Selection.activeGameObject = tran.gameObject;
                        Debug.Log(tran.gameObject + " has a missing script!"); //Console中输出
                        break;
                    }
                }
            }
            //Get all components on the GameObject, then loop through them 

        }
        if (objectsWithDeadLinks.Count > 0)
        {
            //Set the selection in the editor
            Selection.objects = objectsWithDeadLinks.ToArray();
        }
        else
        {
            Debug.Log("No GameObjects in '" + currentScene.name + "' have missing scripts! Yay!");
        }
    }
}
```

将上面的代码复制到SelectGameObjectsWithMissingScripts.cs中。
然后点击菜单栏上的Tool->Select GameObjects With Missing Scripts 项 即可在console中输出所有丢失脚本的物体名称。

实现原理：
获得当前场景Hierarchy窗口中最顶层的物体，然后通过GetComponentsInChildren() 获得所有自身及所有子物体。
遍历这些子物体，通过
Component[] components = tran.GetComponents();
获得子物体上的所有Component，如果某个Component为null,则说明该物体上有丢失脚本。

## 参考

[Unity：一键移除所有预制体上的Missing脚本](<https://blog.csdn.net/qq_35207836/article/details/100165414>)
[Unity移除丢失的脚本](<https://blog.csdn.net/SendSI/article/details/114369256>)
