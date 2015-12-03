using System;
using System.Reflection;
using System.Linq;

using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps
{
	internal static class TilemapsReflectionInfo
	{
		public static readonly PropertyInfo Property_Tilemap_Tiles;

		static TilemapsReflectionInfo()
		{
			Type tilemap = typeof(Tilemap);
			Property_Tilemap_Tiles = GetProperty(tilemap, "Tiles");
		}
		
		private static PropertyInfo GetProperty(Type type, string name)
		{
			return type.GetRuntimeProperties().FirstOrDefault(m => !m.IsStatic() && m.Name == name);
		}
	}
}
