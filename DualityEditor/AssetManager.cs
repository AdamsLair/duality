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
			importers.StableSort((a, b) => b.Priority > a.Priority ? 1 : -1);
		}
		internal static void Terminate()
		{
			importers.Clear();
		}

		public static IEnumerable<ContentRef<Resource>> ImportAssets(string targetBaseDir, string inputBaseDir, IEnumerable<string> inputFiles)
		{
			// Early-out, if no input files are specified
			if (!inputFiles.Any()) return Enumerable.Empty<ContentRef<Resource>>();

			bool userAbort = false;
			bool success = false;

			// Set up an import operation and process it
			AssetFirstImportOperation importOperation = new AssetFirstImportOperation(targetBaseDir, inputBaseDir, inputFiles);
			importOperation.ConfirmOverwriteCallback = ConfirmOverwriteData;
			importOperation.ImporterConflictHandler = data =>
			{
				IAssetImporter userSelection = ResolveImporterConflict(data);
				if (userSelection == null) userAbort = true;
				return userSelection;
			};
			success = importOperation.Perform();

			// If the operation was a failure, display an error message in the editor UI.
			if (!success && !userAbort)
			{
				MessageBox.Show(
					String.Format(Properties.GeneralRes.Msg_CantImport_Text, inputFiles.First()), 
					Properties.GeneralRes.Msg_CantImport_Caption, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}

			return importOperation.Output;
		}
		public static IEnumerable<ContentRef<Resource>> ReImportAssets(IEnumerable<string> localInputFiles)
		{
			// Early-out, if no input files are specified
			if (!localInputFiles.Any()) return Enumerable.Empty<ContentRef<Resource>>();
			
			// Set up an import operation and process it
			AssetReImportOperation reimportOperation = new AssetReImportOperation(localInputFiles);
			reimportOperation.ImporterConflictHandler = ResolveImporterConflict;
			bool success = reimportOperation.Perform();
			
			// Notify the editor that we have modified some Resources
			if (reimportOperation.Output.Any())
			{
				IEnumerable<Resource> touchedResources = reimportOperation.Output.Res();
				DualityEditorApp.NotifyObjPropChanged(null, new ObjectSelection(touchedResources));
			}

			return reimportOperation.Output;
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
		private static IAssetImporter ResolveImporterConflict(IAssetImporterConflictData data)
		{
			SelectAssetImporterDialog dialog = new SelectAssetImporterDialog(
				data.Importers, 
				data.DefaultImporter, 
				data.InputFiles);
			DialogResult result = dialog.ShowDialog();
			return dialog.SelectedImporter;
		}
	}
}
