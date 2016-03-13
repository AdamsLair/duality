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
	public class RemoveTilesetVisualLayerAction : UndoRedoAction
	{
		private Tileset            tileset;
		private TilesetRenderInput layer;

		public override string Name
		{
			get { return TilemapsRes.UndoRedo_TilesetRemoveVisualLayer; }
		}
		public override bool IsVoid
		{
			get { return this.tileset == null || this.layer == null; }
		}

		public RemoveTilesetVisualLayerAction(Tileset tileset, TilesetRenderInput layer)
		{
			this.tileset = tileset;
			this.layer = layer;
		}

		public override void Do()
		{
			this.tileset.RenderConfig.Remove(this.layer);
			this.OnNotifyPropertyChanged();
		}
		public override void Undo()
		{
			this.tileset.RenderConfig.Add(this.layer);
			this.OnNotifyPropertyChanged();
		}

		private void OnNotifyPropertyChanged()
		{
			DualityEditorApp.NotifyObjPropChanged(
				this,
				new ObjectSelection(this.tileset),
				TilemapsReflectionInfo.Property_Tileset_RenderConfig);
		}
	}
}
