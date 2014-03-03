namespace Duality.Editor.Plugins.LogView
{
	partial class LogView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogView));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.labelVisibility = new System.Windows.Forms.ToolStripLabel();
			this.buttonErrors = new System.Windows.Forms.ToolStripButton();
			this.buttonWarnings = new System.Windows.Forms.ToolStripButton();
			this.buttonMessages = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.buttonCore = new System.Windows.Forms.ToolStripButton();
			this.buttonEditor = new System.Windows.Forms.ToolStripButton();
			this.buttonGame = new System.Windows.Forms.ToolStripButton();
			this.actionClear = new System.Windows.Forms.ToolStripSplitButton();
			this.checkAutoClear = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonPauseOnError = new System.Windows.Forms.ToolStripButton();
			this.textBoxEntry = new System.Windows.Forms.TextBox();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.logEntryList = new Duality.Editor.Plugins.LogView.LogEntryList();
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.toolStrip.GripMargin = new System.Windows.Forms.Padding(0);
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelVisibility,
            this.buttonErrors,
            this.buttonWarnings,
            this.buttonMessages,
            this.toolStripSeparator1,
            this.buttonCore,
            this.buttonEditor,
            this.buttonGame,
            this.actionClear,
            this.buttonPauseOnError});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(683, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip";
			// 
			// labelVisibility
			// 
			this.labelVisibility.Name = "labelVisibility";
			this.labelVisibility.Size = new System.Drawing.Size(48, 22);
			this.labelVisibility.Text = "Display:";
			// 
			// buttonErrors
			// 
			this.buttonErrors.Checked = true;
			this.buttonErrors.CheckOnClick = true;
			this.buttonErrors.CheckState = System.Windows.Forms.CheckState.Checked;
			this.buttonErrors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonErrors.Image = global::Duality.Editor.Plugins.LogView.Properties.Resources.log_error;
			this.buttonErrors.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonErrors.Margin = new System.Windows.Forms.Padding(1, 1, 1, 2);
			this.buttonErrors.Name = "buttonErrors";
			this.buttonErrors.Size = new System.Drawing.Size(23, 22);
			this.buttonErrors.Text = "Errors";
			this.buttonErrors.CheckedChanged += new System.EventHandler(this.buttonErrors_CheckedChanged);
			// 
			// buttonWarnings
			// 
			this.buttonWarnings.Checked = true;
			this.buttonWarnings.CheckOnClick = true;
			this.buttonWarnings.CheckState = System.Windows.Forms.CheckState.Checked;
			this.buttonWarnings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonWarnings.Image = global::Duality.Editor.Plugins.LogView.Properties.Resources.log_warning;
			this.buttonWarnings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonWarnings.Margin = new System.Windows.Forms.Padding(1, 1, 1, 2);
			this.buttonWarnings.Name = "buttonWarnings";
			this.buttonWarnings.Size = new System.Drawing.Size(23, 22);
			this.buttonWarnings.Text = "Warnings";
			this.buttonWarnings.CheckedChanged += new System.EventHandler(this.buttonWarnings_CheckedChanged);
			// 
			// buttonMessages
			// 
			this.buttonMessages.Checked = true;
			this.buttonMessages.CheckOnClick = true;
			this.buttonMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this.buttonMessages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonMessages.Image = global::Duality.Editor.Plugins.LogView.Properties.Resources.log_message;
			this.buttonMessages.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonMessages.Margin = new System.Windows.Forms.Padding(1, 1, 1, 2);
			this.buttonMessages.Name = "buttonMessages";
			this.buttonMessages.Size = new System.Drawing.Size(23, 22);
			this.buttonMessages.Text = "Messages";
			this.buttonMessages.CheckedChanged += new System.EventHandler(this.buttonMessages_CheckedChanged);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// buttonCore
			// 
			this.buttonCore.Checked = true;
			this.buttonCore.CheckOnClick = true;
			this.buttonCore.CheckState = System.Windows.Forms.CheckState.Checked;
			this.buttonCore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonCore.Image = global::Duality.Editor.Plugins.LogView.Properties.Resources.log_core;
			this.buttonCore.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonCore.Margin = new System.Windows.Forms.Padding(1, 1, 1, 2);
			this.buttonCore.Name = "buttonCore";
			this.buttonCore.Size = new System.Drawing.Size(23, 22);
			this.buttonCore.Text = "Core";
			this.buttonCore.CheckedChanged += new System.EventHandler(this.buttonCore_CheckedChanged);
			// 
			// buttonEditor
			// 
			this.buttonEditor.Checked = true;
			this.buttonEditor.CheckOnClick = true;
			this.buttonEditor.CheckState = System.Windows.Forms.CheckState.Checked;
			this.buttonEditor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonEditor.Image = global::Duality.Editor.Plugins.LogView.Properties.Resources.log_editor;
			this.buttonEditor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonEditor.Margin = new System.Windows.Forms.Padding(1, 1, 1, 2);
			this.buttonEditor.Name = "buttonEditor";
			this.buttonEditor.Size = new System.Drawing.Size(23, 22);
			this.buttonEditor.Text = "Editor";
			this.buttonEditor.CheckedChanged += new System.EventHandler(this.buttonEditor_CheckedChanged);
			// 
			// buttonGame
			// 
			this.buttonGame.Checked = true;
			this.buttonGame.CheckOnClick = true;
			this.buttonGame.CheckState = System.Windows.Forms.CheckState.Checked;
			this.buttonGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonGame.Image = global::Duality.Editor.Plugins.LogView.Properties.Resources.log_game;
			this.buttonGame.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonGame.Margin = new System.Windows.Forms.Padding(1, 1, 1, 2);
			this.buttonGame.Name = "buttonGame";
			this.buttonGame.Size = new System.Drawing.Size(23, 22);
			this.buttonGame.Text = "Game";
			this.buttonGame.CheckedChanged += new System.EventHandler(this.buttonGame_CheckedChanged);
			// 
			// actionClear
			// 
			this.actionClear.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.actionClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.actionClear.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAutoClear});
			this.actionClear.Image = global::Duality.Editor.Plugins.LogView.Properties.Resources.cross;
			this.actionClear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.actionClear.Name = "actionClear";
			this.actionClear.Size = new System.Drawing.Size(32, 22);
			this.actionClear.Text = "Clear View";
			this.actionClear.ButtonClick += new System.EventHandler(this.actionClear_ButtonClick);
			// 
			// checkAutoClear
			// 
			this.checkAutoClear.Checked = true;
			this.checkAutoClear.CheckOnClick = true;
			this.checkAutoClear.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAutoClear.Name = "checkAutoClear";
			this.checkAutoClear.Size = new System.Drawing.Size(143, 22);
			this.checkAutoClear.Text = "Clear on Play";
			// 
			// buttonPauseOnError
			// 
			this.buttonPauseOnError.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.buttonPauseOnError.Checked = true;
			this.buttonPauseOnError.CheckOnClick = true;
			this.buttonPauseOnError.CheckState = System.Windows.Forms.CheckState.Checked;
			this.buttonPauseOnError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonPauseOnError.Image = global::Duality.Editor.Plugins.LogView.Properties.Resources.bell;
			this.buttonPauseOnError.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonPauseOnError.Margin = new System.Windows.Forms.Padding(1, 1, 1, 2);
			this.buttonPauseOnError.Name = "buttonPauseOnError";
			this.buttonPauseOnError.Size = new System.Drawing.Size(23, 22);
			this.buttonPauseOnError.Text = "Pause on Error";
			this.buttonPauseOnError.CheckedChanged += new System.EventHandler(this.buttonPauseOnError_CheckedChanged);
			// 
			// textBoxEntry
			// 
			this.textBoxEntry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.textBoxEntry.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxEntry.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxEntry.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxEntry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.textBoxEntry.Location = new System.Drawing.Point(0, 0);
			this.textBoxEntry.Multiline = true;
			this.textBoxEntry.Name = "textBoxEntry";
			this.textBoxEntry.ReadOnly = true;
			this.textBoxEntry.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxEntry.Size = new System.Drawing.Size(681, 79);
			this.textBoxEntry.TabIndex = 3;
			this.textBoxEntry.WordWrap = false;
			this.textBoxEntry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxEntry_KeyDown);
			// 
			// splitContainer
			// 
			this.splitContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
			this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer.Location = new System.Drawing.Point(0, 25);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.logEntryList);
			this.splitContainer.Panel1MinSize = 50;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.textBoxEntry);
			this.splitContainer.Panel2MinSize = 20;
			this.splitContainer.Size = new System.Drawing.Size(683, 170);
			this.splitContainer.SplitterDistance = 85;
			this.splitContainer.TabIndex = 4;
			// 
			// logEntryList
			// 
			this.logEntryList.AutoScroll = true;
			this.logEntryList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.logEntryList.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
			this.logEntryList.DisplayFilter = ((Duality.Editor.Plugins.LogView.LogEntryList.MessageFilter)((((((Duality.Editor.Plugins.LogView.LogEntryList.MessageFilter.SourceCore | Duality.Editor.Plugins.LogView.LogEntryList.MessageFilter.SourceEditor)
						| Duality.Editor.Plugins.LogView.LogEntryList.MessageFilter.SourceGame)
						| Duality.Editor.Plugins.LogView.LogEntryList.MessageFilter.TypeMessage)
						| Duality.Editor.Plugins.LogView.LogEntryList.MessageFilter.TypeWarning)
						| Duality.Editor.Plugins.LogView.LogEntryList.MessageFilter.TypeError)));
			this.logEntryList.DisplayMinTime = new System.DateTime(((long)(0)));
			this.logEntryList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logEntryList.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.logEntryList.Location = new System.Drawing.Point(0, 0);
			this.logEntryList.Name = "logEntryList";
			this.logEntryList.ScrollOffset = 0;
			this.logEntryList.SelectedEntry = null;
			this.logEntryList.Size = new System.Drawing.Size(681, 83);
			this.logEntryList.TabIndex = 2;
			this.logEntryList.SelectionChanged += new System.EventHandler(this.logEntryList_SelectionChanged);
			this.logEntryList.NewEntry += new System.EventHandler<Duality.Editor.Plugins.LogView.LogEntryList.ViewEntryEventArgs>(this.logEntryList_NewEntry);
			this.logEntryList.Enter += new System.EventHandler(this.logEntryList_Enter);
			// 
			// LogView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(683, 195);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.toolStrip);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "LogView";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide;
			this.Text = "Log View";
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton buttonCore;
		private System.Windows.Forms.ToolStripButton buttonEditor;
		private System.Windows.Forms.ToolStripButton buttonGame;
		private System.Windows.Forms.ToolStripButton buttonMessages;
		private System.Windows.Forms.ToolStripButton buttonErrors;
		private System.Windows.Forms.ToolStripButton buttonWarnings;
		private System.Windows.Forms.ToolStripLabel labelVisibility;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSplitButton actionClear;
		private System.Windows.Forms.ToolStripMenuItem checkAutoClear;
		private LogEntryList logEntryList;
		private System.Windows.Forms.ToolStripButton buttonPauseOnError;
		private System.Windows.Forms.TextBox textBoxEntry;
		private System.Windows.Forms.SplitContainer splitContainer;
	}
}