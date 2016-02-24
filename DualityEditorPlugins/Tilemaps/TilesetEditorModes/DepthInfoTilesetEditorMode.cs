using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality.Editor.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	public class DepthInfoTilesetEditorMode : TilesetEditorMode
	{
		public override string Id
		{
			get { return "DepthInfoTilesetEditorMode"; }
		}
		public override string Name
		{
			get { return TilemapsRes.TilesetEditorMode_DepthInfo_Name; }
		}
		public override string Description
		{
			get { return TilemapsRes.TilesetEditorMode_DepthInfo_Desc; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTilesetDepthInfo; }
		}
		public override int SortOrder
		{
			get { return -80; }
		}
	}
}
