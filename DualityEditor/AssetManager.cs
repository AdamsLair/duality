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

		public static AssetImportOutput[] ImportAssets(string targetBaseDir, string inputBaseDir, IEnumerable<string> inputFiles)
		{
			// Early-out, if no input files are specified
			if (!inputFiles.Any()) return new AssetImportOutput[0];

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

			return importOperation.Output.ToArray();
		}
		public static AssetImportOutput[] ReImportAssets(IEnumerable<string> localInputFiles)
		{
			// Early-out, if no input files are specified
			if (!localInputFiles.Any()) return new AssetImportOutput[0];
			
			// Set up an import operation and process it
			AssetReImportOperation reimportOperation = new AssetReImportOperation(localInputFiles);
			reimportOperation.ImporterConflictHandler = ResolveImporterConflict;
			bool success = reimportOperation.Perform();
			
			// Notify the editor that we have modified some Resources
			if (reimportOperation.Output.Any())
			{
				IEnumerable<Resource> touchedResources = reimportOperation.Output.Select(item => item.Resource).Res();
				DualityEditorApp.NotifyObjPropChanged(null, new ObjectSelection(touchedResources));
			}

			return reimportOperation.Output.ToArray();
		}

		public delegate IEnumerable<string> SourceFileGetter(Resource resource, string sourceFileBaseDir);
		public delegate void SourceFileSaver(Resource resource, string sourceFileBaseDir);
		public static void OpenSourceFile(ContentRef<Resource> resourceRef, SourceFileGetter getSourceFiles, SourceFileSaver saveSrcToAction)
		{
			Resource resource = resourceRef.Res;
			string mainSourceFileDir = resourceRef.IsDefaultContent ? Path.GetTempPath() : SelectMainSourceFileDir(resource);
			string[] sourceFilePaths = getSourceFiles(resource, mainSourceFileDir).ToArray();

			// Make sure the required directories exist
			foreach (string path in sourceFilePaths)
			{
				string dirName = Path.GetDirectoryName(path);
				if (!Directory.Exists(dirName))
					Directory.CreateDirectory(dirName);
			}
			
			// Perform an export operation
			saveSrcToAction(resource, mainSourceFileDir);

			// If there is only a single source path, open the file right away
			if (sourceFilePaths.Length == 1)
			{
				System.Diagnostics.Process.Start(sourceFilePaths[0]);
			}
			// If there are multiple source paths, just open the base directory
			else
			{
				System.Diagnostics.Process.Start(mainSourceFileDir);
			}
		}
		public static string SelectMainSourceFileDir(ContentRef<Resource> res)
		{
			string filePath = PathHelper.MakeFilePathRelative(res.Path, DualityApp.DataDirectory);
			string fileDir = Path.GetDirectoryName(filePath);
			if (filePath.Contains(".."))
			{
				filePath = Path.GetFileName(filePath);
				fileDir = ".";
			}
			string targetDir = Path.Combine(EditorHelper.SourceMediaDirectory, fileDir);
			return targetDir;
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
