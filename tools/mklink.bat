
@echo off
cd /d %~dp0
cd ..\out

for /D "%cd%" %%d in (*) do (
  echo Found folder: %%d
  rem 在此处添加你需要对每个子文件夹执行的操作
)

echo 遍历完成。
gsudo mklink /D "x64\Debug\net6.0-windows10.0.19041.0\Plugins\TestPlugin" "..\..\..\..\Plugins\Debug\net6.0-windows10.0.19041.0"
pause