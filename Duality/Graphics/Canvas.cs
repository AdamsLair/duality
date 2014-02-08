using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Graphics.OpenGL;
using OpenTK;

using Duality.VertexFormat;
using Duality.ColorFormat;
using Duality.Resources;
using Duality.Cloning;
using ICloneable = Duality.Cloning.ICloneable;

namespace Duality
{
	/// <summary>
	/// Provides high level drawing operations on top of an existing <see cref="IDrawDevice"/>. However, this class is not designed
	/// for drawing large batches of primitives / vertices at once. For large amounts of primitives you should consider directly 
	/// using the underlying IDrawDevice instead to achieve best Profile.
	/// </summary>
	public class Canvas
	{
		/// <summary>
		/// Describes the state of a <see cref="Canvas"/>.
		/// </summary>
		public class State : ICloneable
		{
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
			/// [GET / SET] The angle by which all shapes are transformed.
			/// </summary>
			public float TransformAngle
			{
				get { return this.transformAngle; }
				set { this.transformAngle = value; this.UpdateTransform(); }
			}
			/// <summary>
			/// [GET / SET] The scale by which all shapes are transformed.
			/// </summary>
			public Vector2 TransformScale
			{
				get { return this.transformScale; }
				set { this.transformScale = value; this.UpdateTransform(); }
			}
			/// <summary>
			/// [GET / SET] The handle used for transforming all shapes.
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


			public State() 
			{
				this.Reset();
			}
			public State(State other)
			{
				other.CopyTo(this);
			}
			
			/// <summary>
			/// Copies all state data to the specified target.
			/// </summary>
			/// <param name="target"></param>
			public void CopyTo(State target)
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
			public State Clone()
			{
				return new State(this);
			}
			/// <summary>
			/// Resets this State to its initial settings.
			/// </summary>
			public void Reset()
			{
				this.batchInfo = new BatchInfo(DrawTechnique.Mask, ColorRgba.White);
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
				this.batchInfo = material;
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
				this.batchInfo = material.Res.InfoDirect;
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
			
			void ICloneable.CopyDataTo(object targetObj, CloneProvider provider)
			{
				this.CopyTo(targetObj as State);
			}
		}


		private static Dictionary<uint,Texture>	dashTextures = new Dictionary<uint,Texture>();

		private	IDrawDevice		device		= null;
		private	Stack<State>	stateStack	= new Stack<State>(new [] { new State() });
		private	CanvasBuffer	buffer		= null;


		/// <summary>
		/// [GET] The underlying <see cref="IDrawDevice"/> that is used for drawing.
		/// </summary>
		public IDrawDevice DrawDevice
		{
			get { return this.device; }
		}
		/// <summary>
		/// [GET / SET] The Canvas' current <see cref="State"/>.
		/// </summary>
		public State CurrentState
		{
			get { return this.stateStack.Peek(); }
			set 
			{
				this.stateStack.Pop();
				this.stateStack.Push(value);
			}
		}
		/// <summary>
		/// [GET] The available width to draw on this Canvas.
		/// </summary>
		public int Width
		{
			get { return MathF.RoundToInt(this.device.TargetSize.X); }
		}
		/// <summary>
		/// [GET] The available height to draw on this Canvas.
		/// </summary>
		public int Height
		{
			get { return MathF.RoundToInt(this.device.TargetSize.Y); }
		}


		/// <summary>
		/// Creates a new Canvas that uses the specified <see cref="Duality.IDrawDevice"/>. You may optionally specify a
		/// <see cref="Duality.CanvasBuffer"/> for improving rendering performance and memory footprint when rendering similar
		/// shapes throughout multiple frames.
		/// </summary>
		/// <param name="device"></param>
		/// <param name="buffer"></param>
		public Canvas(IDrawDevice device, CanvasBuffer buffer = null)
		{
			this.device = device;
			this.buffer = buffer ?? new CanvasBuffer(true);
			this.buffer.Reset();
		}
		

		/// <summary>
		/// Adds a clone of the <see cref="Canvas.CurrentState">current state</see> on top of the internal
		/// <see cref="State"/> stack.
		/// </summary>
		public void PushState()
		{
			this.stateStack.Push(this.stateStack.Peek().Clone());
		}
		/// <summary>
		/// Removes the topmost <see cref="State"/> from the internal State stack.
		/// </summary>
		public void PopState()
		{
			this.stateStack.Pop();
			if (this.stateStack.Count == 0) this.stateStack.Push(new State());
		}

		
		/// <summary>
		/// Draws a predefined set of vertices using the Canvas transformation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="vertices"></param>
		/// <param name="mode"></param>
		public void DrawVertices<T>(T[] vertices, VertexMode mode) where T : struct, IVertexData
		{
			Vector3 pos = vertices[0].Pos;
			float scale = 1.0f;
			device.PreprocessCoords(ref pos, ref scale);

			this.CurrentState.TransformVertices(vertices, pos.Xy, scale);
			this.device.AddVertices<T>(this.CurrentState.MaterialDirect, mode, vertices);
		}

		/// <summary>
		/// Draws a convex polygon. All vertices share the same Z value.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void DrawPolygon(Vector2[] points, float x, float y, float z = 0.0f)
		{
			Vector3 pos = new Vector3(x, y, z);

			float scale = 1.0f;
			this.device.PreprocessCoords(ref pos, ref scale);

			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			Rect texCoordRect = this.CurrentState.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.buffer.RequestVertexArray(points.Length);
			for (int i = 0; i < points.Length; i++)
			{
				vertices[i].Pos.X = points[i].X * scale + pos.X + 0.5f;
				vertices[i].Pos.Y = points[i].Y * scale + pos.Y + 0.5f;
				vertices[i].Pos.Z = pos.Z;
				vertices[i].TexCoord.X = texCoordRect.X + texCoordRect.W * (float)i / (float)(points.Length - 1);
				vertices[i].TexCoord.Y = texCoordRect.Y;
				vertices[i].Color = shapeColor;
			}

			this.CurrentState.TransformVertices(vertices, pos.Xy, scale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices, points.Length);
		}

