using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Drawing;
using Duality.Resources;
using Duality.Cloning;
using Duality.Properties;

namespace Duality.Components.Renderers
{
	/// <summary>
	/// Renders an animated sprite to represent the <see cref="GameObject"/>.
	/// </summary>
	[ManuallyCloned]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageAnimSpriteRenderer)]
	public class SpriteSheetRenderer : SpriteRenderer
	{
		private	string		atlasKey			= null;

		/// <summary>
		/// [GET / SET] The key of the frame to display.
		/// </summary>
		/// <remarks>
		/// Animation indices are looked up in the <see cref="Duality.Resources.Pixmap.Atlas"/> map
		/// of the <see cref="Duality.Resources.Texture"/> that is used.
		/// </remarks>
		public string AtlasKey
		{
			get { return this.atlasKey; }
			set { this.atlasKey = value; }
		}

		public SpriteSheetRenderer() {}
		public SpriteSheetRenderer(Rect rect, ContentRef<Material> mainMat) : base(rect, mainMat) { }
		
		protected void PrepareVerticesSmooth(ref VertexC1P3T4A1[] vertices, IDrawDevice device, float curAnimFrameFade, ColorRgba mainClr, Rect uvRect, Rect uvRectNext)
		{
			Vector3 posTemp = this.gameobj.Transform.Pos;
			float scaleTemp = 1.0f;
			device.PreprocessCoords(ref posTemp, ref scaleTemp);

			Vector2 xDot, yDot;
			MathF.GetTransformDotVec(this.GameObj.Transform.Angle, scaleTemp, out xDot, out yDot);

			Rect rectTemp = this.rect.Transformed(this.gameobj.Transform.Scale, this.gameobj.Transform.Scale);
			Vector2 edge1 = rectTemp.TopLeft;
			Vector2 edge2 = rectTemp.BottomLeft;
			Vector2 edge3 = rectTemp.BottomRight;
			Vector2 edge4 = rectTemp.TopRight;

			MathF.TransformDotVec(ref edge1, ref xDot, ref yDot);
			MathF.TransformDotVec(ref edge2, ref xDot, ref yDot);
			MathF.TransformDotVec(ref edge3, ref xDot, ref yDot);
			MathF.TransformDotVec(ref edge4, ref xDot, ref yDot);
			
			if (vertices == null || vertices.Length != 4) vertices = new VertexC1P3T4A1[4];

			vertices[0].Pos.X = posTemp.X + edge1.X;
			vertices[0].Pos.Y = posTemp.Y + edge1.Y;
			vertices[0].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[0].TexCoord.X = uvRect.X;
			vertices[0].TexCoord.Y = uvRect.Y;
			vertices[0].TexCoord.Z = uvRectNext.X;
			vertices[0].TexCoord.W = uvRectNext.Y;
			vertices[0].Color = mainClr;
			vertices[0].Attrib = curAnimFrameFade;

			vertices[1].Pos.X = posTemp.X + edge2.X;
			vertices[1].Pos.Y = posTemp.Y + edge2.Y;
			vertices[1].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[1].TexCoord.X = uvRect.X;
			vertices[1].TexCoord.Y = uvRect.BottomY;
			vertices[1].TexCoord.Z = uvRectNext.X;
			vertices[1].TexCoord.W = uvRectNext.BottomY;
			vertices[1].Color = mainClr;
			vertices[1].Attrib = curAnimFrameFade;

			vertices[2].Pos.X = posTemp.X + edge3.X;
			vertices[2].Pos.Y = posTemp.Y + edge3.Y;
			vertices[2].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[2].TexCoord.X = uvRect.RightX;
			vertices[2].TexCoord.Y = uvRect.BottomY;
			vertices[2].TexCoord.Z = uvRectNext.RightX;
			vertices[2].TexCoord.W = uvRectNext.BottomY;
			vertices[2].Color = mainClr;
			vertices[2].Attrib = curAnimFrameFade;
				
			vertices[3].Pos.X = posTemp.X + edge4.X;
			vertices[3].Pos.Y = posTemp.Y + edge4.Y;
			vertices[3].Pos.Z = posTemp.Z + this.VertexZOffset;
			vertices[3].TexCoord.X = uvRect.RightX;
			vertices[3].TexCoord.Y = uvRect.Y;
			vertices[3].TexCoord.Z = uvRectNext.RightX;
			vertices[3].TexCoord.W = uvRectNext.Y;
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
		protected void GetSpriteData(Texture mainTex, DrawTechnique tech, out Rect uvRect)
		{
			if (mainTex != null)
			{
				mainTex.LookupAtlas(this.atlasKey, out uvRect);
			}
			else
				uvRect = new Rect(1.0f, 1.0f);
		}

		public override void Draw(IDrawDevice device)
		{
			Texture mainTex = this.RetrieveMainTex();
			ColorRgba mainClr = this.RetrieveMainColor();
			DrawTechnique tech = this.RetrieveDrawTechnique();

			Rect uvRect;
			this.GetSpriteData(mainTex, tech, out uvRect);
			
			this.PrepareVertices(ref this.vertices, device, mainClr, uvRect);
			if (this.customMat != null)	device.AddVertices(this.customMat, VertexMode.Quads, this.vertices);
			else						device.AddVertices(this.sharedMat, VertexMode.Quads, this.vertices);
		}

		protected override void OnSetupCloneTargets(object targetObj, ICloneTargetSetup setup)
		{
			base.OnSetupCloneTargets(targetObj, setup);
			AnimSpriteRenderer target = targetObj as AnimSpriteRenderer;
		}

		protected override void OnCopyDataTo(object targetObj, ICloneOperation operation)
		{
			base.OnCopyDataTo(targetObj, operation);
			SpriteSheetRenderer target = targetObj as SpriteSheetRenderer;

			target.atlasKey	= this.atlasKey;
		}
	}
}
