using System;
using System.IO;
using System.Linq;
using System.Drawing;

using Duality;
using Duality.Drawing;
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
			PixelData pixelData = LoadPixelData(srcFile);
			Pixmap res = new Pixmap(pixelData);
			res.Save(output[0]);
		}

		public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
		{
			return r.Is<Pixmap>();
		}
		public void ReImportFile(ContentRef<Resource> r, string srcFile)
		{
			PixelData pixelData = LoadPixelData(srcFile);
			Pixmap res = r.Res as Pixmap;
			res.MainLayer = pixelData;
		}

		public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
		{
			ContentRef<Pixmap> p = r.As<Pixmap>();
			return p != null && p.Res.SourcePath == srcFile;
		}
		public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
		{
			string targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), Resource.GetFileExtByType<Pixmap>());
			return new string[] { targetResPath };
		}

		private static PixelData LoadPixelData(string filePath)
		{
			PixelData pixelData = new PixelData();
			byte[] imageData = File.ReadAllBytes(filePath);
			using (Stream stream = new MemoryStream(imageData))
			using (Bitmap bitmap = Bitmap.FromStream(stream) as Bitmap)
			{
				pixelData.FromBitmap(bitmap);
			}
			return pixelData;
		}
	}
}
