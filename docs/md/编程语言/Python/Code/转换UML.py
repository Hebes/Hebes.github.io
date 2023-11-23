from tkinter import *

TitleDict = {'public': "+",'protected': "-",'private': "-",'///': "///",}
TitleDict1 = {'///': "///",}


# 替换修饰
def CheckTitle(input : str):
    for key in TitleDict:
       input = input.replace(key,TitleDict[key])
    return input

def ReplaceBracket(input : str):
    T12 = input.split('{{')
    return T12(0)
    
# 创建一个窗口
def CreatWindow():
    top = Tk()  #通过Tk()方法建立一个根窗口
    top.title("自定义窗口")
    top.geometry('500x300')
    top.mainloop()  #进入等待处理窗口事件

def UMLHelp():
    file = open("text.txt", "r+",encoding="utf-8")
    line = file.readline()
    content=""
    newStr=""
    while line:
        line = file.readline()
        strLine = str(line.strip())
        if(strLine.endswith('>')):
            continue
        for key in TitleDict:
            if(strLine.startswith(key)):
                content += strLine+"\r\n"#.replace(key,TitleDict[key])+"\r\n"
    print(content)
    file.close()
    
def Enum():
    file = open("text.txt", "r+",encoding="utf-8")
    line = file.readline()
    content=""
    newStr=""
    while line:
        line = file.readline()
        strLine = str(line.strip())
        if(strLine.endswith('>')):
            continue
        content += strLine+"\r\n"#.replace(key,TitleDict[key])+"\r\n"
    print(content)
    file.close()

if __name__ == "__main__":
    # CreatWindow()
#    UMLHelp()
   Enum()
    