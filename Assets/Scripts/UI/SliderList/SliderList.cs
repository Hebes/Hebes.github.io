using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderList : MonoBehaviour
{
    private ScrollRect m_scroll;
    private RectTransform m_rect;

    /// <summary>
    /// Item
    /// </summary>
    public RectTransform Item;
    /// <summary>
    /// 方向
    /// </summary>
    public Dirction Dirction;
    /// <summary>
    /// 总数量
    /// </summary>
    public uint count;
    /// <summary>
    /// Item的间隔
    /// </summary>
    public int m_ItemInterval;
    /// <summary>
    /// 是否参与计算
    /// </summary>
    private bool m_calculate;
    /// <summary>
    /// 间隔
    /// </summary>
    private float m_Interval;
    /// <summary>
    /// 当前最小索引
    /// </summary>
    public uint m_curMinIndex;
    /// <summary>
    /// 当前最大索引
    /// </summary>
    public uint m_curMaxIndex;
    /// <summary>
    /// 可以展示Item的最大数量
    /// </summary>
    private uint m_maxShowCount;
    /// <summary>
    /// item存放的容器
    /// </summary>
    private List<SliderItem> m_List = new List<SliderItem>();

    private void Awake()
    {
        Item.gameObject.SetActive(false);
        m_scroll = this.GetComponentInParent<ScrollRect>();
        var tempRect = m_scroll.GetComponent<RectTransform>();
        m_rect = this.gameObject.GetComponent<RectTransform>();
        //可滑动区域的大小
        Vector2 size=m_rect.sizeDelta;
        switch(Dirction)
        {
            //横向
            case Dirction.Horizontal:
                //设置Content的对齐方式
                m_rect.anchorMin = new Vector2(0, 0);
                m_rect.anchorMax = new Vector2(0, 1);
                //计算间隔
                m_Interval = Item.rect.size.x + m_ItemInterval;
                //计算滑动的最大区间
                size.x = m_Interval * count;
                m_calculate = size.x > tempRect.rect.size.x;
                //计算最大显示数量
                m_maxShowCount = (uint)(tempRect.rect.size.x / Item.sizeDelta.x + 2);
                break;
            //纵向
            case Dirction.Vertical:
                //设置Content的对齐方式
                m_rect.anchorMin = new Vector2(0, 1);
                m_rect.anchorMax = new Vector2(1, 1);
                //计算间隔
                m_Interval = Item.rect.size.y + m_ItemInterval;
                //计算滑动的最大区间
                size.y = m_Interval * count;
                m_calculate = size.y > tempRect.rect.size.y;
                //计算最大显示数量
                m_maxShowCount = (uint)(tempRect.rect.size.y / Item.sizeDelta.y + 2);
                break;
            default:
                break;
        }
        m_rect.sizeDelta = size;
        m_rect.localPosition = Vector3.zero;
        //当前索引
        int index = 0;
        for(int i=0;i<m_maxShowCount;i++)
        {
            if(i>=count)
            {
                break;
            }
            index++;
            GameObject go = GameObject.Instantiate<GameObject>(Item.gameObject,Item.parent);
            go.transform.localPosition = GetPosition((int)i);
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
            SliderItem item = go.AddComponent<SliderItem>();
            item.Set(0, (uint)i);
            m_curMaxIndex++;
            m_List.Add(item);
        }
        m_curMinIndex = 0;
    }

    private void Update()
    {
        if(m_calculate)
        {
            //当前的索引位置
            float index = 0;
            switch(Dirction)
            {
                case Dirction.Horizontal:
                    index = Mathf.Abs(m_rect.localPosition.x) / m_Interval;
                    break;
                case Dirction.Vertical:
                    index = Mathf.Abs(m_rect.localPosition.y) / m_Interval;
                    break;
                default:
                    break;
            }
            //向下滑动
            if (index-m_curMinIndex>=2) 
            {
                //滑动过快
                if(index-m_curMinIndex>m_maxShowCount)
                {
                    //还没有到底
                    if(index<=count-m_maxShowCount)
                    {
                        for(int i=0;i<m_List.Count;i++)
                        {
                            var tempItem=m_List[i];
                            tempItem.transform.localPosition = GetPosition((int)(i + index));
                            tempItem.Set(tempItem.Id, (uint)(i + index));
                        }
                        m_curMaxIndex = (uint)index + m_maxShowCount;
                        m_curMinIndex= (uint)index;
                    }
                    else  //到底
                    {
                        int idx = (int)(count - m_maxShowCount);
                        for (int i = 0; i < m_List.Count; i++)
                        {
                            var tempItem = m_List[i];
                            tempItem.transform.localPosition = GetPosition(i+idx);
                            tempItem.Set(tempItem.Id, (uint)(i+idx));
                        }
                        m_curMaxIndex = count;
                        m_curMinIndex = (uint)idx;
                    }
                }else if(m_curMaxIndex<count)
                {
                    int indexId = 0;
                    var item = m_List[indexId];
                    m_List.RemoveAt(indexId);
                    item.transform.localPosition = GetPosition((int)m_curMaxIndex);
                    item.Set(m_curMinIndex, (uint)m_curMaxIndex);
                    m_curMaxIndex += 1;
                    m_curMinIndex += 1;
                    m_List.Add(item);
                }
            }else if(m_curMinIndex-index>=0)
            {
                if(m_curMinIndex-index>=m_maxShowCount)
                {
                    //滑动过快 直接设置
                    for(int i=0;i<m_List.Count;i++)
                    {
                        var item= m_List[i];
                        item.transform.localPosition = GetPosition((int)(i + index));
                        item.Set(item.Id, (uint)(i + index));
                    }
                    m_curMinIndex = (uint)index;
                    m_curMaxIndex = m_curMinIndex + m_maxShowCount;
                }else if(m_curMinIndex>0)
                {
                    int indexId = (int)(m_List.Count-1);
                    var item=m_List[indexId];
                    m_List.RemoveAt(indexId);
                    m_curMinIndex -= 1;
                    m_curMaxIndex -= 1;
                    item.transform.localPosition = GetPosition((int)(m_curMinIndex));
                    item.Set(item.Id, (uint)(m_curMinIndex));
                    m_List.Insert((int)0, item);
                }
            }
        }
    }

    /// <summary>
    /// 根据总索引获取位置
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private Vector3 GetPosition(int index)
    {
        Vector3 pos = Vector3.zero;
        switch(Dirction)
        {
            case Dirction.Horizontal:
                pos = new Vector3((index * m_Interval), Item.localPosition.y);
                break;
            case Dirction.Vertical:
                pos = new Vector3(Item.localPosition.x, -(index * m_Interval));
                break;
        }
        return pos;
    }

    /// <summary>
    /// 跳到指定索引的内容位置
    /// index=0时，跳到头部；index>count-maxShowCount时 跳到底部
    /// </summary>
    /// <param name="index"></param>
    public void Jump(uint index)
    {
        if(index<0 || index>=this.count)
        {
            Debug.LogError("Index out of Range");
            return;
        }
        if(index>=this.count-m_maxShowCount+2)  //跳到底部
        {
            index = this.count - m_maxShowCount+2;
            m_curMinIndex = (uint)index-2;
            m_curMaxIndex = m_curMinIndex + m_maxShowCount;
            m_rect.localPosition = new Vector3(-GetPosition((int)index+1).x, -GetPosition((int)index+1).y);
            for(int i=0;i<m_List.Count;i++)
            {
                var tempItem = m_List[i];
                tempItem.transform.localPosition = GetPosition((int)(i + m_curMinIndex));
                tempItem.Set(tempItem.Id, (uint)(i + m_curMinIndex));
            }
        }
        else
        {
            m_curMinIndex = index;
            m_curMaxIndex = m_curMinIndex + m_maxShowCount;
            m_rect.localPosition = new Vector3(-GetPosition((int)index).x, -GetPosition((int)index).y);
            for (int i = 0; i < m_List.Count; i++)
            {
                var tempItem = m_List[i];
                tempItem.transform.localPosition = GetPosition((int)(i + index));
                tempItem.Set(tempItem.Id, (uint)(i + index));
            }
        }
        
    }
}

/// <summary>
/// 滑动方向
/// </summary>
public enum Dirction
{
    //横向
    Horizontal,
    //纵向
    Vertical,
}
