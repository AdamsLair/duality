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
	public abstract class ImagePreviewPropertyEditor : PropertyEditor
	{
		protected const int HeaderHeight = 30;
		protected const int SmallHeight = 64 + HeaderHeight;
		protected const int BigHeight = 256 + HeaderHeight;

		private		List<Bitmap>	prevImageFrame		= new List<Bitmap>();
		private		float			prevImageLum		= 0.0f;
		private		int				prevImageHash		= -1;
		protected	Rectangle		rectHeader			= Rectangle.Empty;
		protected	Rectangle		rectHeaderContent	= Rectangle.Empty;
		protected	Rectangle		rectPreview			= Rectangle.Empty;
		protected	Rectangle		rectLabelName		= Rectangle.Empty;
		private		NumericEditorTemplate	subImageSelector	= null;


		protected abstract int PreviewFrameCount { get; }


		public ImagePreviewPropertyEditor()
		{
			this.subImageSelector = new NumericEditorTemplate(this);
			this.subImageSelector.Edited += this.subImageSelector_Edited;
			this.subImageSelector.Invalidate += this.subImageSelector_Invalidate;
			this.subImageSelector.ReadOnly = false;
			this.subImageSelector.Minimum = -1;
			this.subImageSelector.Maximum = -1;
			this.subImageSelector.Value = -1;

			this.Height = SmallHeight;
			this.Hints = HintFlags.None;
		}
		
		protected abstract int GetPreviewHash();
		protected abstract Bitmap GeneratePreviewFrame(int frameIndex);

		protected void ResetPreviewImage()
		{
			int prevHash = this.GetPreviewHash();
			if (this.prevImageHash == prevHash) return;
			this.prevImageHash = prevHash;

			// Clear old preview images
			foreach (Bitmap bmp in this.prevImageFrame.NotNull()) bmp.Dispose();
			this.prevImageFrame.Clear();

			// Generate base preview
			Bitmap baseImage = this.GeneratePreviewFrame(-1);
			if (baseImage != null)
			{
				var avgColor = baseImage.GetAverageColor();
				this.prevImageLum = avgColor.GetLuminance();
			}
			
			// Update frame selector visibility
			if (this.PreviewFrameCount > 0)
			{
				this.subImageSelector.ReadOnly = false;
				this.subImageSelector.Maximum = this.PreviewFrameCount - 1;
			}
			else
			{
				this.subImageSelector.ReadOnly = true;
				this.subImageSelector.Value = -1;
				this.subImageSelector.Maximum = -1;
			}
			this.UpdateGeometry();

			this.AdjustPreviewHeight(false);
			this.Invalidate();
		}
		protected Bitmap GetPreviewFrame(int frameIndex)
		{
			while (this.prevImageFrame.Count <= frameIndex + 1)
				this.prevImageFrame.Add(null);

			if (this.prevImageFrame[frameIndex + 1] == null)
				this.prevImageFrame[frameIndex + 1] = this.GeneratePreviewFrame(frameIndex);

			return this.prevImageFrame[frameIndex + 1];
		}
		protected void AdjustPreviewHeight(bool toggle)
		{
			Bitmap basePreview = this.GetPreviewFrame(-1);
			int targetHeight = MathF.Clamp(basePreview == null ? 0 : basePreview.Height + 2 + this.rectHeader.Height, SmallHeight, BigHeight);
			if (!toggle || this.Height != targetHeight)
				this.Height = targetHeight;
			else
				this.Height = SmallHeight;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			Rectangle rectImage = new Rectangle(this.rectPreview.X + 1, this.rectPreview.Y + 1, this.rectPreview.Width - 2, this.rectPreview.Height - 2);
			Color brightChecker = this.prevImageLum > 0.5f ? Color.FromArgb(48, 48, 48) : Color.FromArgb(224, 224, 224);
			Color darkChecker = this.prevImageLum > 0.5f ? Color.FromArgb(32, 32, 32) : Color.FromArgb(192, 192, 192);
			Bitmap img = this.GetPreviewFrame((int)this.subImageSelector.Value);
			e.Graphics.FillRectangle(new HatchBrush(HatchStyle.LargeCheckerBoard, brightChecker, darkChecker), rectImage);
			if (img != null)
			{
				Size imgSize = img.Size;
				float widthForHeight = (float)imgSize.Width / (float)imgSize.Height;
				if (widthForHeight * (imgSize.Height - rectImage.Height) > imgSize.Width - rectImage.Width)
				{
					imgSize.Height = Math.Min(rectImage.Height, imgSize.Height);
					imgSize.Width = MathF.RoundToInt(widthForHeight * imgSize.Height);
				}
				else
				{
					imgSize.Width = Math.Min(rectImage.Width, imgSize.Width);
					imgSize.Height = MathF.RoundToInt(imgSize.Width / widthForHeight);
				}
				e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				e.Graphics.DrawImage(img, 
					rectImage.X + rectImage.Width / 2 - imgSize.Width / 2,
					rectImage.Y + rectImage.Height / 2 - imgSize.Height / 2,
					imgSize.Width,
					imgSize.Height);
				e.Graphics.InterpolationMode = InterpolationMode.Default;
			}

			ControlRenderer.DrawBorder(e.Graphics, 
				this.rectPreview, 
				BorderStyle.Simple, 
				!this.Enabled ? BorderState.Disabled : BorderState.Normal);

			bool focusBg = this.Focused || (this is IPopupControlHost && (this as IPopupControlHost).IsDropDownOpened);
			Color headerBgColor = this.ControlRenderer.ColorBackground;
			if (focusBg)
			{
				headerBgColor = headerBgColor.ScaleBrightness(this.ControlRenderer.FocusBrightnessScale);
			}
			GroupedPropertyEditor.DrawGroupHeaderBackground(
				e.Graphics, 
				this.rectHeader, 
				headerBgColor, 
				GroupedPropertyEditor.GroupHeaderStyle.SmoothSunken);
			
			if (this.subImageSelector.Rect.Width > 0)
			{
				this.ControlRenderer.DrawStringLine(e.Graphics, 
					"Frame Index", 
					SystemFonts.DefaultFont, 
					this.rectLabelName, 
					!this.Enabled ? this.ControlRenderer.ColorGrayText : this.ControlRenderer.ColorText,
					StringAlignment.Far);
				this.subImageSelector.OnPaint(e, this.Enabled && !this.subImageSelector.ReadOnly, false);
			}
		}
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.subImageSelector.OnGotFocus(e);
		}
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.subImageSelector.OnLostFocus(e);
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			this.subImageSelector.OnKeyDown(e);
			base.OnKeyDown(e);
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			this.subImageSelector.OnKeyUp(e);
		}
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			this.subImageSelector.OnKeyPress(e);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.subImageSelector.OnMouseLeave(e);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.subImageSelector.OnMouseDown(e);
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.subImageSelector.OnMouseUp(e);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.subImageSelector.OnMouseMove(e);
		}
		protected override void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			this.AdjustPreviewHeight(true);
		}
		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();

			int subImageSelWidth = this.subImageSelector.ReadOnly ? 0 : 40;
			this.subImageSelector.Rect = new Rectangle(
				this.ClientRectangle.Right - subImageSelWidth,
				this.ClientRectangle.Y + HeaderHeight / 2 - 20 / 2,
				subImageSelWidth,
				20);
			this.rectHeader = new Rectangle(
				this.ClientRectangle.X,
				this.ClientRectangle.Y,
				this.ClientRectangle.Width,
				HeaderHeight);
			this.rectPreview = new Rectangle(
				this.ClientRectangle.X,
				this.ClientRectangle.Y + HeaderHeight,
				this.ClientRectangle.Width,
				this.ClientRectangle.Height - HeaderHeight);

			this.rectHeaderContent = new Rectangle(
				this.rectHeader.X + 2,
				this.rectHeader.Y + 2,
				this.rectHeader.Width - 4 - this.subImageSelector.Rect.Width,
				this.rectHeader.Height - 4);

			this.rectLabelName = new Rectangle(
				this.rectHeaderContent.X,
				this.rectHeaderContent.Y,
				this.rectHeaderContent.Width,
				this.rectHeaderContent.Height);
		}

		private void subImageSelector_Edited(object sender, EventArgs e)
		{
			this.Invalidate();
		}
		private void subImageSelector_Invalidate(object sender, EventArgs e)
		{
			this.Invalidate();
		}
	}
}
