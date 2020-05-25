using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor.AssetManagement
{
	public class AssetReImportOperation : AssetImportOperation
	{
		public AssetReImportOperation(IEnumerable<string> inputFiles)
		{
			if (inputFiles.Any(file => !PathOp.IsPathLocatedIn(file, DualityEditorApp.EditorAppData.ImportPath)))
			{
				throw new ArgumentException(
					string.Format("Can't re-import Assets using source files that are located outside the Duality '{0}' direcctory", DualityEditorApp.EditorAppData.ImportPath),
					"inputFiles");
			}

			string[] inputFileArray = inputFiles.ToArray();
			this.input = new AssetImportInput[inputFileArray.Length];
			for (int i = 0; i < this.input.Length; i++)
			{
				this.input[i] = new AssetImportInput(inputFileArray[i], DualityEditorApp.EditorAppData.ImportPath);
			}
		}

		protected override void OnResetWorkingData() { }
		protected override bool OnPerform()
		{
			if (!this.DetermineImportInputMapping())
				return false;

			if (!this.ImportFromLocalFolder())
				return false;

			return true;
		}
		protected override bool OnSimulatePerform()
		{
			return this.DetermineImportInputMapping();
		}

		private bool DetermineImportInputMapping()
		{
			AssetImportEnvironment prepareEnv = new AssetImportEnvironment(
				DualityApp.DataDirectory,
				DualityEditorApp.EditorAppData.ImportPath, 
				this.input);
			prepareEnv.IsPrepareStep = true;
			prepareEnv.IsReImport = true;

			this.inputMapping = this.SelectImporter(prepareEnv);

			// Filter out mappings without existing target Resources - can't Re-Import what isn't there.
			for (int i = this.inputMapping.Count - 1; i >= 0; i--)
			{
				if (this.inputMapping.Data[i].ExpectedOutput.Any(item => !item.Resource.IsAvailable))
				{
					this.inputMapping.RemoveAt(i);
				}
			}

			// Since this is a Re-Import, all input files are located in source / media
			for (int i = 0; i < this.inputMapping.Count; i++)
			{
				this.inputMapping.Data[i].HandledInputInSourceMedia = this.inputMapping.Data[i].HandledInput;
			}

			return this.inputMapping.Count > 0;
		}
		private bool ImportFromLocalFolder()
		{
			bool failed = false;

			// Import all files this importer previously requested to handle
			this.output = new List<AssetImportOutput>();
			for (int assignmentIndex = 0; assignmentIndex < this.inputMapping.Count; assignmentIndex++)
			{
				ImportInputAssignment assignment = this.inputMapping.Data[assignmentIndex];
				AssetImportEnvironment importEnv = new AssetImportEnvironment(
					DualityApp.DataDirectory,
					DualityEditorApp.EditorAppData.ImportPath, 
					assignment.HandledInputInSourceMedia);
				importEnv.IsReImport = true;
				
				if (!this.RunImporter(importEnv, assignment, this.output))
					failed = true;
			}

			return !failed;
		}
	}
}
