using System.IO;

using Duality;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base
{
	public class FontFileImporter : IFileImporter
	{
		public bool CanImportFile(string srcFile)
		{
			string ext = Path.GetExtension(srcFile).ToLower();
			return ext == ".ttf";
		}
		public void ImportFile(string srcFile, string targetName, string targetDir)
		{
			string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);
			Font res = new Font();
			res.LoadCustomFamilyData(srcFile);
			res.ReloadData();
			res.Save(output[0]);
		}

		public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
		{
			return r.Is<Font>();
		}
		public void ReImportFile(ContentRef<Resource> r, string srcFile)
		{
			Font f = r.Res as Font;
			f.LoadCustomFamilyData(srcFile);
			f.ReloadData();
		}

		public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
		{
			ContentRef<Font> f = r.As<Font>();
			return f != null && f.Res.CustomFamilyData != null && f.Res.SourcePath == srcFile;
		}
		public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
		{
			string targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), Resource.GetFileExtByType<Font>());
			return new string[] { targetResPath };
		}
	}
}
