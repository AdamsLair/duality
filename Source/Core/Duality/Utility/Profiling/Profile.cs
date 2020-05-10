﻿using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;
using System;

using Duality.Resources;
using Duality.Drawing;
using Duality.IO;

namespace Duality
{
	/// <summary>
	/// This class houses several performance counters and performance measurement utility
	/// </summary>
	public static class Profile
	{
		private static Dictionary<string,ProfileCounter> counterMap = new Dictionary<string,ProfileCounter>();

		public static readonly TimeCounter TimeFrame;
		public static readonly TimeCounter TimeUpdate;
		public static readonly TimeCounter TimeUpdateScene;
		public static readonly TimeCounter TimeUpdateSceneComponents;
		public static readonly TimeCounter TimeUpdateCoroutines;
		public static readonly TimeCounter TimeUpdateAudio;
		public static readonly TimeCounter TimeUpdatePhysics;
		public static readonly TimeCounter TimeUpdatePhysicsContacts;
		public static readonly TimeCounter TimeUpdatePhysicsController;
		public static readonly TimeCounter TimeUpdatePhysicsContinous;
		public static readonly TimeCounter TimeUpdatePhysicsAddRemove;
		public static readonly TimeCounter TimeUpdatePhysicsSolve;
		public static readonly TimeCounter TimeRender;
		public static readonly TimeCounter TimeSwapBuffers;
		public static readonly TimeCounter TimeQueryVisibleRenderers;
		public static readonly TimeCounter TimeCollectDrawcalls;
		public static readonly TimeCounter TimeOptimizeDrawcalls;
		public static readonly TimeCounter TimeProcessDrawcalls;
		public static readonly TimeCounter TimeLog;
		public static readonly TimeCounter TimeVisualPicking;
		public static readonly TimeCounter TimeUnaccounted;

		public static readonly StatCounter StatNumPlaying2D;
		public static readonly StatCounter StatNumPlaying3D;
		public static readonly StatCounter StatNumDrawcalls;
		public static readonly StatCounter StatNumRawBatches;
		public static readonly StatCounter StatNumMergedBatches;
		public static readonly StatCounter StatNumOptimizedBatches;
		public static readonly StatCounter StatMemoryTotalUsage;
		public static readonly StatCounter StatMemoryGarbageCollect0;
		public static readonly StatCounter StatMemoryGarbageCollect1;
		public static readonly StatCounter StatMemoryGarbageCollect2;

