using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// Environment class for an <see cref="IAssetImporter"/> during an export operation.
	/// It specifies the API that is used to interact with the overall export process and
	/// acts as an abstraction layer between importer and editor. This allows to perform
	/// simulated exports and manage detailed information about inputs and outputs.
	/// </summary>
	public class AssetExportEnvironment : IAssetExportEnvironment
	{
		private bool isPrepareStep = false;
		private string exportDir = null;
		private Resource input = null;
		private bool isHandled = false;
		private bool isAssetInfoChanged = false;
		private List<string> outputPaths = new List<string>();

		/// <summary>
		/// [GET / SET] Whether the export operation is currently in the preparation phase.
		/// </summary>
		public bool IsPrepareStep
		{
			get { return this.isPrepareStep; }
			set { this.isPrepareStep = value; }
		}
		/// <summary>
		/// [GET] The directory to export the source files into.
		/// </summary>
		public string ExportDirectory
		{
			get { return this.exportDir; }
		}
		/// <summary>
		/// [GET] The input <see cref="Duality.Resource"/> that should be exported to the <see cref="ExportDirectory"/>.
		/// </summary>
		public Resource Input
		{
			get { return this.input; }
		}
		/// <summary>
		/// [GET] Whether the operations input was handled by one of the available <see cref="IAssetImporter"/> instances.
		/// If this returns false, the input <see cref="Resource"/> might not have a suitable exporter.
		/// </summary>
		public bool IsHandled
		{
			get { return this.isHandled; }
		}
		/// <summary>
		/// [GET] Whether the export operation modified export parameters of the exported <see cref="Resource"/>.
		/// </summary>
		public bool IsAssetInfoChanged
		{
			get { return this.isAssetInfoChanged; }
		}
		/// <summary>
		/// [GET] Enumerates the (simulated or actual) output paths that have been added by the active <see cref="IAssetImporter"/>.
		/// </summary>
		public IEnumerable<string> OutputPaths
		{
			get { return this.outputPaths; }
		}
		

		public AssetExportEnvironment(string exportDir, Resource input)
		{
			this.exportDir = exportDir;
			this.input = input;
		}


		/// <summary>
		/// Resets all working and result data of the environment. This is used as part of the exporter selection
		/// in the preparation phase in order to provide a clean slate for each <see cref="IAssetImporter"/> 
		/// that is queried.
		/// </summary>
		public void ResetAcquiredData()
		{
			this.outputPaths.Clear();
			this.isHandled = false;
			this.isAssetInfoChanged = false;
		}
		/// <summary>
		/// Registers the specified local file path as a result of this export operation, transforms it
		/// according to the current source directory and returns a relative file path that can then
		/// be used for writing the registered output file.
		/// </summary>
		/// <param name="localFilePath"></param>
		public string AddOutputPath(string localFilePath)
		{
			if (string.IsNullOrWhiteSpace(localFilePath)) 
				throw new ArgumentException("File path can't be null or whitespace.", "localFilePath");

			string filePath = Path.Combine(this.exportDir, localFilePath);

			// If we're doing actual work, make sure the directory exists
			if (!this.isPrepareStep)
			{
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			}

			this.outputPaths.Add(filePath);
			this.isHandled = true;
			return filePath;
		}
		
		/// <summary>
		/// Retrieves the value of an export parameter for the exported <see cref="Resource"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">An out reference to the variable to which the retrieved value will be assigned.</param>
		/// <returns>True, if the value was retrieved successfully. False otherwise.</returns>
		public bool GetParameter<T>(string parameterName, out T value)
		{
			return AssetInternalHelper.GetAssetInfoCustomValue<T>(this.input, parameterName, out value);
		}
		/// <summary>
		/// Sets the value of an export parameter for the exported <see cref="Resource"/> 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The new value that should be assigned to the parameter.</param>
		public void SetParameter<T>(string parameterName, T value)
		{
			// Disallow adjusting parameters in the preparation step.
			if (this.isPrepareStep) throw new InvalidOperationException(
				"Cannot adjust parameter values in the preparation step. " +
				"At this point, any Resource data is considered read-only.");

			AssetInternalHelper.SetAssetInfoCustomValue<T>(this.input, parameterName, value);
			this.isAssetInfoChanged = true;
		}
	}
}
