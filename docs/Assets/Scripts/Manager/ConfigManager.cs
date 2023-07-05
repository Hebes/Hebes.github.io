using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 配置管理器
/// </summary>
public class ConfigManager : Singleton<ConfigManager>
{
    public List<CharacterConfig> CharacterConfigs =null;

    public ConfigManager()
    {
        
    }

    public void LoadConfigs()
    {
        string json = File.ReadAllText(PathUtil.configPath + "CharacterConfig.json");
        CharacterConfigs = JsonConvert.DeserializeObject<List<CharacterConfig>> (json);
    }

    /// <summary>
    /// 获取指定id的角色配置表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CharacterConfig GetCharacterConfigById(int id)
    {
        CharacterConfig resConfig = null;
        foreach (var config in CharacterConfigs)
        {
            if (config.Id == id)
            {
                resConfig = config;
            }
        }
        return resConfig;
    }
}