		static Profile()
		{
			TimeFrame                   = RequestCounter<TimeCounter>(@"Duality\Frame");
			TimeUpdate                  = RequestCounter<TimeCounter>(@"Duality\Frame\Update");
			TimeUpdatePhysics           = RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics");
			TimeUpdatePhysicsContacts   = RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Contacts");
			TimeUpdatePhysicsController = RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Controller");
			TimeUpdatePhysicsContinous  = RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Continous");
			TimeUpdatePhysicsAddRemove  = RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\AddRemove");
			TimeUpdatePhysicsSolve      = RequestCounter<TimeCounter>(@"Duality\Frame\Update\Physics\Solve");
			TimeUpdateScene             = RequestCounter<TimeCounter>(@"Duality\Frame\Update\Scene");
			TimeUpdateSceneComponents   = RequestCounter<TimeCounter>(@"Duality\Frame\Update\Scene\All Components");
			TimeUpdateCoroutines        = RequestCounter<TimeCounter>(@"Duality\Frame\Update\Scene\Coroutines");
			TimeUpdateAudio             = RequestCounter<TimeCounter>(@"Duality\Frame\Update\Audio");
			TimeRender                  = RequestCounter<TimeCounter>(@"Duality\Frame\Render");
			TimeSwapBuffers             = RequestCounter<TimeCounter>(@"Duality\Frame\Render\SwapBuffers");
			TimeQueryVisibleRenderers   = RequestCounter<TimeCounter>(@"Duality\Frame\Render\QueryVisibleRenderers");
			TimeCollectDrawcalls        = RequestCounter<TimeCounter>(@"Duality\Frame\Render\CollectDrawcalls");
			TimeOptimizeDrawcalls       = RequestCounter<TimeCounter>(@"Duality\Frame\Render\OptimizeDrawcalls");
			TimeProcessDrawcalls        = RequestCounter<TimeCounter>(@"Duality\Frame\Render\ProcessDrawcalls");
			TimeLog                     = RequestCounter<TimeCounter>(@"Duality\Frame\Log");
			TimeVisualPicking           = RequestCounter<TimeCounter>(@"Duality\Frame\VisualPicking");
			TimeUnaccounted             = RequestCounter<TimeCounter>(@"Duality\Frame\Unaccounted");

			StatNumPlaying2D            = RequestCounter<StatCounter>(@"Duality\Stats\Audio\NumPlaying2D");
			StatNumPlaying3D            = RequestCounter<StatCounter>(@"Duality\Stats\Audio\NumPlaying3D");
			StatNumDrawcalls            = RequestCounter<StatCounter>(@"Duality\Stats\Render\NumDrawcalls");
			StatNumRawBatches           = RequestCounter<StatCounter>(@"Duality\Stats\Render\NumRawBatches");
			StatNumMergedBatches        = RequestCounter<StatCounter>(@"Duality\Stats\Render\NumMergedBatches");
			StatNumOptimizedBatches     = RequestCounter<StatCounter>(@"Duality\Stats\Render\NumOptimizedBatches");
			StatMemoryTotalUsage        = RequestCounter<StatCounter>(@"Duality\Stats\Memory\TotalUsage");
			StatMemoryGarbageCollect0   = RequestCounter<StatCounter>(@"Duality\Stats\Memory\GarbageCollect0");
			StatMemoryGarbageCollect1   = RequestCounter<StatCounter>(@"Duality\Stats\Memory\GarbageCollect1");
			StatMemoryGarbageCollect2   = RequestCounter<StatCounter>(@"Duality\Stats\Memory\GarbageCollect2");

			StatMemoryGarbageCollect0.IsSingleValue = true;
			StatMemoryGarbageCollect1.IsSingleValue = true;
			StatMemoryGarbageCollect2.IsSingleValue = true;
		}

		/// <summary>
		/// Completely resets all <see cref="ProfileCounter"/> instances, discarding
		/// all data that has been collected so far and starting over.
		/// </summary>
		public static void ResetCounters()
		{
			foreach (var pair in counterMap)
			{
				pair.Value.ResetAll();
			}
		}

		/// <summary>
		/// Returns an existing <see cref="ProfileCounter"/> with the specified name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">The <see cref="ProfileCounter"/> name to use for this measurement. For nested measurements, use path strings, e.g. "ParentCounter\ChildCounter"</param>
		public static T GetCounter<T>(string name) where T : ProfileCounter
		{
			if (name == null) return null;

			ProfileCounter c;
			if (!counterMap.TryGetValue(name, out c)) return null;

			T cc = c as T;
			if (cc == null) throw new InvalidOperationException(string.Format("The specified performance counter '{0}' is not a {1}.", name, LogFormat.Type(typeof(T))));
			return cc;
		}
		/// <summary>
		/// Returns an existing <see cref="ProfileCounter"/> with the specified name, or creates one if none is found.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">The <see cref="ProfileCounter"/> name to use for this measurement. For nested measurements, use path strings, e.g. "ParentCounter\ChildCounter"</param>
		public static T RequestCounter<T>(string name) where T : ProfileCounter, new()
		{
			if (name == null) return null;

			T c = GetCounter<T>(name);
			if (c != null) return c;
			
			c = new T();
			c.FullName = name;
			counterMap[name] = c;
			return c;
		}
		/// <summary>
		/// Enumerates all <see cref="ProfileCounter"/> objects that have been actively used this frame.
		/// </summary>
		public static IEnumerable<ProfileCounter> GetUsedCounters()
		{
			return counterMap.Values.Where(p => p.WasUsed);
		}

