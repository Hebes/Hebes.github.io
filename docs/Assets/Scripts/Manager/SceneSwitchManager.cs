using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理器
/// </summary>
public class SceneSwitchManager : MonoBehaviour
{
    //是否加载完成
    public bool isLoaded=false;

    public Action onSceneLoaded;
    
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    public void LoadScene(string sceneName,Action callBack=null)
    {
        StartCoroutine(LoadSceneAsync(sceneName,callBack));
    }

    IEnumerator LoadSceneAsync(string sceneName,Action callBack)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        operation.completed += OnLoadComplete;
        //显示加载界面
        UISceneSwitch uiSceneSwitch = GameManager.Instance.UIManager.ShowWindow<UISceneSwitch>();
        uiSceneSwitch.Init();
        yield return new WaitForSeconds(1f);
        //更新加载界面进度
        uiSceneSwitch.UpdateProgress(0.4f);
        for (int i = 40; i < 98; i++)
        {
            uiSceneSwitch.UpdateProgress((float)i/100);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        if (operation.progress > 0.98f)
        {
            isLoaded = true;
        }

        operation.allowSceneActivation = true;
        callBack?.Invoke();
    }

    public void OnLoadComplete(AsyncOperation operation)
    {
        if (operation.isDone)
        {
            onSceneLoaded?.Invoke();
        }
    }

}
