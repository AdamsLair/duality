namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	partial class PackageViewDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageViewDialog));
			this.packageList = new Aga.Controls.Tree.TreeViewAdv();
			this.treeColumnName = new Aga.Controls.Tree.TreeColumn();
			this.treeColumnVersion = new Aga.Controls.Tree.TreeColumn();
			this.treeColumnDownloads = new Aga.Controls.Tree.TreeColumn();
			this.nodeStateIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
			this.nodeTextBoxName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.nodeTextBoxVersion = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.nodeTextBoxDownloads = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.toolStripMain = new System.Windows.Forms.ToolStrip();
			this.toolStripSearchBox = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabelSearch = new System.Windows.Forms.ToolStripLabel();
			this.toolStripLabelFilter = new System.Windows.Forms.ToolStripLabel();
			this.toolStripFilterBox = new System.Windows.Forms.ToolStripComboBox();
			this.buttonClose = new System.Windows.Forms.Button();
			this.panelLowerArea = new System.Windows.Forms.Panel();
			this.miniToolStrip = new System.Windows.Forms.ToolStrip();
			this.labelHeaderText = new System.Windows.Forms.Label();
			this.labelHeader = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.toolStripMain.SuspendLayout();
			this.panelLowerArea.SuspendLayout();
			this.SuspendLayout();
			// 
			// packageList
			// 
			this.packageList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.packageList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.packageList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.packageList.Columns.Add(this.treeColumnName);
			this.packageList.Columns.Add(this.treeColumnVersion);
			this.packageList.Columns.Add(this.treeColumnDownloads);
			this.packageList.DefaultToolTipProvider = null;
			this.packageList.DragDropMarkColor = System.Drawing.Color.Black;
			this.packageList.FullRowSelect = true;
			this.packageList.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.packageList.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.packageList.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.packageList.Location = new System.Drawing.Point(0, 29);
			this.packageList.Model = null;
			this.packageList.Name = "packageList";
			this.packageList.NodeControls.Add(this.nodeStateIcon);
			this.packageList.NodeControls.Add(this.nodeTextBoxName);
			this.packageList.NodeControls.Add(this.nodeTextBoxVersion);
			this.packageList.NodeControls.Add(this.nodeTextBoxDownloads);
			this.packageList.NodeFilter = null;
			this.packageList.RowHeight = 32;
			this.packageList.SelectedNode = null;
			this.packageList.ShowLines = false;
			this.packageList.ShowNodeToolTips = true;
			this.packageList.ShowPlusMinus = false;
			this.packageList.Size = new System.Drawing.Size(478, 357);
			this.packageList.TabIndex = 0;
			this.packageList.Text = "packageList";
			this.packageList.UseColumns = true;
			// 
			// treeColumnName
			// 
			this.treeColumnName.Header = "Name";
			this.treeColumnName.Sortable = true;
			this.treeColumnName.SortOrder = System.Windows.Forms.SortOrder.Ascending;
			this.treeColumnName.TooltipText = null;
			this.treeColumnName.Width = 280;
			// 
			// treeColumnVersion
			// 
			this.treeColumnVersion.Header = "Version";
			this.treeColumnVersion.Sortable = true;
			this.treeColumnVersion.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumnVersion.TooltipText = null;
			this.treeColumnVersion.Width = 80;
			// 
			// treeColumnDownloads
			// 
			this.treeColumnDownloads.Header = "Downloads";
			this.treeColumnDownloads.Sortable = true;
			this.treeColumnDownloads.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumnDownloads.TooltipText = null;
			this.treeColumnDownloads.Width = 65;
			// 
			// nodeStateIcon
			// 
			this.nodeStateIcon.LeftMargin = 1;
			this.nodeStateIcon.ParentColumn = this.treeColumnName;
			this.nodeStateIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
			// 
			// nodeTextBoxName
			// 
			this.nodeTextBoxName.IncrementalSearchEnabled = true;
			this.nodeTextBoxName.LeftMargin = 3;
			this.nodeTextBoxName.ParentColumn = this.treeColumnName;
			// 
			// nodeTextBoxVersion
			// 
			this.nodeTextBoxVersion.IncrementalSearchEnabled = true;
			this.nodeTextBoxVersion.LeftMargin = 3;
			this.nodeTextBoxVersion.ParentColumn = this.treeColumnVersion;
			this.nodeTextBoxVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// nodeTextBoxDownloads
			// 
			this.nodeTextBoxDownloads.IncrementalSearchEnabled = true;
			this.nodeTextBoxDownloads.LeftMargin = 3;
			this.nodeTextBoxDownloads.ParentColumn = this.treeColumnDownloads;
			this.nodeTextBoxDownloads.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// splitMain
			// 
			this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitMain.Location = new System.Drawing.Point(3, 77);
			this.splitMain.Margin = new System.Windows.Forms.Padding(0);
			this.splitMain.Name = "splitMain";
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.packageList);
			this.splitMain.Panel1.Controls.Add(this.toolStripMain);
			this.splitMain.Panel2MinSize = 150;
			this.splitMain.Size = new System.Drawing.Size(666, 386);
			this.splitMain.SplitterDistance = 479;
			this.splitMain.TabIndex = 0;
			this.splitMain.TabStop = false;
			// 
			// toolStripMain
			// 
			this.toolStripMain.AutoSize = false;
			this.toolStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSearchBox,
            this.toolStripLabelSearch,
            this.toolStripLabelFilter,
            this.toolStripFilterBox});
			this.toolStripMain.Location = new System.Drawing.Point(0, 0);
			this.toolStripMain.Name = "toolStripMain";
			this.toolStripMain.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
			this.toolStripMain.Size = new System.Drawing.Size(479, 26);
			this.toolStripMain.TabIndex = 1;
			// 
			// toolStripSearchBox
			// 
			this.toolStripSearchBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripSearchBox.AutoSize = false;
			this.toolStripSearchBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.toolStripSearchBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.toolStripSearchBox.Name = "toolStripSearchBox";
			this.toolStripSearchBox.Size = new System.Drawing.Size(146, 23);
			// 
			// toolStripLabelSearch
			// 
			this.toolStripLabelSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripLabelSearch.Name = "toolStripLabelSearch";
			this.toolStripLabelSearch.Size = new System.Drawing.Size(45, 17);
			this.toolStripLabelSearch.Text = "Search:";
			// 
			// toolStripLabelFilter
			// 
			this.toolStripLabelFilter.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
			this.toolStripLabelFilter.Name = "toolStripLabelFilter";
			this.toolStripLabelFilter.Size = new System.Drawing.Size(52, 17);
			this.toolStripLabelFilter.Text = "Filter by:";
			// 
			// toolStripFilterBox
			// 
			this.toolStripFilterBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolStripFilterBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.toolStripFilterBox.Name = "toolStripFilterBox";
			this.toolStripFilterBox.Size = new System.Drawing.Size(121, 20);
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonClose.Location = new System.Drawing.Point(585, 7);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(75, 23);
			this.buttonClose.TabIndex = 11;
			this.buttonClose.Text = "Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			// 
			// panelLowerArea
			// 
			this.panelLowerArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelLowerArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.panelLowerArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelLowerArea.Controls.Add(this.buttonClose);
			this.panelLowerArea.Location = new System.Drawing.Point(-3, 466);
			this.panelLowerArea.Name = "panelLowerArea";
			this.panelLowerArea.Size = new System.Drawing.Size(672, 39);
			this.panelLowerArea.TabIndex = 12;
			// 
			// miniToolStrip
			// 
			this.miniToolStrip.AutoSize = false;
			this.miniToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.miniToolStrip.CanOverflow = false;
			this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.miniToolStrip.Location = new System.Drawing.Point(1, 2);
			this.miniToolStrip.Name = "miniToolStrip";
			this.miniToolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 1, 3);
			this.miniToolStrip.Size = new System.Drawing.Size(414, 26);
			this.miniToolStrip.TabIndex = 1;
			// 
			// labelHeaderText
			// 
			this.labelHeaderText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelHeaderText.Location = new System.Drawing.Point(82, 35);
			this.labelHeaderText.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.labelHeaderText.Name = "labelHeaderText";
			this.labelHeaderText.Size = new System.Drawing.Size(574, 42);
			this.labelHeaderText.TabIndex = 13;
			this.labelHeaderText.Text = "Each Duality project consists of multiple Packages that can carry plugins and dat" +
    "a. This dialog provides an overview of installed and available Packages and help" +
    "s you manage them.";
			// 
			// labelHeader
			// 
			this.labelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHeader.Location = new System.Drawing.Point(79, 7);
			this.labelHeader.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
			this.labelHeader.Name = "labelHeader";
			this.labelHeader.Size = new System.Drawing.Size(577, 22);
			this.labelHeader.TabIndex = 17;
			this.labelHeader.Text = "Manage Duality Packages";
			this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel1
			// 
			this.panel1.BackgroundImage = global::Duality.Editor.Plugins.PackageManagerFrontend.Properties.Resources.packagebig;
			this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.panel1.Location = new System.Drawing.Point(8, 7);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(68, 67);
			this.panel1.TabIndex = 18;
			// 
			// PackageViewDialog
			// 
			this.AcceptButton = this.buttonClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(668, 504);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.labelHeader);
			this.Controls.Add(this.labelHeaderText);
			this.Controls.Add(this.splitMain);
			this.Controls.Add(this.panelLowerArea);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(600, 400);
			this.Name = "PackageViewDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Package Management";
			this.splitMain.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.toolStripMain.ResumeLayout(false);
			this.toolStripMain.PerformLayout();
			this.panelLowerArea.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Aga.Controls.Tree.TreeViewAdv packageList;
		private Aga.Controls.Tree.NodeControls.NodeStateIcon nodeStateIcon;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxName;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Panel panelLowerArea;
		private System.Windows.Forms.ToolStrip miniToolStrip;
		private System.Windows.Forms.ToolStripTextBox toolStripSearchBox;
		private System.Windows.Forms.ToolStripLabel toolStripLabelSearch;
		private System.Windows.Forms.ToolStrip toolStripMain;
		private System.Windows.Forms.ToolStripLabel toolStripLabelFilter;
		private System.Windows.Forms.ToolStripComboBox toolStripFilterBox;
		private Aga.Controls.Tree.TreeColumn treeColumnName;
		private Aga.Controls.Tree.TreeColumn treeColumnVersion;
		private Aga.Controls.Tree.TreeColumn treeColumnDownloads;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxVersion;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxDownloads;
		private System.Windows.Forms.Label labelHeaderText;
		private System.Windows.Forms.Label labelHeader;
		private System.Windows.Forms.Panel panel1;
	}
}