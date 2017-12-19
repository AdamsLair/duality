using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Duality.Resources;
using Duality.Backend;

namespace Duality.Drawing
{
	[DontSerialize]
	public class DrawDevice : IDrawDevice, IDisposable
	{
		/// <summary>
		/// Represents a drawing input from <see cref="AddVertices"/> using 
		/// dynamically gathered vertices.
		/// </summary>
		private struct VertexDrawItem
		{
			public VertexDeclaration Type;
			public int Offset;
			public int Count;
			public VertexMode Mode;
			public BatchInfo Material;

			/// <summary>
			/// Determines whether this item could share the same <see cref="DrawBatch"/>
			/// with the specified other item.
			/// </summary>
			/// <param name="other"></param>
			/// <returns></returns>
			public bool CanShareBatchWith(ref VertexDrawItem other)
			{
				return
					this.Mode == other.Mode &&
					this.Type == other.Type &&
					this.Material.Equals(other.Material);
			}
			/// <summary>
			/// Determines whether the specified item could be appended as-is to this item.
			/// </summary>
			/// <param name="other"></param>
			/// <returns></returns>
			public bool CanAppend(ref VertexDrawItem other)
			{
				return
					this.Offset + this.Count == other.Offset &&
					this.Mode == other.Mode &&
					this.Type == other.Type &&
					this.Material.Equals(other.Material);
			}

			public override string ToString()
			{
				return string.Format(
					"{0}[{1}@{2}] as {3}, '{4}'",
					this.Type.DataType.Name,
					this.Count,
					this.Offset,
					this.Mode,
					this.Material);
			}
		}
		/// <summary>
		/// Represents an item in a sorting queue that is associated with 
		/// a <see cref="VertexDrawItem"/>. This struct is actually a union
		/// that uses either <see cref="SortDepth"/> or <see cref="SortIndex"/>
		/// depending on whether sorting is done based on material or depth.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		private struct SortItem
		{
			/// <summary>
			/// A drawing buffer index where the associated <see cref="VertexDrawItem"/>
			/// can be found.
			/// </summary>
			[FieldOffset(0)] public int DrawItemIndex;
			/// <summary>
			/// The material sorting index for this item.
			/// </summary>
			[FieldOffset(4)] public int SortIndex;
			/// <summary>
			/// The depth sorting reference value for this item.
			/// </summary>
			[FieldOffset(4)] public float SortDepth;

			/// <summary>
			/// Determines whether this item shares roughly the specified reference depth.
			/// </summary>
			/// <param name="otherDepth"></param>
			/// <returns></returns>
			public bool CanShareDepth(float otherDepth)
			{
				 return Math.Abs(this.SortDepth - otherDepth) < 0.00001f;
			}
			/// <summary>
			/// Adjusts this items <see cref="SortDepth"/> value to account for a merge
			/// with another item.
			/// </summary>
			/// <param name="otherDepth"></param>
			/// <param name="count"></param>
			/// <param name="otherCount"></param>
			public void MergeDepth(float otherDepth, int count, int otherCount)
			{
				this.SortDepth = 
					(float)(count * this.SortDepth + otherCount * otherDepth) /
					(float)(count + otherCount);
			}

			public override string ToString()
			{
				return string.Format(
					"Item {0}, Sort Index {1} / Depth {2:F}",
					this.DrawItemIndex,
					this.SortIndex,
					this.SortDepth);
			}
		}
		
		
		/// <summary>
		/// The default reference distance for perspective rendering.
		/// </summary>
		public const float DefaultFocusDist	= 500.0f;

		
		private bool                      disposed         = false;
		private float                     nearZ            = 50.0f;
		private float                     farZ             = 10000.0f;
		private float                     focusDist        = DefaultFocusDist;
		private ClearFlag                 clearFlags       = ClearFlag.All;
		private ColorRgba                 clearColor       = ColorRgba.TransparentBlack;
		private float                     clearDepth       = 1.0f;
		private Vector2                   targetSize       = Vector2.Zero;
		private Rect                      viewportRect     = Rect.Empty;
		private Vector3                   refPos           = Vector3.Zero;
		private float                     refAngle         = 0.0f;
		private ContentRef<RenderTarget>  renderTarget     = null;
		private RenderMode                renderMode       = RenderMode.Screen;
		private ProjectionMode            projection       = ProjectionMode.Perspective;
		private Matrix4                   matView          = Matrix4.Identity;
		private Matrix4                   matProjection    = Matrix4.Identity;
		private Matrix4                   matFinal         = Matrix4.Identity;
		private VisibilityFlag            visibilityMask   = VisibilityFlag.All;
		private int                       pickingIndex     = 0;
		private ShaderParameterCollection shaderParameters = new ShaderParameterCollection();

