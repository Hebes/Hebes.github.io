using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 新建存档
/// </summary>
public class UINewFile : UIWindowBase
{
    public Text characterDesctiption;
    public InputField nameInput;
    public string characterName;

    public GameObject[] characterObjs;
    public Image[] selectedImgs;
    
    //当前职业
    private int curClass;
    //职业描述
    private string desc;
    //名称
    private string name;

    public void Init()
    {
        //默认选择战士
        SelectCharacter(1);
    }
    
    /// <summary>
    /// 选择职业
    /// </summary>
    /// <param name="index">ְid</param>
    public void SelectCharacter(int index)
    {
        curClass = index;
        //Todo：展示角色
        ShowCharacter(index);
        //职业描述
        desc = ConfigManager.Instance.GetCharacterConfigById(index).Description;
        characterDesctiption.text = desc;
        
    }

    /// <summary>
    /// 展示角色
    /// </summary>
    /// <param name="id">角色id</param>
    private void ShowCharacter(int id)
    {
        for (int i = 0; i < characterObjs.Length; i++)
        {
            characterObjs[i].SetActive(i==id-1);
        }

        for (int i = 0; i < selectedImgs.Length; i++)
        {
            selectedImgs[i].gameObject.SetActive(i==id-1);
        }
    }
    
    public void OnClickEnter()
    {
        Debug.Log("进入游戏");
        //判断名称是否合法
        if (nameInput.text == null)
        {
            Debug.Log("角色名称不合法");
            return;
        }

        name = nameInput.text;
        //存档数据
        int characterId;
        if (CharacterManager.Instance.allCharacters == null || CharacterManager.Instance.allCharacters.Count <= 0)
        {
            characterId = 1;
        }
        else
        {
            characterId = CharacterManager.Instance.allCharacters.Count+1;
        }
        CharacterManager.Instance.CreateCharacter(characterId,name,curClass);
        CharacterManager.Instance.selectCharacterId = characterId;
        FileManager.Instance.SaveCharacter();
        //Todo:进入游戏
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
