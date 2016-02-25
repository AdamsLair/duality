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
	/// Allows to edit the depth information of each tile, which can be used for Z-Offset generation and Z sorting during rendering.
	/// </summary>
	public class DepthInfoTilesetEditorMode : TilesetEditorMode
	{
		private TreeModel treeModel = new TreeModel();

		public override string Id
		{
			get { return "DepthInfoTilesetEditorMode"; }
		}
		public override string Name
		{
			get { return TilemapsRes.TilesetEditorMode_DepthInfo_Name; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTilesetDepthInfo; }
		}
		public override int SortOrder
		{
			get { return -80; }
		}
		public override ITreeModel LayerModel
		{
			get { return this.treeModel; }
		}
	}
}
