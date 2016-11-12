using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Provides event arguments for <see cref="Tilemap"/> changes that affect a certain region,
	/// such as editing operations or runtime modifications of a <see cref="Tilemap"/>.
	/// </summary>
	public class TilemapChangedEventArgs : ComponentEventArgs
	{
		private Point2 pos;
		private Point2 size;

		/// <summary>
		/// [GET] The top left position of the affected <see cref="Tilemap"/> region.
		/// </summary>
		public Point2 Pos
		{
			get { return this.pos; }
		}
		/// <summary>
		/// [GET] The size of the affected <see cref="Tilemap"/> region, starting from <see cref="Pos"/>.
		/// </summary>
		public Point2 Size
		{
			get { return this.size; }
		}

		public TilemapChangedEventArgs(Tilemap tilemap, int x, int y, int width, int height) : base(tilemap)
		{
			this.pos = new Point2(x, y);
			this.size = new Point2(width, height);
		}
	}
}
