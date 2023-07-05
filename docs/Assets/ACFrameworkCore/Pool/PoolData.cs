using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    对象池数据

-----------------------*/


namespace ACFrameworkCore
{
    /// <summary>
    /// 对象池数据
    /// </summary>
    public class PoolData
    {
        /// <summary>
        /// 对象挂载的父节点
        /// </summary>
        public GameObject fatherObj;

        /// <summary>
        /// 对象的容器
        /// </summary>
        public List<GameObject> poolList;


        public PoolData(GameObject obj, GameObject poolObj)
        {
            fatherObj = new GameObject(obj.name);
            fatherObj.transform.SetParent(poolObj.transform, false);
            poolList = new List<GameObject>() { };
            PushObj(obj);
        }

        /// <summary>
        /// 隐藏对象
        /// </summary>
        /// <param name="obj"></param>
        public void PushObj(GameObject obj)
        {
            obj.SetActive(false);
            poolList.Add(obj);
            obj.transform.SetParent(fatherObj.transform, false);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public GameObject GetObj()
        {
            GameObject obj = poolList[0];
            poolList.RemoveAt(0);
            obj.SetActive(true);
            obj.transform.SetParent(null);
            return obj;
        }
    }
}