		/// <summary>
		/// Begins time measurement using a new or existing <see cref="ProfileCounter"/> with the specified name.
		/// </summary>
		/// <param name="counter">The <see cref="ProfileCounter"/> name to use for this measurement. For nested measurements, use path strings, e.g. "ParentCounter\ChildCounter"</param>
		public static TimeCounter BeginMeasure(string counter)
		{
			TimeCounter tc = RequestCounter<TimeCounter>(counter);
			tc.BeginMeasure();
			return tc;
		}
		/// <summary>
		/// Ends time measurement using an existing <see cref="ProfileCounter"/> with the specified name.
		/// </summary>
		/// <param name="counter">The <see cref="ProfileCounter"/> name to use for this measurement. For nested measurements, use path strings, e.g. "ParentCounter\ChildCounter"</param>
		public static void EndMeasure(string counter)
		{
			TimeCounter tc = RequestCounter<TimeCounter>(counter);
			tc.EndMeasure();
		}
		/// <summary>
		/// Queries this frames time measurement value from an existing <see cref="ProfileCounter"/> with the specified name.
		/// </summary>
		/// <param name="counter">The <see cref="ProfileCounter"/> name to use for this measurement. For nested measurements, use path strings, e.g. "ParentCounter\ChildCounter"</param>
		public static float GetMeasure(string counter)
		{
			TimeCounter tc = GetCounter<TimeCounter>(counter);
			if (tc != null)
				return tc.LastValue;
			else
				return 0.0f;
		}

		/// <summary>
		/// Accumulates a statistical information value to a new or existing <see cref="ProfileCounter"/> with the specified name.
		/// </summary>
		/// <param name="counter">The <see cref="ProfileCounter"/> name to use for this measurement. For nested measurements, use path strings, e.g. "ParentCounter\ChildCounter"</param>
		/// <param name="value"></param>
		public static void AddToStat(string counter, int value)
		{
			StatCounter sc = RequestCounter<StatCounter>(counter);
			sc.Add(value);
		}
		/// <summary>
		/// Queries a statistical information value from an existing <see cref="ProfileCounter"/> with the specified name.
		/// </summary>
		/// <param name="counter">The <see cref="ProfileCounter"/> name to use for this measurement. For nested measurements, use path strings, e.g. "ParentCounter\ChildCounter"</param>
		public static int GetStat(string counter)
		{
			StatCounter sc = RequestCounter<StatCounter>(counter);
			if (sc != null)
				return sc.LastValue;
			else
				return 0;
		}

