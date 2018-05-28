using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Duality.Editor.Controls.ToolStrip;
using Duality.Resources;
using Font = System.Drawing.Font;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	public abstract class PixmapSlicerState : IPixmapSlicerState
	{
		protected static Pen	rectPenLight			= new Pen(Color.Black, 1);
		protected static Pen	rectPenDark				= new Pen(Color.White, 1);
		protected static Pen	selectedRectPenLight	= new Pen(Color.Blue, 1);
		protected static Pen	selectedRectPenDark		= new Pen(Color.DeepSkyBlue, 1);
		protected static Brush	indexTextBackBrush		= new SolidBrush(Color.FromArgb(128, Color.Black));
		protected static Brush	indexTextForeBrush		= new SolidBrush(Color.White);
		protected static Font	font					= new Font(FontFamily.GenericSansSerif, 8.25f);

		protected Pen RectPen { get { return this.Context.DarkMode ? rectPenDark : rectPenLight; } }
		protected Pen SelectedRectPen { get { return this.Context.DarkMode ? selectedRectPenDark : selectedRectPenLight; } }

		private int		selectedRectIndex	= -1;
		private Cursor	cursor				= Cursors.Default;
		private Pixmap	targetPixmap		= null;

		protected int					hoveredRectIndex		= -1;

		public List<ToolStripItem>		StateControls				{ get; private set; }
		public MouseTransformDelegate	TransformMouseCoordinates	{ get; set; }
		public Func<Rect, Rect>			GetAtlasRect				{ get; set; }
		public Func<Rect, Rect>			GetDisplayRect				{ get; set; }
		public Rectangle				DisplayBounds				{ get; set; }
		public virtual PixmapNumberingStyle NumberingStyle			{ get { return this.Context.NumberingStyle; } }

		public Pixmap TargetPixmap
		{
			get { return this.targetPixmap; }
			set
			{
				if (this.targetPixmap != value)
				{
					this.targetPixmap = value;
					this.OnPixmapChanged();
				}
			}
		}

		public Cursor Cursor
		{
			get { return this.cursor; }
			set
			{
				if (this.cursor != value)
				{
					this.cursor = value;
					if (this.CursorChanged != null)
						this.CursorChanged.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public int SelectedRectIndex
		{
			get { return this.selectedRectIndex; }
			set
			{
				if (this.selectedRectIndex != value)
				{
					this.selectedRectIndex = value;
					if (this.SelectionChanged != null)
						this.SelectionChanged.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public PixmapSlicingContext Context { get; set; }

		public event EventHandler DisplayUpdated;
		public event EventHandler CursorChanged;
		public event EventHandler StateCancelled;
		public event EventHandler SelectionChanged;
		public event EventHandler<PixmapSlicerForm.PixmapSlicerStateEventArgs> StateChangeRequested;

		protected PixmapSlicerState()
		{
			this.StateControls = new List<ToolStripItem>();
		}

		public virtual void ClearSelection()
		{
			this.SelectedRectIndex = -1;
		}

		public virtual void OnMouseDown(MouseEventArgs e)
		{
		}

		public virtual void OnMouseUp(MouseEventArgs e)
		{
		}

		public virtual void OnMouseMove(MouseEventArgs e)
		{
			if (this.targetPixmap == null || this.targetPixmap.Atlas == null)
				return;

			float x, y;
			this.TransformMouseCoordinates(e.Location, out x, out y);

			for (int i = 0; i < this.targetPixmap.Atlas.Count; i++)
			{
				Rect rect = this.GetDisplayRect(this.targetPixmap.Atlas[i]);
				if (rect.Contains(x, y))
				{
					int originalHoveredIndex = this.hoveredRectIndex;
					this.hoveredRectIndex = i;

					if ((this.NumberingStyle & PixmapNumberingStyle.Hovered) > 0 
						&& this.hoveredRectIndex != originalHoveredIndex)
						this.UpdateDisplay();
					return;
				}
			}

			if (this.hoveredRectIndex != -1)
			{
				this.hoveredRectIndex = -1;
				this.UpdateDisplay();
			}
		}

		public virtual void OnKeyUp(KeyEventArgs e)
		{
		}

		/// <summary>
		/// Called during the paint event of the parent control.
		/// Displays all atlas rects and highlights the selected atlas rect.
		/// </summary>
		public virtual void OnPaint(PaintEventArgs e)
		{
			if (this.targetPixmap == null || this.targetPixmap.Atlas == null)
				return;

			for (int i = 0; i < this.targetPixmap.Atlas.Count; i++)
			{
				if (i != this.SelectedRectIndex)
					this.DisplayAtlasRect(e.Graphics, i);
			}

			if (this.SelectedRectIndex != -1)
				this.DisplayAtlasRect(e.Graphics, this.SelectedRectIndex);
		}

		protected void DisplayAtlasRect(Graphics g, int atlastIndex)
		{
			Rect rect = this.GetDisplayRect(this.targetPixmap.Atlas[atlastIndex]);

			Pen pen = atlastIndex == this.SelectedRectIndex
				? this.SelectedRectPen
				: this.RectPen;

			float originalWidth = pen.Width;
			pen.Width /= this.Context.ScaleFactor;
			g.DrawRectangle(pen, rect.X, rect.Y, rect.W, rect.H);
			pen.Width = originalWidth;

			bool showingAllNumbers = (this.NumberingStyle & PixmapNumberingStyle.All) > 0;
			bool showingThisRect = (this.NumberingStyle & PixmapNumberingStyle.Hovered) > 0
									&& this.hoveredRectIndex == atlastIndex;

			if (showingAllNumbers || showingThisRect)
			{
				this.DisplayRectIndex(g, rect, atlastIndex);
			}
		}

		/// <summary>
		/// Displays an index value in the middle of the given Rect
		/// </summary>
		protected void DisplayRectIndex(Graphics g, Rect displayRect, int index)
		{
			string indexText = index.ToString();
			SizeF textSize = g.MeasureString(indexText, font);
			RectangleF textRect = new RectangleF(
				displayRect.X,
				displayRect.Y,
				3 + textSize.Width,
				3 + textSize.Height);
			textRect.X += (displayRect.W - textRect.Width) / 2;
			textRect.Y += (displayRect.H - textRect.Height) / 2;
			PointF textPos = new PointF(
				textRect.X + textRect.Width * 0.5f - textSize.Width * 0.5f,
				textRect.Y + textRect.Height * 0.5f - textSize.Height * 0.5f);

			g.FillRectangle(indexTextBackBrush, textRect);
			g.DrawString(indexText, font, indexTextForeBrush, textPos.X, textPos.Y);
		}

		protected virtual void OnPixmapChanged()
		{

		}

		protected void CancelState()
		{
			if (this.StateCancelled != null)
			{
				this.StateCancelled.Invoke(this, EventArgs.Empty);
			}
		}

		protected void UpdateDisplay()
		{
			if (this.DisplayUpdated != null)
			{
				this.DisplayUpdated.Invoke(this, EventArgs.Empty);
			}
		}

		protected void ChangeState(Type newStateType)
		{
			if (this.StateChangeRequested != null)
			{
				this.StateChangeRequested.Invoke(this, 
					new PixmapSlicerForm.PixmapSlicerStateEventArgs(newStateType));
			}
		}

		public abstract HelpInfo ProvideHoverHelp(Point localPos, ref bool captured);

		/// <summary>
		/// Creates a <see cref="ToolStripNumericUpDown"/> with default styling.
		/// The range of the control is set to [0,maximum].
		/// </summary>
		protected static ToolStripNumericUpDown CreateNumericUpDown(string text, int minimum = 0, int maximum = 100)
		{
			return new ToolStripNumericUpDown
			{
				BackColor = Color.Transparent,
				DecimalPlaces = 0,
				Minimum = minimum,
				Maximum = maximum,
				NumBackColor = Color.FromArgb(196, 196, 196),
				Text = text
			};
		}
	}
}
