using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Resources;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps
{
	public class SourcePaletteTilesetView : TilesetView
	{
		private Rectangle  selectedArea           = Rectangle.Empty;
		private Grid<Tile> selectedTiles          = new Grid<Tile>();
		private Point      actionBeginTilePos     = Point.Empty;
		private bool       isUserSelecting        = false;

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
					5);
			}
		}
		public IReadOnlyGrid<Tile> SelectedTiles
		{
			get { return this.selectedTiles; }
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
			if (e.Button == MouseButtons.Left)
			{
				int tileIndex = this.PickTileIndexAt(e.X, e.Y);
				if (tileIndex != -1)
				{
					this.actionBeginTilePos = this.GetTilePos(tileIndex);
					this.isUserSelecting = true;
					this.SelectedArea = new Rectangle(
						this.GetDisplayedTilePos(this.actionBeginTilePos.X, this.actionBeginTilePos.Y), 
						new Size(1, 1));
					this.HoveredTileIndex = -1;
				}
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!this.isUserSelecting)
			{
				this.SelectedArea = Rectangle.Empty;
			}
			this.actionBeginTilePos = Point.Empty;
			this.isUserSelecting = false;
			this.RaiseSelectedAreaEditingFinished();
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
			else
			{
				int lastHovered = this.HoveredTileIndex;
				base.OnMouseMove(e);
				if (lastHovered != this.HoveredTileIndex)
				{
					this.InvalidateTile(lastHovered, 5);
					this.InvalidateTile(this.HoveredTileIndex, 5);
				}
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			this.InvalidateTile(this.HoveredTileIndex, 5);
			base.OnMouseLeave(e);
		}

		private void UpdateSelectedTiles()
		{
			Point selectedDisplayedPos = this.GetDisplayedTilePos(
				this.selectedArea.X, 
				this.selectedArea.Y);

			this.selectedTiles.ResizeClear(this.selectedArea.Width, this.selectedArea.Height);
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
					this.selectedTiles[x, y] = new Tile
					{
						Index = this.GetTileIndex(tilesetPos.X, tilesetPos.Y)
					};
				}
			}
		}

		private void RaiseSelectedAreaEditingFinished()
		{
			if (this.SelectedAreaEditingFinished != null)
				this.SelectedAreaEditingFinished(this, EventArgs.Empty);
		}
		private void RaiseSelectedAreaChanged()
		{
			if (this.SelectedAreaChanged != null)
				this.SelectedAreaChanged(this, EventArgs.Empty);
		}
	}
}