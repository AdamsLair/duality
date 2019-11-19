using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Duality.Input;
using Duality.Resources;
using Duality.Plugins.Tilemaps;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;


namespace Duality.Editor.Plugins.Tilemaps
{
	public class SourcePaletteTilesetView : TilesetView
	{
		private Rectangle  selectedArea           = Rectangle.Empty;
		private Grid<Tile> selectedTiles          = new Grid<Tile>();
		private Point      actionBeginTilePos     = Point.Empty;
		private bool       areTilesSelected       = false;
		private bool       isUserSelecting        = false;
		private bool       isUserScrolling        = false;
		private int        lastMouseX             = -1;
		private int        lastMouseY             = -1;
		private SelectionSide activeSelectionSide = SelectionSide.None;

		private enum SelectionSide
		{
			None,
			Right,
			Left, 
			Up, 
			Down
		}
		

		public event EventHandler SelectedAreaChanged = null;
		public event EventHandler SelectedAreaEditingFinished = null;


		/// <summary>
		/// The selected area of tiles, in displayed tile space.
		/// </summary>
		public Rectangle SelectedArea
		{
			get
			{
				return new Rectangle(
					this.GetDisplayedTilePos(this.selectedArea.X, this.selectedArea.Y), 
					this.selectedArea.Size);
			}
			set
			{
				if (this.selectedArea == value) return;

				// Invalidate the previous selected area to account for repaints when reducing selection size
				this.InvalidateTiles(
					this.GetTileIndex(this.selectedArea.X, this.selectedArea.Y), 
					this.selectedArea.Width, 
					this.selectedArea.Height, 
					6);

				// Determine an actually valid area we can select, in displayed space
				Rectangle croppedArea = new Rectangle(
					Math.Max(value.X, 0),
					Math.Max(value.Y, 0),
					Math.Min(value.Width, this.DisplayedTileCount.X - Math.Max(value.X, 0)),
					Math.Min(value.Height, this.DisplayedTileCount.Y - Math.Max(value.Y, 0)));

				// Assign selected area, but go back to original tileset space for this,
				// so our selection area remains consistent when changing the display mode
				// for tiles.
				this.selectedArea = new Rectangle(
					this.GetTilesetTilePos(croppedArea.X, croppedArea.Y),
					croppedArea.Size);

				this.UpdateSelectedTiles();
				this.RaiseSelectedAreaChanged();
				this.InvalidateTiles(
					this.GetTileIndex(this.selectedArea.X, this.selectedArea.Y), 
					this.selectedArea.Width, 
					this.selectedArea.Height, 
					6);
			}
		}
		public IReadOnlyGrid<Tile> SelectedTiles
		{
			get { return this.selectedTiles; }
		}
		internal void TranslateSelectedArea(int offsetX, int offsetY)
		{
			if (this.selectedArea.IsEmpty)
			{
				this.InitializeSelectedArea();
				return;
			}

			int newX = MathF.Clamp(this.selectedArea.X + offsetX, 0, this.DisplayedTileCount.X - this.selectedArea.Width);
			int newY = MathF.Clamp(this.selectedArea.Y + offsetY, 0, this.DisplayedTileCount.Y - this.selectedArea.Height);

			this.SelectedArea = new Rectangle(newX, newY, this.selectedArea.Width, this.selectedArea.Height); 
			this.RaiseSelectedAreaEditingFinished();
			this.Invalidate();
		}
		internal void ExpandSelectedArea(int diffX, int diffY)
		{
			if (this.selectedArea.IsEmpty)
			{
				this.InitializeSelectedArea();
				return;
			}

			int newX = MathF.Max(this.selectedArea.X + MathF.Min(diffX, 0), 0);
			int newY = MathF.Max(this.selectedArea.Y + MathF.Min(diffY, 0), 0);

			int newWidth = this.selectedArea.Width + MathF.Max(diffX, 0);
			newWidth = MathF.Min(newWidth, this.DisplayedTileCount.X - this.selectedArea.X);
			newWidth += this.selectedArea.X - newX;

			int newHeight = this.selectedArea.Height + MathF.Max(diffY, 0);
			newHeight = MathF.Min(newHeight, this.DisplayedTileCount.Y - this.selectedArea.Y);
			newHeight += this.selectedArea.Y - newY;

			this.SelectedArea = new Rectangle(newX, newY, newWidth, newHeight);
			this.RaiseSelectedAreaEditingFinished();
			this.Invalidate();
		}
		public void ShrinkSelectedArea(int diffX, int diffY)
		{
			if (this.selectedArea.IsEmpty)
			{
				this.InitializeSelectedArea();
				return;
			}

			int newX = MathF.Min(this.selectedArea.X + MathF.Max(diffX, 0),
				this.selectedArea.X + this.selectedArea.Width - 1);
			int newY = MathF.Min(this.selectedArea.Y + MathF.Max(diffY, 0),
				this.selectedArea.Y + this.selectedArea.Height - 1);

			int newWidth = MathF.Max(this.selectedArea.Width + MathF.Min(diffX, 0), 1);
			newWidth -= newX - this.selectedArea.X;

			int newHeight = MathF.Max(this.selectedArea.Height + MathF.Min(diffY, 0), 1);
			newHeight -= newY - this.selectedArea.Y;

			this.SelectedArea = new Rectangle(newX, newY, newWidth, newHeight);
			this.RaiseSelectedAreaEditingFinished();
			this.Invalidate();
		}

