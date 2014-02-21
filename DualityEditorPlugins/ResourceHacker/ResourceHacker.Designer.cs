namespace ResourceHacker
{
	partial class ResourceHacker
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceHacker));
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.treeView = new Aga.Controls.Tree.TreeViewAdv();
			this.treeViewColumnName = new Aga.Controls.Tree.TreeColumn();
			this.treeViewColumnObjId = new Aga.Controls.Tree.TreeColumn();
			this.treeViewColumnValue = new Aga.Controls.Tree.TreeColumn();
			this.treeViewColumnType = new Aga.Controls.Tree.TreeColumn();
			this.nodeStateIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
			this.nodeTextBoxName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.nodeTextBoxObjId = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.nodeTextBoxType = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.nodeTextBoxValue = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.mainToolStrip = new System.Windows.Forms.ToolStrip();
			this.actionOpen = new System.Windows.Forms.ToolStripButton();
			this.actionSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.actionRenameType = new System.Windows.Forms.ToolStripButton();
			this.batchActionButton = new System.Windows.Forms.ToolStripSplitButton();
			this.batchActionRenameType = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.propertyGrid = new Duality.Editor.Controls.DualitorPropertyGrid();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.mainToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 0);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.treeView);
			this.splitContainer.Panel1.Controls.Add(this.mainToolStrip);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.propertyGrid);
			this.splitContainer.Size = new System.Drawing.Size(537, 447);
			this.splitContainer.SplitterDistance = 288;
			this.splitContainer.TabIndex = 0;
			// 
			// treeView
			// 
			this.treeView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.treeView.Columns.Add(this.treeViewColumnName);
			this.treeView.Columns.Add(this.treeViewColumnObjId);
			this.treeView.Columns.Add(this.treeViewColumnValue);
			this.treeView.Columns.Add(this.treeViewColumnType);
			this.treeView.DefaultToolTipProvider = null;
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.DragDropMarkColor = System.Drawing.Color.Black;
			this.treeView.FullRowSelect = true;
			this.treeView.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.treeView.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.treeView.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.treeView.Location = new System.Drawing.Point(0, 25);
			this.treeView.Model = null;
			this.treeView.Name = "treeView";
			this.treeView.NodeControls.Add(this.nodeStateIcon);
			this.treeView.NodeControls.Add(this.nodeTextBoxName);
			this.treeView.NodeControls.Add(this.nodeTextBoxObjId);
			this.treeView.NodeControls.Add(this.nodeTextBoxType);
			this.treeView.NodeControls.Add(this.nodeTextBoxValue);
			this.treeView.NodeFilter = null;
			this.treeView.SelectedNode = null;
			this.treeView.Size = new System.Drawing.Size(537, 263);
			this.treeView.TabIndex = 0;
			this.treeView.Text = "DataNodes";
			this.treeView.UseColumns = true;
			this.treeView.SelectionChanged += new System.EventHandler(this.treeView_SelectionChanged);
			// 
			// treeViewColumnName
			// 
			this.treeViewColumnName.Header = "Name";
			this.treeViewColumnName.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeViewColumnName.TooltipText = null;
			this.treeViewColumnName.Width = 250;
			// 
			// treeViewColumnObjId
			// 
			this.treeViewColumnObjId.Header = "ObjId";
			this.treeViewColumnObjId.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeViewColumnObjId.TooltipText = null;
			this.treeViewColumnObjId.Width = 40;
			// 
			// treeViewColumnValue
			// 
			this.treeViewColumnValue.Header = "Value";
			this.treeViewColumnValue.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeViewColumnValue.TooltipText = null;
			this.treeViewColumnValue.Width = 75;
			// 
			// treeViewColumnType
			// 
			this.treeViewColumnType.Header = "Type";
			this.treeViewColumnType.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeViewColumnType.TooltipText = null;
			this.treeViewColumnType.Width = 250;
			// 
			// nodeStateIcon
			// 
			this.nodeStateIcon.DataPropertyName = "Image";
			this.nodeStateIcon.LeftMargin = 1;
			this.nodeStateIcon.ParentColumn = this.treeViewColumnName;
			this.nodeStateIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
			// 
			// nodeTextBoxName
			// 
			this.nodeTextBoxName.DataPropertyName = "Text";
			this.nodeTextBoxName.IncrementalSearchEnabled = true;
			this.nodeTextBoxName.LeftMargin = 3;
			this.nodeTextBoxName.ParentColumn = this.treeViewColumnName;
			// 
			// nodeTextBoxObjId
			// 
			this.nodeTextBoxObjId.DataPropertyName = "ObjId";
			this.nodeTextBoxObjId.IncrementalSearchEnabled = true;
			this.nodeTextBoxObjId.LeftMargin = 3;
			this.nodeTextBoxObjId.ParentColumn = this.treeViewColumnObjId;
			// 
			// nodeTextBoxType
			// 
			this.nodeTextBoxType.DataPropertyName = "ResolvedTypeName";
			this.nodeTextBoxType.IncrementalSearchEnabled = true;
			this.nodeTextBoxType.LeftMargin = 3;
			this.nodeTextBoxType.ParentColumn = this.treeViewColumnType;
			// 
			// nodeTextBoxValue
			// 
			this.nodeTextBoxValue.DataPropertyName = "DataValue";
			this.nodeTextBoxValue.IncrementalSearchEnabled = true;
			this.nodeTextBoxValue.LeftMargin = 3;
			this.nodeTextBoxValue.ParentColumn = this.treeViewColumnValue;
			// 
			// mainToolStrip
			// 
			this.mainToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionOpen,
            this.actionSave,
            this.toolStripSeparator1,
            this.actionRenameType,
            this.batchActionButton});
			this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
			this.mainToolStrip.Name = "mainToolStrip";
			this.mainToolStrip.Size = new System.Drawing.Size(537, 25);
			this.mainToolStrip.TabIndex = 1;
			// 
			// actionOpen
			// 
			this.actionOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionOpen.Image = ((System.Drawing.Image)(resources.GetObject("actionOpen.Image")));
			this.actionOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionOpen.Name = "actionOpen";
			this.actionOpen.Size = new System.Drawing.Size(23, 22);
			this.actionOpen.Text = "Open Resource File...";
			this.actionOpen.Click += new System.EventHandler(this.actionOpen_Click);
			// 
			// actionSave
			// 
			this.actionSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionSave.Image = global::ResourceHacker.Properties.Resources.iconSaveFile;
			this.actionSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionSave.Name = "actionSave";
			this.actionSave.Size = new System.Drawing.Size(23, 22);
			this.actionSave.Text = "Save Resource File...";
			this.actionSave.Click += new System.EventHandler(this.actionSave_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// actionRenameType
			// 
			this.actionRenameType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionRenameType.Image = global::ResourceHacker.Properties.Resources.iconRenameClass;
			this.actionRenameType.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionRenameType.Name = "actionRenameType";
			this.actionRenameType.Size = new System.Drawing.Size(23, 22);
			this.actionRenameType.Text = "Rename Type...";
			this.actionRenameType.Click += new System.EventHandler(this.actionRenameType_Click);
			// 
			// batchActionButton
			// 
			this.batchActionButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.batchActionButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.batchActionRenameType});
			this.batchActionButton.Image = global::ResourceHacker.Properties.Resources.iconBatchAction;
			this.batchActionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.batchActionButton.Name = "batchActionButton";
			this.batchActionButton.Size = new System.Drawing.Size(121, 22);
			this.batchActionButton.Text = "Batch Actions...";
			this.batchActionButton.Click += new System.EventHandler(this.batchActionButton_Click);
			// 
			// batchActionRenameType
			// 
			this.batchActionRenameType.Image = global::ResourceHacker.Properties.Resources.iconRenameClass;
			this.batchActionRenameType.Name = "batchActionRenameType";
			this.batchActionRenameType.Size = new System.Drawing.Size(155, 22);
			this.batchActionRenameType.Text = "Rename Type...";
			this.batchActionRenameType.Click += new System.EventHandler(this.batchActionRenameType_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
			// 
			// propertyGrid
			// 
			this.propertyGrid.AllowDrop = true;
			this.propertyGrid.AutoScroll = true;
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.ReadOnly = false;
			this.propertyGrid.Size = new System.Drawing.Size(537, 155);
			this.propertyGrid.TabIndex = 0;
			// 
			// ResourceHacker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
			this.ClientSize = new System.Drawing.Size(537, 447);
			this.Controls.Add(this.splitContainer);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ResourceHacker";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
			this.Text = "Resource Hacker";
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel1.PerformLayout();
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.mainToolStrip.ResumeLayout(false);
			this.mainToolStrip.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer;
		private Aga.Controls.Tree.TreeViewAdv treeView;
		private System.Windows.Forms.ToolStrip mainToolStrip;
		private Duality.Editor.Controls.DualitorPropertyGrid propertyGrid;
		private System.Windows.Forms.ToolStripButton actionOpen;
		private System.Windows.Forms.ToolStripButton actionSave;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private Aga.Controls.Tree.TreeColumn treeViewColumnName;
		private Aga.Controls.Tree.TreeColumn treeViewColumnObjId;
		private Aga.Controls.Tree.NodeControls.NodeStateIcon nodeStateIcon;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxName;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxObjId;
		private Aga.Controls.Tree.TreeColumn treeViewColumnType;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxType;
		private Aga.Controls.Tree.TreeColumn treeViewColumnValue;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxValue;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton actionRenameType;
		private System.Windows.Forms.ToolStripSplitButton batchActionButton;
		private System.Windows.Forms.ToolStripMenuItem batchActionRenameType;


	}
}