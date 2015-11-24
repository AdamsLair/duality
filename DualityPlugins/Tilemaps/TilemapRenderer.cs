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

		[DontSerialize] private Tilemap localTilemap = null;
		[DontSerialize] private RawList<VertexC1P3T2> vertices = null;

		
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
		public override float BoundRadius
		{
			get
			{
				Transform transform = this.GameObj.Transform;
				Tilemap tilemap = this.ActiveTilemap;
				Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
				Point2 tileCount = tilemap != null ? tilemap.TileCount : new Point2(1, 1);
				Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;
				Rect tilemapRect = Rect.Align(this.origin, 0, 0, tileCount.X * tileSize.X, tileCount.Y * tileSize.Y);
				return tilemapRect.BoundingRadius * transform.Scale;
			}
		}
		private Tilemap ActiveTilemap
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

			// Early-out, if too small to be visible
			if ((tileSize * objScale).Length <= 0.00000001f) return;

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
				float cameraRectEdgeLen = MathF.Max(device.TargetSize.X, device.TargetSize.Y);
				float localCameraRectEdgeLen = cameraRectEdgeLen * MathF.Sqrt(2) / cameraScaleAtObj;
				int maxCameraTileCount = 2 + (int)MathF.Ceiling(localCameraRectEdgeLen / (MathF.Min(tileSize.X, tileSize.Y) * this.GameObj.Transform.Scale));

				// Determine the tile indices (xy) that are visible within that rect
				tileGridStartPos = new Point2(
					viewCenterTile.X - maxCameraTileCount / 2,
					viewCenterTile.Y - maxCameraTileCount / 2);
				Point2 tileGridEndPos = new Point2(
					tileGridStartPos.X + maxCameraTileCount,
					tileGridStartPos.Y + maxCameraTileCount);
				tileGridStartPos.X = MathF.Max(tileGridStartPos.X, 0);
				tileGridStartPos.Y = MathF.Max(tileGridStartPos.Y, 0);
				tileGridEndPos.X = MathF.Min(tileGridEndPos.X, tileCount.X);
				tileGridEndPos.Y = MathF.Min(tileGridEndPos.Y, tileCount.Y);
				visibleTileCount = new Point2(
					tileGridEndPos.X - tileGridStartPos.X,
					tileGridEndPos.Y - tileGridStartPos.Y);

				// Determine start position for rendering
				renderStartPos = 
					objPos + 
					new Vector3(renderOrigin) + 
					new Vector3(tileGridStartPos.X * tileXStep + tileGridStartPos.Y * tileYStep);
			}
			int renderedTileCount = visibleTileCount.X * visibleTileCount.Y;

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

				vertexData[vertexBaseIndex + 0].Pos.X = renderPos.X;
				vertexData[vertexBaseIndex + 0].Pos.Y = renderPos.Y;
				vertexData[vertexBaseIndex + 0].Pos.Z = renderPos.Z;
				vertexData[vertexBaseIndex + 0].TexCoord.X = 0.0f;
				vertexData[vertexBaseIndex + 0].TexCoord.Y = 0.0f;
				vertexData[vertexBaseIndex + 0].Color = ColorRgba.Red;

				vertexData[vertexBaseIndex + 1].Pos.X = renderPos.X + tileYStep.X;
				vertexData[vertexBaseIndex + 1].Pos.Y = renderPos.Y + tileYStep.Y;
				vertexData[vertexBaseIndex + 1].Pos.Z = renderPos.Z;
				vertexData[vertexBaseIndex + 1].TexCoord.X = 0.0f;
				vertexData[vertexBaseIndex + 1].TexCoord.Y = 0.0f;
				vertexData[vertexBaseIndex + 1].Color = ColorRgba.Green;

				vertexData[vertexBaseIndex + 2].Pos.X = renderPos.X + tileXStep.X + tileYStep.X;
				vertexData[vertexBaseIndex + 2].Pos.Y = renderPos.Y + tileXStep.Y + tileYStep.Y;
				vertexData[vertexBaseIndex + 2].Pos.Z = renderPos.Z;
				vertexData[vertexBaseIndex + 2].TexCoord.X = 0.0f;
				vertexData[vertexBaseIndex + 2].TexCoord.Y = 0.0f;
				vertexData[vertexBaseIndex + 2].Color = ColorRgba.Blue;
				
				vertexData[vertexBaseIndex + 3].Pos.X = renderPos.X + tileXStep.X;
				vertexData[vertexBaseIndex + 3].Pos.Y = renderPos.Y + tileXStep.Y;
				vertexData[vertexBaseIndex + 3].Pos.Z = renderPos.Z;
				vertexData[vertexBaseIndex + 3].TexCoord.X = 0.0f;
				vertexData[vertexBaseIndex + 3].TexCoord.Y = 0.0f;
				vertexData[vertexBaseIndex + 3].Color = ColorRgba.Black;

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
			device.AddVertices(Material.SolidWhite, VertexMode.Quads, vertexData, this.vertices.Count);
		}
	}
}
