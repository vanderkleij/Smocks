param(
	[Parameter(Mandatory=$true)][string]$solutionName,
	[string]$configuration="Debug"
)

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$baseDir  = resolve-path "$scriptPath\.."
$sourceDir = "$baseDir\Source"
$targets = "Clean", "Build"

$msbuild = (Get-Command msbuild.exe -errorAction SilentlyContinue | Select -Property Definition).Definition

if (-Not $msbuild) {
	Write-Host "MSbuild not found!"
	return
}

# Manual cleaning, since the "clean" target does not seem to delete contents of the 'obj' folder generated
# by the solutions for other framework versions.
Write-Host "Cleaning solution..."
Get-ChildItem -include "obj","bin" -recurse $sourceDir | ?{ $_.PSIsContainer } | foreach ($_) {Remove-Item -Recurse -Force $_.fullname}

$targetsOption = "/t:" + [string]::Join(",", $targets)
Write-Host $targetsOption
& $msbuild "$sourceDir\$solutionName" "/property:Configuration=$configuration" $targetsOption