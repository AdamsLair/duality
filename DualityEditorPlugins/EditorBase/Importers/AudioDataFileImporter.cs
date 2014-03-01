using System.IO;

using Duality;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base
{
	public class AudioDataFileImporter : IFileImporter
	{
		public bool CanImportFile(string srcFile)
		{
			string ext = Path.GetExtension(srcFile).ToLower();
			return ext == ".ogg";
		}
		public void ImportFile(string srcFile, string targetName, string targetDir)
		{
			string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);
			AudioData res = new AudioData(srcFile);
			res.Save(output[0]);
		}
		public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
		{
			string targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), AudioData.FileExt);
			return new string[] { targetResPath };
		}


		public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
		{
			ContentRef<AudioData> a = r.As<AudioData>();
			return a != null && a.Res.SourcePath == srcFile;
		}
		public void ReimportFile(ContentRef<Resource> r, string srcFile)
		{
			AudioData a = r.Res as AudioData;
			a.LoadOggVorbisData(srcFile);
		}
	}
}
