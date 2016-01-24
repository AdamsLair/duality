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
			this.originSelector = new AdamsLair.WinForms.OriginSelector();
			this.groupBoxSize = new System.Windows.Forms.GroupBox();
			this.labelHeader = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.labelWidth = new System.Windows.Forms.Label();
			this.labelHeight = new System.Windows.Forms.Label();
			this.editorWidth = new System.Windows.Forms.NumericUpDown();
			this.editorHeight = new System.Windows.Forms.NumericUpDown();
			this.panelBottomBack.SuspendLayout();
			this.groupBoxSize.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.editorWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.editorHeight)).BeginInit();
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
			this.panelBottomBack.Location = new System.Drawing.Point(-1, 161);
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
			this.originSelector.Size = new System.Drawing.Size(76, 76);
			this.originSelector.TabIndex = 7;
			this.originSelector.Text = "button1";
			// 
			// groupBoxSize
			// 
			this.groupBoxSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxSize.Controls.Add(this.tableLayoutPanel1);
			this.groupBoxSize.Controls.Add(this.originSelector);
			this.groupBoxSize.Location = new System.Drawing.Point(12, 42);
			this.groupBoxSize.Name = "groupBoxSize";
			this.groupBoxSize.Size = new System.Drawing.Size(240, 104);
			this.groupBoxSize.TabIndex = 8;
			this.groupBoxSize.TabStop = false;
			this.groupBoxSize.Text = "Map Size";
			// 
			// labelHeader
			// 
			this.labelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelHeader.Location = new System.Drawing.Point(12, 9);
			this.labelHeader.Name = "labelHeader";
			this.labelHeader.Size = new System.Drawing.Size(240, 30);
			this.labelHeader.TabIndex = 9;
			this.labelHeader.Text = "ToDo: Some Header Text";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.editorHeight, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelHeight, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelWidth, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.editorWidth, 1, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(88, 19);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(146, 79);
			this.tableLayoutPanel1.TabIndex = 8;
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
			// editorWidth
			// 
			this.editorWidth.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editorWidth.Location = new System.Drawing.Point(44, 3);
			this.editorWidth.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.editorWidth.Name = "editorWidth";
			this.editorWidth.Size = new System.Drawing.Size(102, 20);
			this.editorWidth.TabIndex = 2;
			// 
			// editorHeight
			// 
			this.editorHeight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editorHeight.Location = new System.Drawing.Point(44, 26);
			this.editorHeight.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.editorHeight.Name = "editorHeight";
			this.editorHeight.Size = new System.Drawing.Size(102, 20);
			this.editorHeight.TabIndex = 3;
			// 
			// TilemapSetupDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(264, 201);
			this.Controls.Add(this.labelHeader);
			this.Controls.Add(this.groupBoxSize);
			this.Controls.Add(this.panelBottomBack);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(280, 240);
			this.Name = "TilemapSetupDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Tilemap Setup";
			this.panelBottomBack.ResumeLayout(false);
			this.groupBoxSize.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.editorWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.editorHeight)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelBottomBack;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private AdamsLair.WinForms.OriginSelector originSelector;
		private System.Windows.Forms.GroupBox groupBoxSize;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.NumericUpDown editorHeight;
		private System.Windows.Forms.Label labelHeight;
		private System.Windows.Forms.Label labelWidth;
		private System.Windows.Forms.NumericUpDown editorWidth;
		private System.Windows.Forms.Label labelHeader;

	}
}