# Unity编辑器-预制体

[物体对象是否预制体](<https://blog.csdn.net/sinat_34870723/article/details/86607676>)

## 先打开预设判断

```C#
 var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
if (prefabStage == null)
{
    Debug.LogError("先打开预设！！！");
    return;
}
else
{
    if (!objs[0].name.Equals(prefabStage.prefabContentsRoot.name))
    {
        Debug.LogError("请选择对应的预设！！！");
        return;
    }
}
```
