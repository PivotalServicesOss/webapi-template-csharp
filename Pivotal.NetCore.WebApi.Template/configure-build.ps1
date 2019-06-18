# pull dependencies
$has_psake = Get-Module -ListAvailable | Select-String -Pattern "Psake" -Quiet
if(-Not($has_psake)) {
	#install psake
	Write-Host "No Psake Module Found: Installing now" -ForegroundColor Red
	Set-PSRepository -Name PSGallery -InstallationPolicy Trusted
	Install-Module Psake -Scope CurrentUser
}
