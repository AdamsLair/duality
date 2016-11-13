using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Base.PreviewGenerators
{
	public class TilesetPreviewGenerator : PreviewGenerator<Tileset>
	{
		public override void Perform(Tileset tileset, PreviewImageQuery query)
		{
			int desiredWidth = query.DesiredWidth;
			int desiredHeight = query.DesiredHeight;

			TilesetRenderInput input = tileset.RenderConfig.FirstOrDefault(c => c.SourceData != null);
			Pixmap mainPixmap = (input != null) ? input.SourceData.Res : null;
			PixelData layer = (mainPixmap != null) ? mainPixmap.MainLayer : null;
			if (layer == null)
			{
				query.Result = new Bitmap(1, 1);
				return;
			}
			float widthRatio = (float)layer.Width / (float)layer.Height;

			layer = layer.CloneSubImage(0, 0, desiredWidth * 2, desiredHeight * 2);
			layer.Rescale(desiredWidth, desiredHeight);

			query.Result = layer.ToBitmap();
		}
	}
}
