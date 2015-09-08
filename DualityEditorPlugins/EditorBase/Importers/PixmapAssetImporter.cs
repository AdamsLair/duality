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

		private static bool AcceptsInput(AssetImportInput input)
		{
			string inputFileExt = Path.GetExtension(input.Path);
			bool matchingFileExt = SourceFileExts.Any(acceptedExt => string.Equals(inputFileExt, acceptedExt, StringComparison.InvariantCultureIgnoreCase));
			return matchingFileExt;
		}

		public void PrepareImport(IAssetImportEnvironment env)
		{
			foreach (AssetImportInput input in env.HandleAllInput(AcceptsInput))
			{
				env.AddOutput<Pixmap>(input.FullAssetName);
			}
		}
		public void Import(IAssetImportEnvironment env)
		{
			foreach (AssetImportInput input in env.HandleAllInput())
			{
				ContentRef<Pixmap> targetPixmapRef = env.GetOutput<Pixmap>(input.FullAssetName);
				{
					Pixmap targetPixmap = targetPixmapRef.Res;
					PixelData pixelData = this.LoadPixelData(input.Path);
					targetPixmap.MainLayer = pixelData;
					targetPixmap.SourcePath = input.Path;
				}
				env.AddOutput(targetPixmapRef);
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
