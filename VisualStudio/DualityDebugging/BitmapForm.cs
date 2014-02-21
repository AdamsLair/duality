using System;
using System.Drawing;
using System.Windows.Forms;

using Duality;
using Duality.Drawing;

namespace Duality.VisualStudio
{
	public partial class BitmapForm : Form
	{
		public Bitmap Bitmap
		{
			get { return this.bitmapView.Bitmap; }
			set
			{
				this.bitmapView.Bitmap = value;
				this.actionSave.Enabled = this.bitmapView.Bitmap != null;
				this.UpdateSize();
			}
		}

		public BitmapForm()
		{
			this.InitializeComponent();
		}

		protected void UpdateSize()
		{
			int xdiff = this.Width - this.bitmapView.ClientRectangle.Width;
			int ydiff = this.Height - this.bitmapView.ClientRectangle.Height;
			this.Width = Math.Min(xdiff + this.bitmapView.AutoScrollMinSize.Width, 600);
			this.Height = Math.Min(ydiff + this.bitmapView.AutoScrollMinSize.Height, 600);
		}

		private void checkAlpha_CheckedChanged(object sender, EventArgs e)
		{
			this.bitmapView.UseAlpha = this.checkAlpha.Checked;
		}
		private void checkRed_CheckedChanged(object sender, EventArgs e)
		{
			this.bitmapView.UseRed = this.checkRed.Checked;
		}
		private void checkGreen_CheckedChanged(object sender, EventArgs e)
		{
			this.bitmapView.UseGreen = this.checkGreen.Checked;
		}
		private void checkBlue_CheckedChanged(object sender, EventArgs e)
		{
			this.bitmapView.UseBlue = this.checkBlue.Checked;
		}
		private void actionSave_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Filter = "PNG Image (*.png)|*.png|Jpeg Image (*.jpg)|*.jpg|TIFF Image (*.tiff)|*.tiff";
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.bitmapView.Bitmap.Save(dialog.FileName);
			}
		}
	}
}
