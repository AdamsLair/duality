﻿using System.Collections.Generic;
using System.Linq;
using System;

namespace Duality
{
	public class StatCounter : ProfileCounter
	{
		// Measurement
		private	int		value				= 0;
		private	int		lastValue			= 0;
		// Report data
		private	long	accumValue			= 0;
		private	int		accumMaxValue		= int.MinValue;
		private	int		accumMinValue		= int.MaxValue;
		private	int[]	valueGraph			= new int[ValueHistoryLen];
		private	int		valueGraphCursor	= 0;
			

		public int LastValue
		{
			get { return this.lastValue; }
		}
		public int MinValue
		{
			get { return this.accumMinValue; }
		}
		public int MaxValue
		{
			get { return this.accumMaxValue; }
		}
		public int[] ValueGraph
		{
			get { return this.valueGraph; }
		}
		public int ValueGraphCursor
		{
			get { return this.valueGraphCursor; }
		}


		public void Add(int value)
		{
			this.value += value;
			this.used = true;
		}
		public void Set(int value)
		{
			this.value = value;
			this.used = true;
		}
		public override void Reset()
		{
			this.lastUsed = this.used;
			this.used = false;

			this.lastValue = this.value;
			this.value = 0;
		}
		
		public override void GetReportData(out ProfileReportCounterData data, ProfileReportOptions options)
		{
			data = new ProfileReportCounterData();
			data.Severity = 0.5f;
			
			if ((options & ProfileReportOptions.LastValue) != ProfileReportOptions.None)
				data.LastValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", this.lastValue);

			if (this.IsSingleValue)
			{
				if ((options & ProfileReportOptions.AverageValue) != ProfileReportOptions.None)
					data.AverageValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", this.lastValue);
			}
			else
			{
				if ((options & ProfileReportOptions.AverageValue) != ProfileReportOptions.None)
					data.AverageValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", (int)Math.Round((double)this.accumValue / (double)this.sampleCount));
				if ((options & ProfileReportOptions.MinValue) != ProfileReportOptions.None)
					data.MinValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", this.accumMinValue);
				if ((options & ProfileReportOptions.MaxValue) != ProfileReportOptions.None)
					data.MaxValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", this.accumMaxValue);
				if ((options & ProfileReportOptions.SampleCount) != ProfileReportOptions.None)
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
			this.Reset();
		}
	}
}
