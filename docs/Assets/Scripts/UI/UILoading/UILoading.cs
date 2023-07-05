using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UILoading : MonoBehaviour
{
    /// <summary>
    /// 点击新建存档
    /// </summary>
    public void OnClickNewFile()
    {
        GameManager.Instance.UIManager.ShowWindow<UINewFile>().Init();
    }

    /// <summary>
    /// 点击读取存档
    /// </summary>
    public void OnClickLoadFile()
    {
        GameManager.Instance.UIManager.ShowWindow<UIFileSystem>().Init();
    }

    /// <summary>
    /// 点击退出游戏
    /// </summary>
    public void OnClickExitGame()
    {
        Debug.Log("退出游戏");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
