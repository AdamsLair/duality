﻿using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Duality.Editor.Controls.ToolStrip;
using Duality.Editor.Plugins.Base.Forms.PixmapSlicer.Utilities;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.Plugins.Base.UndoRedoActions;
using Duality.Editor.Plugins.Base.Utilities;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	/// <summary>
	/// A <see cref="IPixmapSlicerState"/> that provides the ability to
	/// resize selected atlas rectangles and provides controls for accessing
	/// many other <see cref="IPixmapSlicerState"/>
	/// </summary>
	public class DefaultPixmapSlicerState : PixmapSlicerState
	{
		private const float DRAG_OFFSET = 3f;

		private readonly ToolStripButton		addRectButton			= null;
		private readonly ToolStripButton		clearButton				= null;
		private readonly ToolStripButton		deleteSelectedButton	= null;
		private readonly ToolStripButton		autoSliceButton			= null;
		private readonly ToolStripButton		orderRectsButton		= null;
		private readonly ToolStripNumericUpDown	alphaCutoffEntry		= null;

		private Rect	originalDragRect	= Rect.Empty;
		private bool	mouseDown			= false;
		private bool	dragInProgress		= false;
		private Side	hoveredRectSide		= Side.None;

		public DefaultPixmapSlicerState()
		{
			this.addRectButton = new ToolStripButton(EditorBaseRes.Button_AddRect, null,
				(s, e) => this.ChangeState(typeof(NewRectPixmapSlicerState)));

			this.clearButton = new ToolStripButton(EditorBaseRes.Button_Clear, null,
				(s, e) => this.ClearRects());

			this.deleteSelectedButton = new ToolStripButton(EditorBaseRes.Button_Delete, null,
				(s, e) => this.DeleteSelectedRect());
			this.deleteSelectedButton.Enabled = false;

			this.orderRectsButton = new ToolStripButton(EditorBaseRes.Button_OrderRects, null,
				(s, e) => this.ChangeState(typeof(AtlasOrderingPixmapSlicerState)));

			this.autoSliceButton = new ToolStripButton(EditorBaseRes.Button_AutoSlice, null,
				(s, e) => this.AutoSlicePixmap());

			this.alphaCutoffEntry = new ToolStripNumericUpDown();
			this.alphaCutoffEntry.BackColor = Color.Transparent;
			this.alphaCutoffEntry.DecimalPlaces = 0;
			this.alphaCutoffEntry.Maximum = 254;
			this.alphaCutoffEntry.NumBackColor = Color.FromArgb(196, 196, 196);
			this.alphaCutoffEntry.Text = "Alpha Cutoff:";

			this.addRectButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerAddRect;
			this.clearButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerClear;
			this.deleteSelectedButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerDelete;
			this.orderRectsButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerOrderRects;
			this.autoSliceButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerAutoSlice;

			ToolStripSeparator separator = new ToolStripSeparator();
			separator.BackColor = Color.FromArgb(212, 212, 212);

			this.StateControls.Add(this.addRectButton);
			this.StateControls.Add(this.clearButton);
			this.StateControls.Add(this.deleteSelectedButton);
			this.StateControls.Add(this.orderRectsButton);
			this.StateControls.Add(separator);
			this.StateControls.Add(this.autoSliceButton);
			this.StateControls.Add(this.alphaCutoffEntry);
		}

		public override void ClearSelection()
		{
			this.deleteSelectedButton.Enabled = false;
			this.SelectedRectIndex = -1;
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			this.mouseDown = true;
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			this.mouseDown = false;

			// If finishing a drag operation, commit the change
			if (this.dragInProgress)
			{
				// Set the atlas' rect back to the original rect so the undoing
				// the UndoRedoAction will revert to the original rect
				Rect newRect = this.TargetPixmap.Atlas[this.SelectedRectIndex];
				this.TargetPixmap.Atlas[this.SelectedRectIndex] = this.originalDragRect;
				UndoRedoManager.Do(new SetAtlasRectAction(newRect, this.SelectedRectIndex, new []{ this.TargetPixmap }));
				this.dragInProgress = false;
				return;
			}

			if (this.TargetPixmap == null || this.TargetPixmap.Atlas == null)
				return;

			float x, y;
			this.TransformMouseCoordinates(e.Location, out x, out y);

			this.SelectedRectIndex = this.TargetPixmap.Atlas
				.IndexOfFirst(r => this.GetDisplayRect(r).Contains(x, y));

			this.deleteSelectedButton.Enabled = this.SelectedRectIndex != -1;
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			if (this.TargetPixmap == null || this.TargetPixmap.Atlas == null)
				return;

			float x, y;
			this.TransformMouseCoordinates(e.Location, out x, out y);

			// Check for the start of drag operation
			if (this.SelectedRectIndex >= 0)
			{
				Rect selectedRect = this.GetDisplayRect(this.TargetPixmap.Atlas[this.SelectedRectIndex]);

				if (!this.dragInProgress)
				{
					Side side;
					float distanceToBorder = selectedRect.DistanceToBorder(x, y, out side);
					if (distanceToBorder < DRAG_OFFSET)
					{
						if (this.mouseDown)
						{
							this.dragInProgress = true;
							this.originalDragRect = this.TargetPixmap.Atlas[this.SelectedRectIndex];
						}

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

			if (this.dragInProgress)
			{
				Rect displayRect = this.GetDisplayRect(this.TargetPixmap.Atlas[this.SelectedRectIndex]);
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

				// If width/height has gone negative, switch hover side
				if (displayRect.W < 0)
				{
					displayRect = displayRect.Normalized();
					this.hoveredRectSide = this.hoveredRectSide.Opposite();
				}
				if (displayRect.H < 0)
				{
					displayRect = displayRect.Normalized();
					this.hoveredRectSide = this.hoveredRectSide.Opposite();
				}

				// Keep the displayRect within bounds of the image
				displayRect = displayRect.Intersection(new Rect(
					this.DisplayBounds.X, this.DisplayBounds.Y,
					this.DisplayBounds.Width, this.DisplayBounds.Height));

				Rect atlasRect = this.GetAtlasRect(displayRect);

				// Adjust the width/height of the rect when the left/top change
				if (atlasRect.X != this.TargetPixmap.Atlas[this.SelectedRectIndex].X)
				{
					atlasRect.W -= (atlasRect.X - this.TargetPixmap.Atlas[this.SelectedRectIndex].X);
				}
				if (atlasRect.Y != this.TargetPixmap.Atlas[this.SelectedRectIndex].Y)
				{
					atlasRect.H -= (atlasRect.Y - this.TargetPixmap.Atlas[this.SelectedRectIndex].Y);
				}

				this.SetPixmapAtlasRect(atlasRect, this.SelectedRectIndex);
			}
		}

		public override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && this.SelectedRectIndex != -1)
			{
				this.DeleteSelectedRect();
			}
		}

		private void ClearRects()
		{
			if (this.TargetPixmap == null)
				return;


			UndoRedoManager.Do(new ClearAtlasAction(new[] { this.TargetPixmap }));
			this.ClearSelection();
		}

		private void DeleteSelectedRect()
		{
			if (this.SelectedRectIndex == -1)
				return;

			UndoRedoManager.Do(new DeleteAtlasRectAction(this.SelectedRectIndex, new []{ this.TargetPixmap }));
			this.ClearSelection();
		}

		private void AutoSlicePixmap()
		{
			if (this.TargetPixmap == null)
				return;

			byte alpha = (byte)this.alphaCutoffEntry.Value;

			IEnumerable<Rect> rects = PixmapSlicing.FindRects(this.TargetPixmap, alpha);

			UndoRedoManager.Do(new SetAtlasAction(rects, new []{ this.TargetPixmap }));

			this.ClearSelection();
		}

		private void SetPixmapAtlasRect(Rect rect, int index)
		{
			this.TargetPixmap.Atlas[index] = rect;

			this.UpdateDisplay();
		}

		public override HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			return HelpInfo.FromText(EditorBaseRes.Help_PixmapSlicerDefaultState_Topic,
				EditorBaseRes.Help_PixmapSlicerDefaultState_Desc);
		}
	}
}
