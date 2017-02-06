@echo off
call "%vs140comntools%vsvars32.bat"

cd ..\..\
packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:"%VSINSTALLDIR%Common7\IDE\MSTest.exe" -targetargs:"/noresults /noisolation /testcontainer:""FredsImageMagickScripts.NET.Tests\bin\Release\FredsImageMagickScripts.NET.Tests.dll" -register:user -threshold:10 -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:All -output:.\FredsImageMagickScripts.NET.Coverage.xml

SET PATH=C:\\Python34;C:\\Python34\\Scripts;%PATH%
pip install codecov
codecov -f "FredsImageMagickScripts.NET.Coverage.xml"