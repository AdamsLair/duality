using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality.Editor.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	public class VisualLayerTilesetEditorMode : TilesetEditorMode
	{
		public override string Id
		{
			get { return "VisualLayerTilesetEditorMode"; }
		}
		public override string Name
		{
			get { return TilemapsRes.TilesetEditorMode_VisualLayer_Name; }
		}
		public override string Description
		{
			get { return TilemapsRes.TilesetEditorMode_VisualLayer_Desc; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTilesetVisualLayers; }
		}
		public override int SortOrder
		{
			get { return -100; }
		}
	}
}
