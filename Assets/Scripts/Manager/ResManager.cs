using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 资源管理器
/// </summary>
public class ResManager : MonoBehaviour
{
    /// <summary>
    /// bundle信息
    /// </summary>
    public class BundleInfo
    {
        public string assetName;
        public string bundleName;
        public List<string> dependence;
    }

    /// <summary>
    /// Bundle数据
    /// </summary>
    public class BundleData
    {
        public AssetBundle bundle;
        //Bundle引用数量
        public int count;
        public BundleData(AssetBundle ab)
        {
            bundle = ab;
            count = 1;
        }
    }
    //存储bundle信息集合
    public Dictionary<string, BundleInfo> m_BundleInfos = new Dictionary<string, BundleInfo>();
    //存储bundle数据的集合
    public Dictionary<string, BundleData> m_Bundles = new Dictionary<string, BundleData>();

    /// <summary>
    /// 解析版本文件
    /// </summary>
    public void ParseVersionFile()
    {
        //获取版本文件路径
        string url = Path.Combine(PathUtil.bundlePath, GameConfig.FileListName);

        //版本文件信息
        string[] data = File.ReadAllLines(url);
        //解析版本信息
        for (int i = 0; i < data.Length; i++)
        {
            BundleInfo bundleInfo = new BundleInfo();
            string[] info = data[i].Split('|');
            bundleInfo.assetName = info[0];
            bundleInfo.bundleName = info[1];
            bundleInfo.dependence = new List<string>(info.Length - 2);
            for (int j = 2; j < info.Length; j++)
            {
                bundleInfo.dependence.Add(info[j]);
            }
            m_BundleInfos.Add(bundleInfo.assetName, bundleInfo);
        }
    }

    /// <summary>
    /// 异步加载Bundle
    /// </summary>
    /// <param name="assetName">��Դ��</param>
    /// <param name="action">���ػص�</param>
    /// <returns></returns>
    IEnumerator LoadBundleAsync(string assetName, Action<UnityEngine.Object> action = null)
    {
        string bundleName = m_BundleInfos[assetName].bundleName;
        string bundlePath = Path.Combine(PathUtil.buildResourcesPath, bundleName);
        List<string> dependence = m_BundleInfos[assetName].dependence;
        //获取Bundle
        BundleData bundle = GetBundle(bundleName);
        if(bundle == null)
        {
            UnityEngine.Object obj = GameManager.Instance.PoolManager.Get("AssetBundle", bundleName);
            if(obj != null)
            {
                AssetBundle ab = obj as AssetBundle;
                bundle = new BundleData(ab);
            }
            else
            {
                AssetBundleCreateRequest bundleCreateRequest = AssetBundle.LoadFromFileAsync(bundlePath); 
                yield return bundleCreateRequest;
                bundle = new BundleData(bundleCreateRequest.assetBundle);
            }
            m_Bundles.Add(bundleName, bundle);
        }
        //获取依赖
        if(dependence!=null && dependence.Count>0)
        {
            for(int i=0;i<dependence.Count;i++)
            {
                //加载bundle
                StartCoroutine(LoadBundleAsync(dependence[i]));
            }
        }
        //剔除场景
        if(assetName.EndsWith(".unity"))
        {
            action?.Invoke(null);
            yield break;
        }
        if(action==null)
        {
            yield break;
        }

        //加载bundle
        AssetBundleRequest bundleRequest = bundle.bundle.LoadAssetAsync(assetName);
        yield return bundleRequest;
        //执行回调
        if(action!=null && bundleRequest.asset!=null)
        {
            action.Invoke(bundleRequest.asset);
        }
        Debug.Log("this is LoadBundleAsync");
    }

    /// <summary>
    /// 获取BundleData
    /// </summary>
    /// <param name="bundleName">bundle����</param>
    /// <returns></returns>
    private BundleData GetBundle(string bundleName)
    {
        BundleData bundle = null;
        if(m_Bundles.TryGetValue(bundleName,out bundle))
        {
            bundle.count++;
        }
        return bundle;
    }



    public void UnLoadBundle(UnityEngine.Object obj)
    {
        AssetBundle ab = obj as AssetBundle;
        ab.Unload(true);
    }

    /// <summary>
    /// 减少bundle的引用数量
    /// </summary>
    /// <param name="bundleName"></param>
    public void MinusBundleCount(string assetName)
    {

        string bundleName = m_BundleInfos[assetName].bundleName;
        MinuOneBundleCount(bundleName);

        List<string> dependence = m_BundleInfos[assetName].dependence;
        if(dependence!=null && dependence.Count > 0)
        {
            for (int i = 0; i < dependence.Count; i++)
            {
                string name = m_BundleInfos[dependence[i]].bundleName;
                MinuOneBundleCount(name);
            }
        }
    }
    /// <summary>
    /// 减少一次bundle引用
    /// </summary>
    /// <param name="bundleName"></param>
    private void MinuOneBundleCount(string bundleName)
    {
        if (m_Bundles.TryGetValue(bundleName,out BundleData bundle))
        {
            bundle.count--;
            Debug.Log("bundle�����ô�����" + bundleName + "Count:" + bundle.count);
        }
        if(bundle.count<=0)
        {
            Debug.Log($"{bundleName} ��������Ϊ0 �����ս������");
            GameManager.Instance.PoolManager.Set("AssetBundle", bundleName, bundle.bundle);
            m_Bundles.Remove(bundleName);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// 编辑器模式下加载资源
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="callBack"></param>
    private void EditorLoadAsset(string assetName,Action<UnityEngine.Object> callBack)
    {
        Debug.Log("This is EditorLoadAsset");
        UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(assetName, typeof(UnityEngine.Object));
        if (obj == null)
        {
            Debug.LogError("assets name is not exits:" + assetName);
            return;
        }
        callBack?.Invoke(obj);
    }
#endif

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="assetName">��Դ��</param>
    /// <param name="callBack">�ص�</param>
    private void LoadAsset(string assetName,Action<UnityEngine.Object> callBack)
    {
#if UNITY_EDITOR
        if (GameConfig.gameMode==GameMode.EditorMode)
        {
            EditorLoadAsset(assetName,callBack);
        }
        else
#endif
        {
            StartCoroutine(LoadBundleAsync(assetName,callBack));
        }
    }

    //加载指定资源
    public void LoadUI(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetUIPath(assetName), action);
    }
    public void LoadMusic(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetMusicPath(assetName), action);
    }
    public void LoadSound(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetSoundPath(assetName), action);
    }
    public void LoadEffect(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetEffectPath(assetName), action);
    }

    public void LoadScene(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetScenePath(assetName), action);
    }

    /*
    public void LoadLua(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(assetName, action);
    }
    */

    public void LoadPrefab(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetModelPath(assetName), action);
    }

    public void LoadSprite(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetSpritePath(assetName), action);
    }
    
    
    //Todo:卸载

}
