using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor
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
		protected HashSet<ContentRef<Resource>> output = null;
		private AssetImporterConflictHandler conflictHandler = null;


		public IEnumerable<AssetImportInput> Input
		{
			get { return this.input; }
		}
		public IEnumerable<ContentRef<Resource>> Output
		{
			get { return this.output ?? Enumerable.Empty<ContentRef<Resource>>(); }
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

		protected abstract void OnResetWorkingData();
		protected abstract bool OnPerform();
		
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
						ExpectedOutput = env.OutputResources.ToArray()
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
		private int ResolveMappingConflict(ImportInputAssignment[] conflictingAssignments)
		{
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
			public ContentRef<Resource>[] ExpectedOutput;

			public override string ToString()
			{
				return string.Format("{0}: {1} items",
					this.Importer != null ? this.Importer.Id : "null",
					this.HandledInput != null ? this.HandledInput.Length : 0);
			}
		}
	}
}
