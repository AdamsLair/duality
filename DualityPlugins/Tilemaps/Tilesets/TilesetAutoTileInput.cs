using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Describes a single auto-tile input in a <see cref="Tileset"/> definition. When compiling the <see cref="Tileset"/>,
	/// these settings are mapped and transformed into one <see cref="TilesetAutoTileInfo"/> data structure for each
	/// <see cref="TilesetAutoTileInput"/> that was defined.
	/// </summary>
	public class TilesetAutoTileInput
	{
		private int baseTile = 0;
		private Dictionary<TileConnection,int> tiles = new Dictionary<TileConnection,int>();
		private bool generateMissingTiles = true;

		/// <summary>
		/// [GET / SET] The tile index inside the <see cref="Tileset"/> that is considered to be the base
		/// tile representing the entire set of connection-dependend tiles inside this auto-tile.
		/// </summary>
		public int BaseTileIndex
		{
			get { return this.baseTile; }
			set { this.baseTile = value; }
		}
		/// <summary>
		/// [GET] A dictionary that maps connectivity states to tile indices.
		/// </summary>
		public IDictionary<TileConnection,int> BorderTileIndices
		{
			get { return this.tiles; }
		}
		/// <summary>
		/// [GET / SET] Specifies whether the <see cref="Tileset"/> should generate any missing connection tiles for
		/// this auto-tile when compiled.
		/// </summary>
		public bool GenerateMissingTiles
		{
			get { return this.generateMissingTiles; }
			set { this.generateMissingTiles = value; }
		}
	}
}
