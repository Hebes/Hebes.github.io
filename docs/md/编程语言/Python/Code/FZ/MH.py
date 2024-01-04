# -*- coding: utf-8 -*-
# coding=utf-8
# coding: utf-8
from tkinter import *
import io
import sys
import win32gui

# 改变标准输出的默认编码
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf8')
# # 通过Tk()方法建立一个根窗口
# top = Tk()
# top.title("助手")
# # 进入等待处理窗口事件
# top.mainloop()


# def OnGetHandle():
#     # title = w.GetWindowText(w.GetForegroundWindow())
#     # print("获取窗口："+title)
#     hWnd_list = []
#     win32gui.EnumWindows(lambda hWnd, param: param.append(hWnd), hWnd_list)
#     for i in hWnd_list:
#         title =w.GetWindowText(i)
#         print(hWnd_list)
#     return hWnd_list


# import win32gui

# windows_list = []
# win32gui.EnumWindows(lambda hWnd, param: param.append(hWnd), windows_list)
# for window in windows_list:
#     classname = win32gui.GetClassName(window)
#     title = win32gui.GetWindowText(window)
#     print(f'classname:{classname} 标题:{title}')

# 通过鼠标获取爽口句柄名称
def OnGetTitleOfMouse():
    POS =win32gui.GetCursorPos();
    edit=win32gui.WindowFromPoint(POS)
    title = win32gui.GetWindowText(edit)
    print("窗口编号是:"+ title)
    
if __name__ == '__main__':
    # OnGetHandle()
    OnGetTitleOfMouse()