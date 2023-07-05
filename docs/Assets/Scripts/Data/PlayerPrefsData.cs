using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 本地存储数据
/// </summary>
public class PlayerPrefsData : MonoBehaviour
{
    #region 角色数据关键字
    
    public const string CharacterExp = "CharacterExp";

    public const string CharacterLevel = "CharacterLevel";

    public const string CharacterClass = "CharacterClass";

    public const string CharacterName = "CharacterName";

    public const string CharacterId = "CharacterId";
    
    #endregion
    
    public static void SaveInt(string key,int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }

    public static void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key,value);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key,string.Empty);
    }
    
}
