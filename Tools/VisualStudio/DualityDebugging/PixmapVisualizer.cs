using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Duality.Drawing;
using Duality.Resources;

[assembly: DebuggerVisualizer(
	typeof(Duality.VisualStudio.BitmapDebuggerVisualizer), 
	typeof(Duality.VisualStudio.PixmapDebuggerVisualizerObjectSource), 
	Target = typeof(Pixmap), 
	Description = "Pixmap Visualizer")]
[assembly: DebuggerVisualizer(
	typeof(Duality.VisualStudio.BitmapDebuggerVisualizer), 
	typeof(Duality.VisualStudio.PixmapLayerDebuggerVisualizerObjectSource), 
	Target = typeof(PixelData), 
	Description = "Pixmap Layer Visualizer")]

namespace Duality.VisualStudio
{
	public class PixmapDebuggerVisualizerObjectSource : VisualizerObjectSource
	{
		public override void GetData(object target, Stream outgoingData)
		{
			Pixmap pixmap = target as Pixmap;
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(outgoingData, pixmap.ToString());
			formatter.Serialize(outgoingData, pixmap.MainLayer.ToBitmap());
			outgoingData.Flush();
		}
	}
	public class PixmapLayerDebuggerVisualizerObjectSource : VisualizerObjectSource
	{
		public override void GetData(object target, Stream outgoingData)
		{
			PixelData layer = target as PixelData;
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(outgoingData, string.Format("Layer {0}x{1}", layer.Width, layer.Height));
			formatter.Serialize(outgoingData, layer.ToBitmap());
			outgoingData.Flush();
		}
	}
}
