cd %~dp0./../../Build/NightlyBuild/

NightlyBuilder.exe NoDocs=true NoBuild=true NoTests=true NonInteractive=true NoCleanNugetPackageTargetDir=true

if %ERRORLEVEL% NEQ 0 (
  echo NightlyBuilder.exe returned with error code %ERRORLEVEL%
)
exit /b %ERRORLEVEL%