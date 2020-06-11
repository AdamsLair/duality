using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality;
using Duality.IO;

namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// Environment class for an <see cref="IAssetImporter"/> during an import operation.
	/// It specifies the API that is used to interact with the overall import process and
	/// acts as an abstraction layer between importer and editor. This allows to perform
	/// simulated imports and manage detailed information about inputs and outputs.
	/// </summary>
	public class AssetImportEnvironment : IAssetImportEnvironment
	{
		private bool isPrepareStep = false;
		private bool isReImport = false;
		private string targetDir = null;
		private string sourceDir = null;
		private AssetImportInput[] input = null;
		private List<AssetImportInput> handledInput = new List<AssetImportInput>();
		private List<AssetImportOutput> output = new List<AssetImportOutput>();
		private Dictionary<string,string> assetRenameMap = new Dictionary<string,string>();
		
		/// <summary>
		/// [GET / SET] Whether the import operation is currently in the preparation phase.
		/// </summary>
		public bool IsPrepareStep
		{
			get { return this.isPrepareStep; }
			set { this.isPrepareStep = value; }
		}
		/// <summary>
		/// [GET / SET] Whether or not the import operation deals with input files that map to
		/// an already existing <see cref="Resource"/> and aims at updating it.
		/// </summary>
		public bool IsReImport
		{
			get { return this.isReImport; }
			set { this.isReImport = value; }
		}
		/// <summary>
		/// [GET] The target (Data) base directory of this import operation.
		/// </summary>
		public string TargetDirectory
		{
			get { return this.targetDir; }
		}
		/// <summary>
		/// [GET] The source (Source/Media) base directory of this import operation.
		/// </summary>
		public string SourceDirectory
		{
			get { return this.sourceDir; }
		}
		/// <summary>
		/// [GET] Enumerates all input items that are to be imported.
		/// </summary>
		public IEnumerable<AssetImportInput> Input
		{
			get { return this.input; }
		}
		/// <summary>
		/// [GET] Enumerates all input items that are flagged as being handled by the <see cref="IAssetImporter"/>.
		/// </summary>
		public IEnumerable<AssetImportInput> HandledInput
		{
			get { return this.handledInput; }
		}
		/// <summary>
		/// [GET] Enumerates the (simulated or actual) output items that have been added by the active <see cref="IAssetImporter"/>.
		/// </summary>
		public IEnumerable<AssetImportOutput> Output
		{
			get { return this.output; }
		}
		/// <summary>
		/// [GET] If a newly imported asset was automatically renamed due to naming conflicts, this property
		/// provides a mapping from asset name to (renamed) target path. Import operations can use this information
		/// from the preparation step in order to adjust input naming before entering the main import step.
		/// </summary>
		public IReadOnlyDictionary<string,string> AssetRenameMap
		{
			get { return this.assetRenameMap; }
		}
		

		public AssetImportEnvironment(string targetDir, string sourceDir, IEnumerable<AssetImportInput> input)
		{
			this.targetDir = targetDir;
			this.sourceDir = sourceDir;
			this.input = input.ToArray();
		}
		
		/// <summary>
		/// Resets all working and result data of the environment. This is used as part of the importer selection
		/// in the preparation phase in order to provide a clean slate for each <see cref="IAssetImporter"/> 
		/// that is queried.
		/// </summary>
		public void ResetAcquiredData()
		{
			this.output.Clear();
			this.handledInput.Clear();
		}
		
		/// <summary>
		/// Requests the specified input path to be handled by the current importer.
		/// </summary>
		/// <returns>True, if the current importer is allowed to handle this input item, false if not.</returns>
		public bool HandleInput(string filePath)
		{
			int inputIndex = this.input.IndexOfFirst(i => PathOp.ArePathsEqual(i.Path, filePath));

			// Not part of the input file collection - reject this
			if (inputIndex == -1)
				return false;

			// Already handled - reject this
			if (this.handledInput.Any(i => PathOp.ArePathsEqual(i.Path, filePath)))
				return false;

			this.handledInput.Add(this.input[inputIndex]);
			return true;
		}
		/// <summary>
		/// Requests an output <see cref="Duality.Resource"/> with the specified name (see <see cref="AssetImportInput.AssetName"/>).
		/// Use this method to create a new Resource during import, or request the affected one during re-import.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assetName">The name of the requested output <see cref="Duality.Resource"/> (see <see cref="AssetImportInput.AssetName"/>).</param>
		public ContentRef<T> GetOutput<T>(string assetName) where T : Resource, new()
		{
			string targetResPath = this.GetTargetPath<T>(assetName);

			// When requested during preparation, just return an empty ContentRef
			if (this.isPrepareStep || this.isReImport)
			{
				return new ContentRef<T>(null, targetResPath);
			}
			// When requested during import, actually create the requested Resource and set it up
			else
			{
				T targetRes = new T();
				targetRes.Path = targetResPath;
				return new ContentRef<T>(targetRes, targetResPath);
			}
		}
		/// <summary>
		/// Specifies that the current importer will create or modify a <see cref="Duality.Resource"/> with 
		/// the specified name (see <see cref="AssetImportInput.AssetName"/>).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assetName">The name of the generated output <see cref="Duality.Resource"/> (see <see cref="AssetImportInput.AssetName"/>).</param>
		/// <param name="inputPaths">An enumeration of input paths that are used to generate this output <see cref="Duality.Resource"/>.</param>
		public void AddOutput<T>(string assetName, IEnumerable<string> inputPaths) where T : Resource
		{
			string targetResPath = this.GetTargetPath<T>(assetName);
			this.AddOutput(new ContentRef<T>(null, targetResPath), inputPaths);
		}
		/// <summary>
		/// Submits the specified <see cref="Duality.Resource"/> as a generated output of the current importer.
		/// </summary>
		/// <param name="resource">A reference to the generated output <see cref="Duality.Resource"/>.</param>
		/// <param name="inputPaths">An enumeration of input paths that are used to generate this output <see cref="Duality.Resource"/>.</param>
		public void AddOutput(IContentRef resource, IEnumerable<string> inputPaths)
		{
			// Make sure that the provided input paths aren't null or whitespace.
			if (inputPaths == null) 
				throw new ArgumentNullException("inputPaths");
			if (inputPaths.Any(p => string.IsNullOrWhiteSpace(p))) 
				throw new ArgumentException("Input paths enumerable may not contain null or whitespace items.", "inputPaths");

			// If this is not a preparation step, require all output Resources to be actually available.
			if (!this.isPrepareStep && !resource.IsAvailable)
				throw new ArgumentException("Can't add a non-existent output Resource", "resource");

			this.output.Add(new AssetImportOutput(resource.As<Resource>(), inputPaths));
		}
		
		/// <summary>
		/// Retrieves the value of an import parameter for the specified <see cref="Resource"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resource">A reference to the <see cref="Duality.Resource"/> that is parameterized.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">An out reference to the variable to which the retrieved value will be assigned.</param>
		/// <returns>True, if the value was retrieved successfully. False otherwise.</returns>
		public bool GetParameter<T>(IContentRef resource, string parameterName, out T value)
		{
			// If we're importing this asset for the first time, there is no data available.
			if (!this.isReImport)
			{
				value = default(T);
				return false;
			}

			return AssetInternalHelper.GetAssetInfoCustomValue<T>(resource.Res, parameterName, out value);
		}
		/// <summary>
		/// Sets the value of an import parameter for the specified <see cref="Resource"/> 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resource">A reference to the <see cref="Duality.Resource"/> that is parameterized.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The new value that should be assigned to the parameter.</param>
		public void SetParameter<T>(IContentRef resource, string parameterName, T value)
		{
			// Disallow adjusting parameters in the preparation step.
			if (this.isPrepareStep) throw new InvalidOperationException(
				"Cannot adjust parameter values in the preparation step. " +
				"At this point, any Resource data is considered read-only.");

			AssetInternalHelper.SetAssetInfoCustomValue<T>(resource.Res, parameterName, value);
		}

		private string GetTargetPath<T>(string assetName) where T : Resource
		{
			// If this is a Re-Import operation, try to reproduce actual Resource paths
			if (this.isReImport)
			{
				return Path.Combine(this.targetDir, assetName) + Resource.GetFileExtByType<T>();
			}
			// Otherwise, attempt to get a free path for a fresh import
			else
			{
				string ext = Resource.GetFileExtByType<T>();
				string targetPath = PathHelper.GetFreePath(Path.Combine(this.targetDir, assetName), ext);

				// Reverse engineer a new full name based on the determined target path
				string targetFullName;
				{
					string targetFullNameDir = Path.GetDirectoryName(assetName);
					string targetFullNameFile = Path.GetFileName(targetPath);
					targetFullNameFile = targetFullNameFile.Remove(targetFullNameFile.Length - ext.Length, ext.Length);
					targetFullName = Path.Combine(targetFullNameDir, targetFullNameFile);
				}

				// If the new full name is different from the old one, keep the rename operation in mind,
				// so we can also alter the local source / media destination accordingly, should there be one.
				if (!string.Equals(assetName, targetFullName, StringComparison.OrdinalIgnoreCase))
				{
					this.assetRenameMap[assetName] = targetFullName;
				}

				return targetPath;
			}
		}
	}
}
