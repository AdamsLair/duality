using System;

using Duality;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps.CamViewStates
{
	/// <summary>
	/// Defines whether and how a <see cref="Tilemap"/> paint operation will use AutoTile logic
	/// that is defined in the used <see cref="Tileset"/>.
	/// </summary>
	public enum AutoTilePaintMode
	{
		/// <summary>
		/// Potentially defined AutoTiles will be ignored entirely. All painted tiles
		/// are exactly like provided by the source.
		/// </summary>
		None,
		/// <summary>
		/// Only the painted tiles themselves will use AutoTile logic, but no neighbour
		/// tiles will be updated in the painting process.
		/// </summary>
		DirectOnly,
		/// <summary>
		/// Both the painted tiles themselves, as well as neighbouring tiles will be
		/// updated according to AutoTile logic.
		/// </summary>
		Full
	}
}
