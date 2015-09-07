using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	public class DefaultAssetImportEnvironment : IAssetImportEnvironment
	{
		private string targetDir = null;
		private string sourceDir = null;
		private AssetImportInput[] input = null;
		private List<AssetImportInput> handledInput = new List<AssetImportInput>();
		private HashSet<ContentRef<Resource>> output = new HashSet<ContentRef<Resource>>();

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
		
		public DefaultAssetImportEnvironment(string targetDir, string sourceDir, IEnumerable<AssetImportInput> input)
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
		public void AddOutput(IContentRef resource)
		{
			this.output.Add(resource.As<Resource>());
		}
		public void AddOutput<T>(string resourcePath) where T : Resource
		{
			this.AddOutput(new ContentRef<T>(null, resourcePath));
		}
	}
}
