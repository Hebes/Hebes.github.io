# Python-pyinstaller不是内部或外部命令

使用命令pip install pyinstaller
并使用pip list检查是否安装成功

![1](\../Image/Python-pyinstaller不是内部或外部命令/1.png)

右键我的电脑-高级系统设置-环境变量-系统变量-Path

![2](\../Image/Python-pyinstaller不是内部或外部命令/2.png)

添加python的目录（不加文件名）

Python38版本后pip不会默认添加到环境变量中，在python目录找到.\Python\Python38\Scripts，打开后发现该目录中有

![3](\../Image/Python-pyinstaller不是内部或外部命令/3.png)

将该目录也按第二种方法添加到Path系统环境变量中即可！！！

![4](\../Image/Python-pyinstaller不是内部或外部命令/4.png)
