using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using Duality.Cloning;
using Duality.Input;
using Duality.Properties;

namespace Duality.Components.Diagnostics
{
	/// <summary>
	/// A diagnostic <see cref="Duality.Component"/> that displays current performance measurements and other profiling stats.
	/// </summary>
	[EditorHintCategory(CoreResNames.CategoryDiagnostics)]
	[EditorHintImage(CoreResNames.ImageProfileRenderer)]
	public class ProfileRenderer : Component, ICmpRenderer, ICmpUpdatable
	{
		private class GraphCacheEntry
		{
			public bool WasUsed;
			public float[] GraphValues;
			public ColorRgba[] GraphColors;
			public VertexC1P3[] VertGraph;
			public VertexC1P3T2[][] VertText;
		}

		private bool                 textReportPerf    = true;
		private bool                 textReportStat    = true;
		private bool                 drawGraphs        = true;
		private List<string>         counterGraphs     = new List<string> { Profile.TimeFrame.FullName, Profile.TimeRender.FullName, Profile.TimeUpdate.FullName, Profile.StatMemoryTotalUsage.FullName };
		private ProfileReportOptions textReportOptions = ProfileReportOptions.LastValue | ProfileReportOptions.MaxValue | ProfileReportOptions.OmitMinorValues | ProfileReportOptions.Header;
		private int                  updateInterval    = 250;
		private Key                  keyToggleTextPerf = Key.F2;
		private Key                  keyToggleTextStat = Key.F3;
		private Key                  keyToggleGraph    = Key.F4;
		private Key                  keyResetCounters  = Key.F5;

		[DontSerialize] private Canvas           canvas             = new Canvas();
		[DontSerialize] private FormattedText    textReport         = null;
		[DontSerialize] private VertexC1P3T2[]   textReportIconVert = null;
		[DontSerialize] private VertexC1P3T2[][] textReportTextVert = null;
		[DontSerialize] private TimeSpan         textReportLast     = TimeSpan.Zero;
		[DontSerialize] private Dictionary<string,GraphCacheEntry> graphCache = new Dictionary<string,GraphCacheEntry>();


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
		/// [GET / SET] A key that can be used to reset all profile counter data back to zero.
		/// </summary>
		public Key KeyResetCounters
		{
			get { return this.keyResetCounters; }
			set { this.keyResetCounters = value; }
		}
		/// <summary>
		/// [GET / SET] The names of <see cref="Duality.ProfileCounter"/> instances that should be drawn in graph form.
		/// </summary>
		public List<string> CounterGraphs
		{
			get { return this.counterGraphs; }
			set { this.counterGraphs = value ?? new List<string>(); }
		}
		/// <summary>
		/// [GET / SET] A bitmask that specifies which report features are used.
		/// </summary>
		public ProfileReportOptions Options
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


		void ICmpRenderer.GetCullingInfo(out CullingInfo info)
		{
			info.Position = Vector3.Zero;
			info.Radius = float.MaxValue;
			info.Visibility = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Game) return;

			Profile.BeginMeasure(@"ProfileRenderer");
			this.canvas.Begin(device);
			this.canvas.State.SetMaterial(DrawTechnique.Alpha);
			
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
				anyTextReport ? (textReportRect.RightX + 10) : 10, 
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
					string report = Profile.GetTextReport(
						counters, 
						this.textReportOptions | 
						ProfileReportOptions.FormattedText);

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
				this.canvas.DrawText(textReport, ref textReportTextVert, ref textReportIconVert, textReportRect.X, textReportRect.Y, drawBackground: true);
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
						int cursorPos = timeCounter.ValueGraphCursor;
						for (int i = Math.Max(cursorPos - 1, 0); i <= Math.Min(cursorPos, ProfileCounter.ValueHistoryLen - 1); i++)
						{
							float factor = timeCounter.ValueGraph[i] / Time.MillisecondsPerFrame;
							cache.GraphValues[i] = factor * 0.75f;
							cache.GraphColors[i] = ColorRgba.Lerp(ColorRgba.White, ColorRgba.Red, factor);
						}
						this.canvas.State.ColorTint = ColorRgba.Black.WithAlpha(0.5f);
						this.canvas.FillRect(graphRect.X, graphY, graphRect.W, graphH);
						this.canvas.State.ColorTint = ColorRgba.White;
						this.DrawHorizontalGraph(canvas, cache.GraphValues, cache.GraphColors, ref cache.VertGraph, graphRect.X, graphY, graphRect.W, graphH);
						cursorRatio = (float)cursorPos / (float)ProfileCounter.ValueHistoryLen;
					}
					else if (counter is StatCounter)
					{
						StatCounter statCounter = counter as StatCounter;
						int cursorPos = statCounter.ValueGraphCursor;
						for (int i = Math.Max(cursorPos - 1, 0); i <= Math.Min(cursorPos, ProfileCounter.ValueHistoryLen - 1); i++)
						{
							cache.GraphValues[i] = (float)(statCounter.ValueGraph[i] - statCounter.MinValue) / statCounter.MaxValue;
							cache.GraphColors[i] = ColorRgba.White;
						}
						this.canvas.State.ColorTint = ColorRgba.Black.WithAlpha(0.5f);
						this.canvas.FillRect(graphRect.X, graphY, graphRect.W, graphH);
						this.canvas.State.ColorTint = ColorRgba.White;
						this.DrawHorizontalGraph(this.canvas, cache.GraphValues, cache.GraphColors, ref cache.VertGraph, graphRect.X, graphY, graphRect.W, graphH);
						cursorRatio = (float)cursorPos / (float)ProfileCounter.ValueHistoryLen;
					}
					
					this.canvas.DrawText(new string[] { counter.FullName }, ref cache.VertText, graphRect.X, graphY);
					this.canvas.DrawLine(graphRect.X + graphRect.W * cursorRatio, graphY, graphRect.X + graphRect.W * cursorRatio, graphY + graphH);

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

			this.canvas.End();
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
			if (DualityApp.Keyboard.KeyHit(this.keyResetCounters))
				Profile.ResetCounters();
		}
		
		private void DrawHorizontalGraph(Canvas canvas, float[] values, ColorRgba[] colors, ref VertexC1P3[] vertices, float x, float y, float w, float h)
		{
			if (h > 0.0f) h--;
			if (h < 0.0f) h++;
			
			IDrawDevice device = canvas.DrawDevice;

			ColorRgba baseColor = canvas.State.ColorTint;
			float sampleXRatio = w / (float)(values.Length - 1);
			
			if (vertices == null)
				vertices = new VertexC1P3[MathF.Max(values.Length, 16)];
			else if (vertices.Length < values.Length)
				vertices = new VertexC1P3[MathF.Max(vertices.Length * 2, values.Length, 16)];

			for (int i = 0; i < values.Length; i++)
			{
				vertices[i].Pos.X = x + 0.5f + i * sampleXRatio;
				vertices[i].Pos.Y = y + 0.5f + (1.0f - values[i]) * h;
				vertices[i].Pos.Z = 0.0f;
				vertices[i].Color = baseColor * colors[i];
			}
			device.AddVertices(canvas.State.MaterialDirect, VertexMode.LineStrip, vertices, values.Length);
		}
	}
}
