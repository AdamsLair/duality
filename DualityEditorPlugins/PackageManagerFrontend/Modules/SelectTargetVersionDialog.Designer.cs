namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	partial class SelectTargetVersionDialog
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
			this.panelLowerArea = new System.Windows.Forms.Panel();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelHeader = new System.Windows.Forms.Label();
			this.targetVersion = new System.Windows.Forms.TextBox();
			this.labelTargetVersion = new System.Windows.Forms.Label();
			this.panelLowerArea.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelLowerArea
			// 
			this.panelLowerArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelLowerArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.panelLowerArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelLowerArea.Controls.Add(this.buttonOk);
			this.panelLowerArea.Controls.Add(this.buttonCancel);
			this.panelLowerArea.Location = new System.Drawing.Point(-5, 95);
			this.panelLowerArea.Name = "panelLowerArea";
			this.panelLowerArea.Size = new System.Drawing.Size(317, 39);
			this.panelLowerArea.TabIndex = 13;
			// 
			// buttonOk
			// 
			this.buttonOk.Enabled = false;
			this.buttonOk.Location = new System.Drawing.Point(145, 7);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 1;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(226, 7);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// labelHeader
			// 
			this.labelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelHeader.AutoEllipsis = true;
			this.labelHeader.Location = new System.Drawing.Point(12, 9);
			this.labelHeader.Name = "labelHeader";
			this.labelHeader.Size = new System.Drawing.Size(282, 49);
			this.labelHeader.TabIndex = 14;
			this.labelHeader.Text = "This dialog allows you to replace the selected Package with a specific version. Y" +
    "ou can use this to \"downgrade\" a Package that you accidentally updated.";
			// 
			// targetVersion
			// 
			this.targetVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.targetVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.targetVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.targetVersion.Location = new System.Drawing.Point(97, 61);
			this.targetVersion.Name = "targetVersion";
			this.targetVersion.Size = new System.Drawing.Size(197, 20);
			this.targetVersion.TabIndex = 15;
			this.targetVersion.TextChanged += new System.EventHandler(this.targetVersion_TextChanged);
			// 
			// labelTargetVersion
			// 
			this.labelTargetVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelTargetVersion.AutoSize = true;
			this.labelTargetVersion.Location = new System.Drawing.Point(12, 64);
			this.labelTargetVersion.Name = "labelTargetVersion";
			this.labelTargetVersion.Size = new System.Drawing.Size(79, 13);
			this.labelTargetVersion.TabIndex = 16;
			this.labelTargetVersion.Text = "Target Version:";
			// 
			// SelectTargetVersionDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(306, 132);
			this.Controls.Add(this.labelTargetVersion);
			this.Controls.Add(this.targetVersion);
			this.Controls.Add(this.labelHeader);
			this.Controls.Add(this.panelLowerArea);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectTargetVersionDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Target Version";
			this.panelLowerArea.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panelLowerArea;
		private System.Windows.Forms.Label labelHeader;
		private System.Windows.Forms.TextBox targetVersion;
		private System.Windows.Forms.Label labelTargetVersion;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
	}
}