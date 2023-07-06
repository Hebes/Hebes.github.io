using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseData
{
    public BaseData() 
    {

    }
}

/// <summary>
/// 角色数据
/// </summary>
public class CharacterData
{
    public int CharacterId { get; set; }
    public string CharacterName { get; set; }
    public int CharacterLevel { get; set; }
    public int CharacterExp { get; set; }
    /// <summary>
    /// 角色状态 0：被删除  1：正常
    /// </summary>
    public int CharacterState { get; set; }
    
    public CharacterClass CharacterClass { get; set; }

    public CharacterData()
    {

    }
}

/// <summary>
/// 职业类型
/// </summary>
public enum CharacterClass
{
    /// <summary>
    /// 展示
    /// </summary>
    Warrior=1,
    /// <summary>
    /// 弓箭手
    /// </summary>
    Archer=2,
    /// <summary>
    /// 法师
    /// </summary>
    Wizard=3,
}
