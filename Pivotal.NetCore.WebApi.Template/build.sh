#!/bin/bash
echo Executing build with default.ps1 configuration
pwsh -NoProfile -ExecutionPolicy bypass -Command "& {.\configure-build.ps1 }"
pwsh -NoProfile -ExecutionPolicy bypass -Command "& {Invoke-psake .\default.ps1 $1 -parameters @{'project_name'='Pivotal.NetCore.WebApi.Template'}; exit !('$psake.build_success') }"
