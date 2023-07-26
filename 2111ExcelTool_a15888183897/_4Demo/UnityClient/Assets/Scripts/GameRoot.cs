/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 游戏启动

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PERes;
using UnityEngine;

public class GameRoot : MonoBehaviour {
    void Start() {
        CfgSvc.Instance.Init();

        CfgSvc.Instance.LoadTbl<ResSkillList>();
        CfgSvc.Instance.LoadTbl<ResFubenList>();

        ResSkill skillCfg = (ResSkill)CfgSvc.Instance.GetTblItem<ResSkillList>(1013);
        Debug.Log("skill id:" + skillCfg.Id);
        Debug.Log("skill name:" + skillCfg.Name);
        Debug.Log("skill damagetype:" + skillCfg.Damagetype);
        Debug.Log(string.Format("skill cooldown:{0},{1},{2},{3}", skillCfg.Cooldowns[0], skillCfg.Cooldowns[1], skillCfg.Cooldowns[2], skillCfg.Cooldowns[3]));
        Debug.Log("skill des:" + skillCfg.Des);

        ResFuben fubenCfg = (ResFuben)CfgSvc.Instance.GetTblItem<ResFubenList>(3002);
        Debug.Log("fuben id:" + fubenCfg.Id);
        Debug.Log("fuben name:" + fubenCfg.Name);
        Debug.Log("fuben type:" + fubenCfg.Type);
        Debug.Log("fuben mapid:" + fubenCfg.Mapid);
        Debug.Log("fuben drop item[0] id:" + fubenCfg.Drops[0].Id + " num:" + fubenCfg.Drops[0].Num);
    }
}