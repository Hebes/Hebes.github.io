using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public Object Object;
    public string Name;

    public System.DateTime lastUseTime;

    public PoolObject(string name, Object obj)
    {
        this.Name = name;
        this.Object = obj;
        lastUseTime = System.DateTime.Now;
    }

}
