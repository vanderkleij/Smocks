function Get-Builds
{
	$builds = @(
		@{Name = "Smocks.Net40"; FinalDir="Net40"; NuGetDir = "net40"; },
		@{Name = "Smocks.Net45"; FinalDir="Net45"; NuGetDir = "net45"; }
	)
	
	return $builds
}