		private RenderOptions                renderOptions      = new RenderOptions();
		private RenderStats                  renderStats        = new RenderStats();
		private List<BatchInfo>              tempMaterialPool   = new List<BatchInfo>();
		private int                          tempMaterialIndex  = 0;
		private VertexBatchStore             drawVertices       = new VertexBatchStore();
		private RawList<VertexDrawItem>      drawBuffer         = new RawList<VertexDrawItem>();
		private RawList<SortItem>            sortBufferSolid    = new RawList<SortItem>();
		private RawList<SortItem>            sortBufferBlended  = new RawList<SortItem>();
		private RawList<SortItem>            sortBufferTemp     = new RawList<SortItem>();
		private RawList<DrawBatch>           batchBufferSolid   = new RawList<DrawBatch>();
		private RawList<DrawBatch>           batchBufferBlended = new RawList<DrawBatch>();
		private RawListPool<VertexDrawRange> batchIndexPool     = new RawListPool<VertexDrawRange>();
		private int                          numRawBatches      = 0;


		public bool Disposed
		{
			get { return this.disposed; }
		}
		public Vector3 RefCoord
		{
			get { return this.refPos; }
			set { this.refPos = value; }
		}
		public float RefAngle
		{
			get { return this.refAngle; }
			set { this.refAngle = value; }
		}
		public float FocusDist
		{
			get { return this.focusDist; }
			set { this.focusDist = value; }
		}
		public VisibilityFlag VisibilityMask
		{
			get { return this.visibilityMask; }
			set { this.visibilityMask = value; }
		}
		public float NearZ
		{
			get { return this.nearZ; }
			set { this.nearZ = value; }
		}
		public float FarZ
		{
			get { return this.farZ; }
			set { this.farZ = value; }
		}
		/// <summary>
		/// [GET / SET] The clear color to apply when clearing the color buffer.
		/// </summary>
		public ColorRgba ClearColor
		{
			get { return this.clearColor; }
			set { this.clearColor = value; }
		}
		/// <summary>
		/// [GET / SET] The clear depth to apply when clearing the depth buffer
		/// </summary>
		public float ClearDepth
		{
			get { return this.clearDepth; }
			set { this.clearDepth = value; }
		}
		/// <summary>
		/// [GET / SET] Specifies which buffers to clean before rendering with this device.
		/// </summary>
		public ClearFlag ClearFlags
		{
			get { return this.clearFlags; }
			set { this.clearFlags = value; }
		}
		/// <summary>
		/// [GET / SET] Specified the projection that is applied when rendering the world.
		/// </summary>
		public ProjectionMode Projection
		{
			get { return this.projection; }
			set { this.projection = value; }
		}
		public ContentRef<RenderTarget> Target
		{
			get { return this.renderTarget; }
			set { this.renderTarget = value; }
		}
		public int PickingIndex
		{
			get { return this.pickingIndex; }
			set { this.pickingIndex = value; }
		}
		public bool IsPicking
		{
			get { return this.pickingIndex != 0; }
		}
		public RenderMode RenderMode
		{
			get { return this.renderMode; }
			set { this.renderMode = value; }
		}
		public Rect ViewportRect
		{
			get { return this.viewportRect; }
			set { this.viewportRect = value; }
		}
		public Vector2 TargetSize
		{
			get { return this.targetSize; }
			set { this.targetSize = value; }
		}
		public bool DepthWrite
		{
			get { return this.renderMode != RenderMode.Screen; }
		}
		/// <summary>
		/// [GET] Provides access to the drawing devices shared <see cref="ShaderParameterCollection"/>,
		/// which allows to specify a parameter value globally across all materials rendered by this
		/// <see cref="DrawDevice"/>.
		/// </summary>
		public ShaderParameterCollection ShaderParameters
		{
			get { return this.shaderParameters; }
		}


