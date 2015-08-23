using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	public interface IFileImporter
	{
		bool CanImportFile(string srcFile);
		void ImportFile(string srcFile, string targetName, string targetDir);

		bool CanReImportFile(ContentRef<Resource> r, string srcFile);
		void ReImportFile(ContentRef<Resource> r, string srcFile);

		bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile);
		string[] GetOutputFiles(string srcFile, string targetName, string targetDir);
	}

	public static class FileImportProvider
	{
		private static List<IFileImporter> importers = new List<IFileImporter>();
		
		internal static void Init()
		{
			foreach (TypeInfo genType in DualityEditorApp.GetAvailDualityEditorTypes(typeof(IFileImporter)))
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
			string fileExt = Path.GetExtension(filePath);

			// Find an importer to handle the file import
			IFileImporter importer = importers.FirstOrDefault(i => i.CanImportFile(filePath));
			if (importer == null) return;

			// Determine which Resources are affected
			ContentRef<Resource> affectedResource = null;

			// First, try to guess which Resource is affected using the file name
			{
				string guessedResourceName = ContentProvider.GetNameFromPath(filePath);
				string resourceSearchPattern = "*" + guessedResourceName + "*" + Resource.FileExt;
				foreach (string resourcePath in Directory.EnumerateFiles(DualityApp.DataDirectory, resourceSearchPattern, SearchOption.AllDirectories))
				{
					ContentRef<Resource> resourceRef = new ContentRef<Resource>(null, resourcePath);
					string resourceName = ContentProvider.GetNameFromPath(resourcePath);

					// If the name doesn't match, skip
					if (!string.Equals(resourceName, guessedResourceName, StringComparison.InvariantCultureIgnoreCase))
						continue;
					// If there is no association between source fil and Resource, skip it
					if (!IsUsingSourceFile(importer, resourceRef, filePath, fileExt) && importer.CanReImportFile(resourceRef, filePath))
						continue;
					// If the importer can't handle that Resource with that file, skip it
					if (!importer.CanReImportFile(resourceRef, filePath))
						continue;

					affectedResource = resourceRef;
					break; 
				}
			}

			// No idea yet? Try brute force and check all the available Resources
			if (affectedResource == null)
			{
				foreach (ContentRef<Resource> resRef in ContentProvider.GetAvailableContent<Resource>())
				{
					// If this is default content, skip it
					if (resRef.IsDefaultContent)
						continue;
					// If there is no association between source fil and Resource, skip it
					if (!IsUsingSourceFile(importer, resRef, filePath, fileExt) && importer.CanReImportFile(resRef, filePath))
						continue;
					// If the importer can't handle that Resource with that file, skip it
					if (!importer.CanReImportFile(resRef, filePath))
						continue;

					affectedResource = resRef;
					break; 
				}
			}

			// Re-Import the affected Resources
			List<Resource> touchedResources = null;
			if (affectedResource != null)
			{
				try
				{
					importer.ReImportFile(affectedResource, filePath);
					if (affectedResource.IsLoaded)
					{
						if (touchedResources == null) touchedResources = new List<Resource>();
						touchedResources.Add(affectedResource.Res);
					}
				}
				catch (Exception e) 
				{
					Log.Editor.WriteError("Can't re-import file '{0}': {1}", filePath, Log.Exception(e));
				}
			}

			// Notify the editor that we have modified some Resources
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

		private static bool IsUsingSourceFile(IFileImporter importer, ContentRef<Resource> resourceRef, string srcFilePath, string srcFileExt)
		{
			// Does the Resource or Importer recall to use this file?
			if (importer.IsUsingSrcFile(resourceRef, srcFilePath))
				return true;

			// Does the system suggest that the Resource would use that file if it was opened for editing?
			string resourceSourcePath = SelectSourceFilePath(resourceRef, srcFileExt);
			if (PathHelper.ArePathsEqual(resourceSourcePath, srcFilePath))
				return true;

			// Nope.
			return false;
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
