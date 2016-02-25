using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Aga.Controls.Tree;

using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	/// <summary>
	/// Allows to edit a Tileset's visual layers, which define the Texture- and Material properties that will affect how it is rendered.
	/// </summary>
	public class VisualLayerTilesetEditorMode : TilesetEditorMode
	{
		private class VisualLayerNode : TilesetEditorLayerNode
		{
			private TilesetRenderInput layer = null;

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
		}


		private	TreeModel treeModel = new TreeModel();


		public override string Id
		{
			get { return "VisualLayerTilesetEditorMode"; }
		}
		public override string Name
		{
			get { return TilemapsRes.TilesetEditorMode_VisualLayer_Name; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTilesetVisualLayers; }
		}
		public override int SortOrder
		{
			get { return -100; }
		}
		public override ITreeModel LayerModel
		{
			get { return this.treeModel; }
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
			foreach (VisualLayerNode node in this.treeModel.Nodes.ToArray())
			{
				if (!tileset.RenderConfig.Contains(node.VisualLayer))
				{ 
					this.treeModel.Nodes.Remove(node);
				}
			}

			// Add nodes that don't have a corresponding tree model node yet
			foreach (TilesetRenderInput layer in tileset.RenderConfig)
			{
				if (!this.treeModel.Nodes.Any(node => (node as VisualLayerNode).VisualLayer == layer))
				{
					this.treeModel.Nodes.Add(new VisualLayerNode(layer));
				}
			}
		}
		
		protected override void OnEnter()
		{
			this.UpdateTreeModel();
		}
		protected override void OnTilesetSelectionChanged()
		{
			this.UpdateTreeModel();
		}
		protected override void OnTilesetModified(ObjectPropertyChangedEventArgs args)
		{
			this.UpdateTreeModel();
		}
	}
}
