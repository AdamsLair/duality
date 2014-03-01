using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using AdamsLair.PropertyGrid;
using AdamsLair.PropertyGrid.Renderer;
using BorderStyle = AdamsLair.PropertyGrid.Renderer.BorderStyle;

using Duality;
using Duality.Resources;
using Duality.Editor;
using Duality.Editor.Plugins.Base.Properties;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	public partial class AudioDataPreviewPropertyEditor : PropertyEditor
	{
		protected const int HeaderHeight = 32;
		protected const int PreferredHeight = 64 + HeaderHeight;

		private	Rectangle	rectImage			= Rectangle.Empty;
		private	Rectangle	rectPrevSound		= Rectangle.Empty;
		private	AudioData	value				= null;
		private	Sound		prevSound			= null;
		private	SoundInstance	prevSoundInst	= null;
		private	Bitmap		prevImage			= null;
		private	float		prevImageLum		= 0.0f;
		private	int			prevImageValue		= 0;

		public override object DisplayedValue
		{
			get { return this.value; }
		}
		public override bool CanGetFocus
		{
			get { return false; }
		}


		public AudioDataPreviewPropertyEditor()
		{
			this.Height = PreferredHeight;
			this.Hints = HintFlags.None;
		}
		
		protected void GeneratePreview()
		{
			int ovLen = this.value != null && this.value.OggVorbisData != null ? this.value.OggVorbisData.Length : 0;
			if (this.prevImageValue == ovLen) return;
			this.prevImageValue = ovLen;

			this.StopPreviewSound();
			if (this.prevSound != null) this.prevSound.Dispose();
			this.prevSound = null;

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
				
				this.prevSound = PreviewProvider.GetPreviewSound(this.value);
			}

			this.Invalidate();
		}
		public void PlayPreviewSound()
		{
			if (this.prevSound == null) return;
			if (this.prevSoundInst != null) return;

			this.prevSoundInst = DualityApp.Sound.PlaySound(this.prevSound);
			this.prevSoundInst.Looped = true;
			this.Invalidate();
		}
		public void StopPreviewSound()
		{
			if (this.prevSoundInst == null) return;
			
			this.prevSoundInst.FadeOut(0.25f);
			this.prevSoundInst = null;
			this.Invalidate();
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();
			AudioData lastValue = this.value;
			AudioData[] values = this.GetValue().Cast<AudioData>().ToArray();
			this.value = values.NotNull().FirstOrDefault() as AudioData;
			this.GeneratePreview();
			if (this.value != lastValue) this.Invalidate();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			Rectangle rectPreview = this.ClientRectangle;
			Color brightChecker = this.prevImageLum > 0.5f ? Color.FromArgb(48, 48, 48) : Color.FromArgb(224, 224, 224);
			Color darkChecker = this.prevImageLum > 0.5f ? Color.FromArgb(32, 32, 32) : Color.FromArgb(192, 192, 192);
			e.Graphics.FillRectangle(new HatchBrush(HatchStyle.LargeCheckerBoard, brightChecker, darkChecker), this.rectImage);
			if (this.prevImage != null)
			{
				TextureBrush bgImageBrush = new TextureBrush(this.prevImage);
				bgImageBrush.ResetTransform();
				bgImageBrush.TranslateTransform(this.rectImage.X, this.rectImage.Y);
				e.Graphics.FillRectangle(bgImageBrush, this.rectImage);
			}

			if (this.prevSoundInst != null)
			{
				e.Graphics.DrawImage(this.prevImageLum > 0.5f ? EditorBaseResCache.IconSpeakerWhite : EditorBaseResCache.IconSpeakerBlack, 
					this.rectPrevSound.X, 
					this.rectPrevSound.Y);
			}
			else
			{
				e.Graphics.DrawImageAlpha(this.prevImageLum > 0.5f ? EditorBaseResCache.IconSpeakerWhite : EditorBaseResCache.IconSpeakerBlack, 
					0.5f,
					this.rectPrevSound.X, 
					this.rectPrevSound.Y);
			}

			ControlRenderer.DrawBorder(e.Graphics, 
				rectPreview, 
				BorderStyle.Simple, 
				!this.Enabled ? BorderState.Disabled : BorderState.Normal);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.rectPrevSound.Contains(e.Location))
				this.PlayPreviewSound();
			else
				this.StopPreviewSound();
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.StopPreviewSound();
		}
		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();
			Rectangle rectPreview = this.ClientRectangle;

			this.rectImage = new Rectangle(rectPreview.X + 1, rectPreview.Y + 1, rectPreview.Width - 2, rectPreview.Height - 2);
			this.rectPrevSound = new Rectangle(this.rectImage.Right - 16, this.rectImage.Y, 16, 16);
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.StopPreviewSound();
		}
	}
}
