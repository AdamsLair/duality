using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Resources;
using Duality.Plugins.Tilemaps;

using Duality.Editor;
using Duality.Editor.Plugins.Tilemaps.Properties;
using Duality.Editor.Plugins.Tilemaps.CamViewStates;

namespace Duality.Editor.Plugins.Tilemaps.UndoRedoActions
{
	public class EditTilemapAction : UndoRedoAction
	{
		private Tilemap               tilemap;
		private EditTilemapActionType type;
		private Point2                origin;
		private Grid<Tile>            oldTiles;
		private Grid<Tile>            newTiles;
		private Grid<bool>            editMask;

		public override string Name
		{
			get
			{
				switch (this.type)
				{
					default:
					case EditTilemapActionType.DrawTile:  return TilemapsRes.UndoRedo_EditTilemapDrawTiles;
					case EditTilemapActionType.FillRect:  return TilemapsRes.UndoRedo_EditTilemapFillTileRect;
					case EditTilemapActionType.FillOval:  return TilemapsRes.UndoRedo_EditTilemapFillTileOval;
					case EditTilemapActionType.FloodFill: return TilemapsRes.UndoRedo_EditTilemapFloodFillTiles;
				}
			}
		}
		public override bool IsVoid
		{
			get { return this.newTiles.Capacity == 0; }
		}

		public EditTilemapAction(Tilemap tilemap, EditTilemapActionType type, Point2 origin, Grid<Tile> newTiles, Grid<bool> editMask)
		{
			if (tilemap == null) throw new ArgumentNullException("tilemap");
			if (newTiles == null) throw new ArgumentNullException("newTiles");
			if (editMask == null) throw new ArgumentNullException("editMask");

			// Clamp the specified editing area to what is actually valid for this tilemap
			Point2 clampedOrigin = origin;
			Point2 clampedSize = new Point2(newTiles.Width, newTiles.Height);
			clampedSize.X += Math.Min(clampedOrigin.X, 0);
			clampedSize.Y += Math.Min(clampedOrigin.Y, 0);
			clampedOrigin.X = MathF.Clamp(clampedOrigin.X, 0, tilemap.Size.X);
			clampedOrigin.Y = MathF.Clamp(clampedOrigin.Y, 0, tilemap.Size.Y);
			clampedSize.X = MathF.Clamp(clampedSize.X, 0, tilemap.Size.X - clampedOrigin.X);
			clampedSize.Y = MathF.Clamp(clampedSize.Y, 0, tilemap.Size.Y - clampedOrigin.Y);

			this.tilemap = tilemap;
			this.type = type;
			this.origin = clampedOrigin;

			// Copy edited tiles to the operation's tilemap-clamped space
			if (clampedSize.X == newTiles.Width && clampedSize.Y == newTiles.Height)
			{
				this.newTiles = new Grid<Tile>(newTiles);
				this.editMask = new Grid<bool>(editMask);
			}
			else
			{
				this.newTiles = new Grid<Tile>(clampedSize.X, clampedSize.Y);
				this.editMask = new Grid<bool>(clampedSize.X, clampedSize.Y);
				newTiles.CopyTo(this.newTiles, Math.Min(origin.X, 0), Math.Min(origin.Y, 0));
				editMask.CopyTo(this.editMask, Math.Min(origin.X, 0), Math.Min(origin.Y, 0));
			}
		}

		public override void Do()
		{
			Grid<Tile> tiles = this.tilemap.BeginUpdateTiles();
			{
				// Create a backup for Undo support
				if (this.oldTiles == null)
				{
					this.oldTiles = new Grid<Tile>(this.newTiles.Width, this.newTiles.Height);
					tiles.CopyTo(this.oldTiles, 0, 0, this.oldTiles.Width, this.oldTiles.Height, this.origin.X, this.origin.Y);
				}

				// Overwrite the edited tiles
				MaskedCopyGrid(this.newTiles, tiles, this.editMask, this.origin.X, this.origin.Y);

				// Update the connectivity state of the edited tiles. Note that surrounding tiles
				// will be updated too!
				Tileset tileset = this.tilemap.Tileset.Res;
				if (tileset != null)
				{
					Tile.UpdateAutoTileCon(
						tiles, 
						MathF.Max(this.origin.X - 1, 0), 
						MathF.Max(this.origin.Y - 1, 0), 
						MathF.Min(this.newTiles.Width + 2, tiles.Width - MathF.Max(this.origin.X - 1, 0)), 
						MathF.Min(this.newTiles.Height + 2, tiles.Height - MathF.Max(this.origin.Y - 1, 0)), 
						tileset);
				}
			}
			this.tilemap.EndUpdateTiles(this.origin.X, this.origin.Y, this.newTiles.Width, this.newTiles.Height);

			this.OnNotifyPropertyChanged();
		}
		public override void Undo()
		{
			Grid<Tile> tiles = this.tilemap.BeginUpdateTiles();
			{
				// Overwrite the (previously edited) tiles with the backup we made before
				MaskedCopyGrid(this.oldTiles, tiles, this.editMask, this.origin.X, this.origin.Y);
			}
			this.tilemap.EndUpdateTiles(this.origin.X, this.origin.Y, this.oldTiles.Width, this.oldTiles.Height);

			this.OnNotifyPropertyChanged();
		}

