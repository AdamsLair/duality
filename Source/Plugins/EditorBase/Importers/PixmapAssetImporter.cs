using System;
using System.IO;
using System.Linq;
using System.Drawing;

using Duality;
using Duality.IO;
using Duality.Drawing;
using Duality.Resources;
using Duality.Editor;
using Duality.Editor.AssetManagement;

namespace Duality.Editor.Plugins.Base
{
	public class PixmapAssetImporter : AssetImporter<Pixmap>
	{
		private static readonly string[] sourceFileExts = new[] { ".png", ".bmp", ".jpg", ".jpeg", ".tif", ".tiff" };

		protected override string SourceFileExtPrimary
		{
			get { return sourceFileExts[0]; }
		}
		protected override string[] SourceFileExts
		{
			get { return sourceFileExts; }
		}

		public override string Id
		{
			get { return "BasicPixmapAssetImporter"; }
		}
		public override string Name
		{
			get { return "Pixmap Importer"; }
		}

		protected override void ImportResource(ContentRef<Pixmap> resourceRef, AssetImportInput input, IAssetImportEnvironment env)
		{
			Pixmap resource = resourceRef.Res;
			// Update pixel data from the input file
			PixelData pixelData = this.LoadPixelData(input.Path);
			resource.MainLayer = pixelData;
		}

		protected override void ExportResource(ContentRef<Pixmap> resourceRef, string path, IAssetExportEnvironment env)
		{
			Pixmap resource = resourceRef.Res;
			// Take the input Resource's pixel data and save it at the specified location
			this.SavePixelData(resource.MainLayer, path);
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
		private void SavePixelData(PixelData data, string filePath)
		{
			using (Bitmap bmp = data.ToBitmap())
			{
				bmp.Save(filePath);
			}
		}
	}
}
