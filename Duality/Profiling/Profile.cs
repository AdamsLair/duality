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

namespace Duality.Profiling
{
	/// <summary>
	/// This class houses several performance counters and performance measurement utility
	/// </summary>
	public static class Profile
	{
		private	static	Dictionary<string,ProfileCounter>	counterMap			= new Dictionary<string,ProfileCounter>();
		private static	FormattedText						textReport			= null;
		private static  VertexC1P3T2[]						textReportIconVert	= null;
		private static  VertexC1P3T2[][]					textReportTextVert	= null;
		private	static	TimeSpan							textReportLast		= TimeSpan.Zero;

		public static readonly TimeCounter	TimeFrame;
		public static readonly TimeCounter	TimeUpdate;
		public static readonly TimeCounter	TimeUpdateScene;
		public static readonly TimeCounter	TimeUpdateAudio;
		public static readonly TimeCounter	TimeUpdatePhysics;
		public static readonly TimeCounter	TimeUpdatePhysicsContacts;
		public static readonly TimeCounter	TimeUpdatePhysicsController;
		public static readonly TimeCounter	TimeUpdatePhysicsContinous;
		public static readonly TimeCounter	TimeUpdatePhysicsAddRemove;
		public static readonly TimeCounter	TimeUpdatePhysicsSolve;
		public static readonly TimeCounter	TimeRender;
		public static readonly TimeCounter	TimeSwapBuffers;
		public static readonly TimeCounter	TimeCollectDrawcalls;
		public static readonly TimeCounter	TimeOptimizeDrawcalls;
		public static readonly TimeCounter	TimeProcessDrawcalls;
		public static readonly TimeCounter	TimePostProcessing;
		public static readonly TimeCounter	TimeLog;
		public static readonly TimeCounter	TimeVisualPicking;
		public static readonly StatCounter	StatNumPlaying2D;
		public static readonly StatCounter	StatNumPlaying3D;
		public static readonly StatCounter	StatNumDrawcalls;
		public static readonly StatCounter	StatNumRawBatches;
		public static readonly StatCounter	StatNumMergedBatches;
		public static readonly StatCounter	StatNumOptimizedBatches;
		public static readonly StatCounter	StatMemoryTotalUsage;
		public static readonly StatCounter	StatMemoryGarbageCollect0;
		public static readonly StatCounter	StatMemoryGarbageCollect1;
		public static readonly StatCounter	StatMemoryGarbageCollect2;

		static Profile()
		{
			TimeUpdatePhysics			= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics");
			TimeFrame					= RequestCounter<TimeCounter>(@"Duality\Frame");
			TimeUpdatePhysicsContacts	= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Contacts");
			TimeUpdate					= RequestCounter<TimeCounter>(@"Duality\Frame\Update");
			TimeUpdatePhysicsController	= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Controller");
			TimeUpdateScene				= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Scene");
			TimeUpdatePhysicsContinous	= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Continous");
			TimeUpdateAudio				= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Audio");
			TimeUpdatePhysicsAddRemove	= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\AddRemove");
			TimeUpdatePhysicsSolve		= RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Solve");
			TimeRender					= RequestCounter<TimeCounter>(@"Duality\Frame\Render");
			TimeSwapBuffers				= RequestCounter<TimeCounter>(@"Duality\Frame\Render\SwapBuffers");
			TimeCollectDrawcalls		= RequestCounter<TimeCounter>(@"Duality\Frame\Render\CollectDrawcalls");
			TimeOptimizeDrawcalls		= RequestCounter<TimeCounter>(@"Duality\Frame\Render\OptimizeDrawcalls");
			TimeProcessDrawcalls		= RequestCounter<TimeCounter>(@"Duality\Frame\Render\ProcessDrawcalls");
			TimePostProcessing			= RequestCounter<TimeCounter>(@"Duality\Frame\Render\PostProcessing");
			TimeLog						= RequestCounter<TimeCounter>(@"Duality\Frame\Log");
			TimeVisualPicking			= RequestCounter<TimeCounter>(@"Duality\VisualPicking");
			StatNumPlaying2D			= RequestCounter<StatCounter>(@"Duality\Stats\Audio\NumPlaying2D");
			StatNumPlaying3D			= RequestCounter<StatCounter>(@"Duality\Stats\Audio\NumPlaying3D");
			StatNumDrawcalls			= RequestCounter<StatCounter>(@"Duality\Stats\Render\NumDrawcalls");
			StatNumRawBatches			= RequestCounter<StatCounter>(@"Duality\Stats\Render\NumRawBatches");
			StatNumMergedBatches		= RequestCounter<StatCounter>(@"Duality\Stats\Render\NumMergedBatches");
			StatNumOptimizedBatches		= RequestCounter<StatCounter>(@"Duality\Stats\Render\NumOptimizedBatches");
			StatMemoryTotalUsage		= RequestCounter<StatCounter>(@"Duality\Stats\Memory\TotalUsage");
			StatMemoryGarbageCollect0	= RequestCounter<StatCounter>(@"Duality\Stats\Memory\GarbageCollect0");
			StatMemoryGarbageCollect1	= RequestCounter<StatCounter>(@"Duality\Stats\Memory\GarbageCollect1");
			StatMemoryGarbageCollect2	= RequestCounter<StatCounter>(@"Duality\Stats\Memory\GarbageCollect2");

			StatMemoryGarbageCollect0.IsSingleValue = true;
			StatMemoryGarbageCollect1.IsSingleValue = true;
			StatMemoryGarbageCollect2.IsSingleValue = true;
		}

