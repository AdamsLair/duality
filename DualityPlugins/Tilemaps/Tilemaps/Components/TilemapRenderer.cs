using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components;
using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Renders a <see cref="Tilemap"/> that either belongs to the same <see cref="GameObject"/>
	/// or is referenced by <see cref="ExternalTilemap"/>.
	/// </summary>
	[EditorHintCategory(TilemapsResNames.CategoryTilemaps)]
	[EditorHintImage(TilemapsResNames.ImageTilemapRenderer)]
	public class TilemapRenderer : Renderer, ICmpTilemapRenderer
	{
		private Alignment           origin          = Alignment.Center;
		private Tilemap             externalTilemap = null;
		private ColorRgba           colorTint       = ColorRgba.White;
		private float               offset          = 0.0f;
		private int                 tileDepthOffset = 0;
		private float               tileDepthScale  = 0.01f;
		private TileDepthOffsetMode tileDepthMode   = TileDepthOffsetMode.Flat;

		[DontSerialize] private Tilemap localTilemap = null;
		[DontSerialize] private RawList<VertexC1P3T2> vertices = null;

		
		/// <summary>
		/// [GET] The base depth offset that will be used when rendering the <see cref="Tilemap"/>.
		/// This property represents the sum of all non-local depth adjustments in the rendered <see cref="Tilemap"/>,
		/// expressed as an offset to the depth that is implicitly defined by the <see cref="Transform"/> Z position.
		/// 
		/// This offset is automatically calculated based on <see cref="DepthOffset"/> and <see cref="TileDepthOffset"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float BaseDepthOffset
		{
			get
			{
				if (this.tileDepthMode == TileDepthOffsetMode.Flat)
					return this.offset;

				Tilemap tilemap = this.ActiveTilemap;
				Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
				Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;
				float depthPerTile = -tileSize.Y * this.GameObj.Transform.Scale * this.tileDepthScale;

				return this.offset + this.tileDepthOffset * depthPerTile;
			}
		}
		/// <summary>
		/// [GET / SET] The depth offset for the rendered <see cref="Tilemap"/> that is added
		/// to each output vertex without contributing to perspective effects such as parallax.
		/// </summary>
		public float DepthOffset
		{
			get { return this.offset; }
			set { this.offset = value; }
		}
		/// <summary>
		/// [GET / SET] An offset measured in tiles that is assumed in the depth offset generation.
		/// With an offset of one, each tile will be rendered with the base offset of "one tile higher up".
		/// 
		/// This can be used as a quick way to layer different tilemaps on each other where each layer
		/// represents a certain world space height. The same effect can be achieved by carefully adjusting 
		/// the <see cref="DepthOffset"/>.
		/// </summary>
		public int TileDepthOffset
		{
			get { return this.tileDepthOffset; }
			set { this.tileDepthOffset = value; }
		}
		/// <summary>
		/// [GET / SET] The depth offset scale that is used to determine how much depth each 
		/// tile / pixel / unit adds when using non-flat depth offset generation.
		/// </summary>
		public float TileDepthScale
		{
			get { return this.tileDepthScale; }
			set { this.tileDepthScale = value; }
		}
		/// <summary>
		/// [GET / SET] Specifies the way in which depth offsets are generated per-tile.
		/// </summary>
		public TileDepthOffsetMode TileDepthMode
		{
			get { return this.tileDepthMode; }
			set { this.tileDepthMode = value; }
		}
		/// <summary>
		/// [GET / SET] A color by which the rendered <see cref="Tilemap"/> is tinted.
		/// </summary>
		public ColorRgba ColorTint
		{
			get { return this.colorTint; }
			set { this.colorTint = value; }
		}
		/// <summary>
		/// [GET / SET] The origin of the rendered <see cref="Tilemap"/> as a whole, relative to the position of its <see cref="GameObject"/>.
		/// </summary>
		public Alignment Origin
		{
			get { return this.origin; }
			set { this.origin = value; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="Tilemap"/> that should be rendered. 
		/// If this is null, the local <see cref="Tilemap"/> on the same <see cref="GameObject"/> is used.
		/// </summary>
		public Tilemap ExternalTilemap
		{
			get { return this.externalTilemap; }
			set { this.externalTilemap = value; }
		}
		/// <summary>
		/// [GET] A reference to the <see cref="Tilemap"/> that is currently rendered by this <see cref="TilemapRenderer"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Tilemap ActiveTilemap
		{
			get 
			{
				if (this.externalTilemap != null)
				{
					return this.externalTilemap;
				}
				else
				{
					if (this.localTilemap == null || this.localTilemap.GameObj != this.GameObj)
						this.localTilemap = this.GameObj.GetComponent<Tilemap>();
					return this.localTilemap;
				}
			}
		}
		/// <summary>
		/// [GET] The rectangular region that is occupied by the rendered <see cref="Tilemap"/>, in local / object space.
		/// </summary>
		public Rect LocalTilemapRect
		{
			get
			{
				Tilemap tilemap = this.ActiveTilemap;
				Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
				Point2 tileCount = tilemap != null ? tilemap.Size : Point2.Zero;
				Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;
				return Rect.Align(this.origin, 0, 0, tileCount.X * tileSize.X, tileCount.Y * tileSize.Y);
			}
		}
		public override float BoundRadius
		{
			get
			{
				Transform transform = this.GameObj.Transform;
				Rect tilemapRect = this.LocalTilemapRect;
				return tilemapRect.BoundingRadius * transform.Scale;
			}
		}


		/// <summary>
		/// Given the specified coordinate in local / object space, this method returns the
		/// tile index that is located there.
		/// </summary>
		/// <param name="localPos"></param>
		/// <param name="pickMode">Specifies the desired behavior when attempting to get a tile outside the rendered area.</param>
		/// <returns></returns>
		public Point2 GetTileAtLocalPos(Vector2 localPos, TilePickMode pickMode)
		{
			// Early-out, if the specified local position is not within the tilemap rect
			Rect localRect = this.LocalTilemapRect;
			if (pickMode == TilePickMode.Reject && !localRect.Contains(localPos))
				return new Point2(-1, -1);

			Tilemap tilemap = this.ActiveTilemap;
			Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
			Point2 tileCount = tilemap != null ? tilemap.Size : Point2.Zero;
			Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;

			// Determine the tile index at the specified local position
			Point2 tileIndex = new Point2(
				(int)MathF.Floor((localPos.X - localRect.X) / tileSize.X),
				(int)MathF.Floor((localPos.Y - localRect.Y) / tileSize.Y));

			// Clamp or reject the tile index when required
			if (pickMode != TilePickMode.Free)
			{
				if (tileCount.X <= 0 || tileCount.Y <= 0)
					return new Point2(-1, -1);

				tileIndex = new Point2(
					MathF.Clamp(tileIndex.X, 0, tileCount.X - 1),
					MathF.Clamp(tileIndex.Y, 0, tileCount.Y - 1));
			}

			return tileIndex;
		}
		/// <summary>
		/// Determines the generated depth offset for the tile at the specified tile coordinates.
		/// This also inclues the renderers overall offset as specified in <see cref="DepthOffset"/>,
		/// but ignores all actual per-tile and tileset depth offsets. The specified tile position
		/// is considered virtual and does not have to be within the valid tile position range.
		/// </summary>
		/// <param name="tilePos"></param>
		/// <returns></returns>
		public float GetTileDepthOffsetAt(Point2 tilePos)
		{
			if (this.tileDepthMode == TileDepthOffsetMode.Flat)
				return this.offset;

			Tilemap tilemap = this.ActiveTilemap;
			Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
			Point2 tileCount = tilemap != null ? tilemap.Size : new Point2(1, 1);
			Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;

			float depthPerTile = -tileSize.Y * this.GameObj.Transform.Scale * this.tileDepthScale;
			float originDepthOffset = Rect.Align(this.origin, 0, 0, 0, tileCount.Y * depthPerTile).Y;

			if (this.tileDepthMode == TileDepthOffsetMode.World)
				originDepthOffset += (this.GameObj.Transform.Pos.Y / (float)tileSize.Y) * depthPerTile;

			return this.offset + this.tileDepthOffset * depthPerTile + originDepthOffset + tilePos.Y * depthPerTile;
		}

		public override void Draw(IDrawDevice device)
		{
			// Determine basic working data
			Tilemap tilemap = this.ActiveTilemap;
			Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
			Point2 tileCount = tilemap != null ? tilemap.Size : new Point2(1, 1);
			Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;

			// Early-out, if insufficient
			if (tilemap == null) return;
			if (tileset == null) return;

			// Determine the total size and origin of the rendered Tilemap
			Vector2 renderTotalSize = tileCount * tileSize;
			Vector2 renderOrigin = Vector2.Zero;
			this.origin.ApplyTo(ref renderOrigin, ref renderTotalSize);
			MathF.TransformCoord(ref renderOrigin.X, ref renderOrigin.Y, this.GameObj.Transform.Angle, this.GameObj.Transform.Scale);

			// Determine Tile visibility
			TilemapCulling.TileInput cullingIn = new TilemapCulling.TileInput
			{
				// Remember: All these transform values are in world space
				TilemapPos = this.GameObj.Transform.Pos + new Vector3(renderOrigin),
				TilemapScale = this.GameObj.Transform.Scale,
				TilemapAngle = this.GameObj.Transform.Angle,
				TileCount = tileCount,
				TileSize = tileSize
			};
			TilemapCulling.TileOutput cullingOut = TilemapCulling.GetVisibleTileRect(device, cullingIn);
			int renderedTileCount = cullingOut.VisibleTileCount.X * cullingOut.VisibleTileCount.Y;

			// Determine rendering parameters
			Material material = (tileset != null ? tileset.RenderMaterial : null) ?? Material.Checkerboard.Res;
			ColorRgba mainColor = material.MainColor * this.colorTint;

			// Reserve the required space for vertex data in our locally cached buffer
			if (this.vertices == null) this.vertices = new RawList<VertexC1P3T2>();
			this.vertices.Count = renderedTileCount * 4;
			VertexC1P3T2[] vertexData = this.vertices.Data;
			
			// Determine and adjust data for Z offset generation
			float depthPerTile = -cullingIn.TileSize.Y * cullingIn.TilemapScale * this.tileDepthScale;

			if (this.tileDepthMode == TileDepthOffsetMode.Flat)
				depthPerTile = 0.0f;

			float originDepthOffset = Rect.Align(this.origin, 0, 0, 0, tileCount.Y * depthPerTile).Y;

			if (this.tileDepthMode == TileDepthOffsetMode.World)
				originDepthOffset += (this.GameObj.Transform.Pos.Y / (float)tileSize.Y) * depthPerTile;

			cullingOut.RenderOriginView.Z += this.offset + this.tileDepthOffset * depthPerTile + originDepthOffset;

			// Prepare vertex generation data
			Vector2 tileXStep = cullingOut.XAxisView * cullingIn.TileSize.X;
			Vector2 tileYStep = cullingOut.YAxisView * cullingIn.TileSize.Y;
			Vector3 renderPos = cullingOut.RenderOriginView;
			Point2 tileGridPos = cullingOut.VisibleTileStart;

			// Prepare vertex data array for batch-submitting
			IReadOnlyGrid<Tile> tiles = tilemap.Tiles;
			TileInfo[] tileData = tileset.TileData.Data;
			int submittedTileCount = 0;
			int vertexBaseIndex = 0;
			for (int tileIndex = 0; tileIndex < renderedTileCount; tileIndex++)
			{
				Tile tile = tiles[tileGridPos.X, tileGridPos.Y];
				if (tile.Index < tileData.Length)
				{
					Rect uvRect = tileData[tile.Index].TexCoord0;
					bool visualEmpty = tileData[tile.Index].IsVisuallyEmpty;
					int tileBaseOffset = tileData[tile.Index].DepthOffset;
					float localDepthOffset = (tile.DepthOffset + tileBaseOffset) * depthPerTile;

					if (!visualEmpty)
					{
						vertexData[vertexBaseIndex + 0].Pos.X = renderPos.X;
						vertexData[vertexBaseIndex + 0].Pos.Y = renderPos.Y;
						vertexData[vertexBaseIndex + 0].Pos.Z = renderPos.Z + localDepthOffset;
						vertexData[vertexBaseIndex + 0].TexCoord.X = uvRect.X;
						vertexData[vertexBaseIndex + 0].TexCoord.Y = uvRect.Y;
						vertexData[vertexBaseIndex + 0].Color = mainColor;

						vertexData[vertexBaseIndex + 1].Pos.X = renderPos.X + tileYStep.X;
						vertexData[vertexBaseIndex + 1].Pos.Y = renderPos.Y + tileYStep.Y;
						vertexData[vertexBaseIndex + 1].Pos.Z = renderPos.Z + localDepthOffset + depthPerTile;
						vertexData[vertexBaseIndex + 1].TexCoord.X = uvRect.X;
						vertexData[vertexBaseIndex + 1].TexCoord.Y = uvRect.Y + uvRect.H;
						vertexData[vertexBaseIndex + 1].Color = mainColor;

						vertexData[vertexBaseIndex + 2].Pos.X = renderPos.X + tileXStep.X + tileYStep.X;
						vertexData[vertexBaseIndex + 2].Pos.Y = renderPos.Y + tileXStep.Y + tileYStep.Y;
						vertexData[vertexBaseIndex + 2].Pos.Z = renderPos.Z + localDepthOffset + depthPerTile;
						vertexData[vertexBaseIndex + 2].TexCoord.X = uvRect.X + uvRect.W;
						vertexData[vertexBaseIndex + 2].TexCoord.Y = uvRect.Y + uvRect.H;
						vertexData[vertexBaseIndex + 2].Color = mainColor;
				
						vertexData[vertexBaseIndex + 3].Pos.X = renderPos.X + tileXStep.X;
						vertexData[vertexBaseIndex + 3].Pos.Y = renderPos.Y + tileXStep.Y;
						vertexData[vertexBaseIndex + 3].Pos.Z = renderPos.Z + localDepthOffset;
						vertexData[vertexBaseIndex + 3].TexCoord.X = uvRect.X + uvRect.W;
						vertexData[vertexBaseIndex + 3].TexCoord.Y = uvRect.Y;
						vertexData[vertexBaseIndex + 3].Color = mainColor;

						bool vertical = tileData[tile.Index].IsVertical;
						if (vertical)
						{
							vertexData[vertexBaseIndex + 0].Pos.Z += depthPerTile;
							vertexData[vertexBaseIndex + 3].Pos.Z += depthPerTile;
						}

						submittedTileCount++;
						vertexBaseIndex += 4;
					}
				}

				tileGridPos.X++;
				renderPos.X += tileXStep.X;
				renderPos.Y += tileXStep.Y;
				if ((tileGridPos.X - cullingOut.VisibleTileStart.X) >= cullingOut.VisibleTileCount.X)
				{
					tileGridPos.X = cullingOut.VisibleTileStart.X;
					tileGridPos.Y++;
					renderPos = cullingOut.RenderOriginView;
					renderPos.X += tileYStep.X * (tileGridPos.Y - cullingOut.VisibleTileStart.Y);
					renderPos.Y += tileYStep.Y * (tileGridPos.Y - cullingOut.VisibleTileStart.Y);
					renderPos.Z += tileGridPos.Y * depthPerTile;
				}
			}

			// Submit all the vertices as one draw batch
			device.AddVertices(
				material,
				VertexMode.Quads, 
				vertexData, 
				submittedTileCount * 4);

			Profile.AddToStat(@"Duality\Stats\Render\Tilemaps\NumTiles", renderedTileCount);
			Profile.AddToStat(@"Duality\Stats\Render\Tilemaps\NumVertices", submittedTileCount * 4);
		}
	}
}
