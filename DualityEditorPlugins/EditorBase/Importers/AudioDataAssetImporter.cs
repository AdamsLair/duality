using System;
using System.IO;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Editor;
using Duality.IO;

namespace Duality.Editor.Plugins.Base
{
	public class AudioDataAssetImporter : IAssetImporter
	{
		public static readonly string SourceFileExtPrimary = ".ogg";
		private static readonly string[] SourceFileExts = new[] { SourceFileExtPrimary };
		
		public string Id
		{
			get { return "BasicAudioDataAssetImporter"; }
		}
		public string Name
		{
			get { return "AudioData Importer"; }
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
				env.AddOutput<AudioData>(input.AssetName);
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
				ContentRef<AudioData> targetRef = env.GetOutput<AudioData>(input.AssetName);

				// If we successfully acquired one, proceed with the import
				if (targetRef.IsAvailable)
				{
					AudioData target = targetRef.Res;

					// Update audio data from the input file
					target.OggVorbisData = File.ReadAllBytes(input.Path);

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
