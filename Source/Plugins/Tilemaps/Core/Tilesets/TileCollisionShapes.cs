using System;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Aggregates a single tile's multi-layer collision info.
	/// </summary>
	/// <seealso cref="TileCollisionLayer"/>
	/// <seealso cref="TileCollisionShape"/>
	/// <seealso cref="TileInput"/>
	public struct TileCollisionShapes
	{
		public static readonly int LayerCount = 4;

		/// <summary>
		/// The tiles collision shape on the default layer.
		/// </summary>
		private TileCollisionShape Layer0;
		/// <summary>
		/// The tiles collision shape on first auxilliary layer.
		/// </summary>
		private TileCollisionShape Layer1;
		/// <summary>
		/// The tiles collision shape on second auxilliary layer.
		/// </summary>
		private TileCollisionShape Layer2;
		/// <summary>
		/// The tiles collision shape on third auxilliary layer.
		/// </summary>
		private TileCollisionShape Layer3;

		/// <summary>
		/// [GET / SET] The collision shape on a given layer index from zero to (<see cref="LayerCount"/> - 1).
		/// </summary>
		/// <param name="layerIndex"></param>
		/// <returns></returns>
		public TileCollisionShape this[int layerIndex]
		{
			get
			{
				switch (layerIndex)
				{
					case 0: return this.Layer0;
					case 1: return this.Layer1;
					case 2: return this.Layer2;
					case 3: return this.Layer3;
				}
				throw new IndexOutOfRangeException("Invalid collision layer index");
			}
			set
			{
				switch (layerIndex)
				{
					case 0: this.Layer0 = value; return;
					case 1: this.Layer1 = value; return;
					case 2: this.Layer2 = value; return;
					case 3: this.Layer3 = value; return;
				}
				throw new IndexOutOfRangeException("Invalid collision layer index");
			}
		}
		/// <summary>
		/// [GET] The collision shape on the specified (set of) layer(s).
		/// </summary>
		/// <param name="layerMask"></param>
		/// <returns></returns>
		public TileCollisionShape this[TileCollisionLayer layerMask]
		{
			get
			{
				TileCollisionShape result = TileCollisionShape.Free;
				if ((layerMask & TileCollisionLayer.Layer0) != TileCollisionLayer.None) result |= this.Layer0;
				if ((layerMask & TileCollisionLayer.Layer1) != TileCollisionLayer.None) result |= this.Layer1;
				if ((layerMask & TileCollisionLayer.Layer2) != TileCollisionLayer.None) result |= this.Layer2;
				if ((layerMask & TileCollisionLayer.Layer3) != TileCollisionLayer.None) result |= this.Layer3;
				return result;
			}
		}
	}
}
