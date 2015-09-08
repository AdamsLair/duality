using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	public class AssetReImportOperation
	{
		private AssetImportInput[] input = null;
		private RawList<ReImportInputAssignment> inputMapping = null;
		private HashSet<ContentRef<Resource>> output = null;


		public IEnumerable<AssetImportInput> Input
		{
			get { return this.input; }
		}
		public IEnumerable<ContentRef<Resource>> Output
		{
			get { return this.output ?? Enumerable.Empty<ContentRef<Resource>>(); }
		}


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

		public bool Perform()
		{
			this.ResetWorkingData();

			this.DetermineImportInputMapping();
			bool importSuccess = this.ImportFromLocalFolder();

			// Clean up, in case we've done heavy-duty work
			GC.Collect();
			GC.WaitForPendingFinalizers();

			return importSuccess;
		}

		private void ResetWorkingData()
		{
			this.inputMapping = null;
			this.output = null;
		}
		private void DetermineImportInputMapping()
		{
			this.inputMapping = new RawList<ReImportInputAssignment>();

			List<AssetImportInput> unhandledInput = this.input.ToList();
			while (unhandledInput.Count > 0)
			{
				AssetImportEnvironment prepareEnv = new AssetImportEnvironment(
					DualityApp.DataDirectory, 
					EditorHelper.SourceMediaDirectory, 
					unhandledInput);
				prepareEnv.IsPrepareStep = true;
				prepareEnv.IsReImport = true;

				// Find an importer to handle some or all of the unhandled input files
				bool foundImporter = false;
				foreach (IAssetImporter importer in AssetManager.Importers)
				{
					try
					{
						importer.PrepareImport(prepareEnv);
					}
					catch (Exception ex)
					{
						Log.Editor.WriteError("An error occurred in the preparation step of an AssetImporter: {0}", Log.Exception(ex));
						continue;
					}

					// See which files the current importer is able to handle
					AssetImportInput[] handledInput = prepareEnv.HandledInput.ToArray();
					if (handledInput.Length > 0)
					{
						// Remove the handled input from the queue
						for (int i = 0; i < handledInput.Length; i++)
						{
							unhandledInput.Remove(handledInput[i]);
						}

						// We have found a valid input assignment for a set of files
						foundImporter = true;
						this.inputMapping.Add(new ReImportInputAssignment
						{
							Importer = importer,
							ExpectedOutput = prepareEnv.OutputResources.ToArray(),
							HandledInputInSourceMedia = handledInput
						});
						break;
					}
				}

				// If no suitable importer was found to handle the remaining files, stop
				if (!foundImporter)
					break;
			}
		}
		private bool ImportFromLocalFolder()
		{
			bool importFailure = false;
			bool anyImported = false;

			this.output = new HashSet<ContentRef<Resource>>();
			for (int assignmentIndex = 0; assignmentIndex < this.inputMapping.Count; assignmentIndex++)
			{
				ReImportInputAssignment assignment = this.inputMapping.Data[assignmentIndex];

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
						Log.Editor.WriteError("An error occurred while trying to re-import files: {0}", Log.Exception(ex));
						this.inputMapping.RemoveAt(assignmentIndex);
						assignmentIndex--;
					}
				}
			}

			return anyImported && !importFailure;
		}

		private struct ReImportInputAssignment
		{
			public IAssetImporter Importer;
			public AssetImportInput[] HandledInputInSourceMedia;
			public ContentRef<Resource>[] ExpectedOutput;
		}
	}
}
