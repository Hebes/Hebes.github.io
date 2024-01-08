# Unity组件-ToggleGroup

```C#
public ToggleGroup toggleGroup;

private void Start()
{
    //toggleGroup.GetComponentsInChildren<Toggle>().ToList().ForEach(x => 
    //{
    //    x.onValueChanged.AddListener(y => 
    //    {
    //        if (y) Debug.Log(x.name);
    //    });
    //});
    toggleGroup.GetComponentsInChildren<Toggle>().ToList().ForEach(x =>
    {
        x.onValueChanged.AddListener(y =>
        {
            switch (x.name)//遍历Toggle名字  分不同情况执行不同事件
            {
                case "A":
                    Debug.Log(1); break;
                case "B":
                    Debug.Log(2); break;
                case "C":
                    Debug.Log(3); break;
                default: break;
            }
        });
    }); 
}
```
