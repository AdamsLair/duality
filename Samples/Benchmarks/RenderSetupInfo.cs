using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;

namespace Duality.Samples.Benchmarks
{
	/// <summary>
	/// Renders information about the current rendering setup in the top right corner of the screen.
	/// </summary>
	[EditorHintCategory("Benchmarks")]
	public class RenderSetupInfo : Component, ICmpInitializable, ICmpBenchmarkOverlayRenderer
	{
		[DontSerialize] private FormattedText text;
		[DontSerialize] private ContentRef<BenchmarkRenderSetup> renderSetup;

		
		void ICmpBenchmarkOverlayRenderer.DrawOverlay(Canvas canvas)
		{
			BenchmarkRenderSetup setup = this.renderSetup.Res;
			if (setup == null) return;

			this.text.SourceText = string.Format(
				"Render Size: {0} x {1}/n" + 
				"Res. Scaling: {2:F}x/n" +
				"AA Quality: {3}",
				setup.RenderingSize.X, setup.RenderingSize.Y,
				setup.ResolutionScale,
				setup.AntialiasingQuality);
			canvas.DrawText(this.text, 
				canvas.Width - 10,
				10, 
				0, 
				null,
				Alignment.TopRight,
				true);
		}
		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate && DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				this.renderSetup = DualityApp.AppData.RenderingSetup.As<BenchmarkRenderSetup>();
				this.text = new FormattedText();
				this.text.LineAlign = Alignment.Right;
				this.text.MaxWidth = 200;
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) { }
	}
}