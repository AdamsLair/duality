using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	public abstract class PixmapSlicerState : IPixmapSlicerState
	{
		private int		selectedRectIndex	= -1;
		private Cursor	cursor				= Cursors.Default;

		public List<ToolStripItem>		StateControls				{ get; private set; }
		public MouseTransformDelegate	TransformMouseCoordinates	{ get; set; }
		public Func<Rect, Rect>			GetAtlasRect				{ get; set; }
		public Func<Rect, Rect>			GetDisplayRect				{ get; set; }
		public Rectangle				DisplayBounds				{ get; set; }
		public Pixmap					TargetPixmap				{ get; set; }

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
		}

		public virtual void OnKeyUp(KeyEventArgs e)
		{
		}

		public virtual void OnPaint(PaintEventArgs e)
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
	}
}
