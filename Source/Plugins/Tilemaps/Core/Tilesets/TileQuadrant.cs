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
	/// Represents one of four rectangular subsections of a tile in a <see cref="Tileset"/> or <see cref="Tilemap"/>.
	/// </summary>
	public enum TileQuadrant : byte
	{
		TopLeft,
		TopRight,
		BottomRight,
		BottomLeft
	}
}
