using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// UI拖拽的基类
/// </summary>
public class UIDragBase : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [Header("拖拽方向")]
    public bool Horizontal=false;
    public bool Vertical=false;
    //是否拖拽中
    protected bool isDraging = false;
    //指针位置偏移
    protected Vector2 offsetPos;

    // 拖拽开始
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraging)
        {
            isDraging = true;
        }
    }
    //拖拽中
    public void OnDrag(PointerEventData eventData)
    {
        if (isDraging)
        {
            offsetPos = eventData.delta;
            if (Horizontal)
            {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x + offsetPos.x, this.transform.localPosition.y, this.transform.localPosition.z);
            }
            if (Vertical)
            {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + offsetPos.y, this.transform.localPosition.z);
            }
        }
    }
    //指针松开 拖拽结束
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDraging)
        {
            isDraging = false;
        }
    }
}
