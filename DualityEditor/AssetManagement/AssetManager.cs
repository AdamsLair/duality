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

		public static AssetImportOutput[] ImportAssets(IEnumerable<string> inputFiles, string targetBaseDir, string inputBaseDir)
		{
			return ImportAssets(inputFiles, targetBaseDir, inputBaseDir, false);
		}
		public static AssetImportOutput[] SimulateImportAssets(IEnumerable<string> inputFiles, string targetBaseDir, string inputBaseDir)
		{
			return ImportAssets(inputFiles, targetBaseDir, inputBaseDir, true);
		}
		private static AssetImportOutput[] ImportAssets(IEnumerable<string> inputFiles, string targetBaseDir, string inputBaseDir, bool simulate)
		{
			// Early-out, if no input files are specified
			if (!inputFiles.Any()) return new AssetImportOutput[0];

			bool userAbort = false;
			bool success = false;

			// Set up an import operation and process it
			AssetFirstImportOperation importOperation = new AssetFirstImportOperation(inputFiles, targetBaseDir, inputBaseDir);
			importOperation.ConfirmOverwriteCallback = ConfirmOverwriteData;
			importOperation.ImporterConflictHandler = data =>
			{
				IAssetImporter userSelection = ResolveImporterConflict(data);
				if (userSelection == null) userAbort = true;
				return userSelection;
			};
			success = simulate ? 
				importOperation.SimulatePerform() : 
				importOperation.Perform();

			// If the operation was a failure, display an error message in the editor UI.
			if (!simulate && !success && !userAbort)
			{
				MessageBox.Show(
					string.Format(Properties.GeneralRes.Msg_CantImport_Text, inputFiles.First()), 
					Properties.GeneralRes.Msg_CantImport_Caption, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}

			return importOperation.Output.ToArray();
		}

		public static AssetImportOutput[] ReImportAssets(IEnumerable<string> localInputFiles)
		{
			return ReImportAssets(localInputFiles, false);
		}
		public static AssetImportOutput[] SimulateReImportAssets(IEnumerable<string> localInputFiles)
		{
			return ReImportAssets(localInputFiles, true);
		}
		private static AssetImportOutput[] ReImportAssets(IEnumerable<string> localInputFiles, bool simulate)
		{
			// Early-out, if no input files are specified
			if (!localInputFiles.Any()) return new AssetImportOutput[0];
			
			// Set up an import operation and process it
			AssetReImportOperation reimportOperation = new AssetReImportOperation(localInputFiles);
			reimportOperation.ImporterConflictHandler = ResolveImporterConflict;
			bool success = simulate ? 
				reimportOperation.SimulatePerform() : 
				reimportOperation.Perform();
			
			// Notify the editor that we have modified some Resources
			if (!simulate && reimportOperation.Output.Any())
			{
				IEnumerable<Resource> touchedResources = reimportOperation.Output.Select(item => item.Resource).Res();
				DualityEditorApp.NotifyObjPropChanged(null, new ObjectSelection(touchedResources));
			}

			return reimportOperation.Output.ToArray();
		}

		public static string[] ExportAssets(ContentRef<Resource> inputResource, string sourceBaseDir = null)
		{
			return ExportAssets(inputResource, sourceBaseDir, false);
		}
		public static string[] SimulateExportAssets(ContentRef<Resource> inputResource, string sourceBaseDir = null)
		{
			return ExportAssets(inputResource, sourceBaseDir, true);
		}
		private static string[] ExportAssets(ContentRef<Resource> inputResource, string sourceBaseDir, bool simulate)
		{
			// Early-out, if the input Resource isn't available
			if (!inputResource.IsAvailable) return new string[0];

			if (sourceBaseDir == null)
			{
				string resDir = Path.GetDirectoryName(inputResource.Path);
				sourceBaseDir = Path.Combine(
					EditorHelper.SourceMediaDirectory,
					PathHelper.MakeFilePathRelative(resDir, DualityApp.DataDirectory));
			}

			bool userAbort = false;
			bool success = false;

			// Set up an export operation and process it
			AssetExportOperation exportOperation = new AssetExportOperation(inputResource.Res, sourceBaseDir);
			exportOperation.ImporterConflictHandler = data =>
			{
				IAssetImporter userSelection = ResolveImporterConflict(data);
				if (userSelection == null) userAbort = true;
				return userSelection;
			};
			success = simulate ?
				exportOperation.SimulatePerform() :
				exportOperation.Perform();

			// If the operation was a failure, display an error message in the editor UI.
			if (!simulate && !success && !userAbort)
			{
				MessageBox.Show(
					string.Format(Properties.GeneralRes.Msg_CantExport_Text, inputResource.Path), 
					Properties.GeneralRes.Msg_CantExport_Caption, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}

			return exportOperation.OutputPaths.ToArray();
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
