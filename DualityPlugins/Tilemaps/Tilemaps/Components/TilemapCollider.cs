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
			this.tileCount = this.GetTileCount();

			this.sectorCount = new Point2(
				1 + (this.tileCount.X - 1 / SectorSize),
				1 + (this.tileCount.Y - 1 / SectorSize));
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
			int newChecksum = this.CalculateChecksum(sectorX, sectorY);
			w.Stop();
			Log.Core.Write("Checksum Gen Time: {0} ms", w.Elapsed.TotalMilliseconds);

			// If it differs from our previous value, update collision shapes
			if (sector.Checksum != newChecksum)
			{
				Log.Core.Write("Different CheckSum: {0} --> {1}", sector.Checksum, newChecksum);

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
				this.GenerateCollisionShapes(sectorX, sectorY, sector.Shapes);
				sector.Checksum = newChecksum;
			}

			this.sectors[sectorX, sectorY] = sector;
		}
		private int CalculateChecksum(int sectorX, int sectorY)
		{
			Point2 beginTile = new Point2(sectorX * SectorSize, sectorY * SectorSize);
			Point2 endTile = new Point2((sectorX + 1) * SectorSize, (sectorY + 1) * SectorSize);
			endTile.X = Math.Min(endTile.X, this.tileCount.X);
			endTile.Y = Math.Min(endTile.Y, this.tileCount.Y);

			TileInfo[][] tileData = new TileInfo[this.sourceTilemaps.Length][];
			for (int i = 0; i < this.sourceTilemaps.Length; i++)
			{
				if (this.sourceTilemaps[i] == null) continue;
				if (this.sourceTilemaps[i].Tileset == null) continue;
				tileData[i] = this.sourceTilemaps[i].Tileset.Res.TileData.Data;
			}

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
					MathF.CombineHashCode(ref checksum, (int)mergedCollision);
				}
			}

			return checksum;
		}
		private void GenerateCollisionShapes(int sectorX, int sectorY, IList<ShapeInfo> shapeList)
		{

		}
		
		private Point2 GetTileCount()
		{
			Point2 count = new Point2(int.MaxValue, int.MaxValue);
			for (int i = 0; i < this.sourceTilemaps.Length; i++)
			{
				if (this.sourceTilemaps[i] == null) continue;
				count.X = Math.Min(count.X, this.sourceTilemaps[i].TileCount.X);
				count.Y = Math.Min(count.Y, this.sourceTilemaps[i].TileCount.Y);
			}
			return count;
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
			Point2 newTileCount = this.GetTileCount();
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
	}
}
