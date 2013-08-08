using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;
using System;

using OpenTK;

using Duality.Resources;
using Duality.ColorFormat;
using Duality.VertexFormat;

namespace Duality
{
	/// <summary>
	/// This class houses several performance counters and performance measurement utility
	/// </summary>
	public static class Performance
	{
		[Flags]
		public enum ReportOptions
		{
			None			= 0x00,

			LastValue		= 0x01,
			AverageValue	= 0x02,
			SampleCount		= 0x04,
			MinValue		= 0x08,
			MaxValue		= 0x10,
			TotalValue		= 0x20,

			GroupHeader		= 0x1000,
			Header			= 0x2000,
			FormattedText	= 0x4000
		}
		public abstract class Counter
		{
			// Management
			private		string		path		= null;
			private		string		name		= null;
			private		string		parentName	= null;
			private		Counter		parent		= null;
			// Measurement
			protected	bool		used		= true;
			protected	bool		lastUsed	= true;
			protected	int			sampleCount	= 0;


			public string Name
			{
				get { return this.name; }
				private set
				{
					this.path = value;
					this.name = Path.GetFileName(value);
					this.parentName = Path.GetDirectoryName(value);
				}
			}
			public string FullName
			{
				get { return this.path; }
			}
			public string DisplayName
			{
				get { return this.Parent == null ? this.FullName : this.Name; }
			}
			public Counter Parent
			{
				get
				{
					if (this.parent == null && !string.IsNullOrEmpty(this.parentName))
						Performance.counterMap.TryGetValue(this.parentName, out this.parent);
					return this.parent;
				}
			}
			public int ParentDepth
			{
				get
				{
					return 
						this.Parent == null ? 
						0 : 
						1 + this.Parent.ParentDepth;
				}
			}
			public bool WasUsed
			{
				get { return this.lastUsed; }
			}
			public int SampleCount
			{
				get { return this.sampleCount; }
			}

			public virtual float Severity
			{
				get { return 0.5f; }
			}
			protected internal abstract string DisplayLastValue { get; }
			protected internal abstract string DisplayAverageValue { get; }
			protected internal abstract string DisplayMinValue { get; }
			protected internal abstract string DisplayMaxValue { get; }
			protected internal abstract string DisplayTotalValue { get; }


			public abstract void Reset();

			protected internal virtual void OnFrameTick() {}

			public static T Create<T>(string name) where T : Counter, new()
			{
				T counter = new T();
				counter.Name = name;
				return counter;
			}
		}
		public class TimeCounter : Counter
		{
			// Measurement
			private	Stopwatch	watch		= new Stopwatch();
			private	float		value		= 0.0f;
			private	float		lastValue	= 0.0f;
			private	double		accumValue		= 0.0d;
			private	float		accumMaxValue	= float.MinValue;
			private	float		accumMinValue	= float.MaxValue;


			public float LastValue
			{
				get { return this.lastValue; }
			}
			
			public override float Severity
			{
				get { return MathF.Clamp(this.lastValue / Time.MsPFMult, 0.0f, 1.0f); }
			}
			protected internal override string DisplayLastValue
			{
				get { return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F}", this.lastValue); }
			}
			protected internal override string DisplayAverageValue
			{
				get { return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F}", (float)(this.accumValue / (double)this.sampleCount)); }
			}
			protected internal override string DisplayMinValue
			{
				get { return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F}", this.accumMinValue); }
			}
			protected internal override string DisplayMaxValue
			{
				get { return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F}", this.accumMaxValue); }
			}
			protected internal override string DisplayTotalValue
			{
				get { return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F}", this.accumValue); }
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
			public override void Reset()
			{
				this.lastUsed = this.used;
				this.used = false;

				this.lastValue = this.value;
				this.value = 0.0f;
			}

			protected internal override void OnFrameTick()
			{
				if (this.used)
				{
					this.sampleCount++;
					this.accumMaxValue = MathF.Max(this.value, this.accumMaxValue);
					this.accumMinValue = MathF.Min(this.value, this.accumMinValue);
					this.accumValue += this.value;
				}
				this.Reset();
			}
		}
		public class StatCounter : Counter
		{
			// Measurement
			private	int		value		= 0;
			private	int		lastValue	= 0;
			// Report data
			private	long	accumValue		= 0;
			private	int		accumMaxValue	= int.MinValue;
			private	int		accumMinValue	= int.MaxValue;
			

