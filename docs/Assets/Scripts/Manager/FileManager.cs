using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �浵������
/// </summary>
public class FileManager : Singleton<FileManager>
{
    /// <summary>
    /// 保存角色信息
    /// </summary>
    public void SaveCharacter()
    {
        DataManager.SaveData<List<CharacterData>>("PlayerData",CharacterManager.Instance.allCharacters);
    }
}
