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

			WaitForLockRelease("DualityEditor.exe", "Duality.dll");

			Console.WriteLine();
			Console.WriteLine("Begin applying update");
			Console.WriteLine();

			XDocument updateDoc = XDocument.Load(updateFilePath);
			foreach (XElement elem in updateDoc.Root.Elements())
			{
				if (string.Equals(elem.Name.LocalName, "Remove", StringComparison.InvariantCultureIgnoreCase))
				{
					try
					{
						PerformRemove(elem);

						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("success");
						Console.ResetColor();
					}
					catch (Exception e)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("failed");
						Console.WriteLine("Exception: {0}", e);
						Console.ResetColor();
						anyErrorOccurred = true;
					}
				}
				else if (string.Equals(elem.Name.LocalName, "Update", StringComparison.InvariantCultureIgnoreCase))
				{
					try
					{
						PerformUpdate(elem);

						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("success");
						Console.ResetColor();
					}
					catch (Exception e)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("failed");
						Console.WriteLine("Exception: {0}", e);
						Console.ResetColor();
						anyErrorOccurred = true;
					}
				}
				else if (string.Equals(elem.Name.LocalName, "IntegrateProject", StringComparison.InvariantCultureIgnoreCase))
				{
					try
					{
						PerformIntegrateProject(elem);

						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("success");
						Console.ResetColor();
					}
					catch (Exception e)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("failed");
						Console.WriteLine("Exception: {0}", e);
						Console.ResetColor();
						anyErrorOccurred = true;
					}
				}
			}

			// If an error occurred, abort here
			if (anyErrorOccurred)
			{
				Console.WriteLine();
				Console.WriteLine("Some steps of the update process failed. The update was not successfull.");
				Console.WriteLine();
				for (int i = 0; i < 10; i++)
				{
					Thread.Sleep(1000);
					Console.Write(".");
				}
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

		private static void RemoveEmptyDirectory(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return;
			if (File.Exists(path)) path = Path.GetDirectoryName(path);
			if (!Directory.Exists(path)) return;

			if (Directory.EnumerateFiles(path).Any()) return;
			if (Directory.EnumerateDirectories(path).Any()) return;
			Directory.Delete(path);

			string parent;
			try
			{
				parent = Path.GetDirectoryName(path);
			}
			catch (Exception)
			{
				parent = null;
			}

			if (parent != null)
			{
				RemoveEmptyDirectory(parent);
			}
		}
		private static bool IsFileLocked(string filePath)
		{
			return IsFileLocked(new FileInfo(filePath));
		}
		private static bool IsFileLocked(FileInfo file)
		{
			if (!file.Exists) return false;

			FileStream stream = null;
			try
			{
				stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			}
			catch (IOException)
			{
				return true;
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}

			return false;
		}
		private static void WaitForLockRelease(params string[] files)
		{
			bool anyFileLocked;
			int iterations = 0;
			do
			{
				anyFileLocked = false;
				for (int i = 0; i < files.Length; i++)
				{
					if (IsFileLocked(files[i]))
					{
						anyFileLocked = true;
						break;
					}
				}
				if (anyFileLocked)
				{
					iterations++;
					if (iterations > 0 && (iterations % 30) == 0)
					{
						Console.ForegroundColor = ConsoleColor.DarkGray;
						Console.WriteLine("(waiting for file access) ");
						Console.ResetColor();
					}
					Thread.Sleep(100);
				}
			} while (anyFileLocked);
		}

		private static void PerformRemove(XElement element)
		{
			XAttribute attribTarget = element.Attribute("target");
			string target = (attribTarget != null) ? attribTarget.Value : null;

			// Self Update is not supported. Skip it.
			if (string.Equals(Path.GetFileName(target), selfFileName, StringComparison.InvariantCultureIgnoreCase))
				return;

			Console.Write("Delete '{0}'... ", target);
			WaitForLockRelease(target);

			File.Delete(target);
			RemoveEmptyDirectory(Path.GetDirectoryName(target));
		}
		private static void PerformUpdate(XElement element)
		{
			XAttribute attribSource = element.Attribute("source");
			XAttribute attribTarget = element.Attribute("target");
			string source = (attribSource != null) ? attribSource.Value : null;
			string target = (attribTarget != null) ? attribTarget.Value : null;

			Console.Write("Copy '{0}' to '{1}'... ", source, target);
			WaitForLockRelease(source, target);

			string targetDir = Path.GetDirectoryName(target);
			if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
				Directory.CreateDirectory(targetDir);

			File.Copy(source, target, true);
		}
		private static void PerformIntegrateProject(XElement element)
		{
			XAttribute attribProject = element.Attribute("project");
			XAttribute attribSolution = element.Attribute("solution");
			string project = (attribProject != null) ? attribProject.Value : null;
			string solution = (attribSolution != null) ? attribSolution.Value : null;

			Console.Write("Integrating '{0}' into '{1}'... ", project, solution);
			WaitForLockRelease(project, solution);

			// ToDo
		}
	}
}
