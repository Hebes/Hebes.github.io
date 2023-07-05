using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderItem : MonoBehaviour
{
    public uint Id;

    public void Set(uint from,uint to)
    {
        this.Id = to;
        
        this.gameObject.name=to.ToString();
    }

}
