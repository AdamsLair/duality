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
	/// Uses the information from the local <see cref="Tilemap"/> to configure the local <see cref="RigidBody"/> for 
	/// simulating physical interaction with the <see cref="Tilemap"/>.
	/// </summary>
	[RequiredComponent(typeof(RigidBody))]
	[EditorHintCategory(TilemapsResNames.CategoryTilemaps)]
	[EditorHintImage(TilemapsResNames.ImageTilemapCollider)]
	public class TilemapCollider : Component, ICmpInitializable
	{
		private struct Sector
		{
			public List<ShapeInfo> Shapes;
			public int Checksum;
		}

		private static readonly int SectorSize = 32;
		private static readonly TilemapCollisionSource[] DefaultSource = new TilemapCollisionSource[] 
		{
			new TilemapCollisionSource
			{
				SourceTilemap = null, 
				Layers = TileCollisionLayer.Layer0
			}
		};

		private TilemapCollisionSource[] source = DefaultSource;

		[DontSerialize] private Tilemap[] sourceTilemaps = null;
		[DontSerialize] private Point2 tileCount = Point2.Zero;
		[DontSerialize] private Point2 sectorCount = Point2.Zero;
		[DontSerialize] private Grid<Sector> sectors = null;
		[DontSerialize] private Grid<TileCollisionShape> tempCollisionData = new Grid<TileCollisionShape>(SectorSize, SectorSize);
		[DontSerialize] private TileEdgeMap tempEdgeMap = new TileEdgeMap(SectorSize + 1, SectorSize + 1);


		/// <summary>
		/// [GET / SET] Specifies which <see cref="Tilemap"/> components and collision layers to use
		/// to generate the collision shape.
		/// </summary>
		public IReadOnlyList<TilemapCollisionSource> CollisionSource
		{
			get { return this.source; }
			set
			{
				if (this.source != value)
				{
					this.UnsubscribeSourceEvents();
					this.source = value.ToArray() ?? DefaultSource;
					this.RetrieveSourceTilemaps();
					this.SubscribeSourceEvents();
				}
			}
		}

		private void UpdateRigidBody()
		{
			this.tileCount = GetTileCount(this.sourceTilemaps);

			this.sectorCount = new Point2(
				1 + ((this.tileCount.X - 1) / SectorSize),
				1 + ((this.tileCount.Y - 1) / SectorSize));
			this.sectors = new Grid<Sector>(
				this.sectorCount.X, 
				this.sectorCount.Y);

			for (int y = 0; y < this.sectorCount.Y; y++)
			{
				for (int x = 0; x < this.sectorCount.X; x++)
				{
					this.UpdateRigidBody(x, y);
				}
			}
		}
		private void UpdateRigidBody(int sectorX, int sectorY)
		{
			Log.Core.Write("GenerateCollisionShapes {0}", new Point2(sectorX, sectorY));

			RigidBody body = this.GameObj.GetComponent<RigidBody>();
			Sector sector = this.sectors[sectorX, sectorY];

			// Determine collision checksum
			var w = System.Diagnostics.Stopwatch.StartNew();
			int newChecksum = this.MergeCollisionData(sectorX, sectorY, this.tempCollisionData);
			w.Stop();
			Log.Core.Write("Checksum Gen Time: {0} ms", w.Elapsed.TotalMilliseconds);

			// If it differs from our previous value, update collision shapes
			if (sector.Checksum != newChecksum)
			{
				Log.Core.Write("Different CheckSum: {0} --> {1}", sector.Checksum, newChecksum);
				var w2 = System.Diagnostics.Stopwatch.StartNew();

				// Clean up old shapes
				if (sector.Shapes != null)
				{
					foreach (ShapeInfo shape in sector.Shapes)
						body.RemoveShape(shape);
					sector.Shapes.Clear();
				}
				else
				{
					sector.Shapes = new List<ShapeInfo>();
				}

				// Generate new shapes
				this.GenerateCollisionShapes(this.tempCollisionData, sector.Shapes);
				sector.Checksum = newChecksum;

				w2.Stop();
				Log.Core.Write("Shape Gen Time: {0} ms", w2.Elapsed.TotalMilliseconds);
			}

			this.sectors[sectorX, sectorY] = sector;
		}
		private void GenerateCollisionShapes(Grid<TileCollisionShape> collisionData, IList<ShapeInfo> shapeList)
		{
			this.tempEdgeMap.Clear();

			// Populate the edge map with all the fence tiles first
			for (int y = 0; y < SectorSize; y++)
			{
				for (int x = 0; x < SectorSize; x++)
				{
					TileCollisionShape collision = collisionData[x, y];

					// Skip both free and completely solid tiles
					if (collision == TileCollisionShape.Free)
						continue;
					if ((collision & TileCollisionShape.Solid) == TileCollisionShape.Solid)
						continue;

					// Add the various fence collision types
					if ((collision & TileCollisionShape.Top) != TileCollisionShape.Free)
						this.tempEdgeMap.AddEdge(new Point2(x, y), new Point2(x + 1, y));
					if ((collision & TileCollisionShape.Bottom) != TileCollisionShape.Free)
						this.tempEdgeMap.AddEdge(new Point2(x, y + 1), new Point2(x + 1, y + 1));
					if ((collision & TileCollisionShape.Left) != TileCollisionShape.Free)
						this.tempEdgeMap.AddEdge(new Point2(x, y), new Point2(x, y + 1));
					if ((collision & TileCollisionShape.Right) != TileCollisionShape.Free)
						this.tempEdgeMap.AddEdge(new Point2(x + 1, y), new Point2(x + 1, y + 1));
					if ((collision & TileCollisionShape.DiagonalDown) != TileCollisionShape.Free)
						this.tempEdgeMap.AddEdge(new Point2(x, y), new Point2(x + 1, y + 1));
					if ((collision & TileCollisionShape.DiagonalUp) != TileCollisionShape.Free)
						this.tempEdgeMap.AddEdge(new Point2(x, y + 1), new Point2(x + 1, y));
				}
			}

			// Now add block geometry
			for (int y = 0; y < SectorSize; y++)
			{
				for (int x = 0; x < SectorSize; x++)
				{
					// Skip non-solid blocks
					bool center = (collisionData[x, y] & TileCollisionShape.Solid) == TileCollisionShape.Solid;
					if (!center) continue;

					// A filled block will always overwrite its inner diagonal edges
					this.tempEdgeMap.RemoveEdge(new Point2(x, y), new Point2(x + 1, y + 1));
					this.tempEdgeMap.RemoveEdge(new Point2(x, y + 1), new Point2(x + 1, y));

					// Determine block collision neighbourhood
					bool left   = (x == 0)              || (collisionData[x - 1, y] & TileCollisionShape.Solid) == TileCollisionShape.Solid;
					bool right  = (x == SectorSize - 1) || (collisionData[x + 1, y] & TileCollisionShape.Solid) == TileCollisionShape.Solid;
					bool top    = (y == 0)              || (collisionData[x, y - 1] & TileCollisionShape.Solid) == TileCollisionShape.Solid;
					bool bottom = (y == SectorSize - 1) || (collisionData[x, y + 1] & TileCollisionShape.Solid) == TileCollisionShape.Solid;

					// Adjust outer edge states 
					if (center != left )  this.tempEdgeMap.AddEdge   (new Point2(x, y), new Point2(x, y + 1));
					else                  this.tempEdgeMap.RemoveEdge(new Point2(x, y), new Point2(x, y + 1));
					if (center != right)  this.tempEdgeMap.AddEdge   (new Point2(x + 1, y), new Point2(x + 1, y + 1));
					else                  this.tempEdgeMap.RemoveEdge(new Point2(x + 1, y), new Point2(x + 1, y + 1));
					if (center != top)    this.tempEdgeMap.AddEdge   (new Point2(x, y), new Point2(x + 1, y));
					else                  this.tempEdgeMap.RemoveEdge(new Point2(x, y), new Point2(x + 1, y));
					if (center != bottom) this.tempEdgeMap.AddEdge   (new Point2(x, y + 1), new Point2(x + 1, y + 1));
					else                  this.tempEdgeMap.RemoveEdge(new Point2(x, y + 1), new Point2(x + 1, y + 1));
				}
			}
		}
		private int MergeCollisionData(int sectorX, int sectorY, Grid<TileCollisionShape> target)
		{
			Point2 beginTile = new Point2(sectorX * SectorSize, sectorY * SectorSize);
			Point2 endTile = new Point2((sectorX + 1) * SectorSize, (sectorY + 1) * SectorSize);
			endTile.X = Math.Min(endTile.X, this.tileCount.X);
			endTile.Y = Math.Min(endTile.Y, this.tileCount.Y);

			TileInfo[][] tileData = GetRawTileData(this.sourceTilemaps);

			int checksum = 0;
			for (int y = beginTile.Y; y < endTile.Y; y++)
			{
				for (int x = beginTile.X; x < endTile.X; x++)
				{
					TileCollisionShape mergedCollision = TileCollisionShape.Free;
					for (int i = 0; i < this.sourceTilemaps.Length; i++)
					{
						if (this.sourceTilemaps[i] == null) continue;
						if (tileData[i] == null) continue;

						Tile tile = this.sourceTilemaps[i].Tiles[x, y];
						TileCollisionShape collision = tileData[i][tile.Index].Collision[this.source[i].Layers];
						mergedCollision |= collision;
					}
					target[x - beginTile.X, y - beginTile.Y] = mergedCollision;
					MathF.CombineHashCode(ref checksum, (int)mergedCollision);
				}
			}

			return checksum;
		}
		
		private void RetrieveSourceTilemaps()
		{
			Tilemap localTilemap = this.GameObj.GetComponent<Tilemap>();

			this.sourceTilemaps = new Tilemap[this.source.Length];
			for (int i = 0; i < this.sourceTilemaps.Length; i++)
			{
				this.sourceTilemaps[i] = 
					this.source[i].SourceTilemap ?? 
					localTilemap;
			}
		}
		private void SubscribeSourceEvents()
		{
			EventHandler<TilemapChangedEventArgs> handler = this.SourceTilemap_EventTilemapChanged;
			for (int i = 0; i < this.sourceTilemaps.Length; i++)
			{
				if (this.sourceTilemaps[i] == null) continue;

				// Use the unsubscribe-subscribe pattern to avoid subscribing twice
				this.sourceTilemaps[i].EventTilemapChanged -= handler;
				this.sourceTilemaps[i].EventTilemapChanged += handler;
			}
		}
		private void UnsubscribeSourceEvents()
		{
			EventHandler<TilemapChangedEventArgs> handler = this.SourceTilemap_EventTilemapChanged;
			for (int i = 0; i < this.sourceTilemaps.Length; i++)
			{
				if (this.sourceTilemaps[i] == null) continue;
				this.sourceTilemaps[i].EventTilemapChanged -= handler;
			}
		}

		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				this.RetrieveSourceTilemaps();
				this.UpdateRigidBody();
				this.SubscribeSourceEvents();
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
			{
				this.UnsubscribeSourceEvents();
				this.sourceTilemaps = null;
			}
		}

		private void SourceTilemap_EventTilemapChanged(object sender, TilemapChangedEventArgs e)
		{
			Log.Core.Write("TilemapChanged: {0}, [{1}:{2}]", e.Component, e.Pos, e.Size);
			Log.Core.PushIndent();

			// If we resized our tilemap, we'll have to do a full update
			Point2 newTileCount = GetTileCount(this.sourceTilemaps);
			if (newTileCount != this.tileCount)
			{
				Log.Core.Write("Resize from {0} to {1}", this.tileCount, newTileCount);
				this.UpdateRigidBody();
			}
			// Otherwise, only update the sectors that are affected by the change
			else
			{
				Point2 minSector = new Point2(
					MathF.Clamp(e.Pos.X / SectorSize, 0, this.sectorCount.X),
					MathF.Clamp(e.Pos.Y / SectorSize, 0, this.sectorCount.Y));
				Point2 maxSector = new Point2(
					MathF.Clamp(1 + (e.Pos.X + e.Size.X) / SectorSize, 0, this.sectorCount.X),
					MathF.Clamp(1 + (e.Pos.Y + e.Size.Y) / SectorSize, 0, this.sectorCount.Y));
				Log.Core.Write("Selective Update of Sectors {0} [inclusive] to {1} [exclusive]", minSector, maxSector);
				Log.Core.PushIndent();
				for (int y = minSector.Y; y < maxSector.Y; y++)
				{
					for (int x = minSector.X; x < maxSector.X; x++)
					{
						this.UpdateRigidBody(x, y);
					}
				}
				Log.Core.PopIndent();
			}

			Log.Core.PopIndent();
		}

		private static TileInfo[][] GetRawTileData(Tilemap[] tilemaps)
		{
			TileInfo[][] tileData = new TileInfo[tilemaps.Length][];
			for (int i = 0; i < tilemaps.Length; i++)
			{
				if (tilemaps[i] == null) continue;
				if (tilemaps[i].Tileset == null) continue;
				tileData[i] = tilemaps[i].Tileset.Res.TileData.Data;
			}
			return tileData;
		}
		private static Point2 GetTileCount(Tilemap[] tilemaps)
		{
			Point2 count = new Point2(int.MaxValue, int.MaxValue);
			for (int i = 0; i < tilemaps.Length; i++)
			{
				if (tilemaps[i] == null) continue;
				count.X = Math.Min(count.X, tilemaps[i].TileCount.X);
				count.Y = Math.Min(count.Y, tilemaps[i].TileCount.Y);
			}
			return count;
		}
	}
}
