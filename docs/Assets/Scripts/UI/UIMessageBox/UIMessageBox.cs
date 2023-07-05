using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 消息框
/// </summary>
public class UIMessageBox : UIWindowBase
{
    public Text titleTxt;
    public Text messageTxt;
    public Button confrimBtn;
    public Button cancelBtn;
    //按钮事件
    public Action confirm;
    public Action cancel;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="title">消息框的标题</param>
    /// <param name="message">消息框的内容</param>
    /// <param name="confrimCB"></param>
    /// <param name="cancelCB"></param>
    public void Init(string title,string message,Action confrimCB=null,Action cancelCB=null)
    {
        base.Init();
        if (string.IsNullOrEmpty(message))
        {
            Debug.LogError("消息内容不能为空！");
            this.gameObject.SetActive(false);
            return;
        }
        messageTxt.text = message;
        if (string.IsNullOrEmpty(title))
        {
            titleTxt.text = "Message";
        }
        else
        {
            titleTxt.text = title;
        }
        confirm = confrimCB;
        cancel = cancelCB;

        //按钮添加监听
        confrimBtn.onClick.AddListener(OnClickConfrim);
        cancelBtn.onClick.AddListener(OnClickCancel);
    }

    private void OnClickConfrim()
    {
        confirm?.Invoke();
        GameManager.Instance.UIManager.HideWindow(this);
    }

    private void OnClickCancel()
    {
        cancel?.Invoke();
        GameManager.Instance.UIManager.HideWindow(this);
    }
}
