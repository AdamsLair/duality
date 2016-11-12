cd %~dp0./../../Other/NightlyBuild/

:: delete old nupkg files, so failed packages don't generate artifacts
for /r %%i in (NuGetPackages\*.nupkg) do del %%i

NightlyBuilder.exe NoDocs=true NoBuild=true NoTests=true NonInteractive=true

if %ERRORLEVEL% NEQ 0 (
  echo NightlyBuilder.exe returned with error code %ERRORLEVEL%
)
exit /b %ERRORLEVEL%