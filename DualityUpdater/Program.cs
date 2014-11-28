using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Duality.Updater
{
	public static class Program
	{
		private static string selfFileName = Path.GetFileName(typeof(Program).Assembly.CodeBase);

		public static void Main(string[] args)
		{
			string updateFilePath = (args.Length >= 1) ? args[0] : null;
			string runAfterFinishPath = (args.Length >= 2) ? args[1] : null;
			string runAfterFinishWorkDir = (args.Length >= 3) ? args[2] : null;
			if (string.IsNullOrEmpty(updateFilePath) || !File.Exists(updateFilePath)) return;

			bool anyErrorOccurred = false;
			
			Console.WriteLine();
			Console.WriteLine("Waiting for file locks to release...");
			Console.WriteLine();

			IOHelper.WaitForLockRelease("DualityEditor.exe", "Duality.dll");

			Console.WriteLine();
			Console.WriteLine("Begin applying update");
			Console.WriteLine();

			XDocument updateDoc = XDocument.Load(updateFilePath);
			string lastCommandName = null;
			foreach (XElement elem in updateDoc.Root.Elements())
			{
				string commandName = elem.Name.LocalName;

				if (!string.Equals(commandName, lastCommandName, StringComparison.InvariantCultureIgnoreCase))
				{
					Console.WriteLine();
				}

				try
				{
					if (string.Equals(commandName, "Remove", StringComparison.InvariantCultureIgnoreCase))
						PerformRemove(elem);
					else if (string.Equals(commandName, "Update", StringComparison.InvariantCultureIgnoreCase))
						PerformUpdate(elem);
					else if (string.Equals(commandName, "IntegrateProject", StringComparison.InvariantCultureIgnoreCase))
						PerformIntegrateProject(elem);
					else
						throw new InvalidOperationException(string.Format("Unknown command: {0}", commandName));

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("success");
					Console.ResetColor();
				}
				catch (Exception e)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("failed");
					Console.WriteLine();
					Console.WriteLine("Exception: {0}", e);
					Console.WriteLine();
					Console.ResetColor();
					IOHelper.WaitForUserRead();
					anyErrorOccurred = true;
				}
				lastCommandName = commandName;
			}

			// If an error occurred, abort here
			if (anyErrorOccurred)
			{
				Console.WriteLine();
				Console.WriteLine("Some steps of the update process failed. The update was not successfull.");
				Console.WriteLine();
				IOHelper.WaitForUserRead();
				return;
			}

			File.Delete(updateFilePath);
			Console.WriteLine();
			Console.WriteLine("Update applied.");
			Console.WriteLine();
			
			if (!string.IsNullOrEmpty(runAfterFinishPath) && File.Exists(runAfterFinishPath))
			{
				Console.WriteLine();
				Console.WriteLine("Running scheduled application...");
				Console.WriteLine();

				ProcessStartInfo startInfo = new ProcessStartInfo(runAfterFinishPath);
				if (!string.IsNullOrEmpty(runAfterFinishWorkDir) && Directory.Exists(runAfterFinishWorkDir))
				{
					startInfo.WorkingDirectory = runAfterFinishWorkDir;
				}
				Process.Start(startInfo);
			}
		}

		private static void PerformRemove(XElement commandElement)
		{
			XAttribute attribTarget = commandElement.Attribute("target");
			string target = (attribTarget != null) ? attribTarget.Value : null;

			// Self Update is not supported. Skip it.
			if (string.Equals(Path.GetFileName(target), selfFileName, StringComparison.InvariantCultureIgnoreCase))
				return;
			
			PrettyPrint.PrintCommand(
				new PrettyPrint.Element("Delete", PrettyPrint.ElementType.Command),
				new PrettyPrint.Element(target, PrettyPrint.ElementType.FilePathArgument));
			IOHelper.WaitForLockRelease(target);

			File.Delete(target);
			IOHelper.RemoveEmptyDirectory(Path.GetDirectoryName(target));
		}
		private static void PerformUpdate(XElement commandElement)
		{
			XAttribute attribSource = commandElement.Attribute("source");
			XAttribute attribTarget = commandElement.Attribute("target");
			string source = (attribSource != null) ? attribSource.Value : null;
			string target = (attribTarget != null) ? attribTarget.Value : null;
			
			PrettyPrint.PrintCommand(
				new PrettyPrint.Element("Copy", PrettyPrint.ElementType.Command),
				new PrettyPrint.Element(source, PrettyPrint.ElementType.FilePathArgument),
				new PrettyPrint.Element("to", PrettyPrint.ElementType.Command),
				new PrettyPrint.Element(Path.GetDirectoryName(target), PrettyPrint.ElementType.FilePathArgument));
			IOHelper.WaitForLockRelease(source, target);

			string targetDir = Path.GetDirectoryName(target);
			if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
				Directory.CreateDirectory(targetDir);

			File.Copy(source, target, true);
		}
		private static void PerformIntegrateProject(XElement commandElement)
		{
			XAttribute attribProject = commandElement.Attribute("project");
			XAttribute attribSolution = commandElement.Attribute("solution");
			string projectFile = (attribProject != null) ? attribProject.Value : null;
			string solutionFile = (attribSolution != null) ? attribSolution.Value : null;
			
			PrettyPrint.PrintCommand(
				new PrettyPrint.Element("Integrating", PrettyPrint.ElementType.Command),
				new PrettyPrint.Element(projectFile, PrettyPrint.ElementType.FilePathArgument),
				new PrettyPrint.Element("into", PrettyPrint.ElementType.Command),
				new PrettyPrint.Element(solutionFile, PrettyPrint.ElementType.FilePathArgument));
			IOHelper.WaitForLockRelease(projectFile, solutionFile);

			// Read the project file
			XDocument csproj = XDocument.Load(projectFile);

			// Set up schedule for elements to remove
			List<XElement> removeElements = new List<XElement>();

			// Remove all elements that have been flagged to not be included in Duality packages
			foreach (var element in csproj.Descendants("DualityPackageExcludeParentElement", true))
			{
				removeElements.Add(element.Parent);
			}

			// Remove all traces of NuGet. Dependencies are handled by Duality Package Management!
			{
				// Get rid of packages.config items
				foreach (var attribute in csproj.Descendants("ItemGroup", true).Elements().Attributes("Include", true))
				{
					if (string.Equals(attribute.Value, "packages.config", StringComparison.InvariantCultureIgnoreCase))
					{
						removeElements.Add(attribute.Parent);
					}
				}

				// Get rid of NuGet imports
				foreach (var attribute in csproj.Descendants("Import", true).Attributes("Project", true))
				{
					string lowerAttrib = attribute.Value.ToLower();
					if (lowerAttrib.Contains(".nuget") || lowerAttrib.Contains("nuget.targets"))
					{
						removeElements.Add(attribute.Parent);
					}
				}

				// Get rid of NuGet target data
				foreach (var attribute in csproj.Descendants("Target", true).Attributes("Name", true))
				{
					string lowerAttrib = attribute.Value.ToLower();
					if (lowerAttrib.Contains("EnsureNuGetPackageBuildImports".ToLower()))
					{
						removeElements.Add(attribute.Parent);
					}
				}
			}

			// Remove all elements that were scheduled for removal
			foreach (var element in removeElements)
			{
				element.RemoveUpwards();
			}

			// ToDo: Remove all NuGet-related data, as Duality handles it via dependencies
			// ToDo: Transform all References and Project References
			// ToDo: Add PropertyGroups for debugging the code (StartProgram stuff)
		}
	}
}
