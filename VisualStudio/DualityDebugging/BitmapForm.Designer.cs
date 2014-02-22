namespace Duality.VisualStudio
{
	partial class BitmapForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BitmapForm));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.checkAlpha = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.checkBlue = new System.Windows.Forms.ToolStripButton();
			this.checkGreen = new System.Windows.Forms.ToolStripButton();
			this.checkRed = new System.Windows.Forms.ToolStripButton();
			this.bitmapView = new Duality.VisualStudio.BitmapView();
			this.actionSave = new System.Windows.Forms.ToolStripButton();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkRed,
            this.checkGreen,
            this.checkBlue,
            this.checkAlpha,
            this.toolStripSeparator1,
            this.actionSave});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(570, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip1";
			// 
			// checkAlpha
			// 
			this.checkAlpha.AutoToolTip = false;
			this.checkAlpha.Checked = true;
			this.checkAlpha.CheckOnClick = true;
			this.checkAlpha.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAlpha.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.checkAlpha.Image = ((System.Drawing.Image)(resources.GetObject("checkAlpha.Image")));
			this.checkAlpha.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.checkAlpha.Name = "checkAlpha";
			this.checkAlpha.Size = new System.Drawing.Size(23, 22);
			this.checkAlpha.Text = "A";
			this.checkAlpha.ToolTipText = "Display Alpha";
			this.checkAlpha.CheckedChanged += new System.EventHandler(this.checkAlpha_CheckedChanged);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// checkBlue
			// 
			this.checkBlue.AutoToolTip = false;
			this.checkBlue.Checked = true;
			this.checkBlue.CheckOnClick = true;
			this.checkBlue.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBlue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.checkBlue.Image = ((System.Drawing.Image)(resources.GetObject("checkBlue.Image")));
			this.checkBlue.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.checkBlue.Name = "checkBlue";
			this.checkBlue.Size = new System.Drawing.Size(23, 22);
			this.checkBlue.Text = "B";
			this.checkBlue.ToolTipText = "Display Blue";
			this.checkBlue.CheckedChanged += new System.EventHandler(this.checkBlue_CheckedChanged);
			// 
			// checkGreen
			// 
			this.checkGreen.AutoToolTip = false;
			this.checkGreen.Checked = true;
			this.checkGreen.CheckOnClick = true;
			this.checkGreen.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkGreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.checkGreen.Image = ((System.Drawing.Image)(resources.GetObject("checkGreen.Image")));
			this.checkGreen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.checkGreen.Name = "checkGreen";
			this.checkGreen.Size = new System.Drawing.Size(23, 22);
			this.checkGreen.Text = "G";
			this.checkGreen.ToolTipText = "Display Green";
			this.checkGreen.CheckedChanged += new System.EventHandler(this.checkGreen_CheckedChanged);
			// 
			// checkRed
			// 
			this.checkRed.AutoToolTip = false;
			this.checkRed.Checked = true;
			this.checkRed.CheckOnClick = true;
			this.checkRed.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkRed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.checkRed.Image = ((System.Drawing.Image)(resources.GetObject("checkRed.Image")));
			this.checkRed.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.checkRed.Name = "checkRed";
			this.checkRed.Size = new System.Drawing.Size(23, 22);
			this.checkRed.Text = "R";
			this.checkRed.ToolTipText = "Display Red";
			this.checkRed.CheckedChanged += new System.EventHandler(this.checkRed_CheckedChanged);
			// 
			// bitmapView
			// 
			this.bitmapView.AutoScroll = true;
			this.bitmapView.Bitmap = null;
			this.bitmapView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bitmapView.Location = new System.Drawing.Point(0, 25);
			this.bitmapView.Name = "bitmapView";
			this.bitmapView.Size = new System.Drawing.Size(570, 230);
			this.bitmapView.TabIndex = 0;
			this.bitmapView.UseAlpha = true;
			this.bitmapView.UseBlue = true;
			this.bitmapView.UseGreen = true;
			this.bitmapView.UseRed = true;
			// 
			// actionSave
			// 
			this.actionSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionSave.Image = global::Duality.VisualStudio.Properties.Resources.disk;
			this.actionSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionSave.Name = "actionSave";
			this.actionSave.Size = new System.Drawing.Size(23, 22);
			this.actionSave.Text = "Save as...";
			this.actionSave.Click += new System.EventHandler(this.actionSave_Click);
			// 
			// BitmapForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(570, 255);
			this.Controls.Add(this.bitmapView);
			this.Controls.Add(this.toolStrip);
			this.Name = "BitmapForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "BitmapForm";
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BitmapView bitmapView;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton checkAlpha;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton checkRed;
		private System.Windows.Forms.ToolStripButton checkGreen;
		private System.Windows.Forms.ToolStripButton checkBlue;
		private System.Windows.Forms.ToolStripButton actionSave;
	}
}