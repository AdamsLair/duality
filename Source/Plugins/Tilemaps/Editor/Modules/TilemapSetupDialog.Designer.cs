using Duality.Editor.Controls;

namespace Duality.Editor.Plugins.Tilemaps
{
	partial class TilemapSetupDialog
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
			this.panelBottomBack = new System.Windows.Forms.Panel();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBoxSize = new System.Windows.Forms.GroupBox();
			this.settingsGrid = new Duality.Editor.Controls.DualitorPropertyGrid();
			this.labelDialogDesc = new System.Windows.Forms.Label();
			this.pictureBoxDialogIcon = new System.Windows.Forms.PictureBox();
			this.labelHeader = new System.Windows.Forms.Label();
			this.panelBottomBack.SuspendLayout();
			this.groupBoxSize.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxDialogIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// panelBottomBack
			// 
			this.panelBottomBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelBottomBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.panelBottomBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelBottomBack.Controls.Add(this.buttonOk);
			this.panelBottomBack.Controls.Add(this.buttonCancel);
			this.panelBottomBack.Location = new System.Drawing.Point(-1, 271);
			this.panelBottomBack.Name = "panelBottomBack";
			this.panelBottomBack.Size = new System.Drawing.Size(333, 58);
			this.panelBottomBack.TabIndex = 6;
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.Location = new System.Drawing.Point(161, 7);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 2;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(242, 7);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// groupBoxSize
			// 
			this.groupBoxSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxSize.Controls.Add(this.settingsGrid);
			this.groupBoxSize.Location = new System.Drawing.Point(12, 86);
			this.groupBoxSize.Name = "groupBoxSize";
			this.groupBoxSize.Padding = new System.Windows.Forms.Padding(5, 15, 5, 5);
			this.groupBoxSize.Size = new System.Drawing.Size(305, 170);
			this.groupBoxSize.TabIndex = 8;
			this.groupBoxSize.TabStop = false;
			this.groupBoxSize.Text = "Settings";
			// 
			// settingsGrid
			// 
			this.settingsGrid.AllowDrop = true;
			this.settingsGrid.AutoScroll = true;
			this.settingsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.settingsGrid.Location = new System.Drawing.Point(5, 28);
			this.settingsGrid.Name = "settingsGrid";
			this.settingsGrid.ReadOnly = false;
			this.settingsGrid.ShowNonPublic = false;
			this.settingsGrid.Size = new System.Drawing.Size(295, 137);
			this.settingsGrid.SplitterPosition = 118;
			this.settingsGrid.SplitterRatio = 0.4F;
			this.settingsGrid.TabIndex = 1;
			// 
			// labelDialogDesc
			// 
			this.labelDialogDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDialogDesc.AutoEllipsis = true;
			this.labelDialogDesc.Location = new System.Drawing.Point(69, 36);
			this.labelDialogDesc.Margin = new System.Windows.Forms.Padding(3);
			this.labelDialogDesc.Name = "labelDialogDesc";
			this.labelDialogDesc.Size = new System.Drawing.Size(248, 44);
			this.labelDialogDesc.TabIndex = 9;
			this.labelDialogDesc.Text = "This dialog will help you generate a set of layered Tilemaps and configure them a" +
    "s needed. You could do all of this manually, this is just a shortcut.\r\n";
			// 
			// pictureBoxDialogIcon
			// 
			this.pictureBoxDialogIcon.Image = global::Duality.Editor.Plugins.Tilemaps.Properties.Resources.IconSetupBig;
			this.pictureBoxDialogIcon.Location = new System.Drawing.Point(12, 9);
			this.pictureBoxDialogIcon.Name = "pictureBoxDialogIcon";
			this.pictureBoxDialogIcon.Size = new System.Drawing.Size(48, 48);
			this.pictureBoxDialogIcon.TabIndex = 10;
			this.pictureBoxDialogIcon.TabStop = false;
			// 
			// labelHeader
			// 
			this.labelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHeader.Location = new System.Drawing.Point(69, 9);
			this.labelHeader.Margin = new System.Windows.Forms.Padding(0);
			this.labelHeader.Name = "labelHeader";
			this.labelHeader.Size = new System.Drawing.Size(248, 22);
			this.labelHeader.TabIndex = 20;
			this.labelHeader.Text = "Setup Tilemap Layers";
			this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TilemapSetupDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(329, 311);
			this.Controls.Add(this.labelHeader);
			this.Controls.Add(this.pictureBoxDialogIcon);
			this.Controls.Add(this.labelDialogDesc);
			this.Controls.Add(this.groupBoxSize);
			this.Controls.Add(this.panelBottomBack);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(345, 350);
			this.Name = "TilemapSetupDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Create new layered Tilemaps";
			this.TopMost = true;
			this.panelBottomBack.ResumeLayout(false);
			this.groupBoxSize.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxDialogIcon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelBottomBack;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.GroupBox groupBoxSize;
		private System.Windows.Forms.Label labelDialogDesc;
		private Duality.Editor.Controls.DualitorPropertyGrid settingsGrid;
		private System.Windows.Forms.PictureBox pictureBoxDialogIcon;
		private System.Windows.Forms.Label labelHeader;

	}
}