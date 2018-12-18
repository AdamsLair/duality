using System;

using Duality;
using Duality.Editor;
using Duality.Components.Renderers;
using Duality.Drawing;
using Duality.Resources;
using Duality.Properties;
using DynamicLighting.Properties;

namespace DynamicLighting
{
	/// <summary>
	/// Renders a sprite using dynamic lighting, either per-vertex or per-pixel, depending on the DrawTechnique that is used.
	/// </summary>
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(DynLightResNames.IconComponentLightingSpriteRenderer)]
	public class LightingSpriteRenderer : SpriteRenderer
	{
		private float vertexTranslucency = 0.0f;
		[DontSerialize] private VertexDynamicLighting[] verticesLight = null;

		/// <summary>
		/// [GET / SET] Specifies the objects translucency for Light when using vertex lighting.
		/// A very translucent object (1.0) is affected from Lights behind it as well as from Lights in front of it.
		/// Non-translucent objects (0.0) are only affected by Lights in front of them.
		/// </summary>
		[EditorHintIncrement(0.1f)]
		[EditorHintRange(0.0f, 1.0f)]
		public float VertexTranslucency
		{
			get { return this.vertexTranslucency; }
			set { this.vertexTranslucency = value; }
		}

		protected void PrepareVerticesLight(ref VertexDynamicLighting[] vertices, IDrawDevice device, ColorRgba mainClr, Rect uvRect, Vector2 pivot, DrawTechnique tech)
		{
			bool perPixel = tech is LightingTechnique;

			Vector3 pos = this.GameObj.Transform.Pos;
			pos.X -= pivot.X;
			pos.Y -= pivot.Y;

			Vector2 xDot, yDot;
			float rotation = this.GameObj.Transform.Angle;
			MathF.GetTransformDotVec(rotation, out xDot, out yDot);

			Rect rectTemp = this.rect.Transformed(this.GameObj.Transform.Scale, this.GameObj.Transform.Scale);
			Vector2 edge1 = rectTemp.TopLeft;
			Vector2 edge2 = rectTemp.BottomLeft;
			Vector2 edge3 = rectTemp.BottomRight;
			Vector2 edge4 = rectTemp.TopRight;

			MathF.TransformDotVec(ref edge1, ref xDot, ref yDot);
			MathF.TransformDotVec(ref edge2, ref xDot, ref yDot);
			MathF.TransformDotVec(ref edge3, ref xDot, ref yDot);
			MathF.TransformDotVec(ref edge4, ref xDot, ref yDot);

			// Using Per-Vertex Lighting? Calculate vertex light values
			Vector4[] vertexLight = null;
			if (!perPixel)
			{
				vertexLight = new Vector4[4];
				Light.GetLightAtWorldPos(pos + new Vector3(edge1), out vertexLight[0], this.vertexTranslucency);
				Light.GetLightAtWorldPos(pos + new Vector3(edge2), out vertexLight[1], this.vertexTranslucency);
				Light.GetLightAtWorldPos(pos + new Vector3(edge3), out vertexLight[2], this.vertexTranslucency);
				Light.GetLightAtWorldPos(pos + new Vector3(edge4), out vertexLight[3], this.vertexTranslucency);
			}

			// Using Per-Pixel Lighting? Pass objRotation Matrix via vertex attribute.
			Vector4 objRotMat = Vector4.Zero;
			if (perPixel)
				objRotMat = new Vector4((float)Math.Cos(-rotation), -(float)Math.Sin(-rotation), (float)Math.Sin(-rotation), (float)Math.Cos(-rotation));
			
			// Calculate UV coordinates
			float left   = uvRect.X;
			float right  = uvRect.RightX;
			float top    = uvRect.Y;
			float bottom = uvRect.BottomY;

			if ((this.flipMode & FlipMode.Horizontal) != FlipMode.None)
				MathF.Swap(ref left, ref right);
			if ((this.flipMode & FlipMode.Vertical) != FlipMode.None)
				MathF.Swap(ref top, ref bottom);

			if (vertices == null || vertices.Length != 4) vertices = new VertexDynamicLighting[4];

			// Directly pass World Position with each vertex, see note in Light.cs
			vertices[0].Pos.X = pos.X + edge1.X;
			vertices[0].Pos.Y = pos.Y + edge1.Y;
			vertices[0].Pos.Z = pos.Z;
			vertices[0].DepthOffset = this.offset;
			vertices[0].TexCoord.X = left;
			vertices[0].TexCoord.Y = top;
			vertices[0].Color = mainClr;
			vertices[0].LightingParam = perPixel ? objRotMat : vertexLight[0];

			vertices[1].Pos.X = pos.X + edge2.X;
			vertices[1].Pos.Y = pos.Y + edge2.Y;
			vertices[1].Pos.Z = pos.Z;
			vertices[1].DepthOffset = this.offset;
			vertices[1].TexCoord.X = left;
			vertices[1].TexCoord.Y = bottom;
			vertices[1].Color = mainClr;
			vertices[1].LightingParam = perPixel ? objRotMat : vertexLight[1];

			vertices[2].Pos.X = pos.X + edge3.X;
			vertices[2].Pos.Y = pos.Y + edge3.Y;
			vertices[2].Pos.Z = pos.Z;
			vertices[2].DepthOffset = this.offset;
			vertices[2].TexCoord.X = right;
			vertices[2].TexCoord.Y = bottom;
			vertices[2].Color = mainClr;
			vertices[2].LightingParam = perPixel ? objRotMat : vertexLight[2];
				
			vertices[3].Pos.X = pos.X + edge4.X;
			vertices[3].Pos.Y = pos.Y + edge4.Y;
			vertices[3].Pos.Z = pos.Z;
			vertices[3].DepthOffset = this.offset;
			vertices[3].TexCoord.X = right;
			vertices[3].TexCoord.Y = top;
			vertices[3].Color = mainClr;
			vertices[3].LightingParam = perPixel ? objRotMat : vertexLight[3];
			
			if (this.pixelGrid)
			{
				vertices[0].Pos.X = MathF.Round(vertices[0].Pos.X);
				vertices[1].Pos.X = MathF.Round(vertices[1].Pos.X);
				vertices[2].Pos.X = MathF.Round(vertices[2].Pos.X);
				vertices[3].Pos.X = MathF.Round(vertices[3].Pos.X);

				if (MathF.RoundToInt(device.TargetSize.X) != (MathF.RoundToInt(device.TargetSize.X) / 2) * 2)
				{
					vertices[0].Pos.X += 0.5f;
					vertices[1].Pos.X += 0.5f;
					vertices[2].Pos.X += 0.5f;
					vertices[3].Pos.X += 0.5f;
				}

				vertices[0].Pos.Y = MathF.Round(vertices[0].Pos.Y);
				vertices[1].Pos.Y = MathF.Round(vertices[1].Pos.Y);
				vertices[2].Pos.Y = MathF.Round(vertices[2].Pos.Y);
				vertices[3].Pos.Y = MathF.Round(vertices[3].Pos.Y);

				if (MathF.RoundToInt(device.TargetSize.Y) != (MathF.RoundToInt(device.TargetSize.Y) / 2) * 2)
				{
					vertices[0].Pos.Y += 0.5f;
					vertices[1].Pos.Y += 0.5f;
					vertices[2].Pos.Y += 0.5f;
					vertices[3].Pos.Y += 0.5f;
				}
			}
		}

		public override void Draw(IDrawDevice device)
		{
			// When rendering a sprite that uses dynamic lighting, make sure to update
			// the devices shared / global lighting variables this frame
			Light.UpdateLighting(device);

			Texture mainTex = this.RetrieveMainTex();
			DrawTechnique tech = this.RetrieveDrawTechnique();

			Rect uvRect;
			Vector2 pivot;
			this.GetRectData(mainTex, this.spriteIndex, out uvRect, out pivot);
			this.PrepareVerticesLight(ref this.verticesLight, device, this.colorTint, uvRect, pivot, tech);

			if (this.customMat != null)
				device.AddVertices(this.customMat, VertexMode.Quads, this.verticesLight);
			else
				device.AddVertices(this.sharedMat, VertexMode.Quads, this.verticesLight);
		}
	}
}
