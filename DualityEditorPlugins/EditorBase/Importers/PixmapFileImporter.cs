using System;
using System.IO;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base
{
	public class PixmapFileImporter : IFileImporter
	{
		public static readonly string SourceFileExtPrimary = ".png";
		private static readonly string[] SourceFileExts = new[] { SourceFileExtPrimary, ".bmp", ".jpg", ".jpeg", ".tif", ".tiff" };

		public bool CanImportFile(string srcFile)
		{
			string ext = Path.GetExtension(srcFile);
			return SourceFileExts.Any(e => string.Equals(ext, e, StringComparison.InvariantCultureIgnoreCase));
		}
		public void ImportFile(string srcFile, string targetName, string targetDir)
		{
			string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);
			Pixmap res = new Pixmap(srcFile);
			res.Save(output[0]);
		}

		public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
		{
			return r.Is<Pixmap>();
		}
		public void ReImportFile(ContentRef<Resource> r, string srcFile)
		{
			Pixmap p = r.Res as Pixmap;
			p.LoadPixelData(srcFile);
		}

		public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
		{
			ContentRef<Pixmap> p = r.As<Pixmap>();
			return p != null && p.Res.SourcePath == srcFile;
		}
		public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
		{
			string targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), Pixmap.FileExt);
			return new string[] { targetResPath };
		}
	}
}
