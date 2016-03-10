using System;
using System.Reflection;
using System.Linq;

using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps
{
	internal static class TilemapsReflectionInfo
	{
		public static readonly PropertyInfo Property_Tilemap_Tiles;
		public static readonly PropertyInfo Property_Tileset_RenderConfig;

		static TilemapsReflectionInfo()
		{
			Type tilemap = typeof(Tilemap);
			Property_Tilemap_Tiles = GetProperty(tilemap, "Tiles");

			Type tileset = typeof(Tileset);
			Property_Tileset_RenderConfig = GetProperty(tileset, "RenderConfig");
		}
		
		private static PropertyInfo GetProperty(Type type, string name)
		{
			return type.GetRuntimeProperties().First(m => !m.IsStatic() && m.Name == name);
		}
	}
}
