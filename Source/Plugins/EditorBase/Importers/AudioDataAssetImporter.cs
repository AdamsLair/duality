using System;
using System.IO;
using System.Linq;

using Duality;
using Duality.IO;
using Duality.Resources;
using Duality.Editor;
using Duality.Editor.AssetManagement;

namespace Duality.Editor.Plugins.Base
{
	public class AudioDataAssetImporter : AssetImporter<AudioData>
	{
		public AudioDataAssetImporter()
			: base("AudioData Importer", "BasicAudioDataAssetImporter", PriorityGeneral, ".ogg")
		{
		}

		protected override void ImportResource(ContentRef<AudioData> resourceRef, AssetImportInput input, IAssetImportEnvironment env)
		{
			AudioData resource = resourceRef.Res;
			// Update audio data from the input file
			resource.OggVorbisData = File.ReadAllBytes(input.Path);
		}

		protected override bool CanExport(AudioData resource)
		{
			return true;
		}

		protected override void ExportResource(ContentRef<AudioData> resourceRef, string path, IAssetExportEnvironment env)
		{
			AudioData resource = resourceRef.Res;
			// Take the input Resource's audio data and save it at the specified location
			File.WriteAllBytes(path, resource.OggVorbisData);
		}
	}
}
