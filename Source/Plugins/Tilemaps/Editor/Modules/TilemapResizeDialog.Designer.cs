namespace Duality.Editor.Plugins.Tilemaps
{
	partial class TilemapResizeDialog
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
			this.originSelector = new AdamsLair.WinForms.OriginSelector();
			this.groupBoxSize = new System.Windows.Forms.GroupBox();
			this.tableLayoutMapSize = new System.Windows.Forms.TableLayoutPanel();
			this.editorHeight = new System.Windows.Forms.NumericUpDown();
			this.labelHeight = new System.Windows.Forms.Label();
			this.labelWidth = new System.Windows.Forms.Label();
			this.editorWidth = new System.Windows.Forms.NumericUpDown();
			this.labelHeader = new System.Windows.Forms.Label();
			this.labelMultiselect = new System.Windows.Forms.Label();
			this.panelBottomBack.SuspendLayout();
			this.groupBoxSize.SuspendLayout();
			this.tableLayoutMapSize.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.editorHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.editorWidth)).BeginInit();
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
			this.panelBottomBack.Location = new System.Drawing.Point(-1, 183);
			this.panelBottomBack.Name = "panelBottomBack";
			this.panelBottomBack.Size = new System.Drawing.Size(268, 58);
			this.panelBottomBack.TabIndex = 6;
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.Location = new System.Drawing.Point(96, 7);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 3;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(177, 7);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// originSelector
			// 
			this.originSelector.Location = new System.Drawing.Point(6, 19);
			this.originSelector.Name = "originSelector";
			this.originSelector.SelectedOrigin = AdamsLair.WinForms.OriginSelector.Origin.Center;
			this.originSelector.Size = new System.Drawing.Size(82, 82);
			this.originSelector.TabIndex = 7;
			// 
			// groupBoxSize
			// 
			this.groupBoxSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxSize.Controls.Add(this.tableLayoutMapSize);
			this.groupBoxSize.Controls.Add(this.originSelector);
			this.groupBoxSize.Location = new System.Drawing.Point(12, 60);
			this.groupBoxSize.Name = "groupBoxSize";
			this.groupBoxSize.Size = new System.Drawing.Size(240, 108);
			this.groupBoxSize.TabIndex = 8;
			this.groupBoxSize.TabStop = false;
			this.groupBoxSize.Text = "Map Size";
			// 
			// tableLayoutMapSize
			// 
			this.tableLayoutMapSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutMapSize.ColumnCount = 2;
			this.tableLayoutMapSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutMapSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutMapSize.Controls.Add(this.editorHeight, 1, 1);
			this.tableLayoutMapSize.Controls.Add(this.labelHeight, 0, 1);
			this.tableLayoutMapSize.Controls.Add(this.labelWidth, 0, 0);
			this.tableLayoutMapSize.Controls.Add(this.editorWidth, 1, 0);
			this.tableLayoutMapSize.Controls.Add(this.labelMultiselect, 0, 2);
			this.tableLayoutMapSize.Location = new System.Drawing.Point(94, 19);
			this.tableLayoutMapSize.Name = "tableLayoutMapSize";
			this.tableLayoutMapSize.RowCount = 3;
			this.tableLayoutMapSize.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutMapSize.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutMapSize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutMapSize.Size = new System.Drawing.Size(140, 83);
			this.tableLayoutMapSize.TabIndex = 8;
			// 
			// editorHeight
			// 
			this.editorHeight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editorHeight.Location = new System.Drawing.Point(44, 26);
			this.editorHeight.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.editorHeight.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.editorHeight.Name = "editorHeight";
			this.editorHeight.Size = new System.Drawing.Size(96, 20);
			this.editorHeight.TabIndex = 3;
			this.editorHeight.ValueChanged += new System.EventHandler(this.editorHeight_ValueChanged);
			// 
			// labelHeight
			// 
			this.labelHeight.AutoSize = true;
			this.labelHeight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelHeight.Location = new System.Drawing.Point(0, 23);
			this.labelHeight.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.labelHeight.Name = "labelHeight";
			this.labelHeight.Size = new System.Drawing.Size(41, 23);
			this.labelHeight.TabIndex = 1;
			this.labelHeight.Text = "Height:";
			this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelWidth
			// 
			this.labelWidth.AutoSize = true;
			this.labelWidth.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelWidth.Location = new System.Drawing.Point(0, 0);
			this.labelWidth.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(41, 23);
			this.labelWidth.TabIndex = 0;
			this.labelWidth.Text = "Width:";
			this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// editorWidth
			// 
			this.editorWidth.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editorWidth.Location = new System.Drawing.Point(44, 3);
			this.editorWidth.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.editorWidth.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.editorWidth.Name = "editorWidth";
			this.editorWidth.Size = new System.Drawing.Size(96, 20);
			this.editorWidth.TabIndex = 2;
			this.editorWidth.ValueChanged += new System.EventHandler(this.editorWidth_ValueChanged);
			// 
			// labelHeader
			// 
			this.labelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelHeader.AutoEllipsis = true;
			this.labelHeader.Location = new System.Drawing.Point(12, 9);
			this.labelHeader.Name = "labelHeader";
			this.labelHeader.Size = new System.Drawing.Size(240, 48);
			this.labelHeader.TabIndex = 9;
			this.labelHeader.Text = "Resizing the selected Tilemaps will preserve the part of their content that still" +
    " fits the new size and origin.";
			// 
			// labelMultiselect
			// 
			this.tableLayoutMapSize.SetColumnSpan(this.labelMultiselect, 2);
			this.labelMultiselect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelMultiselect.Enabled = false;
			this.labelMultiselect.Location = new System.Drawing.Point(3, 46);
			this.labelMultiselect.Name = "labelMultiselect";
			this.labelMultiselect.Size = new System.Drawing.Size(134, 37);
			this.labelMultiselect.TabIndex = 4;
			this.labelMultiselect.Text = "Multiple Tilemaps with differing sizes selected.";
			this.labelMultiselect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelMultiselect.Visible = false;
			// 
			// TilemapSetupDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(264, 223);
			this.Controls.Add(this.labelHeader);
			this.Controls.Add(this.groupBoxSize);
			this.Controls.Add(this.panelBottomBack);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(280, 260);
			this.Name = "TilemapSetupDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Resize Tilemap";
			this.panelBottomBack.ResumeLayout(false);
			this.groupBoxSize.ResumeLayout(false);
			this.tableLayoutMapSize.ResumeLayout(false);
			this.tableLayoutMapSize.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.editorHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.editorWidth)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelBottomBack;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private AdamsLair.WinForms.OriginSelector originSelector;
		private System.Windows.Forms.GroupBox groupBoxSize;
		private System.Windows.Forms.TableLayoutPanel tableLayoutMapSize;
		private System.Windows.Forms.NumericUpDown editorHeight;
		private System.Windows.Forms.Label labelHeight;
		private System.Windows.Forms.Label labelWidth;
		private System.Windows.Forms.NumericUpDown editorWidth;
		private System.Windows.Forms.Label labelHeader;
		private System.Windows.Forms.Label labelMultiselect;

	}
}