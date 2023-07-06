using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterManager : Singleton<CharacterManager>
{
    
    private CharacterData character;
    /// <summary>
    /// 玩家的所有角色信息
    /// </summary>
    public List<CharacterData> allCharacters;

    public int selectCharacterId;

    /// <summary>
    /// 玩家正常的角色信息
    /// </summary>
    public List<CharacterData> characters;

    public void Init()
    {
        allCharacters = new List<CharacterData>();
        allCharacters = DataManager.LoadData<List<CharacterData>>(PathUtil.playerDataPath + "PlayerData.json");
        characters = new List<CharacterData>();
        if (allCharacters!=null && allCharacters.Count > 0)
        {
            //将未删除的角色添加到角色列表
            foreach (var c in allCharacters)
            {
                if (c.CharacterState == 1)
                {
                    characters.Add(c);
                }
            }
            Debug.Log($"该玩家角色数量为:{allCharacters.Count}");
        }
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="characterId"></param>
    /// <param name="characterName"></param>
    /// <param name="characterClass"></param>
    public void CreateCharacter(int characterId, string characterName, int characterClass)
    {
        character = new CharacterData();
        character.CharacterId = characterId;
        character.CharacterName = characterName;
        character.CharacterLevel = 1;
        character.CharacterExp = 0;
        character.CharacterClass = (CharacterClass)characterClass;
        character.CharacterState = 1;
        if (allCharacters == null)
        {
            allCharacters = new List<CharacterData>();
        }
        allCharacters.Add(character);
        //将数据临时存到本地
        PlayerPrefsData.SaveInt(PlayerPrefsData.CharacterClass, characterClass);
        PlayerPrefsData.SaveString(PlayerPrefsData.CharacterName, characterName);
        PlayerPrefsData.SaveInt(PlayerPrefsData.CharacterLevel, character.CharacterLevel);
        PlayerPrefsData.SaveInt(PlayerPrefsData.CharacterExp, character.CharacterExp);
        PlayerPrefsData.SaveInt(PlayerPrefsData.CharacterId, character.CharacterId);
    }

    /// <summary>
    /// 移除角色
    /// </summary>
    /// <param name="id">角色id</param>
    public void RemoveCharacter(int id)
    {
        int index=-1;
        //修改所有角色列表中对应角色的状态
        for (int i = 0; i < allCharacters.Count; i++)
        {
            if(allCharacters[i].CharacterId == id)
            {
                index = i;
            }
        }
        allCharacters[index].CharacterState = 0;
        //移除玩家拥有列表中的对应角色
        for (int i = 0; i < characters.Count; i++)
        {
            if(characters[i].CharacterId == id)
            {
                index = i;
            }
        }
        characters.RemoveAt(index);
    }

    /// <summary>
    /// 更新角色信息
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="character">角色信息</param>
    public void UpdateCharacter(int id,CharacterData character)
    {
        for (int i = 0; i < allCharacters.Count; i++)
        {
            if(allCharacters[i].CharacterId == id)
            {
                allCharacters[i] = character;
            }
        }
        for (int i = 0; i < characters.Count; i++)
        {
            if(characters[i].CharacterId == id)
            {
                characters[i] = character;
            }
        }
        Debug.Log("指定id角色不存在");
    }

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <param name="characterId">角色id</param>
    /// <returns></returns>
    public CharacterData GetCharacter(int characterId)
    {
        foreach (var character in allCharacters)
        {
            if (character.CharacterId == characterId)
            {
                 return character;
            }
        }
        Debug.Log($"id为{characterId}的角色不存在");
        return null;
    }

    /// <summary>
    /// 增加经验
    /// </summary>
    /// <param name="exp"></param>
    public void AddExp(int exp)
    {
        int currentExp = PlayerPrefsData.GetInt(PlayerPrefsData.CharacterExp);
        currentExp += exp;
        if (currentExp >1000)
        {
            LevelUp();
        }
        PlayerPrefsData.SaveInt(PlayerPrefsData.CharacterExp, currentExp);
    }

    /// <summary>
    /// 角色升级
    /// </summary>
    public void LevelUp()
    {
        int level = PlayerPrefsData.GetInt(PlayerPrefsData.CharacterLevel);
        level++;
        PlayerPrefsData.SaveInt(PlayerPrefsData.CharacterLevel, level);
    }
    
    
}
