﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using Duality;
using Duality.Editor;

namespace Duality.Editor.Plugins.LogView
{
	public class LogEntryList : UserControl, ILogOutput
	{
		[Flags]
		public enum MessageFilter
		{
			None           = 0x0,

			SourceCore     = 0x01,
			SourceEditor   = 0x02,
			SourceGame     = 0x04,

			TypeMessage    = 0x08,
			TypeWarning    = 0x10,
			TypeError      = 0x20,

			SourceAll      = SourceCore | SourceEditor | SourceGame,
			TypeAll        = TypeMessage | TypeWarning | TypeError,
			All            = SourceAll | TypeAll
		}
		public class ViewEntry
		{
			private LogEntry     log      = default(LogEntry);
			private int          msgLines = 1;
			private int          height   = 0;
			
			public LogEntry LogEntry
			{
				get { return this.log; }
			}
			public int Height
			{
				get { return this.height; }
			}
			public int Indent
			{
				get { return this.log.Indent; }
			}
			public Image TypeIcon
			{
				get
				{
					if (this.log.Type == LogMessageType.Error) return Properties.LogViewResCache.IconLogError;
					if (this.log.Type == LogMessageType.Warning) return Properties.LogViewResCache.IconLogWarning;
					return Properties.LogViewResCache.IconLogMessage;
				}
			}
			public Image SourceIcon
			{
				get
				{
					if (this.log.Source == Log.Game) return Properties.LogViewResCache.IconLogGame;
					if (this.log.Source == Log.Editor) return Properties.LogViewResCache.IconLogEditor;
					return Properties.LogViewResCache.IconLogCore;
				}
			}

			public ViewEntry(LogEntryList parent, LogEntry log)
			{
				this.log = log;
				this.msgLines = log.Message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Length;
				this.height = Math.Max(20, 7 + this.msgLines * parent.Font.Height);
			}

			public bool Matches(MessageFilter filter)
			{
				if (this.log.Type == LogMessageType.Message && (filter & MessageFilter.TypeMessage) == MessageFilter.None) return false;
				if (this.log.Type == LogMessageType.Warning && (filter & MessageFilter.TypeWarning) == MessageFilter.None) return false;
				if (this.log.Type == LogMessageType.Error && (filter & MessageFilter.TypeError) == MessageFilter.None) return false;
				if (this.log.Source == Log.Core && (filter & MessageFilter.SourceCore) == MessageFilter.None) return false;
				if (this.log.Source == Log.Editor && (filter & MessageFilter.SourceEditor) == MessageFilter.None) return false;
				if (this.log.Source == Log.Game && (filter & MessageFilter.SourceGame) == MessageFilter.None) return false;
				return true;
			}
			public void GetFullText(StringBuilder appendTo)
			{
				appendTo.Append(this.log.Source.Prefix);
				switch (this.log.Type)
				{
					case LogMessageType.Message: appendTo.Append("Info:    "); break;
					case LogMessageType.Warning: appendTo.Append("Warning: "); break;
					case LogMessageType.Error:   appendTo.Append("Error:   "); break;
				}
				appendTo.Append(' ', this.log.Indent * 4);
				appendTo.Append(this.log.Message);
			}
			public string GetFullText()
			{
				StringBuilder builder = new StringBuilder();
				this.GetFullText(builder);
				return builder.ToString();
			}
		}
		public class ViewEntryEventArgs : EventArgs
		{
			private ViewEntry entry;
			public ViewEntry Entry
			{
				get { return this.entry; }
			}
			public ViewEntryEventArgs(ViewEntry entry)
			{
				this.entry = entry;
			}
		}


		private List<ViewEntry>     entryList          = new List<ViewEntry>();
		private List<ViewEntry>     displayedEntryList = new List<ViewEntry>();
		private MessageFilter       displayFilter      = MessageFilter.All;
		private Color               baseColor          = SystemColors.Control;
		private bool                boundToDualityLogs = false;
		private bool                scrolledToEnd      = true;
		private bool                lastSelected       = true;
		private int                 firstDisplayIndex  = 0;
		private int                 firstDisplayOffset = 0;
		private ViewEntry           hoveredEntry       = null;
		private ViewEntry           selectedEntry      = null;
		private ContextMenuStrip    entryMenu          = null;
		private Timer               timerLogSchedule   = null;
		private RawList<LogEntry>   logSchedule        = new RawList<LogEntry>();
		private bool                logScheduleActive  = false;
		private object              logScheduleLock    = new object();
		private System.ComponentModel.IContainer components = null;


