using System;
using System.Reflection;
using System.Linq;

using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps
{
	/// <summary>
	/// Provides commonly used utility methods for setting up <see cref="Tilemap"/> layers
	/// to be edited by the user. This usually involves and initial resize and default tile
	/// assignments.
	/// </summary>
	public static class TilemapsSetupUtility
	{
		public static readonly Point2 DefaultTilemapSize = new Point2(32, 20);

		/// <summary>
		/// Determines the default tile index of the specified <see cref="Tileset"/>
		/// depending on the role of the <see cref="Tilemap"/> in which it is used.
		/// </summary>
		/// <param name="tileset"></param>
		/// <param name="isUpperLayer"></param>
		/// <returns></returns>
		public static int GetDefaultTileIndex(Tileset tileset, bool isUpperLayer)
		{
			if (tileset == null) return 0;

			for (int i = 0; i < tileset.TileData.Count; i++)
			{
				// Use solid tiles for the base layer, but transparent tiles
				// for all upper layers.
				if (tileset.TileData[i].IsVisuallyEmpty == isUpperLayer)
					return i;
			}

			return 0;
		}

		/// <summary>
		/// Prepares the specified <see cref="Tilemap"/> for user editing using the default size.
		/// </summary>
		/// <param name="tilemap"></param>
		/// <param name="tilesetRef"></param>
		public static void SetupTilemap(Tilemap tilemap, ContentRef<Tileset> tilesetRef)
		{
			SetupTilemap(tilemap, tilesetRef, DefaultTilemapSize.X, DefaultTilemapSize.Y, false);
		}
		/// <summary>
		/// Prepares the specified <see cref="Tilemap"/> for user editing using the specified size.
		/// </summary>
		/// <param name="tilemap"></param>
		/// <param name="tilesetRef"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="isUpperLayer"></param>
		public static void SetupTilemap(Tilemap tilemap, ContentRef<Tileset> tilesetRef, int width, int height, bool isUpperLayer)
		{
			Tileset tileset = tilesetRef.Res;

			// Determine the first tile index that matches the layer type.
			int fillTileIndex = GetDefaultTileIndex(tileset, isUpperLayer);

			// Resize the Tilemap and fill it with the first visually non-empty tile.
			tilemap.Tileset = tileset;
			tilemap.Resize(width, height);
			tilemap.BeginUpdateTiles().Fill(
				new Tile { Index = fillTileIndex }, 
				0, 
				0, 
				tilemap.Size.X, 
				tilemap.Size.Y);
			tilemap.EndUpdateTiles();
		}
	}
}
