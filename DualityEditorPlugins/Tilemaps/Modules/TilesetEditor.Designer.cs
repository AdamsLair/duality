namespace Duality.Editor.Plugins.Tilemaps
{
	partial class TilesetEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TilesetEditor));
			this.toolStripModeSelect = new System.Windows.Forms.ToolStrip();
			this.checkModeVisualLayers = new System.Windows.Forms.ToolStripButton();
			this.checkModeCollisionInfo = new System.Windows.Forms.ToolStripButton();
			this.layerView = new Aga.Controls.Tree.TreeViewAdv();
			this.tilesetView = new Duality.Editor.Plugins.Tilemaps.TilesetView();
			this.toolStripEdit = new System.Windows.Forms.ToolStrip();
			this.buttonAddLayer = new System.Windows.Forms.ToolStripButton();
			this.buttonRemoveLayer = new System.Windows.Forms.ToolStripButton();
			this.buttonBrightness = new System.Windows.Forms.ToolStripButton();
			this.toolStripModeSelect.SuspendLayout();
			this.toolStripEdit.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripModeSelect
			// 
			this.toolStripModeSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.toolStripModeSelect.GripMargin = new System.Windows.Forms.Padding(0);
			this.toolStripModeSelect.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripModeSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkModeVisualLayers,
            this.checkModeCollisionInfo});
			this.toolStripModeSelect.Location = new System.Drawing.Point(0, 0);
			this.toolStripModeSelect.Name = "toolStripModeSelect";
			this.toolStripModeSelect.Size = new System.Drawing.Size(503, 25);
			this.toolStripModeSelect.TabIndex = 0;
			this.toolStripModeSelect.Text = "Main Toolstrip";
			// 
			// checkModeVisualLayers
			// 
			this.checkModeVisualLayers.Image = global::Duality.Editor.Plugins.Tilemaps.Properties.Resources.IconTilesetVisualLayers;
			this.checkModeVisualLayers.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.checkModeVisualLayers.Name = "checkModeVisualLayers";
			this.checkModeVisualLayers.Size = new System.Drawing.Size(94, 22);
			this.checkModeVisualLayers.Text = "Visual Layers";
			// 
			// checkModeCollisionInfo
			// 
			this.checkModeCollisionInfo.Image = global::Duality.Editor.Plugins.Tilemaps.Properties.Resources.IconTilesetCollisionInfo;
			this.checkModeCollisionInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.checkModeCollisionInfo.Name = "checkModeCollisionInfo";
			this.checkModeCollisionInfo.Size = new System.Drawing.Size(97, 22);
			this.checkModeCollisionInfo.Text = "Collision Info";
			// 
			// layerView
			// 
			this.layerView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.layerView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.layerView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.layerView.ColumnHeaderHeight = 0;
			this.layerView.DefaultToolTipProvider = null;
			this.layerView.DragDropMarkColor = System.Drawing.Color.Black;
			this.layerView.FullRowSelect = true;
			this.layerView.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.layerView.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.layerView.Indent = 0;
			this.layerView.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.layerView.Location = new System.Drawing.Point(0, 50);
			this.layerView.Margin = new System.Windows.Forms.Padding(0);
			this.layerView.Model = null;
			this.layerView.Name = "layerView";
			this.layerView.NodeFilter = null;
			this.layerView.SelectedNode = null;
			this.layerView.ShowLines = false;
			this.layerView.ShowPlusMinus = false;
			this.layerView.Size = new System.Drawing.Size(186, 302);
			this.layerView.TabIndex = 1;
			// 
			// tilesetView
			// 
			this.tilesetView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tilesetView.AutoScroll = true;
			this.tilesetView.Location = new System.Drawing.Point(186, 50);
			this.tilesetView.Margin = new System.Windows.Forms.Padding(0);
			this.tilesetView.Name = "tilesetView";
			this.tilesetView.Size = new System.Drawing.Size(317, 302);
			this.tilesetView.Spacing = new System.Drawing.Size(0, 0);
			this.tilesetView.TabIndex = 2;
			this.tilesetView.TabStop = true;
			// 
			// toolStripEdit
			// 
			this.toolStripEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.toolStripEdit.GripMargin = new System.Windows.Forms.Padding(0);
			this.toolStripEdit.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonAddLayer,
            this.buttonRemoveLayer,
            this.buttonBrightness});
			this.toolStripEdit.Location = new System.Drawing.Point(0, 25);
			this.toolStripEdit.Name = "toolStripEdit";
			this.toolStripEdit.Size = new System.Drawing.Size(503, 25);
			this.toolStripEdit.TabIndex = 0;
			// 
			// buttonAddLayer
			// 
			this.buttonAddLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonAddLayer.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddLayer.Image")));
			this.buttonAddLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonAddLayer.Name = "buttonAddLayer";
			this.buttonAddLayer.Size = new System.Drawing.Size(23, 22);
			this.buttonAddLayer.Text = "Add";
			// 
			// buttonRemoveLayer
			// 
			this.buttonRemoveLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonRemoveLayer.Image = ((System.Drawing.Image)(resources.GetObject("buttonRemoveLayer.Image")));
			this.buttonRemoveLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonRemoveLayer.Name = "buttonRemoveLayer";
			this.buttonRemoveLayer.Size = new System.Drawing.Size(23, 22);
			this.buttonRemoveLayer.Text = "Remove";
			// 
			// buttonBrightness
			// 
			this.buttonBrightness.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.buttonBrightness.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonBrightness.Image = global::Duality.Editor.Plugins.Tilemaps.Properties.Resources.TilesetViewBrightness;
			this.buttonBrightness.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonBrightness.Name = "buttonBrightness";
			this.buttonBrightness.Size = new System.Drawing.Size(23, 22);
			this.buttonBrightness.Text = "Toggle Background";
			// 
			// TilesetEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.ClientSize = new System.Drawing.Size(503, 352);
			this.Controls.Add(this.toolStripEdit);
			this.Controls.Add(this.toolStripModeSelect);
			this.Controls.Add(this.layerView);
			this.Controls.Add(this.tilesetView);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TilesetEditor";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
			this.ShowInTaskbar = false;
			this.Text = "Tileset Editor";
			this.toolStripModeSelect.ResumeLayout(false);
			this.toolStripModeSelect.PerformLayout();
			this.toolStripEdit.ResumeLayout(false);
			this.toolStripEdit.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStripModeSelect;
		private System.Windows.Forms.ToolStripButton checkModeVisualLayers;
		private System.Windows.Forms.ToolStripButton checkModeCollisionInfo;
		private Aga.Controls.Tree.TreeViewAdv layerView;
		private TilesetView tilesetView;
		private System.Windows.Forms.ToolStrip toolStripEdit;
		private System.Windows.Forms.ToolStripButton buttonAddLayer;
		private System.Windows.Forms.ToolStripButton buttonRemoveLayer;
		private System.Windows.Forms.ToolStripButton buttonBrightness;
	}
}