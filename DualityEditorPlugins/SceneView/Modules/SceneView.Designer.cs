namespace Duality.Editor.Plugins.SceneView
{
	partial class SceneView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SceneView));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.buttonShowComponents = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonCreateScene = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSaveScene = new System.Windows.Forms.ToolStripButton();
			this.toolStripLabelSceneName = new System.Windows.Forms.ToolStripLabel();
			this.contextMenuNode = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripSeparatorCustomActions = new System.Windows.Forms.ToolStripSeparator();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gameObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newGameObjectSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparatorNew = new System.Windows.Forms.ToolStripSeparator();
			this.cloneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparatorGameObject = new System.Windows.Forms.ToolStripSeparator();
			this.lockedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.timerFlashItem = new System.Windows.Forms.Timer(this.components);
			this.contextMenuDragMoveCopy = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.moveHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.textBoxFilter = new System.Windows.Forms.TextBox();
			this.labelFilter = new System.Windows.Forms.Label();
			this.objectView = new Aga.Controls.Tree.TreeViewAdv();
			this.nodeStateIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
			this.nodeTextBoxName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.toolStrip.SuspendLayout();
			this.contextMenuNode.SuspendLayout();
			this.contextMenuDragMoveCopy.SuspendLayout();
			this.panelBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.toolStrip.GripMargin = new System.Windows.Forms.Padding(0);
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonShowComponents,
            this.toolStripSeparator2,
            this.toolStripButtonCreateScene,
            this.toolStripButtonSaveScene,
            this.toolStripLabelSceneName});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(206, 25);
			this.toolStrip.TabIndex = 0;
			// 
			// buttonShowComponents
			// 
			this.buttonShowComponents.Checked = true;
			this.buttonShowComponents.CheckOnClick = true;
			this.buttonShowComponents.CheckState = System.Windows.Forms.CheckState.Checked;
			this.buttonShowComponents.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonShowComponents.Image = global::Duality.Editor.Plugins.SceneView.Properties.Resources.iconCmpUnknown;
			this.buttonShowComponents.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonShowComponents.Name = "buttonShowComponents";
			this.buttonShowComponents.Size = new System.Drawing.Size(23, 22);
			this.buttonShowComponents.Text = "Show Components";
			this.buttonShowComponents.CheckedChanged += new System.EventHandler(this.buttonShowComponents_CheckedChanged);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonCreateScene
			// 
			this.toolStripButtonCreateScene.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonCreateScene.Image = global::Duality.Editor.Plugins.SceneView.Properties.Resources.AddScene;
			this.toolStripButtonCreateScene.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonCreateScene.Name = "toolStripButtonCreateScene";
			this.toolStripButtonCreateScene.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonCreateScene.Text = "Create new Scene";
			this.toolStripButtonCreateScene.Click += new System.EventHandler(this.buttonCreateScene_Click);
			// 
			// toolStripButtonSaveScene
			// 
			this.toolStripButtonSaveScene.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSaveScene.Image = global::Duality.Editor.Plugins.SceneView.Properties.Resources.disk;
			this.toolStripButtonSaveScene.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSaveScene.Name = "toolStripButtonSaveScene";
			this.toolStripButtonSaveScene.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonSaveScene.Text = "Save Scene";
			this.toolStripButtonSaveScene.Click += new System.EventHandler(this.buttonSaveScene_Click);
			// 
			// toolStripLabelSceneName
			// 
			this.toolStripLabelSceneName.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripLabelSceneName.AutoToolTip = true;
			this.toolStripLabelSceneName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.toolStripLabelSceneName.Name = "toolStripLabelSceneName";
			this.toolStripLabelSceneName.Size = new System.Drawing.Size(103, 22);
			this.toolStripLabelSceneName.Text = "Scene: Some Name";
			this.toolStripLabelSceneName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// contextMenuNode
			// 
			this.contextMenuNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparatorCustomActions,
            this.newToolStripMenuItem,
            this.toolStripSeparatorNew,
            this.cloneToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.toolStripSeparatorGameObject,
            this.lockedToolStripMenuItem});
			this.contextMenuNode.Name = "contextMenuNode";
			this.contextMenuNode.Size = new System.Drawing.Size(153, 154);
			this.contextMenuNode.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.contextMenuNode_Closing);
			this.contextMenuNode.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuNode_Opening);
			// 
			// toolStripSeparatorCustomActions
			// 
			this.toolStripSeparatorCustomActions.Name = "toolStripSeparatorCustomActions";
			this.toolStripSeparatorCustomActions.Size = new System.Drawing.Size(149, 6);
			this.toolStripSeparatorCustomActions.Visible = false;
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameObjectToolStripMenuItem,
            this.newGameObjectSeparator});
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.newToolStripMenuItem_DropDownItemClicked);
			// 
			// gameObjectToolStripMenuItem
			// 
			this.gameObjectToolStripMenuItem.Name = "gameObjectToolStripMenuItem";
			this.gameObjectToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.gameObjectToolStripMenuItem.Text = "GameObject";
			this.gameObjectToolStripMenuItem.Click += new System.EventHandler(this.gameObjectToolStripMenuItem_Click);
			// 
			// newGameObjectSeparator
			// 
			this.newGameObjectSeparator.Name = "newGameObjectSeparator";
			this.newGameObjectSeparator.Size = new System.Drawing.Size(137, 6);
			// 
			// toolStripSeparatorNew
			// 
			this.toolStripSeparatorNew.Name = "toolStripSeparatorNew";
			this.toolStripSeparatorNew.Size = new System.Drawing.Size(149, 6);
			// 
			// cloneToolStripMenuItem
			// 
			this.cloneToolStripMenuItem.Image = global::Duality.Editor.Plugins.SceneView.Properties.Resources.page_copy;
			this.cloneToolStripMenuItem.Name = "cloneToolStripMenuItem";
			this.cloneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.cloneToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.cloneToolStripMenuItem.Text = "Clone";
			this.cloneToolStripMenuItem.Click += new System.EventHandler(this.cloneToolStripMenuItem_Click);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Image = global::Duality.Editor.Plugins.SceneView.Properties.Resources.cross;
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// renameToolStripMenuItem
			// 
			this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
			this.renameToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.renameToolStripMenuItem.Text = "Rename";
			this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
			// 
			// toolStripSeparatorGameObject
			// 
			this.toolStripSeparatorGameObject.Name = "toolStripSeparatorGameObject";
			this.toolStripSeparatorGameObject.Size = new System.Drawing.Size(149, 6);
			// 
			// lockedToolStripMenuItem
			// 
			this.lockedToolStripMenuItem.Name = "lockedToolStripMenuItem";
			this.lockedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.lockedToolStripMenuItem.Text = "Lock / Hide";
			this.lockedToolStripMenuItem.ToolTipText = "Locks or hides the object in the editor";
			this.lockedToolStripMenuItem.Click += new System.EventHandler(this.lockedToolStripMenuItem_Click);
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
			this.panelBottom.Location = new System.Drawing.Point(0, 521);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Padding = new System.Windows.Forms.Padding(3);
			this.panelBottom.Size = new System.Drawing.Size(206, 26);
			this.panelBottom.TabIndex = 2;
			// 
			// textBoxFilter
			// 
			this.textBoxFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.textBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxFilter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxFilter.Location = new System.Drawing.Point(41, 3);
			this.textBoxFilter.Name = "textBoxFilter";
			this.textBoxFilter.Size = new System.Drawing.Size(162, 20);
			this.textBoxFilter.TabIndex = 0;
			this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
			// 
			// labelFilter
			// 
			this.labelFilter.AutoSize = true;
			this.labelFilter.BackColor = System.Drawing.Color.Transparent;
			this.labelFilter.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelFilter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.labelFilter.Location = new System.Drawing.Point(3, 3);
			this.labelFilter.Name = "labelFilter";
			this.labelFilter.Padding = new System.Windows.Forms.Padding(3);
			this.labelFilter.Size = new System.Drawing.Size(38, 19);
			this.labelFilter.TabIndex = 1;
			this.labelFilter.Text = "Filter:";
			this.labelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// objectView
			// 
			this.objectView.AllowDrop = true;
			this.objectView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.objectView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.objectView.ContextMenuStrip = this.contextMenuNode;
			this.objectView.DefaultToolTipProvider = null;
			this.objectView.DisplayDraggingNodes = true;
			this.objectView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectView.DragDropMarkColor = System.Drawing.Color.Black;
			this.objectView.FullRowSelect = true;
			this.objectView.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.objectView.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.objectView.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.objectView.Location = new System.Drawing.Point(0, 25);
			this.objectView.Model = null;
			this.objectView.Name = "objectView";
			this.objectView.NodeControls.Add(this.nodeStateIcon);
			this.objectView.NodeControls.Add(this.nodeTextBoxName);
			this.objectView.NodeFilter = null;
			this.objectView.SelectedNode = null;
			this.objectView.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
			this.objectView.ShowNodeToolTips = true;
			this.objectView.Size = new System.Drawing.Size(206, 496);
			this.objectView.TabIndex = 1;
			this.objectView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.objectView_ItemDrag);
			this.objectView.NodeMouseDoubleClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.objectView_NodeMouseDoubleClick);
			this.objectView.SelectionChanged += new System.EventHandler(this.objectView_SelectionChanged);
			this.objectView.DragDrop += new System.Windows.Forms.DragEventHandler(this.objectView_DragDrop);
			this.objectView.DragOver += new System.Windows.Forms.DragEventHandler(this.objectView_DragOver);
			this.objectView.Enter += new System.EventHandler(this.objectView_Enter);
			this.objectView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.objectView_KeyDown);
			this.objectView.Leave += new System.EventHandler(this.objectView_Leave);
			this.objectView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.objectView_MouseUp);
			// 
			// nodeStateIcon
			// 
			this.nodeStateIcon.DataPropertyName = "Image";
			this.nodeStateIcon.LeftMargin = 1;
			this.nodeStateIcon.ParentColumn = null;
			this.nodeStateIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
			// 
			// nodeTextBoxName
			// 
			this.nodeTextBoxName.DataPropertyName = "Text";
			this.nodeTextBoxName.EditEnabled = true;
			this.nodeTextBoxName.IncrementalSearchEnabled = true;
			this.nodeTextBoxName.LeftMargin = 3;
			this.nodeTextBoxName.ParentColumn = null;
			// 
			// SceneView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(206, 547);
			this.Controls.Add(this.objectView);
			this.Controls.Add(this.panelBottom);
			this.Controls.Add(this.toolStrip);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
						| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SceneView";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.Text = "Scene View";
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
		private Aga.Controls.Tree.TreeViewAdv objectView;
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
		private System.Windows.Forms.ToolStripSeparator toolStripSeparatorNew;
		private System.Windows.Forms.ToolStripMenuItem cloneToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButtonCreateScene;
		private System.Windows.Forms.ToolStripLabel toolStripLabelSceneName;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.TextBox textBoxFilter;
		private System.Windows.Forms.Label labelFilter;
		private System.Windows.Forms.ToolStripMenuItem gameObjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator newGameObjectSeparator;
		private System.Windows.Forms.ToolStripButton toolStripButtonSaveScene;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparatorCustomActions;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparatorGameObject;
		private System.Windows.Forms.ToolStripMenuItem lockedToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton buttonShowComponents;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
	}
}