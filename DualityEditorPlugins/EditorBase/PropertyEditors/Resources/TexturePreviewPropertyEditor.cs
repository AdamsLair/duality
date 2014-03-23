using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using AdamsLair.WinForms;
using AdamsLair.WinForms.EditorTemplates;
using AdamsLair.WinForms.Renderer;
using BorderStyle = AdamsLair.WinForms.Renderer.BorderStyle;

using Duality;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	public class TexturePreviewPropertyEditor : ImagePreviewPropertyEditor
	{
		private	Texture		value					= null;
		private	Rectangle	rectLabelWidth			= Rectangle.Empty;
		private	Rectangle	rectLabelHeight			= Rectangle.Empty;
		private	Rectangle	rectLabelWidthVal		= Rectangle.Empty;
		private	Rectangle	rectLabelHeightVal		= Rectangle.Empty;
		private	Rectangle	rectLabelOglWidth		= Rectangle.Empty;
		private	Rectangle	rectLabelOglHeight		= Rectangle.Empty;
		private	Rectangle	rectLabelOglWidthVal	= Rectangle.Empty;
		private	Rectangle	rectLabelOglHeightVal	= Rectangle.Empty;

		public override object DisplayedValue
		{
			get { return this.value; }
		}
		protected override int PreviewFrameCount
		{
			get
			{ 
				return 
					this.value != null && this.value.BasePixmap.IsAvailable && this.value.BasePixmap.Res.Atlas != null ? 
					this.value.BasePixmap.Res.Atlas.Count : 
					0;
			}
		}


		public TexturePreviewPropertyEditor()
		{
			this.Height = SmallHeight;
			this.Hints = HintFlags.None;
		}

		protected override int GetPreviewHash()
		{
			Pixmap basePx = this.value != null ? this.value.BasePixmap.Res : null;
			Pixmap.Layer basePxLayer = basePx != null ? basePx.MainLayer : null;
			int hash = basePx != null && basePx.Atlas != null ? basePx.Atlas.GetCombinedHashCode() : 0;
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
					Math.Min(BigHeight - 2, this.value.PixelHeight), 
					PreviewSizeMode.FixedHeight);
			}
			else
			{
				if (!this.value.BasePixmap.IsAvailable) return null;
				if (this.value.BasePixmap.Res.MainLayer == null) return null;

				Rect pxRect;
				this.value.BasePixmap.Res.LookupAtlas(frameIndex, out pxRect);
				Pixmap.Layer subImage = this.value.BasePixmap.Res.MainLayer.CloneSubImage((int)pxRect.X, (int)pxRect.Y, (int)pxRect.W, (int)pxRect.H);
				return subImage.ToBitmap();
			}
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();

			Texture lastValue = this.value;
			Texture[] values = this.GetValue().Cast<Texture>().ToArray();

			this.value = values.NotNull().FirstOrDefault() as Texture;
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
				this.value != null ? this.value.PixelWidth.ToString() : " - ", 
				SystemFonts.DefaultFont, 
				this.rectLabelWidthVal, 
				!this.Enabled ? ControlRenderer.ColorGrayText : ControlRenderer.ColorText);
			ControlRenderer.DrawStringLine(e.Graphics, 
				this.value != null ? this.value.PixelHeight.ToString() : " - ", 
				SystemFonts.DefaultFont, 
				this.rectLabelHeightVal, 
				!this.Enabled ? ControlRenderer.ColorGrayText : ControlRenderer.ColorText);

			ControlRenderer.DrawStringLine(e.Graphics, 
				"OglWidth:", 
				SystemFonts.DefaultFont, 
				this.rectLabelOglWidth, 
				!this.Enabled ? ControlRenderer.ColorGrayText : ControlRenderer.ColorText);
			ControlRenderer.DrawStringLine(e.Graphics, 
				"OglHeight:", 
				SystemFonts.DefaultFont, 
				this.rectLabelOglHeight, 
				!this.Enabled ? ControlRenderer.ColorGrayText : ControlRenderer.ColorText);
			ControlRenderer.DrawStringLine(e.Graphics, 
				this.value != null ? this.value.TexelWidth.ToString() : " - ", 
				SystemFonts.DefaultFont, 
				this.rectLabelOglWidthVal, 
				!this.Enabled ? ControlRenderer.ColorGrayText : ControlRenderer.ColorText);
			ControlRenderer.DrawStringLine(e.Graphics, 
				this.value != null ? this.value.TexelHeight.ToString() : " - ", 
				SystemFonts.DefaultFont, 
				this.rectLabelOglHeightVal, 
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

			this.rectLabelOglWidth = new Rectangle(
				this.rectLabelWidthVal.Right,
				this.rectHeaderContent.Y,
				65,
				this.rectHeaderContent.Height / 2);
			this.rectLabelOglHeight = new Rectangle(
				this.rectLabelWidthVal.Right,
				this.rectLabelOglWidth.Bottom,
				65,
				this.rectHeaderContent.Height / 2);

			this.rectLabelOglWidthVal = new Rectangle(
				this.rectLabelOglWidth.Right,
				this.rectHeaderContent.Y,
				35,
				rectHeaderContent.Height / 2);
			this.rectLabelOglHeightVal = new Rectangle(
				this.rectLabelOglHeight.Right,
				this.rectLabelOglWidthVal.Bottom,
				35,
				this.rectHeaderContent.Height / 2);
			
			this.rectLabelName.X = this.rectLabelOglWidthVal.Right;
			this.rectLabelName.Width = this.rectHeaderContent.Width - this.rectLabelOglWidthVal.Right;
		}
	}
}
