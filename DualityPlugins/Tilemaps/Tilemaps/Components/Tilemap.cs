using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// A <see cref="Tilemap"/> Component holds the actual map information that is used by other Components to display and interact with.
	/// Without the appropriate renderer, it remains invisible and without the appropriate collider, it won't interact physically.
	/// </summary>
	[EditorHintCategory(TilemapsResNames.CategoryTilemaps)]
	[EditorHintImage(TilemapsResNames.ImageTilemap)]
	public class Tilemap : Component
	{
		private ContentRef<Tileset> tileset  = null;
		private TilemapData         tileData = new TilemapData();

		[DontSerialize] private int    updateStack       = 0;
		[DontSerialize] private Point2 updateTopLeft     = Point2.Zero;
		[DontSerialize] private Point2 updateBottomRight = Point2.Zero;

		[DontSerialize] 
		private EventHandler<TilemapChangedEventArgs> eventTilemapChanged = null;
		public event EventHandler<TilemapChangedEventArgs> EventTilemapChanged
		{
			add { this.eventTilemapChanged += value; }
			remove { this.eventTilemapChanged -= value; }
		}


		/// <summary>
		/// [GET / SET] The <see cref="Tileset"/> that defined the properties of each type of <see cref="Tile"/>.
		/// </summary>
		public ContentRef<Tileset> Tileset
		{
			get { return this.tileset; }
			set { this.tileset = value; }
		}
		/// <summary>
		/// [GET] An interface providing read access to the tile data in this <see cref="Tilemap"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IReadOnlyGrid<Tile> Tiles
		{
			get { return this.tileData.Tiles; }
		}
		/// <summary>
		/// [GET] The number of tiles on each axis.
		/// </summary>
		public Point2 Size
		{
			get { return this.tileData.Size; }
		}


		/// <summary>
		/// Sets a single tile at the specified position.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="tile"></param>
		public void SetTile(int x, int y, Tile tile)
		{
			this.tileData.Tiles[x, y] = tile;
			this.OnTilesChanged(x, y, 1, 1);
		}
		/// <summary>
		/// Resizes the <see cref="Tilemap"/>.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="origin"></param>
		public void Resize(int width, int height, Alignment origin = Alignment.TopLeft)
		{
			if (this.tileData.Tiles.Width == width && this.tileData.Tiles.Height == height) return;

			this.tileData.Tiles.Resize(width, height, origin);
			this.OnTilesChanged();
		}
		/// <summary>
		/// Clears the <see cref="Tilemap"/> without modifying its size.
		/// </summary>
		public void Clear()
		{
			this.tileData.Tiles.Clear();
			this.OnTilesChanged();
		}

		/// <summary>
		/// Begins an external update operation to the tile data stored in the <see cref="Tilemap"/>. Calling this
		/// method will grant direct write access to the internal tile data. No data will be copied.
		/// 
		/// Note that you are not required to call this method when using other <see cref="Tilemap"/> API to update
		/// tile data. Instead, use this method only when direct write access to the internal tile data should be
		/// acquired.
		/// </summary>
		/// <returns>The internal tile data of the <see cref="Tilemap"/>, which can now be modified externally</returns>
		public Grid<Tile> BeginUpdateTiles()
		{
			// Push an active editing operation. We'll fire events when the last
			// editing operation is popped from the update stack.
			this.updateStack++;
			return this.tileData.Tiles;
		}
		/// <summary>
		/// Ends an external update operation to the tile data stored in the <see cref="Tilemap"/>. Calling this
		/// method will notify other <see cref="Component"/> classes about changes, so they can update
		/// themselves. By specifying exactly which rectangular region was affected by the operation,
		/// these updates can be performed a lot faster.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void EndUpdateTiles(int x, int y, int width, int height)
		{
			if (this.updateStack == 0) throw new InvalidOperationException("Can't end a Tilemap update when there was none being performed.");

			// Pop an active editing operation.
			this.updateStack--;

			// Adjust updated region for the aggregated event we'll fire when its over.
			// Only do so if we have actually changed something.
			if (width > 0 && height > 0)
			{
				if (this.updateTopLeft == Point2.Zero)
					this.updateTopLeft = new Point2(x, y);

				this.updateTopLeft.X = Math.Min(this.updateTopLeft.X, x);
				this.updateTopLeft.Y = Math.Min(this.updateTopLeft.Y, y);
				this.updateBottomRight.X = Math.Max(this.updateBottomRight.X, x + width);
				this.updateBottomRight.Y = Math.Max(this.updateBottomRight.Y, y + height);
			}

			// If we just ended our last stacked editing operation, fire the change event
			if (this.updateStack == 0)
			{
				this.OnTilesChanged(
					this.updateTopLeft.X, 
					this.updateTopLeft.Y, 
					this.updateBottomRight.X - this.updateTopLeft.X, 
					this.updateBottomRight.Y - this.updateTopLeft.Y);
				this.updateTopLeft = Point2.Zero;
				this.updateBottomRight = Point2.Zero;
			}
		}
		/// <summary>
		/// Ends an external update operation to the tile data stored in the <see cref="Tilemap"/>. Calling this
		/// method will notify other <see cref="Component"/> classes about changes, so they can update
		/// themselves. However, since no specific region is specified that is affected by the operation,
		/// a potentially expensive full update needs to be performed.
		/// </summary>
		public void EndUpdateTiles()
		{
			this.EndUpdateTiles(0, 0, this.tileData.Tiles.Width, this.tileData.Tiles.Height);
		}

		private void OnTilesChanged(int x, int y, int width, int height)
		{
			if (width == 0 || height == 0) return;
			if (this.eventTilemapChanged != null)
				this.eventTilemapChanged(this, new TilemapChangedEventArgs(this, x, y, width, height));
		}
		private void OnTilesChanged()
		{
			this.OnTilesChanged(0, 0, this.tileData.Tiles.Width, this.tileData.Tiles.Height);
		}
	}
}
