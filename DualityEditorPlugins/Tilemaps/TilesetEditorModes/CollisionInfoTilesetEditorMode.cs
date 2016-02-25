using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Aga.Controls.Tree;

using Duality.Editor.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	/// <summary>
	/// Allows to edit the physical shape of each tile which can be used for collision detection and physics.
	/// </summary>
	public class CollisionInfoTilesetEditorMode : TilesetEditorMode
	{
		private TreeModel treeModel = new TreeModel();

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
	}
}
