@echo Executing build with default.ps1 configuration
@echo off
powershell.exe -NoProfile -ExecutionPolicy bypass -Command "& {.\configure-build.ps1 }"
powershell.exe -NoProfile -ExecutionPolicy bypass -Command "& {invoke-psake .\default.ps1 %1 -parameters @{"project_name"="'Pivotal.NetCore.WebApi.Template'"}; exit !($psake.build_success) }"