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
		void AddOutput(IContentRef resource);
		void AddOutput<T>(string resourcePath) where T : Resource;
	}
}
