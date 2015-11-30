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
	public class TilemapRenderer : Renderer
	{
		private Alignment origin          = Alignment.Center;
		private Tilemap   externalTilemap = null;
		private ColorRgba colorTint       = ColorRgba.White;

		[DontSerialize] private Tilemap localTilemap = null;
		[DontSerialize] private RawList<VertexC1P3T2> vertices = null;

		
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
				Point2 tileCount = tilemap != null ? tilemap.TileCount : Point2.Zero;
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
		/// <returns></returns>
		public Point2 GetTileAtLocalPos(Vector2 localPos)
		{
			// Early-out, if the specified local position is not within the tilemap rect
			Rect localRect = this.LocalTilemapRect;
			if (!localRect.Contains(localPos))
				return new Point2(-1, -1);

			Tilemap tilemap = this.ActiveTilemap;
			Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
			Point2 tileCount = tilemap != null ? tilemap.TileCount : Point2.Zero;
			Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;

			// Early-out, if the rendered tilemap is empty
			if (tileCount.X <= 0 || tileCount.Y <= 0)
				return new Point2(-1, -1);

			// Determine the tile index at the specified local position
			return new Point2(
				MathF.Clamp((int)MathF.Floor((localPos.X - localRect.X) / tileSize.X), 0, tileCount.X - 1),
				MathF.Clamp((int)MathF.Floor((localPos.Y - localRect.Y) / tileSize.Y), 0, tileCount.Y - 1));
		}

		public override void Draw(IDrawDevice device)
		{
			// Determine basic working data
			Tilemap tilemap = this.ActiveTilemap;
			Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
			Point2 tileCount = tilemap != null ? tilemap.TileCount : new Point2(1, 1);
			Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;

			// Early-out, if insufficient
			if (tilemap == null) return;

			// Account for Camera-relative coordinates (view space) and parallax effect / Z depth
			Vector3 objPos = this.GameObj.Transform.Pos;
			float objScale = this.GameObj.Transform.Scale;
			float cameraScaleAtObj = 1.0f;
			device.PreprocessCoords(ref objPos, ref cameraScaleAtObj);
			objScale *= cameraScaleAtObj;

			// Early-out, if so small that it might break the math behind rendering a single tile.
			if (objScale <= 0.000000001f) return;

			// Determine transformed X and Y axis in view space
			Vector2 xAxis = Vector2.UnitX;
			Vector2 yAxis = Vector2.UnitY;
			MathF.TransformCoord(ref xAxis.X, ref xAxis.Y, this.GameObj.Transform.Angle, objScale);
			MathF.TransformCoord(ref yAxis.X, ref yAxis.Y, this.GameObj.Transform.Angle, objScale);

			// Determine the total size and origin of the rendered Tilemap
			Vector2 renderTotalSize = tileCount * tileSize;
			Vector2 renderOrigin = Vector2.Zero;
			this.origin.ApplyTo(ref renderOrigin, ref renderTotalSize);

			// Transform rendering origin into world space
			renderOrigin = renderOrigin.X * xAxis + renderOrigin.Y * yAxis;
			renderTotalSize *= objScale;

			// Determine Tile visibility
			Vector2 tileXStep = xAxis * tileSize.X;
			Vector2 tileYStep = yAxis * tileSize.Y;
			Point2 visibleTileCount;
			Point2 tileGridStartPos;
			Vector3 renderStartPos;
			{
				// Determine which tile is in the center of view space.
				Point2 viewCenterTile = Point2.Zero;
				{
					// Project the view center coordinate (view space zero) into the local space of the rendered tilemap
					Vector2 viewRenderStartPos = objPos.Xy + renderOrigin;
					Vector2 localViewCenter = Vector2.Zero - viewRenderStartPos;
					localViewCenter = new Vector2(
						Vector2.Dot(localViewCenter, xAxis.Normalized),
						Vector2.Dot(localViewCenter, yAxis.Normalized)) / objScale;
					viewCenterTile = new Point2(
						(int)MathF.Floor(localViewCenter.X / tileSize.X),
						(int)MathF.Floor(localViewCenter.Y / tileSize.Y));
				}

				// Determine the edge length of a square that is big enough to enclose the world space rect of the Camera view
				float visualAngle = this.GameObj.Transform.Angle - device.RefAngle;
				Vector2 visualBounds = new Vector2(
					device.TargetSize.Y * MathF.Abs(MathF.Sin(visualAngle)) + device.TargetSize.X * MathF.Abs(MathF.Cos(visualAngle)),
					device.TargetSize.X * MathF.Abs(MathF.Sin(visualAngle)) + device.TargetSize.Y * MathF.Abs(MathF.Cos(visualAngle)));
				Vector2 localVisualBounds = visualBounds / cameraScaleAtObj;
				Point2 targetVisibleTileCount = new Point2(
					2 + (int)MathF.Ceiling(localVisualBounds.X / (MathF.Min(tileSize.X, tileSize.Y) * this.GameObj.Transform.Scale)), 
					2 + (int)MathF.Ceiling(localVisualBounds.Y / (MathF.Min(tileSize.X, tileSize.Y) * this.GameObj.Transform.Scale)));

				// Determine the tile indices (xy) that are visible within that rect
				tileGridStartPos = new Point2(
					MathF.Max(viewCenterTile.X - targetVisibleTileCount.X / 2, 0),
					MathF.Max(viewCenterTile.Y - targetVisibleTileCount.Y / 2, 0));
				Point2 tileGridEndPos = new Point2(
					MathF.Min(tileGridStartPos.X + targetVisibleTileCount.X, tileCount.X),
					MathF.Min(tileGridStartPos.Y + targetVisibleTileCount.Y, tileCount.Y));
				visibleTileCount = new Point2(
					MathF.Clamp(tileGridEndPos.X - tileGridStartPos.X, 0, tileCount.X),
					MathF.Clamp(tileGridEndPos.Y - tileGridStartPos.Y, 0, tileCount.Y));

				// Determine start position for rendering
				renderStartPos = 
					objPos + 
					new Vector3(renderOrigin) + 
					new Vector3(tileGridStartPos.X * tileXStep + tileGridStartPos.Y * tileYStep);
			}
			int renderedTileCount = visibleTileCount.X * visibleTileCount.Y;

			// Determine rendering parameters
			Material material = (tileset != null ? tileset.RenderMaterial : null) ?? Material.Checkerboard.Res;
			ColorRgba mainColor = material.MainColor * this.colorTint;

			// Reserve the required space for vertex data in our locally cached buffer
			if (this.vertices == null) this.vertices = new RawList<VertexC1P3T2>();
			this.vertices.Count = renderedTileCount * 4;
			VertexC1P3T2[] vertexData = this.vertices.Data;

			// Configure vertices
			Vector3 renderPos = renderStartPos;
			Point2 tileGridPos = tileGridStartPos;
			for (int tileIndex = 0; tileIndex < renderedTileCount; tileIndex++)
			{
				int vertexBaseIndex = tileIndex * 4;
				Tile tile = tilemap.Tiles[tileGridPos.X, tileGridPos.Y];

				Rect uv;
				tileset.LookupTileAtlas(0, tile.Index, out uv);

				vertexData[vertexBaseIndex + 0].Pos.X = renderPos.X;
				vertexData[vertexBaseIndex + 0].Pos.Y = renderPos.Y;
				vertexData[vertexBaseIndex + 0].Pos.Z = renderPos.Z;
				vertexData[vertexBaseIndex + 0].TexCoord.X = uv.X;
				vertexData[vertexBaseIndex + 0].TexCoord.Y = uv.Y;
				vertexData[vertexBaseIndex + 0].Color = mainColor;

				vertexData[vertexBaseIndex + 1].Pos.X = renderPos.X + tileYStep.X;
				vertexData[vertexBaseIndex + 1].Pos.Y = renderPos.Y + tileYStep.Y;
				vertexData[vertexBaseIndex + 1].Pos.Z = renderPos.Z;
				vertexData[vertexBaseIndex + 1].TexCoord.X = uv.X;
				vertexData[vertexBaseIndex + 1].TexCoord.Y = uv.Y + uv.H;
				vertexData[vertexBaseIndex + 1].Color = mainColor;

				vertexData[vertexBaseIndex + 2].Pos.X = renderPos.X + tileXStep.X + tileYStep.X;
				vertexData[vertexBaseIndex + 2].Pos.Y = renderPos.Y + tileXStep.Y + tileYStep.Y;
				vertexData[vertexBaseIndex + 2].Pos.Z = renderPos.Z;
				vertexData[vertexBaseIndex + 2].TexCoord.X = uv.X + uv.W;
				vertexData[vertexBaseIndex + 2].TexCoord.Y = uv.Y + uv.H;
				vertexData[vertexBaseIndex + 2].Color = mainColor;
				
				vertexData[vertexBaseIndex + 3].Pos.X = renderPos.X + tileXStep.X;
				vertexData[vertexBaseIndex + 3].Pos.Y = renderPos.Y + tileXStep.Y;
				vertexData[vertexBaseIndex + 3].Pos.Z = renderPos.Z;
				vertexData[vertexBaseIndex + 3].TexCoord.X = uv.X + uv.W;
				vertexData[vertexBaseIndex + 3].TexCoord.Y = uv.Y;
				vertexData[vertexBaseIndex + 3].Color = mainColor;

				tileGridPos.X++;
				renderPos.Xy += tileXStep;
				if ((tileGridPos.X - tileGridStartPos.X) >= visibleTileCount.X)
				{
					tileGridPos.X = tileGridStartPos.X;
					tileGridPos.Y++;
					renderPos = renderStartPos;
					renderPos.Xy += tileYStep * (tileGridPos.Y - tileGridStartPos.Y);
				}
			}

			// Submit all the vertices as one draw batch
			device.AddVertices(
				material,
				VertexMode.Quads, 
				vertexData, this.vertices.Count);

			Profile.AddToStat(@"Duality\Stats\Render\Tilemaps\NumTiles", renderedTileCount);
			Profile.AddToStat(@"Duality\Stats\Render\Tilemaps\NumVertices", this.vertices.Count);
		}
	}
}
