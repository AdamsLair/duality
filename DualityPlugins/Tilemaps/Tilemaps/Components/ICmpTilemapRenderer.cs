using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components;
using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Describes an interface for a <see cref="Component"/> that renders a <see cref="Tilemap"/>.
	/// This interface is typically used by the editor to query data for providing <see cref="Tilemap"/>
	/// editing capabilities.
	/// </summary>
	public interface ICmpTilemapRenderer : ICmpRenderer
	{
		/// <summary>
		/// [GET] A reference to the <see cref="Tilemap"/> that is currently rendered by this <see cref="ICmpTilemapRenderer"/>.
		/// </summary>
		Tilemap ActiveTilemap { get; }
		/// <summary>
		/// [GET] The rectangular region that is occupied by the rendered <see cref="Tilemap"/>, in local / object space.
		/// </summary>
		Rect LocalTilemapRect { get; }
		/// <summary>
		/// [GET / SET] A color by which the rendered <see cref="Tilemap"/> is tinted.
		/// </summary>
		ColorRgba ColorTint { get; set; }
		/// <summary>
		/// [GET] The base depth offset that will be used when rendering the <see cref="Tilemap"/>.
		/// This property represents the sum of all non-local depth adjustments in the rendered <see cref="Tilemap"/>,
		/// expressed as an offset to the depth that is implicitly defined by the <see cref="Transform"/> Z position.
		/// </summary>
		float BaseDepthOffset { get; }


		/// <summary>
		/// Given the specified coordinate in local / object space, this method returns the
		/// tile index that is located there.
		/// </summary>
		/// <param name="localPos"></param>
		/// <param name="pickMode">Specifies the desired behavior when attempting to get a tile outside the rendered area.</param>
		/// <returns></returns>
		Point2 GetTileAtLocalPos(Vector2 localPos, TilePickMode pickMode);
		/// <summary>
		/// Determines the generated depth offset for the tile at the specified tile coordinates.
		/// This also inclues the renderers overall depth offset.
		/// </summary>
		/// <param name="tilePos"></param>
		/// <returns></returns>
		float GetTileDepthOffsetAt(Point2 tilePos);
	}
}
