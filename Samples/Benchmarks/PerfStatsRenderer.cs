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
	/// Renders minimalistic perf stats for the current benchmark in the bottom left corner of the screen.
	/// </summary>
	[EditorHintCategory("Benchmarks")]
	public class PerfStatsRenderer : Component, ICmpUpdatable, ICmpBenchmarkOverlayRenderer
	{
		private struct Measurement
		{
			public float LowPassValue;
			public float Min;
			public float Max;
			public float Avg;

			public void Reset()
			{
				this.Max = 0.0f;
				this.Min = 100000.0f;
				this.Avg = this.LowPassValue;
			}
			public void Update(float time)
			{
				this.Max = MathF.Max(this.Max, time);
				this.Min = MathF.Min(this.Min, time);
				this.LowPassValue += (time - this.LowPassValue) * 0.1f;
			}
		}

		[DontSerialize] private float displayUpdateTimer;
		[DontSerialize] private Measurement frameTime;
		[DontSerialize] private Measurement renderTime;
		[DontSerialize] private Measurement updateTime;


		void ICmpUpdatable.OnUpdate()
		{
			this.displayUpdateTimer += Time.DeltaTime;
			if (this.displayUpdateTimer >= 0.25f)
			{
				this.displayUpdateTimer -= 0.25f;
				this.frameTime.Reset();
				this.renderTime.Reset();
				this.updateTime.Reset();
			}

			this.frameTime.Update(Profile.TimeFrame.LastValue);
			this.renderTime.Update(Profile.TimeRender.LastValue);
			this.updateTime.Update(Profile.TimeUpdate.LastValue);
		}
		void ICmpBenchmarkOverlayRenderer.DrawOverlay(Canvas canvas)
		{
			string[] text = new string[] 
				{
					"       Frame  |   Render |   Update",
					string.Format("Min: {0,5:F} ms | {1,5:F} ms | {2,5:F} ms", this.frameTime.Min, this.renderTime.Min, this.updateTime.Min),
					string.Format("Max: {0,5:F} ms | {1,5:F} ms | {2,5:F} ms", this.frameTime.Max, this.renderTime.Max, this.updateTime.Max),
					string.Format("Avg: {0,5:F} ms | {1,5:F} ms | {2,5:F} ms", this.frameTime.Avg, this.renderTime.Avg, this.updateTime.Avg),
					"",
					string.Format("Drawcalls: {0}", Profile.StatNumDrawcalls.LastValue),
					string.Format("Batches (raw, mrg, opt): {0}, {1}, {2}", Profile.StatNumRawBatches.LastValue, Profile.StatNumMergedBatches.LastValue, Profile.StatNumOptimizedBatches.LastValue),
					string.Format("GC Collections (0, 1, 2): {0}, {1}, {2}", Profile.StatMemoryGarbageCollect0.LastValue, Profile.StatMemoryGarbageCollect1.LastValue, Profile.StatMemoryGarbageCollect2.LastValue)
				};

			canvas.DrawText(
				text, 
				10, 
				canvas.Height - 10, 
				0, 
				Alignment.BottomLeft,
				true);
		}
	}
}