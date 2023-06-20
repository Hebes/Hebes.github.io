# Unity编辑器-保存

```C#
//提交修改，如果没有这个代码，unity不会察觉到编辑器有改动，同时改动也不会被保存
Undo.RecordObject(t, t.gameObject.name);
```

## 保存数据

```C#
string path = AssetDatabase.GetAssetPath(objs[0]);
newName = levelName + "_" + country.ToString();//新名称
AssetDatabase.RenameAsset(path, newName);
AssetDatabase.SaveAssets();
AssetDatabase.Refresh();
```
