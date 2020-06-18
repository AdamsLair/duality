using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

using Nuke.Common.Tools.NUnit;
using static Nuke.Common.Tools.NUnit.NUnitTasks;

using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

using Nuke.Common.Tools.NuGet;
using static Nuke.Common.Tools.NuGet.NuGetTasks;
using Microsoft.Build.Exceptions;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
	/// Support plugins are available for:
	///   - JetBrains ReSharper        https://nuke.build/resharper
	///   - JetBrains Rider            https://nuke.build/rider
	///   - Microsoft VisualStudio     https://nuke.build/visualstudio
	///   - Microsoft VSCode           https://nuke.build/vscode
	///   

	AbsolutePath BuildDirectory => RootDirectory / "Build";
	AbsolutePath OutputDirectory => BuildDirectory / "Output";
	AbsolutePath NightlyBuildDirectory => BuildDirectory / "NightlyBuild";
	AbsolutePath NugetPackageDirectory => NightlyBuildDirectory / "NuGetPackages";
	AbsolutePath NuGetPackageSpecsDirectory => BuildDirectory / "NuGetPackageSpecs";

	AbsolutePath NunitProject => RootDirectory / "Test" / "Duality.nunit";

	public static int Main() => Execute<Build>(x => x.Compile);

	[Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
	readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

	[Solution] readonly Solution Solution;

	Target Clean => _ => _
		.Executes(() =>
		{
			EnsureCleanDirectory(OutputDirectory);
		});

	Target Restore => _ => _
		.After(Clean)
		.Executes(() =>
		{
			DotNetRestore(s => s
				.SetProjectFile(Solution));
		});

	Target Compile => _ => _
		.DependsOn(Restore)
		.Executes(() =>
		{
			DotNetBuild(s => s
				.SetProjectFile(Solution));
		});

	Target Test => _ => _
		.DependsOn(Compile)
		.Executes(() =>
		{
			NUnit3(s => s
				.AddInputFiles(NunitProject));			
		});

	Target BuildNugetPackages => _ => _
		.DependsOn(Compile)
		.Executes(() =>
		{
			var nuspecs = NuGetPackageSpecsDirectory.GlobFiles("*.nuspec");

			foreach (var item in nuspecs)
			{
				try
				{
					NuGetPack(s => s
						.SetTargetPath(item)
						.SetOutputDirectory(NugetPackageDirectory));
				}
				catch (Exception e)
				{
					Logger.Warn($"Failed to pack {item}");
				}
			}
		});
}