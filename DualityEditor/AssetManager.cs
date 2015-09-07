using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	public static class AssetManager
	{
		private static List<IAssetImporter> importers = new List<IAssetImporter>();
		public static IEnumerable<IAssetImporter> Importers
		{
			get { return importers; }
		}
		
		internal static void Init()
		{
			foreach (TypeInfo genType in DualityEditorApp.GetAvailDualityEditorTypes(typeof(IAssetImporter)))
			{
				if (genType.IsAbstract) continue;
				IAssetImporter gen = genType.CreateInstanceOf() as IAssetImporter;
				if (gen != null) importers.Add(gen);
			}
		}
		internal static void Terminate()
		{
			importers.Clear();
		}

		public static IEnumerable<ContentRef<Resource>> ImportAssets(string targetBaseDir, string inputBaseDir, IEnumerable<string> inputFiles)
		{
			// Early-out, if no input files are specified
			if (!inputFiles.Any()) return Enumerable.Empty<ContentRef<Resource>>();

			// Set up an import operation and process it
			AssetImportOperation importOperation = new AssetImportOperation(targetBaseDir, inputBaseDir, inputFiles);
			importOperation.ConfirmOverwriteCallback = ConfirmOverwriteData;
			bool success = importOperation.Perform();

			// If the operation was a failure, display an error message in the editor UI.
			if (!success)
			{
				MessageBox.Show(
					String.Format(Properties.GeneralRes.Msg_CantImport_Text, inputFiles.First()), 
					Properties.GeneralRes.Msg_CantImport_Caption, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}

			return importOperation.OutputResources;
		}
		public static IEnumerable<ContentRef<Resource>> ReImportAssets(IEnumerable<string> localInputFiles)
		{
			// Early-out, if no input files are specified
			if (!localInputFiles.Any()) return Enumerable.Empty<ContentRef<Resource>>();
			
			return Enumerable.Empty<ContentRef<Resource>>();

			/* 
			 * Notes:
			 * 
			 * - In the first step, perform a regular import preparation step with the specified input
			 *   paths, in order to let importers determine Resources to update, and a mapping to files
			 *   
			 * - The only case where this isn't sufficient is when users manually restructure their
			 *   Source/Media directory, in which case it wouldn't be expected behavior for Duality to
			 *   still react to the moved / renamed files.
			 *   
			 * - Thus, ReImport will be a lot easier to do with the new API.
			 */

			// -------------------------------------------------------------------------------------------------------

			//string fileExt = Path.GetExtension(filePath);

			//// Find an importer to handle the file import
			//IAssetImporter importer = importers.FirstOrDefault(i => i.CanImportFile(filePath));
			//if (importer == null) return;

			//// Determine which Resources are affected
			//ContentRef<Resource> affectedResource = null;

			//// First, try to guess which Resource is affected using the file name
			//{
			//	string guessedResourceName = ContentProvider.GetNameFromPath(filePath);
			//	string resourceSearchPattern = "*" + guessedResourceName + "*" + Resource.FileExt;
			//	foreach (string resourcePath in Directory.EnumerateFiles(DualityApp.DataDirectory, resourceSearchPattern, SearchOption.AllDirectories))
			//	{
			//		ContentRef<Resource> resourceRef = new ContentRef<Resource>(null, resourcePath);
			//		string resourceName = ContentProvider.GetNameFromPath(resourcePath);

			//		// If the name doesn't match, skip
			//		if (!string.Equals(resourceName, guessedResourceName, StringComparison.InvariantCultureIgnoreCase))
			//			continue;
			//		// If there is no association between source fil and Resource, skip it
			//		if (!IsUsingSourceFile(importer, resourceRef, filePath, fileExt) && importer.CanReImportFile(resourceRef, filePath))
			//			continue;
			//		// If the importer can't handle that Resource with that file, skip it
			//		if (!importer.CanReImportFile(resourceRef, filePath))
			//			continue;

			//		affectedResource = resourceRef;
			//		break; 
			//	}
			//}

			//// No idea yet? Try brute force and check all the available Resources
			//if (affectedResource == null)
			//{
			//	foreach (ContentRef<Resource> resRef in ContentProvider.GetAvailableContent<Resource>())
			//	{
			//		// If this is default content, skip it
			//		if (resRef.IsDefaultContent)
			//			continue;
			//		// If there is no association between source fil and Resource, skip it
			//		if (!IsUsingSourceFile(importer, resRef, filePath, fileExt) && importer.CanReImportFile(resRef, filePath))
			//			continue;
			//		// If the importer can't handle that Resource with that file, skip it
			//		if (!importer.CanReImportFile(resRef, filePath))
			//			continue;

			//		affectedResource = resRef;
			//		break; 
			//	}
			//}

			//// Re-Import the affected Resources
			//List<Resource> touchedResources = null;
			//if (affectedResource != null)
			//{
			//	try
			//	{
			//		importer.ReImportFile(affectedResource, filePath);
			//		if (affectedResource.IsLoaded)
			//		{
			//			if (touchedResources == null) touchedResources = new List<Resource>();
			//			touchedResources.Add(affectedResource.Res);
			//		}
			//	}
			//	catch (Exception e) 
			//	{
			//		Log.Editor.WriteError("Can't re-import file '{0}': {1}", filePath, Log.Exception(e));
			//	}
			//}

			//// Notify the editor that we have modified some Resources
			//if (touchedResources != null)
			//{
			//	DualityEditorApp.NotifyObjPropChanged(null, new ObjectSelection((IEnumerable<object>)touchedResources));
			//}
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

		private static bool ConfirmOverwriteData()
		{
			DialogResult result = MessageBox.Show(
				String.Format(Properties.GeneralRes.Msg_ImportConfirmOverwrite_Text), 
				Properties.GeneralRes.Msg_ImportConfirmOverwrite_Caption, 
				MessageBoxButtons.YesNo, 
				MessageBoxIcon.Warning);
			return result == DialogResult.Yes;
		}
	}
}
