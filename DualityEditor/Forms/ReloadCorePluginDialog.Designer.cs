namespace Duality.Editor.Forms
{
	partial class ReloadCorePluginDialog
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
			this.components = new System.ComponentModel.Container();
			this.descLabel = new System.Windows.Forms.Label();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.progressTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// descLabel
			// 
			this.descLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.descLabel.Location = new System.Drawing.Point(12, 9);
			this.descLabel.Name = "descLabel";
			this.descLabel.Size = new System.Drawing.Size(260, 44);
			this.descLabel.TabIndex = 0;
			this.descLabel.Text = "Duality Core Plugins are being reloaded. Depending on the size of the currently l" +
    "oaded Scene, this may take some seconds.";
			this.descLabel.UseWaitCursor = true;
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(15, 62);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(260, 23);
			this.progressBar.TabIndex = 1;
			this.progressBar.UseWaitCursor = true;
			// 
			// progressTimer
			// 
			this.progressTimer.Interval = 50;
			this.progressTimer.Tick += new System.EventHandler(this.progressTimer_Tick);
			// 
			// ReloadCorePluginDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(284, 97);
			this.ControlBox = false;
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.descLabel);
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ReloadCorePluginDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Reloading Core Plugins...";
			this.UseWaitCursor = true;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label descLabel;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Timer progressTimer;
	}
}