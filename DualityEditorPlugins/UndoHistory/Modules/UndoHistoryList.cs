using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Duality.Editor.Plugins.UndoHistory.Modules
{
	public class UndoHistoryList : UserControl
	{
		public class ViewEntry
		{
			private UndoHistoryList			parent		= null;
			private	IUndoRedoActionInfo	    action		= null;
			private	int						msgLines	= 1;
		    private bool isCurrentAction = false;
		    private DateTime? timestamp = null;

            public IUndoRedoActionInfo Action
			{
                get { return this.action; }
			}
            public bool IsCurrentAction
            {
                get { return this.isCurrentAction; }
            }
            public DateTime? Timestamp
		    {
                get { return this.timestamp; }
		    }
			public int Height
			{
				get { return Math.Max(20, 7 + this.msgLines * this.parent.Font.Height); }
			}


            public ViewEntry(UndoHistoryList parent, IUndoRedoActionInfo action, bool isCurrentAction = false)
			{
				this.parent = parent;
                this.action = action;
				this.msgLines = action.Name.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Length;
                this.isCurrentAction = isCurrentAction;
                //TODO: DateTime.Now doesn't work as we're clearing the list each time the stack changes
                this.timestamp = null; //DateTime.Now;
			}

			public void GetFullText(StringBuilder appendTo)
			{
			    appendTo.Append(this.parent.Entries.ToList().IndexOf(this));
			    appendTo.Append(": ");
                appendTo.Append(this.action.Name);
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
		private	DateTime			displayMinTime		= DateTime.MinValue;
		private	IUndoRedoActionInfo boundOutput			= null;
		private	Color				baseColor			= SystemColors.Control;
		private	bool				scrolledToEnd		= true;
		private	bool				lastSelected		= true;
		private	ViewEntry			hoveredEntry		= null;
		private	ViewEntry			selectedEntry		= null;
		private	ContextMenuStrip	entryMenu			= null;
        private List<IUndoRedoActionInfo> logSchedule = new List<IUndoRedoActionInfo>();
		private System.ComponentModel.IContainer components = null;
        private Font                boldFont            = null;


		public event EventHandler SelectionChanged = null;
		public event EventHandler ContentChanged = null;
		public event EventHandler<ViewEntryEventArgs> NewEntry = null;


		public IEnumerable<ViewEntry> Entries
		{
			get { return this.entryList; }
		}
		public IEnumerable<ViewEntry> DisplayedEntries
		{
			get { return this.entryList; }
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
		public Color BaseColor
		{
			get { return this.baseColor; }
			set { this.baseColor = value; this.Invalidate(); }
		}
		public bool IsScrolledToEnd
		{
			get { return this.scrolledToEnd; }
		}


		public UndoHistoryList()
		{
			this.components = new System.ComponentModel.Container();

			this.AutoScroll = true;
            this.boldFont = new Font(this.Font, FontStyle.Bold);

			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
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
        public ViewEntry GetViewEntry(IUndoRedoActionInfo entry)
		{
			return this.entryList.FirstOrDefault(e => e.Action == entry);
		}
        public ViewEntry AddEntry(IUndoRedoActionInfo entry)
        {
            bool isCurrentAction = this.entryList.Count == UndoRedoManager.ActionIndex;
			ViewEntry viewEntry = new ViewEntry(this, entry, isCurrentAction);
			this.entryList.Add(viewEntry);

			if (this.NewEntry != null)
				this.NewEntry(this, new ViewEntryEventArgs(viewEntry));

			this.OnContentChanged();
			return viewEntry;
		}
        public void AddEntry(IEnumerable<IUndoRedoActionInfo> entries)
		{
            foreach (IUndoRedoActionInfo entry in entries)
            {
                bool isCurrentAction = this.entryList.Count == UndoRedoManager.ActionIndex;
				ViewEntry viewEntry = new ViewEntry(this, entry, isCurrentAction);
				this.entryList.Add(viewEntry);

				if (this.NewEntry != null)
					this.NewEntry(this, new ViewEntryEventArgs(viewEntry));
			}
			this.OnContentChanged();
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
            //TODO: Add in a timestamp maybe?
		    bool showTimestamp = false;//this.ClientRectangle.Width >= 350;
            int timeStampWidth = this.Font.Height * 6;
			Size textMargin = new Size(10, 2);
			foreach (ViewEntry entry in this.DisplayedEntries)
			{
				int entryHeight = entry.Height;

				if (offsetY + entryHeight >= -this.AutoScrollPosition.Y)
				{
					Rectangle entryRect = new Rectangle(this.ClientRectangle.X, offsetY, this.ClientRectangle.Width, entryHeight);
					Rectangle textRect = new Rectangle(
						textMargin.Width / 2, 
						entryRect.Y + textMargin.Height, 
						Math.Max(1, entryRect.Width - textMargin.Width / 2),
                        entryRect.Height - textMargin.Height * 2);
                    Rectangle timeTextRect = new Rectangle(
                        entryRect.Width - textMargin.Width - (showTimestamp ? timeStampWidth : 0),
                        entryRect.Y + textMargin.Height,
                        (showTimestamp ? timeStampWidth : 0),
                        entryRect.Height - textMargin.Height * 2);

					{
						int newTextHeight;
						newTextHeight = this.Font.Height * (textRect.Height / this.Font.Height);
						textRect.Y = textRect.Y + textRect.Height / 2 - newTextHeight / 2;
						textRect.Height = newTextHeight;
					}

					bool highlightBgColor = (this.selectedEntry == entry && this.Focused) || this.hoveredEntry == entry;
					e.Graphics.FillRectangle((evenEntry && !highlightBgColor) ? backgroundBrushAlt : backgroundBrush, entryRect);

					if (this.selectedEntry == entry && this.Focused)
						e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 255, 255, 255)), entryRect);
					else if (this.hoveredEntry == entry)
						e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 255, 255, 255)), entryRect);

                    e.Graphics.DrawString(entry.GetFullText(), entry.IsCurrentAction ? this.boldFont : this.Font, foregroundBrush, textRect, messageFormat);
                    if (showTimestamp && entry.Timestamp.HasValue)
                    {
                        e.Graphics.DrawString(
                            string.Format("{0:00}:{1:00}:{2:00}",
                                entry.Timestamp.Value.Hour,
                                entry.Timestamp.Value.Minute,
                                entry.Timestamp.Value.Second),
                            this.Font, foregroundBrushAlpha,
                            new Rectangle(timeTextRect.Right - timeStampWidth, timeTextRect.Y, timeStampWidth, timeTextRect.Height),
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
        }

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			if (this.SelectedEntry != null)
			{
			    List<ViewEntry> entriesList = this.Entries.ToList();
			    int selectedIndex = entriesList.IndexOf(this.SelectedEntry);
                int timesToPerform = UndoRedoManager.ActionIndex - selectedIndex;

			    for (int i = 0; i < Math.Abs(timesToPerform); i++)
			    {
			        if (timesToPerform > 0)
			        {
			            UndoRedoManager.Undo();
			        }
			        else
			        {
			            UndoRedoManager.Redo();
			        }
			    }
			}
		}

        private void boundOutput_NewEntry(object sender, IUndoRedoActionInfo e)
		{
			lock (this.logSchedule)
			{
				this.logSchedule.Add(e);
			}
		}
	}
}
