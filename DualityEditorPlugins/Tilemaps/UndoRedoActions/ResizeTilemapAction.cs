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
				this.tilemaps[i].Resize(this.newSize.X, this.newSize.Y, this.origin);

			this.OnNotifyPropertyChanged();
		}
		public override void Undo()
		{
			for (int i = 0; i < this.tilemaps.Length; i++)
			{ 
				this.tilemaps[i].Resize(this.oldData[i].Width, this.oldData[i].Height, this.origin);
				this.oldData[i].CopyTo(this.tilemaps[i].BeginUpdateTiles());
				this.tilemaps[i].EndUpdateTiles();
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
