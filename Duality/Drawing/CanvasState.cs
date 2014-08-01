using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Graphics.OpenGL;
using OpenTK;

using Duality.Resources;
using Duality.Cloning;

namespace Duality.Drawing
{
	/// <summary>
	/// Describes the state of a <see cref="Canvas"/>.
	/// </summary>
	public class CanvasState : ICloneExplicit
	{
		private static readonly BatchInfo DefaultMaterial = new BatchInfo(DrawTechnique.Mask, ColorRgba.White);

		private	BatchInfo			batchInfo;
		private	ColorRgba			color;
		private	ContentRef<Font>	font;
		private	float				zOffset;
		private	bool		invariantTextScale;
		private	float		transformAngle;
		private	Vector2		transformScale;
		private	Vector2		transformHandle;
		private	Rect		uvGenRect;

		private	Vector2		curTX;
		private	Vector2		curTY;
		private	Vector2		texBaseSize;


		internal BatchInfo MaterialDirect
		{
			get { return this.batchInfo; }
		}
		/// <summary>
		/// [GET] The material that is used for drawing.
		/// </summary>
		public BatchInfo Material
		{
			get { return new BatchInfo(this.batchInfo); }
		}
		/// <summary>
		/// [GET / SET] The <see cref="Duality.Resources.Font"/> to use for text rendering.
		/// </summary>
		public ContentRef<Font> TextFont
		{
			get { return this.font; }
			set { this.font = value.IsAvailable ? value : Font.GenericMonospace10; }
		}
		/// <summary>
		/// [GET / SET] If true, text does not scale due to its position in space
		/// </summary>
		public bool TextInvariantScale
		{
			get { return this.invariantTextScale; }
			set { this.invariantTextScale = value; }
		}
		/// <summary>
		/// [GET / SET] The texture coordinate rect which is used for UV generation when drawing shapes.
		/// </summary>
		public Rect TextureCoordinateRect
		{
			get { return this.uvGenRect; }
			set { this.uvGenRect = value; }
		}
		/// <summary>
		/// [GET] The currently bound main textures size.
		/// </summary>
		public Vector2 TextureBaseSize
		{
			get { return this.texBaseSize; }
		}
		/// <summary>
		/// [GET / SET] The color tint to use for drawing.
		/// </summary>
		public ColorRgba ColorTint
		{
			get { return this.color; }
			set { this.color = value; }
		}
		/// <summary>
		/// [GET / SET] A Z-Offset value that is added to each emitted vertices Z coordinate after all projection calculations have been done.
		/// </summary>
		public float ZOffset
		{
			get { return this.zOffset; }
			set { this.zOffset = value; }
		}
		/// <summary>
		/// [GET / SET] The angle by which all shapes are transformed locally.
		/// </summary>
		public float TransformAngle
		{
			get { return this.transformAngle; }
			set { this.transformAngle = value; this.UpdateTransform(); }
		}
		/// <summary>
		/// [GET / SET] The scale by which all shapes are transformed locally.
		/// </summary>
		public Vector2 TransformScale
		{
			get { return this.transformScale; }
			set { this.transformScale = value; this.UpdateTransform(); }
		}
		/// <summary>
		/// [GET / SET] The handle used for locally transforming all shapes. 
		/// You can think of it as the "fixed point" of a shape when rotating or scaling it.
		/// </summary>
		public Vector2 TransformHandle
		{
			get { return this.transformHandle; }
			set { this.transformHandle = value; this.UpdateTransform(); }
		}
		/// <summary>
		/// [GET] Returns whether the current transformation is an identity transformation (i.e. doesn't do anything).
		/// </summary>
		public bool IsTransformIdentity
		{
			get
			{
				return 
					this.transformAngle == 0.0f &&
					this.transformScale == Vector2.One &&
					this.transformHandle == Vector2.Zero;
			}
		}


		public CanvasState() 
		{
			this.Reset();
		}
		public CanvasState(CanvasState other)
		{
			other.CopyTo(this);
		}
			
		/// <summary>
		/// Copies all state data to the specified target.
		/// </summary>
		/// <param name="target"></param>
		public void CopyTo(CanvasState target)
		{
			target.batchInfo			= this.batchInfo;
			target.uvGenRect			= this.uvGenRect;
			target.texBaseSize			= this.texBaseSize;
			target.font					= this.font;
			target.color				= this.color;
			target.invariantTextScale	= this.invariantTextScale;
			target.zOffset				= this.zOffset;
			target.transformAngle		= this.transformAngle;
			target.transformHandle		= this.transformHandle;
			target.transformScale		= this.transformScale;
			target.UpdateTransform();
		}
		/// <summary>
		/// Creates a clone of this State.
		/// </summary>
		/// <returns></returns>
		public CanvasState Clone()
		{
			return new CanvasState(this);
		}
		/// <summary>
		/// Resets this State to its initial settings.
		/// </summary>
		public void Reset()
		{
			this.batchInfo = DefaultMaterial;
			this.uvGenRect = new Rect(1.0f, 1.0f);
			this.texBaseSize = Vector2.Zero;
			this.font = Font.GenericMonospace10;
			this.color = ColorRgba.White;
			this.invariantTextScale = false;
			this.zOffset = 0.0f;
			this.transformAngle = 0.0f;
			this.transformHandle = Vector2.Zero;
			this.transformScale = Vector2.One;
			this.UpdateTransform();
		}

