using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality.Editor.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	public class CollisionInfoTilesetEditorMode : TilesetEditorMode
	{
		public override string Id
		{
			get { return "CollisionInfoTilesetEditorMode"; }
		}
		public override string Name
		{
			get { return TilemapsRes.TilesetEditorMode_CollisionInfo_Name; }
		}
		public override string Description
		{
			get { return TilemapsRes.TilesetEditorMode_CollisionInfo_Desc; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTilesetCollisionInfo; }
		}
		public override int SortOrder
		{
			get { return -90; }
		}
	}
}
