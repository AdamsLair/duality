using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace NightlyBuilder
{
	public class Program
	{
		public static void Main(string[] args)
		{
			ConfigFile config = ConfigFile.Load("BuildConfig.xml");

			// Parse command line arguments
			PropertyInfo[] configProps = typeof(ConfigFile).GetProperties();
			HashSet<PropertyInfo> configOverride = new HashSet<PropertyInfo>();
			foreach (string arg in args)
			{
				string[] token = arg.Split('=');
				if (token.Length != 2) continue;

				PropertyInfo prop = configProps.FirstOrDefault(p => p.Name.ToLower() == token[0].Trim().ToLower());
				if (prop == null) continue;

				object value = null;
				try
				{
					value = Convert.ChangeType(token[1].Trim(), prop.PropertyType, System.Globalization.CultureInfo.InvariantCulture);
				}
				catch {}
				if (value == null) continue;

				prop.SetValue(config, value, null);
				configOverride.Add(prop);
			}

			try
			{
				// Resolve tool paths
				config.NuGetPath = FindToolPath(config.NuGetPath);

				// Write some initial data
				Console.WriteLine("===================================== Init ====================================");
				{
					Console.WriteLine("NightlyBuilder launched");
					Console.WriteLine("Working Dir: {0}", Environment.CurrentDirectory);
					Console.WriteLine("Command Line: {0}", args.Aggregate("", (acc, arg) => acc + " " + arg));
					Console.WriteLine("Config:");
					foreach (PropertyInfo prop in configProps)
					{
						if (configOverride.Contains(prop))
							Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine("  {0}: {1}", prop.Name, prop.GetValue(config, null));
						Console.ForegroundColor = ConsoleColor.Gray;
					}
				}
				Console.WriteLine("===============================================================================");
				Console.WriteLine();
				Console.WriteLine();

				// Run the build
				PerformNightlyBuild(config);
			}
			catch (Exception e)
			{
				Console.WriteLine();
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("ERROR: {0}", e);
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine();
				if (config.NonInteractive)
					Environment.Exit(1);
				else
					Console.ReadLine();
			}

			Environment.Exit(0);
		}

		public static void PerformNightlyBuild(ConfigFile config)
		{
			Console.WriteLine("============================= Clean NuGet Packages ============================");
			if (Directory.Exists(config.NuGetPackageTargetDir))
			{
				Console.WriteLine("Deleting old package files in '{0}'...", config.NuGetPackageTargetDir);
				foreach (string file in Directory.EnumerateFiles(config.NuGetPackageTargetDir, "*.nupkg", SearchOption.TopDirectoryOnly))
				{
					File.Delete(file);
				}
			}
			Console.WriteLine("===============================================================================");
			Console.WriteLine();
			Console.WriteLine();

			string packagePath = Path.Combine(config.PackageDir, config.PackageName);
			FileVersionInfo versionCore = null;
			FileVersionInfo versionEditor = null;
			FileVersionInfo versionLauncher = null;

			// Build the target Solution
			if (!config.NoBuild)
			{
				Console.WriteLine("================================ Build Solution ===============================");
				{
					bool buildSuccess = BuildVisualStudioSolution(config.SolutionPath, "Release");
					if (!buildSuccess)
						throw new ApplicationException("The project doesn't compile properly. Cannot proceed in this state.");

					versionCore = FileVersionInfo.GetVersionInfo(Path.Combine(config.BuildResultDir, "Duality.dll"));
					versionEditor = FileVersionInfo.GetVersionInfo(Path.Combine(config.BuildResultDir, "DualityEditor.exe"));
					versionLauncher = FileVersionInfo.GetVersionInfo(Path.Combine(config.BuildResultDir, "DualityLauncher.exe"));

					Console.WriteLine("Build Successful");
					Console.WriteLine("  Core Version:     {0}", versionCore.FileVersion);
					Console.WriteLine("  Editor Version:   {0}", versionEditor.FileVersion);
					Console.WriteLine("  Launcher Version: {0}", versionLauncher.FileVersion);
				}
				Console.WriteLine("===============================================================================");
				Console.WriteLine();
				Console.WriteLine();
			}

			// Perform unit testing
			if (!config.NoTests)
			{
				Console.WriteLine("================================= Unit Testing ================================");
				foreach (string nunitProjectFile in Directory.EnumerateFiles(config.UnitTestProjectDir, "*.nunit", SearchOption.TopDirectoryOnly))
				{
					Console.Write("Testing '{0}'... ", Path.GetFileName(nunitProjectFile));

					// Don't use /timeout, as it will execute tests from a different thread,
					// which will break a lot of graphics-related Duality stuff!
					string resultFile = "UnitTestResult.xml";
					ExecuteCommand(
						string.Format("{0} {1} /result={2}", 
							Path.Combine(config.NUnitBinDir, "nunit3-console.exe"), 
							nunitProjectFile,
							resultFile), 
						verbose: true);

					if (File.Exists(resultFile))
					{
						XmlDocument resultDoc = new XmlDocument();
						resultDoc.Load(resultFile);

						XmlElement testRunElement = resultDoc["test-run"];
						XmlAttribute failedAttribute = testRunElement.Attributes["failed"];
						int failedCount = int.Parse(failedAttribute.Value);

						if (failedCount > 0)
						{
							ExecuteBackgroundCommand(resultFile);
							ExecuteBackgroundCommand(
								string.Format("{0} {1}", 
									Path.Combine(config.NUnitBinDir, "nunit3-console.exe"), 
									nunitProjectFile));
							throw new ApplicationException(string.Format("At least one unit test has failed. See {0} for more information.", resultFile));
						}
						else
						{
							Console.WriteLine("success!");
							File.Delete(resultFile);
						}
					}
					else
					{
						throw new ApplicationException("Something appears to have failed during unit testing, because no result file was found.");
					}
				}
				Console.WriteLine("===============================================================================");
				Console.WriteLine();
				Console.WriteLine();
			}

			// Build the documentation
			bool includeDocs =
				!string.IsNullOrWhiteSpace(config.DocBuildResultDir) &&
				!string.IsNullOrWhiteSpace(config.DocBuildResultFile);
			if (!config.NoDocs)
			{
				Console.WriteLine("================================== Build Docs =================================");
				{
					bool buildSuccess = BuildVisualStudioSolution(config.DocSolutionPath, "Release");
					if (!buildSuccess)
						throw new ApplicationException("Documentation Build Failure");

					Console.WriteLine("Documentation Build Successful");

					if (includeDocs)
					{
						File.Copy(
							Path.Combine(config.DocBuildResultDir, config.DocBuildResultFile), 
							Path.Combine(config.BuildResultDir, config.DocBuildResultFile),
							true);
						Console.WriteLine("Documentation copied to build directory");
					}
				}
				Console.WriteLine("===============================================================================");
				Console.WriteLine();
				Console.WriteLine();
			}
			else if (includeDocs && File.Exists(Path.Combine(config.DocBuildResultDir, config.DocBuildResultFile)))
			{
				Console.WriteLine("============================== Copy existing Docs =============================");
				{
					File.Copy(
						Path.Combine(config.DocBuildResultDir, config.DocBuildResultFile), 
						Path.Combine(config.BuildResultDir, config.DocBuildResultFile),
						true);
				}
				Console.WriteLine("===============================================================================");
				Console.WriteLine();
				Console.WriteLine();
			}

			// Build all NuGet Packages
			Console.WriteLine("============================= Build NuGet Packages ============================");
			{
				bool nugetFound = File.Exists(config.NuGetPath);
				bool nugetSpecsFound = Directory.Exists(config.NuGetPackageSpecsDir);
				if (nugetFound && nugetSpecsFound)
				{
					if (!Directory.Exists(config.NuGetPackageTargetDir))
						Directory.CreateDirectory(config.NuGetPackageTargetDir);

					Console.WriteLine("Determining package data from '{0}'...", config.NuGetPackageSpecsDir);
					Dictionary<string,string> packageVersions = new Dictionary<string,string>();
					foreach (string file in Directory.EnumerateFiles(config.NuGetPackageSpecsDir, "*.nuspec", SearchOption.AllDirectories))
					{
						Console.Write("  {0}: ", Path.GetFileName(file));

						string fileAbs = Path.GetFullPath(file);
						XDocument doc = XDocument.Load(fileAbs);
						XElement elemId = doc.Descendants("id").FirstOrDefault();
						XElement elemVersion = doc.Descendants("version").FirstOrDefault();

						string id = elemId.Value.Trim();
						string version = elemVersion.Value.Trim();
						packageVersions[id] = version;

						Console.WriteLine("{0}", version);
					}

					Console.WriteLine();
					Console.WriteLine("Creating packages from '{0}'...", config.NuGetPackageSpecsDir);
					foreach (string file in Directory.EnumerateFiles(config.NuGetPackageSpecsDir, "*.nuspec", SearchOption.AllDirectories))
					{
						Console.Write("  {0}... ", Path.GetFileName(file));

						string fileAbs = Path.GetFullPath(file);
						XDocument doc = XDocument.Load(fileAbs);

						bool skip = false;
						foreach (XElement elemDependency in doc.Descendants("dependency"))
						{
							string id = elemDependency.Attribute("id").Value.Trim();
							string version = elemDependency.Attribute("version").Value.Trim();
							string developedAgainstVersion;
							if (packageVersions.TryGetValue(id, out developedAgainstVersion))
							{
								if (version != developedAgainstVersion)
								{
									skip = true;
									break;
								}
							}
						}

						if (skip)
						{
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.WriteLine("dependency mismatch (skip)");
							Console.ResetColor();
							continue;
						}

						XElement elemId = doc.Descendants("id").FirstOrDefault();
						XElement elemVersion = doc.Descendants("version").FirstOrDefault();
						string targetFileName = string.Format("{0}.{1}.nupkg", elemId.Value.Trim(), elemVersion.Value.Trim());
						string packResult = ExecuteCommand(Path.GetFullPath(config.NuGetPath) + " pack " + fileAbs, config.NuGetPackageTargetDir, false);

						if (!File.Exists(Path.Combine(config.NuGetPackageTargetDir, targetFileName)))
						{
							string errorMessage = string.Format(
								"Failed to create NuGet Package {0}:" + Environment.NewLine + "{1}", 
								Path.GetFileName(file), 
								packResult);

							// If in non-interactive mode, continue to build packages even if one of them failed.
							if (config.NonInteractive)
							{
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine("failed");
								Console.WriteLine(errorMessage);
								Console.ForegroundColor = ConsoleColor.Gray;
							}
							// Otherwise, stop with an exception.
							else
							{
								throw new ApplicationException(errorMessage);
							}
						}
						else
						{
							Console.WriteLine("done");
						}
					}
				}
				else if (!nugetFound)
				{
					throw new ApplicationException(string.Format("Can't find NuGet command line tool '{0}'.", config.NuGetPath));
				}
				else if (!nugetSpecsFound)
				{
					throw new ApplicationException(string.Format("Can't find NuGet package specs directory '{0}'.", config.NuGetPackageSpecsDir));
				}
			}
			Console.WriteLine("===============================================================================");
			Console.WriteLine();
			Console.WriteLine();
			
			// Copy ZIP Package
			if (!string.IsNullOrWhiteSpace(config.CopyPackageTo))
			{
				Console.WriteLine("=============================== Copy ZIP Package ==============================");
				{
					Console.WriteLine("Copying package to '{0}'", config.CopyPackageTo);
					if (!Directory.Exists(config.CopyPackageTo))
						Directory.CreateDirectory(config.CopyPackageTo);
					File.Copy(packagePath, Path.Combine(config.CopyPackageTo, config.PackageName), true);
				}
				Console.WriteLine("===============================================================================");
				Console.WriteLine();
				Console.WriteLine();
			}

			Console.WriteLine("Finished Build.");
			if (!config.NonInteractive)
				Console.ReadLine();
		}

		public static string FindToolPath(string preferredPath)
		{
			if (string.IsNullOrEmpty(preferredPath)) return preferredPath;
			if (File.Exists(preferredPath)) return preferredPath;

			string fileName = Path.GetFileName(preferredPath);
			string[] paths = (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';');
			foreach (string path in paths)
			{
				string toolPathCandidate = Path.Combine(path, fileName);
				if (File.Exists(toolPathCandidate))
					return toolPathCandidate;
			}

			return preferredPath;
		}
		public static string WildcardToRegex(string pattern)
		{
			return "^" + Regex.Escape(pattern).
							   Replace(@"\*", ".*").
							   Replace(@"\?", ".") + "$";
		}
		public static void CopyDirectory(string sourcePath, string targetPath, bool overwrite = false, Predicate<string> filter = null)
		{
			if (!Directory.Exists(sourcePath)) throw new DirectoryNotFoundException("Source path not found");
			if (!overwrite && Directory.Exists(targetPath)) return;

			if (!Directory.Exists(targetPath)) 
				Directory.CreateDirectory(targetPath);

			foreach (string file in Directory.GetFiles(sourcePath))
			{
				if (filter != null && !filter(file)) continue;
				File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)), overwrite);
			}
			foreach (string subDir in Directory.GetDirectories(sourcePath))
			{
				if (filter != null && !filter(subDir)) continue;
				CopyDirectory(subDir, Path.Combine(targetPath, Path.GetFileName(subDir)), overwrite, filter);
			}
		}
		public static void ExecuteBackgroundCommand(string command, string workingDir = null)
		{
			if (workingDir == null) workingDir = Environment.CurrentDirectory;

			ProcessStartInfo info = new ProcessStartInfo("cmd.exe", "/C " + command);
			info.WindowStyle = ProcessWindowStyle.Hidden;
			info.WorkingDirectory = workingDir;
			Process proc = Process.Start(info);
		}
		public static string ExecuteCommand(string command, string workingDir = null, bool verbose = true)
		{
			if (workingDir == null) workingDir = Environment.CurrentDirectory;
			if (verbose) Console.WriteLine(command);

			ProcessStartInfo info = new ProcessStartInfo("cmd.exe", "/C " + command);
			info.UseShellExecute = false;
			info.RedirectStandardOutput = true;
			info.RedirectStandardError = true;
			info.WindowStyle = ProcessWindowStyle.Hidden;
			info.WorkingDirectory = workingDir;
			Process proc = Process.Start(info);

			// Retrieve command output
			StringBuilder resultBuilder = new StringBuilder();
			while (!proc.HasExited)
			{
				while (!proc.StandardOutput.EndOfStream)
				{
					string output = proc.StandardOutput.ReadLine();
					resultBuilder.AppendLine(output);
					if (verbose) Console.WriteLine(output);
				}
				while (!proc.StandardError.EndOfStream)
				{
					string output = proc.StandardError.ReadLine();
					resultBuilder.AppendLine(output);
					if (verbose) Console.WriteLine(output);
				}
			}

			proc.WaitForExit();
			return resultBuilder.ToString();
		}
		
		private static bool BuildVisualStudioSolution(string solutionFile, string configuration)
		{
			// Find msbuild binary path
			string msBuildPath = null;
			List<string> searchPaths = new List<string>();
			searchPaths.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft Visual Studio\\2017\\Community\\MSBuild\\15.0\\Bin"));
			searchPaths.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft Visual Studio"));
			foreach (string baseDir in searchPaths)
			{
				if (!Directory.Exists(baseDir)) continue;
				foreach (string candidatePath in Directory.EnumerateFiles(baseDir, "msbuild.exe", SearchOption.AllDirectories))
				{
					msBuildPath = candidatePath;
					break;
				}
				if (msBuildPath != null)
					break;
			}

			// Fail, if msbuild cannot be located
			if (msBuildPath == null) throw new InvalidOperationException("Unable to locate msbuild path.");

			// Run msbuild process
			ProcessStartInfo procInfo = new ProcessStartInfo();
			procInfo.WorkingDirectory = Environment.CurrentDirectory;
			procInfo.FileName = msBuildPath;
			procInfo.Arguments = string.Format(
				"/t:Rebuild /p:Configuration={1} /p:Platform=\"Any CPU\" -verbosity:minimal {0}", 
				solutionFile, 
				configuration);
			procInfo.CreateNoWindow = true;
			procInfo.UseShellExecute = false;
			procInfo.WindowStyle = ProcessWindowStyle.Hidden;

			procInfo.RedirectStandardOutput = true;

			Process proc = Process.Start(procInfo);
			proc.BeginOutputReadLine();
			proc.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
			{
				if (e.Data == null) return;
				Console.WriteLine(e.Data);
			};
			proc.WaitForExit();
			return proc.ExitCode == 0;
		}
	}
}
