using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.VisualStudio.DebuggerVisualizers;

using Duality;
using Duality.Drawing;
using Duality.Resources;

[assembly: DebuggerVisualizer(
	typeof(Duality.VisualStudio.BitmapDebuggerVisualizer), 
	typeof(Duality.VisualStudio.TextureAtlasDebuggerVisualizerObjectSource), 
	Target = typeof(Texture), 
	Description = "Texture Atlas Visualizer")]

namespace Duality.VisualStudio
{
	public class TextureAtlasDebuggerVisualizerObjectSource : VisualizerObjectSource
	{
		public override void GetData(object target, Stream outgoingData)
		{
			Texture texture = target as Texture;
			Pixmap.Layer layer = texture.RetrievePixelData();
			Bitmap bitmap = layer.ToBitmap();

			if (texture.BasePixmap.Res != null)
			{
				PixmapAtlasDebuggerVisualizerObjectSource.VisualizeAtlas(bitmap, texture.BasePixmap.Res.Atlas);
			}

			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(outgoingData, texture.ToString());
			formatter.Serialize(outgoingData, bitmap);
			outgoingData.Flush();
		}
	}
}
