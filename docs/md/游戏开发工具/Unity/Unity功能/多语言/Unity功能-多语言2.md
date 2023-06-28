# Unity功能-多语言2

**[https://github.com/Hebes/UnityCollect.git 多语言](<https://github.com/Hebes/UnityCollect.git>)**

效果图

![1](\Image/1.png)

![2](\Image/2.png)

管理类

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ACLanguage
{
    /// <summary>
    /// 语言类型
    /// </summary>
    public enum ELanguageMode
    {
        Chinese = 0,
        English = 1,
    }

    /// <summary>
    /// 多语言控制脚本
    /// </summary>
    public class LanguageManager : MonoBehaviour
    {
        public Toggle toggle1;//中文
        public Toggle toggle2;//英文
        private Dictionary<string, string> LanguageDic { get; set; }

        public ELanguageMode languageMode { get; set; }

        private Dictionary<string, string> EnglishLanguageDic { get; set; } = new Dictionary<string, string>()
        {
            {"1","Hello" },
        };
        private Dictionary<string, string> ChineseLanguageDic { get; set; } = new Dictionary<string, string>()
        {
            {"1","你好" },
        };

        private void Awake()
        {
            //可以用PlayerPrefs存储语言类型,这里直接首次用的英文
            LanguageDic = new Dictionary<string, string>();

            OnChangeLanuage(ELanguageMode.Chinese);//初始化第一次中文

            toggle1.onValueChanged.AddListener(Ontoggle1);
            toggle2.onValueChanged.AddListener((bool isOn) =>
            {
                if (isOn)
                {
                    Debug.Log("切换英文");
                    OnChangeLanuage(ELanguageMode.English);
                }
            });
        }

        private void Ontoggle1(bool isOn)
        {
            if (isOn)
            {
                Debug.Log("切换中文");
                OnChangeLanuage(ELanguageMode.Chinese);
            }
        }

        /// <summary>
        /// 添加多语言数据
        /// </summary>
        public void OnAddLanguageData(ELanguageMode languageMode)
        {
            //TODO 这里可以添加多语言数据.可以从读取数据加载,这里直接用的测试
            switch (languageMode)
            {
                case ELanguageMode.Chinese:
                    LanguageDic = ChineseLanguageDic;
                    break;
                case ELanguageMode.English:
                    LanguageDic = EnglishLanguageDic;
                    break;
                default:
                    LanguageDic = ChineseLanguageDic;
                    break;
            }
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        public void OnChangeLanuage(ELanguageMode languageMode)
        {
            //添加数据
            OnAddLanguageData(languageMode);

            //保存数据
            PlayerPrefs.SetInt("Language", (int)languageMode);
            this.languageMode = languageMode;
            //调用中间LanaguageBridge切换语言
            LanaguageBridge.Instance.LanguageTextKeyDic = LanguageDic;
            LanaguageBridge.Instance.OnLanguageChange();
        }
    }
}
```

中间类

```C#
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ACLanguage
{
    /// <summary>
    /// 语言的中间组件
    /// </summary>
    public class LanaguageBridge:BaseManager<LanaguageBridge>
    {
        public Dictionary<string, string> LanguageTextKeyDic;//语言字典
        public event Action OnLanguageChangeEvt;//回调事件

        public Font font { get; set; }//字体

        /// <summary>
        /// 切换语言
        /// </summary>
        public void OnLanguageChange()
        {
            OnLanguageChangeEvt?.Invoke();
        }

        /// <summary>
        /// 获取文字
        /// </summary>
        public string GetText(string key)
        {
            if (LanguageTextKeyDic.ContainsKey(key))
                return LanguageTextKeyDic[key];
            Debug.Log("多语言未配置：" + key);
            return key;
        }
    }
}
```

实现类

```C#
using UnityEngine;
using UnityEngine.UI;

namespace ACLanguage
{
    public class LanguageText : MonoBehaviour
    {
        public string key;

        private Text m_Text;
        //private TMP_Text m_MeshText;
        private void Awake()
        {
            m_Text = GetComponent<Text>();
            //m_MeshText = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            //如果是放在UI界面中的话开启这段代码
            //OnSwitchLanguage();
            //LanaguageBridge.Instance.OnLanguageChangeEvt += OnSwitchLanguage;
        }
        private void Start()
        {
            //一下代码在LanguageManagerAwake之后执行
            //测试时候用的放在UI界面中的可以删除
            OnSwitchLanguage();
            LanaguageBridge.Instance.OnLanguageChangeEvt += OnSwitchLanguage;
        }

        private void OnDisable()
        {
            LanaguageBridge.Instance.OnLanguageChangeEvt -= OnSwitchLanguage;
        }

        private void OnSwitchLanguage()
        {
            if (m_Text != null)
                m_Text.text = LanaguageBridge.Instance.GetText(key);
            //if (m_MeshText != null)
                //m_MeshText.text = LanaguageBridge.Instance.GetText(key);
        }
    }
}
```
