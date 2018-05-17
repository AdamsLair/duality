using System;
using System.IO;
using System.Linq;

namespace Duality.Editor.AssetManagement
{
	public abstract class AssetImporter<T> : IAssetImporter where T : Resource, new()
	{
		public const int PriorityGeneral = 0;
		public const int PrioritySpecialized = 20;
		public const int PriorityOverride = 50;

		protected abstract string SourceFileExtPrimary { get; }
		protected abstract string[] SourceFileExts { get; }

		public abstract string Id { get; }
		public abstract string Name { get; }

		public virtual int Priority
		{
			get { return PriorityGeneral; }
		}

		void IAssetImporter.PrepareImport(IAssetImportEnvironment env)
		{
			// Ask to handle all input that matches the conditions in AcceptsInput
			foreach (AssetImportInput input in env.HandleAllInput(this.AcceptsInput))
			{
				// For all handled input items, specify which Resource the importer intends to create / modify
				env.AddOutput<T>(this.ResourceNameFromInput(input), input.Path);
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
				ContentRef<T> targetRef = env.GetOutput<T>(this.ResourceNameFromInput(input));

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
				env.AddOutputPath(this.OutputNameFromResource(resource));
			}
		}

		void IAssetImporter.Export(IAssetExportEnvironment env)
		{
			T resource = env.Input as T;
			string outputPath = env.AddOutputPath(this.OutputNameFromResource(resource));
			this.ExportResource(resource, outputPath, env);
		}

		protected virtual string ResourceNameFromInput(AssetImportInput input)
		{
			return input.AssetName;
		}
		protected virtual bool AcceptsInput(AssetImportInput input)
		{
			string inputFileExt = Path.GetExtension(input.Path);
			bool matchingFileExt = this.SourceFileExts.Any(acceptedExt =>
				string.Equals(inputFileExt, acceptedExt, StringComparison.InvariantCultureIgnoreCase));
			return matchingFileExt;
		}
		protected virtual string OutputNameFromResource(T resource)
		{
			return resource.Name + this.SourceFileExtPrimary;
		}
		protected virtual bool CanExport(T resource)
		{
			return true;
		}

		protected abstract void ImportResource(ContentRef<T> resourceRef, AssetImportInput input, IAssetImportEnvironment env);
		protected abstract void ExportResource(ContentRef<T> resource, string path, IAssetExportEnvironment env);
	}
}
