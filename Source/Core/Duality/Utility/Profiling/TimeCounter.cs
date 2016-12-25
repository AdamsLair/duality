using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System;

namespace Duality
{
	public class TimeCounter : ProfileCounter
	{
		// Measurement
		private	Stopwatch	watch				= new Stopwatch();
		private	float		value				= 0.0f;
		private	float		lastValue			= 0.0f;
		// Report data
		private	double		accumValue			= 0.0d;
		private	float		accumMaxValue		= float.MinValue;
		private	float		accumMinValue		= float.MaxValue;
		private	float[]		valueGraph			= new float[ValueHistoryLen];
		private	int			valueGraphCursor	= 0;


		internal float Value
		{
			get { return this.value; }
		}
		public float LastValue
		{
			get { return this.lastValue; }
		}
		public float[] ValueGraph
		{
			get { return this.valueGraph; }
		}
		public int ValueGraphCursor
		{
			get { return this.valueGraphCursor; }
		}


		public void BeginMeasure()
		{
			this.watch.Restart();
		}
		public void EndMeasure()
		{
			this.value += this.watch.ElapsedTicks * 1000.0f / Stopwatch.Frequency;
			this.used = true;
		}
		public void Add(float value)
		{
			this.value += value;
			this.used = true;
		}
		public void Set(float value)
		{
			this.value = value;
			this.used = true;
		}
		public override void ResetFrame()
		{
			this.lastUsed = this.used;
			this.used = false;

			this.lastValue = this.value;
			this.value = 0.0f;
		}
		public override void ResetAll()
		{
			this.ResetFrame();
			this.accumValue = 0.0d;
			this.accumMaxValue = float.MinValue;
			this.accumMinValue = float.MaxValue;
			this.sampleCount = 0;
		}

		public override void GetReportData(out ProfileReportCounterData data)
		{
			data = new ProfileReportCounterData();
			data.Severity = MathF.Clamp(this.lastValue / Time.MsPFMult, 0.0f, 1.0f);
			data.LastValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F}", this.lastValue);
			if (this.IsSingleValue)
			{
				data.AverageValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F}", this.lastValue);
			}
			else
			{
				if (this.sampleCount > 0)
				{
					data.AverageValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F}", (float)(this.accumValue / (double)this.sampleCount));
					data.MinValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F}", this.accumMinValue);
					data.MaxValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F}", this.accumMaxValue);
				}
				data.SampleCount = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", this.sampleCount);
			}
		}
		protected override void OnFrameTick()
		{
			if (this.used)
			{
				this.sampleCount++;
				this.accumMaxValue = MathF.Max(this.value, this.accumMaxValue);
				this.accumMinValue = MathF.Min(this.value, this.accumMinValue);
				this.accumValue += this.value;
			}
			this.valueGraph[this.valueGraphCursor] = this.value;
			this.valueGraphCursor = (this.valueGraphCursor + 1) % this.valueGraph.Length;
			this.ResetFrame();
		}
	}
}
