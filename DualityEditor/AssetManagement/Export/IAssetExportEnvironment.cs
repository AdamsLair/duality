using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// Provides an API for an <see cref="IAssetImporter"/> to use during export operations.
	/// </summary>
	public interface IAssetExportEnvironment
	{
		/// <summary>
		/// [GET] The directory to export the source files into.
		/// </summary>
		string ExportDirectory { get; }
		/// <summary>
		/// [GET] The input <see cref="Duality.Resource"/> that should be exported to the <see cref="ExportDirectory"/>.
		/// </summary>
		Resource Input { get; }

		/// <summary>
		/// Registers the specified local file path as a result of this export operation, transforms it
		/// according to the current source directory and returns a relative file path that can then
		/// be used for writing the registered output file.
		/// </summary>
		/// <param name="localFilePath"></param>
		string AddOutputPath(string localFilePath);

		/// <summary>
		/// Retrieves the value of an export parameter for the exported <see cref="Resource"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">An out reference to the variable to which the retrieved value will be assigned.</param>
		/// <returns>True, if the value was retrieved successfully. False otherwise.</returns>
		bool GetParameter<T>(string parameterName, out T value);
		/// <summary>
		/// Sets the value of an export parameter for the exported <see cref="Resource"/> 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The new value that should be assigned to the parameter.</param>
		void SetParameter<T>(string parameterName, T value);
	}
}
