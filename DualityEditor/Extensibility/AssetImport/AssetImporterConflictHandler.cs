using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	public delegate IAssetImporter AssetImporterConflictHandler(IAssetImporterConflictData conflictData);
	public interface IAssetImporterConflictData
	{
		IAssetImporter DefaultImporter { get; }
		IEnumerable<IAssetImporter> Importers { get; }
		IEnumerable<string> InputFiles { get; }
	}
}
