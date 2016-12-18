using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Editor;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components.Renderers;
using Duality.Properties;

namespace SmoothAnimation
{
	/// <summary>
	/// Renders an animated sprite with smooth transitions between two active frames.
	/// Similar to <see cref="SpriteRenderer"/>, but extended with blending functionality for different sprite indices.
	/// </summary>
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageAnimSpriteRenderer)]
	public class SmoothAnimSpriteRenderer : SpriteRenderer
	{
		[DontSerialize] private VertexC1P3T4A1[] verticesSmooth   = null;

		
		private void PrepareVerticesSmooth(ref VertexC1P3T4A1[] vertices, IDrawDevice device, float curAnimFrameFade, ColorRgba mainClr, Rect uvRect, Rect uvRectNext)
		{
			Vector3 posTemp = this.GameObj.Transform.Pos;
			float scaleTemp = 1.0f;
			device.PreprocessCoords(ref posTemp, ref scaleTemp);

			Vector2 xDot, yDot;
			MathF.GetTransformDotVec(this.GameObj.Transform.Angle, scaleTemp, out xDot, out yDot);

			Rect rectTemp = this.rect.Transformed(this.GameObj.Transform.Scale, this.GameObj.Transform.Scale);
			Vector2 edge1 = rectTemp.TopLeft;
			Vector2 edge2 = rectTemp.BottomLeft;
			Vector2 edge3 = rectTemp.BottomRight;
			Vector2 edge4 = rectTemp.TopRight;

			MathF.TransformDotVec(ref edge1, ref xDot, ref yDot);
			MathF.TransformDotVec(ref edge2, ref xDot, ref yDot);
			MathF.TransformDotVec(ref edge3, ref xDot, ref yDot);
			MathF.TransformDotVec(ref edge4, ref xDot, ref yDot);
			
			float left       = uvRect.X;
			float right      = uvRect.RightX;
			float top        = uvRect.Y;
			float bottom     = uvRect.BottomY;
			float nextLeft   = uvRectNext.X;
			float nextRight  = uvRectNext.RightX;
			float nextTop    = uvRectNext.Y;
			float nextBottom = uvRectNext.BottomY;

			if ((this.flipMode & FlipMode.Horizontal) != FlipMode.None)
			{
				edge1.X = -edge1.X;
				edge2.X = -edge2.X;
				edge3.X = -edge3.X;
				edge4.X = -edge4.X;
			}
			if ((this.flipMode & FlipMode.Vertical) != FlipMode.None)
			{
				edge1.Y = -edge1.Y;
				edge2.Y = -edge2.Y;
				edge3.Y = -edge3.Y;
				edge4.Y = -edge4.Y;
			}

			if (vertices == null || vertices.Length != 4) vertices = new VertexC1P3T4A1[4];

			vertices[0].Pos.X = posTemp.X + edge1.X;
			vertices[0].Pos.Y = posTemp.Y + edge1.Y;
			vertices[0].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[0].TexCoord.X = left;
			vertices[0].TexCoord.Y = top;
			vertices[0].TexCoord.Z = nextLeft;
			vertices[0].TexCoord.W = nextTop;
			vertices[0].Color = mainClr;
			vertices[0].Attrib = curAnimFrameFade;

			vertices[1].Pos.X = posTemp.X + edge2.X;
			vertices[1].Pos.Y = posTemp.Y + edge2.Y;
			vertices[1].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[1].TexCoord.X = left;
			vertices[1].TexCoord.Y = bottom;
			vertices[1].TexCoord.Z = nextLeft;
			vertices[1].TexCoord.W = nextBottom;
			vertices[1].Color = mainClr;
			vertices[1].Attrib = curAnimFrameFade;

			vertices[2].Pos.X = posTemp.X + edge3.X;
			vertices[2].Pos.Y = posTemp.Y + edge3.Y;
			vertices[2].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[2].TexCoord.X = right;
			vertices[2].TexCoord.Y = bottom;
			vertices[2].TexCoord.Z = nextRight;
			vertices[2].TexCoord.W = nextBottom;
			vertices[2].Color = mainClr;
			vertices[2].Attrib = curAnimFrameFade;
				
			vertices[3].Pos.X = posTemp.X + edge4.X;
			vertices[3].Pos.Y = posTemp.Y + edge4.Y;
			vertices[3].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[3].TexCoord.X = right;
			vertices[3].TexCoord.Y = top;
			vertices[3].TexCoord.Z = nextRight;
			vertices[3].TexCoord.W = nextTop;
			vertices[3].Color = mainClr;
			vertices[3].Attrib = curAnimFrameFade;

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
			Texture mainTex = this.RetrieveMainTex();
			ColorRgba mainClr = this.RetrieveMainColor();
			DrawTechnique tech = this.RetrieveDrawTechnique();

			Rect uvRect;
			Rect uvRectNext;
			if (mainTex != null)
			{
				mainTex.LookupAtlas(this.spriteIndex.Current, out uvRect);
				mainTex.LookupAtlas(this.spriteIndex.Next, out uvRectNext);
			}
			else
			{
				uvRect = new Rect(1.0f, 1.0f);
				uvRectNext = new Rect(1.0f, 1.0f);
			}
			
			this.PrepareVerticesSmooth(ref this.verticesSmooth, device, this.spriteIndex.Blend, mainClr, uvRect, uvRectNext);
			if (this.customMat != null)
				device.AddVertices(this.customMat, VertexMode.Quads, this.verticesSmooth);
			else
				device.AddVertices(this.sharedMat, VertexMode.Quads, this.verticesSmooth);
		}
	}
}
