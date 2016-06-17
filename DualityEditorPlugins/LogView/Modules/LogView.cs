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
		private int               unseenWarnings = 0;
		private int               unseenErrors   = 0;
		private RawList<LogEntry> logSchedule    = new RawList<LogEntry>();


		public LogView()
		{
			this.InitializeComponent();

			this.splitContainer.SplitterDistance = 1000;
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			this.toolStrip.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
		}
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			this.logEntryList.BindToDualityLogs();
			this.logEntryList.ScrollToEnd();

			InMemoryLogOutput logHistory = DualityEditorApp.GlobalLogData;
			for (int i = 0; i < logHistory.Entries.Count; i++)
			{
				LogMessageType type = logHistory.Entries[i].Type;
				if (type == LogMessageType.Warning)
					this.unseenWarnings++;
				else if (type == LogMessageType.Error)
					this.unseenErrors++;
			}
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

			this.logEntryList.UnbindFromDualityLogs();

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

			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.SourceCore, this.buttonCore.Checked);
			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.SourceEditor, this.buttonEditor.Checked);
			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.SourceGame, this.buttonGame.Checked);
			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.TypeMessage, this.buttonMessages.Checked);
			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.TypeWarning, this.buttonWarnings.Checked);
			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.TypeError, this.buttonErrors.Checked);
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

		private void buttonCore_CheckedChanged(object sender, EventArgs e)
		{
			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.SourceCore, this.buttonCore.Checked);
		}
		private void buttonEditor_CheckedChanged(object sender, EventArgs e)
		{
			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.SourceEditor, this.buttonEditor.Checked);
		}
		private void buttonGame_CheckedChanged(object sender, EventArgs e)
		{
			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.SourceGame, this.buttonGame.Checked);
		}
		private void buttonMessages_CheckedChanged(object sender, EventArgs e)
		{
			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.TypeMessage, this.buttonMessages.Checked);
		}
		private void buttonWarnings_CheckedChanged(object sender, EventArgs e)
		{
			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.TypeWarning, this.buttonWarnings.Checked);
		}
		private void buttonErrors_CheckedChanged(object sender, EventArgs e)
		{
			this.logEntryList.SetFilterFlag(LogEntryList.MessageFilter.TypeError, this.buttonErrors.Checked);
		}
		private void buttonPauseOnError_CheckedChanged(object sender, EventArgs e) {}
		private void actionClear_ButtonClick(object sender, EventArgs e)
		{
			this.logEntryList.Clear();
			this.MarkAsRead();
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
				this.textBoxEntry.Text = this.logEntryList.SelectedEntry.LogEntry.Message;
			}
			else
			{
				//this.splitContainer.Panel2Collapsed = true;
				this.textBoxEntry.Clear();
			}
		}
		private void logEntryList_NewEntry(object sender, LogEntryList.ViewEntryEventArgs e)
		{
			LogEntry logEntry = e.Entry.LogEntry;
			bool isHidden = this.DockHandler.DockState.IsAutoHide() && !this.ContainsFocus;
			bool unseenChanges = false;

			if (isHidden)
			{
				if (logEntry.Type == LogMessageType.Warning)
				{
					this.unseenWarnings++;
					unseenChanges = true;
				}
				else if (logEntry.Type == LogMessageType.Error)
				{
					if (this.unseenErrors == 0) System.Media.SystemSounds.Hand.Play();
					this.unseenErrors++;
					unseenChanges = true;
				}
			}

			if (unseenChanges) this.UpdateTabText();

			bool pause = 
				e.Entry.LogEntry.Type == LogMessageType.Error && 
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
