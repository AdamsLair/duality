using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	public class AssetImportOperation
	{
		private string targetDir = null;
		private string sourceDir = null;
		private string inputBaseDir = null;
		private AssetImportInput[] input = null;
		private RawList<ImportInputAssignment> inputMapping = null;
		private HashSet<ContentRef<Resource>> output = null;
		private Func<bool> confirmOverwrite = null;


		public string TargetDirectory
		{
			get { return this.targetDir; }
		}
		public string SourceDirectory
		{
			get { return this.sourceDir; }
		}
		public IEnumerable<AssetImportInput> Input
		{
			get { return this.input; }
		}
		public IEnumerable<ContentRef<Resource>> OutputResources
		{
			get { return this.output ?? Enumerable.Empty<ContentRef<Resource>>(); }
		}
		public Func<bool> ConfirmOverwriteCallback
		{
			get { return this.confirmOverwrite; }
			set { this.confirmOverwrite = value; }
		}


		public AssetImportOperation(string targetBaseDir, string inputBaseDir, IEnumerable<string> inputFiles)
		{
			if (!PathOp.ArePathsEqual(targetBaseDir, DualityApp.DataDirectory) && !PathOp.IsPathLocatedIn(targetBaseDir, DualityApp.DataDirectory))
			{
				throw new ArgumentException(
					string.Format("The specified base directory needs to be located within the Duality '{0}' direcctory", DualityApp.DataDirectory),
					"targetBaseDir");
			}

			if (PathOp.ArePathsEqual(targetBaseDir, DualityApp.DataDirectory))
			{
				this.targetDir = DualityApp.DataDirectory;
				this.sourceDir = EditorHelper.SourceMediaDirectory;
			}
			else
			{
				string baseDir = PathHelper.MakeFilePathRelative(targetBaseDir, DualityApp.DataDirectory);
				this.targetDir = Path.Combine(DualityApp.DataDirectory, baseDir);
				this.sourceDir = Path.Combine(EditorHelper.SourceMediaDirectory, baseDir);
			}

			string[] inputFileArray = inputFiles.ToArray();
			this.inputBaseDir = inputBaseDir;
			this.input = new AssetImportInput[inputFileArray.Length];
			for (int i = 0; i < this.input.Length; i++)
			{
				this.input[i] = new AssetImportInput(inputFileArray[i], inputBaseDir);
			}
		}

		public bool Perform()
		{
			this.ResetWorkingData();

			bool importSuccess = false;
			while (true)
			{
				this.DetermineImportInputMapping();
				this.DetermineLocalInputFilePaths();

				if (this.DoesOverwriteData() && !this.InvokeConfirmOverwrite())
					break;
				if (!this.CopySourceToLocalFolder())
					break;
				if (!this.ImportFromLocalFolder())
					break;

				importSuccess = true;
				break;
			}

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
			this.inputMapping = new RawList<ImportInputAssignment>();

			List<AssetImportInput> unhandledInput = this.input.ToList();
			while (unhandledInput.Count > 0)
			{
				DefaultAssetImportEnvironment prepareEnv = new DefaultAssetImportEnvironment(
					this.targetDir, 
					this.sourceDir, 
					unhandledInput);

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
						foreach (AssetImportInput item in handledInput)
						{
							unhandledInput.Remove(item);
						}

						foundImporter = true;
						this.inputMapping.Add(new ImportInputAssignment
						{
							Importer = importer,
							ExpectedOutput = prepareEnv.OutputResources.ToArray(),
							HandledInput = handledInput
						});
						break;
					}
				}

				// If no suitable importer was found to handle the remaining files, stop
				if (!foundImporter)
					break;
			}
		}
		private void DetermineLocalInputFilePaths()
		{
			for (int assignmentIndex = 0; assignmentIndex < this.inputMapping.Count; assignmentIndex++)
			{
				ImportInputAssignment assignment = inputMapping.Data[assignmentIndex];

				// Create a relative mapping for each of the handled files
				AssetImportInput[] handledInputInSourceMedia = new AssetImportInput[assignment.HandledInput.Length];
				for (int i = 0; i < assignment.HandledInput.Length; i++)
				{
					handledInputInSourceMedia[i] = new AssetImportInput(
						Path.Combine(sourceDir, assignment.HandledInput[i].RelativePath),
						assignment.HandledInput[i].RelativePath,
						assignment.HandledInput[i].FullAssetName);
				}

				// Assign the determined paths back to the input mapping
				this.inputMapping.Data[assignmentIndex].HandledInputInSourceMedia = handledInputInSourceMedia;
			}
		}
		private bool DoesOverwriteData()
		{
			for (int assignmentIndex = 0; assignmentIndex < this.inputMapping.Count; assignmentIndex++)
			{
				ImportInputAssignment assignment = this.inputMapping.Data[assignmentIndex];

				// Determine if we'll be overwriting any Resource files
				for (int i = 0; i < assignment.ExpectedOutput.Length; i++)
				{
					if (File.Exists(assignment.ExpectedOutput[i].Path))
					{
						return true;
					}
				}

				// Determine if we'll be overwriting any source / media files
				for (int i = 0; i < assignment.HandledInput.Length; i++)
				{
					if (File.Exists(assignment.HandledInputInSourceMedia[i].Path))
					{
						if (!PathHelper.FilesEqual(assignment.HandledInput[i].Path, assignment.HandledInputInSourceMedia[i].Path))
						{
							return true;
						}
					}
				}
			}

			return false;
		}
		private bool CopySourceToLocalFolder()
		{
			for (int assignmentIndex = 0; assignmentIndex < this.inputMapping.Count; assignmentIndex++)
			{
				ImportInputAssignment assignment = this.inputMapping.Data[assignmentIndex];

				// Copy all handled files to the media / source directory
				try
				{
					// Copy each handled file into source / media, while preserving the original relative structure
					for (int i = 0; i < assignment.HandledInput.Length; i++)
					{
						string filePath = assignment.HandledInput[i].Path;
						string filePathInSourceMedia = assignment.HandledInputInSourceMedia[i].Path;
						string dirPathInSourceMedia = Path.GetDirectoryName(filePathInSourceMedia);

						// If there already is a similarly named file in the source directory, delete it.
						if (File.Exists(filePathInSourceMedia))
							File.Delete(filePathInSourceMedia);

						// Assure the media / source directory exists
						Directory.CreateDirectory(dirPathInSourceMedia);

						// Copy file from its original location to the source / media directory
						File.Copy(filePath, filePathInSourceMedia);
					}
				}
				catch (Exception ex)
				{
					Log.Editor.WriteError("Can't copy source files to the media / source directory: {0}", Log.Exception(ex));
					return false;
				}
			}

			return true;
		}
		private bool ImportFromLocalFolder()
		{
			bool importSuccess = false;

			this.output = new HashSet<ContentRef<Resource>>();
			for (int assignmentIndex = 0; assignmentIndex < this.inputMapping.Count; assignmentIndex++)
			{
				ImportInputAssignment assignment = this.inputMapping.Data[assignmentIndex];

				// Import the (copied and mapped) files, this importer previously requested to handle
				{
					DefaultAssetImportEnvironment importEnv = new DefaultAssetImportEnvironment(this.targetDir, this.sourceDir, assignment.HandledInputInSourceMedia);
					try
					{
						assignment.Importer.Import(importEnv);
						importSuccess = true;
					}
					catch (Exception ex)
					{
						importSuccess = false;
						Log.Editor.WriteError("An error occurred while trying to import files: {0}", Log.Exception(ex));
						break;
					}

					// Collect references to the imported Resources, so we can return them later
					if (importSuccess)
					{
						foreach (var resourceRef in importEnv.OutputResources)
						{
							this.output.Add(resourceRef);
							if (!assignment.ExpectedOutput.Contains(resourceRef))
							{
								Log.Editor.WriteError(
									"AssetImporter '{0}' created an unpredicted output Resource: '{1}'. " + Environment.NewLine +
									"This may cause problems in the Asset Management system, especially during Asset re-import. " + Environment.NewLine +
									"Please fix the implementation of the PrepareImport method so it properly calls AddOutput for each predicted output Resource.",
									Log.Type(assignment.Importer.GetType()),
									resourceRef);
							}
						}
					}
				}
			}

			return importSuccess;
		}

		private bool InvokeConfirmOverwrite()
		{
			if (this.confirmOverwrite != null)
				return this.confirmOverwrite();
			else
				return false;
		}

		private struct ImportInputAssignment
		{
			public IAssetImporter Importer;
			public AssetImportInput[] HandledInput;
			public AssetImportInput[] HandledInputInSourceMedia;
			public ContentRef<Resource>[] ExpectedOutput;
		}
	}
}
