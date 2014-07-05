:: upload the nuget packages
:: for /r %%i in (Other\NightlyBuild\*.nupkg) do .nuget\nuget push %%i

:: remove nupkg files after uploading them
:: for /r %%i in (Other\NightlyBuild\*.nupkg) do del %%i
