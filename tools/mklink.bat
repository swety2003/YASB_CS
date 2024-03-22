cd /d %~dp0
cd ..\build
gsudo mklink /D "x64\Debug\net6.0-windows10.0.19041.0\Plugins\TestPlugin" "..\..\..\..\Plugins\Debug\net6.0-windows10.0.19041.0"
pause