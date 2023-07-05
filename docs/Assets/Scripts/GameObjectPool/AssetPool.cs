using UnityEngine;

public class AssetPool : PoolBase
{
    public override Object Get(string name)
    {
        return base.Get(name);
    }

    public override void Set(string name, Object obj)
    {
        base.Set(name, obj);
    }

    public override void Release()
    {
        base.Release();
        foreach(PoolObject item in m_Objects)
        {
            if(System.DateTime.Now.Ticks - item.lastUseTime.Ticks >= releaseTime * 10000000)
            {
                Debug.Log("AssetPool Release time:" + System.DateTime.Now + "UnLoad ab" + item.Name);
                //–∂‘ÿ◊ ‘¥
                GameManager.Instance.ResManager.UnLoadBundle(item.Object);
                m_Objects.Remove(item);
                //µ›πÈ ∑¿÷πForeach“Ï≥£
                Release();
                return;
            }
        }
    }
}
