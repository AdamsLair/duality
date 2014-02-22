namespace Duality.Editor.Forms
{
	partial class SplashScreen
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
			this.mainFormLoader = new System.ComponentModel.BackgroundWorker();
			this.logoPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// mainFormLoader
			// 
			this.mainFormLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.mainFormLoader_DoWork);
			this.mainFormLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.mainFormLoader_RunWorkerCompleted);
			// 
			// logoPanel
			// 
			this.logoPanel.BackColor = System.Drawing.Color.DarkGray;
			this.logoPanel.BackgroundImage = global::Duality.Editor.Properties.Resources.DualitorLogoHalfSize;
			this.logoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.logoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.logoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logoPanel.Location = new System.Drawing.Point(0, 0);
			this.logoPanel.Name = "logoPanel";
			this.logoPanel.Size = new System.Drawing.Size(700, 325);
			this.logoPanel.TabIndex = 0;
			// 
			// SplashScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(700, 325);
			this.Controls.Add(this.logoPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SplashScreen";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SplashScreen";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel logoPanel;
		private System.ComponentModel.BackgroundWorker mainFormLoader;

	}
}