		public DrawDevice() { }
		~DrawDevice()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool manually)
		{
			if (!this.disposed)
			{
				// Release Resources
				this.disposed = true;

				// Set big object references to null to make
				// sure they're garbage collected even when keeping
				// a reference to the disposed DrawDevice around.
				this.renderOptions = null;
				this.renderStats = null;
				this.tempMaterialPool = null;
				this.drawVertices = null;
				this.drawBuffer = null;
				this.sortBufferSolid = null;
				this.sortBufferBlended = null;
				this.sortBufferTemp = null;
				this.batchBufferSolid = null;
				this.batchBufferBlended = null;
				this.batchIndexPool = null;
			}
		}

		
		/// <summary>
		/// Returns the scale factor of objects that are located at the specified (world space) z-Coordinate.
		/// </summary>
		/// <param name="z"></param>
		/// <returns></returns>
		public float GetScaleAtZ(float z)
		{
			if (this.projection == ProjectionMode.Perspective)
				return this.focusDist / Math.Max(z - this.refPos.Z, this.nearZ);
			else
				return this.focusDist / DefaultFocusDist;
		}
		/// <summary>
		/// Transforms screen space coordinates to world space coordinates. The screen positions Z coordinate is
		/// interpreted as the target world Z coordinate.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <returns></returns>
		public Vector3 GetSpaceCoord(Vector3 screenPos)
		{
			float targetZ = screenPos.Z;

			// Since screenPos.Z is expected to be a world coordinate, first make that relative
			Vector3 gameObjPos = this.refPos;
			screenPos.Z -= gameObjPos.Z;

			Vector2 targetSize = this.TargetSize;
			screenPos.X -= targetSize.X / 2;
			screenPos.Y -= targetSize.Y / 2;

			MathF.TransformCoord(ref screenPos.X, ref screenPos.Y, this.refAngle);
			
			// Revert active perspective effect
			float scaleTemp;
			if (this.projection == ProjectionMode.Orthographic)
			{
				// Scale globally
				scaleTemp = DefaultFocusDist / this.focusDist;
				screenPos.X *= scaleTemp;
				screenPos.Y *= scaleTemp;
			}
			else if (this.projection == ProjectionMode.Perspective)
			{
				// Scale distance-based
				scaleTemp = Math.Max(screenPos.Z, this.nearZ) / this.focusDist;
				screenPos.X *= scaleTemp;
				screenPos.Y *= scaleTemp;
			}
			//else if (this.perspective == PerspectiveMode.Isometric)
			//{
			//    // Scale globally
			//    scaleTemp = DefaultFocusDist / this.focusDist;
			//    screenPos.X *= scaleTemp;
			//    screenPos.Y *= scaleTemp;
				
			//    // Revert isometric projection
			//    screenPos.Z += screenPos.Y;
			//    screenPos.Y -= screenPos.Z;
			//    screenPos.Z += this.focusDist;
			//}
			
			// Make coordinates absolte
			screenPos.X += gameObjPos.X;
			screenPos.Y += gameObjPos.Y;
			screenPos.Z += gameObjPos.Z;

			//// For isometric projection, assure we'll meet the target Z value.
			//if (this.perspective == PerspectiveMode.Isometric)
			//{
			//    screenPos.Y += screenPos.Z - targetZ;
			//    screenPos.Z = targetZ;
			//}

			return screenPos;
		}
		/// <summary>
		/// Transforms world space coordinates to screen space coordinates.
		/// </summary>
		/// <param name="spacePos"></param>
		/// <returns></returns>
		public Vector3 GetScreenCoord(Vector3 spacePos)
		{
			// Make coordinates relative to the Camera
			Vector3 gameObjPos = this.refPos;
			spacePos.X -= gameObjPos.X;
			spacePos.Y -= gameObjPos.Y;
			spacePos.Z -= gameObjPos.Z;

			// Apply active perspective effect
			float scaleTemp;
			if (this.projection == ProjectionMode.Orthographic)
			{
				// Scale globally
				scaleTemp = this.focusDist / DefaultFocusDist;
				spacePos.X *= scaleTemp;
				spacePos.Y *= scaleTemp;
			}
			else if (this.projection == ProjectionMode.Perspective)
			{
				// Scale distance-based
				scaleTemp = this.focusDist / Math.Max(spacePos.Z, this.nearZ);
				spacePos.X *= scaleTemp;
				spacePos.Y *= scaleTemp;
			}

			MathF.TransformCoord(ref spacePos.X, ref spacePos.Y, -this.refAngle);

			Vector2 targetSize = this.TargetSize;
			spacePos.X += targetSize.X / 2;
			spacePos.Y += targetSize.Y / 2;
			
			// Since the result Z value is expected to be a world coordinate, make it absolute
			spacePos.Z += gameObjPos.Z;
			return spacePos;
		}

