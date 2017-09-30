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
		private struct TimeMeasurement
		{
			public float LowPassValue;
			public float Min;
			public float Max;
			public float Avg;

			public void UpdateDisplay()
			{
				this.Max = 0.0f;
				this.Min = 100000.0f;
				this.Avg = this.LowPassValue;
			}
			public void TickFrame(float time)
			{
				this.Max = MathF.Max(this.Max, time);
				this.Min = MathF.Min(this.Min, time);
				this.LowPassValue += (time - this.LowPassValue) * 0.1f;
			}
		}
		private struct GCCollectMeasurement
		{
			public int LastCount;
			public int LastFrameIndex;
			public int TotalFrames;
			public int TotalCount;
			public float GCsPerMinute;

			public void UpdateDisplay()
			{
				if (this.TotalFrames == 0)
				{
					this.GCsPerMinute = 0.0f;
				}
				else
				{
					float gcsPerFrame = (float)this.TotalCount / (float)this.TotalFrames;
					float gcsPerMinuteAtDefaultFps = 60.0f * Time.FramesPerSecond * gcsPerFrame;
					this.GCsPerMinute = gcsPerMinuteAtDefaultFps;
				}
			}
			public void TickFrame(int gcCountSinceStart)
			{
				if (this.LastCount > 0)
				{
					int frames = Time.FrameCount - this.LastFrameIndex;
					int increase = gcCountSinceStart - this.LastCount;
				
					this.TotalFrames += frames;
					this.TotalCount += increase;
				}
				this.LastFrameIndex = Time.FrameCount;
				this.LastCount = gcCountSinceStart;
			}
		}

		[DontSerialize] private float initialWarmupTimer = 2.0f;
		[DontSerialize] private float resetMinMaxTimer = 0.1f;
		[DontSerialize] private TimeMeasurement frameTime;
		[DontSerialize] private TimeMeasurement renderTime;
		[DontSerialize] private TimeMeasurement updateTime;
		[DontSerialize] private GCCollectMeasurement gcGen0;
		[DontSerialize] private GCCollectMeasurement gcGen1;
		[DontSerialize] private GCCollectMeasurement gcGen2;
		[DontSerialize] private string[] text;
		[DontSerialize] private VertexC1P3T2[][] textBufferGlyphs;


		public void ExportText(StringBuilder targetBuilder)
		{
			for (int i = 0; i < this.text.Length; i++)
			{
				targetBuilder.AppendLine(this.text[i]);
			}
		}

		private void UpdateText()
		{
			if (this.text == null)
				this.text = new string[8];

			this.text[0] = "       Frame  |   Render |   Update";
			this.text[1] = string.Format("Min: {0,5:F} ms | {1,5:F} ms | {2,5:F} ms", this.frameTime.Min, this.renderTime.Min, this.updateTime.Min);
			this.text[2] = string.Format("Max: {0,5:F} ms | {1,5:F} ms | {2,5:F} ms", this.frameTime.Max, this.renderTime.Max, this.updateTime.Max);
			this.text[3] = string.Format("Avg: {0,5:F} ms | {1,5:F} ms | {2,5:F} ms", this.frameTime.Avg, this.renderTime.Avg, this.updateTime.Avg);
			this.text[4] = "";
			this.text[5] = string.Format("Drawcalls: {0}", Profile.StatNumDrawcalls.LastValue);
			this.text[6] = string.Format("Batches (raw, mrg, opt): {0}, {1}, {2}", Profile.StatNumRawBatches.LastValue, Profile.StatNumMergedBatches.LastValue, Profile.StatNumOptimizedBatches.LastValue);
			this.text[7] = string.Format("GC Collections (0, 1, 2): {0:F1}, {1:F1}, {2:F1} / minute at 60 FPS", this.gcGen0.GCsPerMinute, this.gcGen1.GCsPerMinute, this.gcGen2.GCsPerMinute);
		}

		void ICmpUpdatable.OnUpdate()
		{
			this.initialWarmupTimer -= Time.DeltaTime;
			if (this.initialWarmupTimer > 0.0f) return;

			this.resetMinMaxTimer -= Time.DeltaTime;
			if (this.resetMinMaxTimer <= 0.0f)
			{
				this.resetMinMaxTimer += 1.0f;

				this.UpdateText();

				this.frameTime.UpdateDisplay();
				this.renderTime.UpdateDisplay();
				this.updateTime.UpdateDisplay();
				this.gcGen0.UpdateDisplay();
				this.gcGen1.UpdateDisplay();
				this.gcGen2.UpdateDisplay();
			}

			this.frameTime.TickFrame(Profile.TimeFrame.LastValue);
			this.renderTime.TickFrame(Profile.TimeRender.LastValue);
			this.updateTime.TickFrame(Profile.TimeUpdate.LastValue);
			this.gcGen0.TickFrame(Profile.StatMemoryGarbageCollect0.LastValue);
			this.gcGen1.TickFrame(Profile.StatMemoryGarbageCollect1.LastValue);
			this.gcGen2.TickFrame(Profile.StatMemoryGarbageCollect2.LastValue);
		}
		void ICmpBenchmarkOverlayRenderer.DrawOverlay(Canvas canvas)
		{
			canvas.DrawText(
				this.text, 
				ref this.textBufferGlyphs,
				10, 
				canvas.Height - 10, 
				0, 
				Alignment.BottomLeft,
				true);
		}
	}
}