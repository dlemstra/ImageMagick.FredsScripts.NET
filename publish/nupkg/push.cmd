@echo off

set /p ApiKey=<ApiKey.txt
if not %ERRORLEVEL% == 0 goto done

echo Are you sure?
pause

for /r %%i in (*.nupkg) do ..\..\Tools\Programs\nuget.exe push %%i %ApiKey% -src nuget.org
for /r %%i in (*.nupkg) do move %%i ..\..\..\FredsImageMagickScripts.NET.Archive

:done
pause