		public void PreprocessCoords(ref Vector3 pos, ref float scale) { }
		public bool IsCoordInView(Vector3 c, float boundRad)
		{
			if (this.renderMode == RenderMode.Screen)
			{
				if (c.Z < this.nearZ) return false;
			}
			else if (c.Z <= this.refPos.Z) return false;

			// Retrieve center vertex coord
			float scaleTemp = 1.0f;
			this.PreprocessCoords(ref c, ref scaleTemp);

			// Apply final (modelview and projection) matrix
			Vector3 oldPosTemp = c;
			Vector3.Transform(ref oldPosTemp, ref this.matFinal, out c);

			// Apply projection matrices XY rotation and scale to bounding radius
			boundRad *= scaleTemp;
			Vector2 boundRadVec = new Vector2(
				boundRad * Math.Abs(this.matFinal.Row0.X) + boundRad * Math.Abs(this.matFinal.Row1.X),
				boundRad * Math.Abs(this.matFinal.Row0.Y) + boundRad * Math.Abs(this.matFinal.Row1.Y));

			return 
				c.Z >= -1.0f &&
				c.Z <= 1.0f &&
				c.X >= -1.0f - boundRadVec.X &&
				c.Y >= -1.0f - boundRadVec.Y &&
				c.X <= 1.0f + boundRadVec.X &&
				c.Y <= 1.0f + boundRadVec.Y;
		}
		
		/// <summary>
		/// Rents a temporary material instance which can be used for rendering. The instance
		/// is returned implicitly when the device is done with the current rendering operation.
		/// </summary>
		/// <returns></returns>
		public BatchInfo RentMaterial()
		{
			int index = this.tempMaterialIndex++;

			if (index >= this.tempMaterialPool.Count)
				this.tempMaterialPool.Add(new BatchInfo());

			return this.tempMaterialPool[index];
		}

