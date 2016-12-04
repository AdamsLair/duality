using System;
using System.Drawing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using Duality;
using Duality.Editor;

namespace Duality.Editor.Plugins.LogView
{
	public partial class LogView : DockContent
	{
		private int unseenWarnings = 0;
		private int unseenErrors   = 0;
		private Dictionary<string,ToolStripButton> sourceFilterButtons = new Dictionary<string,ToolStripButton>();


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
			if (this.DockPanel.ActiveAutoHideContent == this)
			{
				this.MarkAsRead();
			}
		}
		
		internal void SaveUserData(XElement node)
		{
			node.SetElementValue("ShowMessages", this.buttonMessages.Checked);
			node.SetElementValue("ShowWarnings", this.buttonWarnings.Checked);
			node.SetElementValue("ShowErrors", this.buttonErrors.Checked);
			node.SetElementValue("ShowCore", this.buttonCore.Checked);
			node.SetElementValue("ShowEditor", this.buttonEditor.Checked);
			node.SetElementValue("ShowGame", this.buttonGame.Checked);
			node.SetElementValue("AutoClear", this.checkAutoClear.Checked);
			node.SetElementValue("PauseOnError", this.buttonPauseOnError.Checked);
		}
		internal void LoadUserData(XElement node)
		{
			bool tryParseBool;

			if (node.GetElementValue("ShowMessages", out tryParseBool)) this.buttonMessages.Checked = tryParseBool;
			if (node.GetElementValue("ShowWarnings", out tryParseBool)) this.buttonWarnings.Checked = tryParseBool;
			if (node.GetElementValue("ShowErrors", out tryParseBool))   this.buttonErrors.Checked = tryParseBool;
			if (node.GetElementValue("ShowCore", out tryParseBool))     this.buttonCore.Checked = tryParseBool;
			if (node.GetElementValue("ShowEditor", out tryParseBool))   this.buttonEditor.Checked = tryParseBool;
			if (node.GetElementValue("ShowGame", out tryParseBool))     this.buttonGame.Checked = tryParseBool;
			if (node.GetElementValue("AutoClear", out tryParseBool))    this.checkAutoClear.Checked = tryParseBool;
			if (node.GetElementValue("PauseOnError", out tryParseBool)) this.buttonPauseOnError.Checked = tryParseBool;

			this.logEntryList.SetSourceFilter(Logs.Core.Id, !this.buttonCore.Checked);
			this.logEntryList.SetSourceFilter(Logs.Editor.Id, !this.buttonEditor.Checked);
			this.logEntryList.SetSourceFilter(Logs.Game.Id, !this.buttonGame.Checked);
			this.logEntryList.SetTypeFilter(LogMessageType.Message, !this.buttonMessages.Checked);
			this.logEntryList.SetTypeFilter(LogMessageType.Warning, !this.buttonWarnings.Checked);
			this.logEntryList.SetTypeFilter(LogMessageType.Error, !this.buttonErrors.Checked);
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
		}
		private void buttonMessages_CheckedChanged(object sender, EventArgs e)
		{
			this.logEntryList.SetTypeFilter(LogMessageType.Message, !this.buttonMessages.Checked);
		}
		private void buttonWarnings_CheckedChanged(object sender, EventArgs e)
		{
			this.logEntryList.SetTypeFilter(LogMessageType.Warning, !this.buttonWarnings.Checked);
		}
		private void buttonErrors_CheckedChanged(object sender, EventArgs e)
		{
			this.logEntryList.SetTypeFilter(LogMessageType.Error, !this.buttonErrors.Checked);
		}
		private void buttonPauseOnError_CheckedChanged(object sender, EventArgs e) {}
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

			bool containsWarning = false;
			bool containsError = false;
			for (int i = 0; i < e.ViewEntries.Count; i++)
			{
				EditorLogEntry logEntry = e.ViewEntries[i].LogEntry;
				LogMessageType type = logEntry.Content.Type;
				if (type == LogMessageType.Warning)
					containsWarning = true;
				else if (type == LogMessageType.Error)
					containsError = true;

				this.AddSourceFilterButton(logEntry.Source);
			}

			if (isHidden)
			{
				if (containsWarning)
				{
					this.unseenWarnings++;
					unseenChanges = true;
				}
				else if (containsError)
				{
					if (this.unseenErrors == 0) System.Media.SystemSounds.Hand.Play();
					this.unseenErrors++;
					unseenChanges = true;
				}
			}

			if (unseenChanges) this.UpdateTabText();

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
			if (this.checkAutoClear.Checked) this.actionClear_ButtonClick(sender, e);
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
