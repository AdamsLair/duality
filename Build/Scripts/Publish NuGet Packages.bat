cd %~dp0./../../

:: upload the nuget packages
for /r %%i in (Build\NightlyBuild\NuGetPackages\*.nupkg) do nuget push %%i -Source "https://nuget.org"

:: remove nupkg files after uploading them
for /r %%i in (Build\NightlyBuild\NuGetPackages\*.nupkg) do del %%i
