# -*- coding: utf-8 -*-
'''****************************************************
	文件：trans.py
	作者：Plane
	邮箱: 1785275942@qq.com
    官网：www.qiqiker.com
	功能：封装控制台日志显示
*****************************************************'''

logSwitch = True

import os
import time

from prt_cmd_color import printGreen, printRed, printYellow, printYellowRed


def LogMsg(msg):
    if (logSwitch):
        nt = time.strftime("%H:%M:%S", time.localtime())
        txt = nt + " >" + msg
        print(txt)


def LogInfo(msg):
    if (logSwitch):
        nt = time.strftime("%H:%M:%S", time.localtime())
        txt = nt + " >" + msg
        printGreen(txt)


def LogWarn(msg):
    if (logSwitch):
        nt = time.strftime("%H:%M:%S", time.localtime())
        txt = nt + " >" + msg
        printYellow(txt)


def LogError(msg):
    if (logSwitch):
        nt = time.strftime("%H:%M:%S", time.localtime())
        txt = nt + " >" + msg
        printRed(txt)


def LogSpot(msg):
    if (logSwitch):
        nt = time.strftime("%H:%M:%S", time.localtime())
        txt = nt + " >" + msg
        printYellowRed(txt)


if __name__ == '__main__':
    LogSpot("spot msg from planezhong.")
    LogMsg("log msg from planezhong.")
    LogInfo("info msg from planezhong.")
    LogWarn("warn msg from planezhong.")
    LogError("error msg from planezhong.")