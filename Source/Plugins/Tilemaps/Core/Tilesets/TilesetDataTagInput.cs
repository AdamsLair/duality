using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Plugins.Tilemaps
{
	public class TilesetDataTagInput
	{
		public string Key { get; set; }

		private RawList<TileData> tileData = new RawList<TileData>();
		public RawList<TileData> TileData { get { return this.tileData; } }
	}

	public class TileData
	{
		public string Value { get; set; }
	}
}
