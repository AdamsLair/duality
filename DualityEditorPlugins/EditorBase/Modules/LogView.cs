using System;
using System.Drawing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using Duality;
using DualityEditor;

namespace EditorBase
{
	public partial class LogView : DockContent
	{
		private	int unseenWarnings	= 0;
		private	int	unseenErrors	= 0;
		private	List<DataLogOutput.LogEntry> logSchedule = new List<DataLogOutput.LogEntry>();


		public LogView()
		{
			this.InitializeComponent();

			this.splitContainer.SplitterDistance = 1000;
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			this.toolStrip.Renderer = new DualityEditor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
		}
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			Log.LogData.NewEntry += LogData_NewEntry;
			this.logEntryList.BindToOutput(Log.LogData);
			this.logEntryList.ScrollToEnd();

			foreach (var entry in Log.LogData.Data)
			{
				if (entry.Type == LogMessageType.Warning)
					this.unseenWarnings++;
				else if (entry.Type == LogMessageType.Error)
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

			this.logEntryList.BindToOutput(null);
			Log.LogData.NewEntry -= LogData_NewEntry;

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
		
		internal void SaveUserData(System.Xml.XmlElement node)
		{
			node.SetAttribute("showMessages", this.buttonMessages.Checked.ToString(CultureInfo.InvariantCulture));
			node.SetAttribute("showWarnings", this.buttonWarnings.Checked.ToString(CultureInfo.InvariantCulture));
			node.SetAttribute("showErrors", this.buttonErrors.Checked.ToString(CultureInfo.InvariantCulture));
			node.SetAttribute("showCore", this.buttonCore.Checked.ToString(CultureInfo.InvariantCulture));
			node.SetAttribute("showEditor", this.buttonEditor.Checked.ToString(CultureInfo.InvariantCulture));
			node.SetAttribute("showGame", this.buttonGame.Checked.ToString(CultureInfo.InvariantCulture));
			node.SetAttribute("autoClear", this.checkAutoClear.Checked.ToString(CultureInfo.InvariantCulture));
			node.SetAttribute("pauseOnError", this.buttonPauseOnError.Checked.ToString(CultureInfo.InvariantCulture));
		}
		internal void LoadUserData(System.Xml.XmlElement node)
		{
			bool tryParseBool;

			if (bool.TryParse(node.GetAttribute("showMessages"), out tryParseBool))
				this.buttonMessages.Checked = tryParseBool;
			if (bool.TryParse(node.GetAttribute("showWarnings"), out tryParseBool))
				this.buttonWarnings.Checked = tryParseBool;
			if (bool.TryParse(node.GetAttribute("showErrors"), out tryParseBool))
				this.buttonErrors.Checked = tryParseBool;
			if (bool.TryParse(node.GetAttribute("showCore"), out tryParseBool))
				this.buttonCore.Checked = tryParseBool;
			if (bool.TryParse(node.GetAttribute("showEditor"), out tryParseBool))
				this.buttonEditor.Checked = tryParseBool;
			if (bool.TryParse(node.GetAttribute("showGame"), out tryParseBool))
				this.buttonGame.Checked = tryParseBool;
			if (bool.TryParse(node.GetAttribute("autoClear"), out tryParseBool))
				this.checkAutoClear.Checked = tryParseBool;
			if (bool.TryParse(node.GetAttribute("pauseOnError"), out tryParseBool))
				this.buttonPauseOnError.Checked = tryParseBool;

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
					PluginRes.EditorBaseRes.LogView_Errors,
					PluginRes.EditorBaseRes.LogView_Warnings);
			}
			else if (this.unseenErrors > 0)
			{
				this.DockHandler.TabText = this.Text + string.Format(" ({0} {1})", 
					this.unseenErrors,
					PluginRes.EditorBaseRes.LogView_Errors);
			}
			else if (this.unseenWarnings > 0)
			{
				this.DockHandler.TabText = this.Text + string.Format(" ({0} {1})", 
					this.unseenWarnings,
					PluginRes.EditorBaseRes.LogView_Warnings);
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
			this.logEntryList.DisplayMinTime = DateTime.Now;
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
			DataLogOutput.LogEntry logEntry = e.Entry.LogEntry;
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
		}
		private void LogData_NewEntry(object sender, DataLogOutput.LogEntryEventArgs e)
		{
			if (this.InvokeRequired) return;
			bool pause = 
				e.Entry.Type == LogMessageType.Error && 
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
