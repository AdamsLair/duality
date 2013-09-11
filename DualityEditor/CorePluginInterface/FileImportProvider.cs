using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace DualityEditor.CorePluginInterface
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
		public static bool IsImportFileExisting(string filePath)
		{
			string srcFilePath, targetName, targetDir;
			PrepareImportFilePaths(filePath, out srcFilePath, out targetName, out targetDir);

			// Does the source file already exist?
			if (File.Exists(srcFilePath)) return true;

			// Find an importer and check if one of its output files already exist
			IFileImporter importer = CorePluginRegistry.GetFileImporter(i => i.CanImportFile(srcFilePath));
			return importer != null && importer.GetOutputFiles(srcFilePath, targetName, targetDir).Any(File.Exists);
		}
		public static bool ImportFile(string filePath)
		{
			// Determine & check paths
			string srcFilePath, targetName, targetDir;
			PrepareImportFilePaths(filePath, out srcFilePath, out targetName, out targetDir);

			// Find an importer to handle the file import
			IFileImporter importer = CorePluginRegistry.GetFileImporter(i => i.CanImportFile(srcFilePath));
			if (importer != null)
			{
				try
				{
					// Assure the directory exists
					Directory.CreateDirectory(Path.GetDirectoryName(srcFilePath));

					// Move file from data directory to source directory
					if (File.Exists(srcFilePath))
					{
						File.Copy(filePath, srcFilePath, true);
						File.Delete(filePath);
					}
					else
						File.Move(filePath, srcFilePath);
				} catch (Exception) { return false; }

				// Import it
				importer.ImportFile(srcFilePath, targetName, targetDir);
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
			IFileImporter importer = CorePluginRegistry.GetFileImporter(i => i.CanImportFile(filePath));
			if (importer == null) return;

			// Guess which Resources are affected and check them first
			string fileBaseName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filePath));
			List<ContentRef<Resource>> checkContent = ContentProvider.GetAvailableContent<Resource>();
			for (int i = 0; i < checkContent.Count; ++i)
			{
				ContentRef<Resource> resRef = checkContent[i];
				if (resRef.Name == fileBaseName)
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
		public static void NotifyFileRenamed(string filePathOld, string filePathNew)
		{
			if (string.IsNullOrEmpty(filePathOld)) return;

			// Find an importer to handle the file rename
			IFileImporter importer = CorePluginRegistry.GetFileImporter(i => i.CanImportFile(filePathOld));
			if (importer == null) return;
			
			// Guess which Resources are affected and check them first
			string fileBaseName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filePathOld));
			List<ContentRef<Resource>> checkContent = ContentProvider.GetAvailableContent<Resource>();
			for (int i = 0; i < checkContent.Count; ++i)
			{
				ContentRef<Resource> resRef = checkContent[i];
				if (resRef.Name == fileBaseName)
				{
					checkContent.RemoveAt(i);
					checkContent.Insert(0, resRef);
				}
			}

			// Iterate over all existing Resources to find out which one to modify.
			List<Resource> touchedResources = null;
			foreach (ContentRef<Resource> resRef in checkContent)
			{
				if (resRef.IsDefaultContent) continue;
				if (!importer.IsUsingSrcFile(resRef, filePathOld)) continue;
				try
				{
					Resource res = resRef.Res;
					if (res.SourcePath == filePathOld)
					{
						res.SourcePath = filePathNew;
						if (touchedResources == null) touchedResources = new List<Resource>();
						touchedResources.Add(res);
						// Multiple Resources referring to a single source file shouldn't happen
						// in the current implementation of FileImport and Resource system.
						// Might change later.
						break; 
					}
				}
				catch (Exception) 
				{
					Log.Editor.WriteError("There was an error internally renaming a source file '{0}' to '{1}'", filePathOld, filePathNew);
				}
			}

			if (touchedResources != null)
			{
				DualityEditorApp.FlagResourceUnsaved(touchedResources);
			}
		}
		
		public static void OpenSourceFile(ContentRef<Resource> resourceRef, string srcFileExt, Action<string> saveSrcToAction)
		{
			// Default content: Use temporary location
			if (resourceRef.IsDefaultContent)
			{
				string tmpLoc = Path.Combine(Path.GetTempPath(), resourceRef.Path.Replace(':', '_')) + srcFileExt;
				saveSrcToAction(tmpLoc);
				System.Diagnostics.Process.Start(tmpLoc);
			}
			// Other content: Use permanent src file location
			else
			{
				Resource resource = resourceRef.Res;
				string srcFilePath = resource.SourcePath;
				if (String.IsNullOrEmpty(srcFilePath) || !File.Exists(srcFilePath))
				{
					srcFilePath = GenerateSourceFilePath(resourceRef, srcFileExt);
					Directory.CreateDirectory(Path.GetDirectoryName(srcFilePath));
					resource.SourcePath = srcFilePath;
				}

				if (srcFilePath != null)
				{
					saveSrcToAction(srcFilePath);
					System.Diagnostics.Process.Start(srcFilePath);
				}
			}
		}
		public static string GenerateSourceFilePath(ContentRef<Resource> r, string srcFileExt)
		{
			string filePath = PathHelper.MakeFilePathRelative(r.Path, DualityApp.DataDirectory);
			string fileDir = Path.GetDirectoryName(filePath);
			if (filePath.Contains(".."))
			{
				filePath = Path.GetFileName(filePath);
				fileDir = ".";
			}
			return PathHelper.GetFreePath(Path.Combine(Path.Combine(EditorHelper.SourceMediaDirectory, fileDir), r.Name), srcFileExt);
		}

		private static void PrepareImportFilePaths(string filePath, out string srcFilePath, out string targetName, out string targetDir)
		{
			srcFilePath = PathHelper.MakeFilePathRelative(filePath, DualityApp.DataDirectory);
			if (srcFilePath.Contains("..")) srcFilePath = Path.GetFileName(srcFilePath);

			targetDir = Path.GetDirectoryName(Path.Combine(DualityApp.DataDirectory, srcFilePath));
			targetName = Path.GetFileNameWithoutExtension(filePath);

			srcFilePath = PathHelper.GetFreePath(
				Path.Combine(EditorHelper.SourceMediaDirectory, Path.GetFileNameWithoutExtension(srcFilePath)), 
				Path.GetExtension(srcFilePath));
		}
	}
}
