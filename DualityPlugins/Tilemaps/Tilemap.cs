using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// A <see cref="Tilemap"/> Component holds the actual map information that is used by other Components to display and interact with.
	/// Without the appropriate renderer, it remains invisible and without the appropriate collider, it won't interact physically.
	/// </summary>
	[EditorHintCategory(TilemapsResNames.CategoryTilemaps)]
	[EditorHintImage(TilemapsResNames.ImageTilemap)]
	public class Tilemap : Component
	{
		private Grid<Tile> tiles = new Grid<Tile>();
	}
}
