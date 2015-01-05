using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor
{
	public interface IFileImporter
	{
		bool CanImportFile(string srcFile);
		void ImportFile(string srcFile, string targetName, string targetDir);
		string[] GetOutputFiles(string srcFile, string targetName, string targetDir);

		bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile);
		void ReimportFile(ContentRef<Resource> r, string srcFile);
	}

	public static class FileImportProvider
	{
		private static List<IFileImporter> importers = new List<IFileImporter>();
		
		internal static void Init()
		{
			foreach (Type genType in DualityEditorApp.GetAvailDualityEditorTypes(typeof(IFileImporter)))
			{
				if (genType.IsAbstract) continue;
				IFileImporter gen = genType.CreateInstanceOf() as IFileImporter;
				if (gen != null) importers.Add(gen);
			}
		}
		internal static void Terminate()
		{
			importers.Clear();
		}

		public static bool DoesImportOverwriteData(string filePath)
		{
			string srcFilePath, targetName, targetDir;
			PrepareImportFilePaths(filePath, out srcFilePath, out targetName, out targetDir);

			// Does the source file already exist?
			if (File.Exists(srcFilePath))
			{
				// If they're not the same file, this would overwrite existing data.
				return !PathHelper.FilesEqual(filePath, srcFilePath);
			}

			// Find an importer and check if one of its output files already exist
			IFileImporter importer = importers.FirstOrDefault(i => i.CanImportFile(srcFilePath));
			return importer != null && importer.GetOutputFiles(srcFilePath, targetName, targetDir).Any(File.Exists);
		}
		public static bool ImportFile(string filePath)
		{
			// Determine & check paths
			string srcFilePath, targetName, targetDir;
			PrepareImportFilePaths(filePath, out srcFilePath, out targetName, out targetDir);

			// Find an importer to handle the file import
			IFileImporter importer = importers.FirstOrDefault(i => i.CanImportFile(srcFilePath));
			if (importer != null)
			{
				try
				{
					// Assure the directory exists
					Directory.CreateDirectory(Path.GetDirectoryName(srcFilePath));

					// If there already is a similarly named file in the source directory, delete it.
					if (File.Exists(srcFilePath))
						File.Delete(srcFilePath);

					// Move file from data directory to source directory
					File.Move(filePath, srcFilePath);

					// Import it from there
					importer.ImportFile(srcFilePath, targetName, targetDir);
				}
				catch (Exception ex)
				{
					Log.Editor.WriteError("An error occurred while trying to import file {1}: {0}", Log.Exception(ex), srcFilePath);
					return false;
				}

				GC.Collect();
				GC.WaitForPendingFinalizers();
				return true;
			}
			else
				return false;
		}
		public static void ReimportFile(string filePath)
		{
			// Find an importer to handle the file import
			IFileImporter importer = importers.FirstOrDefault(i => i.CanImportFile(filePath));
			if (importer == null) return;

			// Guess which Resources are affected and check them first
			string resourceName = ContentProvider.GetNameFromPath(filePath);
			List<ContentRef<Resource>> checkContent = ContentProvider.GetAvailableContent<Resource>();
			for (int i = 0; i < checkContent.Count; ++i)
			{
				ContentRef<Resource> resRef = checkContent[i];
				if (resRef.Name == resourceName)
				{
					checkContent.RemoveAt(i);
					checkContent.Insert(0, resRef);
				}
			}

			// Iterate over all existing Resources to find out which one to ReImport.
			List<Resource> touchedResources = null;
			foreach (ContentRef<Resource> resRef in checkContent)
			{
				if (resRef.IsDefaultContent) continue;
				if (!importer.IsUsingSrcFile(resRef, filePath)) continue;
				try
				{
					importer.ReimportFile(resRef, filePath);
					if (resRef.IsLoaded)
					{
						if (touchedResources == null) touchedResources = new List<Resource>();
						touchedResources.Add(resRef.Res);
					}
					// Multiple Resources referring to a single source file shouldn't happen
					// in the current implementation of FileImport and Resource system.
					// Might change later.
					break; 
				}
				catch (Exception) 
				{
					Log.Editor.WriteError("Can't re-import file '{0}'", filePath);
				}
			}

			if (touchedResources != null)
			{
				DualityEditorApp.NotifyObjPropChanged(null, new ObjectSelection((IEnumerable<object>)touchedResources));
			}
		}
		
		public static void OpenSourceFile(ContentRef<Resource> resourceRef, string srcFileExt, Action<string> saveSrcToAction)
		{
			// Default content: Use temporary location
			if (resourceRef.IsDefaultContent)
			{
				string tmpLoc = Path.Combine(Path.GetTempPath(), resourceRef.Path.Replace(':', '_')) + srcFileExt;
				Directory.CreateDirectory(Path.GetDirectoryName(tmpLoc));
				saveSrcToAction(tmpLoc);
				System.Diagnostics.Process.Start(tmpLoc);
			}
			// Other content: Use permanent src file location
			else
			{
				Resource resource = resourceRef.Res;
				string srcFilePath = resource.SourcePath;

				// If the Resource to open doesn't know where its source file is, search or create it
				if (String.IsNullOrEmpty(srcFilePath) || !File.Exists(srcFilePath))
				{
					// Determine the desired source file path
					srcFilePath = SelectSourceFilePath(resourceRef, srcFileExt);

					// If there already is a matching file in the desired path, it's probably been relocated there
					if (File.Exists(srcFilePath))
					{
						// Do nothing and simply use the existing file.
					}
					// Otherwise, export the Resource to the desired path
					else
					{
						Directory.CreateDirectory(Path.GetDirectoryName(srcFilePath));
						saveSrcToAction(srcFilePath);
					}

					// Keep in mind where we left the source file
					resource.SourcePath = srcFilePath;
					DualityEditorApp.FlagResourceUnsaved(resource);
				}

				// Open the source file
				System.Diagnostics.Process.Start(srcFilePath);
			}
		}
		public static string SelectSourceFilePath(ContentRef<Resource> r, string srcFileExt)
		{
			string filePath = PathHelper.MakeFilePathRelative(r.Path, DualityApp.DataDirectory);
			string fileDir = Path.GetDirectoryName(filePath);
			if (filePath.Contains(".."))
			{
				filePath = Path.GetFileName(filePath);
				fileDir = ".";
			}
			string targetPathWithoutExt = Path.Combine(Path.Combine(EditorHelper.SourceMediaDirectory, fileDir), r.Name);
			return targetPathWithoutExt + srcFileExt;
		}

		private static void PrepareImportFilePaths(string filePath, out string srcFilePath, out string targetName, out string targetDir)
		{
			srcFilePath = PathHelper.MakeFilePathRelative(filePath, DualityApp.DataDirectory);
			if (srcFilePath.Contains("..")) srcFilePath = Path.GetFileName(srcFilePath);

			targetDir = Path.GetDirectoryName(Path.Combine(DualityApp.DataDirectory, srcFilePath));
			targetName = Path.GetFileNameWithoutExtension(filePath);

			srcFilePath = Path.Combine(EditorHelper.SourceMediaDirectory, srcFilePath);
		}
	}
}
