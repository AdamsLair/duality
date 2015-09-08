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
		private string targetDir = null;
		private string sourceDir = null;
		private AssetImportInput[] input = null;
		private List<AssetImportInput> handledInput = new List<AssetImportInput>();
		private HashSet<ContentRef<Resource>> output = new HashSet<ContentRef<Resource>>();
		private Dictionary<string,string> assetRenameMap = new Dictionary<string,string>();

		public bool IsPrepareStep
		{
			get { return this.isPrepareStep; }
			set { this.isPrepareStep = value; }
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
		public IEnumerable<ContentRef<Resource>> OutputResources
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
		public ContentRef<T> GetOutput<T>(string fullName) where T : Resource, new()
		{
			string targetResPath = this.GetTargetPath<T>(fullName);

			// When requested during preparation, just return an empty ContentRef
			if (this.isPrepareStep)
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
		public void AddOutput<T>(string fullName) where T : Resource
		{
			string targetResPath = this.GetTargetPath<T>(fullName);
			this.AddOutput(new ContentRef<T>(null, targetResPath));
		}
		public void AddOutput(IContentRef resource)
		{
			this.output.Add(resource.As<Resource>());
		}

		private string GetTargetPath<T>(string fullName) where T : Resource
		{
			string ext = Resource.GetFileExtByType<T>();
			string targetPath = PathHelper.GetFreePath(Path.Combine(this.targetDir, fullName), ext);

			// Reverse engineer a new full name based on the determined target path
			string targetFullName;
			{
				string targetFullNameDir = Path.GetDirectoryName(fullName);
				string targetFullNameFile = Path.GetFileName(targetPath);
				targetFullNameFile = targetFullNameFile.Remove(targetFullNameFile.Length - ext.Length, ext.Length);
				targetFullName = Path.Combine(targetFullNameDir, targetFullNameFile);
			}

			// If the new full name is different from the old one, keep the rename operation in mind,
			// so we can also alter the local source / media destination accordingly, should there be one.
			if (!string.Equals(fullName, targetFullName, StringComparison.OrdinalIgnoreCase))
			{
				this.assetRenameMap[fullName] = targetFullName;
			}

			return targetPath;
		}
	}
}
