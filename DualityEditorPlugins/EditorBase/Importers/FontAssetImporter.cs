using System;
using System.IO;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Editor;

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
				env.AddOutput<Font>(input.AssetName);
			}
		}
		public void Import(IAssetImportEnvironment env)
		{
			// Ask to handle all available input. No need to filter this anymore, as
			// the preparation step already made a selection with AcceptsInput. We won't
			// get any input here that didn't match.
			foreach (AssetImportInput input in env.HandleAllInput())
			{
				// Request a target Resource with a name matching the input
				ContentRef<Font> targetRef = env.GetOutput<Font>(input.AssetName);

				// If we successfully acquired one, proceed with the import
				if (targetRef.IsAvailable)
				{
					Font target = targetRef.Res;

					// Update font data from the input file
					target.EmbeddedTrueTypeFont = File.ReadAllBytes(input.Path);
					target.RenderGlyphs();

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
