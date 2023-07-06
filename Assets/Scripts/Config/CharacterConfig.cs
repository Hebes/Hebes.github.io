using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 角色配置表
/// </summary>
public class CharacterConfig
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CharacterClass Class { get; set; }
    public string Resource { get; set; }
    public string AvatarImage { get; set; }
    public string Description { get; set; }
    public int Speed { get; set; }
    public float MaxHp { get; set; }
    public float MaxMp { get; set; }
    public float GrowthSTR { get; set; }
    public float GrowthINT { get; set; }
    public float GrowthDEX { get; set; }
    public float STR { get; set; }
    public float INT { get; set; }
    public float DEX { get; set; }
    public float AD { get; set; }
    public float AP { get; set; }
    public float DEF { get; set; }
    public float MDEF { get; set; }
    public float SPD { get; set; }
    public float CRI { get; set; }
}