		/// <summary>
		/// Draws a three-dimensional sphere.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="r"></param>
		public void DrawSphere(float x, float y, float z, float r)
		{
			r = MathF.Abs(r);
			Vector3 pos = new Vector3(x, y, z);
			if (!this.device.IsCoordInView(pos, r)) return;

			float scale = 1.0f;
			Vector3 posTemp = pos;
			this.device.PreprocessCoords(ref posTemp, ref scale);

			int segmentNum = MathF.Clamp(MathF.RoundToInt(MathF.Pow(r * scale, 0.65f) * 2.5f), 4, 128);
			Vector2 shapeHandle = pos.Xy;
			float shapeHandleScale = scale;
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			Rect texCoordRect = this.CurrentState.TextureCoordinateRect;
			VertexC1P3T2[] vertices;
			float angle;

			// XY circle
			vertices = this.buffer.RequestVertexArray(segmentNum);
			angle = 0.0f;
			for (int i = 0; i < segmentNum; i++)
			{
				vertices[i].Pos.X = pos.X + (float)Math.Sin(angle) * r;
				vertices[i].Pos.Y = pos.Y - (float)Math.Cos(angle) * r;
				vertices[i].Pos.Z = pos.Z;
				vertices[i].TexCoord.X = texCoordRect.X + texCoordRect.W * (float)i / (float)(segmentNum - 1);
				vertices[i].TexCoord.Y = texCoordRect.Y;
				vertices[i].Color = shapeColor;
				this.device.PreprocessCoords(ref vertices[i].Pos, ref scale);
				angle += (MathF.TwoPi / segmentNum);
			}
			this.CurrentState.TransformVertices(vertices, shapeHandle, shapeHandleScale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices, segmentNum);

			// XZ circle
			vertices = this.buffer.RequestVertexArray(segmentNum);
			angle = 0.0f;
			for (int i = 0; i < segmentNum; i++)
			{
				vertices[i].Pos.X = pos.X + (float)Math.Sin(angle) * r;
				vertices[i].Pos.Y = pos.Y;
				vertices[i].Pos.Z = pos.Z - (float)Math.Cos(angle) * r;
				vertices[i].TexCoord.X = texCoordRect.X + texCoordRect.W * (float)i / (float)(segmentNum - 1);
				vertices[i].TexCoord.Y = texCoordRect.Y;
				vertices[i].Color = shapeColor;
				this.device.PreprocessCoords(ref vertices[i].Pos, ref scale);
				angle += (MathF.TwoPi / segmentNum);
			}
			this.CurrentState.TransformVertices(vertices, shapeHandle, shapeHandleScale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices, segmentNum);

			// YZ circle
			vertices = this.buffer.RequestVertexArray(segmentNum);
			angle = 0.0f;
			for (int i = 0; i < segmentNum; i++)
			{
				vertices[i].Pos.X = pos.X;
				vertices[i].Pos.Y = pos.Y + (float)Math.Sin(angle) * r;
				vertices[i].Pos.Z = pos.Z - (float)Math.Cos(angle) * r;
				vertices[i].TexCoord.X = texCoordRect.X + texCoordRect.W * (float)i / (float)(segmentNum - 1);
				vertices[i].TexCoord.Y = texCoordRect.Y;
				vertices[i].Color = shapeColor;
				this.device.PreprocessCoords(ref vertices[i].Pos, ref scale);
				angle += (MathF.TwoPi / segmentNum);
			}
			this.CurrentState.TransformVertices(vertices, shapeHandle, shapeHandleScale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices, segmentNum);
		}

