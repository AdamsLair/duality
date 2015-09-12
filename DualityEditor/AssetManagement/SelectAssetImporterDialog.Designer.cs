namespace Duality.Editor.AssetManagement
{
	partial class SelectAssetImporterDialog
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
			this.labelInfo = new System.Windows.Forms.Label();
			this.importerView = new Aga.Controls.Tree.TreeViewAdv();
			this.importerTreeNodeStateIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
			this.importerTreeNodeName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.inputFileView = new Aga.Controls.Tree.TreeViewAdv();
			this.fileViewColumnName = new Aga.Controls.Tree.TreeColumn();
			this.fileTreeNodeStateIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
			this.fileTreeNodeName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.labelInputFileView = new System.Windows.Forms.Label();
			this.labelImporterView = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(223, 241);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.Location = new System.Drawing.Point(142, 241);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 3;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// labelInfo
			// 
			this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInfo.Location = new System.Drawing.Point(12, 9);
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Size = new System.Drawing.Size(286, 44);
			this.labelInfo.TabIndex = 6;
			this.labelInfo.Text = "Multiple Asset Importers are able to handle the specified input files. Please sel" +
    "ect the one to use.";
			// 
			// importerView
			// 
			this.importerView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.importerView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.importerView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.importerView.DefaultToolTipProvider = null;
			this.importerView.DragDropMarkColor = System.Drawing.Color.Black;
			this.importerView.FullRowSelect = true;
			this.importerView.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.importerView.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.importerView.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.importerView.Location = new System.Drawing.Point(12, 158);
			this.importerView.Model = null;
			this.importerView.Name = "importerView";
			this.importerView.NodeControls.Add(this.importerTreeNodeStateIcon);
			this.importerView.NodeControls.Add(this.importerTreeNodeName);
			this.importerView.NodeFilter = null;
			this.importerView.SelectedNode = null;
			this.importerView.ShowLines = false;
			this.importerView.ShowPlusMinus = false;
			this.importerView.Size = new System.Drawing.Size(286, 68);
			this.importerView.TabIndex = 5;
			// 
			// importerTreeNodeStateIcon
			// 
			this.importerTreeNodeStateIcon.DataPropertyName = "Image";
			this.importerTreeNodeStateIcon.LeftMargin = 1;
			this.importerTreeNodeStateIcon.ParentColumn = null;
			this.importerTreeNodeStateIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
			// 
			// importerTreeNodeName
			// 
			this.importerTreeNodeName.DataPropertyName = "Text";
			this.importerTreeNodeName.IncrementalSearchEnabled = true;
			this.importerTreeNodeName.LeftMargin = 3;
			this.importerTreeNodeName.ParentColumn = null;
			// 
			// inputFileView
			// 
			this.inputFileView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inputFileView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.inputFileView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.inputFileView.ColumnHeaderHeight = 1;
			this.inputFileView.Columns.Add(this.fileViewColumnName);
			this.inputFileView.DefaultToolTipProvider = null;
			this.inputFileView.DragDropMarkColor = System.Drawing.Color.Black;
			this.inputFileView.FullRowSelect = true;
			this.inputFileView.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.inputFileView.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.inputFileView.HideSelection = true;
			this.inputFileView.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.inputFileView.Location = new System.Drawing.Point(12, 74);
			this.inputFileView.Model = null;
			this.inputFileView.Name = "inputFileView";
			this.inputFileView.NodeControls.Add(this.fileTreeNodeStateIcon);
			this.inputFileView.NodeControls.Add(this.fileTreeNodeName);
			this.inputFileView.NodeFilter = null;
			this.inputFileView.SelectedNode = null;
			this.inputFileView.ShowLines = false;
			this.inputFileView.ShowPlusMinus = false;
			this.inputFileView.Size = new System.Drawing.Size(286, 60);
			this.inputFileView.TabIndex = 7;
			this.inputFileView.TabStop = false;
			this.inputFileView.UseColumns = true;
			this.inputFileView.Resize += new System.EventHandler(this.inputFileView_Resize);
			// 
			// fileViewColumnName
			// 
			this.fileViewColumnName.Header = "";
			this.fileViewColumnName.SortOrder = System.Windows.Forms.SortOrder.None;
			this.fileViewColumnName.TooltipText = null;
			// 
			// fileTreeNodeStateIcon
			// 
			this.fileTreeNodeStateIcon.DataPropertyName = "Image";
			this.fileTreeNodeStateIcon.LeftMargin = 1;
			this.fileTreeNodeStateIcon.ParentColumn = this.fileViewColumnName;
			this.fileTreeNodeStateIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
			// 
			// fileTreeNodeName
			// 
			this.fileTreeNodeName.DataPropertyName = "Text";
			this.fileTreeNodeName.IncrementalSearchEnabled = true;
			this.fileTreeNodeName.LeftMargin = 3;
			this.fileTreeNodeName.ParentColumn = this.fileViewColumnName;
			this.fileTreeNodeName.Trimming = System.Drawing.StringTrimming.EllipsisPath;
			// 
			// labelInputFileView
			// 
			this.labelInputFileView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInputFileView.Location = new System.Drawing.Point(12, 53);
			this.labelInputFileView.Name = "labelInputFileView";
			this.labelInputFileView.Size = new System.Drawing.Size(286, 18);
			this.labelInputFileView.TabIndex = 8;
			this.labelInputFileView.Text = "Input Files:";
			this.labelInputFileView.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelImporterView
			// 
			this.labelImporterView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelImporterView.Location = new System.Drawing.Point(12, 137);
			this.labelImporterView.Name = "labelImporterView";
			this.labelImporterView.Size = new System.Drawing.Size(286, 18);
			this.labelImporterView.TabIndex = 9;
			this.labelImporterView.Text = "Asset Importer to use:";
			this.labelImporterView.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// SelectAssetImporterDialog
			// 
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(310, 276);
			this.Controls.Add(this.labelImporterView);
			this.Controls.Add(this.labelInputFileView);
			this.Controls.Add(this.inputFileView);
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.importerView);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(235, 315);
			this.Name = "SelectAssetImporterDialog";
			this.ShowIcon = false;
			this.Text = "Select Asset Importer...";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Label labelInfo;
		private Aga.Controls.Tree.TreeViewAdv importerView;
		private Aga.Controls.Tree.TreeViewAdv inputFileView;
		private System.Windows.Forms.Label labelInputFileView;
		private System.Windows.Forms.Label labelImporterView;
		private Aga.Controls.Tree.NodeControls.NodeStateIcon importerTreeNodeStateIcon;
		private Aga.Controls.Tree.NodeControls.NodeTextBox importerTreeNodeName;
		private Aga.Controls.Tree.NodeControls.NodeStateIcon fileTreeNodeStateIcon;
		private Aga.Controls.Tree.NodeControls.NodeTextBox fileTreeNodeName;
		private Aga.Controls.Tree.TreeColumn fileViewColumnName;
	}
}