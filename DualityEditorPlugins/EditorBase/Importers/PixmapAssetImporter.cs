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
				env.AddOutput<Pixmap>(input.AssetName);
			}
		}
		public void Import(IAssetImportEnvironment env)
		{
			// Ask to handle all available input. No need to filter this anymore, as
			// the preparation step already made a selection with AcceptsInput. We won't
			// get any input here that didn't match.
			foreach (AssetImportInput input in env.HandleAllInput())
			{
				// Request a target Resource with a name matching the input
				ContentRef<Pixmap> targetRef = env.GetOutput<Pixmap>(input.AssetName);

				// If we successfully acquired one, proceed with the import
				if (targetRef.IsAvailable)
				{
					Pixmap target = targetRef.Res;

					// Update pixel data from the input file
					PixelData pixelData = this.LoadPixelData(input.Path);
					target.MainLayer = pixelData;
					target.SourcePath = input.Path;

					// Add the requested output to signal that we've done something with it
					env.AddOutput(targetRef);
				}
			}
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
	}

	public class PixmapAssetImporter2 : IAssetImporter
	{
		public static readonly string SourceFileExtPrimary = ".png";
		private static readonly string[] SourceFileExts = new[] { SourceFileExtPrimary, ".bmp", ".jpg", ".jpeg", ".tif", ".tiff" };
		
		public string Id
		{
			get { return "BasicPixmapAssetImporter2"; }
		}
		public string Name
		{
			get { return "Pixmap Importer 2"; }
		}
		public int Priority
		{
			get { return -1; }
		}

		public void PrepareImport(IAssetImportEnvironment env)
		{
			// Ask to handle all input that matches the conditions in AcceptsInput
			foreach (AssetImportInput input in env.HandleAllInput(this.AcceptsInput))
			{
				// For all handled input items, specify which Resource the importer intends to create / modify
				env.AddOutput<Pixmap>(input.AssetName);
			}
		}
		public void Import(IAssetImportEnvironment env) { }
		
		private bool AcceptsInput(AssetImportInput input)
		{
			string inputFileExt = Path.GetExtension(input.Path);
			bool matchingFileExt = SourceFileExts.Any(acceptedExt => string.Equals(inputFileExt, acceptedExt, StringComparison.InvariantCultureIgnoreCase));
			return matchingFileExt;
		}
	}
	public class PixmapAssetImporter3 : IAssetImporter
	{
		public static readonly string SourceFileExtPrimary = ".png";
		private static readonly string[] SourceFileExts = new[] { SourceFileExtPrimary, ".bmp", ".jpg", ".jpeg", ".tif", ".tiff" };
		
		public string Id
		{
			get { return "BasicPixmapAssetImporter3"; }
		}
		public string Name
		{
			get { return "Pixmap Importer 3"; }
		}
		public int Priority
		{
			get { return -2; }
		}

		public void PrepareImport(IAssetImportEnvironment env)
		{
			// Ask to handle all input that matches the conditions in AcceptsInput
			foreach (AssetImportInput input in env.HandleAllInput(this.AcceptsInput))
			{
				// For all handled input items, specify which Resource the importer intends to create / modify
				env.AddOutput<Pixmap>(input.AssetName);
			}
		}
		public void Import(IAssetImportEnvironment env) { }
		
		private bool AcceptsInput(AssetImportInput input)
		{
			string inputFileExt = Path.GetExtension(input.Path);
			bool matchingFileExt = SourceFileExts.Any(acceptedExt => string.Equals(inputFileExt, acceptedExt, StringComparison.InvariantCultureIgnoreCase));
			return matchingFileExt;
		}
	}
}
