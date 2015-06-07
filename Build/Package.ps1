# Credits to Json.NET for parts of this packaging script.
. .\Common.ps1

$majorVersion = "0.1"
$majorWithReleaseVersion = "0.1.0"

$packageId = "Smocks"

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$baseDir  = resolve-path "$scriptPath\.."
$sourceDir = "$baseDir\Source"
$buildDir = "$baseDir\Build"
$workingDir = "$buildDir\Work"

$configuration = "Debug"
$builds = Get-Builds

function Get-VersionFromSolutionInfo() {
	$solutionInfo = gc "$sourceDir\SolutionInfo.cs"
	$group = $solutionInfo | select-string '^\[assembly: AssemblyVersion\("(.*)"\)\]' | % {$_.Matches} | % {$_.Groups[1].Value}
	return $group
}

function Package([string]$version) {
	$nugetVersion = $version

	if (Test-Path -path $workingDir)
	{
		Write-Host "Deleting existing working directory $workingDir"
		
		del $workingDir -Recurse -Force
	}
	
	foreach ($build in $builds)
	{
		$finalDir = $build.FinalDir
		
		robocopy "$sourceDir\Smocks\bin\$configuration\$finalDir" $workingDir\Package\Bin\$finalDir *.dll *.pdb *.xml /NFL /NDL /NJS /NC /NS /NP /XO /XF *.CodeAnalysisLog.xml | Out-Default
	}
	
	New-Item -Path $workingDir\NuGet -ItemType Directory

	$nuspecPath = "$workingDir\NuGet\Smocks.nuspec"
	Copy-Item -Path "$buildDir\Smocks.nuspec" -Destination $nuspecPath -recurse

	Write-Host "Updating nuspec file at $nuspecPath" -ForegroundColor Green
	Write-Host

	$xml = [xml](Get-Content $nuspecPath)
	Edit-XmlNodes -doc $xml -xpath "//*[local-name() = 'id']" -value $packageId
	Edit-XmlNodes -doc $xml -xpath "//*[local-name() = 'version']" -value $nugetVersion

	Write-Host $xml.OuterXml

	$xml.save($nuspecPath)
	
	foreach ($build in $builds)
    {
		if ($build.NuGetDir) {
			$name = $build.TestsName
			$finalDir = $build.FinalDir
			$frameworkDirs = $build.NuGetDir.Split(",")
        
			foreach ($frameworkDir in $frameworkDirs)
			{
				robocopy "$sourceDir\$packageId\bin\$configuration\$finalDir" $workingDir\NuGet\lib\$frameworkDir *.dll *.pdb *.xml /NFL /NDL /NJS /NC /NS /NP /XO /XF *.CodeAnalysisLog.xml | Out-Default
			}
		}
	}
	
	robocopy $sourceDir $workingDir\NuGet\src *.cs /S /NFL /NDL /NJS /NC /NS /NP /XD Smocks.Tests Smocks.TestConsole obj | Out-Default

    Write-Host "Building NuGet package with ID $packageId and version $nugetVersion" -ForegroundColor Green
	Write-Host $nuspecPath
    Write-Host

    & "$buildDir\NuGet.exe" pack $nuspecPath -Symbols
    move -Path .\*.nupkg -Destination $workingDir\NuGet
}

function Edit-XmlNodes {
    param (
        [xml] $doc,
        [string] $xpath = $(throw "xpath is a required parameter"),
        [string] $value = $(throw "value is a required parameter")
    )
    
    $nodes = $doc.SelectNodes($xpath)
    $count = $nodes.Count

    Write-Host "Found $count nodes with path '$xpath'"
    
    foreach ($node in $nodes) {
        if ($node -ne $null) {
            if ($node.NodeType -eq "Element")
            {
                $node.InnerXml = $value
            }
            else
            {
                $node.Value = $value
            }
        }
    }
}

$version = Get-VersionFromSolutionInfo
Package($version)