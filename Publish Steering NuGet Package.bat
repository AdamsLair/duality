:: search for msbuild and make it available
set bb.build.msbuild.exe=
for /D %%D in (%SYSTEMROOT%\Microsoft.NET\Framework\v4*) do set msbuildPath=%%D
set PATH=%PATH%;%msbuildPath%

:: make sure we have a clean release build
msbuild /t:Rebuild /p:Configuration=Release /p:Platform="Any CPU" Duality.sln

:: remove any existing nupkg files
del *.nupkg

:: build the nuget packages
.nuget\nuget pack DualityPlugins\Steering\Steering.csproj -Properties Configuration=Release;Platform=AnyCPU

:: upload the nuget packages
:: .nuget\nuget push *.nupkg

:: remove nupkg files after uploading them
:: del *.nupkg
