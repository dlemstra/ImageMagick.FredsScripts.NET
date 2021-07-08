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

. $PSScriptRoot\publish.shared.ps1

function loadCollaborators()
{
    $path = FullPath "Publish\Collaborators.txt"
    $lines = Get-Content $path

    $collaborators = @()

    foreach ($line in $lines)
    {
        $info = $line.Split("|")

        $collaborators += [pscustomobject]@{
            script = $info[0]
            name = $info[1]
            link = $info[2]
        }
    }

    return $collaborators | Sort-Object name
}

function updateReadme($scripts, $collaborators)
{
    $content = @"
# ImageMagick.FredsScripts.NET.NET

[![GitHub license](https://img.shields.io/badge/license-Fred%20Weinhaus-green.svg)](https://github.com/dlemstra/ImageMagick.FredsScripts.NET/blob/master/LICENSE.md)
[![Build Status](https://github.com/dlemstra/ImageMagick.FredsScripts.NET/workflows/master/badge.svg)](https://github.com/dlemstra/ImageMagick.FredsScripts.NET/actions)

This projects goal is to port most of the scripts for ImageMagick that are created by [Fred Weinhaus](http://www.fmwconcepts.com/imagemagick/) to C#. With the help of [Magick.NET](https://github.com/dlemstra/Magick.NET) a library will be created that will make it easy to use Fred's scripts in .NET.

## License
This project uses the same license as Fred's ImageMagick Scripts. You can find the license in the [LICENSE.md]($github/blob/master/LICENSE.md) file.

## Scripts
The scripts below have been ported to .NET and can be found on NuGet. Not all scripts have been ported at the moment. Create an issue if you want a specific script to be ported to C#.

Script | Download | Original
--- | --- | ---
"@

    foreach ($script in $scripts)
    {
        $content += "`r`n" + "[" + $script.name + "](" + $script.url + ")"
        $content += "|[download](https://www.nuget.org/packages/" + $script.id + "/)"
        $content += "|[original](http://www.fmwconcepts.com/imagemagick/" + $script.name.toLower() + "/)"
    }

    $content += @"


## Collaborators
Script | Name
--- | ---
"@

    foreach ($script in $scripts)
    {
        $first = $true

        foreach ($collaborator in $collaborators)
        {
            if ($collaborator.script -eq $script.name)
            {
                if ($first)
                {
                    $content += "`r`n" + "[" + $script.name + "](" + $script.url + ")|"
                    $first = $false
                }
                else
                {
                    $content += "<br>"
                }

                $content += "[" + $collaborator.name + "](" + $collaborator.link + ")"
            }
        }
    }

    $path = FullPath "README.md"
    $content | Out-File -filepath $path
}

$scripts = loadScripts
$collaborators = loadCollaborators

updateReadme $scripts $collaborators
