namespace Duality.Editor.Plugins.ProjectView
{
	partial class ProjectFolderView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectFolderView));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonWorkDir = new System.Windows.Forms.ToolStripButton();
			this.toolStripLabelProjectName = new System.Windows.Forms.ToolStripLabel();
			this.folderView = new Aga.Controls.Tree.TreeViewAdv();
			this.treeColumnName = new Aga.Controls.Tree.TreeColumn();
			this.treeColumnType = new Aga.Controls.Tree.TreeColumn();
			this.contextMenuNode = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripSeparatorCustomActions = new System.Windows.Forms.ToolStripSeparator();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparatorNew = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparatorShowInExplorer = new System.Windows.Forms.ToolStripSeparator();
			this.showInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nodeStateIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
			this.nodeTextBoxName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.nodeTextBoxType = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.timerFlashItem = new System.Windows.Forms.Timer(this.components);
			this.contextMenuDragMoveCopy = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.moveHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.textBoxFilter = new System.Windows.Forms.TextBox();
			this.labelFilter = new System.Windows.Forms.Label();
			this.toolStrip.SuspendLayout();
			this.contextMenuNode.SuspendLayout();
			this.contextMenuDragMoveCopy.SuspendLayout();
			this.panelBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonWorkDir,
            this.toolStripLabelProjectName});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(304, 25);
			this.toolStrip.TabIndex = 0;
			// 
			// toolStripButtonWorkDir
			// 
			this.toolStripButtonWorkDir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonWorkDir.Image = global::Duality.Editor.Plugins.ProjectView.Properties.Resources.WorkingFolderIcon16;
			this.toolStripButtonWorkDir.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonWorkDir.Name = "toolStripButtonWorkDir";
			this.toolStripButtonWorkDir.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonWorkDir.Text = "Open working directory";
			this.toolStripButtonWorkDir.Click += new System.EventHandler(this.toolStripButtonWorkDir_Click);
			// 
			// toolStripLabelProjectName
			// 
			this.toolStripLabelProjectName.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripLabelProjectName.Name = "toolStripLabelProjectName";
			this.toolStripLabelProjectName.Size = new System.Drawing.Size(115, 22);
			this.toolStripLabelProjectName.Text = "Project: Some Name";
			// 
			// folderView
			// 
			this.folderView.AllowDrop = true;
			this.folderView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.folderView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.folderView.Columns.Add(this.treeColumnName);
			this.folderView.Columns.Add(this.treeColumnType);
			this.folderView.ContextMenuStrip = this.contextMenuNode;
			this.folderView.DefaultToolTipProvider = null;
			this.folderView.DisplayDraggingNodes = true;
			this.folderView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.folderView.DragDropMarkColor = System.Drawing.Color.Black;
			this.folderView.FullRowSelect = true;
			this.folderView.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.folderView.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.folderView.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.folderView.Location = new System.Drawing.Point(0, 25);
			this.folderView.Model = null;
			this.folderView.Name = "folderView";
			this.folderView.NodeControls.Add(this.nodeStateIcon);
			this.folderView.NodeControls.Add(this.nodeTextBoxName);
			this.folderView.NodeControls.Add(this.nodeTextBoxType);
			this.folderView.NodeFilter = null;
			this.folderView.SelectedNode = null;
			this.folderView.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
			this.folderView.ShowNodeToolTips = true;
			this.folderView.Size = new System.Drawing.Size(304, 494);
			this.folderView.TabIndex = 1;
			this.folderView.UseColumns = true;
			this.folderView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.folderView_ItemDrag);
			this.folderView.NodeMouseDoubleClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.folderView_NodeMouseDoubleClick);
			this.folderView.SelectionChanged += new System.EventHandler(this.folderView_SelectionChanged);
			this.folderView.Expanding += new System.EventHandler<Aga.Controls.Tree.TreeViewAdvEventArgs>(this.folderView_Expanding);
			this.folderView.DragDrop += new System.Windows.Forms.DragEventHandler(this.folderView_DragDrop);
			this.folderView.DragOver += new System.Windows.Forms.DragEventHandler(this.folderView_DragOver);
			this.folderView.Enter += new System.EventHandler(this.folderView_Enter);
			this.folderView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.folderView_KeyDown);
			this.folderView.Leave += new System.EventHandler(this.folderView_Leave);
			this.folderView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.folderView_MouseUp);
			// 
			// treeColumnName
			// 
			this.treeColumnName.Header = "Name";
			this.treeColumnName.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumnName.TooltipText = null;
			this.treeColumnName.Width = 200;
			// 
			// treeColumnType
			// 
			this.treeColumnType.Header = "Type";
			this.treeColumnType.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumnType.TooltipText = null;
			this.treeColumnType.Width = 100;
			// 
			// contextMenuNode
			// 
			this.contextMenuNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparatorCustomActions,
            this.newToolStripMenuItem,
            this.toolStripSeparatorNew,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.toolStripSeparatorShowInExplorer,
            this.showInExplorerToolStripMenuItem});
			this.contextMenuNode.Name = "contextMenuNode";
			this.contextMenuNode.Size = new System.Drawing.Size(162, 176);
			this.contextMenuNode.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuNode_Opening);
			// 
			// toolStripSeparatorCustomActions
			// 
			this.toolStripSeparatorCustomActions.Name = "toolStripSeparatorCustomActions";
			this.toolStripSeparatorCustomActions.Size = new System.Drawing.Size(158, 6);
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.folderToolStripMenuItem,
            this.toolStripSeparator4});
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.newToolStripMenuItem_DropDownItemClicked);
			// 
			// folderToolStripMenuItem
			// 
			this.folderToolStripMenuItem.Image = global::Duality.Editor.Plugins.ProjectView.Properties.Resources.folder;
			this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
			this.folderToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.folderToolStripMenuItem.Text = "Folder";
			this.folderToolStripMenuItem.Click += new System.EventHandler(this.folderToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(104, 6);
			// 
			// toolStripSeparatorNew
			// 
			this.toolStripSeparatorNew.Name = "toolStripSeparatorNew";
			this.toolStripSeparatorNew.Size = new System.Drawing.Size(158, 6);
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Image = global::Duality.Editor.Plugins.ProjectView.Properties.Resources.cut;
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.cutToolStripMenuItem.Text = "Cut";
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = global::Duality.Editor.Plugins.ProjectView.Properties.Resources.page_copy;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Image = global::Duality.Editor.Plugins.ProjectView.Properties.Resources.page_paste;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Image = global::Duality.Editor.Plugins.ProjectView.Properties.Resources.cross;
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// renameToolStripMenuItem
			// 
			this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
			this.renameToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.renameToolStripMenuItem.Text = "Rename";
			this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
			// 
			// toolStripSeparatorShowInExplorer
			// 
			this.toolStripSeparatorShowInExplorer.Name = "toolStripSeparatorShowInExplorer";
			this.toolStripSeparatorShowInExplorer.Size = new System.Drawing.Size(158, 6);
			// 
			// showInExplorerToolStripMenuItem
			// 
			this.showInExplorerToolStripMenuItem.Name = "showInExplorerToolStripMenuItem";
			this.showInExplorerToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.showInExplorerToolStripMenuItem.Text = "Show in Explorer";
			this.showInExplorerToolStripMenuItem.Click += new System.EventHandler(this.showInExplorerToolStripMenuItem_Click);
			// 
			// nodeStateIcon
			// 
			this.nodeStateIcon.DataPropertyName = "Image";
			this.nodeStateIcon.LeftMargin = 1;
			this.nodeStateIcon.ParentColumn = this.treeColumnName;
			this.nodeStateIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
			// 
			// nodeTextBoxName
			// 
			this.nodeTextBoxName.DataPropertyName = "Text";
			this.nodeTextBoxName.EditEnabled = true;
			this.nodeTextBoxName.IncrementalSearchEnabled = true;
			this.nodeTextBoxName.LeftMargin = 3;
			this.nodeTextBoxName.ParentColumn = this.treeColumnName;
			// 
			// nodeTextBoxType
			// 
			this.nodeTextBoxType.DataPropertyName = "TypeName";
			this.nodeTextBoxType.IncrementalSearchEnabled = true;
			this.nodeTextBoxType.LeftMargin = 3;
			this.nodeTextBoxType.ParentColumn = this.treeColumnType;
			// 
			// timerFlashItem
			// 
			this.timerFlashItem.Interval = 30;
			this.timerFlashItem.Tick += new System.EventHandler(this.timerFlashItem_Tick);
			// 
			// contextMenuDragMoveCopy
			// 
			this.contextMenuDragMoveCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyHereToolStripMenuItem,
            this.moveHereToolStripMenuItem,
            this.toolStripSeparator1,
            this.cancelToolStripMenuItem});
			this.contextMenuDragMoveCopy.Name = "contextMenuDragMoveCopy";
			this.contextMenuDragMoveCopy.Size = new System.Drawing.Size(131, 76);
			// 
			// copyHereToolStripMenuItem
			// 
			this.copyHereToolStripMenuItem.Name = "copyHereToolStripMenuItem";
			this.copyHereToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.copyHereToolStripMenuItem.Text = "Copy here";
			this.copyHereToolStripMenuItem.Click += new System.EventHandler(this.copyHereToolStripMenuItem_Click);
			// 
			// moveHereToolStripMenuItem
			// 
			this.moveHereToolStripMenuItem.Name = "moveHereToolStripMenuItem";
			this.moveHereToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.moveHereToolStripMenuItem.Text = "Move here";
			this.moveHereToolStripMenuItem.Click += new System.EventHandler(this.moveHereToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(127, 6);
			// 
			// cancelToolStripMenuItem
			// 
			this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
			this.cancelToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.cancelToolStripMenuItem.Text = "Cancel";
			// 
			// panelBottom
			// 
			this.panelBottom.BackColor = System.Drawing.Color.Transparent;
			this.panelBottom.Controls.Add(this.textBoxFilter);
			this.panelBottom.Controls.Add(this.labelFilter);
			this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelBottom.Location = new System.Drawing.Point(0, 519);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Padding = new System.Windows.Forms.Padding(3);
			this.panelBottom.Size = new System.Drawing.Size(304, 26);
			this.panelBottom.TabIndex = 2;
			// 
			// textBoxFilter
			// 
			this.textBoxFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.textBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxFilter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxFilter.Location = new System.Drawing.Point(41, 3);
			this.textBoxFilter.Name = "textBoxFilter";
			this.textBoxFilter.Size = new System.Drawing.Size(260, 20);
			this.textBoxFilter.TabIndex = 0;
			this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
			// 
			// labelFilter
			// 
			this.labelFilter.AutoSize = true;
			this.labelFilter.BackColor = System.Drawing.Color.Transparent;
			this.labelFilter.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelFilter.Location = new System.Drawing.Point(3, 3);
			this.labelFilter.Name = "labelFilter";
			this.labelFilter.Padding = new System.Windows.Forms.Padding(3);
			this.labelFilter.Size = new System.Drawing.Size(38, 19);
			this.labelFilter.TabIndex = 1;
			this.labelFilter.Text = "Filter:";
			this.labelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ProjectFolderView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(304, 545);
			this.Controls.Add(this.folderView);
			this.Controls.Add(this.panelBottom);
			this.Controls.Add(this.toolStrip);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ProjectFolderView";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.Text = "Project View";
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.contextMenuNode.ResumeLayout(false);
			this.contextMenuDragMoveCopy.ResumeLayout(false);
			this.panelBottom.ResumeLayout(false);
			this.panelBottom.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip;
		private Aga.Controls.Tree.TreeViewAdv folderView;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxName;
		private Aga.Controls.Tree.NodeControls.NodeStateIcon nodeStateIcon;
		private System.Windows.Forms.Timer timerFlashItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuDragMoveCopy;
		private System.Windows.Forms.ToolStripMenuItem copyHereToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moveHereToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuNode;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparatorNew;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparatorShowInExplorer;
		private System.Windows.Forms.ToolStripMenuItem showInExplorerToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButtonWorkDir;
		private System.Windows.Forms.ToolStripLabel toolStripLabelProjectName;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.TextBox textBoxFilter;
		private System.Windows.Forms.Label labelFilter;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparatorCustomActions;
		private Aga.Controls.Tree.TreeColumn treeColumnName;
		private Aga.Controls.Tree.TreeColumn treeColumnType;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxType;
	}
}