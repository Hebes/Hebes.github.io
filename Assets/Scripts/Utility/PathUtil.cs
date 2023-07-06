using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil
{
    //bundle输出目录
    public static string bundlePath = Application.streamingAssetsPath;
    //bundle资源根目录
    public static string buildResourcesPath = Application.dataPath + "/Bundles/";
    //配置文件资源路径
    public static string configPath = Application.dataPath + "/Bundles/Configs/";
    //本地数据文件资源路径
    public static string playerDataPath = Application.dataPath + "/Bundles/Datas/";
    //UI资源路径
    public static string uiPath = "Bundles/Prefabs/UI/";
    //声音资源路径
    public static string soundPath = "Bundles/Audios/Sound/";
    public static string musicPath = "Bunldes/Audios/Music/";
    //Effect资源路径
    public static string effectPath = "Bundles/Prefabs/Effect/";

    //Sprite资源路径
    public static string spritePath = "Bundles/Sprite/s";
    
    /// <summary>
    /// 获取文件的标准路径
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    public static string GetStandardPath(string path)
    {
        if(string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }
        return path.Trim().Replace("\\", "/");
    }

    /// <summary>
    /// 获取unity的相对路径
    /// </summary>
    /// <param name="path">绝对路径</param>
    /// <returns></returns>
    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }
        return path.Substring(path.IndexOf("Assets"));
    }

    /*
    public static string GetLuaPath(string name)
    {
        return string.Format("Assets/BuildResources/LuaScripts/{0}.bytes", name);
    }
    */
    public static string GetUIPath(string name)
    {
        return string.Format("Assets/Bundles/Prefabs/UI/{0}/{1}.prefab", name,name);
    }
    public static string GetMusicPath(string name)
    {
        return string.Format("Assets/Bundles/Audio/Music/{0}", name);
    }
    public static string GetSoundPath(string name)
    {
        return string.Format("Assets/Bundles/Audio/Sound/{0}", name);
    }
    public static string GetEffectPath(string name)
    {
        return string.Format("Assets/Bundles/Prefabs/Effect/{0}.prefab", name);
    }
    public static string GetModelPath(string name)
    {
        return string.Format("Assets/Bundles/Prefabs/Model/{0}.prefab", name);
    }
    public static string GetSpritePath(string name)
    {
        return string.Format("Assets/Bundles/Sprites/{0}.png", name);
    }
    public static string GetScenePath(string name)
    {
        return string.Format("Assets/Bundles/Scenes/{0}.unity", name);
    }
    
}

