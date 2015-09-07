using System;
using System.IO;
using System.Linq;
using System.Drawing;

using Duality;
using Duality.IO;
using Duality.Drawing;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base
{
	public class PixmapAssetImporter : IAssetImporter
	{
		public static readonly string SourceFileExtPrimary = ".png";
		private static readonly string[] SourceFileExts = new[] { SourceFileExtPrimary, ".bmp", ".jpg", ".jpeg", ".tif", ".tiff" };

		public void PrepareImport(IAssetImportEnvironment env)
		{
			foreach (AssetImportInput input in env.Input)
			{
				string ext = Path.GetExtension(input.Path);
				if (SourceFileExts.Any(e => string.Equals(ext, e, StringComparison.InvariantCultureIgnoreCase)))
				{
					if (env.HandleInput(input.Path))
					{
						string targetResPath = this.GetTargetPath(input.FullAssetName, env.TargetDirectory);
						env.AddOutput<Pixmap>(targetResPath);
					}
				}
			}
		}
		public void Import(IAssetImportEnvironment env)
		{
			foreach (AssetImportInput input in env.Input)
			{
				if (env.HandleInput(input.Path))
				{
					string targetResPath = this.GetTargetPath(input.FullAssetName, env.TargetDirectory);
					PixelData pixelData = this.LoadPixelData(input.Path);
					Pixmap res = new Pixmap(pixelData);
					res.SourcePath = input.Path;
					res.Save(targetResPath);
					env.AddOutput<Pixmap>(targetResPath);
				}
			}
		}

		//public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
		//{
		//	return r.Is<Pixmap>();
		//}
		//public void ReImportFile(ContentRef<Resource> r, string srcFile)
		//{
		//	PixelData pixelData = LoadPixelData(srcFile);
		//	Pixmap res = r.Res as Pixmap;
		//	res.MainLayer = pixelData;
		//	res.SourcePath = srcFile;
		//}

		private string GetTargetPath(string fullAssetName, string targetBaseDir)
		{
			string targetResPath = PathHelper.GetFreePath(Path.Combine(targetBaseDir, fullAssetName), Resource.GetFileExtByType<Pixmap>());
			return targetResPath;
		}
		private PixelData LoadPixelData(string filePath)
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