		protected override void OnTilesetChanged()
		{
			base.OnTilesetChanged();
			this.SelectedArea = Rectangle.Empty;
			this.RaiseSelectedAreaEditingFinished();
		}
		protected override void OnTileDisplayModeChanged()
		{
			base.OnTileDisplayModeChanged();

			// Remove selection if it extends beyound selectable boundaries now that the layout 
			// may have changed. 
			// Note that this only handles cases where the updated selection would tap outside 
			// the valid region. If it's still within after the layout change, this code won't
			// trigger and UpdateSelectedTiles will properly redo the mapping, which is probably
			// just fine for now.
			if (this.DisplayedTileCount.X < this.SelectedArea.X + this.SelectedArea.Width ||
				this.DisplayedTileCount.Y < this.SelectedArea.Y + this.SelectedArea.Height)
			{
				this.SelectedArea = Rectangle.Empty;
			}

			this.UpdateSelectedTiles();
			this.RaiseSelectedAreaChanged();
			this.RaiseSelectedAreaEditingFinished();
			this.Invalidate();
		}
		protected override void OnPaintTiles(PaintEventArgs e)
		{
			Tileset tileset = this.TargetTileset.Res;
			Color highlightColor = Color.White;
			Color highlightBorderColor = Color.Black;
			Region regularClip = e.Graphics.Clip;
			Region selectionClip = regularClip.Clone();

			// Fill the selection background
			if (this.Enabled && !this.selectedArea.IsEmpty)
			{
				int startIndex = this.GetTileIndex(this.selectedArea.X, this.selectedArea.Y);
				Point startPos = this.GetTileIndexLocation(startIndex);
				Rectangle rect = new Rectangle(
					startPos.X, 
					startPos.Y,
					this.selectedArea.Width * (this.DisplayedTileSize.Width + this.Spacing.Width) - this.Spacing.Width, 
					this.selectedArea.Height * (this.DisplayedTileSize.Height + this.Spacing.Height) - this.Spacing.Height);
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(32, highlightBorderColor)), rect);

				rect.Inflate(4, 4);
				selectionClip.Exclude(rect);
				e.Graphics.Clip = selectionClip;
			}