		public static T GetCounter<T>(string name) where T : ProfileCounter
		{
			if (name == null) return null;

			ProfileCounter c;
			if (!counterMap.TryGetValue(name, out c)) return null;

			T cc = c as T;
			if (cc == null) throw new InvalidOperationException(string.Format("The specified performance counter '{0}' is not a {1}.", name, Log.Type(typeof(T))));
			return cc;
		}
		public static T RequestCounter<T>(string name) where T : ProfileCounter, new()
		{
			if (name == null) return null;

			T c = GetCounter<T>(name);
			if (c != null) return c;
			
			c = new T();
			c.Name = name;
			counterMap[name] = c;
			return c;
		}
		public static IEnumerable<ProfileCounter> GetUsedCounters()
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

		public static void DrawTextReport(Canvas canvas, IEnumerable<ProfileCounter> counters = null, float x = 10.0f, float y = 10.0f, float z = 0.0f, bool background = true, ReportOptions options = ReportOptions.LastValue | ReportOptions.FormattedText)
		{
			BeginMeasure(@"DrawTextReport");

			if (counters == null) counters = GetUsedCounters();
			if (textReport == null || (Time.MainTimer - textReportLast).TotalMilliseconds > 250)
			{
				string report = GetTextReport(counters, options);
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
				ReportOptions.SampleCount);
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.Write(report);
			}
		}
		public static string GetTextReport(IEnumerable<ProfileCounter> reportCounters, ReportOptions options = ReportOptions.LastValue)
		{
			// Group Counters by Type
			Dictionary<Type,List<ProfileCounter>> countersByType = new Dictionary<Type,List<ProfileCounter>>();
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
				IEnumerable<ProfileCounter> counters = pair.Value;
				int minDepth = counters.Min(c => c.ParentDepth);
				IEnumerable<ProfileCounter> rootCounters = counters.Where(c => c.ParentDepth == minDepth);

				int maxNameLen	= counters.Max(c => c.DisplayName.Length + c.ParentDepth * 2);
					
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

					reportBuilder.Append(options.HasFlag(ReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);
				}
				Stack<ProfileCounter> appendStack = new Stack<ProfileCounter>(rootCounters.Reverse());
				while (appendStack.Count > 0)
				{
					ProfileCounter current = appendStack.Pop();

					ReportCounterData data;
					current.GetReportData(out data, options);
					
					if (options.HasFlag(ReportOptions.FormattedText))
					{
						float severity = data.Severity;
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
						string valStr = data.LastValue ?? "-";
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ReportOptions.AverageValue))
					{
						string valStr = data.AverageValue ?? "-";
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ReportOptions.MinValue))
					{
						string valStr = data.MinValue ?? "-";
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ReportOptions.MaxValue))
					{
						string valStr = data.MaxValue ?? "-";
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ReportOptions.SampleCount))
					{
						string valStr = data.SampleCount ?? "-";
						reportBuilder.Append(' ', Math.Max(15 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					reportBuilder.Append(options.HasFlag(ReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);

					IEnumerable<ProfileCounter> childCounters = counters.Where(c => c.Parent == current);
					foreach (ProfileCounter child in childCounters.Reverse())
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
			// Collect more globally available data
			StatMemoryTotalUsage.Add((int)(GC.GetTotalMemory(false) / 1024L));
			StatMemoryGarbageCollect0.Add(GC.CollectionCount(0));
			StatMemoryGarbageCollect1.Add(GC.CollectionCount(1));
			StatMemoryGarbageCollect2.Add(GC.CollectionCount(2));

			// Run frametick counter operations
			foreach (ProfileCounter c in counterMap.Values.ToArray())
			{
				c.TickFrame();
			}
		}
	}
}
