@echo off

chcp 65001 % UTF-8 %
title 快捷打开应用程序
echo CPU信息 %PROCESSOR_ARCHITECTURE:~%
echo CPU品牌 %PROCESSOR_ARCHITECTURE:~0,3%
echo CPU位数 %PROCESSOR_ARCHITECTURE:~3%
echo 系统版本信息
ver 
color bf

start "" "D:/Program Files/PixPin/PixPin.exe"
TIMEOUT /T 1

start "" "D:\Program Files\zz_v2rayN-With-Core-SelfContained\v2rayN.exe"
TIMEOUT /T 1

start "" "D:\Program Files\Tencent\WeChat\WeChat.exe"
TIMEOUT /T 1

start "" "D:\Program Files (x86)\NetEase\CloudMusic\cloudmusic.exe"
TIMEOUT /T 1

start "" "D:\Program Files\Unity Hub\Unity Hub.exe"