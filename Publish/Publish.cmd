@echo off
call "%vs140comntools%vsvars32.bat"
C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Unrestricted ..\Tools\Scripts\Publish.ps1
pause
