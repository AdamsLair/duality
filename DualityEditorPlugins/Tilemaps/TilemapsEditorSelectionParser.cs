using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Resources;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps
{
	public static class TilemapsEditorSelectionParser
	{
		public static IEnumerable<Tilemap> QuerySelectedTilemaps()
		{
			return
				DualityEditorApp.Selection.Components.OfType<Tilemap>()
				.Concat(DualityEditorApp.Selection.GameObjects.GetComponents<Tilemap>())
				.Concat(DualityEditorApp.Selection.Components.OfType<TilemapRenderer>().Select(r => r.ExternalTilemap))
				.Concat(DualityEditorApp.Selection.GameObjects.GetComponents<TilemapRenderer>().Select(r => r.ExternalTilemap))
				.NotNull()
				.Distinct();
		}
		public static Tilemap QuerySelectedTilemap()
		{
			return
				DualityEditorApp.Selection.Components.OfType<Tilemap>().FirstOrDefault() ?? 
				DualityEditorApp.Selection.GameObjects.GetComponents<Tilemap>().FirstOrDefault() ??
				DualityEditorApp.Selection.Components.OfType<TilemapRenderer>().Select(r => r.ExternalTilemap).FirstOrDefault() ?? 
				DualityEditorApp.Selection.GameObjects.GetComponents<TilemapRenderer>().Select(r => r.ExternalTilemap).FirstOrDefault();
		}
		public static ContentRef<Tileset> QuerySelectedTileset()
		{
			Tileset tileset = DualityEditorApp.Selection.Resources.OfType<Tileset>().FirstOrDefault();
			if (tileset != null) return tileset;
			
			Tilemap tilemap = QuerySelectedTilemap();
			if (tilemap != null) return tilemap.Tileset.Res;

			return null;
		}

		public static IEnumerable<ContentRef<Tileset>> GetTilesetsInScene(Scene scene)
		{
			HashSet<ContentRef<Tileset>> tilesets = new HashSet<ContentRef<Tileset>>();
			foreach (Tilemap tilemap in scene.FindComponents<Tilemap>())
			{
				if (tilemap.Tileset != null)
					tilesets.Add(tilemap.Tileset);
			}

			return tilesets;
		}
		public static bool SceneContainsTileset(Scene scene, ContentRef<Tileset> tileset)
		{
			foreach (Tilemap tilemap in scene.FindComponents<Tilemap>())
			{
				if (tilemap.Tileset == tileset)
					return true;
			}

			return false;
		}
	}
}
