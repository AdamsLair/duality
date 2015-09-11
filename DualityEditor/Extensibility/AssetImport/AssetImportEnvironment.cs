using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
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

		public bool IsPrepareStep
		{
			get { return this.isPrepareStep; }
			set { this.isPrepareStep = value; }
		}
		public bool IsReImport
		{
			get { return this.isReImport; }
			set { this.isReImport = value; }
		}
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
		public IEnumerable<AssetImportInput> HandledInput
		{
			get { return this.handledInput; }
		}
		public IEnumerable<AssetImportOutput> Output
		{
			get { return this.output; }
		}
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

		public void ResetAcquiredData()
		{
			this.output.Clear();
			this.handledInput.Clear();
		}

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
		public void AddOutput<T>(string assetName, IEnumerable<string> inputPaths) where T : Resource
		{
			string targetResPath = this.GetTargetPath<T>(assetName);
			this.AddOutput(new ContentRef<T>(null, targetResPath), inputPaths);
		}
		public void AddOutput(IContentRef resource, IEnumerable<string> inputPaths)
		{
			// If this is not a preparation step, require all output Resources to be actually available.
			if (!this.isPrepareStep && !resource.IsAvailable)
				throw new ArgumentException("Can't add a non-existent output Resource", "resource");

			this.output.Add(new AssetImportOutput(resource.As<Resource>(), inputPaths));
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
