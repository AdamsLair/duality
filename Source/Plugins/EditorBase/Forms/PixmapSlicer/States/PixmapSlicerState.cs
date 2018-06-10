using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Duality.Editor.Controls.ToolStrip;
using Duality.Resources;
using Font = System.Drawing.Font;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	public abstract class PixmapSlicerState : IPixmapSlicerState
	{
		protected static Pen selectedRectPenLight = new Pen(Color.Blue, 1);
		protected static Pen selectedRectPenDark  = new Pen(Color.DeepSkyBlue, 1);


		private   int    selectedRectIndex = -1;
		private   Cursor cursor            = Cursors.Default;
		private   Pixmap targetPixmap      = null;

		public event EventHandler StateCancelled;
		public event EventHandler<PixmapSlicerStateEventArgs> StateChangeRequested;


		public List<ToolStripItem> StateControls { get; private set; }
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
					this.View.Cursor = this.cursor;
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
					this.View.Invalidate();
				}
			}
		}
		public PixmapSlicingView View { get; set; }
		protected Pen SelectedRectPen
		{
			get { return this.View.DarkMode ? selectedRectPenDark : selectedRectPenLight; }
		}


		protected PixmapSlicerState()
		{
			this.StateControls = new List<ToolStripItem>();
		}

		public virtual void ClearSelection()
		{
			this.SelectedRectIndex = -1;
		}

		protected void UpdateDisplay()
		{
			this.View.InvalidatePixmap();
		}
		protected void UpdateDisplay(Rect rect)
		{
			this.View.InvalidatePixmap(rect);
		}

		protected void CancelState()
		{
			if (this.StateCancelled != null)
			{
				this.StateCancelled.Invoke(this, EventArgs.Empty);
			}
		}
		protected void ChangeState(Type newStateType)
		{
			if (this.StateChangeRequested != null)
				this.StateChangeRequested.Invoke(this, new PixmapSlicerStateEventArgs(newStateType));
		}

		public virtual void OnStateEntered(EventArgs e) { }
		public virtual void OnStateLeaving(EventArgs e) { }
		public virtual void OnMouseDown(MouseEventArgs e) { }
		public virtual void OnMouseUp(MouseEventArgs e) { }
		public virtual void OnMouseMove(MouseEventArgs e) { }
		public virtual void OnKeyUp(KeyEventArgs e) { }

		/// <summary>
		/// Called during the paint event of the parent control.
		/// Displays all atlas rects and highlights the selected atlas rect.
		/// </summary>
		public virtual void OnPaint(PaintEventArgs e)
		{
			if (this.targetPixmap == null || this.targetPixmap.Atlas == null)
				return;

			// Draw selected rect outline
			if (this.selectedRectIndex != -1)
			{
				Rect selectedAtlasRect = this.targetPixmap.Atlas[this.selectedRectIndex];
				Rectangle selectedDisplayRect = this.View.GetDisplayRect(selectedAtlasRect);
				e.Graphics.DrawRectangle(
					this.SelectedRectPen, 
					selectedDisplayRect);
			}
		}

		protected virtual void OnPixmapChanged() { }

		public abstract HelpInfo ProvideHoverHelp(Point localPos, ref bool captured);


		/// <summary>
		/// Creates a <see cref="ToolStripNumericUpDown"/> with default styling.
		/// The range of the control is set to [0,maximum].
		/// </summary>
		protected static ToolStripNumericUpDown CreateNumericUpDown(string text, int minimum, int maximum)
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
