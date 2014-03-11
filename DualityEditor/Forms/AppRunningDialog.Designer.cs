namespace Duality.Editor.Forms
{
	partial class AppRunningDialog
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
			this.timerProcessState = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// descLabel
			// 
			this.descLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.descLabel.Location = new System.Drawing.Point(12, 9);
			this.descLabel.Name = "descLabel";
			this.descLabel.Size = new System.Drawing.Size(260, 42);
			this.descLabel.TabIndex = 0;
			this.descLabel.Text = "A Duality application with access to project files is currently running. For stab" +
    "ility reasons, you can\'t edit the project until the application has quit.";
			this.descLabel.UseWaitCursor = true;
			// 
			// timerProcessState
			// 
			this.timerProcessState.Interval = 200;
			this.timerProcessState.Tick += new System.EventHandler(this.timerProcessState_Tick);
			// 
			// AppRunningDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(284, 60);
			this.ControlBox = false;
			this.Controls.Add(this.descLabel);
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AppRunningDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Duality Application running...";
			this.UseWaitCursor = true;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label descLabel;
		private System.Windows.Forms.Timer timerProcessState;
	}
}