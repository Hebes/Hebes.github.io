using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBase : MonoBehaviour
{
    //�Զ��ͷ�ʱ��
    protected float releaseTime;
    //�ϴ��ͷŵ�ʱ��  ��λ ��΢��  1��=10000000��΢��
    protected long lastReleaseTime = 0;

    protected List<PoolObject> m_Objects;

    public void Start()
    {
        lastReleaseTime = System.DateTime.Now.Ticks;
    }

    /// <summary>
    /// ��ʼ�������
    /// </summary>
    /// <param name="time">�����Զ��ͷ�ʱ��</param>
    public void Init(float time)
    {
        releaseTime = time;
        m_Objects = new List<PoolObject>();
    }

    /// <summary>
    /// ȡ����
    /// </summary>
    /// <param name="name">������</param>
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
    /// �����
    /// </summary>
    /// <param name="name">������</param>
    /// <param name="obj">����</param>
    public virtual void Set(string name, Object obj)
    {
        PoolObject po = new PoolObject(name,obj);
        m_Objects.Add(po);
    }

    /// <summary>
    /// �ͷŶ����
    /// </summary>
    public virtual void Release()
    {

    }

    public void Update()
    {
        //�����ͷ�ʱ�� �Զ��ͷ�
        if(System.DateTime.Now.Ticks - lastReleaseTime >= releaseTime * 10000000 )
        {
            lastReleaseTime = System.DateTime.Now.Ticks;
            Release();
        }
    }
}
