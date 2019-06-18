$script:project_config = "Release"

properties {

  $solution_name = "Pivotal.NetCore.WebApi.Template"
  $domain = ""
  $environment = "Development"
  $app_name = "Pivotal.NetCore.WebApi.Template"
  $release_id = "linux-x64"

  $base_dir = resolve-path .
  $project_dir = "$base_dir\src\$project_name"
  $project_file = "$project_dir\$project_name.csproj"
  $solution_file = "$base_dir\$solution_name.sln"
  $publish_dir = "$base_dir\publish"

  $version = get_version
  $date = Get-Date
  $dotnet_exe = get-dotnet

  $ReleaseNumber =  $version
  
  Write-Host "**********************************************************************"
  Write-Host "Release Number: $ReleaseNumber"
  Write-Host "**********************************************************************"
  

  $packageId = if ($env:package_id) { $env:package_id } else { "$solution_name" }
}
   
#These are aliases for other build tasks. They typically are named after the camelcase letters (rd = Rebuild Databases)
task default -depends InitialPrivateBuild
task dev -depends DeveloperBuild
task ci -depends IntegrationBuild
task ? -depends help
task test -depends RunTests
task pp -depends Publish-Push
task publish_notest_push -depends SetReleaseBuild, Clean, Publish, push


task help {
   Write-Help-Header
   Write-Help-Section-Header "Comprehensive Building"
   Write-Help-For-Alias "(default)" "Intended for first build or when you want a fresh, clean local copy"
   Write-Help-For-Alias "dev" "Optimized for local dev"
   Write-Help-For-Alias "ci" "Continuous Integration build (long and thorough) with packaging"
   Write-Help-For-Alias "test" "Run local tests"
   Write-Help-For-Alias "pnp" "Intended for pushing to PCF"
   Write-Help-For-Alias "pp" "Intended for pushing to PCF. Will run webpack without tests"
   Write-Help-Footer
   exit 0
}

#These are the actual build tasks. They should be Pascal case by convention
task InitialPrivateBuild -depends Clean, test
task RunTests -depends Clean, UnitTests, IntegrationTests
task DeveloperBuild -depends SetDebugBuild, Clean, RunTests
task IntegrationBuild -depends SetReleaseBuild, PackageRestore, Clean, RunTests, Publish
task Publish-Push -depends SetReleaseBuild, Clean, RunTests, Publish, push

task SetDebugBuild {
    $script:project_config = "Debug"
}

task SetReleaseBuild {
    $script:project_config = "Release"
}

task SetVersion {
	set-content $base_dir\CommonAssemblyInfo.cs "// Generated file - do not modify",
	        "using System.Reflection;",
	        "[assembly: AssemblyVersion(`"$version`")]",
	        "[assembly: AssemblyFileVersion(`"$version`")]",
	        "[assembly: AssemblyInformationalVersion(`"$version`")]"
	
	Write-Host "Using version#: $version"
}

task UnitTests {
   Write-Host "******************* Now running Unit Tests *********************"
   exec { & $dotnet_exe test -c $project_config "$project_dir.UnitTests" -- xunit.parallelizeTestCollections=true }
}

task IntegrationTests {
    Write-Host "******************* Now running Integration Tests *********************"
    exec { & $dotnet_exe test -c $project_config "$project_dir.IntegrationTests" -- xunit.parallelizeTestCollections=true }
}


task Clean {
	if (Test-Path $publish_dir) {
		delete_directory $publish_dir
	}

	Write-Host "******************* Now Cleaning the Solution *********************"
    exec { & $dotnet_exe clean -c $project_config $solution_file }
}

task PackageRestore {
	Write-Host "******************* Now restoring the Solution packages *********************"
	exec { & $dotnet_exe restore $solution_file }
}

task Publish {
	Write-Host "Publishing to $publish_dir *****"
	if (!(Test-Path $publish_dir)) {
		New-Item -ItemType Directory -Force -Path $publish_dir
	}
	exec { & $dotnet_exe publish -c $project_config $project_file -o $publish_dir -r $release_id}
}

task Push {
	Push-Location $publish_dir

	Write-Host "Pushing application to PCF"
	exec { & "cf" push --var environment=$environment -n $app_name}

	Pop-Location
}

# -------------------------------------------------------------------------------------------------------------
# generalized functions for Help Section
# --------------------------------------------------------------------------------------------------------------

function Write-Help-Header($description) {
   Write-Host ""
   Write-Host "********************************" -foregroundcolor DarkGreen -nonewline;
   Write-Host " HELP " -foregroundcolor Green  -nonewline; 
   Write-Host "********************************"  -foregroundcolor DarkGreen
   Write-Host ""
   Write-Host "This build script has the following common build " -nonewline;
   Write-Host "task " -foregroundcolor Green -nonewline;
   Write-Host "aliases set up:"
}

function Write-Help-Footer($description) {
   Write-Host ""
   Write-Host " For a complete list of build tasks, view default.ps1."
   Write-Host ""
   Write-Host "**********************************************************************" -foregroundcolor DarkGreen
}

function Write-Help-Section-Header($description) {
   Write-Host ""
   Write-Host " $description" -foregroundcolor DarkGreen
}

function Write-Help-For-Alias($alias,$description) {
   Write-Host "  > " -nonewline;
   Write-Host "$alias" -foregroundcolor Green -nonewline; 
   Write-Host " = " -nonewline; 
   Write-Host "$description"
}

# -------------------------------------------------------------------------------------------------------------
# generalized functions 
# --------------------------------------------------------------------------------------------------------------
function global:delete_file($file) {
    if($file) { remove-item $file -force -ErrorAction SilentlyContinue | out-null } 
}

function global:delete_directory($directory_name)
{
  rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:get_dacDll(){
    return "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\140\Microsoft.SqlServer.Dac.dll";
}

function global:delete_files($directory_name) {
    Get-ChildItem -Path $directory_name -Include * -File -Recurse | foreach { $_.Delete()}
}

function global:get_vstest_executable($lookin_path) {
    $vstest_exe = Get-ChildItem $lookin_path -Filter Microsoft.TestPlatform* | Select-Object -First 1 | Get-ChildItem -Recurse -Filter vstest.console.exe | % { $_.FullName }
    return $vstest_exe
}

function global:get_version(){
	Write-Host "******************* Getting the Version Number ********************"
	$version = get-content "$base_Dir\..\version\number" -ErrorAction SilentlyContinue
	if ($version -eq $null) {
	    Write-Host "--------- No version found defaulting to 1.0.0 --------------------" -foregroundcolor Red
		$version = '1.0.0'
	}
	return $version
}

function global:get-dotnet(){
	return (Get-Command dotnet).Path
}
