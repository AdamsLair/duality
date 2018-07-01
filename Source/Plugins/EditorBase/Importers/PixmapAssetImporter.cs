using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

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
		private static readonly string[] SourceFileExtensions = new[] { ".png", ".bmp", ".jpg", ".jpeg", ".tif", ".tiff" };


		public override string Id
		{
			get { return "BasicPixmapAssetImporter"; }
		}
		public override string Name
		{
			get { return "Pixmap Importer"; }
		}
		public override int Priority
		{
			get { return PriorityGeneral; }
		}
		protected override string[] SourceFileExts
		{
			get { return SourceFileExtensions; }
		}


		protected override void ImportResource(ContentRef<Pixmap> resourceRef, AssetImportInput input, IAssetImportEnvironment env)
		{
			Pixmap resource = resourceRef.Res;

			// Retrieve import parameters
			int sheetCols   = env.GetOrInitParameter(resourceRef, "SpriteSheetColumns", 0);
			int sheetRows   = env.GetOrInitParameter(resourceRef, "SpriteSheetRows"   , 0);
			int frameBorder = env.GetOrInitParameter(resourceRef, "SpriteFrameBorder" , 0);

			// Clamp import parameters
			if (sheetCols   < 0) sheetCols   = 0;
			if (sheetRows   < 0) sheetRows   = 0;
			if (frameBorder < 0) frameBorder = 0;
			env.SetParameter(resourceRef, "SpriteSheetColumns", sheetCols  );
			env.SetParameter(resourceRef, "SpriteSheetRows"   , sheetRows  );
			env.SetParameter(resourceRef, "SpriteFrameBorder" , frameBorder);

			// Update pixel data from the input file
			PixelData pixelData = this.LoadPixelData(input.Path);
			resource.MainLayer = pixelData;

			// Generate a sprite sheet atlas
			if (sheetCols > 0 && sheetRows > 0)
			{
				this.GenerateSpriteSheetAtlas(resource, sheetCols, sheetRows, frameBorder);
			}
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

		/// <summary>
		/// Generates a regular sprite sheet <see cref="Pixmap.Atlas"/> with a fixed number 
		/// of columns and rows.
		/// </summary>
		/// <param name="cols">The number of columns in the sprite sheet.</param>
		/// <param name="rows">The number of rows in the sprite sheet.</param>
		/// <param name="frameBorder">Border size around each sprite sheet rect.</param>
		private void GenerateSpriteSheetAtlas(Pixmap pixmap, int cols, int rows, int frameBorder)
		{
			// Determine working data
			Vector2 frameSize = new Vector2((float)pixmap.Width / cols, (float)pixmap.Height / rows);
			int frameCount = cols * rows;

			// Clear old atlas
			if (pixmap.Atlas == null)
				pixmap.Atlas = new List<Rect>(frameCount);
			else
				pixmap.Atlas.Clear();

			// Set up new atlas
			if (frameCount > 0)
			{
				for (int y = 0; y < rows; y++)
				{
					for (int x = 0; x < cols; x++)
					{
						Rect frameRect = new Rect(
							x * frameSize.X + frameBorder,
							y * frameSize.Y + frameBorder,
							frameSize.X - frameBorder * 2,
							frameSize.Y - frameBorder * 2);
						pixmap.Atlas.Add(frameRect);
					}
				}
			}
		}
	}
}
