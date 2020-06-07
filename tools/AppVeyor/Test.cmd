@echo off
call "VsDevCmd.cmd"

..\Programs\NuGet.exe restore packages.config -PackagesDirectory ..\..\packages

cd ..\..\

packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:"%VSINSTALLDIR%Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" -targetargs:"/platform:x86 /inIsolation tests\FredsImageMagickScripts.NET.Tests\bin\x86\Test\net45\FredsImageMagickScripts.NET.Tests.dll" -register:user -threshold:10 -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:All -returntargetcode -output:.\FredsImageMagickScripts.NET.Coverage.xml
if %errorlevel% neq 0 exit /b %errorlevel%

SET PATH=C:\\Python34;C:\\Python34\\Scripts;%PATH%
pip install codecov
codecov -f "FredsImageMagickScripts.NET.Coverage.xml"