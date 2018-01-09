using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Duality.Editor.Plugins.Tilemaps.Properties;
using Duality.Editor.Plugins.Tilemaps.UndoRedoActions;
using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	public class DataTagTilesetEditorMode : TilesetEditorMode
	{
		private class DataLayerNode : TilesetEditorLayerNode
		{
			private TilesetEditor editor = null;
			private TilesetDataTagInput layer = null;

			internal void Init(TilesetEditor editor)
			{
				this.editor = editor;
			}

			protected TilesetView TilesetView
			{
				get { return this.editor.TilesetView; }
			}

			public override string Title
			{
				get { return this.layer.Key; }
			}
			public override string Description
			{
				get { return this.layer.Key; }
			}
			public TilesetDataTagInput DataTagLayer
			{
				get { return this.layer; }
			}

			public DataLayerNode(TilesetDataTagInput layer) : base()
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

		protected TilesetDataTagInput SelectedDataLayer;

		protected override void OnEnter()
		{
			this.UpdateTreeModel();
			this.TilesetView.MouseClick += this.TilesetView_MouseDown;
		}

		protected override void OnTilesetSelectionChanged(TilesetSelectionChangedEventArgs args)
		{
			this.UpdateTreeModel();
		}

		protected override void OnLeave()
		{
			base.OnLeave();
			this.TilesetView.MouseClick -= this.TilesetView_MouseDown;
		}

		private void TilesetView_MouseDown(object sender, MouseEventArgs e)
		{
			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;

			TilesetDataTagInput selectedDataLayer = this.SelectedDataLayer;
			if (selectedDataLayer == null) return;

			int tileIndex = this.TilesetView.HoveredTileIndex;
			if (tileIndex < 0 || tileIndex > tileset.TileCount) return;

			DualityEditorApp.Select(this, new ObjectSelection(new object[] { selectedDataLayer.TileData[tileIndex] }));
		}

		public override void AddLayer()
		{
			base.AddLayer();

			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;

			//Generate a unique key
			int id = 1;
			string key = id.ToString();
			while (this.SelectedTileset.Res.DataTagConfig.Any(x => x.Key == key))
			{
				id++;
				key = id.ToString();
			}

			TilesetDataTagInput layer = new TilesetDataTagInput()
			{
				Key = key
			};
			layer.TileData.Count = tileset.TileCount;
			this.SelectedTileset.Res.DataTagConfig.Add(layer);

			UndoRedoManager.Do(new AddTilesetConfigLayerAction<TilesetDataTagInput>(
								tileset,
								TilemapsReflectionInfo.Property_TilesetDataTagInput,
								layer));

			DataLayerNode modelNode = this.treeModel
			.Nodes
			.OfType<DataLayerNode>()
			.FirstOrDefault(n => n.DataTagLayer == layer);
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
				TilemapsReflectionInfo.Property_TilesetDataTagInput,
				layer));
		}

		protected override void OnTilesetModified(ObjectPropertyChangedEventArgs args)
		{
			Tileset tileset = this.SelectedTileset.Res;

			// If a visual layer was modified, emit an editor-wide change event for
			// the Tileset as well, so the editor knows it will need to save this Resource.
			if (tileset != null && args.HasAnyObject(tileset.DataTagConfig))
			{
				DualityEditorApp.NotifyObjPropChanged(
					this,
					new ObjectSelection(tileset),
					TilemapsReflectionInfo.Property_TilesetDataTagInput);
			}

			this.UpdateTreeModel();
		}

		protected override void OnLayerSelectionChanged(LayerSelectionChangedEventArgs args)
		{
			base.OnLayerSelectionChanged(args);
			Tileset tileset = this.SelectedTileset.Res;
			DataLayerNode selectedNode = args.SelectedNodeTag as DataLayerNode;
			// Update global editor selection, so an Object Inspector can pick up the AutoTile for editing
			if (selectedNode != null)
			{
				this.SelectedDataLayer = selectedNode.DataTagLayer;
				DualityEditorApp.Select(this, new ObjectSelection(new object[] { selectedNode.DataTagLayer }));
			}
			else
				DualityEditorApp.Deselect(this, obj => obj is DataLayerNode);

			this.TilesetView.Invalidate();
		}

		private void UpdateTreeModel()
		{
			Tileset tileset = this.SelectedTileset.Res;

			// If the tileset is unavailable, or none is selected, there are no nodes
			if (tileset == null)
			{
				this.treeModel.Nodes.Clear();
				return;
			}

			// Remove nodes that no longer have an equivalent in the Tileset
			foreach (DataLayerNode node in this.treeModel.Nodes.ToArray())
			{
				if (!tileset.DataTagConfig.Any(x => x.Key == node.DataTagLayer.Key))
				{
					this.treeModel.Nodes.Remove(node);
				}
			}

			// Notify the model of changes for the existing nodes to account for name and id changes.
			// This is only okay because we have so few notes in total. Don't do this for actual data.
			foreach (DataLayerNode node in this.treeModel.Nodes)
			{
				node.Invalidate();
			}

			// Add nodes that don't have a corresponding tree model node yet
			foreach (TilesetDataTagInput layer in tileset.DataTagConfig)
			{
				if (!this.treeModel.Nodes.Any(node => (node as DataLayerNode).DataTagLayer == layer))
				{
					this.treeModel.Nodes.Add(new DataLayerNode(layer));
				}
			}
		}
	}
}
