# WIndows批处理

windows10 CMD 命令大全

```text
AT          计划在计算机上运行的命令和程序。
ATTRIB      显示或更改文件属性。
BREAK       设置或清除扩展式 CTRL+C 检查。
CACLS       显示或修改文件的访问控制列表(ACLs)。
CALL        从另一个批处理程序调用这一个。
CD          显示当前目录的名称或将其更改。
CHCP        显示或设置活动代码页数。
CHDIR       显示当前目录的名称或将其更改。
CHKDSK      检查磁盘并显示状态报告。
CHKNTFS     显示或修改启动时间磁盘检查。
CLS         清除屏幕。
CMD         打开另一个 Windows 命令解释程序窗口。
COLOR       设置默认控制台前景和背景颜色。
COMP        比较两个或两套文件的内容。
COMPACT     显示或更改 NTFS 分区上文件的压缩。
CONVERT     将 FAT 卷转换成 NTFS。您不能转换当前驱动器。
COPY        将至少一个文件复制到另一个位置。
DATE        显示或设置日期。
DEL         删除至少一个文件。
DIR         显示一个目录中的文件和子目录。
DISKCOMP    比较两个软盘的内容。
DISKCOPY    将一个软盘的内容复制到另一个软盘。
DOSKEY      编辑命令行、调用 Windows 命令并创建宏。
ECHO        显示消息，或将命令回显打开或关上。
ENDLOCAL    结束批文件中环境更改的本地化。
ERASE       删除至少一个文件。
EXIT        退出 CMD.EXE 程序(命令解释程序)。
FC          比较两个或两套文件，并显示不同处。
FIND        在文件中搜索文字字符串。
FINDSTR     在文件中搜索字符串。
FOR         为一套文件中的每个文件运行一个指定的命令。
FORMAT      格式化磁盘，以便跟 Windows 使用。
FTYPE       显示或修改用于文件扩展名关联的文件类型。
GOTO        将 Windows 命令解释程序指向批处理程序中某个标明的行。
GRAFTABL    启用 Windows 来以图像模式显示扩展字符集。
HELP        提供 Windows 命令的帮助信息。
IF          执行批处理程序中的条件性处理。
LABEL       创建、更改或删除磁盘的卷标。
MD          创建目录。
MKDIR       创建目录。
MODE        配置系统设备。
MORE        一次显示一个结果屏幕。
MOVE        将文件从一个目录移到另一个目录。
PATH        显示或设置可执行文件的搜索路径。
PAUSE       暂停批文件的处理并显示消息。
POPD        还原PUSHD保存的当前目录的上一个值。
PRINT       打印文本文件。
PROMPT      更改Windows命令提示符。
PUSHD       保存当前目录，然后对其进行更改。
RD          删除目录。
RECOVER     从有问题的磁盘恢复可读信息。
REM         记录批文件或CONFIG.SYS中的注释。
REN         重命名文件。
RENAME      重命名文件。
REPLACE     替换文件。
RMDIR       删除目录
SET         显示、设置或删除Windows环境变量。
SETLOCAL    开始批文件中环境更改的本地化。
SHIFT       更换批文件中可替换参数的位置。
SORT        对输入进行分类。
START       启动另一个窗口来运行指定的程序或命令。
SUBST       将路径跟一个驱动器号关联。
TIME        显示或设置系统时间。
TITLE       设置CMD.EXE会话的窗口标题。
TREE        以图形模式显示驱动器或路径的目录结构。
TYPE        显示文本文件的内容。
VER         显示Windows版本。
VERIFY      告诉Windows   是否验证文件是否已正确写入磁盘。
VOL         显示磁盘卷标和序列号。
XCOPY       复制文件和目录树。
appwiz.cpl  添加删除程序
control userpasswords2  用户帐户设置
cleanmgr    垃圾整理
CMD         命令提示符可以当作是Windows的一个附件，Ping，Convert这些不能在图形环境下使用的功能要借助它来完成。
cmd         jview察看Java虚拟机版本。
command.com 调用的则是系统内置的NTVDM，一个DOS虚拟机。它完全是一个类似VirtualPC的虚拟环境，和系统本身联系不大。当我们在命令提示符下运行  DOS程序时，实际上也是自动转移到NTVDM虚拟机下，和CMD本身没什么关系。
calc        启动计算器
chkdsk.exe  Chkdsk磁盘检查
compmgmt.msc    计算机管理
conf        启动netmeeting
control userpasswords2 User Account 权限设置
devmgmt.msc     设备管理器
diskmgmt.msc    磁盘管理实用程序
dfrg.msc        磁盘碎片整理程序
drwtsn32        系统医生
dvdplay         启动Media Player
dxdiag DirectX Diagnostic Tool
gpedit.msc      组策略编辑器
gpupdate /target:computer /force    强制刷新组策略
eventvwr.exe    事件查看器
explorer    打开资源管理器
logoff      注销命令
lusrmgr.msc 本机用户和组
msinfo32    系统信息
msconfig    系统配置实用程序
net start (servicename) 启动该服务
net stop (servicename)  停止该服务
notepad     打开记事本
nusrmgr.cpl 同control userpasswords，打开用户帐户控制面板
Nslookup    IP地址侦测器
oobe/msoobe /a  检查系统是否激活
perfmon.msc 计算机性能监测程序
progman     程序管理器
regedit     注册表编辑器
regedt32    注册表编辑器
regsvr32 /u *.dll   停止dll文件运行
route print     查看路由表           
rononce -p      15秒关机
rsop.msc        组策略结果集
rundll32.exe rundll32.exe %Systemroot%System32shimgvw.dll,ImageView_Fullscreen  启动一个空白的Windows图片和传真查看器
secpol.msc      本地安全策略
services.msc    本地服务设置
sfc /scannow    启动系统文件检查器
sndrec32        录音机
taskmgr         任务管理器（适用于2000／xp／2003）
tsshutdn        60秒倒计时关机命令
winchat         XP自带局域网聊天
winmsd          系统信息
winver          显示About Windows窗口
wupdmgr         Windows Update          
```

https://www.cnblogs.com/xpwi/p/9626959.html

https://cloud.tencent.com/developer/article/1760666

https://blog.csdn.net/u012815136/article/details/101549751