using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Duality;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;
using Font = Duality.Resources.Font;


namespace Duality.Editor.Plugins.Base.PreviewGenerators
{
	public class PixmapPreviewGenerator : PreviewGenerator<Pixmap>
	{
		public override void Perform(Pixmap pixmap, PreviewImageQuery query)
		{
			Point2 desiredSize = new Point2(query.DesiredWidth, query.DesiredHeight);
			Point2 cropToSize = new Point2(4096, 4096);

			PixelData layer = pixmap.MainLayer;
			if (layer == null || layer.Width * layer.Height == 0)
			{
				query.Result = new Bitmap(1, 1);
				return;
			}

			// If the desired preview is way smaller than the source data, specify a crop size
			if (layer.Width > desiredSize.X * 8)  cropToSize.X = Math.Min(cropToSize.X, desiredSize.X * 8);
			if (layer.Height > desiredSize.Y * 8) cropToSize.Y = Math.Min(cropToSize.Y, desiredSize.Y * 8);
			
			// If out image is too big, crop it
			if (layer.Width > cropToSize.X || layer.Height > cropToSize.Y)
			{
				layer = layer.CloneSubImage(
					layer.Width / 2 - MathF.Min(layer.Width, cropToSize.X) / 2,
					layer.Height / 2 - MathF.Min(layer.Height, cropToSize.Y) / 2,
					MathF.Min(layer.Width, cropToSize.X),
					MathF.Min(layer.Height, cropToSize.Y));
			}

			// Determine the target size for the preview based on desired and actual size
			Point2 targetSize;
			float widthRatio = (float)layer.Width / (float)MathF.Max(layer.Height, 1);
			if (query.SizeMode == PreviewSizeMode.FixedWidth)
				targetSize = new Point2(desiredSize.X, MathF.RoundToInt(desiredSize.X / widthRatio));
			else if (query.SizeMode == PreviewSizeMode.FixedHeight)
				targetSize = new Point2(MathF.RoundToInt(widthRatio * desiredSize.Y), desiredSize.Y);
			else
				targetSize = desiredSize;
			
			// Create a properly resized version of the image data
			if (layer.Width != targetSize.X || layer.Height != targetSize.Y)
				layer = layer.CloneRescale(targetSize.X, targetSize.Y, ImageScaleFilter.Linear);

			query.Result = layer.ToBitmap();
		}
	}
}
