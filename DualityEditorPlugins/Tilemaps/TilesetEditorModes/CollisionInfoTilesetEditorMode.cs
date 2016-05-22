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
	/// Allows to edit the physical shape of each tile which can be used for collision detection and physics.
	/// </summary>
	public class CollisionInfoTilesetEditorMode : TilesetEditorMode
	{
		private class CollisionInfoLayerNode : TilesetEditorLayerNode
		{
			private int layerIndex = 0;
			private string name = null;
			private string desc = null;

			public override string Title
			{
				get { return this.name; }
			}
			public override string Description
			{
				get { return this.desc; }
			}

			public CollisionInfoLayerNode(int layerIndex, string name, string description, Image image) : base()
			{
				this.layerIndex = layerIndex;
				this.name = name;
				this.desc = description;
				this.Image = image;
			}
		}


		private TreeModel treeModel = new TreeModel();
		private CollisionInfoLayerNode[] layerNodes = null;


		public override string Id
		{
			get { return "CollisionInfoTilesetEditorMode"; }
		}
		public override string Name
		{
			get { return TilemapsRes.TilesetEditorMode_CollisionInfo_Name; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTilesetCollisionInfo; }
		}
		public override int SortOrder
		{
			get { return -90; }
		}
		public override ITreeModel LayerModel
		{
			get { return this.treeModel; }
		}


		public CollisionInfoTilesetEditorMode()
		{
			this.layerNodes = new CollisionInfoLayerNode[TileCollisionShapes.LayerCount];
			for (int layerIndex = 0; layerIndex < this.layerNodes.Length; layerIndex++)
			{
				string nameBase = (layerIndex == 0) ? 
					TilemapsRes.TilesetEditorCollisionMainLayer_Name : 
					TilemapsRes.TilesetEditorCollisionAuxLayer_Name;
				this.layerNodes[layerIndex] = new CollisionInfoLayerNode(
					layerIndex,
					string.Format(nameBase, layerIndex),
					string.Format(TilemapsRes.TilesetEditorCollisionLayer_Desc, layerIndex),
					TilemapsResCache.IconTilesetCollisionLayer);
				this.treeModel.Nodes.Add(this.layerNodes[layerIndex]);
			}
		}
		
		protected override void OnEnter()
		{
			base.OnEnter();
			this.SelectLayer(this.layerNodes[0]);
		}
		protected override void OnLayerSelectionChanged(LayerSelectionChangedEventArgs args)
		{
			base.OnLayerSelectionChanged(args);

			// There always needs to be a selected collision layer.
			if (args.SelectedNodeTag == null)
				this.SelectLayer(this.layerNodes[0]);

			this.TilesetView.Invalidate();
		}
	}
}
