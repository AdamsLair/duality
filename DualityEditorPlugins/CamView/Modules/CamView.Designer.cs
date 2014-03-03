namespace Duality.Editor.Plugins.CamView
{
	partial class CamView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CamView));
			this.stateSelector = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.focusDist = new Duality.Editor.Controls.ToolStrip.ToolStripNumericUpDown();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.camSelector = new System.Windows.Forms.ToolStripComboBox();
			this.toolbarCamera = new System.Windows.Forms.ToolStrip();
			this.layerSelector = new System.Windows.Forms.ToolStripDropDownButton();
			this.buttonResetZoom = new System.Windows.Forms.ToolStripButton();
			this.perspectiveDropDown = new System.Windows.Forms.ToolStripDropDownButton();
			this.showBgColorDialog = new System.Windows.Forms.ToolStripButton();
			this.toolbarCamera.SuspendLayout();
			this.SuspendLayout();
			// 
			// stateSelector
			// 
			this.stateSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.stateSelector.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.stateSelector.Name = "stateSelector";
			this.stateSelector.Size = new System.Drawing.Size(121, 25);
			this.stateSelector.ToolTipText = "Select the editing state of this View";
			this.stateSelector.DropDown += new System.EventHandler(this.stateSelector_DropDown);
			this.stateSelector.DropDownClosed += new System.EventHandler(this.stateSelector_DropDownClosed);
			this.stateSelector.SelectedIndexChanged += new System.EventHandler(this.stateSelector_SelectedIndexChanged);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// focusDist
			// 
			this.focusDist.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.focusDist.BackColor = System.Drawing.Color.Transparent;
			this.focusDist.DecimalPlaces = 2;
			this.focusDist.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.focusDist.Name = "focusDist";
			this.focusDist.NumBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.focusDist.NumericWidth = 75;
			this.focusDist.Size = new System.Drawing.Size(128, 22);
			this.focusDist.Text = "RefDist";
			this.focusDist.ToolTipText = "Adjust the Cameras reference distance";
			this.focusDist.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.focusDist.ValueChanged += new System.EventHandler(this.focusDist_ValueChanged);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripSeparator2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// camSelector
			// 
			this.camSelector.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.camSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.camSelector.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.camSelector.Name = "camSelector";
			this.camSelector.Size = new System.Drawing.Size(121, 25);
			this.camSelector.ToolTipText = "Select the Camera object to display in this View";
			this.camSelector.DropDown += new System.EventHandler(this.camSelector_DropDown);
			this.camSelector.DropDownClosed += new System.EventHandler(this.camSelector_DropDownClosed);
			this.camSelector.SelectedIndexChanged += new System.EventHandler(this.camSelector_SelectedIndexChanged);
			// 
			// toolbarCamera
			// 
			this.toolbarCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.toolbarCamera.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbarCamera.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stateSelector,
            this.layerSelector,
            this.toolStripSeparator1,
            this.buttonResetZoom,
            this.perspectiveDropDown,
            this.focusDist,
            this.toolStripSeparator2,
            this.showBgColorDialog,
            this.camSelector});
			this.toolbarCamera.Location = new System.Drawing.Point(0, 0);
			this.toolbarCamera.Name = "toolbarCamera";
			this.toolbarCamera.Size = new System.Drawing.Size(539, 25);
			this.toolbarCamera.TabIndex = 1;
			this.toolbarCamera.Text = "toolStrip";
			// 
			// layerSelector
			// 
			this.layerSelector.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.layerSelector.Image = global::Duality.Editor.Plugins.CamView.Properties.Resources.layers;
			this.layerSelector.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.layerSelector.Name = "layerSelector";
			this.layerSelector.Size = new System.Drawing.Size(29, 22);
			this.layerSelector.Text = "Select visible Layers";
			this.layerSelector.DropDownClosed += new System.EventHandler(this.layerSelector_DropDownClosed);
			this.layerSelector.DropDownOpening += new System.EventHandler(this.layerSelector_DropDownOpening);
			this.layerSelector.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.layerSelector_DropDownItemClicked);
			// 
			// buttonResetZoom
			// 
			this.buttonResetZoom.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.buttonResetZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonResetZoom.Image = global::Duality.Editor.Plugins.CamView.Properties.Resources.magnifier_one;
			this.buttonResetZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonResetZoom.Name = "buttonResetZoom";
			this.buttonResetZoom.Size = new System.Drawing.Size(23, 22);
			this.buttonResetZoom.Text = "Reset Camera Z";
			this.buttonResetZoom.Click += new System.EventHandler(this.buttonResetZoom_Click);
			// 
			// perspectiveDropDown
			// 
			this.perspectiveDropDown.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.perspectiveDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.perspectiveDropDown.Image = global::Duality.Editor.Plugins.CamView.Properties.Resources.shape_perspective;
			this.perspectiveDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.perspectiveDropDown.Name = "perspectiveDropDown";
			this.perspectiveDropDown.Size = new System.Drawing.Size(29, 22);
			this.perspectiveDropDown.Text = "Select Perspective Mode";
			this.perspectiveDropDown.DropDownClosed += new System.EventHandler(this.perspectiveDropDown_DropDownClosed);
			this.perspectiveDropDown.DropDownOpening += new System.EventHandler(this.perspectiveDropDown_DropDownOpening);
			this.perspectiveDropDown.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.perspectiveDropDown_DropDownItemClicked);
			// 
			// showBgColorDialog
			// 
			this.showBgColorDialog.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.showBgColorDialog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.showBgColorDialog.Image = ((System.Drawing.Image)(resources.GetObject("showBgColorDialog.Image")));
			this.showBgColorDialog.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showBgColorDialog.Name = "showBgColorDialog";
			this.showBgColorDialog.Size = new System.Drawing.Size(23, 22);
			this.showBgColorDialog.Text = "Change Background Color";
			this.showBgColorDialog.Click += new System.EventHandler(this.showBgColorDialog_Click);
			// 
			// CamView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
			this.ClientSize = new System.Drawing.Size(539, 397);
			this.Controls.Add(this.toolbarCamera);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CamView";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
			this.Text = "CamView";
			this.toolbarCamera.ResumeLayout(false);
			this.toolbarCamera.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripComboBox stateSelector;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private Duality.Editor.Controls.ToolStrip.ToolStripNumericUpDown focusDist;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton showBgColorDialog;
		private System.Windows.Forms.ToolStripComboBox camSelector;
		private System.Windows.Forms.ToolStrip toolbarCamera;
		private System.Windows.Forms.ToolStripDropDownButton layerSelector;
		private System.Windows.Forms.ToolStripButton buttonResetZoom;
		private System.Windows.Forms.ToolStripDropDownButton perspectiveDropDown;

	}
}