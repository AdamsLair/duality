using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Resources;
using Duality.Editor;
using Duality.Drawing;
using Duality.Input;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Samples.Physics
{
	[EditorHintCategory("PhysicsSample")]
    public class PhysicsSampleInfo : Component, ICmpRenderer, ICmpInitializable
	{
		private int maxWidth = 500;
		private Vector2 margin = new Vector2(15, 8);
		private ContentRef<Font> mainFont = null;
		private ContentRef<Font> headlineFont = null;
		private string sampleName = "Sample Name";
		private string sampleDesc = "Sample Description.";
		private string sampleControls = "Sample Controls.";
		private string generalControls = "General Controls.";
		
		[DontSerialize] 
		private string textTemplate = "/f[1]Example: {0}/f[0]/n/n{1}/n/n{2}{3}";
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
		public string SampleName
		{
			get { return this.sampleName; }
			set { this.sampleName = value; this.UpdateText(); }
		}
		public string SampleDesc
		{
			get { return this.sampleDesc; }
			set { this.sampleDesc = value; this.UpdateText(); }
		}
		public string SampleControls
		{
			get { return this.sampleControls; }
			set { this.sampleControls = value; this.UpdateText(); }
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
				this.sampleName,
				this.sampleDesc,
				string.IsNullOrEmpty(this.sampleControls) ? string.Empty : this.sampleControls + "/n",
				this.generalControls);
		}

		void ICmpRenderer.GetCullingInfo(out CullingInfo info)
		{
			info.Position = Vector3.Zero;
			info.Radius = float.MaxValue;
			info.Visibility = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			Canvas canvas = new Canvas();
			canvas.Begin(device);

			Vector2 textBlockSize = this.text.TextMetrics.Size;
			canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha));
			canvas.State.ColorTint = ColorRgba.Black.WithAlpha(0.75f);
			canvas.FillRect(10, 10, textBlockSize.X + this.margin.X * 2, textBlockSize.Y + this.margin.Y * 2);

			canvas.State.ColorTint = ColorRgba.White;
			canvas.DrawText(this.text, 10 + this.margin.X, 10 + this.margin.Y);

			canvas.End();
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
