using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Templates;
using AdamsLair.WinForms.Drawing;
using BorderStyle = AdamsLair.WinForms.Drawing.BorderStyle;

using Duality;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	public class PixmapPreviewPropertyEditor : ImagePreviewPropertyEditor
	{
		private	Pixmap		value				= null;
		private	Rectangle	rectLabelWidth		= Rectangle.Empty;
		private	Rectangle	rectLabelHeight		= Rectangle.Empty;
		private	Rectangle	rectLabelWidthVal	= Rectangle.Empty;
		private	Rectangle	rectLabelHeightVal	= Rectangle.Empty;

		public override object DisplayedValue
		{
			get { return this.value; }
		}
		protected override int PreviewFrameCount
		{
			get { return this.value != null && this.value.Atlas != null ? this.value.Atlas.Count : 0; }
		}


		public PixmapPreviewPropertyEditor()
		{
			this.Height = SmallHeight;
			this.Hints = HintFlags.None;
		}
		
		protected override int GetPreviewHash()
		{
			Pixmap.Layer basePxLayer = this.value != null ? this.value.MainLayer : null;
			int hash = this.value != null && this.value.Atlas != null ? this.value.Atlas.GetCombinedHashCode() : 0;
			MathF.CombineHashCode(ref hash, basePxLayer != null ? basePxLayer.GetHashCode() : 0);
			return hash;
		}
		protected override Bitmap GeneratePreviewFrame(int frameIndex)
		{
			if (this.value == null)
			{
				return null;
			}
			else if (frameIndex == -1)
			{
				return PreviewProvider.GetPreviewImage(
					this.value, 
					this.ClientRectangle.Width - 2, 
					Math.Min(BigHeight - 2, this.value.Height), 
					PreviewSizeMode.FixedHeight);
			}
			else
			{
				if (this.value.MainLayer == null) return null;

				Rect pxRect;
				this.value.LookupAtlas(frameIndex, out pxRect);
				Pixmap.Layer subImage = this.value.MainLayer.CloneSubImage((int)pxRect.X, (int)pxRect.Y, (int)pxRect.W, (int)pxRect.H);
				return subImage.ToBitmap();
			}
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();

			Pixmap lastValue = this.value;
			Pixmap[] values = this.GetValue().Cast<Pixmap>().ToArray();

			this.value = values.NotNull().FirstOrDefault() as Pixmap;
			this.ResetPreviewImage();

			if (this.value != lastValue)
				this.Invalidate();
			else
				this.Invalidate(this.rectHeader);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			ControlRenderer.DrawStringLine(e.Graphics, 
				"Width:", 
				SystemFonts.DefaultFont, 
				this.rectLabelWidth, 
				!this.Enabled ? ControlRenderer.ColorGrayText : ControlRenderer.ColorText);
			ControlRenderer.DrawStringLine(e.Graphics, 
				"Height:", 
				SystemFonts.DefaultFont, 
				this.rectLabelHeight, 
				!this.Enabled ? ControlRenderer.ColorGrayText : ControlRenderer.ColorText);
			ControlRenderer.DrawStringLine(e.Graphics, 
				this.value != null ? this.value.Width.ToString() : " - ", 
				SystemFonts.DefaultFont, 
				this.rectLabelWidthVal, 
				!this.Enabled ? ControlRenderer.ColorGrayText : ControlRenderer.ColorText);
			ControlRenderer.DrawStringLine(e.Graphics, 
				this.value != null ? this.value.Height.ToString() : " - ", 
				SystemFonts.DefaultFont, 
				this.rectLabelHeightVal, 
				!this.Enabled ? ControlRenderer.ColorGrayText : ControlRenderer.ColorText);

			if (this.PreviewFrameCount == 0)
			{
				ControlRenderer.DrawStringLine(e.Graphics, 
					this.value != null ? this.value.Name : " - ", 
					SystemFonts.DefaultFont, 
					this.rectLabelName, 
					!this.Enabled ? ControlRenderer.ColorGrayText : ControlRenderer.ColorText,
					StringAlignment.Far);
			}
		}
		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();

			this.rectLabelWidth = new Rectangle(
				this.rectHeaderContent.X,
				this.rectHeaderContent.Y,
				45,
				this.rectHeaderContent.Height / 2);
			this.rectLabelHeight = new Rectangle(
				this.rectHeaderContent.X,
				this.rectLabelWidth.Bottom,
				45,
				this.rectHeaderContent.Height / 2);

			this.rectLabelWidthVal = new Rectangle(
				this.rectLabelWidth.Right,
				this.rectHeaderContent.Y,
				35,
				this.rectHeaderContent.Height / 2);
			this.rectLabelHeightVal = new Rectangle(
				this.rectLabelHeight.Right,
				this.rectLabelWidthVal.Bottom,
				35,
				this.rectHeaderContent.Height / 2);
			
			this.rectLabelName.X = this.rectLabelWidthVal.Right;
			this.rectLabelName.Width = this.rectHeaderContent.Width - this.rectLabelWidthVal.Right;
		}
	}
}
