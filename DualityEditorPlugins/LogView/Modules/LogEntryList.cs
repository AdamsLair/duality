using System;
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
	public class LogEntryList : UserControl
	{
		[Flags]
		public enum MessageFilter
		{
			None			= 0x0,

			SourceCore		= 0x01,
			SourceEditor	= 0x02,
			SourceGame		= 0x04,

			TypeMessage		= 0x08,
			TypeWarning		= 0x10,
			TypeError		= 0x20,

			SourceAll		= SourceCore | SourceEditor | SourceGame,
			TypeAll			= TypeMessage | TypeWarning | TypeError,
			All				= SourceAll | TypeAll
		}
		public class ViewEntry
		{
			private LogEntryList			parent		= null;
			private	DataLogOutput.LogEntry	log			= null;
			private	int						msgLines	= 1;
			
			public DataLogOutput.LogEntry LogEntry
			{
				get { return this.log; }
			}
			public int Height
			{
				get { return Math.Max(20, 7 + this.msgLines * this.parent.Font.Height); }
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

			public ViewEntry(LogEntryList parent, DataLogOutput.LogEntry log)
			{
				this.parent = parent;
				this.log = log;
				this.msgLines = log.Message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Length;
			}

			public bool Matches(DateTime minTime, MessageFilter filter)
			{
				if (this.log.Timestamp < minTime) return false;
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
					case LogMessageType.Message:	appendTo.Append("Info:    "); break;
					case LogMessageType.Warning:	appendTo.Append("Warning: "); break;
					case LogMessageType.Error:		appendTo.Append("Error:   "); break;
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
			private ViewEntry	entry;
			public ViewEntry Entry
			{
				get { return this.entry; }
			}
			public ViewEntryEventArgs(ViewEntry entry)
			{
				this.entry = entry;
			}
		}


		private	List<ViewEntry>		entryList			= new List<ViewEntry>();
		private	MessageFilter		displayFilter		= MessageFilter.All;
		private	DateTime			displayMinTime		= DateTime.MinValue;
		private	DataLogOutput		boundOutput			= null;
		private	Color				baseColor			= SystemColors.Control;
		private	bool				scrolledToEnd		= true;
		private	bool				lastSelected		= true;
		private	ViewEntry			hoveredEntry		= null;
		private	ViewEntry			selectedEntry		= null;
		private	ContextMenuStrip	entryMenu			= null;
		private Timer				timerLogSchedule	= null;
		private	List<DataLogOutput.LogEntry> logSchedule = new List<DataLogOutput.LogEntry>();
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
			get { return this.entryList.Where(e => e.Matches(this.displayMinTime, this.displayFilter)); }
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
				ViewEntry lastEntry = this.GetEntryAt(this.ScrollOffset);
				int entryOff = this.ScrollOffset - this.GetEntryOffset(lastEntry);

				this.displayFilter = value;
				this.OnContentChanged();

				this.ScrollToEntry(lastEntry, entryOff);
			}
		}
		public DateTime DisplayMinTime
		{
			get { return this.displayMinTime; }
			set
			{
				ViewEntry lastEntry = this.GetEntryAt(this.ScrollOffset);
				int entryOff = this.ScrollOffset - this.GetEntryOffset(lastEntry);

				this.displayMinTime = value;
				this.OnContentChanged();

				this.ScrollToEntry(lastEntry, entryOff);
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
			this.OnContentChanged();
		}
		public ViewEntry GetViewEntry(DataLogOutput.LogEntry entry)
		{
			return this.entryList.FirstOrDefault(e => e.LogEntry == entry);
		}
		public ViewEntry AddEntry(DataLogOutput.LogEntry entry)
		{
			ViewEntry viewEntry = new ViewEntry(this, entry);
			this.entryList.Add(viewEntry);

			if (this.NewEntry != null)
				this.NewEntry(this, new ViewEntryEventArgs(viewEntry));

			this.OnContentChanged();
			return viewEntry;
		}
		public void AddEntry(IEnumerable<DataLogOutput.LogEntry> entries)
		{
			foreach (DataLogOutput.LogEntry entry in entries)
			{
				ViewEntry viewEntry = new ViewEntry(this, entry);
				this.entryList.Add(viewEntry);

				if (this.NewEntry != null)
					this.NewEntry(this, new ViewEntryEventArgs(viewEntry));
			}
			this.OnContentChanged();
		}
		public void UpdateFromLog(DataLogOutput dualityLog)
		{
			if (dualityLog == null)
			{
				this.Clear();
				return;
			}

			this.entryList.Clear();
			foreach (var entry in dualityLog.Data)
				this.entryList.Add(new ViewEntry(this, entry));

			this.OnContentChanged();
		}
		public void BindToOutput(DataLogOutput dualityLog)
		{
			if (this.boundOutput == dualityLog) return;

			if (this.boundOutput != null)
				this.boundOutput.NewEntry -= this.boundOutput_NewEntry;

			this.boundOutput = dualityLog;
			this.UpdateFromLog(this.boundOutput);

			if (this.boundOutput != null)
				this.boundOutput.NewEntry += this.boundOutput_NewEntry;
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
			foreach (ViewEntry e in this.DisplayedEntries)
			{
				if (e == entry) break;
				totalHeight += e.Height;
			}
			return totalHeight;
		}
		public ViewEntry GetEntryAt(int offsetY)
		{
			int totalHeight = 0;
			foreach (ViewEntry entry in this.DisplayedEntries)
			{
				totalHeight += entry.Height;
				if (totalHeight >= offsetY) return entry;
			}
			return null;
		}
		
		private void ProcessIncomingEntries(IEnumerable<DataLogOutput.LogEntry> entries)
		{
			bool wasAtEnd = this.IsScrolledToEnd;
			this.AddEntry(entries);
			if (wasAtEnd) this.ScrollToEnd();
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
		}
		private void OnContentChanged()
		{
			this.UpdateContentSize();
			this.UpdateHoveredEntry(this.PointToClient(Cursor.Position));
			if (this.lastSelected)
				this.SelectedEntry = this.DisplayedEntries.LastOrDefault();
			else if (!this.DisplayedEntries.Contains(this.SelectedEntry))
				this.SelectedEntry = null;
			this.Invalidate();

			if (this.ContentChanged != null)
				this.ContentChanged(this, EventArgs.Empty);
		}
		private void OnSelectionChanged()
		{
			this.Invalidate();
			this.lastSelected = this.SelectedEntry == this.DisplayedEntries.LastOrDefault();
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

			int offsetY = 0;
			bool evenEntry = false;
			bool showTimestamp = this.ClientRectangle.Width >= 350;
			bool showFramestamp = this.ClientRectangle.Width >= 400;
			int timeStampWidth = this.Font.Height * 6;
			int frameStampWidth = this.Font.Height * 5;
			Size textMargin = new Size(10, 2);
			foreach (ViewEntry entry in this.DisplayedEntries)
			{
				int entryHeight = entry.Height;

				if (offsetY + entryHeight >= -this.AutoScrollPosition.Y)
				{
					int textIndent = entry.LogEntry.Indent * 20;
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
					e.Graphics.FillRectangle((evenEntry && !highlightBgColor) ? backgroundBrushAlt : backgroundBrush, entryRect);
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
								entry.LogEntry.Timestamp.Hour, 
								entry.LogEntry.Timestamp.Minute,
								entry.LogEntry.Timestamp.Second), 
							this.Font, foregroundBrushAlpha, 
							new Rectangle(timeTextRect.Right - timeStampWidth, timeTextRect.Y, timeStampWidth, timeTextRect.Height), 
							messageFormatTimestamp);
					}
					if (showFramestamp)
					{
						e.Graphics.DrawString(
							string.Format("#{0}", entry.LogEntry.FrameIndex), 
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
				evenEntry = !evenEntry;
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
			this.UpdateScrolledToEnd();
			this.UpdateHoveredEntry(this.PointToClient(Cursor.Position));
		}
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			int wheelDivisor = Math.Max(1, 800 / this.ClientSize.Height);
			base.OnMouseWheel(new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, Math.Sign(e.Delta) * Math.Max(Math.Abs(e.Delta) / wheelDivisor, 1)));
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
			else if (e.KeyCode == Keys.Down && this.DisplayedEntries.Any())
			{
				ViewEntry[] visEntries = this.DisplayedEntries.ToArray();
				this.SelectedEntry = visEntries[MathF.Clamp(visEntries.IndexOfFirst(this.SelectedEntry) + 1, 0, visEntries.Length - 1)];
				this.EnsureVisible(this.SelectedEntry);
			}
			else if (e.KeyCode == Keys.Up && this.DisplayedEntries.Any())
			{
				ViewEntry[] visEntries = this.DisplayedEntries.ToArray();
				this.SelectedEntry = visEntries[MathF.Clamp(visEntries.IndexOfFirst(this.SelectedEntry) - 1, 0, visEntries.Length - 1)];
				this.EnsureVisible(this.SelectedEntry);
			}
		}
		
		private void timerLogSchedule_Tick(object sender, EventArgs e)
		{
			// Process a clone of the logSchedule to prevent any interference due to cross-thread logs
			DataLogOutput.LogEntry[] logScheduleArray = null;
			lock (this.logSchedule)
			{
				logScheduleArray = this.logSchedule.ToArray();
				this.logSchedule.Clear();
			}
			this.ProcessIncomingEntries(logScheduleArray);
			this.timerLogSchedule.Enabled = false;
		}
		private void boundOutput_NewEntry(object sender, DataLogOutput.LogEntryEventArgs e)
		{
			lock (this.logSchedule)
			{
				this.logSchedule.Add(e.Entry);
			}
			if (!this.timerLogSchedule.Enabled)
			{
				// Don't use a synchronous Invoke. It will block while the BuildManager is active (why?)
				// and thus lead to a deadlock when something is logged while it is.
				this.InvokeEx(() => this.timerLogSchedule.Enabled = true, false);
			}
		}
		private void entryMenu_CopyAllItems_Click(object sender, EventArgs e)
		{
			StringBuilder completeLog = new StringBuilder();
			foreach (ViewEntry entry in this.DisplayedEntries)
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
	}
}