		public void AddVertices<T>(BatchInfo material, VertexMode vertexMode, T[] vertexBuffer, int vertexCount) where T : struct, IVertexData
		{
			if (vertexCount == 0) return;
			if (vertexBuffer == null || vertexBuffer.Length == 0) return;
			if (vertexCount > vertexBuffer.Length) vertexCount = vertexBuffer.Length;
			if (material == null) material = Material.Checkerboard.Res.Info;

			// In picking mode, override incoming vertices material and vertex colors
			// to generate a lookup texture by which we can retrieve each pixels object.
			if (this.pickingIndex != 0)
			{
				ColorRgba clr = new ColorRgba((this.pickingIndex << 8) | 0xFF);
				for (int i = 0; i < vertexCount; ++i)
					vertexBuffer[i].Color = clr;

				material = this.RentMaterial(material);
				material.Technique = DrawTechnique.Picking;
				material.MainColor = ColorRgba.White;
			}
			else if (material.Technique == null || !material.Technique.IsAvailable)
			{
				material = this.RentMaterial(material);
				material.Technique = DrawTechnique.Solid;
			}
			
			// Move the added vertices to an internal shared buffer
			VertexSlice<T> slice = this.drawVertices.Rent<T>(vertexCount);
			Array.Copy(vertexBuffer, 0, slice.Data, slice.Offset, slice.Length);

			// Aggregate all info we have about our incoming vertices
			VertexDrawItem drawItem = new VertexDrawItem
			{
				Type = VertexDeclaration.Get<T>(),
				Offset = slice.Offset,
				Count = slice.Length,
				Mode = vertexMode,
				Material = material
			};
			
			// Determine whether we need depth sorting and calculate a reference depth
			bool sortByDepth = !this.DepthWrite || material.Technique.Res.NeedsZSort;
			RawList<SortItem> sortBuffer = sortByDepth ? this.sortBufferBlended : this.sortBufferSolid;
			SortItem sortItem = new SortItem();
			if (sortByDepth)
			{
				sortItem.SortDepth = this.CalcZSortIndex<T>(vertexBuffer, vertexCount);
			}

			// Determine whether we can batch the new vertex item with the previous one
			int prevDrawIndex = -1;
			if (vertexMode.IsBatchableMode() && sortBuffer.Count > 0)
			{
				// Since we require a complete material match to append items, we can
				// assume that the previous items sortByDepth value is the same as ours.
				SortItem prevSortItem = sortBuffer.Data[sortBuffer.Count - 1];

				// Compare the previously added item with the new one. If everything
				// except the vertices themselves is the same, we can append them directly.
				if (this.drawBuffer.Data[prevSortItem.DrawItemIndex].CanAppend(ref drawItem) && 
					(!sortByDepth || sortItem.CanShareDepth(prevSortItem.SortDepth)))
				{
					prevDrawIndex = prevSortItem.DrawItemIndex;
				}
			}

			// Append the new item directly to the previous one, or add it as a new one
			if (prevDrawIndex != -1)
			{
				if (sortByDepth)
				{
					sortBuffer.Data[sortBuffer.Count - 1].MergeDepth(
						sortItem.SortDepth, 
						this.drawBuffer.Data[prevDrawIndex].Count, 
						drawItem.Count);
				}
				this.drawBuffer.Data[prevDrawIndex].Count += drawItem.Count;
			}
			else
			{
				sortItem.DrawItemIndex = this.drawBuffer.Count;
				sortBuffer.Add(sortItem);
				this.drawBuffer.Add(drawItem);
			}

			++this.numRawBatches;
		}
		/// <summary>
		/// Generates a single drawcall that renders a fullscreen quad using the specified material.
		/// Assumes that the <see cref="DrawDevice"/> is set up to render in screen space.
		/// </summary>
		/// <param name="material"></param>
		/// <param name="resizeMode"></param>
		public void AddFullscreenQuad(BatchInfo material, TargetResize resizeMode)
		{
			Texture tex = material.MainTexture.Res;
			Vector2 uvRatio = tex != null ? tex.UVRatio : Vector2.One;
			Point2 inputSize = tex != null ? tex.ContentSize : Point2.Zero;

			// Fit the input material rect to the output size according to rendering step config
			Vector2 targetSize = resizeMode.Apply(inputSize, this.TargetSize);
			Rect targetRect = Rect.Align(
				Alignment.Center, 
				this.TargetSize.X * 0.5f, 
				this.TargetSize.Y * 0.5f, 
				targetSize.X, 
				targetSize.Y);

			// Fit the target rect to actual pixel coordinates to avoid unnecessary filtering offsets
			targetRect.X = (int)targetRect.X;
			targetRect.Y = (int)targetRect.Y;
			targetRect.W = MathF.Ceiling(targetRect.W);
			targetRect.H = MathF.Ceiling(targetRect.H);

			VertexC1P3T2[] vertices = new VertexC1P3T2[4];

			vertices[0].Pos = new Vector3(targetRect.LeftX, targetRect.TopY, 0.0f);
			vertices[1].Pos = new Vector3(targetRect.RightX, targetRect.TopY, 0.0f);
			vertices[2].Pos = new Vector3(targetRect.RightX, targetRect.BottomY, 0.0f);
			vertices[3].Pos = new Vector3(targetRect.LeftX, targetRect.BottomY, 0.0f);

			vertices[0].TexCoord = new Vector2(0.0f, 0.0f);
			vertices[1].TexCoord = new Vector2(uvRatio.X, 0.0f);
			vertices[2].TexCoord = new Vector2(uvRatio.X, uvRatio.Y);
			vertices[3].TexCoord = new Vector2(0.0f, uvRatio.Y);

			vertices[0].Color = ColorRgba.White;
			vertices[1].Color = ColorRgba.White;
			vertices[2].Color = ColorRgba.White;
			vertices[3].Color = ColorRgba.White;

			this.AddVertices(material, VertexMode.Quads, vertices);
		}

