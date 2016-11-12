using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// Provides an API for an <see cref="IAssetImporter"/> to use during import operations.
	/// </summary>
	public interface IAssetImportEnvironment
	{
		/// <summary>
		/// [GET] The target (Data) base directory of this import operation.
		/// </summary>
		string TargetDirectory { get; }
		/// <summary>
		/// [GET] The source (Source/Media) base directory of this import operation.
		/// </summary>
		string SourceDirectory { get; }
		/// <summary>
		/// [GET] Enumerates all input items that are to be imported.
		/// </summary>
		IEnumerable<AssetImportInput> Input { get; }

		/// <summary>
		/// Requests the specified input path to be handled by the current importer.
		/// </summary>
		/// <param name="inputPath"></param>
		/// <returns>True, if the current importer is allowed to handle this input item, false if not.</returns>
		bool HandleInput(string inputPath);

		/// <summary>
		/// Requests an output <see cref="Duality.Resource"/> with the specified name (see <see cref="AssetImportInput.AssetName"/>).
		/// Use this method to create a new Resource during import, or request the affected one during re-import.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assetName">The name of the requested output <see cref="Duality.Resource"/> (see <see cref="AssetImportInput.AssetName"/>).</param>
		/// <returns></returns>
		ContentRef<T> GetOutput<T>(string assetName) where T : Resource, new();
		/// <summary>
		/// Specifies that the current importer will create or modify a <see cref="Duality.Resource"/> with 
		/// the specified name (see <see cref="AssetImportInput.AssetName"/>).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assetName">The name of the generated output <see cref="Duality.Resource"/> (see <see cref="AssetImportInput.AssetName"/>).</param>
		/// <param name="inputPaths">An enumeration of input paths that are used to generate this output <see cref="Duality.Resource"/>.</param>
		void AddOutput<T>(string assetName, IEnumerable<string> inputPaths) where T : Resource;
		/// <summary>
		/// Submits the specified <see cref="Duality.Resource"/> as a generated output of the current importer.
		/// </summary>
		/// <param name="resource">A reference to the generated output <see cref="Duality.Resource"/>.</param>
		/// <param name="inputPaths">An enumeration of input paths that are used to generate this output <see cref="Duality.Resource"/>.</param>
		void AddOutput(IContentRef resource, IEnumerable<string> inputPaths);
		
		/// <summary>
		/// Retrieves the value of an import parameter for the specified <see cref="Resource"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resource">A reference to the <see cref="Duality.Resource"/> that is parameterized.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">An out reference to the variable to which the retrieved value will be assigned.</param>
		/// <returns>True, if the value was retrieved successfully. False otherwise.</returns>
		bool GetParameter<T>(IContentRef resource, string parameterName, out T value);
		/// <summary>
		/// Sets the value of an import parameter for the specified <see cref="Resource"/> 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resource">A reference to the <see cref="Duality.Resource"/> that is parameterized.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The new value that should be assigned to the parameter.</param>
		void SetParameter<T>(IContentRef resource, string parameterName, T value);
	}
}
