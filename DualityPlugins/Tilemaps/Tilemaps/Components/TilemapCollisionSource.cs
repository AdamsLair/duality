using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Editor;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Specifies a source of <see cref="Tilemap"/> collision information.
	/// </summary>
	public struct TilemapCollisionSource
	{
		/// <summary>
		/// The <see cref="Tilemap"/> that will serve as a provider for collision information.
		/// </summary>
		public Tilemap SourceTilemap;
		/// <summary>
		/// A <see cref="TileCollisionLayer"/> bitmask that will specify which collision layers to use
		/// from the <see cref="TilemapCollisionSource.SourceTilemap"/>.
		/// </summary>
		public TileCollisionLayer Layers;

		public override string ToString()
		{
			if (this.SourceTilemap == null)
				return string.Format("{0}", this.Layers);
			else if (this.SourceTilemap.GameObj != null)
				return string.Format("{0}, {1}", this.SourceTilemap.GameObj.Name, this.Layers);
			else
				return string.Format("Private Tilemap, {0}", this.Layers);
		}
	}
}