		public event EventHandler SelectionChanged = null;
		public event EventHandler ContentChanged = null;
		public event EventHandler<ViewEntryEventArgs> NewEntry = null;


		public IEnumerable<ViewEntry> Entries
		{
			get { return this.entryList; }
		}
		public IEnumerable<ViewEntry> DisplayedEntries
		{
			get { return this.displayedEntryList; }
		}
		public ViewEntry SelectedEntry
		{
			get { return this.selectedEntry; }
			set
			{
				if (this.selectedEntry != value)
				{
					this.selectedEntry = value;
					this.OnSelectionChanged();
				}
			}
		}

		public int ScrollOffset
		{
			get { return -this.AutoScrollPosition.Y; }
			set { this.AutoScrollPosition = new Point(0, Math.Min(value, this.MaxScrollOffset)); }
		}
		public int MaxScrollOffset
		{
			get { return this.AutoScrollMinSize.Height - this.ClientRectangle.Height; }
		}
		public int ContentHeight
		{
			get { return this.AutoScrollMinSize.Height; }
		}
		public MessageFilter DisplayFilter
		{
			get { return this.displayFilter; }
			set 
			{
				if (this.displayFilter != value)
				{
					ViewEntry lastEntry = this.GetEntryAt(this.ScrollOffset);
					int entryOff = this.ScrollOffset - this.GetEntryOffset(lastEntry);

					this.displayFilter = value;
					this.UpdateDisplayedEntries();
					this.OnContentChanged();

					this.ScrollToEntry(lastEntry, entryOff);
				}
			}
		}
		public Color BaseColor
		{
			get { return this.baseColor; }
			set { this.baseColor = value; this.Invalidate(); }
		}
		public bool IsScrolledToEnd
		{
			get { return this.scrolledToEnd; }
		}