		public override bool CanAppend(UndoRedoAction action)
		{
			if (this.type != EditTilemapActionType.DrawTile) return false;

			EditTilemapAction editAction = action as EditTilemapAction;
			if (editAction == null) return false;
			if (editAction.tilemap != this.tilemap) return false;
			if (editAction.type != this.type) return false;

			return true;
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			EditTilemapAction editAction = action as EditTilemapAction;

			// Perform the newly appended action individually
			if (performAction)
			{
				editAction.Do();
			}
			
			// Determine working data for merging the two tile grids
			Point2 originDiff = new Point2(
				editAction.origin.X - this.origin.X,
				editAction.origin.Y - this.origin.Y);
			Point2 minPos = new Point2(
				Math.Min(this.origin.X, editAction.origin.X),
				Math.Min(this.origin.Y, editAction.origin.Y));
			Point2 maxPos = new Point2(
				Math.Max(this.origin.X + this.newTiles.Width, editAction.origin.X + editAction.newTiles.Width),
				Math.Max(this.origin.Y + this.newTiles.Height, editAction.origin.Y + editAction.newTiles.Height));
			Point2 newSize = new Point2(
				maxPos.X - minPos.X,
				maxPos.Y - minPos.Y);
			Point2 growOffset = new Point2(
				Math.Min(0, originDiff.X), 
				Math.Min(0, originDiff.Y));
			Point2 drawOffset = new Point2(
				Math.Max(0, originDiff.X), 
				Math.Max(0, originDiff.Y));
			
			// Make sure to extend the tile data storage of this action, so the appended action can fit in
			this.newTiles.AssumeRect(growOffset.X, growOffset.Y, newSize.X, newSize.Y);
			this.editMask.AssumeRect(growOffset.X, growOffset.Y, newSize.X, newSize.Y);
			this.oldTiles.AssumeRect(growOffset.X, growOffset.Y, newSize.X, newSize.Y);

			// Move the operation origin accordingly
			this.origin.X += growOffset.X;
			this.origin.Y += growOffset.Y;

			// Determine the tiles that are edited in the appended operation, but weren't before
			Grid<bool> newlyEditedMask = new Grid<bool>(editAction.editMask);
			{
				Point2 bounds = new Point2(
					Math.Min(newlyEditedMask.Width, this.editMask.Width - drawOffset.X),
					Math.Min(newlyEditedMask.Height, this.editMask.Height - drawOffset.Y));

				for (int y = 0; y < bounds.Y; y++)
				{
					for (int x = 0; x < bounds.X; x++)
					{
						bool existingMask = this.editMask[x + drawOffset.X, y + drawOffset.Y];
						bool appendedMask = editAction.editMask[x, y];

						newlyEditedMask[x, y] = !existingMask && appendedMask;
					}
				}
			}

			// Apply new tile data from the appended action to this one
			MaskedCopyGrid(editAction.newTiles, this.newTiles, editAction.editMask, drawOffset.X, drawOffset.Y);
			MaskedCopyGrid(editAction.oldTiles, this.oldTiles, newlyEditedMask,     drawOffset.X, drawOffset.Y);
			MaskedCopyGrid(editAction.editMask, this.editMask, editAction.editMask, drawOffset.X, drawOffset.Y);
		}

		private void OnNotifyPropertyChanged()
		{
			DualityEditorApp.NotifyObjPropChanged(
				this,
				new ObjectSelection(this.tilemap),
				TilemapsReflectionInfo.Property_Tilemap_Tiles);
		}

		private static void MaskedCopyGrid<T>(Grid<T> source, Grid<T> target, Grid<bool> sourceMask, int destX = 0, int destY = 0, int width = -1, int height = -1, int srcX = 0, int srcY = 0)
		{
			if (width == -1) width = source.Width;
			if (height == -1) height = source.Height;

			int beginX = MathF.Max(0, -destX, -srcX);
			int beginY = MathF.Max(0, -destY, -srcY);
			int endX = MathF.Min(width, source.Width, target.Width - destX, source.Width - srcX);
			int endY = MathF.Min(height, source.Height, target.Height - destY, source.Height - srcY);
			if (endX - beginX < 1) return;
			if (endY - beginY < 1) return;

			T[] targetData = target.RawData;
			T[] sourceData = source.RawData;
			bool[] maskData = sourceMask.RawData;
			for (int i = beginX; i < endX; i++)
			{
				for (int j = beginY; j < endY; j++)
				{
					int sourceN = srcX + i + source.Width * (srcY + j);
					if (!maskData[sourceN]) continue;

					int targetN = destX + i + target.Width * (destY + j);
					targetData[targetN] = sourceData[sourceN];
				}
			}
		}
	}
}
