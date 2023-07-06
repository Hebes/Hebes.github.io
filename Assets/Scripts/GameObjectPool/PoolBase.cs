using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBase : MonoBehaviour
{
    //自动释放时间
    protected float releaseTime;
    //上次释放的时间  单位 毫微秒  1秒=10000000毫微秒
    protected long lastReleaseTime = 0;

    protected List<PoolObject> m_Objects;

    public void Start()
    {
        lastReleaseTime = System.DateTime.Now.Ticks;
    }

    /// <summary>
    /// 初始化对象池
    /// </summary>
    /// <param name="time">对象自动释放时间</param>
    public void Init(float time)
    {
        releaseTime = time;
        m_Objects = new List<PoolObject>();
    }

    /// <summary>
    /// 取对象
    /// </summary>
    /// <param name="name">对象名</param>
    /// <returns></returns>
    public virtual Object Get(string name)
    {
        foreach (PoolObject obj in m_Objects)
        {
            if (obj.Name == name)
            {
                m_Objects.Remove(obj);
                return obj.Object;
            }
        }
        return null;
    }

    /// <summary>
    /// 存对象
    /// </summary>
    /// <param name="name">对象名</param>
    /// <param name="obj">对象</param>
    public virtual void Set(string name, Object obj)
    {
        PoolObject po = new PoolObject(name,obj);
        m_Objects.Add(po);
    }

    /// <summary>
    /// 释放对象池
    /// </summary>
    public virtual void Release()
    {

    }

    public void Update()
    {
        //超过释放时间 自动释放
        if(System.DateTime.Now.Ticks - lastReleaseTime >= releaseTime * 10000000 )
        {
            lastReleaseTime = System.DateTime.Now.Ticks;
            Release();
        }
    }
}