		public void UpdateMatrices()
		{
			this.GenerateViewMatrix(out this.matView);
			this.GenerateProjectionMatrix(new Rect(this.targetSize), out this.matProjection);
			this.matFinal = this.matView * this.matProjection;
		}
		public void PrepareForDrawcalls()
		{
			// Recalculate matrices according to current mode
			this.UpdateMatrices();
		}
		public void Render()
		{
			if (DualityApp.GraphicsBackend == null) return;

			// Prepare forwarding the collected data and parameters to the graphics backend
			this.AggregateBatches();
			this.UpdateBuiltinShaderParameters();

			this.renderOptions.ClearFlags = this.clearFlags;
			this.renderOptions.ClearColor = this.clearColor;
			this.renderOptions.ClearDepth = this.clearDepth;
			this.renderOptions.Viewport = this.viewportRect;
			this.renderOptions.RenderMode = this.renderMode;
			this.renderOptions.ViewMatrix = this.matView;
			this.renderOptions.ProjectionMatrix = this.matProjection;
			this.renderOptions.Target = this.renderTarget.IsAvailable ? this.renderTarget.Res.Native : null;
			this.renderOptions.ShaderParameters = this.shaderParameters;

			this.renderStats.Reset();

			// Invoke graphics backend functionality to do the rendering
			DualityApp.GraphicsBackend.BeginRendering(this, this.drawVertices, this.renderOptions, this.renderStats);
			{
				Profile.TimeProcessDrawcalls.BeginMeasure();

				// Sorted as needed by batch optimizer
				DualityApp.GraphicsBackend.Render(this.batchBufferSolid);
				// Z-Sorted, back to Front
				DualityApp.GraphicsBackend.Render(this.batchBufferBlended);

				Profile.TimeProcessDrawcalls.EndMeasure();
			}
			DualityApp.GraphicsBackend.EndRendering();
			Profile.StatNumDrawcalls.Add(this.renderStats.DrawCalls);

			// Reset all temp materials and return them to the pool
			for (int i = 0; i < this.tempMaterialIndex; i++)
			{
				this.tempMaterialPool[i].Technique = DrawTechnique.Mask;
				this.tempMaterialPool[i].Reset();
			}
			this.tempMaterialIndex = 0;

			// Clear all working buffers for vertex and drawcall processing
			this.drawBuffer.Clear();
			this.sortBufferSolid.Clear();
			this.sortBufferBlended.Clear();
			this.sortBufferTemp.Clear();
			this.batchBufferSolid.Clear();
			this.batchBufferBlended.Clear();
			this.drawVertices.Clear();
			this.batchIndexPool.Reset();
		}


		private float CalcZSortIndex<T>(T[] vertices, int count) where T : struct, IVertexData
		{
			if (count < 0) count = vertices.Length;

			// Require double precision, so we don't get "z fighting" issues in our sort.
			double zSortIndex = 0.0d;
			for (int i = 0; i < count; i++)
			{
				zSortIndex += vertices[i].Pos.Z;
			}
			return (float)(zSortIndex / (double)count);
		}

		private void GenerateViewMatrix(out Matrix4 viewMat)
		{
			viewMat = Matrix4.Identity;
			if (this.renderMode == RenderMode.World)
			{
				// Translate opposite to camera position
				viewMat *= Matrix4.CreateTranslation(-this.refPos);
				// Rotate opposite to camera angle
				viewMat *= Matrix4.CreateRotationZ(-this.refAngle);
			}
		}
		private void GenerateProjectionMatrix(Rect orthoAbs, out Matrix4 projMat)
		{
			// Flip Z direction from "out of the screen" to "into the screen".
			Matrix4 flipZDir;
			Matrix4.CreateScale(1.0f, 1.0f, -1.0f, out flipZDir);

			if (this.renderMode == RenderMode.Screen)
			{
				// When rendering in screen space, all reasonable positive depth should be valid,
				// so we'll ignore any of the projection specific near and far plane settings.
				Matrix4.CreateOrthographicOffCenter(
					orthoAbs.X,
					orthoAbs.X + orthoAbs.W, 
					orthoAbs.Y + orthoAbs.H, 
					orthoAbs.Y, 
					// These values give us a linear depth precision of ~0.006 at 24 bit
					0.0f, 
					100000.0f,
					out projMat);

				projMat = flipZDir * projMat;
			}
			else
			{
				if (this.projection == ProjectionMode.Orthographic)
				{
					Matrix4.CreateOrthographicOffCenter(
						orthoAbs.X - orthoAbs.W * 0.5f,
						orthoAbs.X + orthoAbs.W * 0.5f, 
						orthoAbs.Y + orthoAbs.H * 0.5f, 
						orthoAbs.Y - orthoAbs.H * 0.5f,
						this.nearZ, 
						this.farZ,
						out projMat);

					projMat = flipZDir * projMat;
				}
				else
				{
					// Clamp near plane to above-zero, so we avoid division by zero problems
					float clampedNear = MathF.Max(this.nearZ, 1.0f);

					Matrix4.CreatePerspectiveOffCenter(
						orthoAbs.X - orthoAbs.W * 0.5f, 
						orthoAbs.X + orthoAbs.W * 0.5f, 
						orthoAbs.Y + orthoAbs.H * 0.5f, 
						orthoAbs.Y - orthoAbs.H * 0.5f, 
						clampedNear, 
						this.farZ,
						out projMat);

					projMat = flipZDir * projMat;

					// Apply custom "focus distance", where objects appear at 1:1 scale.
					// Otherwise, that distance would be the near plane. 
					projMat.M33 *= clampedNear / this.focusDist; // Output Z scale
					projMat.M43 *= clampedNear / this.focusDist; // Output Z offset
					projMat.M34 *= clampedNear / this.focusDist; // Perspective divide scale
				}
			}
		}

