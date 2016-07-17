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

		private Alignment                origin          = Alignment.Center;
		private TilemapCollisionSource[] source          = DefaultSource;
		private bool                     solidOuterEdges = true;
		private bool                     roundedCorners  = false;

		[DontSerialize] private Tilemap referenceTilemap = null;
		[DontSerialize] private Tilemap[] sourceTilemaps = null;
		[DontSerialize] private Point2 tileCount = Point2.Zero;
		[DontSerialize] private Point2 sectorCount = Point2.Zero;
		[DontSerialize] private Grid<Sector> sectors = null;
		[DontSerialize] private Grid<TileCollisionShape> tempCollisionData = new Grid<TileCollisionShape>(SectorSize, SectorSize);
		[DontSerialize] private TileEdgeMap tempEdgeMap = new TileEdgeMap(SectorSize + 1, SectorSize + 1);

		
		/// <summary>
		/// [GET / SET] The origin of the generated <see cref="RigidBody"/> shapes, relative to the position of its <see cref="GameObject"/>.
		/// </summary>
		public Alignment Origin
		{
			get { return this.origin; }
			set
			{
				if (this.origin != value)
				{
					this.origin = value;
					this.UpdateRigidBody(true);
				}
			}
		}
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
					this.UpdateRigidBody(false);
				}
			}
		}
		/// <summary>
		/// [GET / SET] Whether the <see cref="TilemapCollider"/> will generate solid edges at the tilemaps outer borders.
		/// </summary>
		public bool SolidOuterEdges
		{
			get { return this.solidOuterEdges; }
			set
			{
				if (this.solidOuterEdges != value)
				{
					this.solidOuterEdges = value;
					this.UpdateRigidBody(true);
				}
			}
		}
		/// <summary>
		/// [GET / SET] Whether the <see cref="TilemapCollider"/> will attempt to generate rounded corners instead of sharp ones.
		/// </summary>
		public bool RoundedCorners
		{
			get { return this.roundedCorners; }
			set
			{
				if (this.roundedCorners != value)
				{
					this.roundedCorners = value;
					this.UpdateRigidBody(true);
				}
			}
		}
		/// <summary>
		/// [GET] The rectangular region that is occupied by the generated <see cref="RigidBody"/> shapes, in local / object space.
		/// </summary>
		public Rect LocalTilemapRect
		{
			get
			{
				Tilemap tilemap = this.referenceTilemap;
				Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
				Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;
				return Rect.Align(this.origin, 0, 0, this.tileCount.X * tileSize.X, this.tileCount.Y * tileSize.Y);
			}
		}
		
		private void ClearRigidBody()
		{
			if (this.GameObj == null) return;

			RigidBody body = this.GameObj.GetComponent<RigidBody>();
			if (body == null) return;

			for (int y = 0; y < this.sectorCount.Y; y++)
			{
				for (int x = 0; x < this.sectorCount.X; x++)
				{
					Sector sector = this.sectors[x, y];
					if (sector.Shapes != null)
					{
						foreach (ShapeInfo shape in sector.Shapes)
							body.RemoveShape(shape);
						sector.Shapes.Clear();
					}
					sector.Checksum = 0;
					this.sectors[x, y] = sector;
				}
			}
		}
		private void UpdateRigidBody(bool rebuildEvenIfUnchanged)
		{
			if (this.sourceTilemaps == null) return;

			Point2 newTileCount = GetTileCount(this.sourceTilemaps);
			Point2 newSectorCount = new Point2(
				1 + ((newTileCount.X - 1) / SectorSize),
				1 + ((newTileCount.Y - 1) / SectorSize));

			if (this.sectors == null)
			{
				this.sectors = new Grid<Sector>(
					newSectorCount.X, 
					newSectorCount.Y);
			}
			else if (rebuildEvenIfUnchanged || 
				this.sectors.Width != newSectorCount.X || 
				this.sectors.Height != newSectorCount.Y)
			{
				this.ClearRigidBody();
				this.sectors.ResizeClear(
					newSectorCount.X, 
					newSectorCount.Y);
			}

			this.tileCount = newTileCount;
			this.sectorCount = newSectorCount;

			RigidBody body = this.GameObj.GetComponent<RigidBody>();
			for (int y = 0; y < this.sectorCount.Y; y++)
			{
				for (int x = 0; x < this.sectorCount.X; x++)
				{
					this.UpdateRigidBody(body, x, y);
				}
			}
		}
		private void UpdateRigidBody(RigidBody body, int sectorX, int sectorY)
		{
			Sector sector = this.sectors[sectorX, sectorY];

			// Determine collision checksum
			this.tempCollisionData.Clear();
			int newChecksum = this.MergeCollisionData(sectorX, sectorY, this.tempCollisionData);

			// If it differs from our previous value, update collision shapes
			if (sector.Checksum != newChecksum)
			{
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
				{
					// Determine general working data
					Tilemap tilemap = this.referenceTilemap;
					Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
					Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;
					Point2 sectorBaseTile = new Point2(
						sectorX * SectorSize,
						sectorY * SectorSize);
					Vector2 sectorBasePos = sectorBaseTile * tileSize;

					// Clear the temporary edge map first
					this.tempEdgeMap.Clear();

					// Populate the edge map with fence and block geometry
					AddFenceCollisionEdges(this.tempCollisionData, this.tempEdgeMap);
					AddBlockCollisionEdges(this.tempCollisionData, this.tempEdgeMap, sectorBaseTile, this.tileCount);
					if (this.solidOuterEdges)
						AddBorderCollisionEdges(this.tempEdgeMap, sectorBaseTile, this.tileCount);

					// Now traverse the edge map and gradually create chain / loop 
					// shapes until all edges have been used.
					Rect localRect = Rect.Align(this.origin, 0, 0, this.tileCount.X * tileSize.X, this.tileCount.Y * tileSize.Y);
					GenerateCollisionShapes(this.tempEdgeMap, localRect.TopLeft + sectorBasePos, tileSize, this.roundedCorners, sector.Shapes);

					// Add all the generated shapes to the target body
					foreach (ShapeInfo shape in sector.Shapes)
						body.AddShape(shape);
				}
				sector.Checksum = newChecksum;
			}

			this.sectors[sectorX, sectorY] = sector;
		}
		private int MergeCollisionData(int sectorX, int sectorY, Grid<TileCollisionShape> target)
		{
			Point2 beginTile = new Point2(sectorX * SectorSize, sectorY * SectorSize);
			Point2 endTile = new Point2((sectorX + 1) * SectorSize, (sectorY + 1) * SectorSize);
			endTile.X = Math.Min(endTile.X, this.tileCount.X);
			endTile.Y = Math.Min(endTile.Y, this.tileCount.Y);

			TileInfo[][] tileData = GetRawTileData(this.sourceTilemaps);

			// Start with a non-zero checksum, so it doesn't equal the uninitialized sector checksum
			int checksum = 1;
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

			this.referenceTilemap = null;
			this.sourceTilemaps = new Tilemap[this.source.Length];
			for (int i = 0; i < this.sourceTilemaps.Length; i++)
			{
				this.sourceTilemaps[i] = 
					this.source[i].SourceTilemap ?? 
					localTilemap;
				if (this.referenceTilemap == null && this.sourceTilemaps[i] != null)
					this.referenceTilemap = this.sourceTilemaps[i];
			}
		}
		private void SubscribeSourceEvents()
		{
			if (this.sourceTilemaps == null) return;

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
			if (this.sourceTilemaps == null) return;

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
				this.UpdateRigidBody(true);
				this.SubscribeSourceEvents();
			}
			else if (context == InitContext.Saved)
			{
				// Since we're removing all generated bodies in the saving process,
				// we'll have to add them back now. Note that we don't actually 
				// re-generate them.
				RigidBody body = this.GameObj.GetComponent<RigidBody>();
				for (int y = 0; y < this.sectorCount.Y; y++)
				{
					for (int x = 0; x < this.sectorCount.X; x++)
					{
						Sector sector = this.sectors[x, y];
						if (sector.Shapes != null)
						{
							foreach (ShapeInfo shape in sector.Shapes)
								body.AddShape(shape);
						}
						this.sectors[x, y] = sector;
					}
				}
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
			{
				this.UnsubscribeSourceEvents();
				this.ClearRigidBody();
				this.sourceTilemaps = null;
			}
			else if (context == ShutdownContext.Saving)
			{
				// To avoid saving the generated collider redundantly, remove
				// all of the generated shapes before saving. We'll add them again later.
				RigidBody body = this.GameObj.GetComponent<RigidBody>();
				for (int y = 0; y < this.sectorCount.Y; y++)
				{
					for (int x = 0; x < this.sectorCount.X; x++)
					{
						Sector sector = this.sectors[x, y];
						if (sector.Shapes != null)
						{
							foreach (ShapeInfo shape in sector.Shapes)
								body.RemoveShape(shape);
						}
					}
				}
			}
		}

		private void SourceTilemap_EventTilemapChanged(object sender, TilemapChangedEventArgs e)
		{
			// If we resized our tilemap, we'll have to do a full update
			Point2 newTileCount = GetTileCount(this.sourceTilemaps);
			if (newTileCount != this.tileCount)
			{
				this.UpdateRigidBody(false);
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
				RigidBody body = this.GameObj.GetComponent<RigidBody>();
				for (int y = minSector.Y; y < maxSector.Y; y++)
				{
					for (int x = minSector.X; x < maxSector.X; x++)
					{
						this.UpdateRigidBody(body, x, y);
					}
				}
			}
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
				count.X = Math.Min(count.X, tilemaps[i].Size.X);
				count.Y = Math.Min(count.Y, tilemaps[i].Size.Y);
			}
			return count;
		}

		private static void AddFenceCollisionEdges(Grid<TileCollisionShape> collisionData, TileEdgeMap targetEdgeMap)
		{
			// Populate the edge map with all the collision fences
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
						targetEdgeMap.AddEdge(new Point2(x, y), new Point2(x + 1, y));
					if ((collision & TileCollisionShape.Bottom) != TileCollisionShape.Free)
						targetEdgeMap.AddEdge(new Point2(x, y + 1), new Point2(x + 1, y + 1));
					if ((collision & TileCollisionShape.Left) != TileCollisionShape.Free)
						targetEdgeMap.AddEdge(new Point2(x, y), new Point2(x, y + 1));
					if ((collision & TileCollisionShape.Right) != TileCollisionShape.Free)
						targetEdgeMap.AddEdge(new Point2(x + 1, y), new Point2(x + 1, y + 1));
					if ((collision & TileCollisionShape.DiagonalDown) != TileCollisionShape.Free)
						targetEdgeMap.AddEdge(new Point2(x, y), new Point2(x + 1, y + 1));
					if ((collision & TileCollisionShape.DiagonalUp) != TileCollisionShape.Free)
						targetEdgeMap.AddEdge(new Point2(x, y + 1), new Point2(x + 1, y));
				}
			}
		}
		private static void AddBlockCollisionEdges(Grid<TileCollisionShape> collisionData, TileEdgeMap targetEdgeMap, Point2 edgeMapPos, Point2 totalSize)
		{
			int leftBorderPos = 0 - edgeMapPos.X;
			int rightBorderPos = totalSize.X - edgeMapPos.X;
			int topBorderPos = 0 - edgeMapPos.Y;
			int bottomBorderPos = totalSize.Y - edgeMapPos.Y;

			// Add block geometry to the specified edge map
			for (int y = 0; y < SectorSize; y++)
			{
				for (int x = 0; x < SectorSize; x++)
				{
					// Skip non-solid blocks
					bool center = (collisionData[x, y] & TileCollisionShape.Solid) == TileCollisionShape.Solid;
					if (!center) continue;

					// A filled block will always overwrite its inner diagonal edges
					targetEdgeMap.RemoveEdge(new Point2(x, y), new Point2(x + 1, y + 1));
					targetEdgeMap.RemoveEdge(new Point2(x, y + 1), new Point2(x + 1, y));

					// Determine block collision neighbourhood
					bool left   = (x == 0)              ? (x == leftBorderPos  ) : (collisionData[x - 1, y] & TileCollisionShape.Solid) == TileCollisionShape.Solid;
					bool right  = (x == SectorSize - 1) ? (x == rightBorderPos ) : (collisionData[x + 1, y] & TileCollisionShape.Solid) == TileCollisionShape.Solid;
					bool top    = (y == 0)              ? (y == topBorderPos   ) : (collisionData[x, y - 1] & TileCollisionShape.Solid) == TileCollisionShape.Solid;
					bool bottom = (y == SectorSize - 1) ? (y == bottomBorderPos) : (collisionData[x, y + 1] & TileCollisionShape.Solid) == TileCollisionShape.Solid;

					// Adjust outer edge states 
					if (center != left )  targetEdgeMap.AddEdge   (new Point2(x, y), new Point2(x, y + 1));
					else                  targetEdgeMap.RemoveEdge(new Point2(x, y), new Point2(x, y + 1));
					if (center != right)  targetEdgeMap.AddEdge   (new Point2(x + 1, y), new Point2(x + 1, y + 1));
					else                  targetEdgeMap.RemoveEdge(new Point2(x + 1, y), new Point2(x + 1, y + 1));
					if (center != top)    targetEdgeMap.AddEdge   (new Point2(x, y), new Point2(x + 1, y));
					else                  targetEdgeMap.RemoveEdge(new Point2(x, y), new Point2(x + 1, y));
					if (center != bottom) targetEdgeMap.AddEdge   (new Point2(x, y + 1), new Point2(x + 1, y + 1));
					else                  targetEdgeMap.RemoveEdge(new Point2(x, y + 1), new Point2(x + 1, y + 1));
				}
			}

			// Detect diagonal fences next to solid blocks and remove the
			// edges that might have become redundant. This can't be done
			// in the above loop without complicating control flow, so it's 
			// done here.
			for (int y = 0; y < SectorSize; y++)
			{
				for (int x = 0; x < SectorSize; x++)
				{
					TileCollisionShape centerShape = collisionData[x, y];
					bool diagonalDown = (centerShape & TileCollisionShape.DiagonalDown) == TileCollisionShape.DiagonalDown;
					bool diagonalUp = (centerShape & TileCollisionShape.DiagonalUp) == TileCollisionShape.DiagonalUp;

					// Skip tiles that aren't diagonal fences
					if (!diagonalDown && !diagonalUp) continue;

					// Determine block collision neighbourhood
					bool left   = (x == 0)              ? (x == leftBorderPos  ) : (collisionData[x - 1, y] & TileCollisionShape.Solid) == TileCollisionShape.Solid;
					bool right  = (x == SectorSize - 1) ? (x == rightBorderPos ) : (collisionData[x + 1, y] & TileCollisionShape.Solid) == TileCollisionShape.Solid;
					bool top    = (y == 0)              ? (y == topBorderPos   ) : (collisionData[x, y - 1] & TileCollisionShape.Solid) == TileCollisionShape.Solid;
					bool bottom = (y == SectorSize - 1) ? (y == bottomBorderPos) : (collisionData[x, y + 1] & TileCollisionShape.Solid) == TileCollisionShape.Solid;

					// Remove perpendicular edges that are redundant because of the diagonal fence
					// connecting two adjacent solid blocks.
					if (diagonalDown)
					{
						if (top && right)
						{
							targetEdgeMap.RemoveEdge(new Point2(x, y), new Point2(x + 1, y));
							targetEdgeMap.RemoveEdge(new Point2(x + 1, y), new Point2(x + 1, y + 1));
						}
						if (bottom && left)
						{
							targetEdgeMap.RemoveEdge(new Point2(x, y + 1), new Point2(x + 1, y + 1));
							targetEdgeMap.RemoveEdge(new Point2(x, y), new Point2(x, y + 1));
						}
					}
					else
					{
						if (top && left)
						{
							targetEdgeMap.RemoveEdge(new Point2(x, y), new Point2(x + 1, y));
							targetEdgeMap.RemoveEdge(new Point2(x, y), new Point2(x, y + 1));
						}
						if (bottom && right)
						{
							targetEdgeMap.RemoveEdge(new Point2(x, y + 1), new Point2(x + 1, y + 1));
							targetEdgeMap.RemoveEdge(new Point2(x + 1, y), new Point2(x + 1, y + 1));
						}
					}
				}
			}
		}
		private static void AddBorderCollisionEdges(TileEdgeMap targetEdgeMap, Point2 edgeMapPos, Point2 totalSize)
		{
			int rightBorderPos = totalSize.X - edgeMapPos.X;
			int bottomBorderPos = totalSize.Y - edgeMapPos.Y;

			// Top border
			if (edgeMapPos.Y == 0)
			{
				for (int x = 1; x < Math.Min(targetEdgeMap.Width, rightBorderPos + 1); x++)
					targetEdgeMap.AddEdge(new Point2(x - 1, 0), new Point2(x, 0));
			}
			// Bottom border
			if (bottomBorderPos < targetEdgeMap.Height)
			{
				for (int x = 1; x < Math.Min(targetEdgeMap.Width, rightBorderPos + 1); x++)
					targetEdgeMap.AddEdge(new Point2(x - 1, bottomBorderPos), new Point2(x, bottomBorderPos));
			}

			// Left border
			if (edgeMapPos.X == 0)
			{
				for (int y = 1; y < Math.Min(targetEdgeMap.Height, bottomBorderPos + 1); y++)
					targetEdgeMap.AddEdge(new Point2(0, y - 1), new Point2(0, y));
			}
			// Right border
			if (rightBorderPos < targetEdgeMap.Width)
			{
				for (int y = 1; y < Math.Min(targetEdgeMap.Height, bottomBorderPos + 1); y++)
					targetEdgeMap.AddEdge(new Point2(rightBorderPos, y - 1), new Point2(rightBorderPos, y));
			}
		}
		private static void GenerateCollisionShapes(TileEdgeMap edgeMap, Vector2 origin, Vector2 tileSize, bool roundedCorners, IList<ShapeInfo> shapeList)
		{
			// Traverse the edge map and gradually create chain / loop 
			// shapes until all edges have been used.
			RawList<Point2> currentChain = new RawList<Point2>();
			RawList<Vector2> vertexBuffer = new RawList<Vector2>();
			while (true)
			{
				// Begin a new continuous chain of nodes
				currentChain.Clear();

				// Find a starting node for our current chain.
				// If there is none, we found and handled all edges.
				Point2 start = edgeMap.FindNonEmpty();
				if (start == new Point2(-1, -1))
					break;

				// Traverse the current chain node-by-node from the start we found
				Point2 current = start;
				while (true)
				{
					// Add the current node to our continuous chain
					currentChain.Add(current);

					// Find the next node that connects to the current one.
					// If there is none, our current chain is done.
					Point2 next = edgeMap.GetClockwiseNextFrom(current);
					if (next == new Point2(-1, -1))
						break;

					// Remove the edge we used to get to the next node
					edgeMap.RemoveEdge(current, next);

					// Use the next node as origin for traversing further
					current = next;
				}

				// Generate a shape from the current chain
				bool isLoop = (start == currentChain[currentChain.Count - 1]);
				if (isLoop) currentChain.RemoveAt(currentChain.Count - 1);
				vertexBuffer.Clear();

				// Rounded corners
				if (roundedCorners && currentChain.Count >= 3)
				{
					vertexBuffer.Reserve(currentChain.Count * 2);
					vertexBuffer.Count = 0;
					for (int i = 0; i < currentChain.Count; i++)
					{
						int prevIndex = (i - 1 + currentChain.Count) % currentChain.Count;
						int nextIndex = (i + 1) % currentChain.Count;

						Vector2 currentVert = origin + tileSize * (Vector2)currentChain[i];
						Vector2 prevVert = origin + tileSize * (Vector2)currentChain[prevIndex];
						Vector2 nextVert = origin + tileSize * (Vector2)currentChain[nextIndex];

						if (nextVert - currentVert != currentVert - prevVert)
						{
							if (!isLoop && (i == 0 || i == currentChain.Count - 1))
							{
								vertexBuffer.Add(currentVert);
							}
							else
							{
								vertexBuffer.Add(currentVert + (prevVert - currentVert).Normalized * tileSize * 0.2f);
								vertexBuffer.Add(currentVert + (nextVert - currentVert).Normalized * tileSize * 0.2f);
							}
						}
					}
				}
				// Sharp corners
				else
				{
					vertexBuffer.Reserve(currentChain.Count);
					vertexBuffer.Count = 0;
					for (int i = 0; i < currentChain.Count; i++)
					{
						int prevIndex = (i - 1 + currentChain.Count) % currentChain.Count;
						int nextIndex = (i + 1) % currentChain.Count;

						Vector2 currentVert = origin + tileSize * (Vector2)currentChain[i];
						Vector2 prevVert = origin + tileSize * (Vector2)currentChain[prevIndex];
						Vector2 nextVert = origin + tileSize * (Vector2)currentChain[nextIndex];

						if (nextVert - currentVert != currentVert - prevVert)
							vertexBuffer.Add(currentVert);
					}
				}
				shapeList.Add(isLoop ? 
					(ShapeInfo)new LoopShapeInfo(vertexBuffer) : 
					(ShapeInfo)new ChainShapeInfo(vertexBuffer));
			}
		}
	}
}
