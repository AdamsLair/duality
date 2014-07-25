:: upload the nuget packages
for /r %%i in (Other\NightlyBuild\NuGetPackages\*.nupkg) do .nuget\nuget push %%i

:: remove nupkg files after uploading them
for /r %%i in (Other\NightlyBuild\NuGetPackages\*.nupkg) do del %%i
