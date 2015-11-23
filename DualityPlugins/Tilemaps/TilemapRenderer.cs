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
				Tilemap tilemap = this.ActiveTilemap;
				Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
				Point2 tileCount = tilemap != null ? tilemap.TileCount : new Point2(1, 1);
				Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;
				Rect tilemapRect = Rect.Align(this.origin, 0, 0, tileCount.X * tileSize.X, tileCount.Y * tileSize.Y);
				return tilemapRect.BoundingRadius;
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
			Tilemap tilemap = this.ActiveTilemap;
			Tileset tileset = tilemap != null ? tilemap.Tileset.Res : null;
			Point2 tileCount = tilemap != null ? tilemap.TileCount : new Point2(1, 1);
			Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;

			Vector3 posTemp = this.GameObj.Transform.Pos;
			float scaleTemp = 1.0f;
			device.PreprocessCoords(ref posTemp, ref scaleTemp);

			Vector2 xDot, yDot;
			MathF.GetTransformDotVec(this.GameObj.Transform.Angle, scaleTemp, out xDot, out yDot);

			Rect tilemapRect = Rect.Align(this.origin, 0, 0, tileCount.X * tileSize.X, tileCount.Y * tileSize.Y);
			tilemapRect = tilemapRect.Transformed(this.GameObj.Transform.Scale, this.GameObj.Transform.Scale);
			Vector2 tilemapTopLeft = tilemapRect.TopLeft;
			Vector2 tilemapBottomLeft = tilemapRect.BottomLeft;
			Vector2 tilemapBottomRight = tilemapRect.BottomRight;
			Vector2 tilemapTopRight = tilemapRect.TopRight;

			MathF.TransformDotVec(ref tilemapTopLeft, ref xDot, ref yDot);
			MathF.TransformDotVec(ref tilemapBottomLeft, ref xDot, ref yDot);
			MathF.TransformDotVec(ref tilemapBottomRight, ref xDot, ref yDot);
			MathF.TransformDotVec(ref tilemapTopRight, ref xDot, ref yDot);

			if (this.vertices == null) this.vertices = new RawList<VertexC1P3T2>();
			this.vertices.Count = 4;
			VertexC1P3T2[] vertexData = this.vertices.Data;

			vertexData[0].Pos.X = posTemp.X + tilemapTopLeft.X;
			vertexData[0].Pos.Y = posTemp.Y + tilemapTopLeft.Y;
			vertexData[0].Pos.Z = posTemp.Z;
			vertexData[0].TexCoord.X = 0.0f;
			vertexData[0].TexCoord.Y = 0.0f;
			vertexData[0].Color = ColorRgba.Red;

			vertexData[1].Pos.X = posTemp.X + tilemapBottomLeft.X;
			vertexData[1].Pos.Y = posTemp.Y + tilemapBottomLeft.Y;
			vertexData[1].Pos.Z = posTemp.Z;
			vertexData[1].TexCoord.X = 0.0f;
			vertexData[1].TexCoord.Y = 0.0f;
			vertexData[1].Color = ColorRgba.Green;

			vertexData[2].Pos.X = posTemp.X + tilemapBottomRight.X;
			vertexData[2].Pos.Y = posTemp.Y + tilemapBottomRight.Y;
			vertexData[2].Pos.Z = posTemp.Z;
			vertexData[2].TexCoord.X = 0.0f;
			vertexData[2].TexCoord.Y = 0.0f;
			vertexData[2].Color = ColorRgba.Blue;
				
			vertexData[3].Pos.X = posTemp.X + tilemapTopRight.X;
			vertexData[3].Pos.Y = posTemp.Y + tilemapTopRight.Y;
			vertexData[3].Pos.Z = posTemp.Z;
			vertexData[3].TexCoord.X = 0.0f;
			vertexData[3].TexCoord.Y = 0.0f;
			vertexData[3].Color = ColorRgba.Black;

			device.AddVertices(Material.SolidWhite, VertexMode.Quads, vertexData, this.vertices.Count);
		}
	}
}
