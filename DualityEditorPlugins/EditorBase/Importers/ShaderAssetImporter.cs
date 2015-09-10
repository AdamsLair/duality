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
					env.AddOutput<VertexShader>(input.AssetName);
				else
					env.AddOutput<FragmentShader>(input.AssetName);
			}
		}
		public void Import(IAssetImportEnvironment env)
		{
			// Ask to handle all available input. No need to filter this anymore, as
			// the preparation step already made a selection with AcceptsInput. We won't
			// get any input here that didn't match.
			foreach (AssetImportInput input in env.HandleAllInput())
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
					env.AddOutput(targetRef);
				}
			}
		}
		
		private bool AcceptsInput(AssetImportInput input)
		{
			string inputFileExt = Path.GetExtension(input.Path);
			bool matchingFileExt = SourceFileExts.Any(acceptedExt => string.Equals(inputFileExt, acceptedExt, StringComparison.InvariantCultureIgnoreCase));
			return matchingFileExt;
		}
	}
}
