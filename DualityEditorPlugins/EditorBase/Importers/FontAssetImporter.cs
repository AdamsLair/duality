using System;
using System.IO;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Editor;
using Duality.Editor.AssetManagement;

namespace Duality.Editor.Plugins.Base
{
	public class FontAssetImporter : IAssetImporter
	{
		public static readonly string SourceFileExtPrimary = ".ttf";
		private static readonly string[] SourceFileExts = new[] { SourceFileExtPrimary };
		
		public string Id
		{
			get { return "BasicFontAssetImporter"; }
		}
		public string Name
		{
			get { return "TrueType Font Importer"; }
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
				env.AddOutput<Font>(input.AssetName, input.Path);
			}
		}
		public void Import(IAssetImportEnvironment env)
		{
			// Handle all available input. No need to filter or ask for this anymore, as
			// the preparation step already made a selection with AcceptsInput. We won't
			// get any input here that didn't match.
			foreach (AssetImportInput input in env.Input)
			{
				// Request a target Resource with a name matching the input
				ContentRef<Font> targetRef = env.GetOutput<Font>(input.AssetName);

				// If we successfully acquired one, proceed with the import
				if (targetRef.IsAvailable)
				{
					Font target = targetRef.Res;

					// Update font data from the input file
					target.EmbeddedTrueTypeFont = File.ReadAllBytes(input.Path);
					target.RenderGlyphs(null);

					// Add the requested output to signal that we've done something with it
					env.AddOutput(targetRef, input.Path);
				}
			}
		}

		public void PrepareExport(IAssetExportEnvironment env)
		{
			// We can export any Resource that is a Font with an embedded TrueType face
			Font input = env.Input as Font;
			if (input != null && input.EmbeddedTrueTypeFont != null)
			{
				// Add the file path of the exported output we'll produce.
				env.AddOutputPath(env.Input.Name + SourceFileExtPrimary);
			}
		}
		public void Export(IAssetExportEnvironment env)
		{
			// Determine input and output path
			Font input = env.Input as Font;
			string outputPath = env.AddOutputPath(input.Name + SourceFileExtPrimary);

			// Take the input Resource's TrueType font data and save it at the specified location
			File.WriteAllBytes(outputPath, input.EmbeddedTrueTypeFont);
		}
		
		private bool AcceptsInput(AssetImportInput input)
		{
			string inputFileExt = Path.GetExtension(input.Path);
			bool matchingFileExt = SourceFileExts.Any(acceptedExt => string.Equals(inputFileExt, acceptedExt, StringComparison.InvariantCultureIgnoreCase));
			return matchingFileExt;
		}
	}
}
