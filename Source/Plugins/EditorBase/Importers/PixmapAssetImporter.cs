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
	public class PixmapAssetImporter : IAssetImporter
	{
		public static readonly string SourceFileExtPrimary = ".png";
		private static readonly string[] SourceFileExts = new[] { SourceFileExtPrimary, ".bmp", ".jpg", ".jpeg", ".tif", ".tiff" };
		
		public string Id
		{
			get { return "BasicPixmapAssetImporter"; }
		}
		public string Name
		{
			get { return "Pixmap Importer"; }
		}
		public int Priority
		{
			get { return 0; }
		}

		public void PrepareImport(IAssetImportEnvironment env)
		{
			// Ask to handle all input that matches the conditions in AcceptsInput
			foreach (AssetImportInput input in env.HandleAllInput(this.AcceptsInput))
			{
				// For all handled input items, specify which Resource the importer intends to create / modify
				env.AddOutput<Pixmap>(input.AssetName, input.Path);
			}
		}
		public void Import(IAssetImportEnvironment env)
		{
			// Handle all available input. No need to filter or ask for this anymore, as
			// the preparation step already made a selection with AcceptsInput. We won't
			// get any input here that didn't match.
			foreach (AssetImportInput input in env.Input)
			{
				// Request a target Resource with a name matching the input
				ContentRef<Pixmap> targetRef = env.GetOutput<Pixmap>(input.AssetName);

				// If we successfully acquired one, proceed with the import
				if (targetRef.IsAvailable)
				{
					Pixmap target = targetRef.Res;

					// Retrieve import parameters
					int sheetCols   = env.GetOrInitParameter(targetRef, "SpriteSheetColumns", 0);
					int sheetRows   = env.GetOrInitParameter(targetRef, "SpriteSheetRows"   , 0);
					int frameBorder = env.GetOrInitParameter(targetRef, "SpriteFrameBorder" , 0);

					// Clamp import parameters
					if (sheetCols   < 0) sheetCols   = 0;
					if (sheetRows   < 0) sheetRows   = 0;
					if (frameBorder < 0) frameBorder = 0;
					env.SetParameter(targetRef, "SpriteSheetColumns", sheetCols  );
					env.SetParameter(targetRef, "SpriteSheetRows"   , sheetRows  );
					env.SetParameter(targetRef, "SpriteFrameBorder" , frameBorder);

					// Update pixel data from the input file
					PixelData pixelData = this.LoadPixelData(input.Path);
					target.MainLayer = pixelData;

					// Generate a sprite sheet atlas
					if (sheetCols > 0 && sheetRows > 0)
					{
						this.GenerateSpriteSheetAtlas(target, sheetCols, sheetRows, frameBorder);
					}

					// Add the requested output to signal that we've done something with it
					env.AddOutput(targetRef, input.Path);
				}
			}
		}
		
		public void PrepareExport(IAssetExportEnvironment env)
		{
			// We can export any Resource that is a Pixmap
			if (env.Input is Pixmap)
			{
				// Add the file path of the exported output we'll produce.
				env.AddOutputPath(env.Input.Name + SourceFileExtPrimary);
			}
		}
		public void Export(IAssetExportEnvironment env)
		{
			// Determine input and output path
			Pixmap input = env.Input as Pixmap;
			string outputPath = env.AddOutputPath(input.Name + SourceFileExtPrimary);

			// Take the input Resource's pixel data and save it at the specified location
			this.SavePixelData(input.MainLayer, outputPath);
		}
		
		private bool AcceptsInput(AssetImportInput input)
		{
			string inputFileExt = Path.GetExtension(input.Path);
			bool matchingFileExt = SourceFileExts.Any(acceptedExt => string.Equals(inputFileExt, acceptedExt, StringComparison.InvariantCultureIgnoreCase));
			return matchingFileExt;
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
