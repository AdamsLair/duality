using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using Duality;
using Duality.EditorHints;
using Duality.Components.Renderers;
using Duality.ColorFormat;
using Duality.Resources;

namespace DynamicLighting
{
	/// <summary>
	/// Renders an animated sprite using dynamic lighting, either per-vertex or per-pixel, depending on the DrawTechnique that is used.
	/// </summary>
	[Serializable]
	public class LightingAnimSpriteRenderer : AnimSpriteRenderer
	{
		private	float	vertexTranslucency	= 0.0f;
		[NonSerialized]	private	VertexC1P3T2A4[]	verticesLight		= null;
		[NonSerialized]	private	VertexC1P3T4A4A1[]	verticesLightSmooth	= null;

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

		protected void PrepareVerticesLightSmooth(ref VertexC1P3T4A4A1[] vertices, IDrawDevice device, float curAnimFrameFade, ColorRgba mainClr, Rect uvRect, Rect uvRectNext, DrawTechnique tech)
		{
			bool perPixel = tech is LightingTechnique;

			Vector3 pos = this.GameObj.Transform.Pos;
			Vector3 posTemp = pos;
			float scaleTemp = 1.0f;
			device.PreprocessCoords(ref posTemp, ref scaleTemp);

			Vector2 xDot, yDot;
			float rotation = this.GameObj.Transform.Angle;
			MathF.GetTransformDotVec(rotation, out xDot, out yDot);

			Rect rectTemp = this.rect.Transform(this.GameObj.Transform.Scale, this.GameObj.Transform.Scale);
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

			Vector2.Multiply(ref edge1, scaleTemp, out edge1);
			Vector2.Multiply(ref edge2, scaleTemp, out edge2);
			Vector2.Multiply(ref edge3, scaleTemp, out edge3);
			Vector2.Multiply(ref edge4, scaleTemp, out edge4);

			// Using Per-Pixel Lighting? Pass objRotation Matrix via vertex attribute.
			Vector4 objRotMat = Vector4.Zero;
			if (perPixel)
				objRotMat = new Vector4((float)Math.Cos(-rotation), -(float)Math.Sin(-rotation), (float)Math.Sin(-rotation), (float)Math.Cos(-rotation));

			if (vertices == null || vertices.Length != 4) vertices = new VertexC1P3T4A4A1[4];

			// Directly pass World Position with each vertex, see note in Light.cs
			vertices[0].Pos.X = posTemp.X + edge1.X;
			vertices[0].Pos.Y = posTemp.Y + edge1.Y;
			vertices[0].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[0].TexCoord.X = uvRect.X;
			vertices[0].TexCoord.Y = uvRect.Y;
			vertices[0].TexCoord.Z = uvRectNext.X;
			vertices[0].TexCoord.W = uvRectNext.Y;
			vertices[0].Color = mainClr;
			vertices[0].Attrib = perPixel ? objRotMat : vertexLight[0];
			vertices[0].Attrib2 = curAnimFrameFade;

			vertices[1].Pos.X = posTemp.X + edge2.X;
			vertices[1].Pos.Y = posTemp.Y + edge2.Y;
			vertices[1].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[1].TexCoord.X = uvRect.X;
			vertices[1].TexCoord.Y = uvRect.MaximumY;
			vertices[1].TexCoord.Z = uvRectNext.X;
			vertices[1].TexCoord.W = uvRectNext.MaximumY;
			vertices[1].Color = mainClr;
			vertices[1].Attrib = perPixel ? objRotMat : vertexLight[1];
			vertices[1].Attrib2 = curAnimFrameFade;

			vertices[2].Pos.X = posTemp.X + edge3.X;
			vertices[2].Pos.Y = posTemp.Y + edge3.Y;
			vertices[2].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[2].TexCoord.X = uvRect.MaximumX;
			vertices[2].TexCoord.Y = uvRect.MaximumY;
			vertices[2].TexCoord.Z = uvRectNext.MaximumX;
			vertices[2].TexCoord.W = uvRectNext.MaximumY;
			vertices[2].Color = mainClr;
			vertices[2].Attrib = perPixel ? objRotMat : vertexLight[2];
			vertices[2].Attrib2 = curAnimFrameFade;
				
			vertices[3].Pos.X = posTemp.X + edge4.X;
			vertices[3].Pos.Y = posTemp.Y + edge4.Y;
			vertices[3].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[3].TexCoord.X = uvRect.MaximumX;
			vertices[3].TexCoord.Y = uvRect.Y;
			vertices[3].TexCoord.Z = uvRectNext.MaximumX;
			vertices[3].TexCoord.W = uvRectNext.Y;
			vertices[3].Color = mainClr;
			vertices[3].Attrib = perPixel ? objRotMat : vertexLight[3];
			vertices[3].Attrib2 = curAnimFrameFade;
			
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
		protected void PrepareVerticesLight(ref VertexC1P3T2A4[] vertices, IDrawDevice device, ColorRgba mainClr, Rect uvRect, DrawTechnique tech)
		{
			bool perPixel = tech is LightingTechnique;

			Vector3 pos = this.GameObj.Transform.Pos;
			Vector3 posTemp = pos;
			float scaleTemp = 1.0f;
			device.PreprocessCoords(ref posTemp, ref scaleTemp);

			Vector2 xDot, yDot;
			float rotation = this.GameObj.Transform.Angle;
			MathF.GetTransformDotVec(rotation, out xDot, out yDot);

			Rect rectTemp = this.rect.Transform(this.GameObj.Transform.Scale, this.GameObj.Transform.Scale);
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

			Vector2.Multiply(ref edge1, scaleTemp, out edge1);
			Vector2.Multiply(ref edge2, scaleTemp, out edge2);
			Vector2.Multiply(ref edge3, scaleTemp, out edge3);
			Vector2.Multiply(ref edge4, scaleTemp, out edge4);

			// Using Per-Pixel Lighting? Pass objRotation Matrix via vertex attribute.
			Vector4 objRotMat = Vector4.Zero;
			if (perPixel)
				objRotMat = new Vector4((float)Math.Cos(-rotation), -(float)Math.Sin(-rotation), (float)Math.Sin(-rotation), (float)Math.Cos(-rotation));

			if (vertices == null || vertices.Length != 4) vertices = new VertexC1P3T2A4[4];

			vertices[0].Pos.X = posTemp.X + edge1.X;
			vertices[0].Pos.Y = posTemp.Y + edge1.Y;
			vertices[0].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[0].TexCoord.X = uvRect.X;
			vertices[0].TexCoord.Y = uvRect.Y;
			vertices[0].Color = mainClr;
			vertices[0].Attrib = perPixel ? objRotMat : vertexLight[0];

			vertices[1].Pos.X = posTemp.X + edge2.X;
			vertices[1].Pos.Y = posTemp.Y + edge2.Y;
			vertices[1].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[1].TexCoord.X = uvRect.X;
			vertices[1].TexCoord.Y = uvRect.MaximumY;
			vertices[1].Color = mainClr;
			vertices[1].Attrib = perPixel ? objRotMat : vertexLight[1];

			vertices[2].Pos.X = posTemp.X + edge3.X;
			vertices[2].Pos.Y = posTemp.Y + edge3.Y;
			vertices[2].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[2].TexCoord.X = uvRect.MaximumX;
			vertices[2].TexCoord.Y = uvRect.MaximumY;
			vertices[2].Color = mainClr;
			vertices[2].Attrib = perPixel ? objRotMat : vertexLight[2];
				
			vertices[3].Pos.X = posTemp.X + edge4.X;
			vertices[3].Pos.Y = posTemp.Y + edge4.Y;
			vertices[3].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[3].TexCoord.X = uvRect.MaximumX;
			vertices[3].TexCoord.Y = uvRect.Y;
			vertices[3].Color = mainClr;
			vertices[3].Attrib = perPixel ? objRotMat : vertexLight[3];
		}

		public override void Draw(IDrawDevice device)
		{
			Texture mainTex = this.RetrieveMainTex();
			ColorRgba mainClr = this.RetrieveMainColor();
			DrawTechnique tech = this.RetrieveDrawTechnique();

			Rect uvRect;
			Rect uvRectNext;
			bool smoothShaderInput = tech != null && tech.PreferredVertexFormat == VertexC1P3T4A4A1.VertexTypeIndex;
			this.GetAnimData(mainTex, tech, smoothShaderInput, out uvRect, out uvRectNext);
			
			if (!smoothShaderInput)
			{
				this.PrepareVerticesLight(ref this.verticesLight, device, mainClr, uvRect, tech);
				if (this.customMat != null)	device.AddVertices(this.customMat, VertexMode.Quads, this.verticesLight);
				else						device.AddVertices(this.sharedMat, VertexMode.Quads, this.verticesLight);
			}
			else
			{
				this.PrepareVerticesLightSmooth(ref this.verticesLightSmooth, device, this.CurrentFrameProgress, mainClr, uvRect, uvRectNext, tech);
				if (this.customMat != null)	device.AddVertices(this.customMat, VertexMode.Quads, this.verticesLightSmooth);
				else						device.AddVertices(this.sharedMat, VertexMode.Quads, this.verticesLightSmooth);
			}
		}
	}
}
