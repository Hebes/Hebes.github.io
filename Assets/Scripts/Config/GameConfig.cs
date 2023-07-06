using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏全局配置
/// </summary>
public class GameConfig
{
    /// <summary>
    /// 游戏模式
    /// </summary>
    public static GameMode gameMode=GameMode.EditorMode;

    public static bool isOpenLog=true;

    /// <summary>
    /// 资源Bundle清单
    /// </summary>
    public static string FileListName = "AssetBundleList.txt";

    public static readonly string AppVersion = "v1.0.0";

    //热更资源地址
    //public const string ResourcesUrl = "http://127.0.0.1/AssetBundles";
}

/// <summary>
/// 游戏事件
/// </summary>
public enum GameEvent
{
    GameInit=10000,
    StartLua,
}

/// <summary>
/// 游戏模式
/// </summary>
public enum GameMode
{
    /// <summary>
    /// 编辑器模式
    /// </summary>
    EditorMode,
    /// <summary>
    /// 包内
    /// </summary>
    PackageBundle,
    /// <summary>
    /// 热更新模式
    /// </summary>
    UpdateMode,
}