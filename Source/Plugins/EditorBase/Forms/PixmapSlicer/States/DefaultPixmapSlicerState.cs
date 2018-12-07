using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Duality.Editor.Controls.ToolStrip;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.Plugins.Base.UndoRedoActions;
using Duality.Resources;


namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	/// <summary>
	/// A <see cref="IPixmapSlicerState"/> that provides the ability to
	/// resize selected atlas rectangles and provides controls for accessing
	/// many other <see cref="IPixmapSlicerState"/>
	/// </summary>
	public class DefaultPixmapSlicerState : PixmapSlicerState
	{
		private readonly ToolStripButton        addRectButton        = null;
		private readonly ToolStripButton        clearButton          = null;
		private readonly ToolStripButton        deleteSelectedButton = null;
		private readonly ToolStripButton        autoSliceButton      = null;
		private readonly ToolStripButton        orderRectsButton     = null;
		private readonly ToolStripNumericUpDown alphaCutoffEntry     = null;
		private readonly ToolStripButton        gridSliceButton      = null;

		private Rect                  originalDragRect = Rect.Empty;
		private int                   draggedRectIndex = -1;
		private PixmapSlicingRectSide draggedRectSide  = PixmapSlicingRectSide.None;


		public DefaultPixmapSlicerState()
		{
			this.addRectButton = new ToolStripButton(null, EditorBaseResCache.IconSquareAdd,
				(s, e) => this.SwitchToState(typeof(NewRectPixmapSlicerState)));

			this.deleteSelectedButton = new ToolStripButton(null, EditorBaseResCache.IconSquareDelete,
				(s, e) => this.DeleteSelectedRect());
			this.deleteSelectedButton.Enabled = false;

			this.clearButton = new ToolStripButton(null, EditorBaseResCache.IconSquareDeleteMany,
				(s, e) => this.ClearRects());

			this.orderRectsButton = new ToolStripButton(null, EditorBaseResCache.IconSquareNumbers,
				(s, e) => this.SwitchToState(typeof(AtlasOrderingPixmapSlicerState)));

			this.autoSliceButton = new ToolStripButton(EditorBaseRes.Button_AutoSlice, null,
				(s, e) => this.AutoSlicePixmap());

			this.gridSliceButton = new ToolStripButton(EditorBaseRes.Button_GridSlice, null,
				(s, e) => this.SwitchToState(typeof(GridSlicePixmapSlicerState)));

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

		private void ClearRects()
		{
			if (this.TargetPixmap == null)
				return;

			UndoRedoManager.Do(new ClearAtlasAction(new[] { this.TargetPixmap }));
			this.View.ClearSelection();
		}
		private void DeleteSelectedRect()
		{
			if (this.View.SelectedAtlasIndex == -1)
				return;

			UndoRedoManager.Do(new DeleteAtlasRectAction(this.View.SelectedAtlasIndex, new[] { this.TargetPixmap }));
			this.View.ClearSelection();
		}

		private void AutoSlicePixmap()
		{
			if (this.TargetPixmap == null) return;

			byte alpha = (byte)this.alphaCutoffEntry.Value;
			RectAtlas atlas = PixmapSlicingUtility.SliceAutoFit(this.TargetPixmap, alpha);
			this.SetAtlas(atlas);
			UndoRedoManager.Finish();
		}

		private void BeginDragRect(int index, PixmapSlicingRectSide side)
		{
			this.draggedRectIndex = index;
			this.draggedRectSide = side;
		}
		private void EndDragRect()
		{
			this.draggedRectIndex = -1;
			this.draggedRectSide = PixmapSlicingRectSide.None;
		}

		private void UpdateCursor()
		{
			switch (this.View.HoveredAtlasRectSide)
			{
				case PixmapSlicingRectSide.Left:
				case PixmapSlicingRectSide.Right:
					this.View.Cursor = Cursors.SizeWE;
					break;
				case PixmapSlicingRectSide.Top:
				case PixmapSlicingRectSide.Bottom:
					this.View.Cursor = Cursors.SizeNS;
					break;
				case PixmapSlicingRectSide.Pivot:
					this.View.Cursor = Cursors.SizeAll;
					break;
				default:
					this.View.Cursor = Cursors.Default;
					break;
			}
		}

		public override void OnStateEntered(EventArgs e)
		{
			base.OnStateEntered(e);
			this.View.SelectedAtlasChanged += this.View_SelectedAtlasChanged;
			this.View.HoveredAtlasChanged += this.View_HoveredAtlasChanged;
			this.UpdateCursor();
		}
		public override void OnStateLeaving(EventArgs e)
		{
			base.OnStateLeaving(e);
			this.EndDragRect();
			this.View.SelectedAtlasChanged -= this.View_SelectedAtlasChanged;
			this.View.HoveredAtlasChanged -= this.View_HoveredAtlasChanged;
			this.View.Cursor = Cursors.Default;
		}
		public override void OnMouseDown(MouseEventArgs e)
		{
			if (this.View.HoveredAtlasRectSide != PixmapSlicingRectSide.None)
			{
				this.BeginDragRect(
					this.View.HoveredAtlasIndex, 
					this.View.HoveredAtlasRectSide);
			}
			else
			{
				this.EndDragRect();
			}
		}
		public override void OnMouseUp(MouseEventArgs e)
		{
			if (this.draggedRectIndex != -1)
			{
				this.EndDragRect();
				UndoRedoManager.Finish();
			}
		}
		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (this.draggedRectSide != PixmapSlicingRectSide.None)
			{
				if (this.draggedRectSide == PixmapSlicingRectSide.Pivot)
				{
					Vector2 atlasPos = this.View.GetAtlasPos(e.Location);
					Rect draggedAtlasRect = this.View.GetAtlasRect(this.draggedRectIndex);

					// Keep the new pivot within the atlas rect
					if (atlasPos.X < draggedAtlasRect.LeftX) atlasPos.X = draggedAtlasRect.LeftX;
					if (atlasPos.Y < draggedAtlasRect.TopY) atlasPos.Y = draggedAtlasRect.TopY;
					if (atlasPos.X > draggedAtlasRect.RightX) atlasPos.X = draggedAtlasRect.RightX;
					if (atlasPos.Y > draggedAtlasRect.BottomY) atlasPos.Y = draggedAtlasRect.BottomY;

					Vector2 newPivotInRectSpace = atlasPos - draggedAtlasRect.Center;
					this.SetAtlasPivot(this.draggedRectIndex, newPivotInRectSpace);
				}
				else
				{
					Vector2 atlasPos = this.View.GetAtlasPos(e.Location);
					Rect draggedAtlasRect = this.View.GetAtlasRect(this.draggedRectIndex);

					// Move dragged side to mouse
					switch (this.draggedRectSide)
					{
						case PixmapSlicingRectSide.Left:
							draggedAtlasRect.W += draggedAtlasRect.X - atlasPos.X;
							draggedAtlasRect.X = atlasPos.X;
							break;
						case PixmapSlicingRectSide.Right:
							draggedAtlasRect.W += atlasPos.X - draggedAtlasRect.RightX;
							break;
						case PixmapSlicingRectSide.Top:
							draggedAtlasRect.H += draggedAtlasRect.Y - atlasPos.Y;
							draggedAtlasRect.Y = atlasPos.Y;
							break;
						case PixmapSlicingRectSide.Bottom:
							draggedAtlasRect.H += atlasPos.Y - draggedAtlasRect.BottomY;
							break;
					}

					// If width / height has gone negative, switch hover side
					if (draggedAtlasRect.W < 0 || draggedAtlasRect.H < 0)
					{
						draggedAtlasRect = draggedAtlasRect.Normalized();
						this.draggedRectSide = this.draggedRectSide.Opposite();
					}

					// Keep the displayRect within bounds of the image
					Rect pixmapRect = new Rect(0.0f, 0.0f, this.TargetPixmap.Width, this.TargetPixmap.Height);
					draggedAtlasRect = draggedAtlasRect.Intersection(pixmapRect);

					// Clamp the atlas rect to full pixel values
					draggedAtlasRect = new Rect(
						MathF.RoundToInt(draggedAtlasRect.LeftX),
						MathF.RoundToInt(draggedAtlasRect.TopY),
						MathF.RoundToInt(draggedAtlasRect.RightX) - MathF.RoundToInt(draggedAtlasRect.LeftX),
						MathF.RoundToInt(draggedAtlasRect.BottomY) - MathF.RoundToInt(draggedAtlasRect.TopY));

					// Apply the new atlas rect
					if (draggedAtlasRect.W != 0 && draggedAtlasRect.H != 0)
					{
						this.SetAtlasRect(this.draggedRectIndex, draggedAtlasRect);
					}
				}
			}
		}
		public override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && this.View.SelectedAtlasIndex != -1)
			{
				this.EndDragRect();
				this.DeleteSelectedRect();
			}
		}
		protected override void OnTargetPixmapChanged()
		{
			base.OnTargetPixmapChanged();
			this.EndDragRect();
		}

		private void View_SelectedAtlasChanged(object sender, EventArgs e)
		{
			this.deleteSelectedButton.Enabled = this.View.SelectedAtlasIndex != -1;
		}
		private void View_HoveredAtlasChanged(object sender, EventArgs e)
		{
			this.UpdateCursor();
		}

		public override HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			return HelpInfo.FromText(EditorBaseRes.Help_PixmapSlicerDefaultState_Topic,
				EditorBaseRes.Help_PixmapSlicerDefaultState_Desc);
		}
	}
}
