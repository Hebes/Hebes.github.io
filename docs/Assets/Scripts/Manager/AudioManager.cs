using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource soundAudio;
    public AudioSource musicAudio;

    public bool isPlayingMusic = false;
   

    private void Awake()
    {
        soundAudio = this.transform.Find("SoundAudio").gameObject.GetComponent<AudioSource>();
        musicAudio = this.transform.Find("MusicAudio").gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// 音效音量
    /// </summary>
    public float SoundVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("soundVolume", 1.0f);
        }
        set
        {
            soundAudio.volume = value;
            PlayerPrefs.SetFloat("soundVolume", value);
        }
    }

    /// <summary>
    /// 音乐音量
    /// </summary>
    public float MusicVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("musicVolume", 1.0f);
        }
        set
        {
            musicAudio.volume = value;
            PlayerPrefs.SetFloat("musicVolume", value);
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundName">音效名称</param>
    public void PlaySound(string soundName)
    {
        AudioClip clip=null;
        GameManager.Instance.ResManager.LoadSound(soundName, (obj) =>
        {
            clip = obj as AudioClip;
        });
        if(clip == null)
        {
            Debug.Log("音效加载失败！");
            return;
        }
        if(SoundVolume<0)
        {
            Debug.Log("音效音量过低");
            return;
        }
        soundAudio.PlayOneShot(clip);
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="musicName">音乐名称</param>
    public void PlayMusic(string musicName)
    {
        AudioClip clip = null;
        GameManager.Instance.ResManager.LoadSound(musicName, (obj) =>
        {
            clip = obj as AudioClip;
        });
        if (clip==null)
        {
            Debug.Log("音乐加载失败！");
            return;
        }
        if(MusicVolume<0)
        {
            Debug.Log("音乐音量过低");
            return;
        }
        musicAudio.clip = clip;
        musicAudio.Play();
        isPlayingMusic = true;
    }

    /// <summary>
    /// 关闭音乐
    /// </summary>
    public void StopMusic()
    {
        musicAudio.Stop();
        isPlayingMusic = false;
    }

    public void PauseMusic()
    {
        musicAudio.Pause();
    }

    public void UnPauseMusic()
    {
        musicAudio.UnPause();
    }

    public void SetSoundVolume(float value)
    {
        this.SoundVolume = value;
    }

    public void SetMusicVolume(float value)
    {
        this.MusicVolume = value;
    }
}
