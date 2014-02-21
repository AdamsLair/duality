namespace Duality.Editor.Plugins.HelpAdvisor
{
	partial class HelpAdvisor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpAdvisor));
			this.labelTopic = new System.Windows.Forms.Label();
			this.labelDescription = new System.Windows.Forms.Label();
			this.animTimer = new System.Windows.Forms.Timer(this.components);
			this.commitTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// labelTopic
			// 
			this.labelTopic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTopic.AutoEllipsis = true;
			this.labelTopic.BackColor = System.Drawing.Color.Transparent;
			this.labelTopic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTopic.Location = new System.Drawing.Point(4, 6);
			this.labelTopic.Margin = new System.Windows.Forms.Padding(0);
			this.labelTopic.Name = "labelTopic";
			this.labelTopic.Size = new System.Drawing.Size(242, 19);
			this.labelTopic.TabIndex = 0;
			// 
			// labelDescription
			// 
			this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDescription.AutoEllipsis = true;
			this.labelDescription.BackColor = System.Drawing.Color.Transparent;
			this.labelDescription.Location = new System.Drawing.Point(9, 28);
			this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(232, 33);
			this.labelDescription.TabIndex = 1;
			// 
			// animTimer
			// 
			this.animTimer.Interval = 15;
			this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
			// 
			// commitTimer
			// 
			this.commitTimer.Tick += new System.EventHandler(this.commitTimer_Tick);
			// 
			// HelpAdvisor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.ClientSize = new System.Drawing.Size(250, 70);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.labelTopic);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "HelpAdvisor";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
			this.Text = "Advisor";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labelTopic;
		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.Timer animTimer;
		private System.Windows.Forms.Timer commitTimer;
	}
}