using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.VisualStudio.DebuggerVisualizers;

using Duality;
using Duality.Resources;

[assembly: DebuggerVisualizer(
	typeof(Duality.VisualStudio.BitmapDebuggerVisualizer), 
	typeof(Duality.VisualStudio.TextureDebuggerVisualizerObjectSource), 
	Target = typeof(Texture), 
	Description = "Texture Visualizer")]

namespace Duality.VisualStudio
{
	public class TextureDebuggerVisualizerObjectSource : VisualizerObjectSource
	{
		public override void GetData(object target, Stream outgoingData)
		{
			Texture texture = target as Texture;
			Pixmap.Layer layer = texture.RetrievePixelData();
			Bitmap bitmap = layer.ToBitmap();

			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(outgoingData, texture.ToString());
			formatter.Serialize(outgoingData, bitmap);
			outgoingData.Flush();
		}
	}
}
