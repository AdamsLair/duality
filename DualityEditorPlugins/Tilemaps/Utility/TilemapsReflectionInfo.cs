using System;
using System.Reflection;
using System.Linq;

using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps
{
	internal static class TilemapsReflectionInfo
	{
		public static readonly PropertyInfo Property_Tilemap_Tiles;
		public static readonly PropertyInfo Property_Tilemap_Tileset;
		public static readonly PropertyInfo Property_Tileset_RenderConfig;
		public static readonly PropertyInfo Property_Tileset_TileInput;

		static TilemapsReflectionInfo()
		{
			Type tilemap = typeof(Tilemap);
			Property_Tilemap_Tiles = GetProperty(tilemap, "Tiles");
			Property_Tilemap_Tileset = GetProperty(tilemap, "Tileset");

			Type tileset = typeof(Tileset);
			Property_Tileset_RenderConfig = GetProperty(tileset, "RenderConfig");
			Property_Tileset_TileInput = GetProperty(tileset, "TileInput");
		}
		
		private static PropertyInfo GetProperty(Type type, string name)
		{
			return type.GetRuntimeProperties().First(m => !m.IsStatic() && m.Name == name);
		}
	}
}
