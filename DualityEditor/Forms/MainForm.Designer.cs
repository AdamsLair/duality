namespace DualityEditor.Forms
{
	partial class MainForm
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
			this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
			this.mainToolStrip = new System.Windows.Forms.ToolStrip();
			this.actionSaveAll = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.actionOpenCode = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.actionRunApp = new System.Windows.Forms.ToolStripButton();
			this.actionDebugApp = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.actionRunSandbox = new System.Windows.Forms.ToolStripButton();
			this.actionStepSandbox = new System.Windows.Forms.ToolStripButton();
			this.actionPauseSandbox = new System.Windows.Forms.ToolStripButton();
			this.actionStopSandbox = new System.Windows.Forms.ToolStripButton();
			this.splitButtonBackupSettings = new System.Windows.Forms.ToolStripSplitButton();
			this.checkBackups = new System.Windows.Forms.ToolStripMenuItem();
			this.menuAutosave = new System.Windows.Forms.ToolStripMenuItem();
			this.optionAutosaveDisabled = new System.Windows.Forms.ToolStripMenuItem();
			this.optionAutosaveTenMinutes = new System.Windows.Forms.ToolStripMenuItem();
			this.optionAutosaveThirtyMinutes = new System.Windows.Forms.ToolStripMenuItem();
			this.optionAutoSaveOneHour = new System.Windows.Forms.ToolStripMenuItem();
			this.selectFormattingMethod = new System.Windows.Forms.ToolStripSplitButton();
			this.formatBinary = new System.Windows.Forms.ToolStripMenuItem();
			this.formatXml = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.formatUpdateAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mainToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// dockPanel
			// 
			this.dockPanel.ActiveAutoHideContent = null;
			this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
			this.dockPanel.Location = new System.Drawing.Point(0, 49);
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.ShowDocumentIcon = true;
			this.dockPanel.Size = new System.Drawing.Size(916, 639);
			dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
			tabGradient1.EndColor = System.Drawing.SystemColors.Control;
			tabGradient1.StartColor = System.Drawing.SystemColors.Control;
			tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin1.TabGradient = tabGradient1;
			dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
			tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
			dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
			tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
			dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
			tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
			tabGradient5.EndColor = System.Drawing.SystemColors.Control;
			tabGradient5.StartColor = System.Drawing.SystemColors.Control;
			tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
			dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
			tabGradient6.EndColor = System.Drawing.SystemColors.InactiveCaption;
			tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient6.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
			dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
			tabGradient7.EndColor = System.Drawing.Color.Transparent;
			tabGradient7.StartColor = System.Drawing.Color.Transparent;
			tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
			dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
			dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
			this.dockPanel.Skin = dockPanelSkin1;
			this.dockPanel.TabIndex = 0;
			// 
			// mainMenuStrip
			// 
			this.mainMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.mainMenuStrip.Name = "mainMenuStrip";
			this.mainMenuStrip.Size = new System.Drawing.Size(916, 24);
			this.mainMenuStrip.TabIndex = 2;
			this.mainMenuStrip.Text = "menuStrip1";
			// 
			// BottomToolStripPanel
			// 
			this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.BottomToolStripPanel.Name = "BottomToolStripPanel";
			this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// TopToolStripPanel
			// 
			this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.TopToolStripPanel.Name = "TopToolStripPanel";
			this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// RightToolStripPanel
			// 
			this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.RightToolStripPanel.Name = "RightToolStripPanel";
			this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// LeftToolStripPanel
			// 
			this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.LeftToolStripPanel.Name = "LeftToolStripPanel";
			this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// ContentPanel
			// 
			this.ContentPanel.Size = new System.Drawing.Size(916, 639);
			// 
			// mainToolStrip
			// 
			this.mainToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionSaveAll,
            this.toolStripSeparator1,
            this.actionOpenCode,
            this.toolStripSeparator2,
            this.actionRunApp,
            this.actionDebugApp,
            this.toolStripSeparator3,
            this.actionRunSandbox,
            this.actionStepSandbox,
            this.actionPauseSandbox,
            this.actionStopSandbox,
            this.splitButtonBackupSettings,
            this.selectFormattingMethod});
			this.mainToolStrip.Location = new System.Drawing.Point(0, 24);
			this.mainToolStrip.Name = "mainToolStrip";
			this.mainToolStrip.Size = new System.Drawing.Size(916, 25);
			this.mainToolStrip.TabIndex = 4;
			// 
			// actionSaveAll
			// 
			this.actionSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionSaveAll.Image = global::DualityEditor.Properties.Resources.disk_multiple;
			this.actionSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionSaveAll.Name = "actionSaveAll";
			this.actionSaveAll.Size = new System.Drawing.Size(23, 22);
			this.actionSaveAll.Text = "Save All";
			this.actionSaveAll.Click += new System.EventHandler(this.actionSaveAll_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// actionOpenCode
			// 
			this.actionOpenCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionOpenCode.Image = global::DualityEditor.Properties.Resources.page_white_csharp;
			this.actionOpenCode.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionOpenCode.Name = "actionOpenCode";
			this.actionOpenCode.Size = new System.Drawing.Size(23, 22);
			this.actionOpenCode.Text = "Open Sourcecode";
			this.actionOpenCode.Click += new System.EventHandler(this.actionOpenCode_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// actionRunApp
			// 
			this.actionRunApp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionRunApp.Image = global::DualityEditor.Properties.Resources.application_go;
			this.actionRunApp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionRunApp.Name = "actionRunApp";
			this.actionRunApp.Size = new System.Drawing.Size(23, 22);
			this.actionRunApp.Text = "Run Game";
			this.actionRunApp.Click += new System.EventHandler(this.actionRunApp_Click);
			// 
			// actionDebugApp
			// 
			this.actionDebugApp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionDebugApp.Image = global::DualityEditor.Properties.Resources.application_bug;
			this.actionDebugApp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionDebugApp.Name = "actionDebugApp";
			this.actionDebugApp.Size = new System.Drawing.Size(23, 22);
			this.actionDebugApp.Text = "Debug Game";
			this.actionDebugApp.Click += new System.EventHandler(this.actionDebugApp_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// actionRunSandbox
			// 
			this.actionRunSandbox.AutoToolTip = false;
			this.actionRunSandbox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionRunSandbox.Image = global::DualityEditor.Properties.Resources.control_play_blue;
			this.actionRunSandbox.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionRunSandbox.Name = "actionRunSandbox";
			this.actionRunSandbox.Size = new System.Drawing.Size(23, 22);
			this.actionRunSandbox.Text = "Play";
			this.actionRunSandbox.ToolTipText = "Enter Sandbox mode";
			this.actionRunSandbox.Click += new System.EventHandler(this.actionRunSandbox_Click);
			// 
			// actionStepSandbox
			// 
			this.actionStepSandbox.AutoToolTip = false;
			this.actionStepSandbox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionStepSandbox.Image = global::DualityEditor.Properties.Resources.control_step_blue;
			this.actionStepSandbox.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionStepSandbox.Name = "actionStepSandbox";
			this.actionStepSandbox.Size = new System.Drawing.Size(23, 22);
			this.actionStepSandbox.Text = "Step Frame";
			this.actionStepSandbox.ToolTipText = "Process one Frame";
			this.actionStepSandbox.Click += new System.EventHandler(this.actionStepSandbox_Click);
			// 
			// actionPauseSandbox
			// 
			this.actionPauseSandbox.AutoToolTip = false;
			this.actionPauseSandbox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionPauseSandbox.Image = global::DualityEditor.Properties.Resources.control_pause_blue;
			this.actionPauseSandbox.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionPauseSandbox.Name = "actionPauseSandbox";
			this.actionPauseSandbox.Size = new System.Drawing.Size(23, 22);
			this.actionPauseSandbox.Text = "Pause";
			this.actionPauseSandbox.ToolTipText = "Pause the Sandbox";
			this.actionPauseSandbox.Click += new System.EventHandler(this.actionPauseSandbox_Click);
			// 
			// actionStopSandbox
			// 
			this.actionStopSandbox.AutoToolTip = false;
			this.actionStopSandbox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionStopSandbox.Image = global::DualityEditor.Properties.Resources.control_stop_blue;
			this.actionStopSandbox.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionStopSandbox.Name = "actionStopSandbox";
			this.actionStopSandbox.Size = new System.Drawing.Size(23, 22);
			this.actionStopSandbox.Text = "Stop";
			this.actionStopSandbox.ToolTipText = "Leave Sandbox mode";
			this.actionStopSandbox.Click += new System.EventHandler(this.actionStopSandbox_Click);
			// 
			// splitButtonBackupSettings
			// 
			this.splitButtonBackupSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.splitButtonBackupSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.splitButtonBackupSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkBackups,
            this.menuAutosave});
			this.splitButtonBackupSettings.Image = global::DualityEditor.Properties.Resources.drive_disk;
			this.splitButtonBackupSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.splitButtonBackupSettings.Name = "splitButtonBackupSettings";
			this.splitButtonBackupSettings.Size = new System.Drawing.Size(32, 22);
			this.splitButtonBackupSettings.Text = "Backup Settings";
			this.splitButtonBackupSettings.DropDownOpening += new System.EventHandler(this.splitButtonBackupSettings_DropDownOpening);
			this.splitButtonBackupSettings.Click += new System.EventHandler(this.splitButtonBackupSettings_Click);
			// 
			// checkBackups
			// 
			this.checkBackups.Checked = true;
			this.checkBackups.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBackups.Name = "checkBackups";
			this.checkBackups.Size = new System.Drawing.Size(123, 22);
			this.checkBackups.Text = "Backups";
			this.checkBackups.ToolTipText = "If active, Duality will backup each file before saving it.";
			this.checkBackups.Click += new System.EventHandler(this.checkBackups_Clicked);
			// 
			// menuAutosave
			// 
			this.menuAutosave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionAutosaveDisabled,
            this.optionAutosaveTenMinutes,
            this.optionAutosaveThirtyMinutes,
            this.optionAutoSaveOneHour});
			this.menuAutosave.Name = "menuAutosave";
			this.menuAutosave.Size = new System.Drawing.Size(123, 22);
			this.menuAutosave.Text = "Autosave";
			// 
			// optionAutosaveDisabled
			// 
			this.optionAutosaveDisabled.Name = "optionAutosaveDisabled";
			this.optionAutosaveDisabled.Size = new System.Drawing.Size(132, 22);
			this.optionAutosaveDisabled.Text = "Disabled";
			this.optionAutosaveDisabled.Click += new System.EventHandler(this.optionAutosaveDisabled_Clicked);
			// 
			// optionAutosaveTenMinutes
			// 
			this.optionAutosaveTenMinutes.Name = "optionAutosaveTenMinutes";
			this.optionAutosaveTenMinutes.Size = new System.Drawing.Size(132, 22);
			this.optionAutosaveTenMinutes.Text = "10 Minutes";
			this.optionAutosaveTenMinutes.Click += new System.EventHandler(this.optionAutosaveTenMinutes_Clicked);
			// 
			// optionAutosaveThirtyMinutes
			// 
			this.optionAutosaveThirtyMinutes.Checked = true;
			this.optionAutosaveThirtyMinutes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.optionAutosaveThirtyMinutes.Name = "optionAutosaveThirtyMinutes";
			this.optionAutosaveThirtyMinutes.Size = new System.Drawing.Size(132, 22);
			this.optionAutosaveThirtyMinutes.Text = "30 Minutes";
			this.optionAutosaveThirtyMinutes.Click += new System.EventHandler(this.optionAutosaveThirtyMinutes_Clicked);
			// 
			// optionAutoSaveOneHour
			// 
			this.optionAutoSaveOneHour.Name = "optionAutoSaveOneHour";
			this.optionAutoSaveOneHour.Size = new System.Drawing.Size(132, 22);
			this.optionAutoSaveOneHour.Text = "1 Hour";
			this.optionAutoSaveOneHour.Click += new System.EventHandler(this.optionAutoSaveOneHour_Clicked);
			// 
			// selectFormattingMethod
			// 
			this.selectFormattingMethod.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.selectFormattingMethod.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.selectFormattingMethod.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatBinary,
            this.formatXml,
            this.toolStripSeparator4,
            this.formatUpdateAll});
			this.selectFormattingMethod.Image = ((System.Drawing.Image)(resources.GetObject("selectFormattingMethod.Image")));
			this.selectFormattingMethod.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.selectFormattingMethod.Name = "selectFormattingMethod";
			this.selectFormattingMethod.Size = new System.Drawing.Size(32, 22);
			this.selectFormattingMethod.Text = "Project Data Format";
			this.selectFormattingMethod.Click += new System.EventHandler(this.selectFormattingMethod_Click);
			// 
			// formatBinary
			// 
			this.formatBinary.Image = global::DualityEditor.Properties.Resources.page_gear;
			this.formatBinary.Name = "formatBinary";
			this.formatBinary.Size = new System.Drawing.Size(152, 22);
			this.formatBinary.Text = "Binary";
			this.formatBinary.Click += new System.EventHandler(this.formatBinary_Click);
			// 
			// formatXml
			// 
			this.formatXml.Image = global::DualityEditor.Properties.Resources.page_code;
			this.formatXml.Name = "formatXml";
			this.formatXml.Size = new System.Drawing.Size(152, 22);
			this.formatXml.Text = "Xml";
			this.formatXml.Click += new System.EventHandler(this.formatXml_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
			// 
			// formatUpdateAll
			// 
			this.formatUpdateAll.Name = "formatUpdateAll";
			this.formatUpdateAll.Size = new System.Drawing.Size(152, 22);
			this.formatUpdateAll.Text = "Update All";
			this.formatUpdateAll.Click += new System.EventHandler(this.formatUpdateAll_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
			this.ClientSize = new System.Drawing.Size(916, 688);
			this.Controls.Add(this.dockPanel);
			this.Controls.Add(this.mainToolStrip);
			this.Controls.Add(this.mainMenuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.mainMenuStrip;
			this.Name = "MainForm";
			this.Text = "Dualitor";
			this.mainToolStrip.ResumeLayout(false);
			this.mainToolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
		private System.Windows.Forms.MenuStrip mainMenuStrip;
		private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
		private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
		private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
		private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
		private System.Windows.Forms.ToolStripContentPanel ContentPanel;
		private System.Windows.Forms.ToolStrip mainToolStrip;
		private System.Windows.Forms.ToolStripButton actionRunApp;
		private System.Windows.Forms.ToolStripButton actionDebugApp;
		private System.Windows.Forms.ToolStripButton actionSaveAll;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton actionOpenCode;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton actionPauseSandbox;
		private System.Windows.Forms.ToolStripButton actionStopSandbox;
		private System.Windows.Forms.ToolStripSplitButton selectFormattingMethod;
		private System.Windows.Forms.ToolStripMenuItem formatBinary;
		private System.Windows.Forms.ToolStripMenuItem formatXml;
		private System.Windows.Forms.ToolStripSplitButton splitButtonBackupSettings;
		private System.Windows.Forms.ToolStripMenuItem checkBackups;
		private System.Windows.Forms.ToolStripMenuItem menuAutosave;
		private System.Windows.Forms.ToolStripMenuItem optionAutosaveDisabled;
		private System.Windows.Forms.ToolStripMenuItem optionAutosaveTenMinutes;
		private System.Windows.Forms.ToolStripMenuItem optionAutosaveThirtyMinutes;
		private System.Windows.Forms.ToolStripMenuItem optionAutoSaveOneHour;
		private System.Windows.Forms.ToolStripButton actionRunSandbox;
		private System.Windows.Forms.ToolStripButton actionStepSandbox;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem formatUpdateAll;
	}
}

