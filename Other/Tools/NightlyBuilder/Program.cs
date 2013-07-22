using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Reflection;

using Microsoft.Win32;
using Microsoft.Build.Execution;
using Microsoft.Build.Logging;
using Microsoft.Build.Framework;

using Ionic.Zip;

namespace NightlyBuilder
{
	public class Program
	{
		public static void Main(string[] args)
		{
			ConfigFile config = ConfigFile.Load("BuildConfig.xml");
			string packagePath = Path.Combine(config.PackageDir, config.PackageName);
			FileVersionInfo versionCore = null;
			FileVersionInfo versionEditor = null;
			FileVersionInfo versionLauncher = null;

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

			// Do an SVN Revert of the package
			if (config.CommitSVN)
			{
				Console.WriteLine("================================== SVN Revert =================================");
				{
					ExecuteCommand(
						string.Format("svn revert *"),
						config.PackageDir);
				}
				Console.WriteLine("===============================================================================");
				Console.WriteLine();
				Console.WriteLine();
			}

			// Build the target Solution
			Console.WriteLine("================================ Build Solution ===============================");
			{
				var buildProperties = new Dictionary<string,string>(){ { "Configuration", "Release"} };
				var buildRequest = new BuildRequestData(config.SolutionPath, buildProperties, null, new string[] { "Build" }, null);
				var buildParameters = new BuildParameters();
				buildParameters.Loggers = new[] { new ConsoleLogger(LoggerVerbosity.Minimal) };
				var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);
				if (buildResult.OverallResult != BuildResultCode.Success)
					throw new ApplicationException("Project Build Failure");

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

			// Build the documentation
			if (!config.NoDocs)
			{
				Console.WriteLine("================================== Build Docs =================================");
				{
					var buildProperties = new Dictionary<string,string>(){ { "Configuration", "Release"} };
					var buildRequest = new BuildRequestData(config.DocSolutionPath, buildProperties, null, new string[] { "Build" }, null);
					var buildParameters = new BuildParameters();
					buildParameters.Loggers = new[] { new ConsoleLogger(LoggerVerbosity.Normal) };
					var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);
					if (buildResult.OverallResult != BuildResultCode.Success)
						throw new ApplicationException("Documentation Build Failure");
					File.Copy(
						Path.Combine(config.DocBuildResultDir, config.DocBuildResultFile), 
						Path.Combine(config.BuildResultDir, config.DocBuildResultFile),
						true);
				}
				Console.WriteLine("===============================================================================");
				Console.WriteLine();
				Console.WriteLine();
			}
			else if (File.Exists(Path.Combine(config.DocBuildResultDir, config.DocBuildResultFile)))
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

			// Copy the results to the target directory
			Console.WriteLine("================================ Copy to Target ===============================");
			{
				Console.WriteLine("Creating target directory '{0}'", config.TargetDir);
				if (Directory.Exists(config.TargetDir))
					Directory.Delete(config.TargetDir, true);
				CopyDirectory(config.BuildResultDir, config.TargetDir, true, path => 
					{
						string fileName = Path.GetFileName(path);
						foreach (string blackListEntry in config.FileCopyBlackList)
						{
							if (Regex.IsMatch(fileName, WildcardToRegex(blackListEntry), RegexOptions.IgnoreCase))
							{
								Console.ForegroundColor = ConsoleColor.DarkGray;
								Console.WriteLine("Ignore {0}", path);
								Console.ForegroundColor = ConsoleColor.Gray;
								return false;
							}
						}
						Console.WriteLine("Copy   {0}", path);
						return true;
					});
				CopyDirectory(config.AdditionalFileDir, config.TargetDir, true);
			}
			Console.WriteLine("===============================================================================");
			Console.WriteLine();
			Console.WriteLine();

			// Create the ZIP package
			Console.WriteLine("================================ Create Package ===============================");
			{
				Console.WriteLine("Package Path: {0}", packagePath);
				if (!Directory.Exists(config.PackageDir))
					Directory.CreateDirectory(config.PackageDir);

				ZipFile package = new ZipFile();
				string[] files = Directory.GetFiles(config.TargetDir, "*", SearchOption.AllDirectories);
				package.AddFiles(files);
				package.Save(packagePath);
			}
			Console.WriteLine("===============================================================================");
			Console.WriteLine();
			Console.WriteLine();
			
			// Cleanup
			Console.WriteLine("=================================== Cleanup ===================================");
			{
				Console.WriteLine("Deleting target directory '{0}'", config.TargetDir);
				if (Directory.Exists(config.TargetDir))
					Directory.Delete(config.TargetDir, true);
			}
			Console.WriteLine("===============================================================================");
			Console.WriteLine();
			Console.WriteLine();

			// Do an SVN Commit of the package
			if (config.CommitSVN)
			{
				Console.WriteLine("================================== SVN Commit =================================");
				{
					// "svn add --force * --auto-props --parents --depth infinity -q"
				
					string commitMessage = string.Format("Updated Binary Package{0}{1}",
						Environment.NewLine,
						versionCore.FileVersion,
						versionEditor.FileVersion,
						versionLauncher.FileVersion);
					string commitMessageFile = Path.Combine(config.PackageDir, "CommitMsg.txt");
					File.WriteAllText(commitMessageFile, commitMessage);
					ExecuteCommand(
						string.Format("svn commit -F CommitMsg.txt"),
						config.PackageDir);
					File.Delete(commitMessageFile);
				}
				Console.WriteLine("===============================================================================");
				Console.WriteLine();
				Console.WriteLine();
			}
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
		public static void ExecuteCommand(string command, string workingDir = null)
		{
			if (workingDir == null) workingDir = Environment.CurrentDirectory;
			Console.WriteLine(command);

			ProcessStartInfo info = new ProcessStartInfo("cmd.exe", "/C " + command);
			info.UseShellExecute = false;
			info.RedirectStandardOutput = true;
			info.WindowStyle = ProcessWindowStyle.Hidden;
			info.WorkingDirectory = workingDir;
			Process proc = Process.Start(info);
			while (!proc.StandardOutput.EndOfStream)
			{
				Console.WriteLine(proc.StandardOutput.ReadLine());
			}
			proc.WaitForExit();
		}
	}
}
