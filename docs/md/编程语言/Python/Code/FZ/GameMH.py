import os
import win32gui;
import sys
import io
import math
import operator
from pymouse import PyMouse; #解决window问题 安装pip install pyUserInput   https://blog.csdn.net/qq_41871694/article/details/111058019  https://www.cnblogs.com/sskkt7/articles/16365118.html
from time import sleep, time
from PIL import ImageGrab
import pytesseract;#文字识别库 pip install pytesseract https://www.cnblogs.com/chengxuyuanaa/p/12986233.html 出问题的结局办法： https://blog.csdn.net/qq_39208536/article/details/81252700
from PIL import Image;
from PIL import ImageGrab
from functools import reduce

#改变标准输出的默认编码
sys.stdout=io.TextIOWrapper(sys.stdout.buffer,encoding='utf8')
m = PyMouse()
#设置游戏界面的宽和高
width =900;
height=700;
wdName = u'《梦幻西游》手游'

zgOpen=True;


#找到窗口句柄
def OnGetWDtitle():
    # 取得窗口句柄 窗口类名 窗口标题名
    hwnd  = win32gui.FindWindow(0,wdName)
    if not hwnd:
        print("窗口找不到，请确认窗口句柄名称：【%s】" % wdName )
        exit()
    if(win32gui.IsIconic(hwnd)):
        win32gui.ShowWindow(hwnd, 9)
    win32gui.MoveWindow(hwnd,0,0,900,700,True)
    print("窗口找到，请确认窗口句柄名称：【%s】" % wdName)
    # 窗口显示最前面 
    win32gui.SetForegroundWindow(hwnd)
    #移动某窗口hld到指定位置。句柄 屏幕左上角距离 长和高 是否重绘
    OnMouseClick();
    
#鼠标拖拽 改变因鼠标产生的偏离
def OnMouseClick():
    m.press(width-5,height-5,1);
    sleep(.2)
    print("睡眠.1秒执行")
    m.release(width-1,height-1)
    
    
# 截图ImageGrab
def Screenshots1(name,x1,y1,x2,y2):
    size = (x1,y1,x2,y2)
    img = ImageGrab.grab(size)
    img.save(name+".png")
    # img.show()
    
# 图片识别
def read_image(ImageName):
    text=pytesseract.image_to_string(Image.open(ImageName+".png"),lang='chi_sim') #设置为中文文字的识别
    return text;

# 图片比对
def image_contrast(img1, img2):#结果越小越相似
    image1 = Image.open(img1)
    image2 = Image.open(img2)
    h1 = image1.histogram()
    h2 = image2.histogram()
    result = math.sqrt(reduce(operator.add, list(map(lambda a,b: (a-b)**2, h1, h2)))/len(h1) )
    return result
# -------------------------------------------------------捉鬼
#抓鬼
def zg():
    print("-------------------开始抓鬼--------------")
    while True:
        # sleep(2);
        if Check_OnCombat():#在战斗中
            continue;
        if Check_OnCombat()==False:#没有在战斗中
            sleep(2);
            # m.click(576,382,2)#右击
            if(CheckQuest_zg()):#检查抓鬼任务
                print("正在进行抓鬼任务")
            
            
        
    
#判断抓鬼任务是否存在
def CheckQuest_zg():
    sleep(1);
    Screenshots1("zg",702,166,893,239);print("-------截图完毕------")#截图任务栏的抓鬼
    str = read_image("zg");
    print("截图内容是：\n"+str);
    isInclude1="捉" in str;
    isInclude2="鬼" in str;
    isInclude = isInclude2 or isInclude1;
    sleep(1);
    if isInclude:
        print("抓鬼任务存在");
        sleep(3);
        m.click(773,200,1);#持续点击，防止中途不动
        m.click(773,200,2);#持续点击，防止中途不动
        return False;
    else:
        print("抓鬼任务不存在");
        if Check_OnCombat()==False:
            print("领取抓鬼任务");
            GetQuest_Zg();sleep(1);#领取抓鬼任务
            m.click(773,200,2);
            sleep(0.5);
            m.click(773,200,1);#点击进行抓鬼
        return True;

#检查是否在战斗中
def Check_OnCombat():
    # Screenshots1("matching/zd",838,632,890,686)#匹配的图片首先要保存一次  此代码不能删除
    Screenshots1("zd",837,567,882,621)#匹配的图片首先要保存一次
    isOnCombat = image_contrast("zd.png","matching/zd.png")
    if isOnCombat<35:
        print("相似度是："+str(isOnCombat));
        print("不在战斗中")
        return False;
    print("相似度是："+str(isOnCombat));
    print("在战斗中")
    return True;

#领取抓鬼任务
def GetQuest_Zg():
    m.click(36,69,1); sleep(1);#点击世界地图
    m.click(415,415,1);sleep(1);#点击长安城
    m.click(130,71,1);sleep(1);#点击小地图
    m.click(261,383,1);sleep(8);#点击钟馗
    m.click(730,294,1);#领取抓鬼任务
    print("抓鬼任务领取成功")
    
# 滑动任务列表
def SlidingTaskList(args):
    print("滑动任务列表")

if __name__ == '__main__':
    # Screenshots1("matching/zd",837,567,882,621)#匹配的图片首先要保存一次  此代码不能删除
    OnGetWDtitle() #窗口变化
    zg()#抓鬼
    # print(11);
    
    # str = read_image("zg");
    # # print(str)
    # Check_OnCombat();
    # CheckQuest_zg()