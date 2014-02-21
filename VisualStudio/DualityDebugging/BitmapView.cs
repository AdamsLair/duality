using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Duality;
using Duality.Drawing;

namespace Duality.VisualStudio
{
	public partial class BitmapView : UserControl
	{
		private	Bitmap		bmp			= null;
		private	bool		useAlpha	= true;
		private	bool		useRed		= true;
		private	bool		useGreen	= true;
		private	bool		useBlue		= true;
		private	ColorRgba	avgColor	= ColorRgba.Black;

		public Bitmap Bitmap
		{
			get { return this.bmp; }
			set
			{
				this.bmp = value;
				this.avgColor = this.bmp != null ? this.bmp.GetAverageColor() : ColorRgba.Black;
				this.UpdateSize();
				this.Invalidate();
			}
		}
		public bool UseRed
		{
			get { return this.useRed; }
			set
			{
				this.useRed = value;
				this.Invalidate();
			}
		}
		public bool UseGreen
		{
			get { return this.useGreen; }
			set
			{
				this.useGreen = value;
				this.Invalidate();
			}
		}
		public bool UseBlue
		{
			get { return this.useBlue; }
			set
			{
				this.useBlue = value;
				this.Invalidate();
			}
		}
		public bool UseAlpha
		{
			get { return this.useAlpha; }
			set
			{
				this.useAlpha = value;
				this.Invalidate();
			}
		}

		public BitmapView()
		{
			InitializeComponent();
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		protected void UpdateSize()
		{
			if (this.bmp == null) return;
			int xdiff = this.Width - this.ClientRectangle.Width;
			int ydiff = this.Height - this.ClientRectangle.Height;
			this.Width = Math.Min(xdiff + this.bmp.Width, 600);
			this.Height = Math.Min(ydiff + this.bmp.Height, 600);
			this.AutoScrollMinSize = this.bmp.Size;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.UpdateSize();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Color brightChecker = this.avgColor.GetLuminance() > 0.5f ? Color.FromArgb(48, 48, 48) : Color.FromArgb(224, 224, 224);
			Color darkChecker = this.avgColor.GetLuminance() > 0.5f ? Color.FromArgb(32, 32, 32) : Color.FromArgb(192, 192, 192);
			
			e.Graphics.FillRectangle(new HatchBrush(HatchStyle.LargeCheckerBoard, brightChecker, darkChecker), this.ClientRectangle);
			
			if (this.bmp != null)
			{
				// Create a new color matrix and set the alpha value to 0.5
				ColorMatrix cm = new ColorMatrix();
				cm.Matrix00 = cm.Matrix11 = cm.Matrix22 = cm.Matrix44 = 1;
				if (!this.useAlpha) cm.Matrix43 = 1.0f;
				if (!this.useRed) cm.Matrix00 = 0.0f;
				if (!this.useGreen) cm.Matrix11 = 0.0f;
				if (!this.useBlue) cm.Matrix22 = 0.0f;

				// Create a new image attribute object and set the color matrix to
				// the one just created
				ImageAttributes ia = new ImageAttributes();
				ia.SetColorMatrix(cm);

				// Draw the original image with the image attributes specified
				e.Graphics.DrawImage(this.bmp,
					new Rectangle(this.AutoScrollPosition.X, this.AutoScrollPosition.Y, this.bmp.Width, this.bmp.Height),
					0, 0, this.bmp.Width, this.bmp.Height, GraphicsUnit.Pixel,
					ia);
			}
		}
		protected override void OnScroll(ScrollEventArgs se)
		{
			base.OnScroll(se);
			this.Invalidate();
		}
	}
}
