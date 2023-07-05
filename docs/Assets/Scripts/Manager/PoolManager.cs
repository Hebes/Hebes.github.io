using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
///对象池管理器
/// </summary>
public class PoolManager : MonoBehaviour
{
    //����صĸ��ڵ�
    Transform rootTrans;

    //���ж����
    Dictionary<string, PoolBase> m_Pools = new Dictionary<string, PoolBase>();

    private void Awake()
    {
        rootTrans = this.transform.Find("Pool");
    }

    /// <summary>
    /// ���������
    /// </summary>
    /// <typeparam name="T">Asset/GameObject</typeparam>
    /// <param name="poolName">���������</param>
    /// <param name="releaseTime">�Զ��ͷ�ʱ��</param>
    private void CreatePool<T>(string poolName,float releaseTime) where T :PoolBase
    {
        if(!m_Pools.TryGetValue(poolName,out PoolBase pool))
        {
            GameObject go = new GameObject(poolName);
            go.transform.SetParent(rootTrans);
            pool.AddComponent<T>();
            pool.Init(releaseTime);
            m_Pools.Add(poolName, pool);
        }
    }
    /// <summary>
    /// ������Ϸ��������
    /// </summary>
    /// <param name="poolName"></param>
    /// <param name="releaseTime"></param>
    public void CreateGameObjectPool(string poolName,float releaseTime)
    {
        CreatePool<GameObjectPool>(poolName,releaseTime);
    }
    /// <summary>
    /// ������Դ�����
    /// </summary>
    /// <param name="poolName"></param>
    /// <param name="releaseTime"></param>
    public void CreateAssetPool(string poolName,float releaseTime)
    {
        CreatePool<AssetPool>(poolName,releaseTime);
    }

    /// <summary>
    /// �Ӷ������ȡ����
    /// </summary>
    /// <param name="poolName">���������</param>
    /// <param name="assetName">��Դ����</param>
    /// <returns></returns>
    public Object Get(string poolName,string assetName)
    {
        Object obj = null;
        if(m_Pools.TryGetValue(poolName,out PoolBase pool))
        {
            obj = pool.Get(assetName);
        }
        return obj;
    }

    /// <summary>
    /// ���ն���
    /// </summary>
    /// <param name="poolName">���������</param>
    /// <param name="assetName"></param>
    /// <param name="obj"></param>
    public void Set(string poolName,string assetName,Object obj)
    {
        if (m_Pools.TryGetValue(poolName, out PoolBase pool))
        {
            pool.Set(assetName, obj);
        }
    }
}
