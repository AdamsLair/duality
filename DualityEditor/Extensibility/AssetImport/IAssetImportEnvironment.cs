using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor
{
	public interface IAssetImportEnvironment
	{
		string TargetDirectory { get; }
		string SourceDirectory { get; }
		IEnumerable<AssetImportInput> Input { get; }

		bool HandleInput(string inputPath);

		ContentRef<T> GetOutput<T>(string fullName) where T : Resource, new();
		void AddOutput<T>(string fullName) where T : Resource;
		void AddOutput(IContentRef resource);
	}
}
