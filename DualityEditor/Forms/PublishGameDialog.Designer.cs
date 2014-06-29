namespace Duality.Editor.Forms {
	partial class PublishGameDialog {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishGameDialog));
			this.publishFolder = new System.Windows.Forms.FolderBrowserDialog();
			this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
			this.labelHeader = new System.Windows.Forms.Label();
			this.labelHeaderDescription = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonBrowse = new System.Windows.Forms.Button();
			this.groupOptions = new System.Windows.Forms.GroupBox();
			this.checkboxCompress = new System.Windows.Forms.CheckBox();
			this.checkboxEditor = new System.Windows.Forms.CheckBox();
			this.checkboxSource = new System.Windows.Forms.CheckBox();
			this.buttonPublish = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelGameTitle = new System.Windows.Forms.Label();
			this.textboxTitle = new System.Windows.Forms.TextBox();
			this.textboxFolderPath = new Duality.Editor.Controls.CueTextBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
			this.groupOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// publishFolder
			// 
			this.publishFolder.Description = "Select a folder to publish the game to";
			// 
			// pictureBoxLogo
			// 
			this.pictureBoxLogo.Image = global::Duality.Editor.Properties.Resources.DualityIcon48;
			this.pictureBoxLogo.Location = new System.Drawing.Point(9, 9);
			this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(0);
			this.pictureBoxLogo.Name = "pictureBoxLogo";
			this.pictureBoxLogo.Size = new System.Drawing.Size(48, 48);
			this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxLogo.TabIndex = 16;
			this.pictureBoxLogo.TabStop = false;
			// 
			// labelHeader
			// 
			this.labelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHeader.Location = new System.Drawing.Point(69, 9);
			this.labelHeader.Margin = new System.Windows.Forms.Padding(0);
			this.labelHeader.Name = "labelHeader";
			this.labelHeader.Size = new System.Drawing.Size(74, 22);
			this.labelHeader.TabIndex = 1;
			this.labelHeader.Text = "Publish Game";
			this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelHeaderDescription
			// 
			this.labelHeaderDescription.Location = new System.Drawing.Point(69, 31);
			this.labelHeaderDescription.Name = "labelHeaderDescription";
			this.labelHeaderDescription.Size = new System.Drawing.Size(290, 26);
			this.labelHeaderDescription.TabIndex = 2;
			this.labelHeaderDescription.Text = "Publishing your game will create a folder with just the files you need to share y" +
    "our game with others";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 82);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Publish Directory";
			// 
			// buttonBrowse
			// 
			this.buttonBrowse.Location = new System.Drawing.Point(274, 77);
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
			this.buttonBrowse.TabIndex = 5;
			this.buttonBrowse.Text = "Browse...";
			this.buttonBrowse.UseVisualStyleBackColor = true;
			this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
			// 
			// groupOptions
			// 
			this.groupOptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.groupOptions.Controls.Add(this.checkboxCompress);
			this.groupOptions.Controls.Add(this.checkboxEditor);
			this.groupOptions.Controls.Add(this.checkboxSource);
			this.groupOptions.Location = new System.Drawing.Point(17, 119);
			this.groupOptions.Name = "groupOptions";
			this.groupOptions.Size = new System.Drawing.Size(126, 100);
			this.groupOptions.TabIndex = 3;
			this.groupOptions.TabStop = false;
			this.groupOptions.Text = "Options";
			// 
			// checkboxCompress
			// 
			this.checkboxCompress.AutoSize = true;
			this.checkboxCompress.Location = new System.Drawing.Point(15, 22);
			this.checkboxCompress.Name = "checkboxCompress";
			this.checkboxCompress.Size = new System.Drawing.Size(104, 17);
			this.checkboxCompress.TabIndex = 0;
			this.checkboxCompress.Text = "Compress Folder";
			this.checkboxCompress.UseVisualStyleBackColor = true;
			// 
			// checkboxEditor
			// 
			this.checkboxEditor.AutoSize = true;
			this.checkboxEditor.Location = new System.Drawing.Point(15, 68);
			this.checkboxEditor.Name = "checkboxEditor";
			this.checkboxEditor.Size = new System.Drawing.Size(91, 17);
			this.checkboxEditor.TabIndex = 2;
			this.checkboxEditor.Text = "Include Editor";
			this.checkboxEditor.UseVisualStyleBackColor = true;
			// 
			// checkboxSource
			// 
			this.checkboxSource.AutoSize = true;
			this.checkboxSource.Location = new System.Drawing.Point(15, 45);
			this.checkboxSource.Name = "checkboxSource";
			this.checkboxSource.Size = new System.Drawing.Size(98, 17);
			this.checkboxSource.TabIndex = 1;
			this.checkboxSource.Text = "Include Source";
			this.checkboxSource.UseVisualStyleBackColor = true;
			// 
			// buttonPublish
			// 
			this.buttonPublish.Location = new System.Drawing.Point(193, 196);
			this.buttonPublish.Name = "buttonPublish";
			this.buttonPublish.Size = new System.Drawing.Size(75, 23);
			this.buttonPublish.TabIndex = 7;
			this.buttonPublish.Text = "Publish";
			this.buttonPublish.UseVisualStyleBackColor = true;
			this.buttonPublish.Click += new System.EventHandler(this.buttonPublish_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(274, 196);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// labelGameTitle
			// 
			this.labelGameTitle.AutoSize = true;
			this.labelGameTitle.Location = new System.Drawing.Point(161, 144);
			this.labelGameTitle.Name = "labelGameTitle";
			this.labelGameTitle.Size = new System.Drawing.Size(58, 13);
			this.labelGameTitle.TabIndex = 17;
			this.labelGameTitle.Text = "Game Title";
			// 
			// textboxTitle
			// 
			this.textboxTitle.Location = new System.Drawing.Point(225, 141);
			this.textboxTitle.Name = "textboxTitle";
			this.textboxTitle.Size = new System.Drawing.Size(123, 20);
			this.textboxTitle.TabIndex = 4;
			this.textboxTitle.Text = "Duality Game";
			// 
			// textboxFolderPath
			// 
			this.textboxFolderPath.CueText = "Select a folder to publish to";
			this.textboxFolderPath.Location = new System.Drawing.Point(104, 79);
			this.textboxFolderPath.Name = "textboxFolderPath";
			this.textboxFolderPath.Size = new System.Drawing.Size(164, 20);
			this.textboxFolderPath.TabIndex = 6;
			// 
			// PublishGameDialog
			// 
			this.AcceptButton = this.buttonPublish;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(363, 238);
			this.Controls.Add(this.textboxTitle);
			this.Controls.Add(this.labelGameTitle);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonPublish);
			this.Controls.Add(this.groupOptions);
			this.Controls.Add(this.buttonBrowse);
			this.Controls.Add(this.textboxFolderPath);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelHeaderDescription);
			this.Controls.Add(this.labelHeader);
			this.Controls.Add(this.pictureBoxLogo);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PublishGameDialog";
			this.Text = "Publish";
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
			this.groupOptions.ResumeLayout(false);
			this.groupOptions.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog publishFolder;
		private System.Windows.Forms.PictureBox pictureBoxLogo;
		private System.Windows.Forms.Label labelHeader;
		private System.Windows.Forms.Label labelHeaderDescription;
		private System.Windows.Forms.Label label1;
		private Controls.CueTextBox textboxFolderPath;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.GroupBox groupOptions;
		private System.Windows.Forms.CheckBox checkboxCompress;
		private System.Windows.Forms.CheckBox checkboxEditor;
		private System.Windows.Forms.CheckBox checkboxSource;
		private System.Windows.Forms.Button buttonPublish;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelGameTitle;
		private System.Windows.Forms.TextBox textboxTitle;

	}
}