		/// <summary>
		/// Updates all <see cref="BuiltinShaderFields"/> in the devices shared <see cref="ShaderParameters"/>
		/// to match its current configuration.
		/// </summary>
		private void UpdateBuiltinShaderParameters()
		{
			this.shaderParameters.Set(BuiltinShaderFields.RealTime, (float)Time.MainTimer.TotalSeconds);
			this.shaderParameters.Set(BuiltinShaderFields.GameTime, (float)Time.GameTimer.TotalSeconds);
			this.shaderParameters.Set(BuiltinShaderFields.DeltaTime, Time.DeltaTime);
			this.shaderParameters.Set(BuiltinShaderFields.FrameCount, Time.FrameCount);

			this.shaderParameters.Set(BuiltinShaderFields.CameraPosition, this.refPos);
			this.shaderParameters.Set(BuiltinShaderFields.CameraParallax, this.projection == ProjectionMode.Perspective);
			this.shaderParameters.Set(BuiltinShaderFields.CameraFocusDist, this.focusDist);
		}

		private static int MaterialSortComparison(SortItem first, SortItem second)
		{
			return first.SortIndex - second.SortIndex;
		}
		private static int DepthSortComparison(SortItem first, SortItem second)
		{
			if (second.SortDepth < first.SortDepth) return -1;
			if (second.SortDepth > first.SortDepth) return 1;
			if (second.SortDepth == first.SortDepth) return 0;
			if (float.IsNaN(second.SortDepth))
				return (float.IsNaN(first.SortDepth) ? 0 : -1);
			else
				return 1;
		}
		private void AggregateBatches()
		{
			int batchCountBefore = this.sortBufferSolid.Count + this.sortBufferBlended.Count;
			if (this.pickingIndex == 0) Profile.TimeOptimizeDrawcalls.BeginMeasure();

			// Material-sorted (solid) batches
			if (this.sortBufferSolid.Count > 0)
			{
				// Assign material and vertex type based sort indices to each item
				this.AssignMaterialSortIDs(this.sortBufferSolid, this.drawBuffer);

				// Stable sort assures maintaining draw order for batches of equal material
				this.sortBufferTemp.Count = this.sortBufferSolid.Count;
				this.sortBufferSolid.StableSort(this.sortBufferTemp, MaterialSortComparison);
				this.sortBufferTemp.Clear();

				// Sweep over the sorted draws and aggregate as many as possible into a single batch
				this.AggregateBatches(this.sortBufferSolid, this.drawBuffer, this.batchBufferSolid);
			}

			// Depth-sorted (blended) batches
			if (this.sortBufferBlended.Count > 0)
			{
				// Stable sort assures maintaining draw order for batches of equal depth
				this.sortBufferTemp.Count = this.sortBufferBlended.Count;
				this.sortBufferBlended.StableSort(this.sortBufferTemp, DepthSortComparison);
				this.sortBufferTemp.Clear();

				// Sweep over the sorted draws and aggregate as many as possible into a single batch
				this.AggregateBatches(this.sortBufferBlended, this.drawBuffer, this.batchBufferBlended);
			}

			if (this.pickingIndex == 0) Profile.TimeOptimizeDrawcalls.EndMeasure();
			int batchCountAfter = this.batchBufferSolid.Count + this.batchBufferBlended.Count;

			Profile.StatNumRawBatches.Add(this.numRawBatches);
			Profile.StatNumMergedBatches.Add(batchCountBefore);
			Profile.StatNumOptimizedBatches.Add(batchCountAfter);
			this.numRawBatches = 0;
		}
		private void AssignMaterialSortIDs(RawList<SortItem> sortItems, RawList<VertexDrawItem> drawItems)
		{
			VertexDrawItem[] drawData = drawItems.Data;
			SortItem[] sortData = sortItems.Data;
			int count = sortItems.Count;
			for (int i = 0; i < sortData.Length; i++)
			{
				if (i >= count) break;
				
				int drawIndex = sortData[i].DrawItemIndex;
				int vertexTypeIndex = drawData[drawIndex].Type.TypeIndex;
				VertexMode vertexMode = drawData[drawIndex].Mode;
				BatchInfo material = drawData[drawIndex].Material;

				int matHash;
				unchecked
				{
					// Avoid just "cutting off" parts of the original hash,
					// as this is likely to lead to collisions.
					matHash = material.GetHashCode();
					matHash = (13 * matHash + 17 * (matHash >> 9)) % (1 << 23);
				}

				// Bit significancy is used to achieve sorting by multiple traits at once.
				// The higher a traits bit significancy, the higher its priority when sorting.
				sortData[i].SortIndex = 
					(((int)vertexMode & 15) << 0) | //                           XXXX  4 Bit   Vertex Mode  Offset 4
					((matHash & 8388607) << 4) |    //    XXXXXXXXXXXXXXXXXXXXXXXaaaa  23 Bit  Material     Offset 27
					((vertexTypeIndex & 15) << 27); // XXXbbbbbbbbbbbbbbbbbbbbbbbaaaa  4 Bit   Vertex Type  Offset 31

				// Keep an eye on this. If for example two material hash codes randomly have the same 23 lower bits, they
				// will be sorted as if equal, resulting in blocking batch aggregation.
			}
		}
		private void AggregateBatches(RawList<SortItem> sortItems, RawList<VertexDrawItem> drawItems, RawList<DrawBatch> batches)
		{
			VertexDrawItem[] drawData = drawItems.Data;
			SortItem[] sortData = sortItems.Data;

			SortItem activeSortItem = sortData[0];
			VertexDrawItem activeItem = drawData[activeSortItem.DrawItemIndex];
			int beginBatchIndex = 0;

			// Find sequences of draw items that can be batched together
			int count = sortItems.Count;
			for (int sortIndex = 1; sortIndex <= count; sortIndex++)
			{
				// Skip items until we can no longer put the next one into the same batch
				if (sortIndex < count)
				{
					SortItem sortItem = sortData[sortIndex];
					if (activeItem.CanShareBatchWith(ref drawData[sortItem.DrawItemIndex]))
						continue;
				}

				// Create a batch for all previous items
				DrawBatch batch = new DrawBatch(
					activeItem.Type, 
					this.batchIndexPool.Rent(sortIndex - beginBatchIndex), 
					activeItem.Mode,
					activeItem.Material);

				for (int i = beginBatchIndex; i < sortIndex; i++)
				{
					batch.VertexRanges.Add(new VertexDrawRange
					{
						Index = drawData[sortData[i].DrawItemIndex].Offset,
						Count = drawData[sortData[i].DrawItemIndex].Count
					});
				}

				batches.Add(batch);

				// Proceed with the current item being the new sharing reference
				if (sortIndex < count)
				{
					beginBatchIndex = sortIndex;
					activeSortItem = sortData[sortIndex];
					activeItem = drawData[activeSortItem.DrawItemIndex];
				}
			}
		}

		public static void RenderVoid(Rect viewportRect)
		{
			RenderVoid(viewportRect, ColorRgba.TransparentBlack);
		}
		public static void RenderVoid(Rect viewportRect, ColorRgba color)
		{
			if (DualityApp.GraphicsBackend == null) return;

			RenderOptions options = new RenderOptions
			{
				ClearFlags = ClearFlag.All,
				ClearColor = color,
				ClearDepth = 1.0f,
				Viewport = viewportRect,
				RenderMode = RenderMode.Screen
			};
			DualityApp.GraphicsBackend.BeginRendering(null, null, options);
			DualityApp.GraphicsBackend.EndRendering();
		}
	}
}
