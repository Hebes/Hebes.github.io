using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------�ű�����-----------
				
�������䣺
	1607388033@qq.com
����:
	����
����:
    ��Ƶģ��

-----------------------*/


namespace ACFrameworkCore
{
    public class AduioComponent : ICoreComponent
    {
        public List<ICoreComponent> CoreComponentList { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        List<ICoreComponent> ICoreComponent.CoreComponentList { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        void ICoreComponent.OnCroeComponentInit()
        {
            throw new System.NotImplementedException();
        }

        Dictionary<string, AudioClip> AudioClipDic = new Dictionary<string, AudioClip>();
    }
}
