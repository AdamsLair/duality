namespace Duality.Editor.Plugins.ObjectInspector
{
	partial class ObjectInspector
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectInspector));
			this.timerSelectSched = new System.Windows.Forms.Timer(this.components);
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.buttonAutoRefresh = new System.Windows.Forms.ToolStripButton();
			this.buttonDebug = new System.Windows.Forms.ToolStripButton();
			this.buttonClone = new System.Windows.Forms.ToolStripButton();
			this.buttonLock = new System.Windows.Forms.ToolStripButton();
			this.propertyGrid = new Duality.Editor.Controls.DualitorPropertyGrid();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// timerSelectSched
			// 
			this.timerSelectSched.Interval = 50;
			this.timerSelectSched.Tick += new System.EventHandler(this.timerSelectSched_Tick);
			// 
			// toolStrip
			// 
			this.toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.toolStrip.GripMargin = new System.Windows.Forms.Padding(0);
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonAutoRefresh,
            this.buttonDebug,
            this.toolStripSeparator1,
            this.buttonClone,
            this.buttonLock});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(231, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip";
			// 
			// buttonAutoRefresh
			// 
			this.buttonAutoRefresh.CheckOnClick = true;
			this.buttonAutoRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonAutoRefresh.Image = global::Duality.Editor.Plugins.ObjectInspector.Properties.Resources.arrow_refresh;
			this.buttonAutoRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonAutoRefresh.Name = "buttonAutoRefresh";
			this.buttonAutoRefresh.Size = new System.Drawing.Size(23, 22);
			this.buttonAutoRefresh.Text = "Auto-Refresh in Sandbox";
			this.buttonAutoRefresh.CheckedChanged += new System.EventHandler(this.buttonAutoRefresh_CheckedChanged);
			// 
			// buttonDebug
			// 
			this.buttonDebug.AutoToolTip = false;
			this.buttonDebug.CheckOnClick = true;
			this.buttonDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonDebug.Image = global::Duality.Editor.Plugins.ObjectInspector.Properties.Resources.bug;
			this.buttonDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonDebug.Name = "buttonDebug";
			this.buttonDebug.Size = new System.Drawing.Size(23, 22);
			this.buttonDebug.Text = "Debug Mode";
			this.buttonDebug.ToolTipText = "Debug Mode: Show non-public members.";
			this.buttonDebug.CheckedChanged += new System.EventHandler(this.buttonDebug_CheckedChanged);
			// 
			// buttonClone
			// 
			this.buttonClone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonClone.Enabled = false;
			this.buttonClone.Image = global::Duality.Editor.Plugins.ObjectInspector.Properties.Resources.page_copy;
			this.buttonClone.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonClone.Name = "buttonClone";
			this.buttonClone.Size = new System.Drawing.Size(23, 22);
			this.buttonClone.Text = "Clone View";
			this.buttonClone.Click += new System.EventHandler(this.buttonClone_Click);
			// 
			// buttonLock
			// 
			this.buttonLock.CheckOnClick = true;
			this.buttonLock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonLock.Enabled = false;
			this.buttonLock.Image = global::Duality.Editor.Plugins.ObjectInspector.Properties.Resources.lockIcon;
			this.buttonLock.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonLock.Name = "buttonLock";
			this.buttonLock.Size = new System.Drawing.Size(23, 22);
			this.buttonLock.Text = "Lock View";
			// 
			// propertyGrid
			// 
			this.propertyGrid.AllowDrop = true;
			this.propertyGrid.AutoScroll = true;
			this.propertyGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.Location = new System.Drawing.Point(0, 25);
			this.propertyGrid.Margin = new System.Windows.Forms.Padding(0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.ReadOnly = false;
			this.propertyGrid.ShowNonPublic = false;
			this.propertyGrid.Size = new System.Drawing.Size(231, 407);
			this.propertyGrid.TabIndex = 0;
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// ObjectInspector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(231, 432);
			this.Controls.Add(this.propertyGrid);
			this.Controls.Add(this.toolStrip);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ObjectInspector";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
			this.Text = "Object Inspector";
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Duality.Editor.Controls.DualitorPropertyGrid propertyGrid;
		private System.Windows.Forms.Timer timerSelectSched;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton buttonAutoRefresh;
		private System.Windows.Forms.ToolStripButton buttonClone;
		private System.Windows.Forms.ToolStripButton buttonLock;
		private System.Windows.Forms.ToolStripButton buttonDebug;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
	}
}