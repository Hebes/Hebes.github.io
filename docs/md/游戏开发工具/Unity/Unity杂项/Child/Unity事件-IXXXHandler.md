# Unity EventSystem IXXXHandler事件详解

## 参考

**本文链接:<https://www.ngui.cc/el/1799165.html?action=onClick>**

**官方文档链接：<http://docs.unity3d.com/460/Documentation/Manual/SupportedEvents.html>**

**部分内容参考：<https://blog.csdn.net/lyh916/article/details/44570503>**

Unity自己的事件系统开放了一些接口，我们只需要实现这些接口就行。但是这个接口所代表的含义需要我们深入理解。

**示例查看路径：<https://docs.unity3d.com/cn/2017.4/ScriptReference/EventSystems.IDragHandler.html>**

UnityEngine->UnityEngine.EventSystems->Interface

## 类

**基类:**

`IEventSystemHandler`基类，所有 EventSystem 事件都继承自该类。

**Pointer鼠标类:**

`IPointerClickHandler`在同一物体上按下并释放

`IPointerDownHandler` 指针按下

`IPointerEnterHandler`指针进入

`IPointerExitHandler`指针退出

`IPointerUpHandler`指针释放(可能按下时的指针位置跟释放时的指针位置不同，这里指的是按下时指针指着的物体)

**Drag&Drop拖拽类:**

`IDropHandler`拖拽结束(拖拽结束后的位置(即鼠标位置)如果有物体，则那个物体调用)

`IBeginDragHandler`鼠标是否开始拖拽

`IDragHandler`拖拽中

`IEndDragHandler`拖拽结束(被拖拽的物体调用)

`IInitializePotentialDragHandler`拖拽时的初始化，跟IPointerDownHandler差不多，在按下时调用

**Select点选类:**

`IDeselectHandler`物体从选中到取消选中时

`ISelectHandler`物体被选中时(EventSystem.current.SetSelectedGameObjec(gameObject))

`IUpdateSelectedHandler`被选中的物体每帧调用

**Input输入类：**

`IMoveHandler`物体移动时(与InputManager里的Horizontal和Vertica按键相对应)，前条件是物体被选中

`ICancelHandler`取消按钮被按下时(与InputManager里的Cancel按键相对应，PC上默认的Esc键)，前提条件是物体被选中

`IScrollHandler`滚轮滚动

`ISubmitHandler`提交按钮被按下时(与InputManager里的Submit按键相对应，PC上默认的Enter键)，前提条件是物体被选中

## 详解

**指针进入**
IPointerEnterHandler - OnPointerEnter - Called when a pointer enters the object

```C#
public void OnPointerEnter(PointerEventData eventData);
```

**指针退出**
IPointerExitHandler - OnPointerExit - Called when a pointer exits the object

```C#
public void OnPointerExit(PointerEventData eventData);
```

**鼠标按下，鼠标抬起**
这个对于鼠标的正键与反键都会响应，在接口的实现中注意用input.getmouse这个来判断当前按下的是鼠标正键还是反键。

需要注意的是，一个对象同时实现了按下抬起两个接口，当鼠标在这个物体上按下触发了按下事件，鼠标不松开的情况下移到另个实现了 抬起接口的物体上，在鼠标抬起的时候，触发的抬起事件对应的仍是第一个点击的物体的。

*指针按下*
IPointerDownHandler - OnPointerDown - Called when a pointer is pressed on the object

```C#
public void OnPointerDown(PointerEventData eventData);
```

*指针释放(可能按下时的指针位置跟释放时的指针位置不同，这里指的是按下时指针指着的物体)*
IPointerUpHandler - OnPointerUp - Called when a pointer is released (called on the original the pressed object)

```C#
public void OnPointerUp(PointerEventData eventData);
```

**同一物体上点击**
IPointerClickHandler - OnPointerClick - Called when a pointer is pressed and released on the same object
在同一物体上按下并释放

```C#
public void OnPointerClick(PointerEventData eventData);
```

**拖拽时的初始化，跟IPointerDownHandler差不多，在按下时调用**
IInitializePotentialDragHandler - OnInitializePotentialDrag - Called when a drag target is found, can be used to initialise values

```C#
public void OnInitializePotentialDrag(PointerEventData eventData);
```

**开始拖拽**
IBeginDragHandler - OnBeginDrag - Called on the drag object when dragging is about to begin

```C#
public void OnBeginDrag(PointerEventData eventData);
```

**拖拽中**
IDragHandler - OnDrag - Called on the drag object when a drag is happening

```C#
public void OnDrag(PointerEventData eventData);
```

**拖拽结束(被拖拽的物体调用)**
IEndDragHandler - OnEndDrag - Called on the drag object when a drag finishes

```C#
public void OnEndDrag(PointerEventData eventData);
```

**拖拽结束(拖拽结束后的位置(即鼠标位置)如果有物体，则那个物体调用)**
IDropHandler - OnDrop - Called on the object where a drag finishes

```C#
public void OnDrop(PointerEventData eventData);
```

**滚轮滚动**
IScrollHandler - OnScroll - Called when a mouse wheel scrolls

```C#
public void OnScroll(PointerEventData eventData);
```

**被选中的物体每帧调用**
IUpdateSelectedHandler - OnUpdateSelected - Called on the selected object each tick

```C#
public void OnUpdateSelected(BaseEventData eventData);
```

**物体被选中时(EventSystem.current.SetSelectedGameObject(gameObject))**
ISelectHandler - OnSelect - Called when the object becomes the selected object

```C#
public void OnSelect(BaseEventData eventData);
```

**物体从选中到取消选中时**
IDeselectHandler - OnDeselect - Called on the selected object becomes deselected

```C#
public void OnDeselect(BaseEventData eventData);
```

**物体移动时(与InputManager里的Horizontal和Vertica按键相对应)，前提条件是物体被选中**
IMoveHandler - OnMove - Called when a move event occurs (left, right, up, down, ect)

```C#
public void OnMove(AxisEventData eventData);
```

**提交按钮被按下时(与InputManager里的Submit按键相对应，PC上默认的是Enter键)，前提条件是物体被选中**
ISubmitHandler - OnSubmit - Called when the submit button is pressed

```C#
public void OnSubmit(BaseEventData eventData);
```

**取消按钮被按下时(与InputManager里的Cancel按键相对应，PC上默认的是Esc键)，前提条件是物体被选中**
ICancelHandler - OnCancel - Called when the cancel button is pressed

```C#
public void OnSubmit(BaseEventData eventData);
```

## 总结

1. 共有17个接口方法
2. 其中OnMove使用的参数是AxisEventData eventData
3. 其中OnUpdateSelected、OnSelect 、OnDeselect 、OnSubmit 、OnCancel 使用的参数是BaseEventData eventData
4. 其余使用的参数是PointerEventData eventData
