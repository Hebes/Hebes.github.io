using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 玩家Item
/// </summary>
public class PlayerInfoItem : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Image avatarImage;
    public GameObject selectedImg;
    public int curCharacterId;
    /// <summary>
    /// 初始化item
    /// </summary>
    /// <param name="name"></param>
    /// <param name="level"></param>
    /// <param name="classId"></param>
    public void InitItem(int characterId,string name, int level,int classId)
    {
        curCharacterId = characterId;
        nameText.text = name;
        levelText.text = $"Lv.{level}";
        GetAvatar(classId);
    }
    
    /// <summary>
    /// 获取职业头像
    /// </summary>
    /// <param name="classId"></param>
    private void GetAvatar(int classId)
    {
        Texture2D tex;
        CharacterConfig config = ConfigManager.Instance.GetCharacterConfigById(classId);
        string imgUrl = config.AvatarImage;
        GameManager.Instance.ResManager.LoadSprite(imgUrl, (obj) =>
        {
            //加载图片
            tex = obj as Texture2D;
            avatarImage.sprite = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));
        });
    }

    /// <summary>
    /// 选中角色
    /// </summary>
    public void SelectItem()
    {
        Debug.Log($"当前选中的角色id为:{curCharacterId}");
        CharacterManager.Instance.selectCharacterId = curCharacterId;
    }

    private void Update()
    {
        selectedImg.gameObject.SetActive(curCharacterId==CharacterManager.Instance.selectCharacterId);
    }
}
