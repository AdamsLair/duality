using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.Drawing;
using BorderStyle = AdamsLair.WinForms.Drawing.BorderStyle;

using Duality;
using Duality.Editor;
using Font = Duality.Resources.Font;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	public partial class FontPreviewPropertyEditor : PropertyEditor
	{
		protected const int HeaderHeight = 32;
		protected const int PreferredHeight = 64 + HeaderHeight;

		private	Font	value				= null;
		private	Bitmap	prevImage			= null;
		private	float	prevImageLum		= 0.0f;
		private	Font	prevImageValue		= null;

		public override object DisplayedValue
		{
			get { return this.value; }
		}
		public override bool CanGetFocus
		{
			get { return false; }
		}


		public FontPreviewPropertyEditor()
		{
			this.Height = PreferredHeight;
			this.Hints = HintFlags.None;
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
		}
		
		public void InvalidatePreview()
		{
			this.prevImageValue = null;
		}
		protected void GeneratePreviewImage()
		{
			if (this.prevImageValue == this.value) return;
			this.prevImageValue = this.value;

			if (this.prevImage != null) this.prevImage.Dispose();
			this.prevImage = null;

			if (this.value != null)
			{
				this.prevImage = PreviewProvider.GetPreviewImage(this.value, this.ClientRectangle.Width - 2, this.ClientRectangle.Height - 2, PreviewSizeMode.FixedHeight);
				if (this.prevImage != null)
				{
					var avgColor = this.prevImage.GetAverageColor();
					this.prevImageLum = avgColor.GetLuminance();
				}
			}
			this.Invalidate();
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();
			Font[] values = this.GetValue().OfType<Font>().ToArray();
			this.value = values.NotNull().FirstOrDefault() as Font;
			this.GeneratePreviewImage();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			Rectangle rectPreview = this.ClientRectangle;
			Rectangle rectImage = new Rectangle(rectPreview.X + 1, rectPreview.Y + 1, rectPreview.Width - 2, rectPreview.Height - 2);
			Color brightChecker = this.prevImageLum > 0.5f ? Color.FromArgb(48, 48, 48) : Color.FromArgb(224, 224, 224);
			Color darkChecker = this.prevImageLum > 0.5f ? Color.FromArgb(32, 32, 32) : Color.FromArgb(192, 192, 192);
			e.Graphics.FillRectangle(new HatchBrush(HatchStyle.LargeCheckerBoard, brightChecker, darkChecker), rectImage);
			if (this.prevImage != null)
			{
				TextureBrush bgImageBrush = new TextureBrush(this.prevImage);
				bgImageBrush.ResetTransform();
				bgImageBrush.TranslateTransform(rectImage.X, rectImage.Y);
				e.Graphics.FillRectangle(bgImageBrush, rectImage);
			}

			ControlRenderer.DrawBorder(e.Graphics, 
				rectPreview, 
				BorderStyle.Simple, 
				!this.Enabled ? BorderState.Disabled : BorderState.Normal);
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			DualityEditorApp.ObjectPropertyChanged -= this.DualityEditorApp_ObjectPropertyChanged;
		}

		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.HasObject(this.prevImageValue))
			{
				this.InvalidatePreview();
				this.PerformGetValue();
			}
		}
	}
}
