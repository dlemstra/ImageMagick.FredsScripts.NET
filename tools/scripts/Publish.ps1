#==================================================================================================
# Copyright 2017 Dirk Lemstra, Fred Weinhaus
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
#==================================================================================================
$scriptPath = Split-Path -parent $MyInvocation.MyCommand.Path
. $scriptPath\Shared\Functions.ps1
SetFolder $scriptPath

. Tools\Scripts\Shared\Build.ps1

$github = "https://github.com/dlemstra/FredsImageMagickScripts.NET"

function AddFileElement($xml, $src, $target)
{
  $files = $xml.package.files

  if (!($files))
  {
    $files = $xml.CreateElement("files", $xml.DocumentElement.NamespaceURI)
    [void] $xml.package.AppendChild($files)
  }

  $file = $xml.CreateElement("file", $xml.DocumentElement.NamespaceURI)

  $srcAtt = $xml.CreateAttribute("src")
  $srcAtt.Value = $src
  [void] $file.Attributes.Append($srcAtt)

  $targetAtt = $xml.CreateAttribute("target")
  $targetAtt.Value = $target
  [void] $file.Attributes.Append($targetAtt)

  [void] $files.AppendChild($file)
}

function CreateNuGetPackages()
{
  foreach ($script in $scripts)
  {
    $archived = FullPath "..\FredsImageMagickScripts.NET.Archive\$($script.id).$($script.version).nupkg"
    if (Test-Path $archived)
    {
      Write-Host "$($script.id) is already published."
      continue
    }

    $nuspecFile = FullPath "Publish\Nuspec\$($script.id).nuspec"
    $dir = FullPath "Publish\Nupkg"

    .\Tools\Programs\NuGet.exe pack $nuspecFile -NoPackageAnalysis -OutputDirectory $dir
    CheckExitCode "Failed to create NuGet package"
  }
}

function CreateNuspecFiles()
{
  foreach ($script in $scripts)
  {
    $nuspecFile = FullPath "Publish\Nuspec\$($script.id).nuspec"
    if (Test-Path $nuspecFile)
    {
      $xml = [xml](Get-Content $nuspecFile)
      $script.version = $xml.package.metadata.version
      continue
    }

    $title = "Fred's ImageMagick Script " + $script.name

    $path = FullPath "Publish\FredsImageMagickScripts.NET.nuspec"
    $xml = [xml](Get-Content $path)
    $xml.package.metadata.id = $script.id
    $xml.package.metadata.version = $script.version
    $xml.package.metadata.title = $title
    $xml.package.metadata.authors = "Dirk Lemstra, Fred Weinhaus"
    foreach ($collaborator in $collaborators)
    {
      if ($collaborator.script -eq $script.name)
      {
        $xml.package.metadata.authors += ", " + $collaborator.name
      }
    }
    $xml.package.metadata.summary = $title + " (requires Magick.NET)"
    $xml.package.metadata.description = $script.description
    $xml.package.metadata.copyright = "Copyright 2015-$((Get-Date).year) Dirk Lemstra, Fred Weinhaus"
    $xml.package.metadata.tags = "Fred Weinhaus ImageMagick " + $script.name

    $path = FullPath "src\FredsImageMagickScripts.NET\$($script.path)"
    foreach ($file in Get-ChildItem $path -Filter "$($script.name)*")
    {
      $file = "$($script.path)\$file"
      AddFileElement $xml "..\..\src\FredsImageMagickScripts.NET\$file" "Content\FredsImageMagickScripts\$file"
    }

    $xml.Save($nuspecFile)
    Write-Host "Created: $nuspecFile"
  }
}

function LoadCollaborators()
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

function LoadScripts()
{
  $path = FullPath "src\FredsImageMagickScripts.NET\bin\Release\FredsImageMagickScripts.NET.xml"
  $documentation = ([xml](Get-Content $path)).doc.members

  $utcNow = [System.DateTime]::Now.ToUniversalTime()
  $build = ($utcNow.Date - (New-Object DateTime(2000, 1, 1)).Date).TotalDays
  $revision = [int]($utcNow.TimeOfDay.TotalSeconds / 2)

  $assembly = [System.Reflection.Assembly]::LoadFrom("..\src\FredsImageMagickScripts.NET\bin\Release\FredsImageMagickScripts.NET.dll")
  $types = $assembly.GetTypes() | Where { $_.Name.EndsWith("Script") }

  $scripts = @()

  foreach ($type in $types)
  {
    $name = $type.Name.Replace("Script", "")
    $file = Get-ChildItem -Filter "$($type.Name).cs" -Recurse
    $path = FullPath "src\FredsImageMagickScripts.NET"
    $path = $file.FullName.SubString($path.Length + 1)
    $path = Split-Path -parent $path
    $summary = $documentation.SelectSingleNode("member[@name='T:FredsImageMagickScripts.$($type.Name)']").summary.Trim()
    $summary = [System.Text.RegularExpressions.Regex]::Replace($summary, '\s+', ' ')

    $scripts += [pscustomobject]@{
      id = "FredsImageMagickScripts.$name"
      version = "1.0.$build.$revision"
      name = $name
      description = $summary
      path = $path
      url = "$github/tree/master/src/FredsImageMagickScripts.NET/" + $path.Replace("\", "/")
    }
  }

  return $scripts | Sort-Object name
}

function Test()
{
  $dll = "tests\FredsImageMagickScripts.NET.Tests\bin\Release\FredsImageMagickScripts.NET.Tests.dll"
  vstest.console /inIsolation /platform:x64 $dll
  CheckExitCode ("Test failed for FredsImageMagickScripts.NET.Tests.dll")
}

function UpdateReadme()
{
$content = @"
# FredsImageMagickScripts.NET

[![GitHub license](https://img.shields.io/badge/license-Fred%20Weinhaus-green.svg)](https://github.com/dlemstra/FredsImageMagickScripts.NET/blob/master/LICENSE.md)
[![Build Status](https://github.com/dlemstra/FredsImageMagickScripts.NET/workflows/master/badge.svg)](https://github.com/dlemstra/FredsImageMagickScripts.NET/actions)

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

Build
Test

$scripts = LoadScripts
$collaborators = LoadCollaborators

CreateNuspecFiles
UpdateReadme
CreateNuGetPackages
