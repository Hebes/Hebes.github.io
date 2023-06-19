# VSCoede配置

**[VSCode插件市场](<https://marketplace.visualstudio.com/>)**

**转换中文:**

Chinese (Simplified) (简体中文) Language Pack for Visual Studio Code

**画图软件:**

Draw.io Integration
![画图软件](\Image\QQ截图20230318234626.png)

**Markdown:**

- Markdown All in One
- Markdown Preview Enhanced
- markdownlint
- vscode all markdown

**Todo工具:**

Todo Tree

## VSCode的一些小操作(四)——自定义折叠代码区域

参考链接:[VSCode的一些小操作(四)——自定义折叠代码区域](<https://blog.csdn.net/ycx60rzvvbj/article/details/106447130>)

当我们的代码量很多时
我们会把一块一块的代码折叠起来
就像这样：
![1](<https://img-blog.csdnimg.cn/20200530205311246.png>)
我们只需要点击父盒子那一行左边的折叠按钮，就可以将整个父盒子这一块的代码进行折叠：
![2](<https://img-blog.csdnimg.cn/20200530205339143.png?>)
至于每个语言要怎么写这个东西，可以参考这张表↓
![3](<https://img-blog.csdnimg.cn/20200530210132181.png>)

## 一些实用插件

1 Auto-Using for C#    自动添加引用

2 C#

3 C# XML Documentation Comments     注释/// ，你懂得

4 Chinese    汉化

5 Debugger for Unity    调试unity

6 Unity Code Snippets    代码补全

7 Unity Tools   Unity工具

原文链接：<https://www.cnblogs.com/hanjun0612/p/14074850.html>

## 主命令框

F1 或 Ctrl+Shift+P（俗称万能键）  ：打开命令面板。在打开的输入框内，可以输入任何命令,如下图(图片较大，如果查看不清晰，可以在图片上右键 “在新的标签页中打开图片”，查看原图，下同)：

例如：

```javascript {.line-numbers}
按一下 Backspace 会进入到 Ctrl+P 模式
在 Ctrl+P 下输入 > 可以进入 Ctrl+Shift+P 模式
在 Ctrl+P 窗口下还可以直接输入文件名，跳转到该文件
```

在 Ctrl+P 模式下输入 “?” 会弹出下拉菜单

常用的如下：

```javascript {.line-numbers}
?   列出当前可执行的动作
!   显示 Errors或 Warnings，也可以 Ctrl+Shift+M
:   跳转到行数，也可以 Ctrl+G 直接进入
@    跳转到 symbol（搜索变量或者函数），也可以 Ctrl+Shift+O 直接进入
@    根据分类跳转 symbol，查找属性或函数，也可以 Ctrl+Shift+O 后输入" : "进入
#   根据名字查找 symbol，也可以 Ctrl+T
```

## 常用快捷键

**编辑器与窗口管理:**

```javascript {.line-numbers}
新建文件:   Ctrl+N
文件之间切换:   Ctrl+Tab
打开一个新的VS Code编辑器:    Ctrl+Shift+N
关闭当前窗口:   Ctrl+W
关闭当前的VS Code编辑器:   Ctrl+Shift+W
切出一个新的编辑器窗口（最多3个):   Ctrl+\
切换左中右3个编辑器窗口的快捷键:   Ctrl+1  Ctrl+2  Ctrl+3
```

**格式调整:**

```javascript {.line-numbers}
代码行向左或向右缩进:   Ctrl+[ 、 Ctrl+]
复制或剪切当前行/当前选中内容:   Ctrl+C 、 Ctrl+V
代码格式化:   Shift+Alt+F
向上或向下移动一行:   Alt+Up 或 Alt+Down
向上或向下复制一行:   Shift+Alt+Up 或 Shift+Alt+Down
在当前行下方插入一行:   Ctrl+Enter
在当前行上方插入一行:   Ctrl+Shift+Enter
注释：  ctrl+/，取消同理    alt+shift+A，取消同理。
```

**光标相关:**

```javascript {.line-numbers}
移动到行首:   Home
移动到行尾:   End
移动到文件结尾:   Ctrl+End
移动到文件开头:   Ctrl+Home
移动到定义处:   F12
VScode中快速选择当前一整行Ctrl + L
查看定义处缩略图(只看一眼而不跳转过去):    Alt+F12
选择从光标到行尾的内容:   Shift+End
选择从光标到行首的内容： Shift+Home
删除光标右侧的所有内容(当前行):   Ctrl+Delete
扩展/缩小选取范围： Shift+Alt+Right 和 Shift+Alt+Left
多行编辑(列编辑):   Alt+Shift+鼠标左键 或 Ctrl+Alt+Down/Up
同时选中所有匹配编辑(与当前行或选定内容匹配):   Ctrl+Shift+L
下一个匹配的也被选中:   Ctrl+D
回退上一个光标操作:   Ctrl+U
撤销上一步操作: Ctrl+Z
手动保存:   Ctrl+S
```

**查找替换:**

```javascript {.line-numbers}
查找:   Ctrl+F
查找替换:   Ctrl+H
```

**显示相关:**

```javascript {.line-numbers}
全屏显示(再次按则恢复):   F11
放大或缩小(以编辑器左上角为基准):   Ctrl +/-
侧边栏显示或隐藏： Ctrl+B
显示资源管理器(光标切到侧边栏中才有效):   Ctrl+Shift+E
显示搜索(光标切到侧边栏中才有效):   Ctrl+Shift+F
显示(光标切到侧边栏中才有效):   Git Ctrl+Shift+G
显示 Debug:    Ctrl+Shift+D
显示 Output:    Ctrl+Shift+U
```

**其他设置:**

```javascript {.line-numbers}
自动保存：File -> AutoSave(中文界面下“文件”->“自动保存”) 或者 Ctrl+Shift+P，输入 auto
```

**scode忽略打开某些文件或文件夹的设置:**

1.在VS Code中点击快捷键Ctrl/Command+Shift+P，输入setting，会出现很多选项，选择User Settings那一项；
2.放入如下配置：

```javascript {.line-numbers}
 "files.exclude": {
        "*.metadata": true,
        "*.settings": true,
        "**/*.pyc": true, //隐藏所有pyc文件
        "**/*.meta": true, //隐藏所有meta文件
        "**/*.prefab": true, //隐藏所有prefab文件
        "**/*.d.ts": true, //隐藏所有d.ts文件
        "**/*.fnt": true,
        "**/*.sln": true,
        "**/*.csproj": true,
        "**/.vscode": true,
        "**/*.anim": true,
        "**/*.controller": true,
        "**/Logs": true,
        "**/[Pp]lugins": true, //使用中括号时表示不区分大小写
        "**/[Mm]aterials": true,
        "**/[Aa]nimation": true,
        "**/[Pp]ackages": true,
        "**/[Tt]extures": true,
        "**/[Ss]cenes": true,
        "**/temp": true, //隐藏temp文件夹
        "**/library": true, //隐藏library文件夹
        "**/audio": true, //隐藏audio文件夹
        "**/video": true, //隐藏video文件夹
    }
```

**文章来源:**

<https://www.cnblogs.com/schut/p/10461840.html>
