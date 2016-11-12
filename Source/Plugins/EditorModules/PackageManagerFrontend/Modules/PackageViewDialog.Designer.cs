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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageViewDialog));
			this.packageList = new Aga.Controls.Tree.TreeViewAdv();
			this.treeColumnName = new Aga.Controls.Tree.TreeColumn();
			this.treeColumnVersion = new Aga.Controls.Tree.TreeColumn();
			this.treeColumnDate = new Aga.Controls.Tree.TreeColumn();
			this.treeColumnDownloads = new Aga.Controls.Tree.TreeColumn();
			this.treeColumnPackageType = new Aga.Controls.Tree.TreeColumn();
			this.nodeIcon = new Aga.Controls.Tree.NodeControls.NodeIcon();
			this.nodeTextBoxName = new Duality.Editor.Plugins.PackageManagerFrontend.DualityPackageSummaryNodeControl();
			this.nodeTextBoxVersion = new Duality.Editor.Plugins.PackageManagerFrontend.DualityPackageVersionNodeControl();
			this.nodeTextBoxDate = new Duality.Editor.Plugins.PackageManagerFrontend.DualityPackageDateNodeControl();
			this.nodeTextBoxDownloads = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.nodeIconPackageType = new Duality.Editor.Plugins.PackageManagerFrontend.DualityPackageTypeNodeControl();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.toolStripMain = new System.Windows.Forms.ToolStrip();
			this.toolStripSearchBox = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabelSearch = new System.Windows.Forms.ToolStripLabel();
			this.toolStripLabelView = new System.Windows.Forms.ToolStripLabel();
			this.toolStripViewBox = new System.Windows.Forms.ToolStripComboBox();
			this.tableLayoutPanelInfo = new System.Windows.Forms.TableLayoutPanel();
			this.labelPackageVersion = new System.Windows.Forms.Label();
			this.labelPackageUpdatedCaption = new System.Windows.Forms.Label();
			this.labelPackageUpdated = new System.Windows.Forms.Label();
			this.labelPackageVersionCaption = new System.Windows.Forms.Label();
			this.labelPackageTags = new System.Windows.Forms.Label();
			this.labelPackageTagsCaption = new System.Windows.Forms.Label();
			this.labelPackageTitle = new System.Windows.Forms.Label();
			this.labelPackageId = new System.Windows.Forms.Label();
			this.labelPackageDesc = new System.Windows.Forms.Label();
			this.labelPackageWebsiteCaption = new System.Windows.Forms.Label();
			this.labelPackageWebsite = new System.Windows.Forms.LinkLabel();
			this.labelPackageAuthorCaption = new System.Windows.Forms.Label();
			this.labelPackageAuthor = new System.Windows.Forms.Label();
			this.labelPackageLicenseCaption = new System.Windows.Forms.Label();
			this.labelPackageLicense = new System.Windows.Forms.LinkLabel();
			this.textBoxReleaseNotes = new System.Windows.Forms.TextBox();
			this.labelReleaseNotesCaption = new System.Windows.Forms.Label();
			this.buttonClose = new System.Windows.Forms.Button();
			this.panelLowerArea = new System.Windows.Forms.Panel();
			this.buttonAdvanced = new System.Windows.Forms.Button();
			this.labelRequireRestart = new System.Windows.Forms.Label();
			this.flowLayoutBottom = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonApply = new System.Windows.Forms.Button();
			this.bottomFlowSpacer1 = new System.Windows.Forms.Label();
			this.buttonUninstall = new System.Windows.Forms.Button();
			this.buttonInstall = new System.Windows.Forms.Button();
			this.buttonUpdate = new System.Windows.Forms.Button();
			this.bottomFlowSpacer2 = new System.Windows.Forms.Label();
			this.buttonUpdateAll = new System.Windows.Forms.Button();
			this.miniToolStrip = new System.Windows.Forms.ToolStrip();
			this.labelHeaderText = new System.Windows.Forms.Label();
			this.labelHeader = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.timerPackageModelChanged = new System.Windows.Forms.Timer(this.components);
			this.panelTitleImage = new System.Windows.Forms.Panel();
			this.contextMenuAdvanced = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.itemReInstallAll = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.toolStripMain.SuspendLayout();
			this.tableLayoutPanelInfo.SuspendLayout();
			this.panelLowerArea.SuspendLayout();
			this.flowLayoutBottom.SuspendLayout();
			this.contextMenuAdvanced.SuspendLayout();
			this.SuspendLayout();
			// 
			// packageList
			// 
			this.packageList.AllowColumnReorder = true;
			this.packageList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.packageList.AsyncExpanding = true;
			this.packageList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.packageList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.packageList.Columns.Add(this.treeColumnName);
			this.packageList.Columns.Add(this.treeColumnVersion);
			this.packageList.Columns.Add(this.treeColumnDate);
			this.packageList.Columns.Add(this.treeColumnDownloads);
			this.packageList.Columns.Add(this.treeColumnPackageType);
			this.packageList.DefaultToolTipProvider = null;
			this.packageList.DragDropMarkColor = System.Drawing.Color.Black;
			this.packageList.FullRowSelect = true;
			this.packageList.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.packageList.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.packageList.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.packageList.LoadOnDemand = true;
			this.packageList.Location = new System.Drawing.Point(0, 29);
			this.packageList.Model = null;
			this.packageList.Name = "packageList";
			this.packageList.NodeControls.Add(this.nodeIcon);
			this.packageList.NodeControls.Add(this.nodeTextBoxName);
			this.packageList.NodeControls.Add(this.nodeTextBoxVersion);
			this.packageList.NodeControls.Add(this.nodeTextBoxDate);
			this.packageList.NodeControls.Add(this.nodeTextBoxDownloads);
			this.packageList.NodeControls.Add(this.nodeIconPackageType);
			this.packageList.NodeFilter = null;
			this.packageList.RowHeight = 48;
			this.packageList.SelectedNode = null;
			this.packageList.ShowLines = false;
			this.packageList.ShowNodeToolTips = true;
			this.packageList.ShowPlusMinus = false;
			this.packageList.Size = new System.Drawing.Size(602, 363);
			this.packageList.TabIndex = 0;
			this.packageList.Text = "packageList";
			this.packageList.UseColumns = true;
			this.packageList.ColumnClicked += new System.EventHandler<Aga.Controls.Tree.TreeColumnEventArgs>(this.packageList_ColumnClicked);
			this.packageList.SelectionChanged += new System.EventHandler(this.packageList_SelectionChanged);
			this.packageList.Resize += new System.EventHandler(this.packageList_Resize);
			// 
			// treeColumnName
			// 
			this.treeColumnName.Header = "Name";
			this.treeColumnName.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumnName.TooltipText = null;
			this.treeColumnName.Width = 455;
			// 
			// treeColumnVersion
			// 
			this.treeColumnVersion.Header = "Version";
			this.treeColumnVersion.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumnVersion.TooltipText = null;
			this.treeColumnVersion.Width = 80;
			// 
			// treeColumnDate
			// 
			this.treeColumnDate.Header = "Date";
			this.treeColumnDate.IsVisible = false;
			this.treeColumnDate.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumnDate.TooltipText = null;
			this.treeColumnDate.Width = 80;
			// 
			// treeColumnDownloads
			// 
			this.treeColumnDownloads.Header = "Downloads";
			this.treeColumnDownloads.IsVisible = false;
			this.treeColumnDownloads.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumnDownloads.TooltipText = null;
			this.treeColumnDownloads.Width = 65;
			// 
			// treeColumnPackageType
			// 
			this.treeColumnPackageType.Header = "Type";
			this.treeColumnPackageType.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumnPackageType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.treeColumnPackageType.TooltipText = null;
			this.treeColumnPackageType.Width = 40;
			// 
			// nodeIcon
			// 
			this.nodeIcon.DataPropertyName = "Icon";
			this.nodeIcon.LeftMargin = 1;
			this.nodeIcon.ParentColumn = this.treeColumnName;
			this.nodeIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
			// 
			// nodeTextBoxName
			// 
			this.nodeTextBoxName.LeftMargin = 6;
			this.nodeTextBoxName.ParentColumn = this.treeColumnName;
			// 
			// nodeTextBoxVersion
			// 
			this.nodeTextBoxVersion.LeftMargin = 3;
			this.nodeTextBoxVersion.PackageManager = null;
			this.nodeTextBoxVersion.ParentColumn = this.treeColumnVersion;
			// 
			// nodeTextBoxDate
			// 
			this.nodeTextBoxDate.LeftMargin = 3;
			this.nodeTextBoxDate.PackageManager = null;
			this.nodeTextBoxDate.ParentColumn = this.treeColumnDate;
			// 
			// nodeTextBoxDownloads
			// 
			this.nodeTextBoxDownloads.DataPropertyName = "Downloads";
			this.nodeTextBoxDownloads.IncrementalSearchEnabled = true;
			this.nodeTextBoxDownloads.LeftMargin = 3;
			this.nodeTextBoxDownloads.ParentColumn = this.treeColumnDownloads;
			this.nodeTextBoxDownloads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.nodeTextBoxDownloads.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
			// 
			// nodeIconPackageType
			// 
			this.nodeIconPackageType.LeftMargin = 0;
			this.nodeIconPackageType.ParentColumn = this.treeColumnPackageType;
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
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.tableLayoutPanelInfo);
			this.splitMain.Panel2MinSize = 150;
			this.splitMain.Size = new System.Drawing.Size(895, 392);
			this.splitMain.SplitterDistance = 602;
			this.splitMain.TabIndex = 0;
			this.splitMain.TabStop = false;
			// 
			// toolStripMain
			// 
			this.toolStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSearchBox,
            this.toolStripLabelSearch,
            this.toolStripLabelView,
            this.toolStripViewBox});
			this.toolStripMain.Location = new System.Drawing.Point(0, 0);
			this.toolStripMain.Name = "toolStripMain";
			this.toolStripMain.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
			this.toolStripMain.Size = new System.Drawing.Size(602, 29);
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
			this.toolStripSearchBox.TextChanged += new System.EventHandler(this.toolStripSearchBox_TextChanged);
			// 
			// toolStripLabelSearch
			// 
			this.toolStripLabelSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripLabelSearch.Name = "toolStripLabelSearch";
			this.toolStripLabelSearch.Size = new System.Drawing.Size(45, 20);
			this.toolStripLabelSearch.Text = "Search:";
			// 
			// toolStripLabelView
			// 
			this.toolStripLabelView.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
			this.toolStripLabelView.Name = "toolStripLabelView";
			this.toolStripLabelView.Size = new System.Drawing.Size(35, 20);
			this.toolStripLabelView.Text = "View:";
			// 
			// toolStripViewBox
			// 
			this.toolStripViewBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolStripViewBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.toolStripViewBox.Name = "toolStripViewBox";
			this.toolStripViewBox.Size = new System.Drawing.Size(121, 23);
			this.toolStripViewBox.DropDownClosed += new System.EventHandler(this.toolStripViewBox_DropDownClosed);
			this.toolStripViewBox.SelectedIndexChanged += new System.EventHandler(this.toolStripViewBox_SelectedIndexChanged);
			// 
			// tableLayoutPanelInfo
			// 
			this.tableLayoutPanelInfo.ColumnCount = 2;
			this.tableLayoutPanelInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanelInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageVersion, 1, 9);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageUpdatedCaption, 0, 10);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageUpdated, 1, 10);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageVersionCaption, 0, 9);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageTags, 1, 8);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageTagsCaption, 0, 8);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageTitle, 0, 0);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageId, 0, 1);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageDesc, 0, 2);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageWebsiteCaption, 0, 5);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageWebsite, 1, 5);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageAuthorCaption, 0, 7);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageAuthor, 1, 7);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageLicenseCaption, 0, 6);
			this.tableLayoutPanelInfo.Controls.Add(this.labelPackageLicense, 1, 6);
			this.tableLayoutPanelInfo.Controls.Add(this.textBoxReleaseNotes, 0, 4);
			this.tableLayoutPanelInfo.Controls.Add(this.labelReleaseNotesCaption, 0, 3);
			this.tableLayoutPanelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanelInfo.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanelInfo.Name = "tableLayoutPanelInfo";
			this.tableLayoutPanelInfo.RowCount = 11;
			this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanelInfo.Size = new System.Drawing.Size(289, 392);
			this.tableLayoutPanelInfo.TabIndex = 0;
			// 
			// labelPackageVersion
			// 
			this.labelPackageVersion.AutoSize = true;
			this.labelPackageVersion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageVersion.Location = new System.Drawing.Point(60, 357);
			this.labelPackageVersion.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageVersion.Name = "labelPackageVersion";
			this.labelPackageVersion.Size = new System.Drawing.Size(226, 13);
			this.labelPackageVersion.TabIndex = 12;
			this.labelPackageVersion.Text = "1.0.0.0";
			this.labelPackageVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelPackageUpdatedCaption
			// 
			this.labelPackageUpdatedCaption.AutoSize = true;
			this.labelPackageUpdatedCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageUpdatedCaption.Location = new System.Drawing.Point(3, 376);
			this.labelPackageUpdatedCaption.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageUpdatedCaption.Name = "labelPackageUpdatedCaption";
			this.labelPackageUpdatedCaption.Size = new System.Drawing.Size(51, 13);
			this.labelPackageUpdatedCaption.TabIndex = 11;
			this.labelPackageUpdatedCaption.Text = "Updated:";
			this.labelPackageUpdatedCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPackageUpdated
			// 
			this.labelPackageUpdated.AutoSize = true;
			this.labelPackageUpdated.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageUpdated.Location = new System.Drawing.Point(60, 376);
			this.labelPackageUpdated.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageUpdated.Name = "labelPackageUpdated";
			this.labelPackageUpdated.Size = new System.Drawing.Size(226, 13);
			this.labelPackageUpdated.TabIndex = 10;
			this.labelPackageUpdated.Text = "1900-01-01";
			this.labelPackageUpdated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelPackageVersionCaption
			// 
			this.labelPackageVersionCaption.AutoSize = true;
			this.labelPackageVersionCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageVersionCaption.Location = new System.Drawing.Point(3, 357);
			this.labelPackageVersionCaption.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageVersionCaption.Name = "labelPackageVersionCaption";
			this.labelPackageVersionCaption.Size = new System.Drawing.Size(51, 13);
			this.labelPackageVersionCaption.TabIndex = 9;
			this.labelPackageVersionCaption.Text = "Version:";
			this.labelPackageVersionCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPackageTags
			// 
			this.labelPackageTags.AutoSize = true;
			this.labelPackageTags.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageTags.Location = new System.Drawing.Point(60, 338);
			this.labelPackageTags.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageTags.Name = "labelPackageTags";
			this.labelPackageTags.Size = new System.Drawing.Size(226, 13);
			this.labelPackageTags.TabIndex = 8;
			this.labelPackageTags.Text = "Tag1, Tag2, Tag3";
			this.labelPackageTags.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelPackageTagsCaption
			// 
			this.labelPackageTagsCaption.AutoSize = true;
			this.labelPackageTagsCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageTagsCaption.Location = new System.Drawing.Point(3, 338);
			this.labelPackageTagsCaption.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageTagsCaption.Name = "labelPackageTagsCaption";
			this.labelPackageTagsCaption.Size = new System.Drawing.Size(51, 13);
			this.labelPackageTagsCaption.TabIndex = 7;
			this.labelPackageTagsCaption.Text = "Tags:";
			this.labelPackageTagsCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPackageTitle
			// 
			this.labelPackageTitle.AutoSize = true;
			this.tableLayoutPanelInfo.SetColumnSpan(this.labelPackageTitle, 2);
			this.labelPackageTitle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPackageTitle.Location = new System.Drawing.Point(3, 3);
			this.labelPackageTitle.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageTitle.Name = "labelPackageTitle";
			this.labelPackageTitle.Size = new System.Drawing.Size(283, 18);
			this.labelPackageTitle.TabIndex = 0;
			this.labelPackageTitle.Text = "Package Title";
			// 
			// labelPackageId
			// 
			this.labelPackageId.AutoEllipsis = true;
			this.labelPackageId.AutoSize = true;
			this.tableLayoutPanelInfo.SetColumnSpan(this.labelPackageId, 2);
			this.labelPackageId.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageId.ForeColor = System.Drawing.SystemColors.GrayText;
			this.labelPackageId.Location = new System.Drawing.Point(3, 25);
			this.labelPackageId.Margin = new System.Windows.Forms.Padding(3, 1, 3, 12);
			this.labelPackageId.Name = "labelPackageId";
			this.labelPackageId.Size = new System.Drawing.Size(283, 13);
			this.labelPackageId.TabIndex = 1;
			this.labelPackageId.Text = "Package Id";
			// 
			// labelPackageDesc
			// 
			this.labelPackageDesc.AutoEllipsis = true;
			this.labelPackageDesc.AutoSize = true;
			this.tableLayoutPanelInfo.SetColumnSpan(this.labelPackageDesc, 2);
			this.labelPackageDesc.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageDesc.Location = new System.Drawing.Point(3, 50);
			this.labelPackageDesc.Name = "labelPackageDesc";
			this.labelPackageDesc.Size = new System.Drawing.Size(283, 102);
			this.labelPackageDesc.TabIndex = 2;
			this.labelPackageDesc.Text = "This area contains the complete package description.";
			// 
			// labelPackageWebsiteCaption
			// 
			this.labelPackageWebsiteCaption.AutoSize = true;
			this.labelPackageWebsiteCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageWebsiteCaption.Location = new System.Drawing.Point(3, 281);
			this.labelPackageWebsiteCaption.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageWebsiteCaption.Name = "labelPackageWebsiteCaption";
			this.labelPackageWebsiteCaption.Size = new System.Drawing.Size(51, 13);
			this.labelPackageWebsiteCaption.TabIndex = 3;
			this.labelPackageWebsiteCaption.Text = "Website:";
			this.labelPackageWebsiteCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPackageWebsite
			// 
			this.labelPackageWebsite.AutoSize = true;
			this.labelPackageWebsite.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageWebsite.Location = new System.Drawing.Point(60, 281);
			this.labelPackageWebsite.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageWebsite.Name = "labelPackageWebsite";
			this.labelPackageWebsite.Size = new System.Drawing.Size(226, 13);
			this.labelPackageWebsite.TabIndex = 4;
			this.labelPackageWebsite.TabStop = true;
			this.labelPackageWebsite.Text = "http://www.example.com";
			this.labelPackageWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelPackageWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelPackageWebsite_LinkClicked);
			// 
			// labelPackageAuthorCaption
			// 
			this.labelPackageAuthorCaption.AutoSize = true;
			this.labelPackageAuthorCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageAuthorCaption.Location = new System.Drawing.Point(3, 319);
			this.labelPackageAuthorCaption.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageAuthorCaption.Name = "labelPackageAuthorCaption";
			this.labelPackageAuthorCaption.Size = new System.Drawing.Size(51, 13);
			this.labelPackageAuthorCaption.TabIndex = 5;
			this.labelPackageAuthorCaption.Text = "Authors:";
			this.labelPackageAuthorCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPackageAuthor
			// 
			this.labelPackageAuthor.AutoSize = true;
			this.labelPackageAuthor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageAuthor.Location = new System.Drawing.Point(60, 319);
			this.labelPackageAuthor.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageAuthor.Name = "labelPackageAuthor";
			this.labelPackageAuthor.Size = new System.Drawing.Size(226, 13);
			this.labelPackageAuthor.TabIndex = 6;
			this.labelPackageAuthor.Text = "Author1, Author2";
			this.labelPackageAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelPackageLicenseCaption
			// 
			this.labelPackageLicenseCaption.AutoSize = true;
			this.labelPackageLicenseCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageLicenseCaption.Location = new System.Drawing.Point(3, 297);
			this.labelPackageLicenseCaption.Name = "labelPackageLicenseCaption";
			this.labelPackageLicenseCaption.Size = new System.Drawing.Size(51, 19);
			this.labelPackageLicenseCaption.TabIndex = 13;
			this.labelPackageLicenseCaption.Text = "License:";
			this.labelPackageLicenseCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPackageLicense
			// 
			this.labelPackageLicense.AutoSize = true;
			this.labelPackageLicense.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelPackageLicense.Location = new System.Drawing.Point(60, 300);
			this.labelPackageLicense.Margin = new System.Windows.Forms.Padding(3);
			this.labelPackageLicense.Name = "labelPackageLicense";
			this.labelPackageLicense.Size = new System.Drawing.Size(226, 13);
			this.labelPackageLicense.TabIndex = 14;
			this.labelPackageLicense.TabStop = true;
			this.labelPackageLicense.Text = "http://www.license.com";
			this.labelPackageLicense.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelPackageLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelPackageLicense_LinkClicked);
			// 
			// textBoxReleaseNotes
			// 
			this.textBoxReleaseNotes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.textBoxReleaseNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tableLayoutPanelInfo.SetColumnSpan(this.textBoxReleaseNotes, 2);
			this.textBoxReleaseNotes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxReleaseNotes.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxReleaseNotes.Location = new System.Drawing.Point(3, 177);
			this.textBoxReleaseNotes.Multiline = true;
			this.textBoxReleaseNotes.Name = "textBoxReleaseNotes";
			this.textBoxReleaseNotes.ReadOnly = true;
			this.textBoxReleaseNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxReleaseNotes.Size = new System.Drawing.Size(283, 98);
			this.textBoxReleaseNotes.TabIndex = 15;
			this.textBoxReleaseNotes.Text = "Release Notes:\r\n--------------";
			// 
			// labelReleaseNotesCaption
			// 
			this.labelReleaseNotesCaption.AutoSize = true;
			this.tableLayoutPanelInfo.SetColumnSpan(this.labelReleaseNotesCaption, 2);
			this.labelReleaseNotesCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelReleaseNotesCaption.Location = new System.Drawing.Point(3, 158);
			this.labelReleaseNotesCaption.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.labelReleaseNotesCaption.Name = "labelReleaseNotesCaption";
			this.labelReleaseNotesCaption.Size = new System.Drawing.Size(283, 13);
			this.labelReleaseNotesCaption.TabIndex = 16;
			this.labelReleaseNotesCaption.Text = "Release Notes:";
			this.labelReleaseNotesCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonClose.Location = new System.Drawing.Point(399, 0);
			this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
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
			this.panelLowerArea.Controls.Add(this.buttonAdvanced);
			this.panelLowerArea.Controls.Add(this.labelRequireRestart);
			this.panelLowerArea.Controls.Add(this.flowLayoutBottom);
			this.panelLowerArea.Location = new System.Drawing.Point(-3, 472);
			this.panelLowerArea.Name = "panelLowerArea";
			this.panelLowerArea.Size = new System.Drawing.Size(901, 39);
			this.panelLowerArea.TabIndex = 12;
			// 
			// buttonAdvanced
			// 
			this.buttonAdvanced.Location = new System.Drawing.Point(14, 7);
			this.buttonAdvanced.Name = "buttonAdvanced";
			this.buttonAdvanced.Size = new System.Drawing.Size(85, 23);
			this.buttonAdvanced.TabIndex = 17;
			this.buttonAdvanced.Text = "Advanced...";
			this.buttonAdvanced.UseVisualStyleBackColor = true;
			this.buttonAdvanced.Click += new System.EventHandler(this.buttonAdvanced_Click);
			// 
			// labelRequireRestart
			// 
			this.labelRequireRestart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelRequireRestart.ForeColor = System.Drawing.Color.Blue;
			this.labelRequireRestart.Location = new System.Drawing.Point(114, 2);
			this.labelRequireRestart.Name = "labelRequireRestart";
			this.labelRequireRestart.Size = new System.Drawing.Size(366, 32);
			this.labelRequireRestart.TabIndex = 16;
			this.labelRequireRestart.Text = "Click Apply in order to restart Duality and finish the update.";
			this.labelRequireRestart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// flowLayoutBottom
			// 
			this.flowLayoutBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutBottom.Controls.Add(this.buttonClose);
			this.flowLayoutBottom.Controls.Add(this.buttonApply);
			this.flowLayoutBottom.Controls.Add(this.bottomFlowSpacer1);
			this.flowLayoutBottom.Controls.Add(this.buttonUninstall);
			this.flowLayoutBottom.Controls.Add(this.buttonInstall);
			this.flowLayoutBottom.Controls.Add(this.buttonUpdate);
			this.flowLayoutBottom.Controls.Add(this.bottomFlowSpacer2);
			this.flowLayoutBottom.Controls.Add(this.buttonUpdateAll);
			this.flowLayoutBottom.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutBottom.Location = new System.Drawing.Point(415, 7);
			this.flowLayoutBottom.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutBottom.Name = "flowLayoutBottom";
			this.flowLayoutBottom.Size = new System.Drawing.Size(474, 23);
			this.flowLayoutBottom.TabIndex = 15;
			// 
			// buttonApply
			// 
			this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonApply.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonApply.Location = new System.Drawing.Point(324, 0);
			this.buttonApply.Margin = new System.Windows.Forms.Padding(0);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(75, 23);
			this.buttonApply.TabIndex = 15;
			this.buttonApply.Text = "Apply";
			this.toolTip.SetToolTip(this.buttonApply, "Restart the editor to apply changes.");
			this.buttonApply.UseVisualStyleBackColor = true;
			this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
			// 
			// bottomFlowSpacer1
			// 
			this.bottomFlowSpacer1.Location = new System.Drawing.Point(314, 0);
			this.bottomFlowSpacer1.Margin = new System.Windows.Forms.Padding(0);
			this.bottomFlowSpacer1.Name = "bottomFlowSpacer1";
			this.bottomFlowSpacer1.Size = new System.Drawing.Size(10, 15);
			this.bottomFlowSpacer1.TabIndex = 16;
			// 
			// buttonUninstall
			// 
			this.buttonUninstall.Location = new System.Drawing.Point(239, 0);
			this.buttonUninstall.Margin = new System.Windows.Forms.Padding(0);
			this.buttonUninstall.Name = "buttonUninstall";
			this.buttonUninstall.Size = new System.Drawing.Size(75, 23);
			this.buttonUninstall.TabIndex = 14;
			this.buttonUninstall.Text = "Uninstall";
			this.buttonUninstall.UseVisualStyleBackColor = true;
			this.buttonUninstall.Click += new System.EventHandler(this.buttonUninstall_Click);
			// 
			// buttonInstall
			// 
			this.buttonInstall.Location = new System.Drawing.Point(164, 0);
			this.buttonInstall.Margin = new System.Windows.Forms.Padding(0);
			this.buttonInstall.Name = "buttonInstall";
			this.buttonInstall.Size = new System.Drawing.Size(75, 23);
			this.buttonInstall.TabIndex = 12;
			this.buttonInstall.Text = "Install";
			this.buttonInstall.UseVisualStyleBackColor = true;
			this.buttonInstall.Click += new System.EventHandler(this.buttonInstall_Click);
			// 
			// buttonUpdate
			// 
			this.buttonUpdate.Location = new System.Drawing.Point(89, 0);
			this.buttonUpdate.Margin = new System.Windows.Forms.Padding(0);
			this.buttonUpdate.Name = "buttonUpdate";
			this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
			this.buttonUpdate.TabIndex = 13;
			this.buttonUpdate.Text = "Update";
			this.toolTip.SetToolTip(this.buttonUpdate, "Update the Package to the newest version");
			this.buttonUpdate.UseVisualStyleBackColor = true;
			this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
			// 
			// bottomFlowSpacer2
			// 
			this.bottomFlowSpacer2.Location = new System.Drawing.Point(79, 0);
			this.bottomFlowSpacer2.Margin = new System.Windows.Forms.Padding(0);
			this.bottomFlowSpacer2.Name = "bottomFlowSpacer2";
			this.bottomFlowSpacer2.Size = new System.Drawing.Size(10, 15);
			this.bottomFlowSpacer2.TabIndex = 19;
			// 
			// buttonUpdateAll
			// 
			this.buttonUpdateAll.Location = new System.Drawing.Point(4, 0);
			this.buttonUpdateAll.Margin = new System.Windows.Forms.Padding(0);
			this.buttonUpdateAll.Name = "buttonUpdateAll";
			this.buttonUpdateAll.Size = new System.Drawing.Size(75, 23);
			this.buttonUpdateAll.TabIndex = 18;
			this.buttonUpdateAll.Text = "Update All";
			this.toolTip.SetToolTip(this.buttonUpdateAll, "Update all packages to their newest version");
			this.buttonUpdateAll.UseVisualStyleBackColor = true;
			this.buttonUpdateAll.Click += new System.EventHandler(this.buttonUpdateAll_Click);
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
			this.labelHeaderText.Size = new System.Drawing.Size(803, 42);
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
			this.labelHeader.Size = new System.Drawing.Size(806, 22);
			this.labelHeader.TabIndex = 17;
			this.labelHeader.Text = "Manage Duality Packages";
			this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// timerPackageModelChanged
			// 
			this.timerPackageModelChanged.Tick += new System.EventHandler(this.timerPackageModelChanged_Tick);
			// 
			// panelTitleImage
			// 
			this.panelTitleImage.BackgroundImage = global::Duality.Editor.Plugins.PackageManagerFrontend.Properties.Resources.packagebig;
			this.panelTitleImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.panelTitleImage.Location = new System.Drawing.Point(8, 7);
			this.panelTitleImage.Name = "panelTitleImage";
			this.panelTitleImage.Size = new System.Drawing.Size(68, 67);
			this.panelTitleImage.TabIndex = 18;
			// 
			// contextMenuAdvanced
			// 
			this.contextMenuAdvanced.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemReInstallAll});
			this.contextMenuAdvanced.Name = "contextMenuAdvanced";
			this.contextMenuAdvanced.Size = new System.Drawing.Size(191, 26);
			// 
			// itemReInstallAll
			// 
			this.itemReInstallAll.Name = "itemReInstallAll";
			this.itemReInstallAll.Size = new System.Drawing.Size(190, 22);
			this.itemReInstallAll.Text = "Re-Install all Packages";
			this.itemReInstallAll.Click += new System.EventHandler(this.itemReInstallAll_Click);
			// 
			// PackageViewDialog
			// 
			this.AcceptButton = this.buttonApply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.CancelButton = this.buttonClose;
			this.ClientSize = new System.Drawing.Size(897, 510);
			this.Controls.Add(this.panelTitleImage);
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
			this.splitMain.Panel1.PerformLayout();
			this.splitMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.toolStripMain.ResumeLayout(false);
			this.toolStripMain.PerformLayout();
			this.tableLayoutPanelInfo.ResumeLayout(false);
			this.tableLayoutPanelInfo.PerformLayout();
			this.panelLowerArea.ResumeLayout(false);
			this.flowLayoutBottom.ResumeLayout(false);
			this.contextMenuAdvanced.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Aga.Controls.Tree.TreeViewAdv packageList;
		private Duality.Editor.Plugins.PackageManagerFrontend.DualityPackageSummaryNodeControl nodeTextBoxName;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Panel panelLowerArea;
		private System.Windows.Forms.ToolStrip miniToolStrip;
		private System.Windows.Forms.ToolStripTextBox toolStripSearchBox;
		private System.Windows.Forms.ToolStripLabel toolStripLabelSearch;
		private System.Windows.Forms.ToolStrip toolStripMain;
		private System.Windows.Forms.ToolStripLabel toolStripLabelView;
		private System.Windows.Forms.ToolStripComboBox toolStripViewBox;
		private Aga.Controls.Tree.TreeColumn treeColumnName;
		private Aga.Controls.Tree.TreeColumn treeColumnVersion;
		private Aga.Controls.Tree.TreeColumn treeColumnDownloads;
		private Duality.Editor.Plugins.PackageManagerFrontend.DualityPackageVersionNodeControl nodeTextBoxVersion;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxDownloads;
		private System.Windows.Forms.Label labelHeaderText;
		private System.Windows.Forms.Label labelHeader;
		private System.Windows.Forms.Panel panelTitleImage;
		private Aga.Controls.Tree.NodeControls.NodeIcon nodeIcon;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanelInfo;
		private System.Windows.Forms.Label labelPackageTitle;
		private System.Windows.Forms.Label labelPackageId;
		private System.Windows.Forms.Label labelPackageDesc;
		private System.Windows.Forms.Label labelPackageWebsiteCaption;
		private System.Windows.Forms.LinkLabel labelPackageWebsite;
		private System.Windows.Forms.Label labelPackageTagsCaption;
		private System.Windows.Forms.Label labelPackageAuthorCaption;
		private System.Windows.Forms.Label labelPackageAuthor;
		private System.Windows.Forms.Label labelPackageUpdated;
		private System.Windows.Forms.Label labelPackageVersionCaption;
		private System.Windows.Forms.Label labelPackageTags;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutBottom;
		private System.Windows.Forms.Button buttonApply;
		private System.Windows.Forms.Label bottomFlowSpacer1;
		private System.Windows.Forms.Button buttonUninstall;
		private System.Windows.Forms.Button buttonInstall;
		private System.Windows.Forms.Button buttonUpdate;
		private System.Windows.Forms.Label labelPackageUpdatedCaption;
		private System.Windows.Forms.Label labelPackageVersion;
		private System.Windows.Forms.Label labelRequireRestart;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Label bottomFlowSpacer2;
		private System.Windows.Forms.Button buttonUpdateAll;
		private System.Windows.Forms.Timer timerPackageModelChanged;
		private Aga.Controls.Tree.TreeColumn treeColumnPackageType;
		private Duality.Editor.Plugins.PackageManagerFrontend.DualityPackageTypeNodeControl nodeIconPackageType;
		private Aga.Controls.Tree.TreeColumn treeColumnDate;
		private Duality.Editor.Plugins.PackageManagerFrontend.DualityPackageDateNodeControl nodeTextBoxDate;
		private System.Windows.Forms.Button buttonAdvanced;
		private System.Windows.Forms.ContextMenuStrip contextMenuAdvanced;
		private System.Windows.Forms.ToolStripMenuItem itemReInstallAll;
		private System.Windows.Forms.Label labelPackageLicenseCaption;
		private System.Windows.Forms.LinkLabel labelPackageLicense;
		private System.Windows.Forms.TextBox textBoxReleaseNotes;
		private System.Windows.Forms.Label labelReleaseNotesCaption;
	}
}