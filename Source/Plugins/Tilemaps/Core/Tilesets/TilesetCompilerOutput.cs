using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Cloning;
using Duality.Resources;
using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Contains output data that was produced or modified by a <see cref="TilesetCompiler"/>.
	/// </summary>
	public struct TilesetCompilerOutput
	{
		/// <summary>
		/// The overall number of tiles in the compiled tileset.
		/// </summary>
		public int TileCount;
		/// <summary>
		/// Per-tile information providing collision and rendering cues.
		/// Corresponds to the tile input that was specified in the <see cref="TilesetCompilerInput"/>.
		/// </summary>
		public RawList<TileInfo> TileData;
		/// <summary>
		/// One texture for each visual layer. Corresponds to the visual
		/// layers that were specified in the <see cref="TilesetCompilerInput"/>.
		/// </summary>
		public List<Texture> RenderData;
		/// <summary>
		/// One AutoTile info for each AutoTile type. Corresponds to the
		/// AutoTile input that was specified in the <see cref="TilesetCompilerInput"/>.
		/// </summary>
		public List<TilesetAutoTileInfo> AutoTileData;
		/// <summary>
		/// The tile index that should be used for painting empty tiles.
		/// -1 indicates that there are no empty tiles.
		/// </summary>
		public int EmptyTileIndex;
	}
}
