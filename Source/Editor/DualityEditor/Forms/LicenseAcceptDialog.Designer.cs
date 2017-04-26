namespace Duality.Editor.Forms
{
	partial class LicenseAcceptDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseAcceptDialog));
			this.labelDescription = new System.Windows.Forms.Label();
			this.textBoxLicenseText = new System.Windows.Forms.TextBox();
			this.linkLabelLicenseUrl = new System.Windows.Forms.LinkLabel();
			this.buttonDecline = new System.Windows.Forms.Button();
			this.buttonAgree = new System.Windows.Forms.Button();
			this.panelBottomBack = new System.Windows.Forms.Panel();
			this.labelTranscriptInfo = new System.Windows.Forms.Label();
			this.panelBottomBack.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelDescription
			// 
			this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.labelDescription.Location = new System.Drawing.Point(12, 9);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(397, 43);
			this.labelDescription.TabIndex = 0;
			this.labelDescription.Text = "To proceed with this operation, a license agreement is required.";
			// 
			// textBoxLicenseText
			// 
			this.textBoxLicenseText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxLicenseText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.textBoxLicenseText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxLicenseText.Location = new System.Drawing.Point(15, 55);
			this.textBoxLicenseText.Multiline = true;
			this.textBoxLicenseText.Name = "textBoxLicenseText";
			this.textBoxLicenseText.ReadOnly = true;
			this.textBoxLicenseText.Size = new System.Drawing.Size(394, 183);
			this.textBoxLicenseText.TabIndex = 4;
			this.textBoxLicenseText.TabStop = false;
			this.textBoxLicenseText.Text = "Retrieving License text transcript...";
			// 
			// linkLabelLicenseUrl
			// 
			this.linkLabelLicenseUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.linkLabelLicenseUrl.Enabled = false;
			this.linkLabelLicenseUrl.Location = new System.Drawing.Point(12, 312);
			this.linkLabelLicenseUrl.Name = "linkLabelLicenseUrl";
			this.linkLabelLicenseUrl.Size = new System.Drawing.Size(397, 39);
			this.linkLabelLicenseUrl.TabIndex = 1;
			this.linkLabelLicenseUrl.TabStop = true;
			this.linkLabelLicenseUrl.Text = "Click here to view the License";
			this.linkLabelLicenseUrl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.linkLabelLicenseUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLicenseUrl_LinkClicked);
			// 
			// buttonDecline
			// 
			this.buttonDecline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonDecline.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonDecline.Location = new System.Drawing.Point(337, 7);
			this.buttonDecline.Name = "buttonDecline";
			this.buttonDecline.Size = new System.Drawing.Size(75, 23);
			this.buttonDecline.TabIndex = 2;
			this.buttonDecline.Text = "Cancel";
			this.buttonDecline.UseVisualStyleBackColor = true;
			this.buttonDecline.Click += new System.EventHandler(this.buttonDecline_Click);
			// 
			// buttonAgree
			// 
			this.buttonAgree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAgree.Location = new System.Drawing.Point(256, 7);
			this.buttonAgree.Name = "buttonAgree";
			this.buttonAgree.Size = new System.Drawing.Size(75, 23);
			this.buttonAgree.TabIndex = 3;
			this.buttonAgree.Text = "I Accept";
			this.buttonAgree.UseVisualStyleBackColor = true;
			this.buttonAgree.Click += new System.EventHandler(this.buttonAgree_Click);
			// 
			// panelBottomBack
			// 
			this.panelBottomBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.panelBottomBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.panelBottomBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelBottomBack.Controls.Add(this.buttonAgree);
			this.panelBottomBack.Controls.Add(this.buttonDecline);
			this.panelBottomBack.Location = new System.Drawing.Point(-4, 354);
			this.panelBottomBack.Name = "panelBottomBack";
			this.panelBottomBack.Size = new System.Drawing.Size(428, 58);
			this.panelBottomBack.TabIndex = 5;
			// 
			// labelTranscriptInfo
			// 
			this.labelTranscriptInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.labelTranscriptInfo.Enabled = false;
			this.labelTranscriptInfo.Location = new System.Drawing.Point(12, 241);
			this.labelTranscriptInfo.Name = "labelTranscriptInfo";
			this.labelTranscriptInfo.Size = new System.Drawing.Size(397, 71);
			this.labelTranscriptInfo.TabIndex = 6;
			this.labelTranscriptInfo.Text = resources.GetString("labelTranscriptInfo.Text");
			this.labelTranscriptInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LicenseAcceptDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(421, 392);
			this.ControlBox = false;
			this.Controls.Add(this.labelTranscriptInfo);
			this.Controls.Add(this.linkLabelLicenseUrl);
			this.Controls.Add(this.textBoxLicenseText);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.panelBottomBack);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LicenseAcceptDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "License Agreement";
			this.panelBottomBack.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.TextBox textBoxLicenseText;
		private System.Windows.Forms.LinkLabel linkLabelLicenseUrl;
		private System.Windows.Forms.Button buttonDecline;
		private System.Windows.Forms.Button buttonAgree;
		private System.Windows.Forms.Panel panelBottomBack;
		private System.Windows.Forms.Label labelTranscriptInfo;
	}
}