			public int LastValue
			{
				get { return this.lastValue; }
			}

			protected internal override string DisplayLastValue
			{
				get { return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", this.lastValue); }
			}
			protected internal override string DisplayAverageValue
			{
				get { return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", (int)Math.Round((double)this.accumValue / (double)this.sampleCount)); }
			}
			protected internal override string DisplayMinValue
			{
				get { return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", this.accumMinValue); }
			}
			protected internal override string DisplayMaxValue
			{
				get { return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", this.accumMaxValue); }
			}
			protected internal override string DisplayTotalValue
			{
				get { return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", this.accumValue); }
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

			protected internal override void OnFrameTick()
			{
				if (this.used)
				{
					this.sampleCount++;
					this.accumMaxValue = MathF.Max(this.value, this.accumMaxValue);
					this.accumMinValue = MathF.Min(this.value, this.accumMinValue);
					this.accumValue += this.value;
				}
				this.Reset();
			}
		}


		private	static	Dictionary<string,Counter>	counterMap			= new Dictionary<string,Counter>();
		private static	FormattedText				textReport			= null;
		private static  VertexC1P3T2[]				textReportIconVert	= null;
		private static  VertexC1P3T2[][]			textReportTextVert	= null;
		private	static	TimeSpan					textReportLast		= TimeSpan.Zero;
		public static readonly TimeCounter	TimeUpdatePhysics			= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics");
		public static readonly TimeCounter	TimeFrame					= RequestCounter<TimeCounter>(@"Duality\Frame");
		public static readonly TimeCounter	TimeUpdatePhysicsContacts	= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Contacts");
		public static readonly TimeCounter	TimeUpdate					= RequestCounter<TimeCounter>(@"Duality\Frame\Update");
		public static readonly TimeCounter	TimeUpdatePhysicsController	= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Controller");
		public static readonly TimeCounter	TimeUpdateScene				= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Scene");
		public static readonly TimeCounter	TimeUpdatePhysicsContinous	= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Continous");
		public static readonly TimeCounter	TimeUpdateAudio				= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Audio");
		public static readonly TimeCounter	TimeUpdatePhysicsAddRemove	= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\AddRemove");
		public static readonly TimeCounter	TimeUpdatePhysicsSolve		= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Solve");
		public static readonly TimeCounter	TimeRender					= RequestCounter<TimeCounter>(@"Duality\Frame\Render");
		public static readonly TimeCounter	TimeSwapBuffers				= RequestCounter<TimeCounter>(@"Duality\Frame\Render\SwapBuffers");
		public static readonly TimeCounter	TimeCollectDrawcalls		= RequestCounter<TimeCounter>(@"Duality\Frame\Render\CollectDrawcalls");
		public static readonly TimeCounter	TimeOptimizeDrawcalls		= RequestCounter<TimeCounter>(@"Duality\Frame\Render\OptimizeDrawcalls");
		public static readonly TimeCounter	TimeProcessDrawcalls		= RequestCounter<TimeCounter>(@"Duality\Frame\Render\ProcessDrawcalls");
		public static readonly TimeCounter	TimePostProcessing			= RequestCounter<TimeCounter>(@"Duality\Frame\Render\PostProcessing");
		public static readonly TimeCounter	TimeLog						= RequestCounter<TimeCounter>(@"Duality\Frame\Log");
		public static readonly TimeCounter	TimeVisualPicking			= RequestCounter<TimeCounter>(@"Duality\VisualPicking");
		public static readonly StatCounter	StatNumPlaying2D			= RequestCounter<StatCounter>(@"Duality\Stats\Audio\NumPlaying2D");
		public static readonly StatCounter	StatNumPlaying3D			= RequestCounter<StatCounter>(@"Duality\Stats\Audio\NumPlaying3D");
		public static readonly StatCounter	StatNumDrawcalls			= RequestCounter<StatCounter>(@"Duality\Stats\Render\NumDrawcalls");
		public static readonly StatCounter	StatNumRawBatches			= RequestCounter<StatCounter>(@"Duality\Stats\Render\NumRawBatches");
		public static readonly StatCounter	StatNumMergedBatches		= RequestCounter<StatCounter>(@"Duality\Stats\Render\NumMergedBatches");
		public static readonly StatCounter	StatNumOptimizedBatches		= RequestCounter<StatCounter>(@"Duality\Stats\Render\NumOptimizedBatches");


		public static T GetCounter<T>(string name) where T : Counter
		{
			Counter c;
			if (!counterMap.TryGetValue(name, out c)) return null;
			T cc = c as T;
			if (cc == null) throw new InvalidOperationException(string.Format("The specified performance counter '{0}' is not a {1}.", name, Log.Type(typeof(T))));
			return cc;
		}
		public static T RequestCounter<T>(string name) where T : Counter, new()
		{
			T c = GetCounter<T>(name);
			if (c != null) return c;
			
			c = Counter.Create<T>(name);
			counterMap[name] = c;
			return c;
		}
		public static IEnumerable<Counter> GetUsedCounters()
		{
			return counterMap.Values.Where(p => p.WasUsed);
		}

		public static TimeCounter BeginMeasure(string counter)
		{
			TimeCounter tc = RequestCounter<TimeCounter>(counter);
			tc.BeginMeasure();
			return tc;
		}
		public static void EndMeasure(string counter)
		{
			TimeCounter tc = RequestCounter<TimeCounter>(counter);
			tc.EndMeasure();
		}
		public static float GetMeasure(string counter)
		{
			TimeCounter tc = GetCounter<TimeCounter>(counter);
			if (tc != null)
				return tc.LastValue;
			else
				return 0.0f;
		}

		public static void AddToStat(string counter, int value)
		{
			StatCounter sc = RequestCounter<StatCounter>(counter);
			sc.Add(value);
		}
		public static int GetStat(string counter)
		{
			StatCounter sc = RequestCounter<StatCounter>(counter);
			if (sc != null)
				return sc.LastValue;
			else
				return 0;
		}

		public static void DrawTextReport(Canvas canvas, float x = 10.0f, float y = 10.0f, float z = 0.0f, bool background = true, ReportOptions options = ReportOptions.LastValue | ReportOptions.FormattedText)
		{
			BeginMeasure(@"DrawTextReport");
			if (textReport == null || (Time.MainTimer - textReportLast).TotalMilliseconds > 250)
			{
				string report = GetTextReport(GetUsedCounters(), options);
				if (textReport == null)
				{
					textReport = new FormattedText(report);
					textReport.Fonts = new [] { canvas.CurrentState.TextFont };
				}
				else
				{
					textReport.SourceText = report;
				}
				textReportLast = Time.MainTimer;
			}

			canvas.PushState();
			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, ColorRgba.White, null));
			if (background) canvas.DrawTextBackground(textReport, x, y, z);
			canvas.DrawText(textReport, ref textReportTextVert, ref textReportIconVert, x, y, z);
			canvas.PopState();

			EndMeasure(@"DrawTextReport");
		}
		public static void SaveTextReport(string filePath)
		{
			using (FileStream str = File.Open(filePath, FileMode.Create))
			{
				SaveTextReport(str);
			}
		}
		public static void SaveTextReport(Stream stream)
		{
			string report = GetTextReport(counterMap.Values, 
				ReportOptions.GroupHeader | 
				ReportOptions.Header | 
				ReportOptions.AverageValue |
				ReportOptions.MaxValue | 
				ReportOptions.MinValue |
				ReportOptions.SampleCount |
				ReportOptions.TotalValue);
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.Write(report);
			}
		}
		public static string GetTextReport(IEnumerable<Counter> reportCounters, ReportOptions options = ReportOptions.LastValue)
		{
			// Group Counters by Type
			Dictionary<Type,List<Counter>> countersByType = new Dictionary<Type,List<Counter>>();
			Type[] existingTypes = reportCounters.Select(c => c.GetType()).Distinct().ToArray();
			foreach (Type type in existingTypes)
			{
				countersByType[type] = reportCounters.Where(c => c.GetType() == type).ToList();
			}

			// Prepare text building
			StringBuilder reportBuilder = new StringBuilder(countersByType.Count * 256);

			// Handle each group separately
			foreach (var pair in countersByType)
			{
				IEnumerable<Counter> counters = pair.Value;
				int minDepth = counters.Min(c => c.ParentDepth);
				IEnumerable<Counter> rootCounters = counters.Where(c => c.ParentDepth == minDepth);

				int maxNameLen	= counters.Max(c => c.DisplayName.Length + c.ParentDepth * 2);
				int maxSamples	= counters.Max(c => c.SampleCount);
					
				if (options.HasFlag(ReportOptions.GroupHeader))
				{
					reportBuilder.Append(options.HasFlag(ReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);
					reportBuilder.AppendLine(("[ " + pair.Key.Name + " ]").PadLeft(35, '-').PadRight(50,'-'));
					reportBuilder.Append(options.HasFlag(ReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);
				}
				else if (reportBuilder.Length > 0)
				{
					reportBuilder.Append(options.HasFlag(ReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);
				}

				if (options.HasFlag(ReportOptions.Header))
				{
					reportBuilder.Append("Name");
					reportBuilder.Append(' ', 1 + Math.Max((1 + maxNameLen) - "Name".Length, 0));

					if (options.HasFlag(ReportOptions.LastValue))
						reportBuilder.Append("   Last Value ");
					if (options.HasFlag(ReportOptions.AverageValue))
						reportBuilder.Append("   Avg. Value ");
					if (options.HasFlag(ReportOptions.MinValue))
						reportBuilder.Append("   Min. Value ");
					if (options.HasFlag(ReportOptions.MaxValue))
						reportBuilder.Append("   Max. Value ");
					if (options.HasFlag(ReportOptions.SampleCount))
						reportBuilder.Append("        Samples ");
					if (options.HasFlag(ReportOptions.TotalValue))
						reportBuilder.Append("    Total Value ");

					reportBuilder.Append(options.HasFlag(ReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);
				}
				Stack<Counter> appendStack = new Stack<Counter>(rootCounters.Reverse());
				while (appendStack.Count > 0)
				{
					Counter current = appendStack.Pop();
					
					if (options.HasFlag(ReportOptions.FormattedText))
					{
						float severity = current.Severity;
						ColorRgba lineColor = severity >= 0.5f ? 
							ColorRgba.Mix(ColorRgba.White, ColorRgba.Red, 2.0f * (severity - 0.5f)) :
							ColorRgba.Mix(ColorRgba.TransparentWhite, ColorRgba.White, 0.1f + 0.9f * (2.0f * severity));
						reportBuilder.Append(FormattedText.FormatColor(lineColor));
					}
					reportBuilder.Append(' ', current.ParentDepth * 2);
					reportBuilder.Append(current.DisplayName);
					reportBuilder.Append(':');
					reportBuilder.Append(' ', 1 + Math.Max((1 + maxNameLen) - (current.ParentDepth * 2 + current.DisplayName.Length + 1), 0));
					if (options.HasFlag(ReportOptions.LastValue))
					{
						string valStr = current.DisplayLastValue;
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ReportOptions.AverageValue))
					{
						string valStr = current.DisplayAverageValue;
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ReportOptions.MinValue))
					{
						string valStr = current.DisplayMinValue;
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ReportOptions.MaxValue))
					{
						string valStr = current.DisplayMaxValue;
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ReportOptions.SampleCount))
					{
						string valStr = string.Format("{0}", current.SampleCount);
						reportBuilder.Append(' ', Math.Max(15 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ReportOptions.TotalValue))
					{
						string valStr = current.DisplayTotalValue;
						reportBuilder.Append(' ', Math.Max(15 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					reportBuilder.Append(options.HasFlag(ReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);

					IEnumerable<Counter> childCounters = counters.Where(c => c.Parent == current);
					foreach (Counter child in childCounters.Reverse())
						appendStack.Push(child);
				}
				if (options.HasFlag(ReportOptions.FormattedText))
				{
					reportBuilder.Append(FormattedText.FormatColor(ColorRgba.White));
				}
			}

			return reportBuilder.ToString();;
		}

		internal static void FrameTick()
		{
			foreach (Counter c in counterMap.Values.ToArray())
				c.OnFrameTick();
		}
	}
}
