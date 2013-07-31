namespace DualityEditor.Forms
{
	partial class ColorPickerDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPickerDialog));
			this.radioHue = new System.Windows.Forms.RadioButton();
			this.radioSaturation = new System.Windows.Forms.RadioButton();
			this.radioValue = new System.Windows.Forms.RadioButton();
			this.radioRed = new System.Windows.Forms.RadioButton();
			this.radioBlue = new System.Windows.Forms.RadioButton();
			this.radioGreen = new System.Windows.Forms.RadioButton();
			this.numHue = new System.Windows.Forms.NumericUpDown();
			this.numSaturation = new System.Windows.Forms.NumericUpDown();
			this.numValue = new System.Windows.Forms.NumericUpDown();
			this.numRed = new System.Windows.Forms.NumericUpDown();
			this.numGreen = new System.Windows.Forms.NumericUpDown();
			this.numBlue = new System.Windows.Forms.NumericUpDown();
			this.textBoxHex = new System.Windows.Forms.TextBox();
			this.labelHex = new System.Windows.Forms.Label();
			this.labelOld = new System.Windows.Forms.Label();
			this.labelNew = new System.Windows.Forms.Label();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.numAlpha = new System.Windows.Forms.NumericUpDown();
			this.labelAlpha = new System.Windows.Forms.Label();
			this.labelHueUnit = new System.Windows.Forms.Label();
			this.labelSaturationUnit = new System.Windows.Forms.Label();
			this.labelValueUnit = new System.Windows.Forms.Label();
			this.alphaSlider = new DualityEditor.Controls.ColorSlider();
			this.colorShowBox = new DualityEditor.Controls.ColorShowBox();
			this.colorSlider = new DualityEditor.Controls.ColorSlider();
			this.colorPanel = new DualityEditor.Controls.ColorPanel();
			((System.ComponentModel.ISupportInitialize)(this.numHue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numSaturation)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numValue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numRed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numGreen)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numBlue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numAlpha)).BeginInit();
			this.SuspendLayout();
			// 
			// radioHue
			// 
			this.radioHue.AutoSize = true;
			this.radioHue.Checked = true;
			this.radioHue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.radioHue.Location = new System.Drawing.Point(363, 82);
			this.radioHue.Name = "radioHue";
			this.radioHue.Size = new System.Drawing.Size(33, 17);
			this.radioHue.TabIndex = 3;
			this.radioHue.TabStop = true;
			this.radioHue.Text = "H";
			this.radioHue.UseVisualStyleBackColor = true;
			this.radioHue.CheckedChanged += new System.EventHandler(this.radioHue_CheckedChanged);
			// 
			// radioSaturation
			// 
			this.radioSaturation.AutoSize = true;
			this.radioSaturation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.radioSaturation.Location = new System.Drawing.Point(363, 105);
			this.radioSaturation.Name = "radioSaturation";
			this.radioSaturation.Size = new System.Drawing.Size(32, 17);
			this.radioSaturation.TabIndex = 4;
			this.radioSaturation.Text = "S";
			this.radioSaturation.UseVisualStyleBackColor = true;
			this.radioSaturation.CheckedChanged += new System.EventHandler(this.radioSaturation_CheckedChanged);
			// 
			// radioValue
			// 
			this.radioValue.AutoSize = true;
			this.radioValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.radioValue.Location = new System.Drawing.Point(363, 128);
			this.radioValue.Name = "radioValue";
			this.radioValue.Size = new System.Drawing.Size(32, 17);
			this.radioValue.TabIndex = 5;
			this.radioValue.Text = "V";
			this.radioValue.UseVisualStyleBackColor = true;
			this.radioValue.CheckedChanged += new System.EventHandler(this.radioValue_CheckedChanged);
			// 
			// radioRed
			// 
			this.radioRed.AutoSize = true;
			this.radioRed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.radioRed.Location = new System.Drawing.Point(363, 159);
			this.radioRed.Name = "radioRed";
			this.radioRed.Size = new System.Drawing.Size(33, 17);
			this.radioRed.TabIndex = 6;
			this.radioRed.Text = "R";
			this.radioRed.UseVisualStyleBackColor = true;
			this.radioRed.CheckedChanged += new System.EventHandler(this.radioRed_CheckedChanged);
			// 
			// radioBlue
			// 
			this.radioBlue.AutoSize = true;
			this.radioBlue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.radioBlue.Location = new System.Drawing.Point(363, 205);
			this.radioBlue.Name = "radioBlue";
			this.radioBlue.Size = new System.Drawing.Size(32, 17);
			this.radioBlue.TabIndex = 7;
			this.radioBlue.Text = "B";
			this.radioBlue.UseVisualStyleBackColor = true;
			this.radioBlue.CheckedChanged += new System.EventHandler(this.radioBlue_CheckedChanged);
			// 
			// radioGreen
			// 
			this.radioGreen.AutoSize = true;
			this.radioGreen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.radioGreen.Location = new System.Drawing.Point(363, 182);
			this.radioGreen.Name = "radioGreen";
			this.radioGreen.Size = new System.Drawing.Size(33, 17);
			this.radioGreen.TabIndex = 8;
			this.radioGreen.Text = "G";
			this.radioGreen.UseVisualStyleBackColor = true;
			this.radioGreen.CheckedChanged += new System.EventHandler(this.radioGreen_CheckedChanged);
			// 
			// numHue
			// 
			this.numHue.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.numHue.Location = new System.Drawing.Point(402, 82);
			this.numHue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
			this.numHue.Name = "numHue";
			this.numHue.Size = new System.Drawing.Size(54, 20);
			this.numHue.TabIndex = 9;
			this.numHue.ValueChanged += new System.EventHandler(this.numHue_ValueChanged);
			// 
			// numSaturation
			// 
			this.numSaturation.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numSaturation.Location = new System.Drawing.Point(402, 105);
			this.numSaturation.Name = "numSaturation";
			this.numSaturation.Size = new System.Drawing.Size(54, 20);
			this.numSaturation.TabIndex = 10;
			this.numSaturation.ValueChanged += new System.EventHandler(this.numSaturation_ValueChanged);
			// 
			// numValue
			// 
			this.numValue.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numValue.Location = new System.Drawing.Point(402, 128);
			this.numValue.Name = "numValue";
			this.numValue.Size = new System.Drawing.Size(54, 20);
			this.numValue.TabIndex = 11;
			this.numValue.ValueChanged += new System.EventHandler(this.numValue_ValueChanged);
			// 
			// numRed
			// 
			this.numRed.Location = new System.Drawing.Point(402, 159);
			this.numRed.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numRed.Name = "numRed";
			this.numRed.Size = new System.Drawing.Size(54, 20);
			this.numRed.TabIndex = 12;
			this.numRed.ValueChanged += new System.EventHandler(this.numRed_ValueChanged);
			// 
			// numGreen
			// 
			this.numGreen.Location = new System.Drawing.Point(402, 182);
			this.numGreen.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numGreen.Name = "numGreen";
			this.numGreen.Size = new System.Drawing.Size(54, 20);
			this.numGreen.TabIndex = 13;
			this.numGreen.ValueChanged += new System.EventHandler(this.numGreen_ValueChanged);
			// 
			// numBlue
			// 
			this.numBlue.Location = new System.Drawing.Point(402, 205);
			this.numBlue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numBlue.Name = "numBlue";
			this.numBlue.Size = new System.Drawing.Size(54, 20);
			this.numBlue.TabIndex = 14;
			this.numBlue.ValueChanged += new System.EventHandler(this.numBlue_ValueChanged);
			// 
			// textBoxHex
			// 
			this.textBoxHex.Location = new System.Drawing.Point(32, 279);
			this.textBoxHex.Name = "textBoxHex";
			this.textBoxHex.Size = new System.Drawing.Size(75, 20);
			this.textBoxHex.TabIndex = 16;
			this.textBoxHex.TextChanged += new System.EventHandler(this.textBoxHex_TextChanged);
			// 
			// labelHex
			// 
			this.labelHex.AutoSize = true;
			this.labelHex.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelHex.Location = new System.Drawing.Point(12, 282);
			this.labelHex.Name = "labelHex";
			this.labelHex.Size = new System.Drawing.Size(14, 13);
			this.labelHex.TabIndex = 17;
			this.labelHex.Text = "#";
			// 
			// labelOld
			// 
			this.labelOld.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelOld.Location = new System.Drawing.Point(423, 12);
			this.labelOld.Name = "labelOld";
			this.labelOld.Size = new System.Drawing.Size(33, 26);
			this.labelOld.TabIndex = 18;
			this.labelOld.Text = "Old";
			this.labelOld.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelNew
			// 
			this.labelNew.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelNew.Location = new System.Drawing.Point(423, 36);
			this.labelNew.Name = "labelNew";
			this.labelNew.Size = new System.Drawing.Size(33, 26);
			this.labelNew.TabIndex = 19;
			this.labelNew.Text = "New";
			this.labelNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.buttonCancel.Location = new System.Drawing.Point(379, 277);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(93, 23);
			this.buttonCancel.TabIndex = 20;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonOk
			// 
			this.buttonOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.buttonOk.Location = new System.Drawing.Point(280, 277);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(93, 23);
			this.buttonOk.TabIndex = 21;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// numAlpha
			// 
			this.numAlpha.Location = new System.Drawing.Point(402, 236);
			this.numAlpha.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numAlpha.Name = "numAlpha";
			this.numAlpha.Size = new System.Drawing.Size(54, 20);
			this.numAlpha.TabIndex = 22;
			this.numAlpha.ValueChanged += new System.EventHandler(this.numAlpha_ValueChanged);
			// 
			// labelAlpha
			// 
			this.labelAlpha.AutoSize = true;
			this.labelAlpha.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelAlpha.Location = new System.Drawing.Point(379, 238);
			this.labelAlpha.Name = "labelAlpha";
			this.labelAlpha.Size = new System.Drawing.Size(14, 13);
			this.labelAlpha.TabIndex = 23;
			this.labelAlpha.Text = "A";
			// 
			// labelHueUnit
			// 
			this.labelHueUnit.AutoSize = true;
			this.labelHueUnit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelHueUnit.Location = new System.Drawing.Point(461, 84);
			this.labelHueUnit.Name = "labelHueUnit";
			this.labelHueUnit.Size = new System.Drawing.Size(11, 13);
			this.labelHueUnit.TabIndex = 24;
			this.labelHueUnit.Text = "°";
			// 
			// labelSaturationUnit
			// 
			this.labelSaturationUnit.AutoSize = true;
			this.labelSaturationUnit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelSaturationUnit.Location = new System.Drawing.Point(462, 107);
			this.labelSaturationUnit.Name = "labelSaturationUnit";
			this.labelSaturationUnit.Size = new System.Drawing.Size(15, 13);
			this.labelSaturationUnit.TabIndex = 25;
			this.labelSaturationUnit.Text = "%";
			// 
			// labelValueUnit
			// 
			this.labelValueUnit.AutoSize = true;
			this.labelValueUnit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelValueUnit.Location = new System.Drawing.Point(461, 130);
			this.labelValueUnit.Name = "labelValueUnit";
			this.labelValueUnit.Size = new System.Drawing.Size(15, 13);
			this.labelValueUnit.TabIndex = 26;
			this.labelValueUnit.Text = "%";
			// 
			// alphaSlider
			// 
			this.alphaSlider.Location = new System.Drawing.Point(312, 7);
			this.alphaSlider.Maximum = System.Drawing.Color.White;
			this.alphaSlider.Minimum = System.Drawing.Color.Transparent;
			this.alphaSlider.Name = "alphaSlider";
			this.alphaSlider.Size = new System.Drawing.Size(32, 266);
			this.alphaSlider.TabIndex = 15;
			this.alphaSlider.PercentualValueChanged += new System.EventHandler(this.alphaSlider_PercentualValueChanged);
			// 
			// colorShowBox
			// 
			this.colorShowBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.colorShowBox.Color = System.Drawing.Color.DarkRed;
			this.colorShowBox.Location = new System.Drawing.Point(363, 12);
			this.colorShowBox.LowerColor = System.Drawing.Color.Maroon;
			this.colorShowBox.Name = "colorShowBox";
			this.colorShowBox.Size = new System.Drawing.Size(54, 50);
			this.colorShowBox.TabIndex = 2;
			this.colorShowBox.UpperColor = System.Drawing.Color.DarkRed;
			this.colorShowBox.UpperClick += new System.EventHandler(this.colorShowBox_UpperClick);
			// 
			// colorSlider
			// 
			this.colorSlider.Location = new System.Drawing.Point(274, 7);
			this.colorSlider.Name = "colorSlider";
			this.colorSlider.Size = new System.Drawing.Size(32, 266);
			this.colorSlider.TabIndex = 1;
			this.colorSlider.PercentualValueChanged += new System.EventHandler(this.colorSlider_PercentualValueChanged);
			// 
			// colorPanel
			// 
			this.colorPanel.BottomLeftColor = System.Drawing.Color.Black;
			this.colorPanel.BottomRightColor = System.Drawing.Color.Black;
			this.colorPanel.Location = new System.Drawing.Point(12, 12);
			this.colorPanel.Name = "colorPanel";
			this.colorPanel.Size = new System.Drawing.Size(256, 256);
			this.colorPanel.TabIndex = 0;
			this.colorPanel.TopLeftColor = System.Drawing.Color.White;
			this.colorPanel.TopRightColor = System.Drawing.Color.Red;
			this.colorPanel.ValuePercentual = ((System.Drawing.PointF)(resources.GetObject("colorPanel.ValuePercentual")));
			this.colorPanel.PercentualValueChanged += new System.EventHandler(this.colorPanel_PercentualValueChanged);
			// 
			// ColorPickerDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(484, 312);
			this.Controls.Add(this.labelValueUnit);
			this.Controls.Add(this.labelSaturationUnit);
			this.Controls.Add(this.labelHueUnit);
			this.Controls.Add(this.labelAlpha);
			this.Controls.Add(this.numAlpha);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.labelNew);
			this.Controls.Add(this.labelOld);
			this.Controls.Add(this.labelHex);
			this.Controls.Add(this.textBoxHex);
			this.Controls.Add(this.alphaSlider);
			this.Controls.Add(this.numBlue);
			this.Controls.Add(this.numGreen);
			this.Controls.Add(this.numRed);
			this.Controls.Add(this.numValue);
			this.Controls.Add(this.numSaturation);
			this.Controls.Add(this.numHue);
			this.Controls.Add(this.radioGreen);
			this.Controls.Add(this.radioBlue);
			this.Controls.Add(this.radioRed);
			this.Controls.Add(this.radioValue);
			this.Controls.Add(this.radioSaturation);
			this.Controls.Add(this.radioHue);
			this.Controls.Add(this.colorShowBox);
			this.Controls.Add(this.colorSlider);
			this.Controls.Add(this.colorPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ColorPickerDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Choose a new color";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ColorPickerDialog_FormClosed);
			((System.ComponentModel.ISupportInitialize)(this.numHue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numSaturation)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numValue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numRed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numGreen)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numBlue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numAlpha)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Controls.ColorPanel colorPanel;
		private Controls.ColorSlider colorSlider;
		private Controls.ColorShowBox colorShowBox;
		private System.Windows.Forms.RadioButton radioHue;
		private System.Windows.Forms.RadioButton radioSaturation;
		private System.Windows.Forms.RadioButton radioValue;
		private System.Windows.Forms.RadioButton radioRed;
		private System.Windows.Forms.RadioButton radioBlue;
		private System.Windows.Forms.RadioButton radioGreen;
		private System.Windows.Forms.NumericUpDown numHue;
		private System.Windows.Forms.NumericUpDown numSaturation;
		private System.Windows.Forms.NumericUpDown numValue;
		private System.Windows.Forms.NumericUpDown numRed;
		private System.Windows.Forms.NumericUpDown numGreen;
		private System.Windows.Forms.NumericUpDown numBlue;
		private Controls.ColorSlider alphaSlider;
		private System.Windows.Forms.TextBox textBoxHex;
		private System.Windows.Forms.Label labelHex;
		private System.Windows.Forms.Label labelOld;
		private System.Windows.Forms.Label labelNew;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.NumericUpDown numAlpha;
		private System.Windows.Forms.Label labelAlpha;
		private System.Windows.Forms.Label labelHueUnit;
		private System.Windows.Forms.Label labelSaturationUnit;
		private System.Windows.Forms.Label labelValueUnit;
	}
}