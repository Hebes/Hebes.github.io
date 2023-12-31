# -*- coding: utf-8 -*-
# coding=utf-8
# coding: utf-8
from ast import While
from distutils.log import debug
from email.mime import image
import io
import os
from re import I;
import sys
from turtle import st
# from PyQt5.QtWidgets import *   #pip install PyQt5  http://www.3qphp.com/python/pybase/4095.html https://www.q578.com/s-5-2457567-0/
from operator import truediv
import sys
from base64 import decodebytes
from time import sleep, time
import win32gui;
import win32ui;
import win32con;
import pymouse;
import time;
import pytesseract;#文字识别库 pip install pytesseract https://www.cnblogs.com/chengxuyuanaa/p/12986233.html 出问题的结局办法： https://blog.csdn.net/qq_39208536/article/details/81252700
# import pyautogui
from pymouse import PyMouse;
from PIL import Image;
from PIL import ImageGrab
import math
import operator
from functools import reduce


#改变标准输出的默认编码
sys.stdout=io.TextIOWrapper(sys.stdout.buffer,encoding='utf8')
m = PyMouse()
#设置游戏界面的宽和高
width =900;
height=700;
wdname = u'《梦幻西游》手游'

#m.click(1268,291)

# 通过鼠标获取爽口句柄名称
def OnGetWindowtitleOfMouse():
    POS =win32gui. GetCursorPos();
    edit=win32gui.WindowFromPoint(POS)
    print("窗口编号是:"+edit)

#鼠标拖拽 改变因鼠标产生的偏离
def OnMouseClick():
    m.press(width-5,height-5,1);
    time.sleep(.2)
    print("睡眠.1秒执行")
    m.release(width-1,height-1)
    

#找到窗口句柄
def OnGetWindowtitle():
    # 取得窗口句柄 窗口类名 窗口标题名
    hwnd  = win32gui.FindWindow(0,wdname)
    if not hwnd:
        print("窗口找不到，请确认窗口句柄名称：【%s】" % wdname )
        exit()
    if(win32gui.IsIconic(hwnd)):
        win32gui.ShowWindow(hwnd, 9)
    win32gui.MoveWindow(hwnd,0,0,900,700,True)
    print("窗口找到，请确认窗口句柄名称：【%s】" % wdname)
    # 窗口显示最前面 
    win32gui.SetForegroundWindow(hwnd)
    #移动某窗口hld到指定位置。句柄 屏幕左上角距离 长和高 是否重绘
    OnMouseClick();

#获取当前路径
def OnGetPath():
    root = os.getcwd()
    print(root);
    return root;

# 图片识别
def read_image(ImageName):
    text=pytesseract.image_to_string(Image.open(ImageName+".png"),lang='chi_sim') #设置为中文文字的识别
    return text;

# pyqt截图
# def Screenshots():
#     hwnd = win32gui.FindWindow(None, wdname)
#     app = QApplication(sys.argv)
#     screen = QApplication.primaryScreen()
#     img = screen.grabWindow(hwnd).toImage()
#     img.save("screenshot.png")
    
# 截图ImageGrab
def Screenshots1(name,x1,y1,x2,y2):
    size = (x1,y1,x2,y2)
    img = ImageGrab.grab(size)
    img.save(name+".png")
    # img.show()

# 图片比对
def image_contrast(img1, img2):#结果越小越相似
    image1 = Image.open(img1)
    image2 = Image.open(img2)
    h1 = image1.histogram()
    h2 = image2.histogram()
    result = math.sqrt(reduce(operator.add, list(map(lambda a,b: (a-b)**2, h1, h2)))/len(h1) )
    return result

# 师门任务
def shimen():
    time.sleep(.2)
    # 滑动任务栏
    # m.press(800,350);
    # time.sleep(.4)
    # m.move(781,500)
    # m.click(781,500)
    # a=0
    # while a<2:
    #     time.sleep(3)
    #     pyautogui.moveTo(800, 350, 0)
    #     pyautogui.dragRel(0, 150, 0)
    #     a+=1
    # pyautogui.moveTo(800, 246)
    # pyautogui.dragRel(0, 200, .3)
    
# -------------------------------------------------------捉鬼
zhuaguiOpen=True;#
#抓鬼
def zhuagui():
    print("-------------------开始抓鬼--------------")
    while True:
        if Check_OnCombat():
            print("在战斗中")
        else:
            print("没有在战斗中")
            if(CheckQuest_zhaugui()):#检查抓鬼任务
                print("正在进行抓鬼任务")
        
    
#判断抓鬼任务是否存在
def CheckQuest_zhaugui():
    sleep(1);
    Screenshots1("zhuogui",702,166,893,239);print("-------截图完毕------")#截图任务栏的抓鬼
    str = read_image("zhuogui");
    print("截图内容是："+str);
    is_contanin = "捉鬼" in str;
    if is_contanin:
        print("抓鬼任务存在");
        sleep(3);m.click(773,200,1);#持续点击，防止中途不动
    else:
        print("抓鬼任务不存在");
        GetQuest_Zhuagui();sleep(1);#领取抓鬼任务
        m.click(773,200,2);sleep(0.5);m.click(773,200,1);#点击进行抓鬼
        return True;
    return is_contanin;

#检查是否在战斗中
def Check_OnCombat():
    # Screenshots1("matching/zhandou",838,632,890,686)#匹配的图片首先要保存一次  此代码不能删除
    Screenshots1("recognition/zhandou",838,632,890,686)#匹配的图片首先要保存一次
    isOnCombat = image_contrast("recognition/zhandou.png","matching/zhandou.png")
    if isOnCombat<50:
        print("相似度是："+str(isOnCombat));
        print("不在战斗中")
        return False;
    print("在战斗中")
    return True;

#领取抓鬼任务
def GetQuest_Zhuagui():
    m.click(36,69,1); sleep(1);#点击世界地图
    m.click(415,415,1);sleep(1);#点击长安城
    m.click(130,71,1);sleep(1);#点击小地图
    m.click(261,383,1);sleep(8);#点击钟馗
    m.click(730,294,1);#领取抓鬼任务
    print("抓鬼任务领取成功")
    
# 滑动任务列表
def SlidingTaskList(args):
    print("滑动任务列表")
    
# ------------------------------------------------------------
def Test():
    image = Image.open("11.png")
    image_l = image.convert('L')
    print(image_l.mode)
    image.show()
    
if __name__ == '__main__':
    OnGetWindowtitleOfMouse();
    # OnGetWindowtitle() #窗口变化
    # Check_OnCombat();#检查是否在战斗中
    # CheckQuest_zhaugui();
    # zhuagui()#抓鬼
    # Screenshots1("2")
    # shimen();
    # result= image_contrast('333.png','444.png');
    # print(result)
    # Test();