		public LogEntryList()
		{
			this.components = new System.ComponentModel.Container();

			this.AutoScroll = true;

			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			this.entryMenu = new ContextMenuStrip();
			this.entryMenu.Items.Add(Properties.LogViewRes.LogView_ContextMenu_CopyItem, null, this.entryMenu_CopyItem_Click);
			this.entryMenu.Items.Add(Properties.LogViewRes.LogView_ContextMenu_CopyAllItems, null, this.entryMenu_CopyAllItems_Click);

			this.timerLogSchedule = new Timer(this.components);
			this.timerLogSchedule.Interval = 50;
			this.timerLogSchedule.Tick += new EventHandler(timerLogSchedule_Tick);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		public void Clear()
		{
			this.entryList.Clear();
			this.UpdateDisplayedEntries();
			this.OnContentChanged();
		}
		public ViewEntry AddEntry(LogEntry entry)
		{
			ViewEntry viewEntry = new ViewEntry(this, entry);
			this.entryList.Add(viewEntry);
			this.UpdateDisplayedEntries();

			if (this.NewEntry != null)
				this.NewEntry(this, new ViewEntryEventArgs(viewEntry));

			this.OnContentChanged();
			return viewEntry;
		}
		public void AddEntries(LogEntry[] entries, int count)
		{
			// Update content
			List<ViewEntry> newEntries = new List<ViewEntry>(entries.Length);
			for (int i = 0; i < count; i++)
			{
				ViewEntry viewEntry = new ViewEntry(this, entries[i]);
				this.entryList.Add(viewEntry);
				newEntries.Add(viewEntry);
			}
			this.UpdateDisplayedEntries();

			// Fire events
			foreach (ViewEntry viewEntry in newEntries)
			{
				if (this.NewEntry != null)
					this.NewEntry(this, new ViewEntryEventArgs(viewEntry));
			}
			this.OnContentChanged();
		}
		public void UpdateFromDataLog(InMemoryLogOutput dataLog)
		{
			if (dataLog == null)
			{
				this.Clear();
				return;
			}

			this.entryList.Clear();
			for (int i = 0; i < dataLog.Entries.Count; i++)
				this.entryList.Add(new ViewEntry(this, dataLog.Entries[i]));
			this.UpdateDisplayedEntries();

			this.OnContentChanged();
		}
		public void BindToDualityLogs()
		{
			if (this.boundToDualityLogs) return;

			Log.AddGlobalOutput(this);
			this.UpdateFromDataLog(DualityEditorApp.GlobalLogData);
		}
		public void UnbindFromDualityLogs()
		{
			if (!this.boundToDualityLogs) return;

			Log.RemoveGlobalOutput(this);
		}
		
		public void SetFilterFlag(MessageFilter flag, bool isSet)
		{
			if (isSet)
				this.DisplayFilter |= flag;
			else
				this.DisplayFilter &= ~flag;
		}

		public void ScrollToEnd()
		{
			this.ScrollOffset = this.MaxScrollOffset;
		}
		public void ScrollToEntry(ViewEntry entry, int offsetY)
		{
			this.ScrollOffset = this.GetEntryOffset(entry) + offsetY;
		}
		public void EnsureVisible(ViewEntry entry)
		{
			int offset = this.GetEntryOffset(entry);
			if (offset - this.ScrollOffset <= entry.Height)
			{
				this.ScrollOffset = offset - entry.Height;
			}
			else if (offset - this.ScrollOffset >= this.ClientSize.Height - entry.Height)
			{
				this.ScrollOffset = offset - this.ClientSize.Height + entry.Height;
			}
		}
		public int GetEntryOffset(ViewEntry entry)
		{
			int totalHeight = 0;
			foreach (ViewEntry e in this.displayedEntryList)
			{
				if (e == entry) break;
				totalHeight += e.Height;
			}
			return totalHeight;
		}
		public ViewEntry GetEntryAt(int offsetY)
		{
			int totalHeight = 0;
			int startIndex = 0;
			if (offsetY > this.firstDisplayOffset)
			{
				totalHeight = this.firstDisplayOffset;
				startIndex = this.firstDisplayIndex;
			}
			for (int i = startIndex; i < this.displayedEntryList.Count; i++)
			{
				totalHeight += this.displayedEntryList[i].Height;
				if (totalHeight >= offsetY) return this.displayedEntryList[i];
			}
			return null;
		}
		
		private void ProcessIncomingEntries(LogEntry[] entries, int count)
		{
			bool wasAtEnd = this.IsScrolledToEnd;
			this.AddEntries(entries, count);
			if (wasAtEnd) this.ScrollToEnd();
		}
		private void UpdateFirstDisplayIndex()
		{
			this.firstDisplayIndex = 0;
			this.firstDisplayOffset = 0;

			int offsetY = 0;
			for (int i = 0; i < this.displayedEntryList.Count; i++)
			{
				int entryHeight = this.displayedEntryList[i].Height;

				if (offsetY + entryHeight >= -this.AutoScrollPosition.Y)
				{
					this.firstDisplayIndex = i;
					this.firstDisplayOffset = offsetY;
					break;
				}

				offsetY += entryHeight;
				if (offsetY > this.ClientRectangle.Height + (-this.AutoScrollPosition.Y))
					break;
			}
		}
		private void UpdateDisplayedEntries()
		{
			this.displayedEntryList.Clear();
			for (int i = 0; i < this.entryList.Count; i++)
			{
				if (!this.entryList[i].Matches(this.displayFilter)) continue;
				this.displayedEntryList.Add(this.entryList[i]);
			}
		}
		private void UpdateScrolledToEnd()
		{
			this.scrolledToEnd = -this.AutoScrollPosition.Y + this.ClientRectangle.Height >= this.AutoScrollMinSize.Height - 20;
		}
		private void UpdateHoveredEntry(Point mouseLoc)
		{
			ViewEntry lastHovered = this.hoveredEntry;

			if (mouseLoc.IsEmpty || !this.ClientRectangle.Contains(mouseLoc))
				this.hoveredEntry = null;
			else
				this.hoveredEntry = this.GetEntryAt(mouseLoc.Y + this.ScrollOffset);

			if (lastHovered != this.hoveredEntry)
				this.Invalidate();
		}
		private void UpdateContentSize()
		{
			this.AutoScrollMinSize = new Size(0, this.GetEntryOffset(null));
			this.UpdateFirstDisplayIndex();
		}
		private void OnContentChanged()
		{
			this.UpdateContentSize();
			this.UpdateHoveredEntry(this.PointToClient(Cursor.Position));
			if (this.lastSelected)
				this.SelectedEntry = this.displayedEntryList.LastOrDefault();
			else if (!this.displayedEntryList.Contains(this.SelectedEntry))
				this.SelectedEntry = null;
			this.Invalidate();

			if (this.ContentChanged != null)
				this.ContentChanged(this, EventArgs.Empty);
		}
		private void OnSelectionChanged()
		{
			this.Invalidate();
			this.lastSelected = this.SelectedEntry == this.displayedEntryList.LastOrDefault();
			if (this.SelectionChanged != null)
				this.SelectionChanged(this, EventArgs.Empty);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

			Pen foregroundPen = new Pen(this.ForeColor);
			Brush foregroundBrush = new SolidBrush(this.ForeColor);
			Brush foregroundBrushAlpha = new SolidBrush(Color.FromArgb(128, this.ForeColor));
			Brush baseBrush = new SolidBrush(this.baseColor);
			Brush backgroundBrush = new SolidBrush(this.BackColor);
			Brush backgroundBrushAlt = new SolidBrush(Color.FromArgb(
				Math.Max(0, this.BackColor.R - 10), 
				Math.Max(0, this.BackColor.G - 10), 
				Math.Max(0, this.BackColor.B - 10)));
			StringFormat messageFormat = StringFormat.GenericDefault;
			messageFormat.Alignment = StringAlignment.Near;
			messageFormat.LineAlignment = StringAlignment.Center;
			messageFormat.Trimming = StringTrimming.EllipsisCharacter;
			messageFormat.FormatFlags = 0;
			StringFormat messageFormatTimestamp = new StringFormat(messageFormat);
			messageFormatTimestamp.Alignment = StringAlignment.Far;

			e.Graphics.FillRectangle(baseBrush, this.ClientRectangle);

			int offsetY = this.firstDisplayOffset;
			bool showTimestamp = this.ClientRectangle.Width >= 350;
			bool showFramestamp = this.ClientRectangle.Width >= 400;
			int timeStampWidth = this.Font.Height * 6;
			int frameStampWidth = this.Font.Height * 5;
			Size textMargin = new Size(10, 2);
			for (int i = this.firstDisplayIndex; i < this.displayedEntryList.Count; i++)
			{
				ViewEntry entry = this.displayedEntryList[i];
				int entryHeight = entry.Height;

				if (offsetY + entryHeight >= -this.AutoScrollPosition.Y)
				{
					int textIndent = entry.Indent * 20;
					Rectangle entryRect = new Rectangle(this.ClientRectangle.X, offsetY, this.ClientRectangle.Width, entryHeight);
					Rectangle typeIconRect = new Rectangle(
						entryRect.X + textMargin.Width / 2, 
						entryRect.Y, 
						20, 
						entryRect.Height);
					Rectangle sourceIconRect = new Rectangle(
						typeIconRect.Right, 
						entryRect.Y, 
						20, 
						entryRect.Height);
					Rectangle timeTextRect = new Rectangle(
						entryRect.Width - textMargin.Width - (showTimestamp ? timeStampWidth : 0) - (showFramestamp ? frameStampWidth : 0), 
						entryRect.Y + textMargin.Height, 
						(showTimestamp ? timeStampWidth : 0) + (showFramestamp ? frameStampWidth : 0), 
						entryRect.Height - textMargin.Height * 2);
					Rectangle textRect = new Rectangle(
						sourceIconRect.Right + textMargin.Width / 2 + textIndent, 
						entryRect.Y + textMargin.Height, 
						Math.Max(1, entryRect.Width - sourceIconRect.Right - textMargin.Width / 2 - textIndent - timeTextRect.Width), 
						entryRect.Height - textMargin.Height * 2);
					Image typeIcon = entry.TypeIcon;
					Image sourceIcon = entry.SourceIcon;

					{
						int newTextHeight;
						newTextHeight = this.Font.Height * (textRect.Height / this.Font.Height);
						textRect.Y = textRect.Y + textRect.Height / 2 - newTextHeight / 2;
						textRect.Height = newTextHeight;
						timeTextRect.Y = textRect.Y;
						timeTextRect.Height = textRect.Height;
					}

					bool highlightBgColor = (this.selectedEntry == entry && this.Focused) || this.hoveredEntry == entry;
					e.Graphics.FillRectangle(((i % 2) == 0 && !highlightBgColor) ? backgroundBrushAlt : backgroundBrush, entryRect);
					if (entry.LogEntry.Type == LogMessageType.Warning)
						e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 245, 200, 85)), entryRect);
					else if (entry.LogEntry.Type == LogMessageType.Error)
						e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 230, 105, 90)), entryRect);

					if (this.selectedEntry == entry && this.Focused)
						e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 255, 255, 255)), entryRect);
					else if (this.hoveredEntry == entry)
						e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 255, 255, 255)), entryRect);

					e.Graphics.DrawImage(typeIcon, 
						typeIconRect.X + typeIconRect.Width / 2 - typeIcon.Width / 2,
						typeIconRect.Y + typeIconRect.Height / 2 - typeIcon.Height / 2);
					e.Graphics.DrawImage(sourceIcon, 
						sourceIconRect.X + sourceIconRect.Width / 2 - sourceIcon.Width / 2,
						sourceIconRect.Y + sourceIconRect.Height / 2 - sourceIcon.Height / 2);
					e.Graphics.DrawString(entry.LogEntry.Message, this.Font, foregroundBrush, textRect, messageFormat);
					if (showTimestamp)
					{
						e.Graphics.DrawString(
							string.Format("{0:00}:{1:00}:{2:00}", 
								entry.LogEntry.TimeStamp.Hour, 
								entry.LogEntry.TimeStamp.Minute,
								entry.LogEntry.TimeStamp.Second), 
							this.Font, foregroundBrushAlpha, 
							new Rectangle(timeTextRect.Right - timeStampWidth, timeTextRect.Y, timeStampWidth, timeTextRect.Height), 
							messageFormatTimestamp);
					}
					if (showFramestamp)
					{
						e.Graphics.DrawString(
							string.Format("#{0}", entry.LogEntry.FrameStamp), 
							this.Font, foregroundBrushAlpha, 
							new Rectangle(timeTextRect.X + 5, timeTextRect.Y, timeTextRect.Width - (showTimestamp ? timeStampWidth + 10 : 0), timeTextRect.Height), 
							messageFormatTimestamp);
					}

					if (this.selectedEntry == entry && this.Focused)
					{
						e.Graphics.DrawRectangle(
							new Pen(Color.FromArgb(32, this.ForeColor)), 
							entryRect.X + 1,
							entryRect.Y + 1,
							entryRect.Width - 3,
							entryRect.Height - 3);
						e.Graphics.DrawRectangle(
							new Pen(Color.FromArgb(192, this.ForeColor)), 
							entryRect.X,
							entryRect.Y,
							entryRect.Width - 1,
							entryRect.Height - 1);
					}
					else if (this.hoveredEntry == entry)
					{
						e.Graphics.DrawRectangle(
							new Pen(Color.FromArgb(128, this.ForeColor)), 
							entryRect.X,
							entryRect.Y,
							entryRect.Width - 1,
							entryRect.Height - 1);
					}
				}

				offsetY += entryHeight;
				if (offsetY > this.ClientRectangle.Height + (-this.AutoScrollPosition.Y)) break;
			}
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			bool wasAtEnd = this.IsScrolledToEnd;
			base.OnSizeChanged(e);
			this.OnContentChanged();
			if (wasAtEnd) this.ScrollToEnd();
		}
		protected override void OnScroll(ScrollEventArgs se)
		{
			base.OnScroll(se);
			this.UpdateFirstDisplayIndex();
			this.UpdateScrolledToEnd();
			this.UpdateHoveredEntry(this.PointToClient(Cursor.Position));
		}
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			int wheelDivisor = Math.Max(1, 800 / this.ClientSize.Height);
			base.OnMouseWheel(new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, Math.Sign(e.Delta) * Math.Max(Math.Abs(e.Delta) / wheelDivisor, 1)));
			this.UpdateFirstDisplayIndex();
			this.UpdateScrolledToEnd();
			this.UpdateHoveredEntry(this.PointToClient(Cursor.Position));
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.UpdateHoveredEntry(e.Location);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.UpdateHoveredEntry(Point.Empty);
		}
		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			this.SelectedEntry = this.hoveredEntry;

			if (e.Button == MouseButtons.Right && this.SelectedEntry != null)
			{
				this.entryMenu.Show(this, e.Location);
			}
		}
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			if (this.SelectedEntry != null)
			{
				this.entryMenu_CopyItem_Click(this, EventArgs.Empty);
				if (this.selectedEntry.LogEntry.Context != null)
				{
					object contextObj = this.selectedEntry.LogEntry.Context;
					if (contextObj is IContentRef)
					{
						contextObj = (contextObj as IContentRef).Res;
					}
					DualityEditorApp.Highlight(this, new ObjectSelection(new[] { contextObj }), HighlightMode.All);
				}
			}
		}
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			e.IsInputKey = // Special key whitelist
				e.KeyCode == Keys.Up || 
				e.KeyCode == Keys.Down;
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
			{
				this.SelectedEntry = null;
			}
			else if (e.KeyCode == Keys.Return)
			{
				this.entryMenu_CopyItem_Click(this, EventArgs.Empty);
			}
			else if (e.KeyCode == Keys.C && e.Control && this.SelectedEntry != null)
			{
				this.entryMenu_CopyItem_Click(this, EventArgs.Empty);
			}
			else if (e.KeyCode == Keys.Down && this.displayedEntryList.Any())
			{
				int newEntryIndex = MathF.Clamp(this.displayedEntryList.IndexOfFirst(this.SelectedEntry) + 1, 0, this.displayedEntryList.Count - 1);
				this.SelectedEntry = this.displayedEntryList[newEntryIndex];
				this.EnsureVisible(this.SelectedEntry);
			}
			else if (e.KeyCode == Keys.Up && this.displayedEntryList.Any())
			{
				int newEntryIndex = MathF.Clamp(this.displayedEntryList.IndexOfFirst(this.SelectedEntry) - 1, 0, this.displayedEntryList.Count - 1);
				this.SelectedEntry = this.displayedEntryList[newEntryIndex];
				this.EnsureVisible(this.SelectedEntry);
			}
		}
		
		private void timerLogSchedule_Tick(object sender, EventArgs e)
		{
			// Process a clone of the logSchedule to prevent any interference due to cross-thread logs
			LogEntry[] logScheduleArray = null;
			lock (this.logScheduleLock)
			{
				logScheduleArray = new LogEntry[this.logSchedule.Count];
				Array.Copy(this.logSchedule.Data, logScheduleArray, this.logSchedule.Count);
				this.logSchedule.Clear();
				this.timerLogSchedule.Enabled = false;
				this.logScheduleActive = false;
			}
			this.ProcessIncomingEntries(logScheduleArray, logScheduleArray.Length);
		}
		private void entryMenu_CopyAllItems_Click(object sender, EventArgs e)
		{
			StringBuilder completeLog = new StringBuilder();
			foreach (ViewEntry entry in this.displayedEntryList)
			{
				entry.GetFullText(completeLog);
				completeLog.AppendLine();
			}
			Clipboard.SetText(completeLog.ToString());
		}
		private void entryMenu_CopyItem_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(this.SelectedEntry.GetFullText());
		}

		void ILogOutput.Write(LogEntry entry)
		{
			lock (this.logScheduleLock)
			{
				this.logSchedule.Add(entry);
				if (!this.logScheduleActive)
				{
					// Don't use a synchronous Invoke. It will block while the BuildManager is active (why?)
					// and thus lead to a deadlock when something is logged while it is.
					this.InvokeEx(() => this.timerLogSchedule.Enabled = true, false);
					this.logScheduleActive = true;
				}
			}
		}
	}
}
