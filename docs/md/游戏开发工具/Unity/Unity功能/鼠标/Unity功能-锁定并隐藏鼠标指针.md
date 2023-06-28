# Unity功能-锁定并隐藏鼠标指针

```C#
public cLass GameManager
{
    #region锁定/隐藏鼠标
    public static bool LockCursor
    {
        get => Cursor .LockState == CursorLockMode. Locked;
        set
        {
            Cursor.visibLe = !vaLue;
            Cursor. LockState = value ? CursorLockMode. Locked : CursorLockMode . None;
        }
    }
    #endregion 
}
```

调用

```C#
using UnityEngine;

pubtic class HideLockCursor : MonoBehaviour
{
    pubtic bool isLocked = true;

    private void Update()
    {
        if(Input.GetKeyDown (KeyCode.Escape))
        {
            isLocked = !isLocked;
        }
        GameManager.LockCursor = isLocked;
    }
}
```
