using System.IO;

using Duality;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base
{
	public class PixmapFileImporter : IFileImporter
	{
		public bool CanImportFile(string srcFile)
		{
			string ext = Path.GetExtension(srcFile).ToLower();
			return ext == ".png" || ext == ".bmp" || ext == ".jpg";
		}
		public void ImportFile(string srcFile, string targetName, string targetDir)
		{
			string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);
			Pixmap res = new Pixmap(srcFile);
			res.Save(output[0]);
		}
		public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
		{
			string targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), Pixmap.FileExt);
			return new string[] { targetResPath };
		}


		public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
		{
			ContentRef<Pixmap> p = r.As<Pixmap>();
			return p != null && p.Res.SourcePath == srcFile;
		}
		public void ReimportFile(ContentRef<Resource> r, string srcFile)
		{
			Pixmap p = r.Res as Pixmap;
			p.LoadPixelData(srcFile);
		}
	}
}
