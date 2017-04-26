namespace Duality.Editor.Plugins.CamView
{
	partial class GridSizeDialog
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
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.activeZ = new System.Windows.Forms.CheckBox();
			this.activeY = new System.Windows.Forms.CheckBox();
			this.editorZ = new System.Windows.Forms.NumericUpDown();
			this.editorY = new System.Windows.Forms.NumericUpDown();
			this.labelZ = new System.Windows.Forms.Label();
			this.labelY = new System.Windows.Forms.Label();
			this.labelX = new System.Windows.Forms.Label();
			this.editorX = new System.Windows.Forms.NumericUpDown();
			this.activeX = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.editorZ)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.editorY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.editorX)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(125, 92);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.Location = new System.Drawing.Point(44, 92);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 1;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel.ColumnCount = 3;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.Controls.Add(this.activeZ, 2, 2);
			this.tableLayoutPanel.Controls.Add(this.activeY, 2, 1);
			this.tableLayoutPanel.Controls.Add(this.editorZ, 1, 2);
			this.tableLayoutPanel.Controls.Add(this.editorY, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.labelZ, 0, 2);
			this.tableLayoutPanel.Controls.Add(this.labelY, 0, 1);
			this.tableLayoutPanel.Controls.Add(this.labelX, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.editorX, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.activeX, 2, 0);
			this.tableLayoutPanel.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 4;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(188, 74);
			this.tableLayoutPanel.TabIndex = 2;
			// 
			// activeZ
			// 
			this.activeZ.AutoSize = true;
			this.activeZ.Checked = true;
			this.activeZ.CheckState = System.Windows.Forms.CheckState.Checked;
			this.activeZ.Dock = System.Windows.Forms.DockStyle.Fill;
			this.activeZ.Location = new System.Drawing.Point(172, 45);
			this.activeZ.Margin = new System.Windows.Forms.Padding(1);
			this.activeZ.Name = "activeZ";
			this.activeZ.Size = new System.Drawing.Size(15, 20);
			this.activeZ.TabIndex = 8;
			this.activeZ.UseVisualStyleBackColor = true;
			this.activeZ.CheckedChanged += new System.EventHandler(this.activeZ_CheckedChanged);
			// 
			// activeY
			// 
			this.activeY.AutoSize = true;
			this.activeY.Checked = true;
			this.activeY.CheckState = System.Windows.Forms.CheckState.Checked;
			this.activeY.Dock = System.Windows.Forms.DockStyle.Fill;
			this.activeY.Location = new System.Drawing.Point(172, 23);
			this.activeY.Margin = new System.Windows.Forms.Padding(1);
			this.activeY.Name = "activeY";
			this.activeY.Size = new System.Drawing.Size(15, 20);
			this.activeY.TabIndex = 7;
			this.activeY.UseVisualStyleBackColor = true;
			this.activeY.CheckedChanged += new System.EventHandler(this.activeY_CheckedChanged);
			// 
			// editorZ
			// 
			this.editorZ.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editorZ.Location = new System.Drawing.Point(28, 45);
			this.editorZ.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
			this.editorZ.Maximum = new decimal(new int[] {
			1024,
			0,
			0,
			0});
			this.editorZ.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.editorZ.Name = "editorZ";
			this.editorZ.Size = new System.Drawing.Size(138, 20);
			this.editorZ.TabIndex = 5;
			this.editorZ.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			// 
			// editorY
			// 
			this.editorY.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editorY.Location = new System.Drawing.Point(28, 23);
			this.editorY.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
			this.editorY.Maximum = new decimal(new int[] {
			1024,
			0,
			0,
			0});
			this.editorY.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.editorY.Name = "editorY";
			this.editorY.Size = new System.Drawing.Size(138, 20);
			this.editorY.TabIndex = 4;
			this.editorY.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			// 
			// labelZ
			// 
			this.labelZ.AutoSize = true;
			this.labelZ.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelZ.Location = new System.Drawing.Point(3, 44);
			this.labelZ.Name = "labelZ";
			this.labelZ.Size = new System.Drawing.Size(17, 22);
			this.labelZ.TabIndex = 2;
			this.labelZ.Text = "Z:";
			this.labelZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelY
			// 
			this.labelY.AutoSize = true;
			this.labelY.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelY.Location = new System.Drawing.Point(3, 22);
			this.labelY.Name = "labelY";
			this.labelY.Size = new System.Drawing.Size(17, 22);
			this.labelY.TabIndex = 1;
			this.labelY.Text = "Y:";
			this.labelY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelX
			// 
			this.labelX.AutoSize = true;
			this.labelX.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelX.Location = new System.Drawing.Point(3, 0);
			this.labelX.Name = "labelX";
			this.labelX.Size = new System.Drawing.Size(17, 22);
			this.labelX.TabIndex = 0;
			this.labelX.Text = "X:";
			this.labelX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// editorX
			// 
			this.editorX.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editorX.Location = new System.Drawing.Point(28, 1);
			this.editorX.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
			this.editorX.Maximum = new decimal(new int[] {
			1024,
			0,
			0,
			0});
			this.editorX.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.editorX.Name = "editorX";
			this.editorX.Size = new System.Drawing.Size(138, 20);
			this.editorX.TabIndex = 3;
			this.editorX.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			// 
			// activeX
			// 
			this.activeX.AutoSize = true;
			this.activeX.Checked = true;
			this.activeX.CheckState = System.Windows.Forms.CheckState.Checked;
			this.activeX.Dock = System.Windows.Forms.DockStyle.Fill;
			this.activeX.Location = new System.Drawing.Point(172, 1);
			this.activeX.Margin = new System.Windows.Forms.Padding(1);
			this.activeX.Name = "activeX";
			this.activeX.Size = new System.Drawing.Size(15, 20);
			this.activeX.TabIndex = 6;
			this.activeX.UseVisualStyleBackColor = true;
			this.activeX.CheckedChanged += new System.EventHandler(this.activeX_CheckedChanged);
			// 
			// GridSizeDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(212, 127);
			this.Controls.Add(this.tableLayoutPanel);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GridSizeDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Grid Size";
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.editorZ)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.editorY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.editorX)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.CheckBox activeZ;
		private System.Windows.Forms.CheckBox activeY;
		private System.Windows.Forms.NumericUpDown editorZ;
		private System.Windows.Forms.NumericUpDown editorY;
		private System.Windows.Forms.Label labelZ;
		private System.Windows.Forms.Label labelY;
		private System.Windows.Forms.Label labelX;
		private System.Windows.Forms.NumericUpDown editorX;
		private System.Windows.Forms.CheckBox activeX;
	}
}