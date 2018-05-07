using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Duality.Editor.Controls.ToolStrip;
using Duality.Editor.Plugins.Base.Forms.PixmapSlicer.Utilities;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	/// <summary>
	/// A <see cref="IPixmapSlicerState"/> that provides the ability to
	/// resize selected atlas rectangles and provides controls for accessing
	/// many other <see cref="IPixmapSlicerState"/>
	/// </summary>
	public class DefaultPixmapSlicerState : IPixmapSlicerState
	{
		private const float DRAG_OFFSET = 3f;

		private readonly ToolStripButton		addRectButton			= null;
		private readonly ToolStripButton		clearButton				= null;
		private readonly ToolStripButton		deleteSelectedButton	= null;
		private readonly ToolStripButton		autoSliceButton			= null;
		private readonly ToolStripNumericUpDown	alphaCutoffEntry		= null;

		private bool	mouseDown			= false;
		private bool	dragInProgress		= false;
		private int		selectedRectIndex	= -1;
		private Side	hoveredRectSide		= Side.None;
		private Cursor	cursor				= Cursors.Default;

		public List<ToolStripItem>	StateControls	{ get; private set; }
		public Rectangle			DisplayBounds	{ get; set; }
		public Pixmap				TargetPixmap	{ get; set; }
		public int SelectedRectIndex
		{
			get { return this.selectedRectIndex; }
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

		public MouseTransformDelegate TransformMouseCoordinates { get; set; }
		public Func<Rect, Rect> GetAtlasRect { get; set; }
		public Func<Rect, Rect> GetDisplayRect { get; set; }

		public event EventHandler PixmapUpdated;
		public event EventHandler CursorChanged;
		public event EventHandler StateCancelled;
		public event EventHandler SelectionChanged;
		public event EventHandler<PixmapSlicerForm.PixmapSlicerStateEventArgs> StateChangeRequested;

		public DefaultPixmapSlicerState()
		{
			this.StateControls = new List<ToolStripItem>();

			this.addRectButton = new ToolStripButton("Add Rect", null,
				(s, e) => this.SwitchToAddRectState());

			this.clearButton = new ToolStripButton("Clear", null,
				(s, e) => this.ClearRects());

			this.deleteSelectedButton = new ToolStripButton("Delete", null,
				(s, e) => this.DeleteSelectedRect());
			this.deleteSelectedButton.Enabled = false;

			this.autoSliceButton = new ToolStripButton("Auto-Slice", null,
				(s, e) => this.AutoSlicePixmap());

			this.alphaCutoffEntry = new ToolStripNumericUpDown();
			this.alphaCutoffEntry.BackColor = Color.Transparent;
			this.alphaCutoffEntry.DecimalPlaces = 0;
			this.alphaCutoffEntry.Maximum = 254;
			this.alphaCutoffEntry.NumBackColor = SystemColors.Window;
			this.alphaCutoffEntry.Text = "Alpha Cutoff:";

			this.StateControls.Add(this.addRectButton);
			this.StateControls.Add(this.clearButton);
			this.StateControls.Add(this.deleteSelectedButton);
			this.StateControls.Add(this.autoSliceButton);
			this.StateControls.Add(this.alphaCutoffEntry);
		}

		public void ClearSelection()
		{
			this.deleteSelectedButton.Enabled = false;
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
			this.mouseDown = false;

			// Return immediately if finishing a drag so 
			// that we don't select a rect the drag ended on
			if (this.dragInProgress)
			{
				this.dragInProgress = false;
				return;
			}

			if (this.TargetPixmap == null || this.TargetPixmap.Atlas == null)
				return;

			float x, y;
			this.TransformMouseCoordinates(e.Location, out x, out y);

			int originalSelectedRect = this.selectedRectIndex;

			this.selectedRectIndex = this.TargetPixmap.Atlas
				.IndexOfFirst(r => this.GetDisplayRect(r).Contains(x, y));

			if (this.selectedRectIndex != originalSelectedRect && this.SelectionChanged != null)
				this.SelectionChanged.Invoke(this, EventArgs.Empty);

			this.deleteSelectedButton.Enabled = this.selectedRectIndex != -1;
		}

		public void OnMouseMove(MouseEventArgs e)
		{
			if (this.TargetPixmap == null || this.TargetPixmap.Atlas == null)
				return;

			float x, y;
			this.TransformMouseCoordinates(e.Location, out x, out y);

			// Check for the start of drag operation
			if (this.selectedRectIndex >= 0)
			{
				Rect selectedRect = this.GetDisplayRect(this.TargetPixmap.Atlas[this.selectedRectIndex]);

				if (!this.dragInProgress)
				{
					Side side;
					float distanceToBorder = selectedRect.DistanceToBorder(x, y, out side);
					if (distanceToBorder < DRAG_OFFSET)
					{
						if (this.mouseDown)
							this.dragInProgress = true;

						this.hoveredRectSide = side;
						this.Cursor = (side == Side.Left || side == Side.Right)
							? Cursors.SizeWE
							: (side == Side.Top || side == Side.Bottom)
								? Cursors.SizeNS
								: Cursors.Default;
					}
					else
					{
						this.hoveredRectSide = Side.None;
						this.Cursor = Cursors.Default;
					}
				}
			}
			else
			{
				this.hoveredRectSide = Side.None;
				this.Cursor = Cursors.Default;
			}

			// TODO: Address what happens when height/width becomes negative
			if (this.dragInProgress)
			{
				Rect displayRect = this.GetDisplayRect(this.TargetPixmap.Atlas[this.selectedRectIndex]);
				switch (this.hoveredRectSide)
				{
					case Side.Left:
						displayRect.X = x;
						break;
					case Side.Right:
						displayRect.W += x - displayRect.RightX;
						break;
					case Side.Top:
						displayRect.Y = y;
						break;
					case Side.Bottom:
						displayRect.H += y - displayRect.BottomY;
						break;
				}

				// Keep the displayRect within bounds of the image
				displayRect = displayRect.Intersection(new Rect(
					this.DisplayBounds.X, this.DisplayBounds.Y,
					this.DisplayBounds.Width, this.DisplayBounds.Height));

				Rect atlasRect = this.GetAtlasRect(displayRect);

				// Adjust the width/height of the rect when the left/top change
				if (atlasRect.X != this.TargetPixmap.Atlas[this.selectedRectIndex].X)
				{
					atlasRect.W -= (atlasRect.X - this.TargetPixmap.Atlas[this.selectedRectIndex].X);
				}
				if (atlasRect.Y != this.TargetPixmap.Atlas[this.selectedRectIndex].Y)
				{
					atlasRect.H -= (atlasRect.Y - this.TargetPixmap.Atlas[this.selectedRectIndex].Y);
				}

				this.SetPixmapAtlasRect(atlasRect, this.selectedRectIndex);
			}
		}

		public void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && this.selectedRectIndex != -1)
			{
				this.DeleteSelectedRect();
			}
		}

		public void OnPaint(PaintEventArgs e)
		{
		}

		private void SwitchToAddRectState()
		{
			if (this.StateChangeRequested != null)
				this.StateChangeRequested.Invoke(this,
					new PixmapSlicerForm.PixmapSlicerStateEventArgs(typeof(NewRectPixmapSlicerState)));
		}

		private void ClearRects()
		{
			this.ClearSelection();
			if (this.TargetPixmap != null)
			{
				this.TargetPixmap.Atlas = null;
				this.TargetPixmap.AnimCols = 0;
				this.TargetPixmap.AnimRows = 0;
				this.TargetPixmap.AnimFrameBorder = 0;
				if (this.PixmapUpdated != null)
					this.PixmapUpdated.Invoke(this, EventArgs.Empty);
			}
		}

		private void DeleteSelectedRect()
		{
			if (this.selectedRectIndex == -1)
				return;

			this.TargetPixmap.Atlas.RemoveAt(this.selectedRectIndex);
			this.deleteSelectedButton.Enabled = false;
			this.selectedRectIndex = -1;
			if (this.SelectionChanged != null)
				this.SelectionChanged.Invoke(this, EventArgs.Empty);
			if (this.PixmapUpdated != null)
				this.PixmapUpdated.Invoke(this, EventArgs.Empty);
		}

		private void AutoSlicePixmap()
		{
			if (this.TargetPixmap == null)
				return;

			byte alpha = (byte)this.alphaCutoffEntry.Value;

			IEnumerable<Rect> rects = Utilities.Utilities.FindRects(this.TargetPixmap, alpha);
			if (this.TargetPixmap.Atlas == null)
				this.TargetPixmap.Atlas = new List<Rect>();

			this.ClearSelection();
			this.TargetPixmap.Atlas = rects.ToList();

			if (this.PixmapUpdated != null)
				this.PixmapUpdated.Invoke(this, EventArgs.Empty);
		}

		private void SetPixmapAtlasRect(Rect rect, int index)
		{
			this.TargetPixmap.Atlas[index] = rect;

			if (this.PixmapUpdated != null)
				this.PixmapUpdated.Invoke(this, EventArgs.Empty);
		}
	}
}
