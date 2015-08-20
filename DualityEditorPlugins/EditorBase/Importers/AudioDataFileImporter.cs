using System;
using System.IO;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Editor;
using Duality.IO;

namespace Duality.Editor.Plugins.Base
{
	public class AudioDataFileImporter : IFileImporter
	{
		public static readonly string SourceFileExtPrimary = ".ogg";
		private static readonly string[] SourceFileExts = new[] { SourceFileExtPrimary };

		public bool CanImportFile(string srcFile)
		{
			string ext = Path.GetExtension(srcFile);
			return SourceFileExts.Any(e => string.Equals(ext, e, StringComparison.InvariantCultureIgnoreCase));
		}
		public void ImportFile(string srcFile, string targetName, string targetDir)
		{
			string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);
			AudioData res = new AudioData(srcFile);
			res.Save(output[0]);
		}

		public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
		{
			return r.Is<AudioData>();
		}
		public void ReImportFile(ContentRef<Resource> r, string srcFile)
		{
			AudioData a = r.Res as AudioData;
			a.LoadOggVorbisData(srcFile);
		}

		public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
		{
			ContentRef<AudioData> a = r.As<AudioData>();
			return a != null && a.Res.SourcePath == srcFile;
		}
		public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
		{
			string targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), Resource.GetFileExtByType<AudioData>());
			return new string[] { targetResPath };
		}
	}
}
