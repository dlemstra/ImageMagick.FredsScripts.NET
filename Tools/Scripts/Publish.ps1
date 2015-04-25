#==================================================================================================
$scriptPath = Split-Path -parent $MyInvocation.MyCommand.Path
. $scriptPath\Shared\Functions.ps1
SetFolder $scriptPath
#==================================================================================================
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
#==================================================================================================
function CreateNuGetPackages()
{
	Foreach ($script in $scripts)
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
#==================================================================================================
function CreateNuspecFiles()
{
	Foreach($script in $scripts)
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
		$xml.package.metadata.summary = $title + " (requires Magick.NET)"
		$xml.package.metadata.description = $script.description
		$xml.package.metadata.tags = "Fred Weinhaus ImageMagick " + $name

		AddFileElement $xml "..\..\FredsImageMagickScripts.NET\$($script.path)" "Content\FredsImageMagickScripts\$($script.path)"

		$xml.Save($nuspecFile)
		Write-Host "Created: $nuspecFile"
	}
}
#==================================================================================================
function LoadScripts()
{
	$path = FullPath "FredsImageMagickScripts.NET\bin\Release\FredsImageMagickScripts.NET.xml"
	$documentation = ([xml](Get-Content $path)).doc.members

	$utcNow = [System.DateTime]::Now.ToUniversalTime()
	$build = ($utcNow.Date - (New-Object DateTime(2000, 1, 1)).Date).TotalDays
	$revision = [int]($utcNow.TimeOfDay.TotalSeconds / 2)

	$assembly = [System.Reflection.Assembly]::LoadFrom("..\FredsImageMagickScripts.NET\bin\Release\FredsImageMagickScripts.NET.dll")
	$types = $assembly.GetTypes() | Where { $_.Name.EndsWith("Script") }

	$scripts = @()

	Foreach ($type in $types)
	{
		$name = $type.Name.Replace("Script", "")
		$file = Get-ChildItem -Filter "$($type.Name).cs" -Recurse
		$path = FullPath "FredsImageMagickScripts.NET"
		$path = $file.FullName.SubString($path.Length + 1)
		$summary = $documentation.SelectSingleNode("member[@name='T:FredsImageMagickScripts.$($type.Name)']").summary.Trim()
		$summary = [System.Text.RegularExpressions.Regex]::Replace($summary, '\s+', ' ')

		$scripts +=	[pscustomobject]@{
			id = "FredsImageMagickScripts.$name"
			version = "1.0.$build.$revision"
			name = $name
			description = $summary
			path = $path
			url = "https://github.com/dlemstra/FredsImageMagickScripts.NET/tree/master/FredsImageMagickScripts.NET/" + $path.Replace("\", "/")
		}
	}

	return $scripts | Sort-Object name
}
#==================================================================================================
function UpdateReadme()
{
$content = @"
#FredsImageMagickScripts.NET
This projects goal is to port most of the scripts for ImageMagick that are created by [Fred Weinhaus](http://www.fmwconcepts.com/imagemagick/) to C#. With the help of [Magick.NET](https://magick.codeplex.com) a library will be created that will make it easy to use Fred's scripts in .NET.

## License
This project uses the same license as Fred's ImageMagick Scripts. You can find the license in the [LICENSE.md](https://github.com/dlemstra/FredsImageMagickScripts.NET/blob/master/LICENSE.md) file.

## Scripts
The following scripts have been ported to .NET and can be found on NuGet.

 | |
--- | ---
"@

	Foreach($script in $scripts)
	{
		$content += "`r`n" + '[' + $script.name + '](' + $script.url + ')|[download](https://www.nuget.org/packages/' + $script.id + '/)'
	}

	$path = FullPath "README.md"
	$content | Out-File -filepath $path
}
#==================================================================================================
$scripts = LoadScripts
#==================================================================================================
CreateNuspecFiles
UpdateReadme
CreateNuGetPackages
#==================================================================================================