		/// <summary>
		/// Sets the States drawing material.
		/// </summary>
		/// <param name="material"></param>
		public void SetMaterial(BatchInfo material)
		{
			this.batchInfo = material ?? DefaultMaterial;
			if (this.batchInfo.MainTexture.IsAvailable)
			{
				Texture tex = this.batchInfo.MainTexture.Res;
				this.uvGenRect = new Rect(tex.UVRatio);
				this.texBaseSize = tex.Size;
			}
			else
			{
				this.texBaseSize = Vector2.Zero;
			}
		}
		/// <summary>
		/// Sets the States drawing material.
		/// </summary>
		/// <param name="material"></param>
		public void SetMaterial(ContentRef<Material> material)
		{
			if (material.IsExplicitNull)
				this.batchInfo = DefaultMaterial;
			else if (material.IsAvailable)
				this.batchInfo = material.Res.InfoDirect;
			else
				this.batchInfo = Resources.Material.Checkerboard.Res.InfoDirect;

			if (this.batchInfo.MainTexture.IsAvailable)
			{
				Texture tex = this.batchInfo.MainTexture.Res;
				this.uvGenRect = new Rect(tex.UVRatio);
				this.texBaseSize = tex.Size;
			}
			else
			{
				this.texBaseSize = Vector2.Zero;
			}
		}

		private void UpdateTransform()
		{
			MathF.GetTransformDotVec(
				this.transformAngle, 
				out this.curTX, 
				out this.curTY);
		}
		internal void TransformVertices<T>(T[] vertexData, Vector2 shapeHandle, float shapeHandleScale) where T : struct, IVertexData
		{
			if (this.IsTransformIdentity)
			{
				for (int i = 0; i < vertexData.Length; i++)
				{
					Vector3 pos = vertexData[i].Pos;
					pos.Z += this.zOffset;
					vertexData[i].Pos = pos;
				}
			}
			else
			{
				this.UpdateTransform();
				Vector2 transformHandle = this.transformHandle;
				Vector2 transformScale = this.transformScale;
				for (int i = 0; i < vertexData.Length; i++)
				{
					Vector3 pos = vertexData[i].Pos;
					pos.X -= transformHandle.X * shapeHandleScale + shapeHandle.X;
					pos.Y -= transformHandle.Y * shapeHandleScale + shapeHandle.Y;
					pos.X *= transformScale.X;
					pos.Y *= transformScale.Y;
					MathF.TransformDotVec(ref pos, ref this.curTX, ref this.curTY);
					pos.X += shapeHandle.X;
					pos.Y += shapeHandle.Y;
					pos.Z += this.zOffset;
					vertexData[i].Pos = pos;
				}
			}
		}
		internal void TransformVertices(VertexC1P3T2[] vertexData, Vector2 shapeHandle, float shapeHandleScale)
		{
			if (this.IsTransformIdentity)
			{
				for (int i = 0; i < vertexData.Length; i++)
				{
					vertexData[i].Pos.Z += this.zOffset;
				}
			}
			else
			{
				Vector2 transformHandle = this.transformHandle;
				Vector2 transformScale = this.transformScale;
				for (int i = 0; i < vertexData.Length; i++)
				{
					vertexData[i].Pos.X -= transformHandle.X * shapeHandleScale + shapeHandle.X;
					vertexData[i].Pos.Y -= transformHandle.Y * shapeHandleScale + shapeHandle.Y;
					vertexData[i].Pos.X *= transformScale.X;
					vertexData[i].Pos.Y *= transformScale.Y;
					MathF.TransformDotVec(ref vertexData[i].Pos, ref this.curTX, ref this.curTY);
					vertexData[i].Pos.X += shapeHandle.X;
					vertexData[i].Pos.Y += shapeHandle.Y;
					vertexData[i].Pos.Z += this.zOffset;
				}
			}
		}
			
		void ICloneExplicit.CopyDataTo(object targetObj, CloneProvider provider)
		{
			this.CopyTo(targetObj as CanvasState);
		}
	}
}