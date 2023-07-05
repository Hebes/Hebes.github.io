using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI界面基类
/// </summary>
public class UIWindowBase : MonoBehaviour
{
    /// <summary>
    /// 是否初始化
    /// </summary>
    protected bool mInitialized = false;
    /// <summary>
    /// UI界面的状态
    /// </summary>
    protected UIState mState = UIState.None;
    public UIState State
    {
        get { return mState; }
    }

    public UIType uiType = UIType.Normal;

    /// <summary>
    /// 界面的事件
    /// </summary>
    /// <param name="window"></param>
    public delegate void OnWindowEventHandler(UIWindowBase window);
    public event OnWindowEventHandler OnWindowClose;

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Init()
    {
        this.mInitialized = true;
    }

    /// <summary>
    /// 展示界面
    /// </summary>
    public virtual void Show()
    {
        this.mState = UIState.Open;
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public virtual void Close()
    {
        this.mState = UIState.Close;
    }

    /// <summary>
    /// 隐藏界面
    /// </summary>
    public virtual void Hide()
    {
        this.mState = UIState.Hiden;
    }
}

/// <summary>
/// UI的状态
/// </summary>
public enum UIState
{
    /// <summary>
    /// 无状态
    /// </summary>
    None,
    /// <summary>
    /// 打开
    /// </summary>
    Open,
    /// <summary>
    /// 关闭
    /// </summary>
    Close,
    /// <summary>
    /// 隐藏
    /// </summary>
    Hiden,
    /// <summary>
    /// 预加载
    /// </summary>
    Preopen,
}

public enum UIType
{
    /// <summary>
    /// 无层级  单独UI
    /// </summary>
    None,
    /// <summary>
    /// 基础UI
    /// </summary>
    Basic,
    /// <summary>
    /// 普通UI
    /// </summary>
    Normal,
    /// <summary>
    /// 顶层UI
    /// </summary>
    Top,
}
