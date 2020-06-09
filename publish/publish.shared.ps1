# Copyright 2017-2020 Dirk Lemstra, Fred Weinhaus
# <https://github.com/dlemstra/FredsImageMagickScripts.NET>
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

. $PSScriptRoot\..\tools\windows\utils.ps1

$github = "https://github.com/dlemstra/FredsImageMagickScripts.NET"

function loadScripts()
{
    $path = FullPath "src\FredsImageMagickScripts.NET\bin\Release\net40\FredsImageMagickScripts.NET.xml"
    $documentation = ([xml](Get-Content $path)).doc.members

    $suffix = "Script``1"
    $assembly = [System.Reflection.Assembly]::LoadFrom("..\src\FredsImageMagickScripts.NET\bin\Release\net40\FredsImageMagickScripts.NET.dll")
    $types = $assembly.GetTypes() | Where { $_.Name.EndsWith($suffix) }

    $src = FullPath "src\FredsImageMagickScripts.NET"

    $scripts = @()
    foreach ($type in $types)
    {
        $name = $type.Name.Replace($suffix, "")
        $file = Get-ChildItem -Path $src -Filter "$($name)Script.cs" -Recurse
        $path = (Split-Path -Parent $file.FullName)
        $path = $path.Replace("\", "/").Split("/") | Select -Last 2
        $path = $path -Join "/"
        $summary = $documentation.SelectSingleNode("member[@name='T:FredsImageMagickScripts.$($type.Name)']").summary.Trim()
        $summary = [System.Text.RegularExpressions.Regex]::Replace($summary, '\s+', ' ')

        $scripts += [pscustomobject]@{
            id = "FredsImageMagickScripts.$name"
            name = $name
            description = $summary
            path = $path
            url = "$github/tree/master/src/FredsImageMagickScripts.NET/$path"
        }
    }

    return $scripts | Sort-Object name
}