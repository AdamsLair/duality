using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps
{
	public static class TilemapsEditorSelectionParser
	{
		public static Tilemap QuerySelectedTilemap()
		{
			return
				DualityEditorApp.Selection.Components.OfType<Tilemap>().FirstOrDefault() ?? 
				DualityEditorApp.Selection.GameObjects.GetComponents<Tilemap>().FirstOrDefault() ??
				DualityEditorApp.Selection.Components.OfType<TilemapRenderer>().Select(r => r.ExternalTilemap).FirstOrDefault() ?? 
				DualityEditorApp.Selection.GameObjects.GetComponents<TilemapRenderer>().Select(r => r.ExternalTilemap).FirstOrDefault();
		}
		public static Tileset QuerySelectedTileset()
		{
			Tileset tileset = DualityEditorApp.Selection.Resources.OfType<Tileset>().FirstOrDefault();
			if (tileset != null) return tileset;
			
			Tilemap tilemap = QuerySelectedTilemap();
			if (tilemap != null) return tilemap.Tileset.Res;

			return null;
		}
	}
}
