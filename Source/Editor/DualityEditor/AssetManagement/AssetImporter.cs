using System;
using System.IO;
using System.Linq;

namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// A helper class that implements <see cref="IAssetImporter"/> and simplifies
	/// the implementation of new importers that can only generate a single resource type.
	/// </summary>
	/// <typeparam name="T">The type of resource that this importer works with.</typeparam>
	public abstract class AssetImporter<T> : IAssetImporter where T : Resource, new()
	{
		public const int PriorityGeneral = 0;
		public const int PrioritySpecialized = 20;
		public const int PriorityOverride = 50;

		/// <summary>
		/// [GET] The main file extension that this importer handles.
		/// Used when determining the file extension of exported resources.
		/// </summary>
		protected string SourceFileExtPrimary
		{
			get { return this.SourceFileExts.Length > 0 ? this.SourceFileExts[0] : null; }
		}
		/// <summary>
		/// [GET] All file extensions that this importer handles.
		/// Used when determining whether or not this importer
		/// can handle a given input file.
		/// </summary>
		protected abstract string[] SourceFileExts { get; }

		/// <summary>
		/// [GET] A fixed system ID that represents this importer.
		/// </summary>
		public abstract string Id { get;}
		/// <summary>
		/// [GET] The user-friendly name of this importer.
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// [GET] The relative priority of this importer over others.
		/// </summary>
		public abstract int Priority { get; }

		/// <summary>
		/// Returns the name of the resource that this importer
		/// would generate for the given input. Defaults to the name
		/// of the input file, without extension.
		/// </summary>
		protected virtual string GetResourceNameFromInput(AssetImportInput input)
		{
			return input.AssetName;
		}
		/// <summary>
		/// Returns the name of the file that would be exported for the given resource.
		/// Defaults to the name of the resource with the extension <see cref="SourceFileExtPrimary"/>
		/// </summary>
		protected virtual string GetOutputNameFromResource(T resource)
		{
			return resource.Name + this.SourceFileExtPrimary;
		}

		/// <summary>
		/// Returns whether or not this importer accepts the given input.
		/// By default, compares the input files extension to <see cref="SourceFileExts"/>.
		/// </summary>
		protected virtual bool AcceptsInput(AssetImportInput input)
		{
			string inputFileExt = Path.GetExtension(input.Path);
			bool matchingFileExt = this.SourceFileExts.Any(acceptedExt =>
				string.Equals(inputFileExt, acceptedExt, StringComparison.InvariantCultureIgnoreCase));
			return matchingFileExt;
		}
		/// <summary>
		/// Returns whether or not the given resource can be exported.
		/// By default, returns true for all non-null resources.
		/// </summary>
		protected virtual bool CanExport(T resource)
		{
			return resource != null;
		}

		/// <summary>
		/// Performs the import operation for a resource.
		/// </summary>
		/// <param name="resourceRef">A <see cref="ContentRef{T}"/> pointing to the resource being imported.</param>
		/// <param name="input">The input information for the import operation.</param>
		/// <param name="env">The input environment in which the import is taking place.</param>
		protected abstract void ImportResource(ContentRef<T> resourceRef, AssetImportInput input, IAssetImportEnvironment env);
		/// <summary>
		/// Performs the export operation for a resource.
		/// </summary>
		/// <param name="resourceRef">A <see cref="ContentRef{T}"/> pointing to the resource being imported.</param>
		/// <param name="path">The export path for the resource</param>
		/// <param name="env">The input environment in which the import is taking place.</param>
		protected abstract void ExportResource(ContentRef<T> resourceRef, string path, IAssetExportEnvironment env);

		void IAssetImporter.PrepareImport(IAssetImportEnvironment env)
		{
			// Ask to handle all input that matches the conditions in AcceptsInput
			foreach (AssetImportInput input in env.HandleAllInput(this.AcceptsInput))
			{
				// For all handled input items, specify which Resource the importer intends to create / modify
				env.AddOutput<T>(this.GetResourceNameFromInput(input), input.Path);
			}
		}
		void IAssetImporter.Import(IAssetImportEnvironment env)
		{
			// Handle all available input. No need to filter or ask for this anymore, as
			// the preparation step already made a selection with AcceptsInput. We won't
			// get any input here that didn't match.
			foreach (AssetImportInput input in env.Input)
			{
				// Request a target Resource with a name matching the input
				ContentRef<T> targetRef = env.GetOutput<T>(this.GetResourceNameFromInput(input));

				// If we successfully acquired one, proceed with the import
				if (targetRef.IsAvailable)
				{
					this.ImportResource(targetRef, input, env);

					// Add the requested output to signal that we've done something with it
					env.AddOutput(targetRef, input.Path);
				}
			}
		}
		void IAssetImporter.PrepareExport(IAssetExportEnvironment env)
		{
			T resource = env.Input as T;
			if (resource != null && this.CanExport(resource))
			{
				env.AddOutputPath(this.GetOutputNameFromResource(resource));
			}
		}
		void IAssetImporter.Export(IAssetExportEnvironment env)
		{
			T resource = env.Input as T;
			string outputPath = env.AddOutputPath(this.GetOutputNameFromResource(resource));
			this.ExportResource(resource, outputPath, env);
		}
	}
}
