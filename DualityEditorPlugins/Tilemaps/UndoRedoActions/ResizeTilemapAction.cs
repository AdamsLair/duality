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
	public class ResizeTilemapAction : UndoRedoAction
	{
		private Tilemap[]    tilemaps;
		private Grid<Tile>[] oldData;
		private Point2       newSize;
		private Alignment    origin;

		public override string Name
		{
			get { return TilemapsRes.UndoRedo_ResizeTilemap; }
		}
		public override bool IsVoid
		{
			get { return this.tilemaps == null || this.tilemaps.Length == 0; }
		}

		public ResizeTilemapAction(IEnumerable<Tilemap> tilemaps, Point2 newSize, Alignment origin)
		{
			this.tilemaps = tilemaps.ToArray();
			this.newSize = newSize;
			this.origin = origin;
		}

		public override void Do()
		{
			if (this.oldData == null)
			{
				this.oldData = new Grid<Tile>[this.tilemaps.Length];
				for (int i = 0; i < this.tilemaps.Length; i++)
				{
					this.oldData[i] = new Grid<Tile>(this.tilemaps[i].BeginUpdateTiles());
					this.tilemaps[i].EndUpdateTiles(0, 0, 0, 0);
				}
			}
			
			for (int i = 0; i < this.tilemaps.Length; i++)
			{
				// Determine the tile which should fill the new area
				Tile fillTile = new Tile
				{
					Index = TilemapsSetupUtility.GetDefaultTileIndex(
						this.tilemaps[i].Tileset.Res, 
						true)
				};

				// Determine the up to four regions that we will add
				int leftAdd = 0;
				int rightAdd = 0;
				int topAdd = 0;
				int bottomAdd = 0;
				{
					if (this.origin == Alignment.Right || 
						this.origin == Alignment.TopRight || 
						this.origin == Alignment.BottomRight)
						leftAdd = this.newSize.X - this.tilemaps[i].Size.X;
					else if (
						this.origin == Alignment.Center || 
						this.origin == Alignment.Top || 
						this.origin == Alignment.Bottom)
						leftAdd = (this.newSize.X - this.tilemaps[i].Size.X) / 2;

					if (this.origin == Alignment.Bottom || 
						this.origin == Alignment.BottomLeft || 
						this.origin == Alignment.BottomRight)
						topAdd = this.newSize.Y - this.tilemaps[i].Size.Y;
					else if (
						this.origin == Alignment.Center || 
						this.origin == Alignment.Left || 
						this.origin == Alignment.Right)
						topAdd = (this.newSize.Y - this.tilemaps[i].Size.Y) / 2;

					rightAdd = (this.newSize.X - this.tilemaps[i].Size.X) - leftAdd;
					bottomAdd = (this.newSize.Y - this.tilemaps[i].Size.Y) - topAdd;
				}

				// Resize the tilemap
				this.tilemaps[i].Resize(this.newSize.X, this.newSize.Y, this.origin);

				// If we have a non-default filling tile, use it
				if (!object.Equals(fillTile, default(Tile)))
				{
					Grid<Tile> data = this.tilemaps[i].BeginUpdateTiles();
					if (topAdd    > 0) data.Fill(fillTile, 0, 0, data.Width, topAdd);
					if (leftAdd   > 0) data.Fill(fillTile, 0, 0, leftAdd, data.Height);
					if (bottomAdd > 0) data.Fill(fillTile, 0, data.Height - bottomAdd, data.Width, bottomAdd);
					if (rightAdd  > 0) data.Fill(fillTile, data.Width - rightAdd, 0, rightAdd, data.Height);
					this.tilemaps[i].EndUpdateTiles(0, 0, 0, 0);
				}
			}

			this.OnNotifyPropertyChanged();
		}
		public override void Undo()
		{
			for (int i = 0; i < this.tilemaps.Length; i++)
			{ 
				this.tilemaps[i].Resize(this.oldData[i].Width, this.oldData[i].Height, this.origin);
				this.oldData[i].CopyTo(this.tilemaps[i].BeginUpdateTiles());
				this.tilemaps[i].EndUpdateTiles(0, 0, 0, 0);
			}

			this.OnNotifyPropertyChanged();
		}

		private void OnNotifyPropertyChanged()
		{
			DualityEditorApp.NotifyObjPropChanged(
				this,
				new ObjectSelection(this.tilemaps),
				TilemapsReflectionInfo.Property_Tilemap_Tiles);
		}
	}
}
