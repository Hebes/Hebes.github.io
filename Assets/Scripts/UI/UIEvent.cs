using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// UI输入事件
/// </summary>
public class UIEvent : UnityEngine.EventSystems.EventTrigger
{
    #region 常量
    protected const float CLICK_INTERVAL_TIME = 0.2f;   //点击时间间隔
    protected const float CLICK_INTERVAL_POS = 5;       //点击的距离间隔
    #endregion

    #region 委托定义
    //点击（指针）事件委托
    public delegate void PointerEventDelegate(PointerEventData eventData, UIEvent ev);
    //基础事件委托
    public delegate void BaseEventDelegate(BaseEventData eventData, UIEvent ev);
    //轴向事件委托
    public delegate void AxisEventDelegate(AxisEventData eventData, UIEvent ev);
    #endregion

    #region 事件定义
    //选择
    public BaseEventDelegate onSelect = null;
    //更新选择
    public BaseEventDelegate onUpdateSelect = null;
    //取消选择
    public BaseEventDelegate onDeselect = null;
    //取消
    public BaseEventDelegate onCancel = null;
    //提交
    public BaseEventDelegate onSubmit = null;

    //开始拖拽
    public PointerEventDelegate onBeginDrag = null;
    //拖拽中
    public PointerEventDelegate onDrag = null;
    //结束拖拽
    public PointerEventDelegate onEndDrag = null;
    //拖拽停下
    public PointerEventDelegate onDrop = null;
    //轴向移动
    public AxisEventDelegate onMove = null;
    //点击
    public PointerEventDelegate onClick = null;
    //指针按下
    public PointerEventDelegate onDown = null;
    //指针进入
    public PointerEventDelegate onEnter = null;
    //指针离开
    public PointerEventDelegate onExit = null;
    //指针松开
    public PointerEventDelegate onUp = null;
    //滑动
    public PointerEventDelegate onScroll = null;
    //初始化潜在的拖拽
    public PointerEventDelegate onInitializePotentialDrag = null;
    #endregion

    //参数字典
    public Dictionary<string, object> mArg = new Dictionary<string, object>();
    //指针数据
    private static PointerEventData mPointData = null;

    /// <summary>
    /// 设置参数
    /// </summary>
    /// <param name="key">参数名</param>
    /// <param name="val">参数值</param>
    public void SetData(string key,object val)
    {
        mArg[key] = val;
    }

    /// <summary>
    /// 获取参数
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <param name="key">参数名</param>
    /// <returns></returns>
    public D GetData<D>(string key)
    {
        if(mArg.ContainsKey(key))
        {
            return (D)mArg[key];
        }
        return default(D);
    }

    /// <summary>
    /// 获取事件
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static UIEvent Get(GameObject go)
    {
        UIEvent listener = go.GetComponent<UIEvent>();
        if(listener==null)
        {
            listener=go.AddComponent<UIEvent>();
        }
        return listener;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
    }

    public override void OnMove(AxisEventData eventData)
    {
        base.OnMove(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if(onClick!=null)
        {
            onClick(eventData,this);
        }

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }

    public override void OnScroll(PointerEventData eventData)
    {
        base.OnScroll(eventData);
    }
}
