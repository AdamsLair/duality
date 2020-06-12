using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// Encapsulates a single Resource export operation.
	/// </summary>
	public class AssetExportOperation
	{
		private class ConflictData : IAssetImporterConflictData
		{
			private Resource input;
			private int defaultIndex;
			private ExportInputAssignment[] conflicts;

			public IAssetImporter DefaultImporter
			{
				get { return this.conflicts[defaultIndex].Importer; }
			}
			public IEnumerable<IAssetImporter> Importers
			{
				get { return this.conflicts.Select(item => item.Importer); }
			}
			public IEnumerable<string> InputFiles
			{
				get { return new string[] { this.input.Path }; }
			}

			public ConflictData(Resource input, ExportInputAssignment[] conflicts, int defaultIndex)
			{
				this.input = input;
				this.conflicts = conflicts;
				this.defaultIndex = defaultIndex;
			}
		}

		private string exportDir = null;
		private Resource input = null;
		private ExportInputAssignment inputMapping = default(ExportInputAssignment);
		private List<string> outputPaths = null;
		private AssetImporterConflictHandler conflictHandler = null;
		private bool isAssetInfoChanged = false;


		/// <summary>
		/// [GET] The <see cref="Resource"/> that is exported in this operation.
		/// </summary>
		public Resource Input
		{
			get { return this.input; }
		}
		/// <summary>
		/// [GET] Enumerates all generated output paths. 
		/// This result is only available after calling <see cref="Perform"/> or <see cref="SimulatePerform"/>.
		/// </summary>
		public IEnumerable<string> OutputPaths
		{
			get { return this.outputPaths ?? Enumerable.Empty<string>(); }
		}
		/// <summary>
		/// [GET / SET] An optional delegate for resolving conflicts when multiple
		/// <see cref="IAssetImporter"/> instances are able to export a given <see cref="Resource"/>.
		/// </summary>
		public AssetImporterConflictHandler ImporterConflictHandler
		{
			get { return this.conflictHandler; }
			set { this.conflictHandler = value; }
		}
		/// <summary>
		/// [GET] Whether the export operation modified export parameters of the exported <see cref="Resource"/>.
		/// </summary>
		public bool IsAssetInfoChanged
		{
			get { return this.isAssetInfoChanged; }
		}

		
		public AssetExportOperation(Resource input, string exportDir)
		{
			this.exportDir = exportDir;
			this.input = input;
		}

		/// <summary>
		/// Performs the operation and returns whether it was successful.
		/// </summary>
		public bool Perform()
		{
			this.ResetWorkingData();

			bool importSuccess =
				this.DetermineExportInputMapping() &&
				this.ExportToLocalFolder();

			// Clean up, in case we've done heavy-duty work
			GC.Collect();
			GC.WaitForPendingFinalizers();

			return importSuccess;
		}
		/// <summary>
		/// Simulates performing the operation without actually doing anything, and returns
		/// whether the simulation was successful.
		/// This can be used to determine which files would be affected when performing
		/// the operation.
		/// </summary>
		public bool SimulatePerform()
		{
			this.ResetWorkingData();
			if (!this.DetermineExportInputMapping())
				return false;

			this.outputPaths = this.inputMapping.ExpectedOutput.ToList();
			return true;
		}
		
		private bool DetermineExportInputMapping()
		{
			AssetExportEnvironment prepareEnv = new AssetExportEnvironment(
				this.exportDir, 
				this.input);
			prepareEnv.IsPrepareStep = true;
			this.inputMapping = this.SelectImporter(prepareEnv);
			return this.inputMapping.Importer != null;
		}
		private bool ExportToLocalFolder()
		{
			this.outputPaths = new List<string>();
			bool success = false;
			{
				AssetExportEnvironment importEnv = new AssetExportEnvironment(this.exportDir, this.input);
				success = this.RunImporter(importEnv, this.inputMapping, this.outputPaths);
				this.isAssetInfoChanged = importEnv.IsAssetInfoChanged;
			}
			return success;
		}

		private ExportInputAssignment SelectImporter(AssetExportEnvironment env)
		{
			if (!env.IsPrepareStep) throw new ArgumentException(
				"The specified export environment must be configured as a preparation environment.", 
				"env");

			// Find an importer to handle some or all of the unhandled input files
			List<ExportInputAssignment> candidates = new List<ExportInputAssignment>();
			foreach (IAssetImporter importer in AssetManager.Importers)
			{
				env.ResetAcquiredData();

				try
				{
					importer.PrepareExport(env);
				}
				catch (Exception ex)
				{
					Logs.Editor.WriteError("An error occurred in the preparation step of '{1}': {0}", 
						LogFormat.Exception(ex),
						LogFormat.Type(importer.GetType()));
					continue;
				}

				if (env.IsHandled)
				{
					candidates.Add(new ExportInputAssignment
					{
						Importer = importer,
						ExpectedOutput = env.OutputPaths.ToArray()
					});
				}
			}

			// If multiple importers intend to handle the same files, resolve conflicts
			if (candidates.Count > 1)
			{
				ExportInputAssignment[] conflictingAssignments = candidates.ToArray();
				int selectedIndex = this.ResolveMappingConflict(env.Input, conflictingAssignments);
				if (selectedIndex == -1)
					return default(ExportInputAssignment);
				else
					return conflictingAssignments[selectedIndex];
			}

			return candidates.FirstOrDefault();
		}
		private bool RunImporter(AssetExportEnvironment env, ExportInputAssignment assignment, IList<string> outputPathCollection)
		{
			try
			{
				assignment.Importer.Export(env);
						
				// Get a list on properly registered output Resources and report warnings on the rest
				foreach (string outputPath in env.OutputPaths)
				{
					FileEventManager.FlagPathEditorModified(outputPath);
					if (!assignment.ExpectedOutput.Contains(outputPath))
					{
						Logs.Editor.WriteWarning(
							"AssetImporter '{0}' created an unpredicted output file: '{1}'. " + Environment.NewLine +
							"This may cause problems in the Asset Management system, especially during Asset re-import. " + Environment.NewLine +
							"Please fix the implementation of the PrepareExport method so it properly calls AddOutputPath for each predicted output file.",
							LogFormat.Type(assignment.Importer.GetType()),
							outputPath);
					}
					else
					{
						outputPathCollection.Add(outputPath);
					}
				}
			}
			catch (Exception ex)
			{
				Logs.Editor.WriteError("An error occurred while trying to export Resource '{2}' using '{1}': {0}", 
					LogFormat.Exception(ex),
					LogFormat.Type(assignment.Importer.GetType()),
					env.Input);
				return false;
			}

			return true;
		}
		private int ResolveMappingConflict(Resource input, ExportInputAssignment[] conflictingAssignments)
		{
			// Ask the input Resource which importer it would prefer
			if (input != null && input.AssetInfo != null && input.AssetInfo.ImporterId != null)
			{
				for (int i = 0; i < conflictingAssignments.Length; i++)
				{
					// If we have a match with the preferred importer, this is definitely the correct assignment to handle this.
					if (conflictingAssignments[i].Importer.Id == input.AssetInfo.ImporterId)
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
				ConflictData data = new ConflictData(input, conflictingAssignments, keepIndex);
				IAssetImporter selectedImporter = this.conflictHandler(data);
				int selectedIndex = conflictingAssignments.IndexOfFirst(item => item.Importer == selectedImporter);
				return selectedIndex;
			}

			return keepIndex;
		}
		private void ResetWorkingData()
		{
			this.isAssetInfoChanged = false;
			this.inputMapping = default(ExportInputAssignment);
			this.outputPaths = null;
		}

		private struct ExportInputAssignment
		{
			public IAssetImporter Importer;
			public string[] ExpectedOutput;

			public override string ToString()
			{
				return string.Format("{0}: {1} files",
					this.Importer != null ? this.Importer.Id : "null",
					this.ExpectedOutput != null ? this.ExpectedOutput.Length : 0);
			}
		}
	}
}
