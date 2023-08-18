:: 代码页更改为Unicode(UTF-8)
chcp 65001

@REM echo OFF
@REM cd /d C:\Program Files\Google\Chrome\Application\chrome.exe
@REM start chrome.exe 127.0.0.1:8000

echo 请用浏览器打开127.0.0:8000
cd /d %~dp0
cd docs & python -m http.server



pause