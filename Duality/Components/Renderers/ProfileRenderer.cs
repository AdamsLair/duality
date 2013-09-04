using System;
using System.Collections.Generic;
using System.Linq;

using Duality.ColorFormat;
using Duality.EditorHints;
using Duality.Profiling;
using Duality.VertexFormat;
using Duality.Resources;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Duality.Components.Renderers
{
	/// <summary>
	/// A diagnostic <see cref="Duality.Component"/> that displays current performance measurements and other profiling stats.
	/// </summary>
	[Serializable]
	public class ProfileRenderer : Component, ICmpRenderer, ICmpUpdatable
	{
		private class GraphCacheEntry
		{
			public bool WasUsed;
			public float[] GraphValues;
			public ColorRgba[] GraphColors;
			public VertexC1P3[] VertGraph;
			public VertexC1P3T2[] VertText;
		}

		private	bool			textReportPerf		= true;
		private	bool			textReportStat		= true;
		private	bool			drawGraphs			= true;
		private	List<string>	counterGraphs		= new List<string> { Profile.TimeFrame.FullName, Profile.TimeRender.FullName, Profile.TimeUpdate.FullName, Profile.StatMemoryTotalUsage.FullName };
		private	ReportOptions	textReportOptions	= ReportOptions.LastValue;
		private	int				updateInterval		= 250;
		private	Key				keyToggleTextPerf	= Key.F2;
		private	Key				keyToggleTextStat	= Key.F3;
		private	Key				keyToggleGraph		= Key.F4;

		[NonSerialized]	private	FormattedText		textReport			= null;
		[NonSerialized]	private	VertexC1P3T2[]		textReportIconVert	= null;
		[NonSerialized]	private	VertexC1P3T2[][]	textReportTextVert	= null;
		[NonSerialized]	private	TimeSpan			textReportLast		= TimeSpan.Zero;
		[NonSerialized] private Dictionary<string,GraphCacheEntry> graphCache = new Dictionary<string,GraphCacheEntry>();


		float ICmpRenderer.BoundRadius
		{
			get { return float.MaxValue; }
		}
		/// <summary>
		/// [GET / SET] Whether or not a text report of the current time profiling results is drawn.
		/// </summary>
		public bool DrawPerfTextReport
		{
			get { return this.textReportPerf; }
			set { this.textReportPerf = value; }
		}
		/// <summary>
		/// [GET / SET] Whether or not a text report of the current stat profiling results is drawn.
		/// </summary>
		public bool DrawStatTextReport
		{
			get { return this.textReportStat; }
			set { this.textReportStat = value; }
		}
		/// <summary>
		/// [GET / SET] Whether or not <see cref="CounterGraphs"/> are drawn.
		/// </summary>
		public bool DrawGraphs
		{
			get { return this.drawGraphs; }
			set { this.drawGraphs = value; }
		}
		/// <summary>
		/// [GET / SET] A key that can be used to toggle performance text reports.
		/// </summary>
		public Key KeyTogglePerfText
		{
			get { return this.keyToggleTextPerf; }
			set { this.keyToggleTextPerf = value; }
		}
		/// <summary>
		/// [GET / SET] A key that can be used to toggle profiling stat text reports.
		/// </summary>
		public Key KeyToggleStatText
		{
			get { return this.keyToggleTextStat; }
			set { this.keyToggleTextStat = value; }
		}
		/// <summary>
		/// [GET / SET] A key that can be used to toggle the display of realtime graphs.
		/// </summary>
		public Key KeyToggleGraphs
		{
			get { return this.keyToggleGraph; }
			set { this.keyToggleGraph = value; }
		}
		/// <summary>
		/// [GET / SET] The names of <see cref="Duality.Profiling.ProfileCounter"/> instances that should be drawn in graph form.
		/// </summary>
		public List<string> CounterGraphs
		{
			get { return this.counterGraphs; }
			set { this.counterGraphs = value ?? new List<string>(); }
		}
		/// <summary>
		/// [GET / SET] A bitmask that specifies which report features are used.
		/// </summary>
		public ReportOptions Options
		{
			get { return this.textReportOptions; }
			set { this.textReportOptions = value; }
		}
		/// <summary>
		/// [GET / SET] The time interval in milliseconds by which the report is updated.
		/// </summary>
		public int UpdateInterval
		{
			get { return this.updateInterval; }
			set { this.updateInterval = value; }
		}


		bool ICmpRenderer.IsVisible(IDrawDevice device)
		{
			return 
				DualityApp.ExecContext == DualityApp.ExecutionContext.Game &&
				(device.VisibilityMask & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None &&
				(device.VisibilityMask & VisibilityFlag.AllGroups) != VisibilityFlag.None;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			Profile.BeginMeasure(@"ProfileRenderer");
			Canvas canvas = new Canvas(device);
			canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, ColorRgba.White, null));
			
			bool anyTextReport = this.textReportPerf || this.textReportStat;
			bool anyGraph = this.drawGraphs && this.counterGraphs.Count > 0;

			// Determine geometry
			int areaWidth = (int)device.TargetSize.X - 20;
			if (anyGraph && anyTextReport)
				areaWidth = (areaWidth - 10) / 2;
			Rect textReportRect = new Rect(
				10, 
				10, 
				anyTextReport ? areaWidth : 0, 
				(int)device.TargetSize.Y - 20);
			Rect graphRect = new Rect(
				anyTextReport ? (textReportRect.MaximumX + 10) : 10, 
				10, 
				anyGraph ? areaWidth : 0, 
				(int)device.TargetSize.Y - 20);

			// Text Reports
			if (anyTextReport)
			{
				// Update Report
				IEnumerable<ProfileCounter> counters = Profile.GetUsedCounters();
				if (!this.textReportPerf) counters = counters.Where(c => !(c is TimeCounter));
				if (!this.textReportStat) counters = counters.Where(c => !(c is StatCounter));
				if (this.textReport == null || (Time.MainTimer - this.textReportLast).TotalMilliseconds > this.updateInterval)
				{
					string report = Profile.GetTextReport(counters, this.textReportOptions | ReportOptions.FormattedText);

					if (this.textReport == null)
					{
						this.textReport = new FormattedText();
						this.textReport.Fonts = new[] { Font.GenericMonospace8 };
					}
					this.textReport.MaxWidth = (int)textReportRect.W;
					this.textReport.SourceText = report;
					this.textReportLast = Time.MainTimer;
				}

				// Draw Report
				canvas.DrawTextBackground(textReport, textReportRect.X, textReportRect.Y);
				canvas.DrawText(textReport, ref textReportTextVert, ref textReportIconVert, textReportRect.X, textReportRect.Y);
			}

			// Counter Graphs
			if (anyGraph)
			{
				// Mark graph cache as unused
				foreach (GraphCacheEntry entry in this.graphCache.Values)
				{
					entry.WasUsed = false;
				}

				int space = 5;
				int graphY = (int)graphRect.Y;
				int graphH = MathF.Min((int)(graphRect.H / this.counterGraphs.Count) - space, (int)graphRect.W / 2);
				foreach (string counterName in this.counterGraphs)
				{
					ProfileCounter counter = Profile.GetCounter<ProfileCounter>(counterName);
					if (counter == null) return;

					// Create or retrieve graph cache entry
					GraphCacheEntry cache = null;
					if (!this.graphCache.TryGetValue(counterName, out cache))
					{
						cache = new GraphCacheEntry();
						cache.GraphValues = new float[ProfileCounter.ValueHistoryLen];
						cache.GraphColors = new ColorRgba[ProfileCounter.ValueHistoryLen];
						this.graphCache[counterName] = cache;
					}
					cache.WasUsed = true;

					float cursorRatio = 0.0f;
					if (counter is TimeCounter)
					{
						TimeCounter timeCounter = counter as TimeCounter;
						for (int i = 0; i < ProfileCounter.ValueHistoryLen; i++)
						{
							float factor = timeCounter.ValueGraph[i] / Time.MsPFMult;
							cache.GraphValues[i] = factor * 0.75f;
							cache.GraphColors[i] = ColorRgba.Mix(ColorRgba.White, ColorRgba.Red, factor);
						}
						canvas.CurrentState.ColorTint = ColorRgba.Black.WithAlpha(0.5f);
						canvas.FillRect(graphRect.X, graphY, graphRect.W, graphH);
						canvas.CurrentState.ColorTint = ColorRgba.White;
						canvas.DrawHorizontalGraph(cache.GraphValues, cache.GraphColors, ref cache.VertGraph, graphRect.X, graphY, graphRect.W, graphH);
						cursorRatio = (float)timeCounter.ValueGraphCursor / (float)ProfileCounter.ValueHistoryLen;
					}
					else if (counter is StatCounter)
					{
						StatCounter statCounter = counter as StatCounter;
						for (int i = 0; i < ProfileCounter.ValueHistoryLen; i++)
						{
							cache.GraphValues[i] = (float)(statCounter.ValueGraph[i] - statCounter.MinValue) / statCounter.MaxValue;
							cache.GraphColors[i] = ColorRgba.White;
						}
						canvas.CurrentState.ColorTint = ColorRgba.Black.WithAlpha(0.5f);
						canvas.FillRect(graphRect.X, graphY, graphRect.W, graphH);
						canvas.CurrentState.ColorTint = ColorRgba.White;
						canvas.DrawHorizontalGraph(cache.GraphValues, cache.GraphColors, ref cache.VertGraph, graphRect.X, graphY, graphRect.W, graphH);
						cursorRatio = (float)statCounter.ValueGraphCursor / (float)ProfileCounter.ValueHistoryLen;
					}
					
					canvas.DrawText(new string[] { counter.FullName }, ref cache.VertText, graphRect.X, graphY);
					canvas.DrawLine(graphRect.X + graphRect.W * cursorRatio, graphY, graphRect.X + graphRect.W * cursorRatio, graphY + graphH);

					graphY += graphH + space;
				}

				// Remove unused graph cache entries
				foreach (var pair in this.graphCache.ToArray())
				{
					if (!pair.Value.WasUsed)
					{
						pair.Value.GraphColors = null;
						pair.Value.GraphValues = null;
						pair.Value.VertGraph = null;
						pair.Value.VertText = null;
						this.graphCache.Remove(pair.Key);
					}
				}
			}

			Profile.EndMeasure(@"ProfileRenderer");
		}
		void ICmpUpdatable.OnUpdate()
		{
			if (DualityApp.Keyboard.KeyHit(this.keyToggleTextPerf))
				this.textReportPerf = !this.textReportPerf;
			if (DualityApp.Keyboard.KeyHit(this.keyToggleTextStat))
				this.textReportStat = !this.textReportStat;
			if (DualityApp.Keyboard.KeyHit(this.keyToggleGraph))
				this.drawGraphs = !this.drawGraphs;
		}

		protected override void OnCopyTo(Component target, Cloning.CloneProvider provider)
		{
			base.OnCopyTo(target, provider);
			ProfileRenderer t = target as ProfileRenderer;
			t.textReportStat		= this.textReportStat;
			t.textReportOptions		= this.textReportOptions;
			t.drawGraphs			= this.drawGraphs;
			t.counterGraphs			= this.counterGraphs;
			t.textReportOptions		= this.textReportOptions;
			t.updateInterval		= this.updateInterval;
			t.keyToggleGraph		= this.keyToggleGraph;
			t.keyToggleTextPerf		= this.keyToggleTextPerf;
			t.keyToggleTextStat		= this.keyToggleTextStat;
		}
	}
}
