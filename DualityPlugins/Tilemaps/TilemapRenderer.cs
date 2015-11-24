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
			device.PreprocessCoords(ref objPos, ref objScale);

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
			int visibleTiles = tileCount.X * tileCount.Y;
			Point2 visibleTileCount = tileCount;
			Point2 tileGridStartPos = Point2.Zero;
			Vector3 renderStartPos = objPos + new Vector3(renderOrigin);

			// Reserve the required space for vertex data in our locally cached buffer
			if (this.vertices == null) this.vertices = new RawList<VertexC1P3T2>();
			this.vertices.Count = visibleTiles * 4;
			VertexC1P3T2[] vertexData = this.vertices.Data;

			// Configure vertices
			Vector3 renderPos = renderStartPos;
			Vector2 tileXStep = xAxis * tileSize.X;
			Vector2 tileYStep = yAxis * tileSize.Y;
			Point2 tileGridPos = tileGridStartPos;
			for (int tileIndex = 0; tileIndex < visibleTiles; tileIndex++)
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
				if (tileGridPos.X >= visibleTileCount.X)
				{
					tileGridPos.X = tileGridStartPos.X;
					tileGridPos.Y++;
					renderPos = renderStartPos;
					renderPos.Xy += tileYStep * tileGridPos.Y;
				}
			}

			// Submit all the vertices as one draw batch
			device.AddVertices(Material.SolidWhite, VertexMode.Quads, vertexData, this.vertices.Count);
		}
	}
}
