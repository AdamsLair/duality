using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Duality.Editor.Controls.ToolStrip;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.Plugins.Base.UndoRedoActions;

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

		private readonly ToolStripButton        addRectButton        = null;
		private readonly ToolStripButton        clearButton          = null;
		private readonly ToolStripButton        deleteSelectedButton = null;
		private readonly ToolStripButton        autoSliceButton      = null;
		private readonly ToolStripButton        orderRectsButton     = null;
		private readonly ToolStripNumericUpDown alphaCutoffEntry     = null;
		private readonly ToolStripButton        gridSliceButton      = null;

		private Rect originalDragRect = Rect.Empty;
		private bool mouseDown        = false;
		private bool dragInProgress   = false;

		private PixmapSlicingRectSide hoveredRectSide = PixmapSlicingRectSide.None;


		public DefaultPixmapSlicerState()
		{
			this.addRectButton = new ToolStripButton(null, EditorBaseResCache.IconSquareAdd,
				(s, e) => this.ChangeState(typeof(NewRectPixmapSlicerState)));

			this.deleteSelectedButton = new ToolStripButton(null, EditorBaseResCache.IconSquareDelete,
				(s, e) => this.DeleteSelectedRect());
			this.deleteSelectedButton.Enabled = false;

			this.clearButton = new ToolStripButton(null, EditorBaseResCache.IconSquareDeleteMany,
				(s, e) => this.ClearRects());

			this.orderRectsButton = new ToolStripButton(null, EditorBaseResCache.IconSquareNumbers,
				(s, e) => this.ChangeState(typeof(AtlasOrderingPixmapSlicerState)));

			this.autoSliceButton = new ToolStripButton(EditorBaseRes.Button_AutoSlice, null,
				(s, e) => this.AutoSlicePixmap());

			this.gridSliceButton = new ToolStripButton(EditorBaseRes.Button_GridSlice, null,
				(s, e) => this.ChangeState(typeof(GridSlicePixmapSlicerState)));

			this.alphaCutoffEntry = CreateNumericUpDown("Alpha Cutoff:", 0, 254);

			this.addRectButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerAddRect;
			this.deleteSelectedButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerDelete;
			this.clearButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerClear;
			this.orderRectsButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerOrderRects;
			this.autoSliceButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerAutoSlice;
			this.gridSliceButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerGridSlice;

			this.StateControls.Add(this.addRectButton);
			this.StateControls.Add(this.deleteSelectedButton);
			this.StateControls.Add(this.clearButton);
			this.StateControls.Add(this.orderRectsButton);
			this.StateControls.Add(new ToolStripSeparator { BackColor = Color.FromArgb(212, 212, 212) });
			this.StateControls.Add(this.autoSliceButton);
			this.StateControls.Add(this.alphaCutoffEntry);
			this.StateControls.Add(new ToolStripSeparator { BackColor = Color.FromArgb(212, 212, 212) });
			this.StateControls.Add(this.gridSliceButton);
		}

		public override void ClearSelection()
		{
			this.deleteSelectedButton.Enabled = false;
			this.SelectedRectIndex = -1;
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

			UndoRedoManager.Do(new DeleteAtlasRectAction(this.SelectedRectIndex, new[] { this.TargetPixmap }));
			this.ClearSelection();
		}

		private void AutoSlicePixmap()
		{
			if (this.TargetPixmap == null)
				return;

			byte alpha = (byte)this.alphaCutoffEntry.Value;

			IEnumerable<Rect> rects = PixmapSlicingUtility.FindRects(this.TargetPixmap, alpha);

			UndoRedoManager.Do(new SetAtlasAction(rects, new[] { this.TargetPixmap }));

			this.ClearSelection();
		}

		private void SetPixmapAtlasRect(Rect rect, int index)
		{
			Rect oldRect = this.TargetPixmap.Atlas[index];
			this.TargetPixmap.Atlas[index] = rect;
			Rect updatedArea = rect.ExpandedToContain(oldRect);
			this.UpdateDisplay(updatedArea);
		}
		private void SetHoveredSide(PixmapSlicingRectSide side)
		{
			this.hoveredRectSide = side;
			switch (side)
			{
				case PixmapSlicingRectSide.Left:
				case PixmapSlicingRectSide.Right:
					this.Cursor = Cursors.SizeWE;
					break;
				case PixmapSlicingRectSide.Top:
				case PixmapSlicingRectSide.Bottom:
					this.Cursor = Cursors.SizeNS;
					break;
				default:
					this.Cursor = Cursors.Default;
					break;
			}
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			this.mouseDown = true;
		}
		public override void OnMouseUp(MouseEventArgs e)
		{
			this.mouseDown = false;

			if (this.TargetPixmap == null || this.TargetPixmap.Atlas == null)
				return;

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

			this.SelectedRectIndex = this.View.HoveredAtlasIndex;
			this.deleteSelectedButton.Enabled = this.SelectedRectIndex != -1;
		}
		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (this.TargetPixmap == null 
				|| this.TargetPixmap.Atlas == null 
				|| this.SelectedRectIndex < 0)
				return;

			Vector2 atlasPos = this.View.GetAtlasPos(e.Location);
			Rect selectedDisplayRect = this.View.GetDisplayRect(this.TargetPixmap.Atlas[this.SelectedRectIndex]);

			// Check for the start of a drag operation
			if (!this.dragInProgress)
			{
				PixmapSlicingRectSide side;
				if (selectedDisplayRect.WithinRangeToBorder(e.X, e.Y, DRAG_OFFSET, out side))
				{
					if (this.mouseDown)
					{
						this.dragInProgress = true;
						this.originalDragRect = this.TargetPixmap.Atlas[this.SelectedRectIndex];
					}

					this.SetHoveredSide(side);
				}
				else
				{
					this.SetHoveredSide(PixmapSlicingRectSide.None);
				}
			}

			if (this.dragInProgress)
			{
				// Move hovered side to mouse
				switch (this.hoveredRectSide)
				{
					case PixmapSlicingRectSide.Left:
						selectedDisplayRect.W += selectedDisplayRect.X - e.X;
						selectedDisplayRect.X = e.X;
						break;
					case PixmapSlicingRectSide.Right:
						selectedDisplayRect.W += e.X - selectedDisplayRect.RightX;
						break;
					case PixmapSlicingRectSide.Top:
						selectedDisplayRect.H += selectedDisplayRect.Y - e.Y;
						selectedDisplayRect.Y = e.Y;
						break;
					case PixmapSlicingRectSide.Bottom:
						selectedDisplayRect.H += e.Y - selectedDisplayRect.BottomY;
						break;
				}

				// If width/height has gone negative, switch hover side
				if (selectedDisplayRect.W < 0 || selectedDisplayRect.H < 0)
				{
					selectedDisplayRect = selectedDisplayRect.Normalized();
					this.hoveredRectSide = this.hoveredRectSide.Opposite();
				}

				// Keep the displayRect within bounds of the image
				Rectangle displayImageRect = this.View.DisplayedImageRect;
				selectedDisplayRect = selectedDisplayRect.Intersection(new Rect(
					displayImageRect.X, displayImageRect.Y,
					displayImageRect.Width, displayImageRect.Height));

				Rect atlasRect = this.View.GetAtlasRect(selectedDisplayRect);
				atlasRect.X = MathF.RoundToInt(atlasRect.X);
				atlasRect.Y = MathF.RoundToInt(atlasRect.Y);
				atlasRect.W = MathF.RoundToInt(atlasRect.W);
				atlasRect.H = MathF.RoundToInt(atlasRect.H);
				if (atlasRect.W != 0 && atlasRect.H != 0)
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

		public override HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			return HelpInfo.FromText(EditorBaseRes.Help_PixmapSlicerDefaultState_Topic,
				EditorBaseRes.Help_PixmapSlicerDefaultState_Desc);
		}
	}
}
