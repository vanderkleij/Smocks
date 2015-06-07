param(
	[string]$configuration="Debug"
)

. .\Common.ps1

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$baseDir  = resolve-path "$scriptPath\.."
$sourceDir = "$baseDir\Source"
$targets = "Clean", "Build"
$targetsOption = "/t:" + [string]::Join(",", $targets)

$msbuild = (Get-Command msbuild.exe -errorAction SilentlyContinue | Select -Property Definition).Definition

if (-Not $msbuild) {
	Write-Host "MSbuild not found!"
	return
}

# Manual cleaning, since the "clean" target does not seem to delete contents of the 'obj' folder generated
# by the solutions for other framework versions.
Write-Host "Cleaning solution..."
Get-ChildItem -include "obj","bin" -recurse $sourceDir | ?{ $_.PSIsContainer } | foreach ($_) {Remove-Item -Recurse -Force -ErrorVariable capturedErrors -ErrorAction SilentlyContinue $_.fullname}

foreach ($build in Get-Builds)
{
	$name = $build.Name
	
	& $msbuild "$sourceDir\$name.sln" "/property:Configuration=$configuration" $targetsOption
}