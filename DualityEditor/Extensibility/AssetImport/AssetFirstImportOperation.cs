using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	public class AssetFirstImportOperation : AssetImportOperation
	{
		private string targetDir = null;
		private string sourceDir = null;
		private string inputBaseDir = null;
		private Dictionary<string,string> assetRenameMap = null;
		private Func<bool> confirmOverwrite = null;


		public string TargetDirectory
		{
			get { return this.targetDir; }
		}
		public string SourceDirectory
		{
			get { return this.sourceDir; }
		}
		public Func<bool> ConfirmOverwriteCallback
		{
			get { return this.confirmOverwrite; }
			set { this.confirmOverwrite = value; }
		}


		public AssetFirstImportOperation(string targetBaseDir, string inputBaseDir, IEnumerable<string> inputFiles)
		{
			if (!PathOp.ArePathsEqual(targetBaseDir, DualityApp.DataDirectory) && !PathOp.IsPathLocatedIn(targetBaseDir, DualityApp.DataDirectory))
			{
				throw new ArgumentException(
					string.Format("The specified target base directory needs to be located within the Duality '{0}' direcctory", DualityApp.DataDirectory),
					"targetBaseDir");
			}
			if (PathOp.ArePathsEqual(inputBaseDir, EditorHelper.SourceMediaDirectory) || PathOp.IsPathLocatedIn(inputBaseDir, EditorHelper.SourceMediaDirectory))
			{
				throw new ArgumentException(
					string.Format("The specified input base directory must not to be located within the Duality '{0}' direcctory", EditorHelper.SourceMediaDirectory),
					"inputBaseDir");
			}
			if (inputFiles.Any(file => PathOp.IsPathLocatedIn(file, EditorHelper.SourceMediaDirectory)))
			{
				throw new ArgumentException(
					string.Format("Can't import Assets using source files that are located within the Duality '{0}' direcctory", EditorHelper.SourceMediaDirectory),
					"inputFiles");
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

		protected override void OnResetWorkingData()
		{
			this.assetRenameMap = null;
		}
		protected override bool OnPerform()
		{
			this.DetermineImportInputMapping();
			this.DetermineLocalInputFilePaths();

			if (this.DoesOverwriteData() && !this.InvokeConfirmOverwrite())
				return false;

			this.CopySourceToLocalFolder();
			if (!this.ImportFromLocalFolder())
				return false;

			return true;
		}

		private void DetermineImportInputMapping()
		{
			AssetImportEnvironment prepareEnv = new AssetImportEnvironment(
				this.targetDir, 
				this.sourceDir, 
				this.input);
			prepareEnv.IsPrepareStep = true;

			this.assetRenameMap = new Dictionary<string,string>();
			this.inputMapping = this.SelectImporter(prepareEnv);

			// If the preparation step auto-renamed output Resources, keep this in mind
			if (prepareEnv.AssetRenameMap.Count > 0)
			{
				foreach (var pair in prepareEnv.AssetRenameMap)
				{
					this.assetRenameMap[pair.Key] = pair.Value;
				}
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
					string oldFullName = assignment.HandledInput[i].AssetName;
					string newFullName;

					// If there was an automatic rename of output Resources, reflect that with local source / media paths
					if (this.assetRenameMap.TryGetValue(oldFullName, out newFullName))
					{
						string ext = Path.GetExtension(assignment.HandledInput[i].Path);
						string newRelativePath = newFullName + ext;
						handledInputInSourceMedia[i] = new AssetImportInput(
							Path.Combine(sourceDir, newRelativePath),
							newRelativePath,
							newFullName);
					}
					// Otherwise, perform a regular mapping from original input location to local source / media
					else
					{
						handledInputInSourceMedia[i] = new AssetImportInput(
							Path.Combine(sourceDir, assignment.HandledInput[i].RelativePath),
							assignment.HandledInput[i].RelativePath,
							assignment.HandledInput[i].AssetName);
					}
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
		private void CopySourceToLocalFolder()
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
					this.inputMapping.RemoveAt(assignmentIndex);
					assignmentIndex--;
				}
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
					AssetImportEnvironment importEnv = new AssetImportEnvironment(this.targetDir, this.sourceDir, assignment.HandledInputInSourceMedia);
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
							resource.Save();
							this.output.Add(resource);
						}
					}
					catch (Exception ex)
					{
						importFailure = true;
						Log.Editor.WriteError("An error occurred while trying to import files using '{1}': {0}", 
							Log.Exception(ex),
							Log.Type(assignment.Importer.GetType()));
						this.inputMapping.RemoveAt(assignmentIndex);
						assignmentIndex--;
					}
				}
			}

			return anyImported && !importFailure;
		}

		private bool InvokeConfirmOverwrite()
		{
			if (this.confirmOverwrite != null)
				return this.confirmOverwrite();
			else
				return false;
		}
	}
}
