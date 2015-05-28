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
			int desiredWidth = query.DesiredWidth;
			int desiredHeight = query.DesiredHeight;

			PixelData layer = pixmap.MainLayer;
			if (layer == null)
			{
				query.Result = new Bitmap(1, 1);
				return;
			}
			float widthRatio = (float)layer.Width / (float)layer.Height;

			if (pixmap.Width * pixmap.Height > 4096 * 4096)
			{
				layer = layer.CloneSubImage(
					pixmap.Width / 2 - Math.Min(desiredWidth, pixmap.Width) / 2,
					pixmap.Height / 2 - Math.Min(desiredHeight, pixmap.Height) / 2,
					Math.Min(desiredWidth, pixmap.Width),
					Math.Min(desiredHeight, pixmap.Height));
				if (layer.Width != desiredWidth || layer.Height != desiredHeight)
					layer = layer.CloneRescale(desiredWidth, desiredHeight, ImageScaleFilter.Linear);
			}
			else if (query.SizeMode == PreviewSizeMode.FixedBoth)
				layer = layer.CloneRescale(desiredWidth, desiredHeight, ImageScaleFilter.Linear);
			else if (query.SizeMode == PreviewSizeMode.FixedWidth)
				layer = layer.CloneRescale(desiredWidth, MathF.RoundToInt(desiredWidth / widthRatio), ImageScaleFilter.Linear);
			else if (query.SizeMode == PreviewSizeMode.FixedHeight)
				layer = layer.CloneRescale(MathF.RoundToInt(widthRatio * desiredHeight), desiredHeight, ImageScaleFilter.Linear);
			else
				layer = layer.Clone();

			query.Result = layer.ToBitmap();
		}
	}
}
