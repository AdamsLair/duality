using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor
{
	public interface IAssetImporter
	{
		/// <summary>
		/// [GET] A fixed system ID that represents this importer.
		/// </summary>
		string Id { get; }
		/// <summary>
		/// [GET] The user-friendly name of this importer.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// [GET] The relative priority of this importer over others.
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// In the preparation step of an import operation, an importer determines which (if any) of the
		/// available files it is able to handle and registers the expected output Resources.
		/// </summary>
		/// <param name="env"></param>
		void PrepareImport(IAssetImportEnvironment env);
		/// <summary>
		/// Performs the previously prepared import operation. Creates and / or modifies Resources
		/// according to the available input files.
		/// </summary>
		/// <param name="env"></param>
		void Import(IAssetImportEnvironment env);
	}
}
