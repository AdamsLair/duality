using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps
{
	/// <summary>
	/// Event arguments that provide data about a switch from one tile index to another.
	/// </summary>
	public class TilesetViewTileIndexChangeEventArgs : EventArgs
	{
		private int tileIndex;
		private int lastTileIndex;

		/// <summary>
		/// [GET] The tile index that is affected by this event.
		/// </summary>
		public int TileIndex
		{
			get { return this.tileIndex; }
		}
		/// <summary>
		/// [GET] The last tile index that was affected by this event.
		/// </summary>
		public int LastTileIndex
		{
			get { return this.lastTileIndex; }
		}

		public TilesetViewTileIndexChangeEventArgs(int tileIndex, int lastTileIndex)
		{
			this.tileIndex = tileIndex;
			this.lastTileIndex = lastTileIndex;
		}
	}
}
