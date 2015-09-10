using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	public class AssetReImportOperation : AssetImportOperation
	{
		public AssetReImportOperation(IEnumerable<string> inputFiles)
		{
			if (inputFiles.Any(file => !PathOp.IsPathLocatedIn(file, EditorHelper.SourceMediaDirectory)))
			{
				throw new ArgumentException(
					string.Format("Can't re-import Assets using source files that are located outside the Duality '{0}' direcctory", EditorHelper.SourceMediaDirectory),
					"inputFiles");
			}

			string[] inputFileArray = inputFiles.ToArray();
			this.input = new AssetImportInput[inputFileArray.Length];
			for (int i = 0; i < this.input.Length; i++)
			{
				this.input[i] = new AssetImportInput(inputFileArray[i], EditorHelper.SourceMediaDirectory);
			}
		}

		protected override void OnResetWorkingData() { }
		protected override bool OnPerform()
		{
			this.DetermineImportInputMapping();

			if (!this.ImportFromLocalFolder())
				return false;

			return true;
		}

		private void DetermineImportInputMapping()
		{
			AssetImportEnvironment prepareEnv = new AssetImportEnvironment(
				DualityApp.DataDirectory, 
				EditorHelper.SourceMediaDirectory, 
				this.input);
			prepareEnv.IsPrepareStep = true;
			prepareEnv.IsReImport = true;

			this.inputMapping = this.SelectImporter(prepareEnv);
			for (int i = 0; i < this.inputMapping.Count; i++)
			{
				this.inputMapping.Data[i].HandledInputInSourceMedia = this.inputMapping.Data[i].HandledInput;
			}
		}
		private bool ImportFromLocalFolder()
		{
			bool importFailure = false;
			bool anyImported = false;

			this.output = new HashSet<ContentRef<Resource>>();
			for (int assignmentIndex = 0; assignmentIndex < this.inputMapping.Count; assignmentIndex++)
			{
				ImportInputAssignment assignment = this.inputMapping.Data[assignmentIndex];

				// Import the (copied and mapped) files, this importer previously requested to handle
				{
					AssetImportEnvironment importEnv = new AssetImportEnvironment(
						DualityApp.DataDirectory, 
						EditorHelper.SourceMediaDirectory, 
						assignment.HandledInputInSourceMedia);
					importEnv.IsReImport = true;

					try
					{
						assignment.Importer.Import(importEnv);
						anyImported = true;
						
						// Get a list on properly registered output Resources and report warnings on the rest
						List<Resource> expectedOutput = new List<Resource>();
						foreach (var resourceRef in importEnv.OutputResources)
						{
							if (!assignment.ExpectedOutput.Contains(resourceRef))
							{
								Log.Editor.WriteWarning(
									"AssetImporter '{0}' created an unpredicted output Resource: '{1}'. " + Environment.NewLine +
									"This may cause problems in the Asset Management system, especially during Asset re-import. " + Environment.NewLine +
									"Please fix the implementation of the PrepareImport method so it properly calls AddOutput for each predicted output Resource.",
									Log.Type(assignment.Importer.GetType()),
									resourceRef);
							}
							else
							{
								expectedOutput.Add(resourceRef.Res);
							}
						}

						// Collect references to the imported Resources and save them
						foreach (Resource resource in expectedOutput)
						{
							this.output.Add(resource);
						}
					}
					catch (Exception ex)
					{
						importFailure = true;
						Log.Editor.WriteError("An error occurred while trying to re-import files using '{1}': {0}", 
							Log.Exception(ex),
							Log.Type(assignment.Importer.GetType()));
						this.inputMapping.RemoveAt(assignmentIndex);
						assignmentIndex--;
					}
				}
			}

			return anyImported && !importFailure;
		}
	}
}
