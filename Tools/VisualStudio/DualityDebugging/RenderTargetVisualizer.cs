using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.VisualStudio.DebuggerVisualizers;

using Duality;
using Duality.Resources;
using Duality.Drawing;

[assembly: DebuggerVisualizer(
	typeof(Duality.VisualStudio.BitmapDebuggerVisualizer), 
	typeof(Duality.VisualStudio.RenderTargetDebuggerVisualizerObjectSource), 
	Target = typeof(RenderTarget), 
	Description = "RenderTarget Visualizer")]

namespace Duality.VisualStudio
{
	public class RenderTargetDebuggerVisualizerObjectSource : VisualizerObjectSource
	{
		public override void GetData(object target, Stream outgoingData)
		{
			RenderTarget renderTarget = target as RenderTarget;
			PixelData layer = renderTarget.GetPixelData();
			Bitmap bitmap = layer.ToBitmap();

			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(outgoingData, renderTarget.ToString());
			formatter.Serialize(outgoingData, bitmap);
			outgoingData.Flush();
		}
	}
}
