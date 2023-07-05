using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    
    private AudioManager audioManager;
    public AudioManager AudioManager
    {
        get
        {
            return audioManager;
        }
    }

    private UIManager uiManager;
    public UIManager UIManager
    {
        get
        {
            return uiManager;
        }
    }

    private PoolManager poolManager;
    public PoolManager PoolManager
    {
        get
        {
            return poolManager;
        }
    }

    private ResManager resManager;
    public ResManager ResManager
    {
        get
        {
            return resManager;
        }
    }

    private SceneSwitchManager sceneSwitchManager;

    public SceneSwitchManager SceneSwitchManager
    {
        get
        {
            return sceneSwitchManager;
        }
    }

    private GameObjectManager gameObjectManager;

    public GameObjectManager GameObjectManager
    {
        get
        {
            return gameObjectManager;
        }
    }
    
    protected override void OnStart()
    {
        base.OnStart();
        
        if(audioManager==null)
        {
            audioManager = this.gameObject.AddComponent<AudioManager>();
        }
        else
        {
            audioManager = this.gameObject.GetComponent<AudioManager>();
        }

        if (uiManager == null)
        {
            uiManager = this.gameObject.AddComponent<UIManager>();
        }
        else
        {
            uiManager = this.gameObject.GetComponent<UIManager>();
        }

        if(poolManager == null)
        {
            poolManager = this.gameObject.AddComponent<PoolManager>();
        }
        else
        {
            poolManager = this.gameObject.GetComponent<PoolManager>();
        }

        if (resManager == null)
        {
            resManager = this.gameObject.AddComponent<ResManager>();
        }
        else
        {
            resManager = this.gameObject.GetComponent<ResManager>();
        }

        if (sceneSwitchManager == null)
        {
            sceneSwitchManager = this.gameObject.AddComponent<SceneSwitchManager>();
        }
        else
        {
            sceneSwitchManager = this.gameObject.GetComponent<SceneSwitchManager>();
        }
        
        if (gameObjectManager == null)
        {
            gameObjectManager = this.gameObject.AddComponent<GameObjectManager>();
        }
        else
        {
            gameObjectManager = this.gameObject.GetComponent<GameObjectManager>();
        }

        //启动配置表管理器
        ConfigManager.Instance.LoadConfigs();
        CharacterManager.Instance.Init();
    }

    void PlayMusic()
    {
        audioManager.PlayMusic(PathUtil.musicPath + "war_music");
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        //暂停游戏
        Time.timeScale = 0;
        //音乐打开时，暂停音乐
        if(audioManager.isPlayingMusic)
        {
            audioManager.PauseMusic();
        }
    }
    /// <summary>
    /// 继续游戏
    /// </summary>
    public void UnPauseGame()
    {
        Time.timeScale = 1;
        if(audioManager.isPlayingMusic)
        {
            audioManager.UnPauseMusic();
        }
    }


}
