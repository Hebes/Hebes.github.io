# 遍历所有scene

自己写的编辑器扩展中要操作scene中的GameObject，用Selection.objects需要手动选择比较麻烦，于是找到了如下方法自动获取所有场景中的对象。需要注意要把操作的场景放到build setting里。

```CSharp
  foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            //在built setting中被勾选的scene            
            if (S.enabled)
            {
                //得到场景的名称
                string name = S.path;

                //打开这个场景
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(name);

                // 遍历场景中的GameObject
                foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
                {
                    Debug.Log(obj.name);
                }
            }
        }
```

注意FindObjectsOfTypeAll虽然会找到所有对象，但是也会找到场景中没有的东西，而且会出现重复查找的情况，最好是指定一下类型，比如Resources.FindObjectsOfTypeAll(typeof(Text))。
