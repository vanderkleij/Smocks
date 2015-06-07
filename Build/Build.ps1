# Credits to Json.NET for parts of this script.
param(
	[string]$configuration="Debug"
)

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$baseDir  = resolve-path "$scriptPath\.."
$buildDir = "$baseDir\Build"
$sourceDir = "$baseDir\Source"
$targets = "Clean", "Build"
$targetsOption = "/t:" + [string]::Join(",", $targets)

. $buildDir\Common.ps1

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
	
	Write-Host
    Write-Host "Restoring"
    [Environment]::SetEnvironmentVariable("EnableNuGetPackageRestore", "true", "Process")
    & $buildDir\NuGet.exe update -self
    & $buildDir\NuGet.exe restore "$sourceDir\$name.sln" | Out-Default
	
	& $msbuild "$sourceDir\$name.sln" "/property:Configuration=$configuration" $targetsOption
}