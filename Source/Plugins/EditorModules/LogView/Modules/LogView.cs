using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.LogView
{
	public partial class LogView : DockContent
	{
		private LogViewSettings userSettings = new LogViewSettings();
		private int unseenWarnings = 0;
		private int unseenErrors = 0;
		private Dictionary<string,ToolStripButton> sourceFilterButtons = new Dictionary<string,ToolStripButton>();


		public LogViewSettings UserSettings
		{
			get { return this.userSettings; }
			set
			{
				if (this.userSettings != value)
				{
					this.userSettings = value;
					this.ApplyUserSettings();
				}
			}
		}


		public LogView()
		{
			this.InitializeComponent();

			this.buttonCore.Tag = Logs.Core.Id;
			this.buttonEditor.Tag = Logs.Editor.Id;
			this.buttonGame.Tag = Logs.Game.Id;
			this.ResetSourceFilterButtons();

			this.splitContainer.SplitterDistance = 1000;
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			this.toolStrip.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
		}

		public void ApplyUserSettings()
		{
			this.buttonMessages.Checked = this.userSettings.ShowMessages;
			this.buttonWarnings.Checked = this.userSettings.ShowWarnings;
			this.buttonErrors.Checked = this.userSettings.ShowErrors;

			this.buttonCore.Checked = this.userSettings.ShowCore;
			this.buttonEditor.Checked = this.userSettings.ShowEditor;
			this.buttonGame.Checked = this.userSettings.ShowGame;

			this.checkAutoClear.Checked = this.userSettings.AutoClear;
			this.buttonPauseOnError.Checked = this.userSettings.PauseOnError;

			this.logEntryList.SetSourceFilter(Logs.Core.Id, !this.userSettings.ShowCore);
			this.logEntryList.SetSourceFilter(Logs.Editor.Id, !this.userSettings.ShowEditor);
			this.logEntryList.SetSourceFilter(Logs.Game.Id, !this.userSettings.ShowGame);

			this.logEntryList.SetTypeFilter(LogMessageType.Message, !this.userSettings.ShowMessages);
			this.logEntryList.SetTypeFilter(LogMessageType.Warning, !this.userSettings.ShowWarnings);
			this.logEntryList.SetTypeFilter(LogMessageType.Error, !this.userSettings.ShowErrors);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			this.logEntryList.BindTo(DualityEditorApp.GlobalLogData);
			this.logEntryList.ScrollToEnd();

			EditorLogOutput logHistory = DualityEditorApp.GlobalLogData;
			this.unseenErrors = logHistory.ErrorCount;
			this.unseenWarnings = logHistory.WarningCount;
			this.UpdateTabText();
		}
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.DockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;
			Sandbox.Entering += this.Sandbox_Entering;
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			this.logEntryList.BindTo(null);

			this.DockPanel.ActiveContentChanged -= DockPanel_ActiveContentChanged;
			Sandbox.Entering -= this.Sandbox_Entering;
		}
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.MarkAsRead();
			this.logEntryList.Focus();
		}
		protected override void OnDockStateChanged(EventArgs e)
		{
			base.OnDockStateChanged(e);
			if (this.DockHandler.DockState != DockState.Hidden &&
				!this.DockHandler.DockState.IsAutoHide())
			{
				this.MarkAsRead();
			}
		}
		private void DockPanel_ActiveContentChanged(object sender, EventArgs e)
		{
			if (this.DockPanel == null)
				return;

			if (this.DockPanel.ActiveAutoHideContent == this)
			{
				this.MarkAsRead();
			}
		}
		
		private void MarkAsRead()
		{
			this.unseenErrors = 0;
			this.unseenWarnings = 0;
			this.UpdateTabText();
		}
		private void UpdateTabText()
		{
			if (this.unseenErrors > 0 && this.unseenWarnings > 0)
			{
				this.DockHandler.TabText = this.Text + string.Format(" ({0} {2}, {1} {3})", 
					this.unseenErrors, 
					this.unseenWarnings,
					Properties.LogViewRes.LogView_Errors,
					Properties.LogViewRes.LogView_Warnings);
			}
			else if (this.unseenErrors > 0)
			{
				this.DockHandler.TabText = this.Text + string.Format(" ({0} {1})", 
					this.unseenErrors,
					Properties.LogViewRes.LogView_Errors);
			}
			else if (this.unseenWarnings > 0)
			{
				this.DockHandler.TabText = this.Text + string.Format(" ({0} {1})", 
					this.unseenWarnings,
					Properties.LogViewRes.LogView_Warnings);
			}
			else
			{
				this.DockHandler.TabText = this.Text;
			}
		}

		private bool AddSourceFilterButton(Log source)
		{
			ToolStripButton button;
			if (this.sourceFilterButtons.TryGetValue(source.Id, out button))
				return false;

			Image image = null;
			if (source.CustomInfo != null)
				image = source.CustomInfo.GetType().GetEditorImage();

			button = new ToolStripButton(source.Name, image);
			button.Tag = source.Id;
			button.DisplayStyle = (image == null) ? ToolStripItemDisplayStyle.Text : ToolStripItemDisplayStyle.Image;
			button.CheckOnClick = true;
			button.Checked = true;
			button.CheckedChanged += this.sourceFilterButton_CheckedChanged;

			this.sourceFilterButtons.Add(source.Id, button);
			this.toolStrip.Items.Add(button);

			return true;
		}
		private bool RemoveSourceFilterButton(string sourceId)
		{
			ToolStripButton button;
			if (!this.sourceFilterButtons.TryGetValue(sourceId, out button))
				return false;

			// Special case for the default three logs
			if (sourceId == Logs.Core.Id) return false;
			if (sourceId == Logs.Editor.Id) return false;
			if (sourceId == Logs.Game.Id) return false;

			this.toolStrip.Items.Remove(button);
			this.sourceFilterButtons.Remove(sourceId);
			button.CheckedChanged -= this.sourceFilterButton_CheckedChanged;
			button.Dispose();

			return true;
		}
		private void ResetSourceFilterButtons()
		{
			this.toolStrip.SuspendLayout();
			foreach (var pair in this.sourceFilterButtons)
			{
				// Special case for the default three logs
				if (pair.Key == Logs.Core.Id) continue;
				if (pair.Key == Logs.Editor.Id) continue;
				if (pair.Key == Logs.Game.Id) continue;

				ToolStripButton button = pair.Value;
				this.toolStrip.Items.Remove(button);
				button.CheckedChanged -= this.sourceFilterButton_CheckedChanged;
				button.Dispose();
			}
			this.toolStrip.ResumeLayout();

			this.sourceFilterButtons.Clear();
			this.sourceFilterButtons.Add(Logs.Core.Id, this.buttonCore);
			this.sourceFilterButtons.Add(Logs.Editor.Id, this.buttonEditor);
			this.sourceFilterButtons.Add(Logs.Game.Id, this.buttonGame);
		}
		
		private void sourceFilterButton_CheckedChanged(object sender, EventArgs e)
		{
			ToolStripButton button = sender as ToolStripButton;
			string sourceId = button.Tag as string;
			this.logEntryList.SetSourceFilter(sourceId, !button.Checked);

			// No support for saving custom log source filter states as user settings right now, could be added later
			if (button == this.buttonCore) this.userSettings.ShowCore = button.Checked;
			else if (button == this.buttonEditor) this.userSettings.ShowEditor = button.Checked;
			else if(button == this.buttonGame) this.userSettings.ShowGame = button.Checked;
		}
		private void buttonMessages_CheckedChanged(object sender, EventArgs e)
		{
			this.userSettings.ShowMessages = this.buttonMessages.Checked;
			this.logEntryList.SetTypeFilter(LogMessageType.Message, !this.userSettings.ShowMessages);
		}
		private void buttonWarnings_CheckedChanged(object sender, EventArgs e)
		{
			this.userSettings.ShowWarnings = this.buttonWarnings.Checked;
			this.logEntryList.SetTypeFilter(LogMessageType.Warning, !this.userSettings.ShowWarnings);
		}
		private void buttonErrors_CheckedChanged(object sender, EventArgs e)
		{
			this.userSettings.ShowErrors = this.buttonErrors.Checked;
			this.logEntryList.SetTypeFilter(LogMessageType.Error, !this.userSettings.ShowErrors);
		}
		private void buttonPauseOnError_CheckedChanged(object sender, EventArgs e)
		{
			this.userSettings.PauseOnError = this.buttonPauseOnError.Checked;
		}
		private void checkAutoClear_CheckedChanged(object sender, EventArgs e)
		{
			this.userSettings.AutoClear = this.checkAutoClear.Checked;
		}
		private void actionClear_ButtonClick(object sender, EventArgs e)
		{
			this.logEntryList.Clear();
			this.MarkAsRead();
			this.ResetSourceFilterButtons();
		}
		private void logEntryList_Enter(object sender, EventArgs e)
		{
			this.MarkAsRead();
		}
		private void logEntryList_SelectionChanged(object sender, EventArgs e)
		{
			if (this.logEntryList.SelectedEntry != null)
			{
				//this.splitContainer.Panel2Collapsed = false;
				this.textBoxEntry.Text = this.logEntryList.SelectedEntry.LogEntry.Content.Message;
			}
			else
			{
				//this.splitContainer.Panel2Collapsed = true;
				this.textBoxEntry.Clear();
			}
		}
		private void logEntryList_LogEntriesAdded(object sender, LogEntryList.ViewEntryEventArgs e)
		{
			bool isHidden = this.DockHandler.DockState.IsAutoHide() && !this.ContainsFocus;
			bool unseenChanges = false;

			bool containsError = false;
			for (int i = 0; i < e.ViewEntries.Count; i++)
			{
				EditorLogEntry logEntry = e.ViewEntries[i].LogEntry;
				LogMessageType type = logEntry.Content.Type;

				if (isHidden)
				{
					if (type == LogMessageType.Warning)
					{
						this.unseenWarnings++;
						unseenChanges = true;
					}
					else if (type == LogMessageType.Error)
					{
						if (this.unseenErrors == 0) System.Media.SystemSounds.Hand.Play();
						this.unseenErrors++;
						unseenChanges = true;
					}
				}
				if (type == LogMessageType.Error)
					containsError = true;

				this.AddSourceFilterButton(logEntry.Source);
			}

			if (unseenChanges)
				this.UpdateTabText();

			bool pause = 
				containsError && 
				this.buttonPauseOnError.Checked && 
				Sandbox.State == SandboxState.Playing && 
				!Sandbox.IsChangingState;
			if (pause)
			{
				System.Media.SystemSounds.Hand.Play();
				Sandbox.Pause();
			}
		}

		private void Sandbox_Entering(object sender, EventArgs e)
		{
			if (this.userSettings.AutoClear) this.actionClear_ButtonClick(sender, e);
		}
		private void textBoxEntry_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A && e.Control)
			{
				this.textBoxEntry.SelectAll();
			}
			else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
			{
				this.textBoxEntry.DeselectAll();
				this.textBoxEntry.SelectionStart = 0;
			}
		}
	}
}
