# Copyright Dirk Lemstra, Fred Weinhaus
# <https://github.com/dlemstra/ImageMagick.FredsScripts.NET>
#
# Licensed under the ImageMagick License (the "License"); you may not use this file except in
# compliance with the License. You may obtain a copy of the License at
#
#   http://www.imagemagick.org/script/license.php
#
# Unless required by applicable law or agreed to in writing, software distributed under the
# License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
# express or implied. See the License for the specific language governing permissions and
# limitations under the License.

. $PSScriptRoot\..\..\tools\windows\utils.ps1

function testImageMagickFredsScriptsNET() {
    $vstest = "$($env:VSINSTALLDIR)\Common7\IDE\Extensions\TestPlatform\vstest.console.exe"

    $folder = fullPath "tests\ImageMagick.FredsScripts.NET.Tests\bin\x64\Test\net45"
    $fileName = "$folder\ImageMagick.FredsScripts.NET.Tests.dll"

    & $vstest $fileName /platform:x64 /TestAdapterPath:$folder

    CheckExitCode("Failed to test ImageMagick.FredsScripts.NET")
}

testImageMagickFredsScriptsNET