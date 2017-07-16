using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Drawing;
using Duality.Resources;

namespace Duality.Samples.Benchmarks
{
	public class PerfStatsRenderer : Component, ICmpUpdatable, ICmpBenchmarkOverlayRenderer
	{
		[DontSerialize] private float displayUpdateTimer;
		[DontSerialize] private float smoothedFrameTime;
		[DontSerialize] private float displayedMinTime;
		[DontSerialize] private float displayedMaxTime;
		[DontSerialize] private float displayedSmoothTime;
		
		void ICmpUpdatable.OnUpdate()
		{
			this.displayUpdateTimer += Time.DeltaTime;
			if (this.displayUpdateTimer >= 0.25f)
			{
				this.displayUpdateTimer -= 0.25f;
				this.displayedMaxTime = 0.0f;
				this.displayedMinTime = 100000.0f;
				this.displayedSmoothTime = this.smoothedFrameTime;
			}

			this.displayedMaxTime = MathF.Max(this.displayedMaxTime, Profile.TimeFrame.LastValue);
			this.displayedMinTime = MathF.Min(this.displayedMinTime, Profile.TimeFrame.LastValue);
			this.smoothedFrameTime += (Profile.TimeFrame.LastValue - this.smoothedFrameTime) * 0.1f;
		}
		void ICmpBenchmarkOverlayRenderer.DrawOverlay(Canvas canvas)
		{
			canvas.DrawText(new string[] 
				{
					string.Format("Min: {0:F} ms", this.displayedMinTime),
					string.Format("Max: {0:F} ms", this.displayedMaxTime),
					string.Format("Avg: {0:F} ms", this.displayedSmoothTime)
				}, 
				10, 
				canvas.Height - 10, 
				0, 
				Alignment.BottomLeft,
				true);
		}
	}
}