# Python-Windows应用程序信息

安装pywin32请参考：https://coco56.blog.csdn.net/article/details/102231139

获取Windows当前窗口的应用程序信息

```python
import win32gui

current_window = win32gui.GetForegroundWindow()
classname = win32gui.GetClassName(current_window)
title = win32gui.GetWindowText(current_window)
print(f'classname:{classname} title:{title}')
```

获取Windows所有窗口的应用程序信息

```python
import win32gui

windows_list = []
win32gui.EnumWindows(lambda hWnd, param: param.append(hWnd), windows_list)
for window in windows_list:
    classname = win32gui.GetClassName(window)
    title = win32gui.GetWindowText(window)
    print(f'classname:{classname} title:{title}')
```
