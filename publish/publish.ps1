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

function addFileElement($xml, $src, $target)
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

function createNuspecFiles()
{
    & cmd /c 'git log -1 --date=format:"2.%Y.%m%d.%H%M" --format="%ad" > version.txt 2> nul'
    $version = [IO.File]::ReadAllText("version.txt").Trim()
    Remove-Item "version.txt"

    foreach ($script in $scripts)
    {
        $nuspecFile = FullPath "publish\nuspec\$($script.id).nuspec"

        $title = "Fred's ImageMagick Script " + $script.name

        $path = FullPath "publish\ImageMagick.FredsScripts.NET.nuspec"
        $xml = [xml](Get-Content $path)
        $xml.package.metadata.id = $script.id
        $xml.package.metadata.version = $version
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
        $xml.package.metadata.copyright = "Copyright Dirk Lemstra, Fred Weinhaus"
        $xml.package.metadata.tags = "Fred Weinhaus ImageMagick " + $script.name

        $path = FullPath "src\ImageMagick.FredsScripts.NET\$($script.path)"
        foreach ($file in Get-ChildItem $path -Filter "$($script.name)*")
        {
            $file = "$($script.path)\$file"
            addFileElement $xml "..\..\src\ImageMagick.FredsScripts.NET\$file" "Content\ImageMagick.FredsScripts\$file"
        }

        $xml.Save($nuspecFile)
        Write-Host "Created: $nuspecFile"
    }
}

function createNuGetPackages()
{
  foreach ($script in $scripts)
  {
    $nuspecFile = FullPath "publish\nuspec\$($script.id).nuspec"
    $dir = FullPath "publish\nupkg"

    ..\tools\windows\nuget.exe pack $nuspecFile -NoPackageAnalysis -OutputDirectory $dir
    checkExitCode "Failed to create NuGet package"
  }
}

$scripts = loadScripts

createNuspecFiles $scripts
createNuGetPackages $scripts