			// Draw hovered tile background
			if (this.Enabled && this.HoveredTileIndex != -1)
			{
				Point startPos = this.GetTileIndexLocation(this.HoveredTileIndex);
				Rectangle rect = new Rectangle(
					startPos.X, 
					startPos.Y,
					this.DisplayedTileSize.Width, 
					this.DisplayedTileSize.Height);
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(32, highlightBorderColor)), rect);
			}

			// Paint the tile layer itself
			e.Graphics.Clip = regularClip;
			base.OnPaintTiles(e);
			e.Graphics.Clip = selectionClip;

			// Draw hovered tile foreground
			if (this.Enabled && this.HoveredTileIndex != -1)
			{
				Point startPos = this.GetTileIndexLocation(this.HoveredTileIndex);
				Rectangle rect = new Rectangle(
					startPos.X, 
					startPos.Y,
					this.DisplayedTileSize.Width, 
					this.DisplayedTileSize.Height);

				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(32, highlightBorderColor)), rect);
				rect.Inflate(1, 1);

				rect.Size = new Size(rect.Size.Width - 1, rect.Size.Height - 1);
				e.Graphics.DrawRectangle(new Pen(highlightBorderColor), rect);
				rect.Inflate(1, 1);
				e.Graphics.DrawRectangle(new Pen(highlightColor), rect);
				rect.Inflate(1, 1);
				e.Graphics.DrawRectangle(new Pen(highlightBorderColor), rect);
			}

			// Draw selection indicators
			if (this.Enabled && !this.selectedArea.IsEmpty)
			{
				e.Graphics.Clip = regularClip;

				int startIndex = this.GetTileIndex(this.selectedArea.X, this.selectedArea.Y);
				Point startPos = this.GetTileIndexLocation(startIndex);
				Rectangle rect = new Rectangle(
					startPos.X, 
					startPos.Y,
					this.selectedArea.Width * (this.DisplayedTileSize.Width + this.Spacing.Width) - this.Spacing.Width, 
					this.selectedArea.Height * (this.DisplayedTileSize.Height + this.Spacing.Height) - this.Spacing.Height);
				rect.Inflate(1, 1);
				rect.Size = new Size(rect.Size.Width - 1, rect.Size.Height - 1);

				// Draw the selected tile area border
				e.Graphics.DrawRectangle(new Pen(highlightBorderColor), rect);
				rect.Inflate(1, 1);
				e.Graphics.DrawRectangle(new Pen(highlightColor), rect);
				rect.Inflate(1, 1);
				e.Graphics.DrawRectangle(new Pen(highlightColor), rect);
				rect.Inflate(1, 1);
				e.Graphics.DrawRectangle(new Pen(highlightBorderColor), rect);
				rect.Inflate(1, 1);

				// Draw the outer shadow of the selected tile area
				e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, highlightBorderColor)), rect);
				rect.Inflate(1, 1);
				e.Graphics.DrawRectangle(new Pen(Color.FromArgb(64, highlightBorderColor)), rect);
				rect.Inflate(1, 1);
				e.Graphics.DrawRectangle(new Pen(Color.FromArgb(32, highlightBorderColor)), rect);
			}
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Shift && !this.isUserScrolling)
			{
				int tileIndex = this.PickTileIndexAt(e.X, e.Y);
				if (tileIndex != -1)
				{
					this.isUserSelecting = true;
					this.HoveredTileIndex = -1;
					if (tileIndex != -1 && !this.areTilesSelected)
					{
						this.areTilesSelected = true;
						this.actionBeginTilePos = this.GetTilePos(tileIndex);
						this.isUserSelecting = true;
						this.SelectedArea = new Rectangle(
							this.GetDisplayedTilePos(this.actionBeginTilePos.X, this.actionBeginTilePos.Y),
							new Size(1, 1));
						this.HoveredTileIndex = -1;
					}
					else if (tileIndex != -1)
					{
						Point tilePos = this.GetTilePos(tileIndex);
						Point displayedBeginPos = this.GetDisplayedTilePos(this.actionBeginTilePos.X, this.actionBeginTilePos.Y);
						Point displayedCurrentPos = this.GetDisplayedTilePos(tilePos.X, tilePos.Y);

						Point selectionTopLeft = new Point(
							Math.Min(displayedBeginPos.X, displayedCurrentPos.X),
							Math.Min(displayedBeginPos.Y, displayedCurrentPos.Y));
						Point selectionBottomRight = new Point(
							Math.Max(displayedBeginPos.X, displayedCurrentPos.X),
							Math.Max(displayedBeginPos.Y, displayedCurrentPos.Y));

						this.SelectedArea = new Rectangle(
							selectionTopLeft.X,
							selectionTopLeft.Y,
							selectionBottomRight.X - selectionTopLeft.X + 1,
							selectionBottomRight.Y - selectionTopLeft.Y + 1);
					}
				}
			}
			else if (e.Button == MouseButtons.Left && !this.isUserScrolling)
			{
				int tileIndex = this.PickTileIndexAt(e.X, e.Y);
				if (tileIndex != -1)
				{
					this.areTilesSelected = true;
					this.actionBeginTilePos = this.GetTilePos(tileIndex);
					this.isUserSelecting = true;
					this.SelectedArea = new Rectangle(
						this.GetDisplayedTilePos(this.actionBeginTilePos.X, this.actionBeginTilePos.Y),
						new Size(1, 1));
					this.HoveredTileIndex = -1;
				}
			}
			else if (e.Button == MouseButtons.Middle && !this.isUserSelecting)
			{
				this.isUserScrolling = true;
				this.lastMouseX = e.X;
				this.lastMouseY = e.Y;
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!this.isUserSelecting && !this.isUserScrolling)
			{
				this.SelectedArea = Rectangle.Empty;
				this.areTilesSelected = false;
			}
			if (e.Button == MouseButtons.Middle)
			{
				this.isUserScrolling = false;
				this.lastMouseX = -1;
				this.lastMouseY = -1;
			}
			if (e.Button == MouseButtons.Left)
			{
				this.isUserSelecting = false;
				this.RaiseSelectedAreaEditingFinished();
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			int tileIndex = this.PickTileIndexAt(e.X, e.Y);
			if (this.isUserSelecting)
			{
				if (tileIndex != -1)
				{
					Point tilePos = this.GetTilePos(tileIndex);
					Point displayedBeginPos = this.GetDisplayedTilePos(this.actionBeginTilePos.X, this.actionBeginTilePos.Y);
					Point displayedCurrentPos = this.GetDisplayedTilePos(tilePos.X, tilePos.Y);

					Point selectionTopLeft = new Point(
						Math.Min(displayedBeginPos.X, displayedCurrentPos.X), 
						Math.Min(displayedBeginPos.Y, displayedCurrentPos.Y));
					Point selectionBottomRight = new Point(
						Math.Max(displayedBeginPos.X, displayedCurrentPos.X), 
						Math.Max(displayedBeginPos.Y, displayedCurrentPos.Y));

					this.SelectedArea = new Rectangle(
						selectionTopLeft.X,
						selectionTopLeft.Y,
						selectionBottomRight.X - selectionTopLeft.X + 1,
						selectionBottomRight.Y - selectionTopLeft.Y + 1);
				}
			}
			else if (this.isUserScrolling)
			{
				int newHoz = this.HorizontalScroll.Value - e.X + this.lastMouseX;
				MathF.Clamp(newHoz, this.HorizontalScroll.Minimum, this.HorizontalScroll.Maximum);
				int newVert = this.VerticalScroll.Value - e.Y + this.lastMouseY;
				MathF.Clamp(newVert, this.VerticalScroll.Minimum, this.VerticalScroll.Maximum);
				this.AutoScrollPosition = new Point(newHoz, newVert);
				this.lastMouseX = e.X;
				this.lastMouseY = e.Y;
			}
			else
			{
				int lastHovered = this.HoveredTileIndex;
				base.OnMouseMove(e);
				if (lastHovered != this.HoveredTileIndex)
				{
					this.InvalidateTile(lastHovered, 6);
					this.InvalidateTile(this.HoveredTileIndex, 6);
				}
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			this.InvalidateTile(this.HoveredTileIndex, 5);
			base.OnMouseLeave(e);
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (Control.ModifierKeys == Keys.Shift)
			{
				switch (e.KeyCode)
				{
					case Keys.Up when this.activeSelectionSide == SelectionSide.Down:
						this.ShrinkSelectedArea(0, -1);
						break;
					case Keys.Up:
						this.ExpandSelectedArea(0, -1);
						this.activeSelectionSide = SelectionSide.Up;
						break;
					case Keys.Down when this.activeSelectionSide == SelectionSide.Up:
						this.ShrinkSelectedArea(0, 1);
						break;
					case Keys.Down:
						this.ExpandSelectedArea(0, 1);
						this.activeSelectionSide = SelectionSide.Down;
						break;
					case Keys.Left when this.activeSelectionSide == SelectionSide.Right:
						this.ShrinkSelectedArea(-1, 0);
						break;
					case Keys.Left:
						this.ExpandSelectedArea(-1, 0);
						this.activeSelectionSide = SelectionSide.Left;
						break;
					case Keys.Right when this.activeSelectionSide == SelectionSide.Left:
						this.ShrinkSelectedArea(1, 0);
						break;
					case Keys.Right:
						this.ExpandSelectedArea(1, 0);
						this.activeSelectionSide = SelectionSide.Right;
						break;
				}
			}
			else
			{
				if (e.KeyCode == Keys.Up)
				{
					this.TranslateSelectedArea(0, -1);
				}
				if (e.KeyCode == Keys.Down)
				{
					this.TranslateSelectedArea(0, 1);
				}
				if (e.KeyCode == Keys.Left)
				{
					this.TranslateSelectedArea(-1, 0);
				}
				if (e.KeyCode == Keys.Right)
				{
					this.TranslateSelectedArea(1, 0);
				}
				this.activeSelectionSide = SelectionSide.None;
			}
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ShiftKey)
			{
				this.activeSelectionSide = SelectionSide.None;
			}
		}
		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Right:
				case Keys.Left:
				case Keys.Up:
				case Keys.Down:
				case Keys.Shift:
				case Keys.Shift | Keys.Right:
				case Keys.Shift | Keys.Left:
				case Keys.Shift | Keys.Up:
				case Keys.Shift | Keys.Down:
					return true;
			}
			return base.IsInputKey(keyData);
		}

		private void UpdateSelectedTiles()
		{
			// Allocate and clear a tile rect space of the appropriate size.
			this.selectedTiles.ResizeClear(this.selectedArea.Width, this.selectedArea.Height);

			// Retrieve the active Tileset and early-out, if there is none.
			// We can't provide a meaningful tile selection when there are
			// none to choose from. Keep the default-initialized ones we got above.
			Tileset tileset = this.TargetTileset.Res;
			if (tileset == null) return;

			// Determine a tile rect based on the current selection inside the Tileset.
			Point selectedDisplayedPos = this.GetDisplayedTilePos(
				this.selectedArea.X,
				this.selectedArea.Y);
			for (int y = 0; y < this.selectedArea.Height; y++)
			{
				for (int x = 0; x < this.selectedArea.Width; x++)
				{
					Point displayedPos = new Point(
						selectedDisplayedPos.X + x,
						selectedDisplayedPos.Y + y);
					Point tilesetPos = this.GetTilesetTilePos(
						displayedPos.X,
						displayedPos.Y);

					int baseIndex = this.GetTileIndex(tilesetPos.X, tilesetPos.Y);
					baseIndex = MathF.Clamp(baseIndex, 0, tileset.TileCount - 1);

					this.selectedTiles[x, y] = new Tile(baseIndex, tileset);
				}
			}
		}
		private void InitializeSelectedArea()
		{
			Rectangle rect = new Rectangle(0, 0, 1, 1);
			this.SelectedArea = rect;
			this.RaiseSelectedAreaEditingFinished();
		}

		private void RaiseSelectedAreaEditingFinished()
		{
			this.SelectedAreaEditingFinished?.Invoke(this, EventArgs.Empty);
		}
		private void RaiseSelectedAreaChanged()
		{
			this.SelectedAreaChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}