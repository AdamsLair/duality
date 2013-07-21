namespace ResourceHacker
{
	partial class RenameTypeDialog
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
			this.labelSearchFor = new System.Windows.Forms.Label();
			this.labelReplaceWith = new System.Windows.Forms.Label();
			this.textBoxReplaceWith = new System.Windows.Forms.TextBox();
			this.buttonAbort = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.comboBoxSearchFor = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// labelSearchFor
			// 
			this.labelSearchFor.AutoSize = true;
			this.labelSearchFor.Location = new System.Drawing.Point(12, 9);
			this.labelSearchFor.Name = "labelSearchFor";
			this.labelSearchFor.Size = new System.Drawing.Size(115, 13);
			this.labelSearchFor.TabIndex = 0;
			this.labelSearchFor.Text = "Search for Type name:";
			// 
			// labelReplaceWith
			// 
			this.labelReplaceWith.AutoSize = true;
			this.labelReplaceWith.Location = new System.Drawing.Point(12, 49);
			this.labelReplaceWith.Name = "labelReplaceWith";
			this.labelReplaceWith.Size = new System.Drawing.Size(128, 13);
			this.labelReplaceWith.TabIndex = 2;
			this.labelReplaceWith.Text = "Replace with Type name:";
			// 
			// textBoxReplaceWith
			// 
			this.textBoxReplaceWith.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxReplaceWith.Location = new System.Drawing.Point(15, 65);
			this.textBoxReplaceWith.Name = "textBoxReplaceWith";
			this.textBoxReplaceWith.Size = new System.Drawing.Size(317, 20);
			this.textBoxReplaceWith.TabIndex = 3;
			this.textBoxReplaceWith.TextChanged += new System.EventHandler(this.textBoxReplaceWith_TextChanged);
			// 
			// buttonAbort
			// 
			this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAbort.Location = new System.Drawing.Point(257, 97);
			this.buttonAbort.Name = "buttonAbort";
			this.buttonAbort.Size = new System.Drawing.Size(75, 23);
			this.buttonAbort.TabIndex = 4;
			this.buttonAbort.Text = "Cancel";
			this.buttonAbort.UseVisualStyleBackColor = true;
			this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.Location = new System.Drawing.Point(176, 97);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 5;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// comboBoxSearchFor
			// 
			this.comboBoxSearchFor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxSearchFor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.comboBoxSearchFor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.comboBoxSearchFor.Location = new System.Drawing.Point(15, 25);
			this.comboBoxSearchFor.Name = "comboBoxSearchFor";
			this.comboBoxSearchFor.Size = new System.Drawing.Size(317, 21);
			this.comboBoxSearchFor.TabIndex = 6;
			this.comboBoxSearchFor.TextChanged += new System.EventHandler(this.comboBoxSearchFor_TextChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label1.Location = new System.Drawing.Point(12, 94);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(158, 26);
			this.label1.TabIndex = 7;
			this.label1.Text = "Does not affect actual value Types of existing data.";
			// 
			// RenameTypeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(344, 132);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBoxSearchFor);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.buttonAbort);
			this.Controls.Add(this.textBoxReplaceWith);
			this.Controls.Add(this.labelReplaceWith);
			this.Controls.Add(this.labelSearchFor);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(800, 170);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(360, 170);
			this.Name = "RenameTypeDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Rename Type...";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelSearchFor;
		private System.Windows.Forms.Label labelReplaceWith;
		private System.Windows.Forms.TextBox textBoxReplaceWith;
		private System.Windows.Forms.Button buttonAbort;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.ComboBox comboBoxSearchFor;
		private System.Windows.Forms.Label label1;
	}
}