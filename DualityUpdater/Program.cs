using System;
using System.Reflection;
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

			// Retrieve elements and order them
			XDocument updateDoc = XDocument.Load(updateFilePath);
			List<CommandInfo> commands = new List<CommandInfo>();
			{
				int elementIndex = 0;
				foreach (XElement element in updateDoc.Root.Elements())
				{
					commands.Add(new CommandInfo(element, elementIndex));
					elementIndex++;
				}
				commands.Sort((a, b) => Comparer<int>.Default.Compare(a.SortValue, b.SortValue));
			}

			// Perform operations in order
			CommandType lastCommandType = CommandType.Unknown;
			for (int i = 0; i < commands.Count; i++)
			{
				CommandInfo command = commands[i];
				if (command.Type != lastCommandType)
				{
					Console.WriteLine();
				}

				try
				{
					CommandResult result = CommandResult.Failure;

					switch (command.Type)
					{
						case CommandType.Remove:
							result = PerformRemove(command.Element);
							break;
						case CommandType.Update:
							result = PerformUpdate(command.Element);
							break;
						case CommandType.IntegrateProject:
							result = PerformIntegrateProject(command.Element);
							break;
						case CommandType.SeparateProject:
							result = PerformSeparateProject(command.Element);
							break;
						default:
							throw new InvalidOperationException(string.Format("Unknown command: {0}", command.Element.Name));
					}

					if (result == CommandResult.Skip)
					{
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.WriteLine("skip");
					}
					else if (result == CommandResult.Success)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("success");
					}
					else
					{
						throw new InvalidOperationException("Operation failed due to unknown reason.");
					}
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
				lastCommandType = command.Type;
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

		private static CommandResult PerformRemove(XElement commandElement)
		{
			XAttribute attribTarget = commandElement.Attribute("target");
			string target = (attribTarget != null) ? attribTarget.Value : null;

			// Self Update is not supported. Skip it.
			if (string.Equals(Path.GetFileName(target), selfFileName, StringComparison.InvariantCultureIgnoreCase))
				return CommandResult.Skip;
			
			PrettyPrint.PrintCommand(
				new PrettyPrint.Element("Delete", PrettyPrint.ElementType.Command),
				new PrettyPrint.Element(target, PrettyPrint.ElementType.FilePathArgument));
			IOHelper.WaitForLockRelease(target);

			if (File.Exists(target))
				File.Delete(target);
			else
				return CommandResult.Skip;

			IOHelper.RemoveEmptyDirectory(Path.GetDirectoryName(target));

			return CommandResult.Success;
		}
		private static CommandResult PerformUpdate(XElement commandElement)
		{
			XAttribute attribSource = commandElement.Attribute("source");
			XAttribute attribTarget = commandElement.Attribute("target");
			string source = (attribSource != null) ? attribSource.Value : null;
			string target = (attribTarget != null) ? attribTarget.Value : null;
			
			// Self Update is not supported. Skip it.
			if (string.Equals(Path.GetFileName(target), selfFileName, StringComparison.InvariantCultureIgnoreCase))
				return CommandResult.Skip;

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

			return CommandResult.Success;
		}
		private static CommandResult PerformIntegrateProject(XElement commandElement)
		{
			XAttribute attribProject = commandElement.Attribute("project");
			XAttribute attribSolution = commandElement.Attribute("solution");
			XAttribute attribPluginDir = commandElement.Attribute("pluginDirectory");
			string projectFile = (attribProject != null) ? attribProject.Value : null;
			string solutionFile = (attribSolution != null) ? attribSolution.Value : null;
			string pluginDirectory = (attribPluginDir != null) ? attribPluginDir.Value : null;
			string projectRelativeBaseDir = IOHelper.MakePathRelative(".", Path.GetDirectoryName(projectFile));
			string solutionRelativeProjectPath = IOHelper.MakePathRelative(projectFile, Path.GetDirectoryName(solutionFile));
			Guid projectGuid = Guid.NewGuid();

			PrettyPrint.PrintCommand(
				new PrettyPrint.Element("Integrating", PrettyPrint.ElementType.Command),
				new PrettyPrint.Element(projectFile, PrettyPrint.ElementType.FilePathArgument),
				new PrettyPrint.Element("into", PrettyPrint.ElementType.Command),
				new PrettyPrint.Element(solutionFile, PrettyPrint.ElementType.FilePathArgument));

			if (!File.Exists(solutionFile))
			{
				return CommandResult.Skip;
			}

			IOHelper.WaitForLockRelease(projectFile, solutionFile);

			{
				// Read the project file
				XDocument csproj = XDocument.Load(projectFile);
				XElement guidElement = csproj.Root.Descendants("ProjectGuid", true).FirstOrDefault();
				if (guidElement != null)
				{
					Guid guid;
					if (Guid.TryParse(guidElement.Value, out guid))
					{
						projectGuid = guid;
					}
				}

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

				// Transform reference HintPaths to local relative paths
				foreach (var element in csproj.Descendants("Reference", true).Elements("HintPath", true))
				{
					string assemblyPath = element.Value;
					string assemblyFileName = Path.GetFileName(assemblyPath);
					if (File.Exists(assemblyFileName))
					{
						element.Value = Path.Combine(projectRelativeBaseDir, assemblyFileName);
					}
					else
					{
						foreach (string file in Directory.EnumerateFiles(pluginDirectory, assemblyFileName, SearchOption.AllDirectories))
						{
							element.Value = Path.Combine(projectRelativeBaseDir, file);
							break;
						}
					}
				}

				// Determine existing ItemGroup with Assembly references, or create one
				XElement referenceGroup;
				{
					XElement existingReferenceItem = csproj.Descendants("Reference", true).FirstOrDefault();
					if (existingReferenceItem != null)
					{
						referenceGroup = existingReferenceItem.Parent;
					}
					else
					{
						XElement sampleItemGroup = csproj.Descendants("ItemGroup", true).FirstOrDefault();
						XName itemGroupName;
						if (sampleItemGroup != null)
							itemGroupName = sampleItemGroup.Name;
						else
							itemGroupName = XName.Get("ItemGroup", csproj.Root.Name.NamespaceName);
						referenceGroup = new XElement(itemGroupName);
						csproj.Root.Add(referenceGroup);
					}
				}

				// Transform project references to local relative paths
				foreach (var element in csproj.Descendants("ProjectReference", true))
				{
					string projectPath = element.Attribute("Include", true).Value;
					string assemblyFileName = Path.GetFileNameWithoutExtension(projectPath) + ".dll";
					string assemblyPath = null;
					if (File.Exists(assemblyFileName))
					{
						assemblyPath = assemblyFileName;
					}
					else
					{
						foreach (string file in Directory.EnumerateFiles(pluginDirectory, assemblyFileName, SearchOption.AllDirectories))
						{
							assemblyPath = file;
							break;
						}
					}
					if (!string.IsNullOrEmpty(assemblyPath) && File.Exists(assemblyPath))
					{
						XNamespace defaultNs = referenceGroup.Name.Namespace;
						AssemblyName name = AssemblyName.GetAssemblyName(assemblyPath);
						referenceGroup.Add(new XElement(defaultNs + "Reference",
							new XAttribute("Include", name.ToString()),
							new XElement(defaultNs + "HintPath", Path.Combine(projectRelativeBaseDir, assemblyPath))));
						removeElements.Add(element);
					}
				}

				// Remove all elements that were scheduled for removal
				foreach (var element in removeElements)
				{
					element.RemoveUpwards();
				}

				// Add a new post-build step that copies the files
				string postBuildCommand =
					"mkdir \"$(SolutionDir)../../Plugins\"" + Environment.NewLine +
					"copy \"$(TargetPath)\" \"$(SolutionDir)../../Plugins\"" + Environment.NewLine +
					"xcopy /Y \"$(TargetDir)*.xml\" \"$(SolutionDir)../../Plugins\"";
				XElement postBuildElement = csproj.Descendants("PostBuildEvent", true).FirstOrDefault();
				if (postBuildElement == null)
				{
					XNamespace defaultNs = referenceGroup.Name.Namespace;
					csproj.Root.Add(new XElement(defaultNs + "PropertyGroup", postBuildElement = new XElement(defaultNs + "PostBuildEvent")));
				}
				postBuildElement.Value = (postBuildElement.Value ?? "") + Environment.NewLine + postBuildCommand;

				// Save the project file and be done with it
				csproj.Save(projectFile);
			}

			// Add the project to the existing solution
			{
				List<string> solutionLines = File.ReadAllLines(solutionFile).ToList();
				Guid projectTypeGuid = Guid.Parse("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");

				// Just to be sure, see if we can extract the project type Guid from existing C# project references.
				foreach (string line in solutionLines)
				{
					string trimmedLine = line.Trim();
					if (line.StartsWith("Project(") && line.Contains(".csproj"))
					{
						string[] token = line.Split('"');
						if (token.Length > 1)
						{
							string csprojGuidString = token[1];
							Guid guidFromFile;
							if (Guid.TryParse(csprojGuidString, out guidFromFile))
							{
								projectTypeGuid = guidFromFile;
							}
						}
					}
				}

				// Add a new project reference to the solution
				int projectIndex = solutionLines.LastIndexOf("EndProject");
				if (projectIndex == -1) projectIndex = solutionLines.Count - 1;
				solutionLines.Insert(projectIndex + 1, string.Format("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"", 
					projectTypeGuid, 
					Path.GetFileNameWithoutExtension(projectFile), 
					solutionRelativeProjectPath, 
					projectGuid));
				solutionLines.Insert(projectIndex + 2, "EndProject");

				File.WriteAllLines(solutionFile, solutionLines);
			}

			return CommandResult.Success;
		}
		private static CommandResult PerformSeparateProject(XElement commandElement)
		{
			XAttribute attribProject = commandElement.Attribute("project");
			XAttribute attribSolution = commandElement.Attribute("solution");
			string projectFile = (attribProject != null) ? attribProject.Value : null;
			string solutionFile = (attribSolution != null) ? attribSolution.Value : null;
			string projectRelativeBaseDir = IOHelper.MakePathRelative(".", Path.GetDirectoryName(projectFile));
			string solutionRelativeProjectPath = IOHelper.MakePathRelative(projectFile, Path.GetDirectoryName(solutionFile));
			
			PrettyPrint.PrintCommand(
				new PrettyPrint.Element("Separating", PrettyPrint.ElementType.Command),
				new PrettyPrint.Element(projectFile, PrettyPrint.ElementType.FilePathArgument),
				new PrettyPrint.Element("from", PrettyPrint.ElementType.Command),
				new PrettyPrint.Element(solutionFile, PrettyPrint.ElementType.FilePathArgument));

			if (!File.Exists(solutionFile))
			{
				return CommandResult.Skip;
			}

			IOHelper.WaitForLockRelease(projectFile, solutionFile);

			// Remove the project from the existing solution
			{
				List<string> solutionLines = File.ReadAllLines(solutionFile).ToList();

				// Find the line where this project is referenced
				string projectFileName = Path.GetFileName(projectFile);
				int startIndex = -1;
				int endIndex = -1;
				for (int i = 0; i < solutionLines.Count; i++)
				{
					string line = solutionLines[i];
					if (startIndex == -1 && line.Contains(projectFileName))
					{
						startIndex = i;
					}
					if (startIndex != -1 && line.Contains("EndProject"))
					{
						endIndex = i;
						break;
					}
				}

				// If we found the reference block, remove it and save the solution file
				if (startIndex != -1 && endIndex != -1)
				{
					solutionLines.RemoveRange(startIndex, 1 + endIndex - startIndex);
					File.WriteAllLines(solutionFile, solutionLines);
				}
				// Otherwise, skip this step
				else
				{
					return CommandResult.Skip;
				}
			}

			return CommandResult.Success;
		}

		private enum CommandType
		{
			Unknown,
			Remove,
			Update,
			IntegrateProject,
			SeparateProject
		}
		private enum CommandResult
		{
			Success,
			Failure,
			Skip
		}
		private struct CommandInfo
		{
			public XElement Element;
			public int SortValue;
			public CommandType Type;

			public CommandInfo(XElement element, int sortOffset)
			{
				this.Element = element;
				if (!Enum.TryParse(element.Name.LocalName, true, out this.Type))
				{
					this.Type = Program.CommandType.Unknown;
				}
				switch (this.Type)
				{
					default:
						this.SortValue = sortOffset + 0;
						break;
					case Program.CommandType.IntegrateProject:
						this.SortValue = sortOffset + 100000;
						break;
					case Program.CommandType.SeparateProject:
						this.SortValue = sortOffset - 100000;
						break;
				}
			}
			public override string ToString()
			{
				return string.Format("{0}: {1}", this.Type, this.Element.Attributes().ToString(a => a.ToString(), ", "));
			}
		}
	}
}
