@echo off

set /p ApiKey=<api.key.txt
if not %ERRORLEVEL% == 0 goto done

echo Are you sure?
pause

for /r %%i in (*.nupkg) do ..\..\Tools\windows\nuget.exe push %%i %ApiKey% -src nuget.org

:done
pause
