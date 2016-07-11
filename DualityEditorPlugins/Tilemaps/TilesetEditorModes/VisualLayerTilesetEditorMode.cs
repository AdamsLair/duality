using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Drawing;

using Aga.Controls.Tree;

using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;
using Duality.Editor.Plugins.Tilemaps.UndoRedoActions;

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

			public void Invalidate()
			{
				this.NotifyModel();
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
		public override LayerEditingCaps AllowLayerEditing
		{
			get { return LayerEditingCaps.All; }
		}
		protected TilesetRenderInput SelectedVisualLayer
		{
			get { return DualityEditorApp.Selection.Objects.OfType<TilesetRenderInput>().FirstOrDefault(); }
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

			// Notify the model of changes for the existing nodes to account for name and id changes.
			// This is only okay because we have so few notes in total. Don't do this for actual data.
			foreach (VisualLayerNode node in this.treeModel.Nodes)
			{
				node.Invalidate();
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

		/// <inheritdoc />
		public override void AddLayer()
		{
			base.AddLayer();
			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;

			// Determine which texture IDs are already present, so we can
			// derive which one we'll pick as a default for creating a new one.
			bool hasMainTex = false;
			int highestCustomTex = -1;
			for (int i = 0; i < tileset.RenderConfig.Count; i++)
			{
				if (tileset.RenderConfig[i].Id == TilesetRenderInput.MainTexId)
				{
					hasMainTex = true;
				}
				else if (tileset.RenderConfig[i].Id.StartsWith(TilesetRenderInput.CustomTexId))
				{
					string customTexIndexString = tileset.RenderConfig[i].Id.Substring(
						TilesetRenderInput.CustomTexId.Length, 
						tileset.RenderConfig[i].Id.Length - TilesetRenderInput.CustomTexId.Length);

					int customTexIndex;
					if (!int.TryParse(customTexIndexString, out customTexIndex))
						customTexIndex = 0;

					highestCustomTex = Math.Max(highestCustomTex, customTexIndex);
				}
			}

			// Decide upon an id for our new layer
			string layerId;
			string layerName;
			if (!hasMainTex)
			{
				layerId = TilesetRenderInput.MainTexId;
				layerName = TilesetRenderInput.MainTexName;
			}
			else
			{
				layerId = TilesetRenderInput.CustomTexId + (highestCustomTex + 1).ToString();
				layerName = TilesetRenderInput.CustomTexName;
			}

			// Create a new layer using an UndoRedo action
			TilesetRenderInput newLayer = new TilesetRenderInput
			{
				Id = layerId,
				Name = layerName
			};
			UndoRedoManager.Do(new AddTilesetVisualLayerAction(
				tileset, 
				newLayer));

			// Select the newly created visual layer
			VisualLayerNode modelNode = this.treeModel
				.Nodes
				.OfType<VisualLayerNode>()
				.FirstOrDefault(n => n.VisualLayer == newLayer);
			this.SelectLayer(modelNode);
		}
		/// <inheritdoc />
		public override void RemoveLayer()
		{
			base.RemoveLayer();

			Tileset tileset = this.SelectedTileset.Res;
			TilesetRenderInput layer = this.SelectedVisualLayer;
			if (tileset == null) return;
			if (layer == null) return;

			UndoRedoManager.Do(new RemoveTilesetVisualLayerAction(
				tileset, 
				layer));
		}
		
		protected override void OnEnter()
		{
			this.UpdateTreeModel();
		}
		protected override void OnTilesetSelectionChanged(TilesetSelectionChangedEventArgs args)
		{
			this.UpdateTreeModel();
		}
		protected override void OnTilesetModified(ObjectPropertyChangedEventArgs args)
		{
			Tileset tileset = this.SelectedTileset.Res;

			// If a visual layer was modified, emit an editor-wide change event for
			// the Tileset as well, so the editor knows it will need to save this Resource.
			if (tileset != null && args.HasAnyObject(tileset.RenderConfig))
			{
				DualityEditorApp.NotifyObjPropChanged(
					this, 
					new ObjectSelection(tileset),
					TilemapsReflectionInfo.Property_Tileset_RenderConfig);
			}

			this.UpdateTreeModel();
		}
		protected override void OnLayerSelectionChanged(LayerSelectionChangedEventArgs args)
		{
			base.OnLayerSelectionChanged(args);
			Tileset tileset = this.SelectedTileset.Res;
			VisualLayerNode selectedNode = args.SelectedNodeTag as VisualLayerNode;

			// Update global editor selection, so an Object Inspector can pick up the layer for editing
			if (selectedNode != null)
				DualityEditorApp.Select(this, new ObjectSelection(new object[] { selectedNode.VisualLayer }));
			else
				DualityEditorApp.Deselect(this, obj => obj is TilesetRenderInput);

			// Update the TilesetView to show the appropriate layer
			if (tileset != null)
			{
				int layerIndex = (selectedNode != null) ? 
					tileset.RenderConfig.IndexOf(selectedNode.VisualLayer) : 
					-1;

				if (layerIndex == -1) 
					layerIndex = 0;

				this.TilesetView.DisplayedConfigIndex = layerIndex;
			}
		}
		protected override void OnApplyRevert()
		{
			base.OnApplyRevert();

			// Deselect whichever layer node we had selected, because
			// Apply / Revert operations affect the Tileset as a whole
			// in ways we can't safely predict editor-wise. It may be
			// best to not make breakable assumptions here.
			this.SelectLayer(null);
		}
	}
}
