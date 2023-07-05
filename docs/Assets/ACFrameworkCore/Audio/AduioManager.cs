using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    音频模块

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
