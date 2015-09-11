using System;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base
{
	public class ShaderAssetImporter : IAssetImporter
	{
		public static readonly string SourceFileExtVertex = ".vert";
		public static readonly string SourceFileExtFragment = ".frag";
		private static readonly string[] SourceFileExts = new[] { SourceFileExtVertex, SourceFileExtFragment };
		
		public string Id
		{
			get { return "BasicShaderAssetImporter"; }
		}
		public string Name
		{
			get { return "GLSL Shader Importer"; }
		}
		public int Priority
		{
			get { return 0; }
		}

		public void PrepareImport(IAssetImportEnvironment env)
		{
			// Ask to handle all input that matches the conditions in AcceptsInput
			foreach (AssetImportInput input in env.HandleAllInput(this.AcceptsInput))
			{
				// For all handled input items, specify which Resource the importer intends to create / modify
				string ext = Path.GetExtension(input.Path);
				if (string.Equals(ext, SourceFileExtVertex, StringComparison.InvariantCultureIgnoreCase))
					env.AddOutput<VertexShader>(input.AssetName, input.Path);
				else
					env.AddOutput<FragmentShader>(input.AssetName, input.Path);
			}
		}
		public void Import(IAssetImportEnvironment env)
		{
			// Handle all available input. No need to filter or ask for this anymore, as
			// the preparation step already made a selection with AcceptsInput. We won't
			// get any input here that didn't match.
			foreach (AssetImportInput input in env.Input)
			{
				string ext = Path.GetExtension(input.Path);

				// Request a target Resource with a name matching the input
				IContentRef targetRef;
				if (string.Equals(ext, SourceFileExtVertex, StringComparison.InvariantCultureIgnoreCase))
					targetRef = env.GetOutput<VertexShader>(input.AssetName);
				else
					targetRef = env.GetOutput<FragmentShader>(input.AssetName);

				// If we successfully acquired one, proceed with the import
				if (targetRef.IsAvailable)
				{
					AbstractShader target = targetRef.Res as AbstractShader;

					// Update shader data from the input file
					target.Source = File.ReadAllText(input.Path);
					target.Compile();

					// Add the requested output to signal that we've done something with it
					env.AddOutput(targetRef, input.Path);
				}
			}
		}

		public void PrepareExport(IAssetExportEnvironment env)
		{
			// We can export any Resource that is a shader
			if (env.Input is AbstractShader)
			{
				// Add the file path of the exported output we'll produce.
				if (env.Input is FragmentShader)
					env.AddOutputPath(env.Input.Name + SourceFileExtFragment);
				else
					env.AddOutputPath(env.Input.Name + SourceFileExtVertex);
			}
		}
		public void Export(IAssetExportEnvironment env)
		{
			// Determine input and output path
			AbstractShader input = env.Input as AbstractShader;
			string outputPath;
			if (env.Input is FragmentShader)
				outputPath = env.AddOutputPath(input.Name + SourceFileExtFragment);
			else
				outputPath = env.AddOutputPath(input.Name + SourceFileExtVertex);

			// Take the input Resource's TrueType font data and save it at the specified location
			File.WriteAllText(outputPath, input.Source);
		}
		
		private bool AcceptsInput(AssetImportInput input)
		{
			string inputFileExt = Path.GetExtension(input.Path);
			bool matchingFileExt = SourceFileExts.Any(acceptedExt => string.Equals(inputFileExt, acceptedExt, StringComparison.InvariantCultureIgnoreCase));
			return matchingFileExt;
		}
	}
}
