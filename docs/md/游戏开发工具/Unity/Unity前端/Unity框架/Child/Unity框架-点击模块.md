# Unity框架-点击模块

开发通用UI事件监听器

PEListener.cs

```C#
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PEListener :
    MonoBehaviour,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler {
    public Action<PointerEventData, object[]> onClick;
    public Action<PointerEventData, object[]> onClickDown;
    public Action<PointerEventData, object[]> onClickUp;
    public Action<PointerEventData, object[]> onDrag;

    public object[] args = null;

    public void OnPointerClick(PointerEventData eventData) {
        onClick?.Invoke(eventData, args);
    }
    public void OnPointerDown(PointerEventData eventData) {
        onClickDown?.Invoke(eventData, args);
    }
    public void OnPointerUp(PointerEventData eventData) {
        onClickUp?.Invoke(eventData, args);
    }
    public void OnDrag(PointerEventData eventData) {
        onDrag?.Invoke(eventData, args);
    }
}
```

WindowRoot.cs

```C#
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowRoot : MonoBehaviour {

    private T GetOrAddComponent<T>(GameObject go) where T : Component {
        T t = go.GetComponent<T>();
        if(t == null) {
            t = go.AddComponent<T>();
        }
        return t;
    }

    protected void OnClick(GameObject go, Action<PointerEventData, object[]> clickCB, params object[] args) {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClick = clickCB;
        if(args != null) {
            listener.args = args;
        }
    }
    protected void OnClickDown(GameObject go, Action<PointerEventData, object[]> clickDownCB, params object[] args) {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickDown = clickDownCB;
        if(args != null) {
            listener.args = args;
        }
    }
    protected void OnClickUp(GameObject go, Action<PointerEventData, object[]> clickUpCB, params object[] args) {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickUp = clickUpCB;
        if(args != null) {
            listener.args = args;
        }
    }
    protected void OnDrag(GameObject go, Action<PointerEventData, object[]> dragCB, params object[] args) {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onDrag = dragCB;
        if(args != null) {
            listener.args = args;
        }
    }
}
```

使用

SelectWindow.cs

```C#
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectWindow : WindowRoot {
    public int heroCount;
    public GameObject heroItem;
    public Transform scrollRoot;
    public Button btnSure;

    void Start() {
        for(int i = 0; i < heroCount; i++) {
            GameObject go = Instantiate(heroItem);
            go.transform.SetParent(scrollRoot);
            go.name = "heroItem_" + i;
            go.transform.localScale = Vector3.one;
            OnClickDown(go, ClickItemDown, go, i);
            OnClickUp(go, ClickItemUp, go, i);
            OnDrag(go, DragItem, go, i);
        }

        btnSure.onClick.AddListener(ClickSureBtn);
    }

    private void ClickItemDown(PointerEventData ped, object[] args) {
        GameObject go = args[0] as GameObject;
        int index = (int)args[1];
        Debug.Log("ClickDown:" + go.name + " index:" + index);
    }
    private void ClickItemUp(PointerEventData ped, object[] args) {
        GameObject go = args[0] as GameObject;
        int index = (int)args[1];
        Debug.Log("ClickUp:" + go.name + " index:" + index);
    }
    private void DragItem(PointerEventData ped, object[] args) {
        GameObject go = args[0] as GameObject;
        int index = (int)args[1];
        Debug.Log("Drag:" + go.name + " index:" + index);
    }

    public void ClickSureBtn() {
        Debug.Log("Click Sure Button.");
    }
}
```
