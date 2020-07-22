@echo Executing build with default.ps1 configuration
@echo off

IF "%2"=="" (SET assemblyVersion=1.0.0.0) ELSE (SET assemblyVersion=%2)

powershell.exe -NoProfile -ExecutionPolicy bypass -Command "& {.\configure-build.ps1 }"
powershell.exe -NoProfile -ExecutionPolicy bypass -Command "& {invoke-psake .\default.ps1 %1 -parameters @{"project_name"="'PivotalServices.WebApiTemplate.CSharp'";"assemblyVersion"="'%assemblyVersion%'"}; exit !($psake.build_success) }"