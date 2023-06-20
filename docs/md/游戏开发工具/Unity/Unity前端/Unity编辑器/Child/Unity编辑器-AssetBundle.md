# Unity功能-Assetbundles

## Assetbundles

文本格式:txt,xml    打包后的类型:TextAsset,数据保存在TextAsset的text属性中

二进制格式:bytes    打包后的类型:TextAsset,数据保存在TextAsset的bytes属性中

预制体格式:prefab   打包后的类型:GameObject,加载后还需要调用Instantiate函数实例化才能使用

FBX文件格式:fbx 打包后的类型:添加了Animator或者添加了Animation的GameObject,模型加载后还需要调用Instantiate函数实例化才能添加到场景,只包含动画的FBX文件动画剪辑的获取方法如下:

Mecanim动画:Mecanim中必须制作为预制件进行加载,所以加载后的人物都是配置好的,不存在需要加载Animation Clip的情况.

Legacy动画

```c#
private AniamtionClip LoadAnimationClip(AssetBundle assetBundle, string name)
{
    GameObject obj = assetBundle.Load(name,typeof(GameObject)) as GameObject;
    return obj.animation.clip;
}
```

图片格式: bmp,jpg,png   打包后的类型:Texture2D,Sprite.默认Texture2D,比如使用AssetDatabase.LoadMainAssetAtPath方法加载时就是Texture2D的类型,如果希望打包后是Sprite类型,可以使用下面的方法加载:

```c#
AssetDatabase.LoadAssetAtPath("Assets/Image.png",typeof(Sprite));
//也可以直接加载
AssetBundle.LoadAsset<Sprite>(path);
```

音乐格式:aiff,wav,mp3,ogg

打包后的类型:AudioClip

ScriptableObject格式: Asset, 打包后的类型:ScriptableObject的派生类

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
public class LoadAB : MonoBehaviour
{
    IEnumerator Start()
    {
        string path = "Assets/AssetBundles/cube.ab";

        //第一种加载AB方式 LoadFromMemoryAsync  内存加载
        //1、异步
        //AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path));
        //yield return request;
        //AssetBundle ab = request.assetBundle;
        //2、同步
        //AssetBundle ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(path));

        //第二种加载AB的方式 LoadFromFile       本地加载
        //1、异步
        //AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
        //yield return request;
        //AssetBundle ab = request.assetBundle;
        //2、同步
        //AssetBundle ab = AssetBundle.LoadFromFile(path);

        //第三种加载AB的方式  WWW
        //while (Caching.ready == false)
        //{
        //    yield return null;
        //}
        ////file://  或者 file:///  具体路径
        //WWW www = WWW.LoadFromCacheOrDownload(@"file:///C:\_UnityFile\_LHL\AssetBundleTest\Assets\AssetBundles\cube.ab", 1);
        //WWW www = WWW.LoadFromCacheOrDownload(@"http://localhost/AssetBundles\cube.ab", 1);
        //yield return www;
        //if (string.IsNullOrEmpty(www.error) == false)
        //{
        //    Debug.Log(www.error);
        //    yield break;
        //}
        //AssetBundle ab = www.assetBundle;

        //第四种加载AB的方式 UnityWebRequest
       string url = @"file:///C:\_UnityFile\_LHL\AssetBundleTest\Assets\AssetBundles\cube.ab";
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        //AssetBundle ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

        //使用资源
        //GameObject CubePrefab = ab.LoadAsset<GameObject>("Cube");
        //Instantiate(CubePrefab);

        //manifest 加载依赖资源
        AssetBundle manifestAB = AssetBundle.LoadFromFile("Assets/AssetBundles/AssetBundles");
        AssetBundleManifest manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] str = manifest.GetAllDependencies("cube.ab");
        foreach (string name in str)
        {
            print(name);
            AssetBundle.LoadFromFile("Assets/AssetBundles/" + name);
        }
    }
}
```

## AssetBundle

```c#
//加载AB包
AssetBundle adMain = AssetBundle.LoadFromFile(string path);
GameObject.Instantiate(adMain.LoadAsset)
//卸载本包
adMain.Unload(false);
//卸载所有加载过的包
AssetBundle.UnloadAllAssetBundles(false);
```

## 参考

https://blog.csdn.net/heliocentricism/article/details/109209801

https://blog.csdn.net/weixin_41573444/article/details/82221718?spm=1001.2014.3001.5502

https://blog.csdn.net/yu1368072332/article/details/85794608

**[Unity AssetBundle 解析 （一）AB包介绍与构建](<https://zhuanlan.zhihu.com/p/342694796>)**