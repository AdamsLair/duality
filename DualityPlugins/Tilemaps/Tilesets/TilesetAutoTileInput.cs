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
		public static readonly string DefaultName    = "AutoTile";
		public static readonly string DefaultId      = "autoTile";

		private string name                 = DefaultName;
		private string id                   = DefaultId;
		private int    baseTile             = 0;
		private bool   generateMissingTiles = true;
		private Dictionary<TileConnection,int> tiles = new Dictionary<TileConnection,int>();
		
		/// <summary>
		/// [GET / SET] The human-friendly name of this AutoTile.
		/// </summary>
		public string Name
		{
			get { return this.name; }
			set { this.name = value ?? DefaultName; }
		}
		/// <summary>
		/// [GET / SET] The id of this AutoTile input, which can be used by game and editor code to identify it.
		/// </summary>
		public string Id
		{
			get { return this.id; }
			set { this.id = value ?? DefaultId; }
		}
		/// <summary>
		/// [GET / SET] The tile index inside the <see cref="Tileset"/> that is considered to be the base
		/// tile representing the entire set of connection-dependend tiles inside this AutoTile.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int BaseTileIndex
		{
			get { return this.baseTile; }
			set { this.baseTile = value; }
		}
		/// <summary>
		/// [GET] A dictionary that maps connectivity states to tile indices.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
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
