#!/bin/bash
echo Executing build with default.ps1 configuration
pwsh -NoProfile -ExecutionPolicy bypass -Command "& {.\configure-build.ps1 }"
pwsh -NoProfile -ExecutionPolicy bypass -Command "& {Invoke-psake .\default.ps1 $1 -parameters @{'project_name'='PivotalServices.WebApiTemplate.CSharp'}; exit !('$psake.build_success') }"
