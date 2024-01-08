# Unity组件-EventTrigger

**[Unity中EventTrigger的几种使用方法](<https://blog.csdn.net/qq_45152631/article/details/107388530>)**

```C#
/// <summary>
/// 扩展的方法
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// 为EventTrigger的事件类型绑定Action方法
    /// </summary>
    /// <param name="trigger">EventTrigger组件对象</param>
    /// <param name="eventType">事件类型</param>
    /// <param name="listenedAction">要执行的方法</param>
    public static void AddListener(this EventTrigger trigger, EventTriggerType eventType, Action<PointerEventData> listenedAction)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(data => listenedAction.Invoke((PointerEventData)data));
        trigger.triggers.Add(entry);
    }
}
```

```C#
go.GetComponent<EventTrigger>()
    .AddListener(EventTriggerType.PointerEnter, (PointerEventData eventData) =>
    {
        Debug.Log($"当前Enter的物体：{p.seriseNo}");
        endImage = p.go;
    });
```
