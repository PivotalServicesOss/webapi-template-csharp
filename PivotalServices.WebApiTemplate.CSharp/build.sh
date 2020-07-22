#!/bin/bash
if [$2 -eq ""] 
then
	assemblyVersion="1.0.0.0" 
else 
	assemblyVersion=$2
fi
app_name="PivotalServices.WebApiTemplate.CSharp"
echo Executing build with default.ps1 configuration
pwsh -NoProfile -ExecutionPolicy bypass -Command "& {.\configure-build.ps1 }"
pwsh -NoProfile -ExecutionPolicy bypass -Command "& {Invoke-psake .\default.ps1 $1 -parameters @{'project_name'='$app_name'; \"assemblyVersion\"=\"$assemblyVersion\"}; exit !((GET-Variable psake -valueOnly).build_success) }"