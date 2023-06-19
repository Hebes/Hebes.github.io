# Unity编辑器-设置AB包标签

```C#
private const string CodeDir = "Assets/AssetsPackage/Code/";
// 设置ab包
AssetImporter assetImporter1 = AssetImporter.GetAtPath($"{CodeDir}/Code.dll.bytes");
assetImporter1.assetBundleName = "Code.unity3d";
AssetImporter assetImporter2 = AssetImporter.GetAtPath($"{CodeDir}/Code.pdb.bytes");
assetImporter2.assetBundleName = "Code.unity3d";
```

```C#
/// <summary>
/// 设置单个资源的ABName
/// </summary>
/// <param name="abName"></param>
/// <param name="path">资源路径</param>
public static void ACSetABName(this string path, string abName)
{
    AssetImporter ai = AssetImporter.GetAtPath(path);
    if (ai != null)
        ai.assetBundleName = abName;
}
```
