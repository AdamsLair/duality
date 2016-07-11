using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Components;
using Duality.Plugins.Tilemaps;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;


namespace Duality.Editor.Plugins.Tilemaps.DataConverters
{
	public class ComponentFromTileset : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(Tilemap); }
		}
		public override int Priority
		{
			// Use lower-than-default priority to allow more general converters 
			// that are based on Pixmaps as well to perform first.
			get { return base.Priority - 1; }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateObj) && 
				convert.CanPerform<Tileset>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			List<object> results = new List<object>();
			List<Tileset> availData = convert.Perform<Tileset>().ToList();

			// Generate objects
			foreach (Tileset tileset in availData)
			{
				if (convert.IsObjectHandled(tileset)) continue;

				// Retrieve previously generated GameObjects and Tilemaps for re-use
				GameObject gameobj = convert.Result.OfType<GameObject>().FirstOrDefault();
				Tilemap tilemap = convert.Result.OfType<Tilemap>().FirstOrDefault();
				TilemapRenderer tilemapRenderer = convert.Result.OfType<TilemapRenderer>().FirstOrDefault();
				if (tilemap == null && gameobj != null) tilemap = gameobj.GetComponent<Tilemap>();

				// Create a new Tilemap (and TilemapRenderer) if none did exist before
				if (tilemap == null)
				{
					tilemap = new Tilemap();

					// Determine the first tile index that isn't visually empty
					int firstNonEmptyTileIndex = 0;
					for (int i = 0; i < tileset.TileData.Count; i++)
					{
						if (!tileset.TileData[i].IsVisuallyEmpty)
						{
							firstNonEmptyTileIndex = i;
							break;
						}
					}

					// Resize the Tilemap and fill it with the first visually non-empty tile.
					tilemap.Resize(10, 10);
					tilemap.BeginUpdateTiles().Fill(
						new Tile { Index = firstNonEmptyTileIndex }, 
						0, 
						0, 
						tilemap.TileCount.X, 
						tilemap.TileCount.Y);
					tilemap.EndUpdateTiles();

					// Add a renderer for this Tilemap to the result list, if there was none before
					if (tilemapRenderer == null)
					{
						results.Add(new TilemapRenderer());
					}
				}

				// Configure the Tilemap according to the Tileset we're converting
				tilemap.Tileset = tileset;

				// Add the Tilemap to our result set
				results.Add(tilemap);
				convert.SuggestResultName(tilemap, tileset.Name);
				convert.MarkObjectHandled(tileset);
			}

			convert.AddResult(results);
			return false;
		}
	}
}
