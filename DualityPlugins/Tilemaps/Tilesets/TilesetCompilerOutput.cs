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
	public struct TilesetCompilerOutput
	{
		public int TileCount;
		public RawList<TileInfo> TileData;
		public List<Texture> RenderData;
	}
}
