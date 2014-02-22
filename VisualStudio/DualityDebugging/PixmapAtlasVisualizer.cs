using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.VisualStudio.DebuggerVisualizers;

using Duality;
using Duality.Drawing;
using Duality.Resources;

[assembly: DebuggerVisualizer(
	typeof(Duality.VisualStudio.BitmapDebuggerVisualizer), 
	typeof(Duality.VisualStudio.PixmapAtlasDebuggerVisualizerObjectSource), 
	Target = typeof(Pixmap), 
	Description = "Pixmap Atlas Visualizer")]

namespace Duality.VisualStudio
{
	public class PixmapAtlasDebuggerVisualizerObjectSource : VisualizerObjectSource
	{
		public override void GetData(object target, Stream outgoingData)
		{
			Pixmap pixmap = target as Pixmap;
			Bitmap bitmap = pixmap.MainLayer.ToBitmap();
			VisualizeAtlas(bitmap, pixmap.Atlas);

			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(outgoingData, pixmap.ToString());
			formatter.Serialize(outgoingData, bitmap);
			outgoingData.Flush();
		}

		public static void VisualizeAtlas(Bitmap bitmap, List<Rect> atlas)
		{
			ColorRgba avgColor = bitmap.GetAverageColor();
			ColorRgba atlasColor = avgColor.GetLuminance() < 0.5f ? new ColorRgba(128, 0, 0, 164) : new ColorRgba(255, 128, 128, 164);

			// Draw atlas rects
			if (atlas != null)
			{
				Pen atlasPen = new Pen(Color.FromArgb(atlasColor.A, atlasColor.R, atlasColor.G, atlasColor.B));
				using (Graphics g = Graphics.FromImage(bitmap))
				{
					foreach (Rect r in atlas) g.DrawRectangle(atlasPen, r.X, r.Y, r.W, r.H);
				}
			}
		}
	}
}
