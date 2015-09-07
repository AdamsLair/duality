using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor
{
	public interface IAssetImporter
	{
		void PrepareImport(IAssetImportEnvironment env);
		void Import(IAssetImportEnvironment env);
	}
}
