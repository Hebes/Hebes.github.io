# Python-windows标题

可用于获取当前最顶层的标题，如果python自身窗口在最前，那么获取到的就是python自身窗口的标题。

需要安装pywin32模块 ，代码如下：

```Python
import win32gui as w
title = w.GetWindowText (w.GetForegroundWindow())
print(title)
```

```Python
import win32gui

current_window = win32gui.GetForegroundWindow()
classname = win32gui.GetClassName(current_window)
title = win32gui.GetWindowText(current_window)
print(f'classname:{classname} title:{title}')
```
