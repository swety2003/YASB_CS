%1 mshta vbscript:CreateObject("Shell.Application").ShellExecute("cmd.exe","/c %~s0 ::","","runas",1)(window.close)&&exit
cd /d %~dp0
cd ..\build\Plugins\Debug
mklink /D "TestPlugin" "net6.0-windows10.0.19041.0"

pause