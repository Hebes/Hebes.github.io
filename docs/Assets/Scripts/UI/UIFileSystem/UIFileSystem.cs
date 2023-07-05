using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFileSystem : UIWindowBase
{
    public GameObject playerItemObj;
    public Transform itemParent;

    public void Init()
    {
        SetPlayerCharacters();
    }
    
    /// <summary>
    /// 读取角色存档
    /// </summary>
    private void SetPlayerCharacters()
    {
        //清除原有的记录
        if (itemParent.childCount > 0)
        {
            for (int i = 0; i < itemParent.childCount; i++)
            {
                Destroy(itemParent.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < CharacterManager.Instance.characters.Count; i++)
        {
            CharacterData character = CharacterManager.Instance.characters[i];
            PlayerInfoItem item = Instantiate(playerItemObj, itemParent).GetComponent<PlayerInfoItem>();
            item.gameObject.SetActive(true);
            item.InitItem(character.CharacterId, character.CharacterName, character.CharacterLevel,
                (int)character.CharacterClass);
        }

    }

    /// <summary>
    /// 点击删除存档
    /// </summary>
    public void OnClickDelete()
    {
        //删除角色
        CharacterManager.Instance.RemoveCharacter(CharacterManager.Instance.selectCharacterId);
        //刷新界面
        SetPlayerCharacters();
        //保存数据
        FileManager.Instance.SaveCharacter();
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void OnClickEnter()
    {
        GameManager.Instance.SceneSwitchManager.LoadScene("City01");
        GameManager.Instance.SceneSwitchManager.onSceneLoaded += OnGameEnter;
    }
    
    /// <summary>
    /// 关闭该界面
    /// </summary>
    public void OnClickColse()
    {
        GameManager.Instance.UIManager.HideWindow(this);
    }

    public void OnGameEnter()
    {
        GameManager.Instance.GameObjectManager.LoadPlayer(CharacterManager.Instance.selectCharacterId);
    }
}
