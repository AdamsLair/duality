using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Editor.Properties;

namespace Duality.Editor.AssetManagement
{
	[EditorHintImage(GeneralResNames.ImageAssetImporter)]
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

		/// <summary>
		/// In the preparation step of an export operation, an importer determines whether it
		/// is able to handle the specified Resource and registers the expected output source files.
		/// </summary>
		/// <param name="env"></param>
		void PrepareExport(IAssetExportEnvironment env);
		/// <summary>
		/// Performs the previously prepared export operation. Creates and / or modifies source files
		/// according to the specified input Resource.
		/// </summary>
		/// <param name="env"></param>
		void Export(IAssetExportEnvironment env);
	}
}
