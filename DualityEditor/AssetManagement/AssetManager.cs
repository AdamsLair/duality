using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Duality;
using Duality.IO;

namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// A static helper class that allows to easily perform operations related to the import and export of Resources.
	/// </summary>
	public static class AssetManager
	{
		private static List<IAssetImporter> importers = new List<IAssetImporter>();

		/// <summary>
		/// [GET] Enumerates all known <see cref="IAssetImporter"/>s, each represented by a single instance
		/// and sorted by priority.
		/// </summary>
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

		/// <summary>
		/// Imports the specified set of input source files and returns a set of output Resources.
		/// </summary>
		/// <param name="inputFiles">An enumerable of input source files that should be imported.</param>
		/// <param name="targetBaseDir">The target directory that serves as a base for creating output Resources.</param>
		/// <param name="inputBaseDir">
		/// The base directory of the previously specified input source files, 
		/// which is mapped to the target base directory.
		/// </param>
		/// <returns></returns>
		public static AssetImportOutput[] ImportAssets(IEnumerable<string> inputFiles, string targetBaseDir, string inputBaseDir)
		{
			return ImportAssets(inputFiles, targetBaseDir, inputBaseDir, false);
		}
		/// <summary>
		/// Simulates an import operation in order to determine the expected output. No changes are made.
		/// </summary>
		/// <param name="inputFiles">An enumerable of input source files that should be imported.</param>
		/// <param name="targetBaseDir">The target directory that serves as a base for creating output Resources.</param>
		/// <param name="inputBaseDir">
		/// The base directory of the previously specified input source files, 
		/// which is mapped to the target base directory.
		/// </param>
		/// <returns></returns>
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

		/// <summary>
		/// Re-Imports the specified set of input source files and returns a set of updated output Resources.
		/// </summary>
		/// <param name="localInputFiles">
		/// An enumerable of input source files that are already part of the local media source folder.
		/// </param>
		/// <returns></returns>
		public static AssetImportOutput[] ReImportAssets(IEnumerable<string> localInputFiles)
		{
			return ReImportAssets(localInputFiles, false);
		}
		/// <summary>
		/// Simulates a re-import operation in order to determine the expected output. No changes are made.
		/// </summary>
		/// <param name="localInputFiles">
		/// An enumerable of input source files that are already part of the local media source folder.
		/// </param>
		/// <returns></returns>
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

		/// <summary>
		/// Exports the specified set of input Resources and returns a set of generated or updated output source files.
		/// </summary>
		/// <param name="inputResource">The <see cref="Resource"/> to be exported.</param>
		/// <param name="exportDir">The target directory that serves as a base for creating output source files.</param>
		/// <returns></returns>
		public static string[] ExportAssets(ContentRef<Resource> inputResource, string exportDir = null)
		{
			return ExportAssets(inputResource, exportDir, false);
		}
		/// <summary>
		/// Simulates an export operation in order to determine the expected output. No changes are made.
		/// </summary>
		/// <param name="inputResource">The <see cref="Resource"/> to be exported.</param>
		/// <param name="exportDir">The target directory that serves as a base for creating output source files.</param>
		/// <returns></returns>
		public static string[] SimulateExportAssets(ContentRef<Resource> inputResource, string exportDir = null)
		{
			return ExportAssets(inputResource, exportDir, true);
		}
		private static string[] ExportAssets(ContentRef<Resource> inputResource, string exportDir, bool simulate)
		{
			// Early-out, if the input Resource isn't available
			if (!inputResource.IsAvailable) return new string[0];

			// If there is no export directory set, derive it from the Resource path in the Data folder
			if (exportDir == null)
				exportDir = AssetInternalHelper.GetSourceMediaBaseDir(inputResource);

			bool userAbort = false;
			bool success = false;

			// Set up an export operation and process it
			AssetExportOperation exportOperation = new AssetExportOperation(inputResource.Res, exportDir);
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

		/// <summary>
		/// Determines the source files of the specified <see cref="Resource"/>, which
		/// were used during the most recent import and can be re-used during export and re-import operations.
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public static string[] GetAssetSourceFiles(ContentRef<Resource> resource)
		{
			// Early-out, if the input Resource isn't available
			if (!resource.IsAvailable) return new string[0];

			// If the Resoure provides an explicit hint which source files were used, rely on that.
			AssetInfo assetInfo = resource.Res.AssetInfo;
			string[] sourceFileHint = (assetInfo != null) ? assetInfo.SourceFileHint : null;
			if (sourceFileHint != null && sourceFileHint.Length > 0)
			{
				string baseDir = AssetInternalHelper.GetSourceMediaBaseDir(resource);
				RawList<string> sourceFilePaths = new RawList<string>(sourceFileHint.Length);
				for (int i = 0; i < sourceFileHint.Length; i++)
				{
					if (string.IsNullOrWhiteSpace(sourceFileHint[i]))
						continue;

					string path = Path.Combine(baseDir, sourceFileHint[i].Replace(AssetInfo.FileHintNameVariable, resource.Name));
					sourceFilePaths.Add(path);
				}
				sourceFilePaths.ShrinkToFit();
				return sourceFilePaths.Data;
			}
			
			// As a fallback, use a simulated export to determine the imported source.
			// This is assuming a symmetrical import-export, which isn't always true.
			return AssetManager.SimulateExportAssets(resource);
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
