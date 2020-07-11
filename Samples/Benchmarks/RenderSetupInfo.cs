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
		[DontSerialize] private VertexC1P3T2[][] textBufferGlyphs;
		[DontSerialize] private VertexC1P3T2[] textBufferIcons;

		[DontSerialize] private Point2 displayedRenderSize;
		[DontSerialize] private float displayedRenderScale;
		[DontSerialize] private AAQuality displayedAAQuality;

		
		void ICmpBenchmarkOverlayRenderer.DrawOverlay(Canvas canvas)
		{
			BenchmarkRenderSetup setup = this.renderSetup.Res;
			if (setup == null) return;

			// Update the displayed info text only when the data changes.
			// This is a benchmark. We don't want any allocation or perf
			// noise in our setup.
			if (this.displayedRenderSize != setup.RenderingSize ||
				this.displayedRenderScale != setup.ResolutionScale ||
				this.displayedAAQuality != setup.AntialiasingQuality)
			{
				this.displayedRenderSize = setup.RenderingSize;
				this.displayedRenderScale = setup.ResolutionScale;
				this.displayedAAQuality = setup.AntialiasingQuality;
				this.text.SourceText = string.Format(
					"Render Size: {0} x {1}/n" + 
					"Res. Scaling: {2:F}x/n" +
					"AA Quality: {3}",
					this.displayedRenderSize.X, this.displayedRenderSize.Y,
					this.displayedRenderScale,
					this.displayedAAQuality);
			}

			canvas.DrawText(this.text, 
				ref this.textBufferGlyphs,
				ref this.textBufferIcons,
				canvas.Width - 10,
				10, 
				0, 
				null,
				Alignment.TopRight,
				true);
		}
		void ICmpInitializable.OnActivate()
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				this.renderSetup = DualityApp.AppData.Instance.RenderingSetup.As<BenchmarkRenderSetup>();
				this.text = new FormattedText();
				this.text.LineAlign = Alignment.Right;
				this.text.MaxWidth = 200;
			}
		}
		void ICmpInitializable.OnDeactivate() { }
	}
}