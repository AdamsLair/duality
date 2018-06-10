namespace Duality.Editor.Plugins.Base.Forms
{
	partial class PixmapSlicerForm
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
			this.stateControlToolStrip = new System.Windows.Forms.ToolStrip();
			this.buttonBrightness = new System.Windows.Forms.ToolStripButton();
			this.buttonIndices = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.buttonDefaultZoom = new System.Windows.Forms.ToolStripButton();
			this.buttonZoomOut = new System.Windows.Forms.ToolStripButton();
			this.buttonZoomIn = new System.Windows.Forms.ToolStripButton();
			this.pixmapView = new Duality.Editor.Plugins.Base.Forms.PixmapSlicer.PixmapSlicingView();
			this.stateControlToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// stateControlToolStrip
			// 
			this.stateControlToolStrip.AutoSize = false;
			this.stateControlToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.stateControlToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
			this.stateControlToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.stateControlToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonBrightness,
            this.buttonIndices,
            this.toolStripSeparator1,
            this.buttonDefaultZoom,
            this.buttonZoomOut,
            this.buttonZoomIn});
			this.stateControlToolStrip.Location = new System.Drawing.Point(0, 0);
			this.stateControlToolStrip.Name = "stateControlToolStrip";
			this.stateControlToolStrip.Size = new System.Drawing.Size(428, 25);
			this.stateControlToolStrip.TabIndex = 3;
			this.stateControlToolStrip.Text = "toolStrip2";
			// 
			// buttonBrightness
			// 
			this.buttonBrightness.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.buttonBrightness.CheckOnClick = true;
			this.buttonBrightness.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonBrightness.Image = global::Duality.Editor.Plugins.Base.Properties.EditorBaseRes.IconViewBrightness;
			this.buttonBrightness.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonBrightness.Name = "buttonBrightness";
			this.buttonBrightness.Size = new System.Drawing.Size(23, 22);
			this.buttonBrightness.ToolTipText = "Toggle Background Color";
			this.buttonBrightness.CheckedChanged += new System.EventHandler(this.buttonBrightness_CheckedChanged);
			// 
			// buttonIndices
			// 
			this.buttonIndices.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.buttonIndices.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonIndices.Image = global::Duality.Editor.Plugins.Base.Properties.EditorBaseRes.IconHideIndices;
			this.buttonIndices.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonIndices.Name = "buttonIndices";
			this.buttonIndices.Size = new System.Drawing.Size(23, 22);
			this.buttonIndices.ToolTipText = "Switch Indices Display Mode";
			this.buttonIndices.Click += new System.EventHandler(this.buttonIndices_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// buttonDefaultZoom
			// 
			this.buttonDefaultZoom.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.buttonDefaultZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonDefaultZoom.Image = global::Duality.Editor.Plugins.Base.Properties.EditorBaseRes.IconZoomDefault;
			this.buttonDefaultZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonDefaultZoom.Name = "buttonDefaultZoom";
			this.buttonDefaultZoom.Size = new System.Drawing.Size(23, 22);
			this.buttonDefaultZoom.ToolTipText = "Default Zoom";
			this.buttonDefaultZoom.Click += new System.EventHandler(this.buttonDefaultZoom_Click);
			// 
			// buttonZoomOut
			// 
			this.buttonZoomOut.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.buttonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonZoomOut.Image = global::Duality.Editor.Plugins.Base.Properties.EditorBaseRes.IconZoomOut;
			this.buttonZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonZoomOut.Name = "buttonZoomOut";
			this.buttonZoomOut.Size = new System.Drawing.Size(23, 22);
			this.buttonZoomOut.ToolTipText = "Zoom Out";
			this.buttonZoomOut.Click += new System.EventHandler(this.buttonZoomOut_Click);
			// 
			// buttonZoomIn
			// 
			this.buttonZoomIn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.buttonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonZoomIn.Image = global::Duality.Editor.Plugins.Base.Properties.EditorBaseRes.IconZoomIn;
			this.buttonZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonZoomIn.Name = "buttonZoomIn";
			this.buttonZoomIn.Size = new System.Drawing.Size(23, 22);
			this.buttonZoomIn.ToolTipText = "Zoom In";
			this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
			// 
			// pixmapView
			// 
			this.pixmapView.AutoScroll = true;
			this.pixmapView.DarkMode = false;
			this.pixmapView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pixmapView.Location = new System.Drawing.Point(0, 25);
			this.pixmapView.Name = "pixmapView";
			this.pixmapView.NumberingStyle = Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States.PixmapNumberingStyle.Hovered;
			this.pixmapView.ScaleFactor = 1F;
			this.pixmapView.Size = new System.Drawing.Size(428, 229);
			this.pixmapView.TabIndex = 4;
			this.pixmapView.TargetPixmap = null;
			this.pixmapView.PaintContentOverlay += new System.EventHandler<System.Windows.Forms.PaintEventArgs>(this.pixmapView_PaintContentOverlay);
			this.pixmapView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.pixmapView_KeyUp);
			this.pixmapView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pixmapView_MouseDown);
			this.pixmapView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pixmapView_MouseMove);
			this.pixmapView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pixmapView_MouseUp);
			// 
			// PixmapSlicerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(428, 254);
			this.Controls.Add(this.pixmapView);
			this.Controls.Add(this.stateControlToolStrip);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(1);
			this.Name = "PixmapSlicerForm";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
			this.Text = "PixmapSlicer";
			this.stateControlToolStrip.ResumeLayout(false);
			this.stateControlToolStrip.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ToolStrip stateControlToolStrip;
		private System.Windows.Forms.ToolStripButton buttonBrightness;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton buttonDefaultZoom;
		private System.Windows.Forms.ToolStripButton buttonZoomOut;
		private System.Windows.Forms.ToolStripButton buttonZoomIn;
		private System.Windows.Forms.ToolStripButton buttonIndices;
		private Duality.Editor.Plugins.Base.Forms.PixmapSlicer.PixmapSlicingView pixmapView;
	}
}