		/// <summary>
		/// Saves a text report of the current profiling data to the specified file.
		/// </summary>
		/// <param name="filePath"></param>
		public static void SaveTextReport(string filePath)
		{
			using (Stream str = FileOp.Create(filePath))
			{
				SaveTextReport(str);
			}
		}
		/// <summary>
		/// Saves a text report of the current profiling data to the specified stream.
		/// </summary>
		public static void SaveTextReport(Stream stream)
		{
			string report = GetTextReport(counterMap.Values.Where(c => c.HasData), 
				ProfileReportOptions.GroupHeader | 
				ProfileReportOptions.Header | 
				ProfileReportOptions.AverageValue |
				ProfileReportOptions.MaxValue | 
				ProfileReportOptions.MinValue |
				ProfileReportOptions.SampleCount);
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.Write(report);
			}
		}
		/// <summary>
		/// Creates a text report of the current profiling data and returns it as string.
		/// </summary>
		public static string GetTextReport(IEnumerable<ProfileCounter> reportCounters, ProfileReportOptions options = ProfileReportOptions.LastValue)
		{
			bool omitMinor = (options & ProfileReportOptions.OmitMinorValues) != ProfileReportOptions.None;

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
					
				if (options.HasFlag(ProfileReportOptions.GroupHeader))
				{
					reportBuilder.Append(options.HasFlag(ProfileReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);
					reportBuilder.AppendLine(("[ " + pair.Key.Name + " ]").PadLeft(35, '-').PadRight(50,'-'));
					reportBuilder.Append(options.HasFlag(ProfileReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);
				}
				else if (reportBuilder.Length > 0)
				{
					reportBuilder.Append(options.HasFlag(ProfileReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);
				}

				if (options.HasFlag(ProfileReportOptions.Header))
				{
					if (options.HasFlag(ProfileReportOptions.FormattedText))
						reportBuilder.Append(FormattedText.FormatColor(ColorRgba.White.WithAlpha(0.5f)));

					reportBuilder.Append("Name");
					reportBuilder.Append(' ', 1 + Math.Max((1 + maxNameLen) - "Name".Length, 0));

					if (options.HasFlag(ProfileReportOptions.LastValue))
						reportBuilder.Append("   Last Value ");
					if (options.HasFlag(ProfileReportOptions.AverageValue))
						reportBuilder.Append("   Avg. Value ");
					if (options.HasFlag(ProfileReportOptions.MinValue))
						reportBuilder.Append("   Min. Value ");
					if (options.HasFlag(ProfileReportOptions.MaxValue))
						reportBuilder.Append("   Max. Value ");
					if (options.HasFlag(ProfileReportOptions.SampleCount))
						reportBuilder.Append("        Samples ");

					reportBuilder.Append(options.HasFlag(ProfileReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);

					if (options.HasFlag(ProfileReportOptions.FormattedText))
						reportBuilder.Append(FormattedText.FormatColor(ColorRgba.White));
				}
				Stack<ProfileCounter> appendStack = new Stack<ProfileCounter>(rootCounters.Reverse());
				while (appendStack.Count > 0)
				{
					ProfileCounter current = appendStack.Pop();

					ProfileReportCounterData data;
					current.GetReportData(out data);
					if (omitMinor && data.Severity <= 0.005f)
						continue;
					
					if (options.HasFlag(ProfileReportOptions.FormattedText))
					{
						float severity = data.Severity;
						ColorRgba lineColor = severity >= 0.5f ? 
							ColorRgba.Lerp(ColorRgba.White, ColorRgba.Red, 2.0f * (severity - 0.5f)) :
							ColorRgba.Lerp(ColorRgba.TransparentWhite, ColorRgba.White, 0.1f + 0.9f * (2.0f * severity));
						reportBuilder.Append(FormattedText.FormatColor(lineColor));
					}
					reportBuilder.Append(' ', current.ParentDepth * 2);
					reportBuilder.Append(current.DisplayName);
					reportBuilder.Append(':');
					reportBuilder.Append(' ', 1 + Math.Max((1 + maxNameLen) - (current.ParentDepth * 2 + current.DisplayName.Length + 1), 0));

					if (options.HasFlag(ProfileReportOptions.LastValue))
					{
						string valStr = data.LastValue ?? "-";
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ProfileReportOptions.AverageValue))
					{
						string valStr = data.AverageValue ?? "-";
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ProfileReportOptions.MinValue))
					{
						string valStr = data.MinValue ?? "-";
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ProfileReportOptions.MaxValue))
					{
						string valStr = data.MaxValue ?? "-";
						reportBuilder.Append(' ', Math.Max(13 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					if (options.HasFlag(ProfileReportOptions.SampleCount))
					{
						string valStr = data.SampleCount ?? "-";
						reportBuilder.Append(' ', Math.Max(15 - valStr.Length, 0));
						reportBuilder.Append(valStr);
						reportBuilder.Append(' ');
					}
					reportBuilder.Append(options.HasFlag(ProfileReportOptions.FormattedText) ? FormattedText.FormatNewline : Environment.NewLine);

					IEnumerable<ProfileCounter> childCounters = counters.Where(c => c.Parent == current);
					foreach (ProfileCounter child in childCounters.Reverse())
						appendStack.Push(child);
				}
				if (options.HasFlag(ProfileReportOptions.FormattedText))
				{
					reportBuilder.Append(FormattedText.FormatColor(ColorRgba.White));
				}
			}

			return reportBuilder.ToString();;
		}

		internal static void FrameTick()
		{
			// Calculate unaccounted for frame time
			TimeUnaccounted.Add(TimeFrame.Value
				- TimeUpdate.Value
				- TimeRender.Value
				- TimeLog.Value
				- TimeVisualPicking.Value);

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
