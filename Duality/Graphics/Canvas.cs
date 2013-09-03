using System;
using System.Collections.Generic;

using OpenTK.Graphics.OpenGL;
using OpenTK;

using Duality.VertexFormat;
using Duality.ColorFormat;
using Duality.Resources;

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
		public class State
		{
			private	BatchInfo			batchInfo;
			private	ColorRgba			color;
			private	ContentRef<Font>	font;
			private	float				zOffset;
			private	bool		invariantTextScale;
			private	float		transformAngle;
			private	Vector2		transformScale;
			private	Vector2		transformHandle;

			private	Vector2		curTX;
			private	Vector2		curTY;


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
				this.batchInfo = other.batchInfo;
				this.font = other.font;
				this.color = other.color;
				this.invariantTextScale = other.invariantTextScale;
				this.zOffset = other.zOffset;
				this.transformAngle = other.transformAngle;
				this.transformHandle = other.transformHandle;
				this.transformScale = other.transformScale;
				this.UpdateTransform();
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
			}
			/// <summary>
			/// Sets the States drawing material.
			/// </summary>
			/// <param name="material"></param>
			public void SetMaterial(ContentRef<Material> material)
			{
				this.batchInfo = material.Res.InfoDirect;
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
			internal void TransformVertices(VertexC1P3[] vertexData, Vector2 shapeHandle, float shapeHandleScale)
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
		}

		/// <summary>
		/// Describes a pattern for dashed lines.
		/// </summary>
		public enum DashPattern : uint
		{
			/// <summary>
			/// There is no line at all.
			/// </summary>
			Empty		= 0x0U,

			/// <summary>
			/// A dotted line with a a lot of dots.
			/// Pattern: #_#_#_#_#_#_#_#_#_#_#_#_#_#_#_#_
			/// </summary>
			DotMore		= 0xAAAAAAAAU,
			/// <summary>
			/// A dotted line.
			/// Pattern: #___#___#___#___#___#___#___#___
			/// </summary>
			Dot			= 0x88888888U,
			/// <summary>
			/// A dotted line with less dots.
			/// Pattern: #_______#_______#_______#_______
			/// </summary>
			DotLess		= 0x80808080U,

			/// <summary>
			/// A dashed line with short dashes.
			/// Pattern: ##__##__##__##__##__##__##__##__
			/// </summary>
			DashShort	= 0xCCCCCCCCU,
			/// <summary>
			/// A dashed line.
			/// Pattern: ####____####____####____####____
			/// </summary>
			Dash		= 0xF0F0F0F0U,
			/// <summary>
			/// A dashed line with long dashes.
			/// Pattern: ########________########________
			/// </summary>
			DashLong	= 0xFF00FF00U,

			/// <summary>
			/// A line with alternating dashes and dots.
			/// Pattern: ###__#__###__#__###__#__###__#__
			/// </summary>
			DashDot		= 0xE4E4E4E4U,
			/// <summary>
			/// An alternating line with more dots than dashes.
			/// Pattern: #####___#___#___#####___#___#___
			/// </summary>
			DashDotDot	= 0xF888F888U,
			/// <summary>
			/// An alternating line with more dashes than dots.
			/// Pattern: ####____####____####____#___#___
			/// </summary>
			DashDashDot	= 0xF0F0F088U,

			/// <summary>
			/// The line isn't dashed.
			/// </summary>
			Full		= 0xFFFFFFFFU
		}

		private static Dictionary<uint,Texture>	dashTextures	= new Dictionary<uint,Texture>();
		private	IDrawDevice		device		= null;
		private	Stack<State>	stateStack	= new Stack<State>(new [] { new State() });

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

		public Canvas(IDrawDevice device)
		{
			this.device = device;
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
		/// <param name="z"></param>
		public void DrawPolygon(Vector2[] points, float z = 0.0f)
		{
			Vector3 pos = new Vector3(points[0].X, points[0].Y, z);

			float scale = 1.0f;
			Vector3 posTemp = pos;
			this.device.PreprocessCoords(ref posTemp, ref scale);

			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			VertexC1P3[] vertices = new VertexC1P3[points.Length];
			for (int i = 0; i < points.Length; i++)
			{
				vertices[i].Pos.X = (points[i].X - pos.X) * scale + posTemp.X + 0.5f;
				vertices[i].Pos.Y = (points[i].Y - pos.Y) * scale + posTemp.Y + 0.5f;
				vertices[i].Pos.Z = (z - pos.Z) * scale + posTemp.Z;
				vertices[i].Color = shapeColor;
			}

			this.CurrentState.TransformVertices(vertices, pos.Xy, scale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices);
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
			VertexC1P3[] vertices;
			float angle;

			// XY circle
			vertices = new VertexC1P3[segmentNum];
			angle = 0.0f;
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i].Pos.X = pos.X + (float)Math.Sin(angle) * r;
				vertices[i].Pos.Y = pos.Y - (float)Math.Cos(angle) * r;
				vertices[i].Pos.Z = pos.Z;
				vertices[i].Color = shapeColor;
				this.device.PreprocessCoords(ref vertices[i].Pos, ref scale);
				angle += (MathF.TwoPi / segmentNum);
			}
			this.CurrentState.TransformVertices(vertices, shapeHandle, shapeHandleScale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices);

			// XZ circle
			vertices = new VertexC1P3[segmentNum];
			angle = 0.0f;
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i].Pos.X = pos.X + (float)Math.Sin(angle) * r;
				vertices[i].Pos.Y = pos.Y;
				vertices[i].Pos.Z = pos.Z - (float)Math.Cos(angle) * r;
				vertices[i].Color = shapeColor;
				this.device.PreprocessCoords(ref vertices[i].Pos, ref scale);
				angle += (MathF.TwoPi / segmentNum);
			}
			this.CurrentState.TransformVertices(vertices, shapeHandle, shapeHandleScale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices);

			// YZ circle
			vertices = new VertexC1P3[segmentNum];
			angle = 0.0f;
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i].Pos.X = pos.X;
				vertices[i].Pos.Y = pos.Y + (float)Math.Sin(angle) * r;
				vertices[i].Pos.Z = pos.Z - (float)Math.Cos(angle) * r;
				vertices[i].Color = shapeColor;
				this.device.PreprocessCoords(ref vertices[i].Pos, ref scale);
				angle += (MathF.TwoPi / segmentNum);
			}
			this.CurrentState.TransformVertices(vertices, shapeHandle, shapeHandleScale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices);
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
			VertexC1P3[] vertices = new VertexC1P3[2];
			vertices[0].Pos = pos + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[1].Pos = target + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.Lines, vertices);
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
			VertexC1P3T2[] vertices = new VertexC1P3T2[2];
			vertices[0].Pos = pos + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[1].Pos = target + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[0].TexCoord = new Vector2(0.0f, 0.0f);
			vertices[1].TexCoord = new Vector2(lineLength * patternLen / 32.0f, 0.0f);
			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;

			BatchInfo customMat = new BatchInfo(this.CurrentState.MaterialDirect);
			customMat.MainTexture = dashTextures[patternBits];
			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(customMat, VertexMode.Lines, vertices);
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
			VertexC1P3[] vertices = new VertexC1P3[4];
			vertices[0].Pos = pos + new Vector3(left);
			vertices[1].Pos = target + new Vector3(left2);
			vertices[2].Pos = target + new Vector3(right2);
			vertices[3].Pos = pos + new Vector3(right);
			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;
			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices);
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
		/// Draws a cross at the specified position.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="r"></param>
		public void DrawCross(float x, float y, float z, float r)
		{
			Vector3 pos = new Vector3(x - r, y, z);
			Vector3 pos2 = new Vector3(x, y - r, z);
			Vector3 target = new Vector3(x + r, y, z);
			Vector3 target2 = new Vector3(x, y + r, z);
			float scale = 1.0f;
			
			device.PreprocessCoords(ref pos, ref scale);
			device.PreprocessCoords(ref pos2, ref scale);
			device.PreprocessCoords(ref target, ref scale);
			device.PreprocessCoords(ref target2, ref scale);

			Vector2 shapeHandle = new Vector2(x, y);
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			VertexC1P3[] vertices = new VertexC1P3[4];
			vertices[0].Pos = pos + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[1].Pos = target + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[2].Pos = pos2 + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[3].Pos = target2 + new Vector3(0.5f, 0.5f, 0.0f);
			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;
			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.Lines, vertices);
		}
		/// <summary>
		/// Draws a cross at the specified position.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="r"></param>
		public void DrawCross(float x, float y, float r)
		{
			this.DrawCross(x, y, 0.0f, r);
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
			VertexC1P3[] vertices = new VertexC1P3[4];
			vertices[0].Pos = new Vector3(pos.X + 0.5f, pos.Y + 0.5f, pos.Z);
			vertices[1].Pos = new Vector3(pos.X + w * scale - 0.5f, pos.Y + 0.5f, pos.Z);
			vertices[2].Pos = new Vector3(pos.X + w * scale - 0.5f, pos.Y + h * scale - 0.5f, pos.Z);
			vertices[3].Pos = new Vector3(pos.X + 0.5f, pos.Y + h * scale - 0.5f, pos.Z);

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineLoop, vertices);
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
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawOvalSegment(float x, float y, float z, float w, float h, float minAngle, float maxAngle, bool outline = false)
		{
			if (minAngle == maxAngle) return;
			if (w < 0.0f) { x += w; w = -w; }
			if (h < 0.0f) { y += h; h = -h; }
			w *= 0.5f; x += w;
			h *= 0.5f; y += h;

			Vector3 pos = new Vector3(x, y, z);
			if (!this.device.IsCoordInView(pos, MathF.Max(w, h) + this.CurrentState.TransformHandle.Length)) return;

			float scale = 1.0f;
			this.device.PreprocessCoords(ref pos, ref scale);
			w *= scale;
			h *= scale;

			if (maxAngle <= minAngle) maxAngle += MathF.Ceiling((minAngle - maxAngle) / MathF.RadAngle360) * MathF.RadAngle360;

			float angleRange = MathF.Min(maxAngle - minAngle, MathF.RadAngle360);
			bool loop = angleRange >= MathF.RadAngle360 - MathF.RadAngle1 * 0.001f;

			if (loop && outline) outline = false;
			else if (outline) loop = true;

			int segmentNum = MathF.Clamp(MathF.RoundToInt(MathF.Pow(MathF.Max(w, h), 0.65f) * 3.5f * angleRange / MathF.RadAngle360), 4, 128);
			float angleStep = angleRange / segmentNum;
			Vector2 shapeHandle = pos.Xy - new Vector2(w, h);
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			VertexC1P3[] vertices = new VertexC1P3[segmentNum + (loop ? 0 : 1) + (outline ? 2 : 0)];
			float angle = minAngle;
			
			if (outline)
			{
				vertices[0].Pos.X = pos.X + 0.5f;
				vertices[0].Pos.Y = pos.Y + 0.5f;
				vertices[0].Pos.Z = pos.Z;
				vertices[0].Color = shapeColor;
			}

			// XY circle
			for (int i = outline ? 1 : 0; i < vertices.Length; i++)
			{
				vertices[i].Pos.X = pos.X + (float)Math.Sin(angle) * (w - 0.5f);
				vertices[i].Pos.Y = pos.Y - (float)Math.Cos(angle) * (h - 0.5f);
				vertices[i].Pos.Z = pos.Z;
				vertices[i].Color = shapeColor;
				angle += angleStep;
			}
			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, loop ? VertexMode.LineLoop : VertexMode.LineStrip, vertices);
		}
		/// <summary>
		/// Draws the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawOvalSegment(float x, float y, float w, float h, float minAngle, float maxAngle, bool outline = false)
		{
			this.DrawOvalSegment(x, y, 0, w, h, minAngle, maxAngle, outline);
		}
		/// <summary>
		/// Draws the section of a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="r"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawCircleSegment(float x, float y, float z, float r, float minAngle, float maxAngle, bool outline = false)
		{
			this.CurrentState.TransformHandle += new Vector2(r, r);
			this.DrawOvalSegment(x, y, z, r * 2, r * 2, minAngle, maxAngle, outline);
			this.CurrentState.TransformHandle -= new Vector2(r, r);
		}
		/// <summary>
		/// Draws the section of a circle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="r"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawCircleSegment(float x, float y, float r, float minAngle, float maxAngle, bool outline = false)
		{
			this.CurrentState.TransformHandle += new Vector2(r, r);
			this.DrawOvalSegment(x, y, 0, r * 2, r * 2, minAngle, maxAngle, outline);
			this.CurrentState.TransformHandle -= new Vector2(r, r);
		}

		/// <summary>
		/// Draws the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawOval(float x, float y, float z, float w, float h)
		{
			this.DrawOvalSegment(x, y, z, w, h, 0.0f, MathF.RadAngle360);
		}
		/// <summary>
		/// Draws the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawOval(float x, float y, float w, float h)
		{
			this.DrawOvalSegment(x, y, 0, w, h, 0.0f, MathF.RadAngle360);
		}
		/// <summary>
		/// Draws the section of a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="r"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawCircle(float x, float y, float z, float r)
		{
			this.CurrentState.TransformHandle += new Vector2(r, r);
			this.DrawOvalSegment(x, y, z, r * 2, r * 2, 0.0f, MathF.RadAngle360);
			this.CurrentState.TransformHandle -= new Vector2(r, r);
		}
		/// <summary>
		/// Draws the section of a circle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="r"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void DrawCircle(float x, float y, float r)
		{
			this.CurrentState.TransformHandle += new Vector2(r, r);
			this.DrawOvalSegment(x, y, 0, r * 2, r * 2, 0.0f, MathF.RadAngle360);
			this.CurrentState.TransformHandle -= new Vector2(r, r);
		}
		
		/// <summary>
		/// Fills a convex polygon. All vertices share the same Z value.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="z"></param>
		public void FillConvexPolygon(Vector2[] points, float z = 0.0f)
		{
			Vector3 pos = new Vector3(points[0].X, points[0].Y, z);

			float scale = 1.0f;
			Vector3 posTemp = pos;
			this.device.PreprocessCoords(ref posTemp, ref scale);

			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			VertexC1P3[] vertices = new VertexC1P3[points.Length];
			for (int i = 0; i < points.Length; i++)
			{
				vertices[i].Pos.X = (points[i].X - pos.X) * scale + posTemp.X;
				vertices[i].Pos.Y = (points[i].Y - pos.Y) * scale + posTemp.Y;
				vertices[i].Pos.Z = (z - pos.Z) * scale + posTemp.Z;
				vertices[i].Color = shapeColor;
			}

			this.CurrentState.TransformVertices(vertices, pos.Xy, scale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.Polygon, vertices);
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
			VertexC1P3[] vertices = new VertexC1P3[4];
			vertices[0].Pos = pos + new Vector3(left);
			vertices[1].Pos = target + new Vector3(left2);
			vertices[2].Pos = target + new Vector3(right2);
			vertices[3].Pos = pos + new Vector3(right);
			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;
			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.Quads, vertices);
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
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void FillOvalSegment(float x, float y, float z, float w, float h, float minAngle, float maxAngle)
		{
			if (minAngle == maxAngle) return;
			if (w < 0.0f) { x += w; w = -w; }
			if (h < 0.0f) { y += h; h = -h; }
			w *= 0.5f; x += w;
			h *= 0.5f; y += h;

			Vector3 pos = new Vector3(x, y, z);
			if (!this.device.IsCoordInView(pos, MathF.Max(w, h) + this.CurrentState.TransformHandle.Length)) return;

			float scale = 1.0f;
			this.device.PreprocessCoords(ref pos, ref scale);
			w *= scale;
			h *= scale;

			if (maxAngle <= minAngle) maxAngle += MathF.Ceiling((minAngle - maxAngle) / MathF.RadAngle360) * MathF.RadAngle360;

			float angleRange = MathF.Min(maxAngle - minAngle, MathF.RadAngle360);
			int segmentNum = MathF.Clamp(MathF.RoundToInt(MathF.Pow(MathF.Max(w, h), 0.65f) * 3.5f * angleRange / MathF.RadAngle360), 4, 128);
			float angleStep = angleRange / segmentNum;
			Vector2 shapeHandle = pos.Xy - new Vector2(w, h);
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			VertexC1P3[] vertices = new VertexC1P3[segmentNum + 2];
			float angle = minAngle;

			vertices[0].Pos = pos;
			vertices[0].Color = shapeColor;
			for (int i = 1; i < vertices.Length; i++)
			{
				vertices[i].Pos.X = pos.X + (float)Math.Sin(angle) * w;
				vertices[i].Pos.Y = pos.Y - (float)Math.Cos(angle) * h;
				vertices[i].Pos.Z = pos.Z;
				vertices[i].Color = shapeColor;
				angle += angleStep;
			}
			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			this.device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.TriangleFan, vertices);
		}
		/// <summary>
		/// Fills the section of an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void FillOvalSegment(float x, float y, float w, float h, float minAngle, float maxAngle)
		{
			this.FillOvalSegment(x, y, 0, w, h, minAngle, maxAngle);
		}
		/// <summary>
		/// Fills the section of a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="r"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		public void FillCircleSegment(float x, float y, float z, float r, float minAngle, float maxAngle)
		{
			this.CurrentState.TransformHandle += new Vector2(r, r);
			this.FillOvalSegment(x, y, z, r * 2, r * 2, minAngle, maxAngle);
			this.CurrentState.TransformHandle -= new Vector2(r, r);
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
			this.FillOvalSegment(x, y, 0, r * 2, r * 2, minAngle, maxAngle);
			this.CurrentState.TransformHandle -= new Vector2(r, r);
		}

		/// <summary>
		/// Fills an oval.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void FillOval(float x, float y, float z, float w, float h)
		{
			this.FillOvalSegment(x, y, z, w, h, 0.0f, MathF.RadAngle360);
		}
		/// <summary>
		/// Fills an oval
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void FillOval(float x, float y, float w, float h)
		{
			this.FillOvalSegment(x, y, 0, w, h, 0.0f, MathF.RadAngle360);
		}
		/// <summary>
		/// Fills a circle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="r"></param>
		public void FillCircle(float x, float y, float z, float r)
		{
			this.CurrentState.TransformHandle += new Vector2(r, r);
			this.FillOvalSegment(x, y, z, r * 2, r * 2, 0.0f, MathF.RadAngle360);
			this.CurrentState.TransformHandle -= new Vector2(r, r);
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
			this.FillOvalSegment(x, y, 0, r * 2, r * 2, 0.0f, MathF.RadAngle360);
			this.CurrentState.TransformHandle -= new Vector2(r, r);
		}

		/// <summary>
		/// Fills a rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void FillRect(float x, float y, float z, float w, float h)
		{
			if (w < 0.0f) { x += w; w = -w; }
			if (h < 0.0f) { y += h; h = -h; }

			Vector3 pos = new Vector3(x, y, z);
			float scale = 1.0f;
			device.PreprocessCoords(ref pos, ref scale);

			Vector2 shapeHandle = pos.Xy;
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			VertexC1P3[] vertices = new VertexC1P3[4];
			vertices[0].Pos = new Vector3(pos.X, pos.Y, pos.Z);
			vertices[1].Pos = new Vector3(pos.X + w * scale, pos.Y, pos.Z);
			vertices[2].Pos = new Vector3(pos.X + w * scale, pos.Y + h * scale, pos.Z);
			vertices[3].Pos = new Vector3(pos.X, pos.Y + h * scale, pos.Z);

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.Quads, vertices);
		}
		/// <summary>
		/// Fills a rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void FillRect(float x, float y, float w, float h)
		{
			this.FillRect(x, y, 0, w, h);
		}

		/// <summary>
		/// Draws a textured rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="uvX">UV x coordinate</param>
		/// <param name="uvY">UV y coordinate</param>
		/// <param name="uvW">UV coordinate width</param>
		/// <param name="uvH">UV coordinate height</param>
		public void DrawTexturedRect(float x, float y, float z, float w, float h, float uvX, float uvY, float uvW, float uvH)
		{
			if (w < 0.0f) { x += w; w = -w; }
			if (h < 0.0f) { y += h; h = -h; }

			Vector3 pos = new Vector3(x, y, z);
			float scale = 1.0f;
			device.PreprocessCoords(ref pos, ref scale);

			Texture mainTex = this.CurrentState.MaterialDirect.MainTexture.Res;
			Vector2 mainTexUVRatio = mainTex != null ? mainTex.UVRatio : Vector2.One;

			Vector2 shapeHandle = pos.Xy;
			ColorRgba shapeColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			VertexC1P3T2[] vertices = new VertexC1P3T2[4];

			vertices[0].Pos = new Vector3(pos.X, pos.Y, pos.Z);
			vertices[1].Pos = new Vector3(pos.X + w * scale, pos.Y, pos.Z);
			vertices[2].Pos = new Vector3(pos.X + w * scale, pos.Y + h * scale, pos.Z);
			vertices[3].Pos = new Vector3(pos.X, pos.Y + h * scale, pos.Z);

			vertices[0].TexCoord = new Vector2(uvX * mainTexUVRatio.X, uvY * mainTexUVRatio.Y);
			vertices[1].TexCoord = new Vector2((uvX + uvW) * mainTexUVRatio.X, uvY * mainTexUVRatio.Y);
			vertices[2].TexCoord = new Vector2((uvX + uvW) * mainTexUVRatio.X, (uvY + uvH) * mainTexUVRatio.Y);
			vertices[3].TexCoord = new Vector2(uvX * mainTexUVRatio.X, (uvY + uvH) * mainTexUVRatio.Y);

			vertices[0].Color = shapeColor;
			vertices[1].Color = shapeColor;
			vertices[2].Color = shapeColor;
			vertices[3].Color = shapeColor;

			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.Quads, vertices);
		}
		/// <summary>
		/// Draws a textured rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="uvX">UV x coordinate</param>
		/// <param name="uvY">UV y coordinate</param>
		/// <param name="uvW">UV coordinate width</param>
		/// <param name="uvH">UV coordinate height</param>
		public void DrawTexturedRect(float x, float y, float z, float uvX, float uvY, float uvW, float uvH)
		{
			Texture mainTex = this.CurrentState.MaterialDirect.MainTexture.Res;
			Vector2 mainTexSize = mainTex != null ? mainTex.Size : Vector2.One * 10.0f;
			this.DrawTexturedRect(x, y, z, mainTexSize.X, mainTexSize.Y, uvX, uvY, uvW, uvH);
		}
		/// <summary>
		/// Draws a textured rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void DrawTexturedRect(float x, float y, float z, float w, float h)
		{
			this.DrawTexturedRect(x, y, z, w, h, 0.0f, 0.0f, 1.0f, 1.0f);
		}
		/// <summary>
		/// Draws a textured rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void DrawTexturedRect(float x, float y, float z)
		{
			Texture mainTex = this.CurrentState.MaterialDirect.MainTexture.Res;
			Vector2 mainTexSize = mainTex != null ? mainTex.Size : Vector2.One * 10.0f;
			this.DrawTexturedRect(x, y, z, mainTexSize.X, mainTexSize.Y, 0.0f, 0.0f, 1.0f, 1.0f);
		}
		/// <summary>
		/// Draws a textured rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="uvX">UV x coordinate</param>
		/// <param name="uvY">UV y coordinate</param>
		/// <param name="uvW">UV coordinate width</param>
		/// <param name="uvH">UV coordinate height</param>
		public void DrawTexturedRect(float x, float y, float w, float h, float uvX, float uvY, float uvW, float uvH)
		{
			this.DrawTexturedRect(x, y, 0, w, h, uvX, uvY, uvW, uvH);
		}
		/// <summary>
		/// Draws a textured rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="uvX">UV x coordinate</param>
		/// <param name="uvY">UV y coordinate</param>
		/// <param name="uvW">UV coordinate width</param>
		/// <param name="uvH">UV coordinate height</param>
		public void DrawTexturedRect(float x, float y, float uvX, float uvY, float uvW, float uvH)
		{
			Texture mainTex = this.CurrentState.MaterialDirect.MainTexture.Res;
			Vector2 mainTexSize = mainTex != null ? mainTex.Size : Vector2.One * 10.0f;
			this.DrawTexturedRect(x, y, 0, mainTexSize.X, mainTexSize.Y, uvX, uvY, uvW, uvH);
		}
		/// <summary>
		/// Draws a textured rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void DrawTexturedRect(float x, float y, float w, float h)
		{
			this.DrawTexturedRect(x, y, 0, w, h, 0.0f, 0.0f, 1.0f, 1.0f);
		}
		/// <summary>
		/// Draws a textured rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void DrawTexturedRect(float x, float y)
		{
			Texture mainTex = this.CurrentState.MaterialDirect.MainTexture.Res;
			Vector2 mainTexSize = mainTex != null ? mainTex.Size : Vector2.One * 10.0f;
			this.DrawTexturedRect(x, y, 0, mainTexSize.X, mainTexSize.Y, 0.0f, 0.0f, 1.0f, 1.0f);
		}

		/// <summary>
		/// Draws a horizontally aligned graph.
		/// </summary>
		/// <param name="values">An array of value samples that will be represented by the graph.</param>
		/// <param name="colors">An array of color values corresponding to the supplied values. Specify null, if no coloring is required.</param>
		/// <param name="vertices">Optional vertex cache to use for the graph. If set, the graphs vertices are cached and re-used for better Profile.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void DrawHorizontalGraph(float[] values, ColorRgba[] colors, ref VertexC1P3[] vertices, float x, float y, float z, float w, float h)
		{
			if (values == null)
				throw new ArgumentNullException("values");
			if (colors != null && colors.Length != values.Length)
				throw new ArgumentException("The number of color samples needs to be equal to the number of value samples.", "colors");

			if (h > 0.0f) h--;
			if (h < 0.0f) h++;
			Vector3 pos = new Vector3(x, y, z);
			float scale = 1.0f;
			
			device.PreprocessCoords(ref pos, ref scale);

			Vector2 shapeHandle = pos.Xy;
			ColorRgba baseColor = this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor;
			float sampleXRatio = w / (float)(values.Length - 1);
			
			if (vertices == null)
				vertices = new VertexC1P3[MathF.Max(values.Length, 16)];
			else if (vertices.Length < values.Length)
				vertices = new VertexC1P3[MathF.Max(vertices.Length * 2, values.Length, 16)];

			for (int i = 0; i < values.Length; i++)
			{
				vertices[i].Pos.X = pos.X + 0.5f + i * sampleXRatio;
				vertices[i].Pos.Y = pos.Y + 0.5f + (1.0f - values[i]) * h;
				vertices[i].Pos.Z = pos.Z;
				vertices[i].Color = (colors != null) ? (baseColor * colors[i]) : baseColor;
			}
			this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
			device.AddVertices(this.CurrentState.MaterialDirect, VertexMode.LineStrip, vertices, values.Length);
		}
		/// <summary>
		/// Draws a horizontally aligned graph.
		/// </summary>
		/// <param name="values">An array of value samples that will be represented by the graph.</param>
		/// <param name="colors">An array of color values corresponding to the supplied values. Specify null, if no coloring is required.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void DrawHorizontalGraph(float[] values, ColorRgba[] colors, float x, float y, float z, float w, float h)
		{
			VertexC1P3[] vertices = null;
			this.DrawHorizontalGraph(values, colors, ref vertices, x, y, z, w, h);
		}
		/// <summary>
		/// Draws a horizontally aligned graph.
		/// </summary>
		/// <param name="values">An array of value samples that will be represented by the graph.</param>
		/// <param name="colors">An array of color values corresponding to the supplied values. Specify null, if no coloring is required.</param>
		/// <param name="vertices">Optional vertex cache to use for the graph. If set, the graphs vertices are cached and re-used for better Profile.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void DrawHorizontalGraph(float[] values, ColorRgba[] colors, ref VertexC1P3[] vertices, float x, float y, float w, float h)
		{
			this.DrawHorizontalGraph(values, colors, ref vertices, x, y, 0.0f, w, h);
		}
		/// <summary>
		/// Draws a horizontally aligned graph.
		/// </summary>
		/// <param name="values">An array of value samples that will be represented by the graph.</param>
		/// <param name="colors">An array of color values corresponding to the supplied values. Specify null, if no coloring is required.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void DrawHorizontalGraph(float[] values, ColorRgba[] colors, float x, float y, float w, float h)
		{
			VertexC1P3[] vertices = null;
			this.DrawHorizontalGraph(values, colors, ref vertices, x, y, 0.0f, w, h);
		}

		/// <summary>
		/// Draws the specified text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block.</param>
		public void DrawText(string text, float x, float y, float z = 0.0f, Alignment blockAlign = Alignment.TopLeft)
		{
			this.DrawText(new string[] { text }, x, y, z, blockAlign);
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
		public void DrawText(string[] text, ref VertexC1P3T2[] vertices, float x, float y, float z = 0.0f, Alignment blockAlign = Alignment.TopLeft)
		{
			if (text == null || text.Length == 0) return;

			if (blockAlign != Alignment.TopLeft)
			{
				Vector2 textSize = this.MeasureText(text);
				blockAlign.ApplyTo(ref x, ref y, textSize.X, textSize.Y);
			}

			Vector3 pos = new Vector3(x, y, z);
			float scale = 1.0f;
			device.PreprocessCoords(ref pos, ref scale);
			if (this.CurrentState.TextInvariantScale) scale = 1.0f;

			Vector2 shapeHandle = pos.Xy;
			Font font = this.CurrentState.TextFont.Res;
			
			BatchInfo customMat = new BatchInfo(this.CurrentState.MaterialDirect);
			customMat.MainTexture = font.Material.MainTexture;

			Vector2 size = Vector2.Zero;
			for (int i = 0; i < text.Length; i++)
			{
				int vertexCount = font.EmitTextVertices(text[i], ref vertices, pos.X, pos.Y, pos.Z, this.CurrentState.ColorTint * this.CurrentState.MaterialDirect.MainColor, 0.0f, scale);

				this.CurrentState.TransformVertices(vertices, shapeHandle, scale);
				device.AddVertices(customMat, VertexMode.Quads, vertices, vertexCount);

				pos.Y += font.LineSpacing * scale;
				vertices = null;
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
		public void DrawText(string[] text, float x, float y, float z = 0.0f, Alignment blockAlign = Alignment.TopLeft)
		{
			VertexC1P3T2[] vertices = null;
			this.DrawText(text, ref vertices, x, y, z, blockAlign);
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
		public void DrawText(FormattedText text, ref VertexC1P3T2[][] vertText, ref VertexC1P3T2[] vertIcon, float x, float y, float z = 0.0f, BatchInfo iconMat = null, Alignment blockAlign = Alignment.TopLeft)
		{
			if (text == null || text.IsEmpty) return;

			if (blockAlign != Alignment.TopLeft)
				blockAlign.ApplyTo(ref x, ref y, text.Size.X, text.Size.Y);

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
		public void DrawText(FormattedText text, float x, float y, float z = 0.0f, BatchInfo iconMat = null, Alignment blockAlign = Alignment.TopLeft)
		{
			VertexC1P3T2[][] vertText = null;
			VertexC1P3T2[] vertIcon = null;
			this.DrawText(text, ref vertText, ref vertIcon, x, y, z, iconMat, blockAlign);
		}

		/// <summary>
		/// Draws a simple background rectangle for the specified text. Its color is automatically determined
		/// based on the current state in order to generate an optimal contrast to the text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="backAlpha"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, use the <see cref="FormattedText"/> overload.</param>
		public void DrawTextBackground(string text, float x, float y, float z = 0.0f, float backAlpha = 0.65f, Alignment blockAlign = Alignment.TopLeft)
		{
			this.DrawTextBackground(this.MeasureText(text), x, y, z, backAlpha, blockAlign);
		}
		/// <summary>
		/// Draws a simple background rectangle for the specified text. Its color is automatically determined
		/// based on the current state in order to generate an optimal contrast to the text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="backAlpha"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, use the <see cref="FormattedText"/> overload.</param>
		public void DrawTextBackground(string[] text, float x, float y, float z = 0.0f, float backAlpha = 0.65f, Alignment blockAlign = Alignment.TopLeft)
		{
			this.DrawTextBackground(this.MeasureText(text), x, y, z, backAlpha, blockAlign);
		}
		/// <summary>
		/// Draws a simple background rectangle for the specified text. Its color is automatically determined
		/// based on the current state in order to generate an optimal contrast to the text.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="backAlpha"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, make use of <see cref="FormattedText"/> format tags.</param>
		public void DrawTextBackground(FormattedText text, float x, float y, float z = 0.0f, float backAlpha = 0.65f, Alignment blockAlign = Alignment.TopLeft)
		{
			this.DrawTextBackground(text.Size, x, y, z, backAlpha, blockAlign);
		}
		/// <summary>
		/// Draws a simple background rectangle for a text of the specified size. Its color is automatically determined
		/// based on the current state in order to generate an optimal contrast to the text.
		/// </summary>
		/// <param name="textSize"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="backAlpha"></param>
		/// <param name="blockAlign">Specifies the alignment of the text block. To make use of individual line alignment, use the <see cref="FormattedText"/> overload.</param>
		public void DrawTextBackground(Vector2 textSize, float x, float y, float z = 0.0f, float backAlpha = 0.65f, Alignment blockAlign = Alignment.TopLeft)
		{
			Vector2 padding = new Vector2(this.CurrentState.TextFont.Res.Height, this.CurrentState.TextFont.Res.Height) * 0.35f;
			
			blockAlign.ApplyTo(ref x, ref y, textSize.X, textSize.Y);

			ColorFormat.ColorRgba clr = this.CurrentState.MaterialDirect.MainColor * this.CurrentState.ColorTint;
			float alpha = (float)clr.A / 255.0f;
			float lum = clr.GetLuminance();

			this.PushState();
			this.CurrentState.SetMaterial(new Resources.BatchInfo(
				Resources.DrawTechnique.Alpha, 
				(lum > 0.5f ? ColorFormat.ColorRgba.Black : ColorFormat.ColorRgba.White).WithAlpha(alpha * backAlpha)));
			this.CurrentState.ColorTint = ColorFormat.ColorRgba.White;
			this.FillRect(x - padding.X, y - padding.Y, textSize.X + padding.X * 2, textSize.Y + padding.Y * 2);
			this.PopState();
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