using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : PoolBase
{
    public override Object Get(string name)
    {
        Object obj = base.Get(name);
        if(obj == null)
        {
            return null;
        }
        GameObject go = obj as GameObject;
        go.SetActive(true);
        return go;
    }

    public override void Set(string name, Object obj)
    {
        GameObject go = obj as GameObject;
        go.SetActive(false);
        go.transform.SetParent(this.transform, false);
        base.Set(name, obj);
    }

    public override void Release()
    {
        base.Release();
        foreach (PoolObject item in m_Objects)
        {
            if (System.DateTime.Now.Ticks - item.lastUseTime.Ticks >= base.releaseTime * 10000000)
            {
                Debug.Log("GameObjectPool Release time" + System.DateTime.Now);
                Destroy(item.Object);
                GameManager.Instance.ResManager.MinusBundleCount(item.Name);
                m_Objects.Remove(item);   //使用fereach删除对象之后 无法继续使用迭代器   所以再递归一下
                Release();
                return;
            }
        }
    }
}