		/// <summary>
		/// Draws a three-dimensional line.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="z2"></param>
		public void DrawLine(float x, float y, float z, float x2, float y2, float z2)
		{
			Vector3 pos = new Vector3(x, y, z);
			Vector3 target = new Vector3(x2, y2, z2);
			float scale = 1.0f;
			
			device.PreprocessCoords(ref pos, ref scale);
			device.PreprocessCoords(ref target, ref scale);

			Vector2 shapeHandle = pos.Xy;
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			Rect texCoordRect = this.CurrentState.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.buffer.RequestVertexArray(2);

			vertices[0].Pos = pos + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[1].Pos = target + new Vector3(0.5f, 0.5f, 0.0f);
			
			vertices[0].TexCoord = new Vector2(texCoordRect.X, 0.0f);
			vertices[1].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W, 0.0f);

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;

			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.Lines, vertices, 2);
		}
		/// <summary>
		/// Draws a flat line.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		public void DrawLine(float x, float y, float x2, float y2)
		{
			this.DrawLine(x, y, 0, x2, y2, 0);
		}
		/// <summary>
		/// Draws a three-dimensional line.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="z2"></param>
		public void DrawDashLine(float x, float y, float z, float x2, float y2, float z2, DashPattern pattern = DashPattern.Dash, float patternLen = 1.0f)
		{
			uint patternBits = (uint)pattern;
			if (!dashTextures.ContainsKey(patternBits))
			{
				Pixmap.Layer pxLayerDash = new Pixmap.Layer(32, 1);
				for (int i = 31; i >= 0; i--) pxLayerDash[i, 0] = ((patternBits & (1U << i)) != 0) ? ColorRgba.White : ColorRgba.TransparentWhite;
				Pixmap pxDash = new Pixmap(pxLayerDash);
				Texture texDash = new Texture(pxDash, Texture.SizeMode.Stretch, TextureMagFilter.Nearest, TextureMinFilter.Nearest, TextureWrapMode.Repeat);
				dashTextures[patternBits] = texDash;
			}

			Vector3 pos = new Vector3(x, y, z);
			Vector3 target = new Vector3(x2, y2, z2);
			float scale = 1.0f;
			float lineLength = (target - pos).Length;
			
			device.PreprocessCoords(ref pos, ref scale);
			device.PreprocessCoords(ref target, ref scale);

			Vector2 shapeHandle = pos.Xy;
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			VertexC1P3T2[] vertices = this.buffer.RequestVertexArray(2);
			vertices[0].Pos = pos + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[1].Pos = target + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[0].TexCoord = new Vector2(0.0f, 0.0f);
			vertices[1].TexCoord = new Vector2(lineLength * patternLen / 32.0f, 0.0f);
			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;

			BatchInfo customMat = new BatchInfo(this.CurrentState.MaterialDirect);
			customMat.MainTexture = dashTextures[patternBits];
			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(customMat, VertexMode.Lines, vertices, 2);
		}
		/// <summary>
		/// Draws a flat line.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		public void DrawDashLine(float x, float y, float x2, float y2, DashPattern pattern = DashPattern.Dash, float patternLen = 1.0f)
		{
			this.DrawDashLine(x, y, 0, x2, y2, 0, pattern, patternLen);
		}
		/// <summary>
		/// Draws a thick, three-dimensional line.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="z2"></param>
		public void DrawThickLine(float x, float y, float z, float x2, float y2, float z2, float width)
		{
			Vector3 pos = new Vector3(x, y, z);
			Vector3 target = new Vector3(x2, y2, z2);
			float scale = 1.0f;
			float scale2 = 1.0f;
			
			device.PreprocessCoords(ref pos, ref scale);
			device.PreprocessCoords(ref target, ref scale2);

			Vector2 dir = (target.Xy - pos.Xy).Normalized;
			Vector2 left = dir.PerpendicularLeft * width * 0.5f * scale;
			Vector2 right = dir.PerpendicularRight * width * 0.5f * scale;
			Vector2 left2 = dir.PerpendicularLeft * width * 0.5f * scale2;
			Vector2 right2 = dir.PerpendicularRight * width * 0.5f * scale2;

			Vector2 shapeHandle = pos.Xy;
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			Rect texCoordRect = this.CurrentState.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.buffer.RequestVertexArray(4);

			vertices[0].Pos = pos + new Vector3(left);
			vertices[1].Pos = target + new Vector3(left2);
			vertices[2].Pos = target + new Vector3(right2);
			vertices[3].Pos = pos + new Vector3(right);

			vertices[0].TexCoord = new Vector2(texCoordRect.X, 0.0f);
			vertices[1].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W * 0.3333333f, 0.0f);
			vertices[2].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W * 0.6666666f, 0.0f);
			vertices[3].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W, 0.0f);

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices, 4);
		}
		/// <summary>
		/// Draws a thick, flat line.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		public void DrawThickLine(float x, float y, float x2, float y2, float width)
		{
			this.DrawThickLine(x, y, 0, x2, y2, 0, width);
		}

		/// <summary>
		/// Draws a rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void DrawRect(float x, float y, float z, float w, float h)
		{
			if (w < 0.0f) { x += w; w = -w; }
			if (h < 0.0f) { y += h; h = -h; }

			Vector3 pos = new Vector3(x, y, z);
			float scale = 1.0f;
			device.PreprocessCoords(ref pos, ref scale);

			Vector2 shapeHandle = pos.Xy;
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			Rect texCoordRect = this.CurrentState.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.buffer.RequestVertexArray(4);

			vertices[0].Pos = new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z);
			vertices[1].Pos = new Vector3(pos.X + w * scale - 0.5f, pos.Y + 0.5f, pos.Z);
			vertices[2].Pos = new Vector3(pos.X + w * scale - 0.5f, pos.Y + h * scale - 0.5f, pos.Z);
			vertices[3].Pos = new Vector3(pos.X + 0.5f, pos.Y + h * scale - 0.5f, pos.Z);

			vertices[0].TexCoord = new Vector2(texCoordRect.X, 0.0f);
			vertices[1].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W * 0.3333333f, 0.0f);
			vertices[2].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W * 0.6666666f, 0.0f);
			vertices[3].TexCoord = new Vector2(texCoordRect.X + texCoordRect.W, 0.0f);

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices, 4);
		}
		/// <summary>
		/// Draws a rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void DrawRect(float x, float y, float w, float h)
		{
			this.DrawRect(x, y, 0, w, h);
		}
		
		/// <summary>
		/// Draws the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="width">The rendered ovals total width.</param>
		/// <param name="height">The rendered ovals total height.</param>
		/// <param name="minAngle">The oval segments minimum angle.</param>
		/// <param name="maxAngle">The oval segments maximum angle.</param>
		/// <param name="outline">If true, the oval sections complete outline is drawn instead of just the outer perimeter.</param>
		public void DrawOvalSegment(float x, float y, float z, float width, float height, float minAngle, float maxAngle, bool outline = false)
		{
			if (minAngle == maxAngle) return;
			if (width < 0.0f) { x += width; width = -width; }
			if (height < 0.0f) { y += height; height = -height; }
			width *= 0.5f; x += width;
			height *= 0.5f; y += height;

			Vector3 pos = new Vector3(x, y, z);
			if (!this.device.IsCoordInView(pos, MathF.Max(width, height) + this.CurrentState.TransformHandle.Length)) return;

			float scale = 1.0f;
			this.device.PreprocessCoords(ref pos, ref scale);
			width *= scale;
			height *= scale;

			if (maxAngle <= minAngle) maxAngle += MathF.Ceiling((minAngle - maxAngle) / MathF.RadAngle360) * MathF.RadAngle360;

			float angleRange = MathF.Min(maxAngle - minAngle, MathF.RadAngle360);
			bool loop = angleRange >= MathF.RadAngle360 - MathF.RadAngle1 * 0.001f;

			if (loop && outline) outline = false;
			else if (outline) loop = true;

			int segmentNum = MathF.Clamp(MathF.RoundToInt(MathF.Pow(MathF.Max(width, height), 0.65f) * 3.5f * angleRange / MathF.RadAngle360), 4, 128);
			float angleStep = angleRange / segmentNum;
			Vector2 shapeHandle = pos.Xy - new Vector2(width, height);
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			Rect texCoordRect = this.CurrentState.TextureCoordinateRect;
			int vertexCount = segmentNum + (loop ? 0 : 1) + (outline ? 2 : 0);
			VertexC1P3T2[] vertices = this.buffer.RequestVertexArray(vertexCount);
			float angle = minAngle;
			
			if (outline)
			{
				vertices[0].Pos.X = pos.X + 0.5f;
				vertices[0].Pos.Y = pos.Y + 0.5f;
				vertices[0].Pos.Z = pos.Z;
				vertices[0].TexCoord.X = texCoordRect.X;
				vertices[0].TexCoord.Y = texCoordRect.Y;
				vertices[0].Color = shapeColor;
			}

			// XY circle
			for (int i = outline ? 1 : 0; i < vertexCount; i++)
			{
				vertices[i].Pos.X = pos.X + (float)Math.Sin(angle) * (width - 0.5f);
				vertices[i].Pos.Y = pos.Y - (float)Math.Cos(angle) * (height - 0.5f);
				vertices[i].Pos.Z = pos.Z;
				vertices[i].TexCoord.X = texCoordRect.X + texCoordRect.W * (float)i / (float)(vertexCount - 1);
				vertices[i].TexCoord.Y = texCoordRect.Y;
				vertices[i].Color = shapeColor;
				angle += angleStep;
			}
			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, loop ? VertexMode.LineLoop : VertexMode.LineStrip, vertices, vertexCount);
		}
		/// <summary>
		/// Draws the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width">The rendered ovals total width.</param>
		/// <param name="height">The rendered ovals total height.</param>
		/// <param name="minAngle">The oval segments minimum angle.</param>
		/// <param name="maxAngle">The oval segments maximum angle.</param>
		/// <param name="outline">If true, the oval sections complete outline is drawn instead of just the outer perimeter.</param>
		public void DrawOvalSegment(float x, float y, float width, float height, float minAngle, float maxAngle, bool outline = false)
		{
			this.DrawOvalSegment(x, y, 0, width, height, minAngle, maxAngle, outline);
		}
		/// <summary>
		/// Draws the section of a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="radius">The rendered circles radius.</param>
		/// <param name="minAngle">The circle segments minimum angle.</param>
		/// <param name="maxAngle">The circle segments maximum angle.</param>
		/// <param name="outline">If true, the circle sections complete outline is drawn instead of just the outer perimeter.</param>
		public void DrawCircleSegment(float x, float y, float z, float radius, float minAngle, float maxAngle, bool outline = false)
		{
			this.CurrentState.TransformHandle += new Vector2(radius, radius);
			this.DrawOvalSegment(x, y, z, radius * 2, radius * 2, minAngle, maxAngle, outline);
			this.CurrentState.TransformHandle -= new Vector2(radius, radius);
		}
		/// <summary>
		/// Draws the section of a circle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="radius">The rendered circles radius.</param>
		/// <param name="minAngle">The circle segments minimum angle.</param>
		/// <param name="maxAngle">The circle segments maximum angle.</param>
		/// <param name="outline">If true, the circle sections complete outline is drawn instead of just the outer perimeter.</param>
		public void DrawCircleSegment(float x, float y, float radius, float minAngle, float maxAngle, bool outline = false)
		{
			this.CurrentState.TransformHandle += new Vector2(radius, radius);
			this.DrawOvalSegment(x, y, 0, radius * 2, radius * 2, minAngle, maxAngle, outline);
			this.CurrentState.TransformHandle -= new Vector2(radius, radius);
		}

		/// <summary>
		/// Draws the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawOval(float x, float y, float z, float width, float height)
		{
			this.DrawOvalSegment(x, y, z, width, height, 0.0f, MathF.RadAngle360);
		}
		/// <summary>
		/// Draws the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawOval(float x, float y, float width, float height)
		{
			this.DrawOvalSegment(x, y, 0, width, height, 0.0f, MathF.RadAngle360);
		}
		/// <summary>
		/// Draws the section of a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="radius"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawCircle(float x, float y, float z, float radius)
		{
			this.CurrentState.TransformHandle += new Vector2(radius, radius);
			this.DrawOvalSegment(x, y, z, radius * 2, radius * 2, 0.0f, MathF.RadAngle360);
			this.CurrentState.TransformHandle -= new Vector2(radius, radius);
		}
		/// <summary>
		/// Draws the section of a circle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="radius"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawCircle(float x, float y, float radius)
		{
			this.CurrentState.TransformHandle += new Vector2(radius, radius);
			this.DrawOvalSegment(x, y, 0, radius * 2, radius * 2, 0.0f, MathF.RadAngle360);
			this.CurrentState.TransformHandle -= new Vector2(radius, radius);
		}
		
		/// <summary>
		/// Fills a polygon. All vertices share the same Z value, and the polygon needs to be convex.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void FillPolygon(Vector2[] points, float x, float y, float z = 0.0f)
		{
			Vector3 pos = new Vector3(x, y, z);

			float scale = 1.0f;
			this.device.PreprocessCoords(ref pos, ref scale);

			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			Rect texCoordRect = this.CurrentState.TextureCoordinateRect;

			// Determine bounding box
			Rect pointBoundingRect = points.BoundingBox();

			// Set up vertex array
			VertexC1P3T2[] vertices = this.buffer.RequestVertexArray(points.Length);
			for (int i = 0; i < points.Length; i++)
			{
				vertices[i].Pos.X = points[i].X * scale + pos.X;
				vertices[i].Pos.Y = points[i].Y * scale + pos.Y;
				vertices[i].Pos.Z = pos.Z;
				vertices[i].TexCoord.X = texCoordRect.X + ((points[i].X - pointBoundingRect.X) / pointBoundingRect.W) * texCoordRect.W;
				vertices[i].TexCoord.Y = texCoordRect.Y + ((points[i].Y - pointBoundingRect.Y) / pointBoundingRect.H) * texCoordRect.H;
				vertices[i].Color = shapeColor;
			}

			this.CurrentState.TransformVertices(vertices, pos.Xy, scale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.Polygon, vertices, points.Length);
		}
		/// <summary>
		/// Fills a polygons outline. All vertices share the same Z value.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="width"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void FillPolygonOutline(Vector2[] points, float width, float x, float y, float z = 0.0f)
		{
			width *= 0.5f;
			Vector3 pos = new Vector3(x, y, z);

			float scale = 1.0f;
			this.device.PreprocessCoords(ref pos, ref scale);

			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			Rect texCoordRect = this.CurrentState.TextureCoordinateRect;

			// Determine bounding box
			Rect pointBoundingRect = points.BoundingBox();
			pointBoundingRect.X -= width * 0.5f;
			pointBoundingRect.Y -= width * 0.5f;
			pointBoundingRect.W += width;
			pointBoundingRect.H += width;

			// Set up vertex array
			int vertexCount = points.Length * 2 + 2;
			VertexC1P3T2[] vertices = this.buffer.RequestVertexArray(vertexCount);
			for (int i = 0; i < points.Length; i++)
			{
				int vertexBase = i * 2;

				int cur = i;
				int prev = (i - 1 + points.Length) % points.Length;
				int next = (i + 1) % points.Length;
				
				Vector2 tangent = (points[cur] - points[prev]).Normalized;
				Vector2 tangent2 = (points[next] - points[cur]).Normalized;
				Vector2 normal = tangent.PerpendicularLeft;
				Vector2 normal2 = tangent2.PerpendicularLeft;
				
				float dot = Vector2.Dot(normal, tangent2);

				Vector2 cross;
				MathF.LinesCross(
					points[prev].X - normal.X * width, points[prev].Y - normal.Y * width, 
					points[cur].X  - normal.X * width, points[cur].Y  - normal.Y * width, 
					points[cur].X  - normal2.X * width, points[cur].Y  - normal2.Y * width,
					points[next].X - normal2.X * width, points[next].Y - normal2.Y * width,
					out cross.X, out cross.Y,
					true);

				Vector2 leftOffset = Vector2.Zero;
				Vector2 rightOffset = (tangent - tangent2).Normalized * (cross - points[cur]).Length * MathF.Sign(dot) * -2;

				vertices[vertexBase + 0].Pos.X = (points[cur].X + leftOffset.X) * scale + pos.X;
				vertices[vertexBase + 0].Pos.Y = (points[cur].Y + leftOffset.Y) * scale + pos.Y;
				vertices[vertexBase + 0].Pos.Z = pos.Z;
				vertices[vertexBase + 0].TexCoord.X = texCoordRect.X + ((points[i].X + leftOffset.X - pointBoundingRect.X) / pointBoundingRect.W) * texCoordRect.W;
				vertices[vertexBase + 0].TexCoord.Y = texCoordRect.Y + ((points[i].Y + leftOffset.Y - pointBoundingRect.Y) / pointBoundingRect.H) * texCoordRect.H;
				vertices[vertexBase + 0].Color = shapeColor;
				
				vertices[vertexBase + 1].Pos.X = (points[cur].X  + rightOffset.X) * scale + pos.X;
				vertices[vertexBase + 1].Pos.Y = (points[cur].Y  + rightOffset.Y) * scale + pos.Y;
				vertices[vertexBase + 1].Pos.Z = pos.Z;
				vertices[vertexBase + 1].TexCoord.X = texCoordRect.X + ((points[i].X + rightOffset.X - pointBoundingRect.X) / pointBoundingRect.W) * texCoordRect.W;
				vertices[vertexBase + 1].TexCoord.Y = texCoordRect.Y + ((points[i].Y + rightOffset.Y - pointBoundingRect.Y) / pointBoundingRect.H) * texCoordRect.H;
				vertices[vertexBase + 1].Color = shapeColor;
			}

			vertices[vertexCount - 2] = vertices[0];
			vertices[vertexCount - 1] = vertices[1];

			this.CurrentState.TransformVertices(vertices, pos.Xy, scale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.TriangleStrip, vertices, vertexCount);
		}

		/// <summary>
		/// Fills a three-dimensional line.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="z2"></param>
		public void FillThickLine(float x, float y, float z, float x2, float y2, float z2, float width)
		{
			Vector3 pos = new Vector3(x, y, z);
			Vector3 target = new Vector3(x2, y2, z2);
			float scale = 1.0f;
			float scale2 = 1.0f;
			
			device.PreprocessCoords(ref pos, ref scale);
			device.PreprocessCoords(ref target, ref scale2);

			Vector2 dir = (target.Xy - pos.Xy).Normalized;
			Vector2 left = dir.PerpendicularLeft * width * 0.5f * scale;
			Vector2 right = dir.PerpendicularRight * width * 0.5f * scale;
			Vector2 left2 = dir.PerpendicularLeft * width * 0.5f * scale2;
			Vector2 right2 = dir.PerpendicularRight * width * 0.5f * scale2;

			Vector2 shapeHandle = pos.Xy;
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			Rect texCoordRect = this.CurrentState.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.buffer.RequestVertexArray(4);

			vertices[0].Pos = pos + new Vector3(left);
			vertices[1].Pos = target + new Vector3(left2);
			vertices[2].Pos = target + new Vector3(right2);
			vertices[3].Pos = pos + new Vector3(right);

			vertices[0].TexCoord = texCoordRect.TopLeft;
			vertices[1].TexCoord = texCoordRect.TopRight;
			vertices[2].TexCoord = texCoordRect.BottomRight;
			vertices[3].TexCoord = texCoordRect.BottomLeft;

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.Quads, vertices, 4);
		}
		/// <summary>
		/// Fills a thick, flat line.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		public void FillThickLine(float x, float y, float x2, float y2, float width)
		{
			this.FillThickLine(x, y, 0, x2, y2, 0, width);
		}
		
		/// <summary>
		/// Fills the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="width">The rendered ovals total width.</param>
		/// <param name="height">The rendered ovals total height.</param>
		/// <param name="minAngle">The oval segments minimum angle.</param>
		/// <param name="maxAngle">The oval segments maximum angle.</param>
		/// <param name="donutWidth">If bigger than zero, a donut with the specified width is rendered instead of a completely filled oval.</param>
		public void FillOvalSegment(float x, float y, float z, float width, float height, float minAngle, float maxAngle, float donutWidth = 0.0f)
		{
			if (minAngle == maxAngle) return;
			if (width < 0.0f) { x += width; width = -width; }
			if (height < 0.0f) { y += height; height = -height; }
			if (donutWidth > MathF.Min(width, height)) donutWidth = 0.0f;
			width *= 0.5f; x += width;
			height *= 0.5f; y += height;

			Vector3 pos = new Vector3(x, y, z);
			if (!this.device.IsCoordInView(pos, MathF.Max(width, height) + this.CurrentState.TransformHandle.Length)) return;

			float scale = 1.0f;
			this.device.PreprocessCoords(ref pos, ref scale);
			width *= scale;
			height *= scale;
			donutWidth *= scale;

			if (maxAngle <= minAngle) maxAngle += MathF.Ceiling((minAngle - maxAngle) / MathF.RadAngle360) * MathF.RadAngle360;

			float angleRange = MathF.Min(maxAngle - minAngle, MathF.RadAngle360);
			int segmentNum = MathF.Clamp(MathF.RoundToInt(MathF.Pow(MathF.Max(width, height), 0.65f) * 3.5f * angleRange / MathF.RadAngle360), 4, 128);
			float angleStep = angleRange / segmentNum;
			Vector2 shapeHandle = pos.Xy - new Vector2(width, height);
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			Rect texCoordRect = this.CurrentState.TextureCoordinateRect;
			int vertexCount;
			VertexC1P3T2[] vertices;

			if (donutWidth <= 0.0f)
			{
				vertexCount = segmentNum + 2;
				vertices = this.buffer.RequestVertexArray(vertexCount);
				vertices[0].Pos = pos;
				vertices[0].Color = shapeColor;
				vertices[0].TexCoord = texCoordRect.Center;
				float angle = minAngle;
				for (int i = 1; i < vertexCount; i++)
				{
					float sin = (float)Math.Sin(angle);
					float cos = (float)Math.Cos(angle); 
					vertices[i].Pos.X = pos.X + sin * width;
					vertices[i].Pos.Y = pos.Y - cos * height;
					vertices[i].Pos.Z = pos.Z;
					vertices[i].Color = shapeColor;
					vertices[i].TexCoord.X = texCoordRect.X + (0.5f + 0.5f * sin) * texCoordRect.W;
					vertices[i].TexCoord.Y = texCoordRect.Y + (0.5f - 0.5f * cos) * texCoordRect.H;
					angle += angleStep;
				}
				this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
				this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.TriangleFan, vertices, vertexCount);
			}
			else
			{
				vertexCount = (segmentNum + 1) * 2;
				vertices = this.buffer.RequestVertexArray(vertexCount);
				float angle = minAngle;
				Vector2 donutWidthTexCoord = 0.5f * donutWidth * Vector2.One / new Vector2(width, height);
				for (int i = 0; i < vertexCount; i += 2)
				{
					float sin = (float)Math.Sin(angle);
					float cos = (float)Math.Cos(angle); 

					vertices[i + 0].Pos.X = pos.X + sin * width;
					vertices[i + 0].Pos.Y = pos.Y - cos * height;
					vertices[i + 0].Pos.Z = pos.Z;
					vertices[i + 0].Color = shapeColor;
					vertices[i + 0].TexCoord.X = texCoordRect.X + (0.5f + 0.5f * sin) * texCoordRect.W;
					vertices[i + 0].TexCoord.Y = texCoordRect.Y + (0.5f - 0.5f * cos) * texCoordRect.H;

					vertices[i + 1].Pos.X = pos.X + sin * (width - donutWidth);
					vertices[i + 1].Pos.Y = pos.Y - cos * (height - donutWidth);
					vertices[i + 1].Pos.Z = pos.Z;
					vertices[i + 1].Color = shapeColor;
					vertices[i + 1].TexCoord.X = texCoordRect.X + (0.5f + (0.5f - donutWidthTexCoord.X) * sin) * texCoordRect.W;
					vertices[i + 1].TexCoord.Y = texCoordRect.Y + (0.5f - (0.5f - donutWidthTexCoord.Y) * cos) * texCoordRect.H;

					angle += angleStep;
				}
				this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
				this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.TriangleStrip, vertices, vertexCount);
			}
		}
		/// <summary>
		/// Fills the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width">The rendered ovals total width.</param>
		/// <param name="height">The rendered ovals total height.</param>
		/// <param name="minAngle">The oval segments minimum angle.</param>
		/// <param name="maxAngle">The oval segments maximum angle.</param>
		public void FillOvalSegment(float x, float y, float width, float height, float minAngle, float maxAngle)
		{
			this.FillOvalSegment(x, y, 0, width, height, minAngle, maxAngle, 0);
		}
		/// <summary>
		/// Fills the section of a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="radius">The circles radius.</param>
		/// <param name="minAngle">The circle segments minimum angle.</param>
		/// <param name="maxAngle">The circle segments maximum angle.</param>
		/// <param name="donutWidth">If bigger than zero, a donut with the specified width is rendered instead of a completely filled circle area.</param>
		public void FillCircleSegment(float x, float y, float z, float radius, float minAngle, float maxAngle, float donutWidth = 0.0f)
		{
			this.CurrentState.TransformHandle += new Vector2(radius, radius);
			this.FillOvalSegment(x, y, z, radius * 2, radius * 2, minAngle, maxAngle, donutWidth);
			this.CurrentState.TransformHandle -= new Vector2(radius, radius);
		}
		/// <summary>
		/// Fills the section of a circle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="r"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void FillCircleSegment(float x, float y, float r, float minAngle, float maxAngle)
		{
			this.CurrentState.TransformHandle += new Vector2(r, r);
			this.FillOvalSegment(x, y, 0, r * 2, r * 2, minAngle, maxAngle, 0);
			this.CurrentState.TransformHandle -= new Vector2(r, r);
		}

		/// <summary>
		/// Fills an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void FillOval(float x, float y, float z, float width, float height)
		{
			this.FillOvalSegment(x, y, z, width, height, 0.0f, MathF.RadAngle360, 0.0f);
		}
		/// <summary>
		/// Fills an oval
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void FillOval(float x, float y, float width, float height)
		{
			this.FillOvalSegment(x, y, 0, width, height, 0.0f, MathF.RadAngle360, 0.0f);
		}
		/// <summary>
		/// Fills a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="radius"></param>
		public void FillCircle(float x, float y, float z, float radius)
		{
			this.CurrentState.TransformHandle += new Vector2(radius, radius);
			this.FillOvalSegment(x, y, z, radius * 2, radius * 2, 0.0f, MathF.RadAngle360, 0.0f);
			this.CurrentState.TransformHandle -= new Vector2(radius, radius);
		}
		/// <summary>
		/// Fills a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="r"></param>
		public void FillCircle(float x, float y, float r)
		{
			this.CurrentState.TransformHandle += new Vector2(r, r);
			this.FillOvalSegment(x, y, 0, r * 2, r * 2, 0.0f, MathF.RadAngle360, 0.0f);
			this.CurrentState.TransformHandle -= new Vector2(r, r);
		}

		/// <summary>
		/// Fills a rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void FillRect(float x, float y, float z, float width, float height)
		{
			if (width < 0.0f) { x += width; width = -width; }
			if (height < 0.0f) { y += height; height = -height; }

			Vector3 pos = new Vector3(x, y, z);
			float scale = 1.0f;
			device.PreprocessCoords(ref pos, ref scale);

			Vector2 shapeHandle = pos.Xy;
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			Rect texCoordRect = this.CurrentState.TextureCoordinateRect;
			VertexC1P3T2[] vertices = this.buffer.RequestVertexArray(4);

			vertices[0].Pos = new Vector3(pos.X, pos.Y, pos.Z);
			vertices[1].Pos = new Vector3(pos.X + width * scale, pos.Y, pos.Z);
			vertices[2].Pos = new Vector3(pos.X + width * scale, pos.Y + height * scale, pos.Z);
			vertices[3].Pos = new Vector3(pos.X, pos.Y + height * scale, pos.Z);

			vertices[0].TexCoord = texCoordRect.TopLeft;
			vertices[1].TexCoord = texCoordRect.TopRight;
			vertices[2].TexCoord = texCoordRect.BottomRight;
			vertices[3].TexCoord = texCoordRect.BottomLeft;

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.Quads, vertices, 4);
		}
		/// <summary>
		/// Fills a rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void FillRect(float x, float y, float width, float height)
		{
			this.FillRect(x, y, 0, width, height);
		}

		/// <summary>
		/// Draws the specified text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block.</param>
		public void DrawText(string text, float x, float y, float z = 0.0f, Alignment blockAlign = Alignment.TopLeft, bool drawBackground = false)
		{
			this.DrawText(new string[] { text }, x, y, z, blockAlign, drawBackground);
		}
		/// <summary>
		/// Draws the specified text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="vertices">Optional vertex cache to use for the text. If set, the texts vertices are cached and re-used for better Profile.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, use the <see cref="FormattedText"/> overload.</param>
		public void DrawText(string[] text, ref VertexC1P3T2[][] vertices, float x, float y, float z = 0.0f, Alignment blockAlign = Alignment.TopLeft, bool drawBackground = false)
		{
			if (text == null || text.Length == 0) return;

			Vector2 textSize = Vector2.Zero;
			if (blockAlign != Alignment.TopLeft)
			{
				if (textSize == Vector2.Zero) textSize = this.MeasureText(text);
				blockAlign.ApplyTo(ref x, ref y, textSize.X, textSize.Y);
			}

			if (drawBackground)
			{
				if (textSize == Vector2.Zero) textSize = this.MeasureText(text);
				Vector2 padding = new Vector2(this.CurrentState.TextFont.Res.Height, this.CurrentState.TextFont.Res.Height) * 0.35f;

				ColorFormat.ColorRgba baseColor = this.CurrentState.MaterialDirect.MainColor * this.CurrentState.ColorTint;
				const float backAlpha = 0.65f;
				float baseAlpha = (float)baseColor.A / 255.0f;
				float baseLuminance = baseColor.GetLuminance();

				this.PushState();
				this.CurrentState.SetMaterial(new Resources.BatchInfo(
					Resources.DrawTechnique.Alpha, 
					(baseLuminance > 0.5f ? ColorFormat.ColorRgba.Black : ColorFormat.ColorRgba.White).WithAlpha(baseAlpha * backAlpha)));
				this.CurrentState.ColorTint = ColorFormat.ColorRgba.White;
				this.FillRect(x - padding.X, y - padding.Y, textSize.X + padding.X * 2, textSize.Y + padding.Y * 2);
				this.PopState();
			}

			Vector3 pos = new Vector3(x, y, z);
			float scale = 1.0f;
			device.PreprocessCoords(ref pos, ref scale);
			if (this.CurrentState.TextInvariantScale) scale = 1.0f;

			Vector2 shapeHandle = pos.Xy;
			Font font = this.CurrentState.TextFont.Res;
			
			BatchInfo customMat = new BatchInfo(this.CurrentState.MaterialDirect);
			customMat.MainTexture = font.Material.MainTexture;

			// Prepare for attempt to use Canvas buffering
			if (vertices == null || vertices.Length < text.Length)
				vertices = new VertexC1P3T2[text.Length][];

			Vector2 size = Vector2.Zero;
			for (int i = 0; i < text.Length; i++)
			{
				// Attempt to use Canvas buffering
				if (vertices[i] == null || vertices[i].Length < text[i].Length * 4)
					vertices[i] = this.buffer.RequestVertexArray(text[i].Length * 4);

				int vertexCount = font.EmitTextVertices(text[i], ref vertices[i], pos.X, pos.Y, pos.Z, this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor, 0.0f, scale);

				this.CurrentState.TransformVertices(vertices[i], shapeHandle, scale);
				device.AddVertices(customMat, VertexMode.Quads, vertices[i], vertexCount);

				pos.Y += font.LineSpacing * scale;
			}
		}
		/// <summary>
		/// Draws the specified text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, use the <see cref="FormattedText"/> overload.</param>
		public void DrawText(string[] text, float x, float y, float z = 0.0f, Alignment blockAlign = Alignment.TopLeft, bool drawBackground = false)
		{
			VertexC1P3T2[][] vertices = null;
			this.DrawText(text, ref vertices, x, y, z, blockAlign, drawBackground);
		}
		/// <summary>
		/// Draws the specified formatted text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="vertText">Optional vertex cache to use for the text. If set, the texts vertices are cached and re-used for better Profile.</param>
		/// <param name="vertIcon">Optional vertex cache to use for the icons. If set, the texts vertices are cached and re-used for better Profile.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="iconMat"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, make use of <see cref="FormattedText"/> format tags.</param>
		public void DrawText(FormattedText text, ref VertexC1P3T2[][] vertText, ref VertexC1P3T2[] vertIcon, float x, float y, float z = 0.0f, BatchInfo iconMat = null, Alignment blockAlign = Alignment.TopLeft, bool drawBackground = false)
		{
			if (text == null || text.IsEmpty) return;

			if (blockAlign != Alignment.TopLeft)
				blockAlign.ApplyTo(ref x, ref y, text.Size.X, text.Size.Y);

			if (drawBackground)
			{
				Vector2 padding = new Vector2(this.CurrentState.TextFont.Res.Height, this.CurrentState.TextFont.Res.Height) * 0.35f;

				ColorFormat.ColorRgba baseColor = this.CurrentState.MaterialDirect.MainColor * this.CurrentState.ColorTint;
				const float backAlpha = 0.65f;
				float baseAlpha = (float)baseColor.A / 255.0f;
				float baseLuminance = baseColor.GetLuminance();

				this.PushState();
				this.CurrentState.SetMaterial(new Resources.BatchInfo(
					Resources.DrawTechnique.Alpha, 
					(baseLuminance > 0.5f ? ColorFormat.ColorRgba.Black : ColorFormat.ColorRgba.White).WithAlpha(baseAlpha * backAlpha)));
				this.CurrentState.ColorTint = ColorFormat.ColorRgba.White;
				this.FillRect(x - padding.X, y - padding.Y, text.Size.X + padding.X * 2, text.Size.Y + padding.Y * 2);
				this.PopState();
			}

			Vector3 pos = new Vector3(x, y, z);
			float scale = 1.0f;
			device.PreprocessCoords(ref pos, ref scale);
			if (this.CurrentState.TextInvariantScale) scale = 1.0f;

			Vector2 shapeHandle = pos.Xy;
			int[] vertLen = text.EmitVertices(ref vertText, ref vertIcon, pos.X, pos.Y, pos.Z, this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor, 0.0f, scale);
			
			if (text.Fonts != null)
			{
				for (int i = 0; i < text.Fonts.Length; i++)
				{
					if (text.Fonts[i] != null && text.Fonts[i].IsAvailable) 
					{
						this.CurrentState.TransformVertices(vertText[i], shapeHandle, scale);
						BatchInfo customMat = new BatchInfo(this.CurrentState.MaterialDirect);
						customMat.MainTexture = text.Fonts[i].Res.Material.MainTexture;
						device.AddVertices(customMat, VertexMode.Quads, vertText[i], vertLen[i + 1]);
					}
				}
			}
			if (text.Icons != null && iconMat != null)
			{
				device.AddVertices(iconMat, VertexMode.Quads, vertIcon, vertLen[0]);
			}
		}
		/// <summary>
		/// Draws the specified formatted text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="iconMat"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, make use of <see cref="FormattedText"/> format tags.</param>
		public void DrawText(FormattedText text, float x, float y, float z = 0.0f, BatchInfo iconMat = null, Alignment blockAlign = Alignment.TopLeft, bool drawBackground = false)
		{
			VertexC1P3T2[][] vertText = null;
			VertexC1P3T2[] vertIcon = null;
			this.DrawText(text, ref vertText, ref vertIcon, x, y, z, iconMat, blockAlign, drawBackground);
		}

		/// <summary>
		/// Measures the specified text using the currently used <see cref="Duality.Resources.Font"/>.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public Vector2 MeasureText(string text)
		{
			Font font = this.CurrentState.TextFont.Res;
			return font.MeasureText(text);
		}
		/// <summary>
		/// Measures the specified text using the currently used <see cref="Duality.Resources.Font"/>.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public Vector2 MeasureText(string[] text)
		{
			Font font = this.CurrentState.TextFont.Res;
			return font.MeasureText(text);
		}
	}
}