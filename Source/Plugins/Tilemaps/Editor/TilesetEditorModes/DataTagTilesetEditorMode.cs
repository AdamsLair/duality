using System.Drawing;
using System.Linq;
using Aga.Controls.Tree;
using Duality.Editor.Plugins.Tilemaps.Properties;
using Duality.Editor.Plugins.Tilemaps.UndoRedoActions;
using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	public class DataTagTilesetEditorMode : TilesetEditorMode
	{
		private class VisualLayerNode : TilesetEditorLayerNode
		{
			private TilesetDataTagInput layer = null;

			public override string Title
			{
				get { return this.layer.Name; }
			}
			public override string Description
			{
				get { return this.layer.Id; }
			}
			public TilesetRenderInput VisualLayer
			{
				get { return this.layer; }
			}

			public VisualLayerNode(TilesetRenderInput layer) : base()
			{
				this.layer = layer;
				this.Image = Properties.TilemapsResCache.IconTilesetSingleVisualLayer;
			}

			public void Invalidate()
			{
				this.NotifyModel();
			}
		}


		public override string Name
		{
			get
			{
				return "Tags";
			}
		}

		public override string Id
		{
			get
			{
				return "DataTagTilesetEditorMode";
			}
		}

		public override LayerEditingCaps AllowLayerEditing
		{
			get { return LayerEditingCaps.All; }
		}

		public override Image Icon
		{
			get { return TilemapsResCache.IconTilesetCollisionInfo; }
		}

		public override ITreeModel LayerModel
		{
			get
			{
				return this.treeModel;
			}
		}

		private TreeModel treeModel = new TreeModel();

		protected TilesetDataTagInput SelectedDataLayer
		{
			get { return DualityEditorApp.Selection.Objects.OfType<TilesetDataTagInput>().FirstOrDefault(); }
		}

		public override void AddLayer()
		{
			base.AddLayer();

			//Generate a unique key
			int id = 1;
			string key = id.ToString();
			while (this.SelectedTileset.Res.DataTagConfig.ContainsKey(key))
			{
				id++;
				key = id.ToString();
			}

			TilesetDataTagInput layer = new TilesetDataTagInput()
			{
				Key = key
			};
			this.SelectedTileset.Res.DataTagConfig.Add(key, layer);
			VisualLayerNode modelNode = this.treeModel
			.Nodes
			.OfType<VisualLayerNode>()
			.FirstOrDefault(n => n.VisualLayer == newLayer);
			this.SelectLayer(modelNode);
		}

		public override void RemoveLayer()
		{
			base.RemoveLayer();

			Tileset tileset = this.SelectedTileset.Res;
			TilesetDataTagInput layer = this.SelectedDataLayer;
			if (tileset == null) return;
			if (layer == null) return;

			UndoRedoManager.Do(new RemoveTilesetConfigLayerAction<TilesetDataTagInput>(
				tileset,
				TilemapsReflectionInfo.Property_Tileset_RenderConfig,
				layer));
		}
	}
}
