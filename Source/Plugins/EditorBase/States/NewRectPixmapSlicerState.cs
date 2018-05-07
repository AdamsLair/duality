using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Duality.Editor.Plugins.Base.Forms;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.States
{
	/// <summary>
	/// A <see cref="IPixmapSlicerState"/> that allows the user
	/// to define new atlas rectangles for the <see cref="TargetPixmap"/>
	/// </summary>
	public class NewRectPixmapSlicerState : IPixmapSlicerState
	{
		private ToolStripButton cancelButton = null;

		private bool	mouseDown			= false;
		private int		selectedRectIndex	= -1;
		private PointF	rectAddStart		= Point.Empty;

		public int SelectedRectIndex
		{
			get { return this.selectedRectIndex; }
		}
		public List<ToolStripItem> StateControls	{ get; private set; }
		public Cursor Cursor						{ get; private set; }
		public Rectangle DisplayBounds				{ get; set; }
		public Pixmap TargetPixmap					{ get; set; }

		public MouseTransformDelegate	TransformMouseCoordinates	{ get; set; }
		public Func<Rect, Rect>			GetAtlasRect				{ get; set; }
		public Func<Rect, Rect>			GetDisplayRect				{ get; set; }

		public event EventHandler PixmapUpdated;
		public event EventHandler CursorChanged;
		public event EventHandler StateCancelled;
		public event EventHandler SelectionChanged;
		public event EventHandler<PixmapSlicerForm.PixmapSlicerStateEventArgs> StateChangeRequested;

		public NewRectPixmapSlicerState()
		{
			this.Cursor = Cursors.Default;
			this.StateControls = new List<ToolStripItem>();

			this.cancelButton = new ToolStripButton("Cancel", null,
				(s, e) => this.CancelState());

			this.StateControls.Add(this.cancelButton);
		}

		public void ClearSelection()
		{
			this.selectedRectIndex = -1;
			if (this.SelectionChanged != null)
				this.SelectionChanged.Invoke(this, EventArgs.Empty);
		}

		public void OnMouseDown(MouseEventArgs e)
		{
			this.mouseDown = true;
		}

		public void OnMouseUp(MouseEventArgs e)
		{
			if (this.mouseDown)
			{
				this.mouseDown = false;
				this.CancelState();
			}
		}

		public void OnMouseMove(MouseEventArgs e)
		{
			if (this.TargetPixmap == null || !this.mouseDown)
				return;

			float x, y;
			this.TransformMouseCoordinates(e.Location, out x, out y);

			// Check if rect creation hasn't started yet
			if (this.selectedRectIndex == -1)
			{
				if (this.TargetPixmap.Atlas == null)
					this.TargetPixmap.Atlas = new List<Rect>();
				this.rectAddStart = new PointF(x, y);
				Rect newDisplayRect = new Rect(x, y, 0, 0);
				this.TargetPixmap.Atlas.Add(this.GetAtlasRect(newDisplayRect));
				this.selectedRectIndex = this.TargetPixmap.Atlas.Count - 1;
				if (this.SelectionChanged != null)
					this.SelectionChanged.Invoke(this, EventArgs.Empty);
			}

			float rx = MathF.Min(x, this.rectAddStart.X);
			float ry = MathF.Min(y, this.rectAddStart.Y);
			float rw = MathF.Abs(x - this.rectAddStart.X);
			float rh = MathF.Abs(y - this.rectAddStart.Y);

			Rect displayRect = new Rect(rx, ry, rw, rh);
			this.SetPixmapAtlasRect(this.GetAtlasRect(displayRect), this.selectedRectIndex);
		}

		public void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.CancelState();
		}

		public void OnPaint(PaintEventArgs e)
		{
		}

		private void CancelState()
		{
			if (this.StateCancelled != null)
				this.StateCancelled.Invoke(this, EventArgs.Empty);
		}

		private void SetPixmapAtlasRect(Rect rect, int index)
		{
			this.TargetPixmap.Atlas[index] = rect;
			if (this.PixmapUpdated != null)
				this.PixmapUpdated.Invoke(this, EventArgs.Empty);
		}
	}
}
