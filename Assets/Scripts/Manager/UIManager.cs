using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{


    //protected Transform transform = null;
    //UI容器，存放已经展示的UI界面
    private Dictionary<string, UIWindowBase> windows = new Dictionary<string, UIWindowBase>();

    public Transform noneTrans;

    public Transform basicTrans;
    public Transform normalTrans;
    public Transform topTrans;

    private void Awake()
    {
        InitTrans();
    }

    public void InitTrans()
    {
        if(GameObject.FindGameObjectWithTag("NoneUI")!=null)
        {
            noneTrans = GameObject.FindGameObjectWithTag("NoneUI").GetComponent<Transform>();
        }
        if (GameObject.FindGameObjectWithTag("BasicUI") != null)
        {
            basicTrans = GameObject.FindGameObjectWithTag("BasicUI").GetComponent<Transform>();
        }
        if (GameObject.FindGameObjectWithTag("NormalUI") != null)
        {
            normalTrans = GameObject.FindGameObjectWithTag("NormalUI").GetComponent<Transform>();
        }
        if(GameObject.FindGameObjectWithTag("TopUI")!=null)
        {
            topTrans = GameObject.FindGameObjectWithTag("TopUI").GetComponent<Transform>();
        }
    }

    /// <summary>
    /// 展示界面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T ShowWindow<T>() where T : UIWindowBase
    {
        UIWindowBase window = FindWindow<T>();
        //实例化出来的ui界面
        GameObject ins = null;
        //window已经存在
        if (window != null)
        {
            //UI已经在展示
            if (window.State == UIState.Open)
            {
                return null;
            }
            window.Show();
            window.gameObject.SetActive(true);
            return window as T;
        }
        else
        {
            GameObject go = null;
            GameManager.Instance.ResManager.LoadUI(typeof(T).Name, (obj) =>
            {
                go = obj as GameObject;
                go.name = typeof(T).Name;
            });
            //当前UI不存在
            if (go == null)
            {
                return null;
            }
            window = go.GetComponent<UIWindowBase>();
            switch (window.uiType)
            {
                case UIType.None:
                    ins = GameObject.Instantiate(window.gameObject, this.noneTrans);
                    break;
                case UIType.Basic:
                    ins = GameObject.Instantiate(window.gameObject, this.basicTrans);
                    break;
                case UIType.Normal:
                    ins = GameObject.Instantiate(window.gameObject, this.normalTrans);
                    break;
                case UIType.Top:
                    ins = GameObject.Instantiate(window.gameObject, this.topTrans);
                    break;
            }
            ins.name = typeof(T).Name;
            ins.SetActive(true);
            UIWindowBase result = ins.GetComponent<UIWindowBase>();
            result.Show();
            if(windows.ContainsKey(typeof(T).Name))
            {
                windows[typeof(T).Name] = result;
            }
            else
            {
                windows.Add(typeof(T).Name, result);
            }
            return result as T;
        }
    }

    /// <summary>
    /// 隐藏界面
    /// </summary>
    /// <param name="window"></param>
    public void HideWindow(UIWindowBase window)
    {
        if (window == null)
        {
            return;
        }
        window.Hide();
        window.gameObject.SetActive(false);
    }

    /// <summary>
    /// 销毁界面
    /// </summary>
    /// <param name="window"></param>
    public void CloseWindow(UIWindowBase window)
    {
        if (window == null)
        {
            return;
        }
        window.Close();
        Destroy(window.gameObject);
        windows.Remove(window.name);
    }

    /// <summary>
    /// 关闭所有界面
    /// </summary>
    public void CloseAllWindow()
    {
        foreach (var window in windows.Values)
        {
            window.Close();
            Destroy(window.gameObject);
        }
        windows.Clear();
    }

    /// <summary>
    /// 界面是否展示
    /// </summary>
    /// <param name="window"></param>
    /// <returns></returns>
    public bool IsShow(UIWindowBase window)
    {
        return (window.gameObject.activeSelf && window.State == UIState.Open);
    }

    /// <summary>
    /// 查找界面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T FindWindow<T>() where T : UIWindowBase
    {
        UIWindowBase window = null;
        windows.TryGetValue(typeof(T).Name, out window);
        return window as T;
    }

    /// <summary>
    /// 将指定窗口移动到当前ui类型的顶端
    /// </summary>
    /// <param name="window">需要移动的界面</param>
    public bool MoveTop(UIWindowBase window)
    {
        //移动的窗口为空或者未激活
        if (window == null || !window.gameObject.activeSelf || window.State != UIState.Open)
        {
            return false;
        }
        window.transform.SetAsLastSibling();
        return true;
    }
}
