using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor.AssetManagement
{
	public abstract class AssetImportOperation
	{
		private class ConflictData : IAssetImporterConflictData
		{
			private int defaultIndex;
			private ImportInputAssignment[] conflicts;

			public IAssetImporter DefaultImporter
			{
				get { return this.conflicts[defaultIndex].Importer; }
			}
			public IEnumerable<IAssetImporter> Importers
			{
				get { return this.conflicts.Select(assign => assign.Importer); }
			}
			public IEnumerable<string> InputFiles
			{
				get { return this.conflicts.SelectMany(assign => assign.HandledInput.Select(i => i.Path)).Distinct(); }
			}

			public ConflictData(ImportInputAssignment[] conflicts, int defaultIndex)
			{
				this.conflicts = conflicts;
				this.defaultIndex = defaultIndex;
			}
		}

		protected AssetImportInput[] input = null;
		protected RawList<ImportInputAssignment> inputMapping = null;
		protected List<AssetImportOutput> output = null;
		private AssetImporterConflictHandler conflictHandler = null;


		public IEnumerable<AssetImportInput> Input
		{
			get { return this.input; }
		}
		public IEnumerable<AssetImportOutput> Output
		{
			get { return this.output ?? Enumerable.Empty<AssetImportOutput>(); }
		}
		public AssetImporterConflictHandler ImporterConflictHandler
		{
			get { return this.conflictHandler; }
			set { this.conflictHandler = value; }
		}


		public bool Perform()
		{
			this.ResetWorkingData();
			bool importSuccess = this.OnPerform();

			// Clean up, in case we've done heavy-duty work
			GC.Collect();
			GC.WaitForPendingFinalizers();

			return importSuccess;
		}
		public bool SimulatePerform()
		{
			this.ResetWorkingData();
			if (!this.OnSimulatePerform())
				return false;

			this.output = this.inputMapping.SelectMany(item => item.ExpectedOutput).ToList();
			return true;
		}

		protected abstract void OnResetWorkingData();
		protected abstract bool OnPerform();
		protected abstract bool OnSimulatePerform();
		
		protected RawList<ImportInputAssignment> SelectImporter(AssetImportEnvironment env)
		{
			if (!env.IsPrepareStep) throw new ArgumentException(
				"The specified import environment must be configured as a preparation environment.", 
				"env");

			// Find an importer to handle some or all of the unhandled input files
			RawList<ImportInputAssignment> candidateMapping = new RawList<ImportInputAssignment>();
			foreach (IAssetImporter importer in AssetManager.Importers)
			{
				env.ResetAcquiredData();

				try
				{
					importer.PrepareImport(env);
				}
				catch (Exception ex)
				{
					Log.Editor.WriteError("An error occurred in the preparation step of '{1}': {0}", 
						Log.Exception(ex),
						Log.Type(importer.GetType()));
					continue;
				}

				if (env.HandledInput.Any())
				{
					candidateMapping.Add(new ImportInputAssignment
					{
						Importer = importer,
						HandledInput = env.HandledInput.ToArray(),
						ExpectedOutput = env.Output.ToArray()
					});
				}
			}

			// Determine if multiple importers intend to handle the same files and resolve conflicts
			List<int> conflictingIndices = new List<int>();
			List<string> conflictingFiles = new List<string>();
			for (int mainIndex = 0; mainIndex < candidateMapping.Count; mainIndex++)
			{
				ImportInputAssignment assignment = candidateMapping[mainIndex];

				// Find all conflicts related to this assignment
				conflictingIndices.Clear();
				conflictingFiles.Clear();
				for (int secondIndex = 0; secondIndex < candidateMapping.Count; secondIndex++)
				{
					if (secondIndex == mainIndex) continue;

					ImportInputAssignment conflictAssignment = candidateMapping[secondIndex];
					IEnumerable<string> mainFiles = assignment.HandledInput.Select(item => item.Path);
					IEnumerable<string> secondFiles = conflictAssignment.HandledInput.Select(item => item.Path);
					string[] conflicts = mainFiles.Union(secondFiles).ToArray();
					if (conflicts.Length > 0)
					{
						if (conflictingIndices.Count == 0) conflictingIndices.Add(mainIndex);
						conflictingIndices.Add(secondIndex);
						conflictingFiles.AddRange(conflicts);
					}
				}

				// Resolve conflicts with this assignment
				if (conflictingIndices.Count > 0)
				{
					// Determine which importer to prefer for this conflict
					ImportInputAssignment[] conflictingAssignments = conflictingIndices.Select(i => candidateMapping[i]).ToArray();
					int keepIndex = this.ResolveMappingConflict(conflictingAssignments);

					// If we somehow decided that none of the options is viable, abort the operation
					if (keepIndex == -1)
					{
						candidateMapping.Clear();
						return candidateMapping;
					}

					// Sort indices to remove in declining order and remove their mappings
					conflictingIndices.Remove(keepIndex);
					conflictingIndices.Sort((a, b) => b - a);
					foreach (int index in conflictingIndices)
					{
						candidateMapping.RemoveAt(index);
					}

					// Start over with the conflict search
					mainIndex = -1;
					continue;
				}
			}

			return candidateMapping;
		}
		protected bool RunImporter(AssetImportEnvironment env, ImportInputAssignment assignment, IList<AssetImportOutput> outputCollection)
		{
			try
			{
				assignment.Importer.Import(env);
						
				// Get a list on properly registered output Resources and report warnings on the rest
				List<AssetImportOutput> expectedOutput = new List<AssetImportOutput>();
				foreach (AssetImportOutput output in env.Output)
				{
					if (!assignment.ExpectedOutput.Any(item => item.Resource == output.Resource))
					{
						Log.Editor.WriteWarning(
							"AssetImporter '{0}' created an unpredicted output Resource: '{1}'. " + Environment.NewLine +
							"This may cause problems in the Asset Management system, especially during Asset re-import. " + Environment.NewLine +
							"Please fix the implementation of the PrepareImport method so it properly calls AddOutput for each predicted output Resource.",
							Log.Type(assignment.Importer.GetType()),
							output.Resource);
					}
					else
					{
						expectedOutput.Add(output);
					}
				}

				// Collect references to the imported Resources and save them
				foreach (AssetImportOutput output in expectedOutput)
				{
					Resource resource = output.Resource.Res;

					AssetInfo assetInfo = resource.AssetInfo ?? new AssetInfo();
					assetInfo.ImporterId = assignment.Importer.Id;
					assetInfo.NameHint = resource.Name;

					resource.AssetInfo = assetInfo;
					outputCollection.Add(output);
				}
			}
			catch (Exception ex)
			{
				Log.Editor.WriteError("An error occurred while trying to import files using '{1}': {0}", 
					Log.Exception(ex),
					Log.Type(assignment.Importer.GetType()));
				return false;
			}

			return true;
		}

		private int ResolveMappingConflict(ImportInputAssignment[] conflictingAssignments)
		{
			// If we have an already-existing expected output, see if it knows which importer to use
			string preferredImporterId = null;
			{
				// Aggregate all existing output Resources
				HashSet<Resource> existingResources = new HashSet<Resource>(); 
				for (int i = 0; i < conflictingAssignments.Length; i++)
				{
					bool ambiguous = false;
					AssetImportOutput[] output = conflictingAssignments[i].ExpectedOutput;
					for (int j = 0; j < output.Length; j++)
					{
						Resource existingRes = output[j].Resource.Res;
						if (existingRes == null)
						{
							ambiguous = true;
							break;
						}
						existingResources.Add(existingRes);
					}
					if (ambiguous) break;
				}

				// See if the existing Resources have a common preferred importer
				foreach (Resource existingRes in existingResources)
				{
					string resImporterPref = existingRes.AssetInfo != null ? existingRes.AssetInfo.ImporterId : null;

					// If at least one Resource doesn't have a preference, that's ambiuous. Cancel it.
					if (resImporterPref == null)
					{
						preferredImporterId = null;
						break;
					}

					// Set up the shared importer preference
					if (preferredImporterId == null)
					{
						preferredImporterId = resImporterPref;
					}
					// If different outputs from this mapping report different preferred importers, that's ambiguous. Cancel it.
					else if (preferredImporterId != resImporterPref)
					{
						preferredImporterId = null;
						break;
					}
				}
			}

			// If we have a preferred ID, see if it's an option
			if (preferredImporterId != null)
			{
				for (int i = 0; i < conflictingAssignments.Length; i++)
				{
					// If we have a match with the preferred importer, this is definitely the correct assignment to handle this.
					if (conflictingAssignments[i].Importer.Id == preferredImporterId)
						return i;
				}
			}

			// By default, fall back on simply prefering the highest-priority importer
			int keepIndex = -1;
			int highestPrio = int.MinValue;
			for (int i = 0; i < conflictingAssignments.Length; i++)
			{
				if (conflictingAssignments[i].Importer.Priority > highestPrio)
				{
					highestPrio = conflictingAssignments[i].Importer.Priority;
					keepIndex = i;
				}
			}

			// If there is a conflict handler (such as "spawn a user dialog"), see if that can deal with it.
			if (this.conflictHandler != null)
			{
				ConflictData data = new ConflictData(conflictingAssignments, keepIndex);
				IAssetImporter selectedImporter = this.conflictHandler(data);
				int selectedIndex = conflictingAssignments.IndexOfFirst(assignment => assignment.Importer == selectedImporter);
				return selectedIndex;
			}

			return keepIndex;
		}
		private void ResetWorkingData()
		{
			this.inputMapping = null;
			this.output = null;
			this.OnResetWorkingData();
		}

		protected struct ImportInputAssignment
		{
			public IAssetImporter Importer;
			public AssetImportInput[] HandledInput;
			public AssetImportInput[] HandledInputInSourceMedia;
			public AssetImportOutput[] ExpectedOutput;

			public override string ToString()
			{
				return string.Format("{0}: {1} items",
					this.Importer != null ? this.Importer.Id : "null",
					this.HandledInput != null ? this.HandledInput.Length : 0);
			}
		}
	}
}
