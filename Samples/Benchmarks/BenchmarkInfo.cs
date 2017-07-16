using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Resources;
using Duality.Editor;
using Duality.Drawing;

namespace Duality.Samples.Benchmarks
{
	[EditorHintCategory("Benchmarks")]
    public class BenchmarkInfo : Component, ICmpBenchmarkOverlayRenderer, ICmpInitializable
	{
		private int maxWidth = 500;
		private Vector2 margin = new Vector2(15, 8);
		private ContentRef<Font> mainFont = null;
		private ContentRef<Font> headlineFont = null;
		private string benchmarkName = "Benchmark Name";
		private string benchmarkDesc = "Benchmark Description.";
		private string generalControls = "General Controls.";
		
		[DontSerialize] 
		private string textTemplate = "/f[1]{0}/f[0]/n/n{1}/n/n{2}";
		[DontSerialize] 
		private FormattedText text = new FormattedText();


		public ContentRef<Font> MainFont
		{
			get { return this.mainFont; }
			set { this.mainFont = value; this.UpdateText(); }
		}
		public ContentRef<Font> HeadlineFont
		{
			get { return this.headlineFont; }
			set { this.headlineFont = value; this.UpdateText(); }
		}
		public int MaxWidth
		{
			get { return this.maxWidth; }
			set { this.maxWidth = value; this.UpdateText(); }
		}
		public Vector2 Margin
		{
			get { return this.margin; }
			set { this.margin = value; this.UpdateText(); }
		}
		public string BenchmarkName
		{
			get { return this.benchmarkName; }
			set { this.benchmarkName = value; this.UpdateText(); }
		}
		public string BenchmarkDesc
		{
			get { return this.benchmarkDesc; }
			set { this.benchmarkDesc = value; this.UpdateText(); }
		}
		public string GeneralControls
		{
			get { return this.generalControls; }
			set { this.generalControls = value; this.UpdateText(); }
		}


		private void UpdateText()
		{
			this.text.Fonts = new ContentRef<Font>[]
			{
				this.mainFont,
				this.headlineFont
			};
			this.text.MaxWidth = this.maxWidth;
			this.text.SourceText = string.Format(
				this.textTemplate,
				this.benchmarkName,
				this.benchmarkDesc,
				this.generalControls);
		}

		void ICmpBenchmarkOverlayRenderer.DrawOverlay(Canvas canvas)
		{
			Vector2 textBlockSize = this.text.TextMetrics.Size;
			canvas.State.ColorTint = ColorRgba.Black.WithAlpha(0.75f);
			canvas.FillRect(10, 10, textBlockSize.X + this.margin.X * 2, textBlockSize.Y + this.margin.Y * 2);

			canvas.State.ColorTint = ColorRgba.White;
			canvas.DrawText(this.text, 10 + this.margin.X, 10 + this.margin.Y);
		}
		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				this.UpdateText();
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) { }
	}
}
