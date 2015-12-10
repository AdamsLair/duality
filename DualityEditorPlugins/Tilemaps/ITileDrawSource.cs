using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps
{
	/// <summary>
	/// When drawing tiles onto a <see cref="Tilemap"/>, this represents the source 
	/// from which the drawn <see cref="Tile"/> data is retrieved.
	/// </summary>
	public interface ITileDrawSource
	{
		/// <summary>
		/// [GET] If the tiles are or were originally retrieved from a <see cref="Tilemap"/>,
		/// this property returns a reference to it. Otherwise, it returns null.
		/// </summary>
		Tilemap SourceTilemap { get; }
		/// <summary>
		/// [GET] An origin offset within the <see cref="ITileDrawSource"/>. 
		/// </summary>
		Point2 SourceOrigin { get; }
		/// <summary>
		/// [GET] The shape of the tiles to draw.
		/// </summary>
		IReadOnlyGrid<bool> SourceShape { get; }

		/// <summary>
		/// Fills the specified target grid with tiles from this source.
		/// </summary>
		/// <param name="target"></param>
		void FillTarget(Grid<Tile> target);
	}
}
