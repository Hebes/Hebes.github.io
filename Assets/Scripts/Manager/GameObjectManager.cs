using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectManager : MonoBehaviour
{
    public GameObject playerObj;
    public Transform playerRoot;
    public GameObject player;
    private CinemachineVirtualCamera virtualCamera;
    
    /// <summary>
    /// 加载玩家
    /// </summary>
    /// <param name="characterId">角色id</param>
    public void LoadPlayer(int characterId)
    {
        CharacterData character = CharacterManager.Instance.GetCharacter(characterId);
        CharacterConfig config = ConfigManager.Instance.GetCharacterConfigById((int)character.CharacterClass);
        Debug.Log(config);
        string modelUrl = config.Resource;
        Debug.Log($"加载模型的地址为：{modelUrl}");
        playerObj =null;
        GameManager.Instance.ResManager.LoadPrefab(modelUrl,obj =>
        {
            playerObj = obj as GameObject;;
        });
        if (playerObj != null)
        {
            player = Instantiate(playerObj,this.transform);
            player.name = "Player";
            player.SetActive(true);
            playerRoot = GameObject.Find("PlayerRoot").transform;
            player.transform.SetParent(playerRoot);
            player.transform.localPosition = new Vector3(-90.5f, 27.5f, 0);
            Debug.Log(player.name);
        }
        SetCamera();
    }

    /// <summary>
    /// 设置摄像头
    /// </summary>
    public void SetCamera()
    {
        virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = player.transform;
    }
    
}
