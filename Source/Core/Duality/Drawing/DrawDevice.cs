using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Resources;
using Duality.Backend;

namespace Duality.Drawing
{
	[DontSerialize]
	public class DrawDevice : IDrawDevice, IDisposable
	{
		private class DrawBatch<T> : IDrawBatch where T : struct, IVertexData
		{
			private static T[] uploadBuffer = null;

			private	T[]			vertices	= null;
			private	int			vertexCount	= 0;
			private	int			sortIndex	= 0;
			private	float		zSortIndex	= 0.0f;
			private	VertexMode	vertexMode	= VertexMode.Points;
			private	BatchInfo	material	= null;

			public int SortIndex
			{
				get { return this.sortIndex; }
			}
			public float ZSortIndex
			{
				get { return this.zSortIndex; }
			}
			public int VertexCount
			{
				get { return this.vertexCount; }
			}
			public VertexMode VertexMode
			{
				get { return this.vertexMode; }
			}
			public VertexDeclaration VertexDeclaration
			{
				get { return this.vertices[0].Declaration; }
			}
			public BatchInfo Material
			{
				get { return this.material; }
			}

			public DrawBatch(BatchInfo material, VertexMode vertexMode, T[] vertices, int vertexCount, float zSortIndex)
			{
				if (vertices == null || vertices.Length == 0) throw new ArgumentException("A zero-vertex DrawBatch is invalid.");
				
				// Assign data
				this.vertexCount = Math.Min(vertexCount, vertices.Length);
				this.vertices = vertices;
				this.material = material;
				this.vertexMode = vertexMode;
				this.zSortIndex = zSortIndex;

				// Determine sorting index for non-Z-Sort materials
				if (!this.material.Technique.Res.NeedsZSort)
				{
					int vTypeSI = vertices[0].Declaration.TypeIndex;
					int matHash = this.material.GetHashCode() % (1 << 23);

					// Bit significancy is used to achieve sorting by multiple traits at once.
					// The higher a traits bit significancy, the higher its priority when sorting.
					this.sortIndex = 
						(((int)vertexMode & 15) << 0) |		//							  XXXX	4 Bit	Vertex Mode		Offset 4
						((matHash & 8388607) << 4) |		//	   XXXXXXXXXXXXXXXXXXXXXXXaaaa	23 Bit	Material		Offset 27
						((vTypeSI & 15) << 27);				//	XXXbbbbbbbbbbbbbbbbbbbbbbbaaaa	4 Bit	Vertex Type		Offset 31

					// Keep an eye on this. If for example two material hash codes randomly have the same 23 lower bits, they
					// will be sorted as if equal, resulting in blocking batch aggregation.
				}
			}

			public void UploadVertices(IVertexUploader target, List<IDrawBatch> uploadBatches)
			{
				int vertexCount = 0;
				T[] vertexData = null;

				if (uploadBatches.Count == 1)
				{
					// Only one batch? Don't bother copying data
					DrawBatch<T> b = uploadBatches[0] as DrawBatch<T>;
					vertexData = b.vertices;
					vertexCount = b.VertexCount;
				}
				else
				{
					// Check how many vertices we got
					vertexCount = uploadBatches.Sum(t => t.VertexCount);
					
					// Allocate a static / shared buffer for uploading vertices
					if (uploadBuffer == null)
						uploadBuffer = new T[Math.Max(vertexCount, 64)];
					else if (uploadBuffer.Length < vertexCount)
						Array.Resize(ref uploadBuffer, Math.Max(vertexCount, uploadBuffer.Length * 2));

					// Collect vertex data in one array
					int curVertexPos = 0;
					vertexData = uploadBuffer;
					for (int i = 0; i < uploadBatches.Count; i++)
					{
						DrawBatch<T> b = uploadBatches[i] as DrawBatch<T>;
						Array.Copy(b.vertices, 0, vertexData, curVertexPos, b.vertexCount);
						curVertexPos += b.vertexCount;
					}
				}

				// Submit vertex data to the GPU
				target.UploadBatchVertices<T>(this.vertices[0].Declaration, vertexData, vertexCount);
			}

			public bool SameVertexType(IDrawBatch other)
			{
				return other is DrawBatch<T>;
			}
			public bool CanAppendJIT<U>(float invZSortAccuracy, float zSortIndex, BatchInfo material, VertexMode vertexMode) where U : struct, IVertexData
			{
				if (invZSortAccuracy != 0.0f && Math.Abs(zSortIndex - this.ZSortIndex) > 0.0000001f) return false;
				return 
					vertexMode == this.vertexMode && 
					this is DrawBatch<U> &&
					this.vertexMode.IsBatchableMode() &&
					material == this.material;
			}
			public void AppendJIT(object vertexData, int length)
			{
				this.AppendJIT((T[])vertexData, length);
			}
			public void AppendJIT(T[] data, int length)
			{
				if (this.vertexCount + length > this.vertices.Length)
				{
					int newArrSize = MathF.Max(16, this.vertexCount * 2, this.vertexCount + length);
					Array.Resize<T>(ref this.vertices, newArrSize);
				}
				Array.Copy(data, 0, this.vertices, this.vertexCount, length);
				this.vertexCount += length;
				
				if (this.material.Technique.Res.NeedsZSort)
					this.zSortIndex = CalcZSortIndex(this.vertices, this.vertexCount);
			}
			public bool CanAppend(IDrawBatch other)
			{
				return
					other.VertexMode == this.vertexMode && 
					other is DrawBatch<T> &&
					this.vertexCount + other.VertexCount < 1024 &&
					this.vertexMode.IsBatchableMode() &&
					other.Material == this.material;
			}
			public void Append(IDrawBatch other)
			{
				this.Append((DrawBatch<T>)other);
			}
			public void Append(DrawBatch<T> other)
			{
				if (this.vertexCount + other.vertexCount > this.vertices.Length)
				{
					int newArrSize = MathF.Max(16, this.vertexCount * 2, this.vertexCount + other.vertexCount);
					Array.Resize<T>(ref this.vertices, newArrSize);
				}
				Array.Copy(other.vertices, 0, this.vertices, this.vertexCount, other.vertexCount);
				this.vertexCount += other.vertexCount;
				
				if (this.material.Technique.Res.NeedsZSort)
					this.zSortIndex = CalcZSortIndex(this.vertices, this.vertexCount);
			}

			public static float CalcZSortIndex(T[] vertices, int count = -1)
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
		}
		
		
		/// <summary>
		/// The default reference distance for perspective rendering.
		/// </summary>
		public const float DefaultFocusDist	= 500.0f;

		
		private	bool				disposed		= false;
		private	float				nearZ			= 0.0f;
		private	float				farZ			= 10000.0f;
		private	float				focusDist		= DefaultFocusDist;
		private	Rect				viewportRect	= Rect.Empty;
		private	Vector3				refPos			= Vector3.Zero;
		private	float				refAngle		= 0.0f;
		private	RenderMatrix		renderMode		= RenderMatrix.OrthoScreen;
		private	PerspectiveMode		perspective		= PerspectiveMode.Parallax;
		private	Matrix4				matModelView	= Matrix4.Identity;
		private	Matrix4				matProjection	= Matrix4.Identity;
		private	Matrix4				matFinal		= Matrix4.Identity;
		private	VisibilityFlag		visibilityMask	= VisibilityFlag.All;
		private	int					pickingIndex	= 0;
		private	List<IDrawBatch>	drawBuffer		= new List<IDrawBatch>();
		private	List<IDrawBatch>	drawBufferZSort	= new List<IDrawBatch>();
		private	int					numRawBatches	= 0;
		private	ContentRef<RenderTarget> renderTarget = null;


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
		/// [GET / SET] Specified the perspective effect that is applied when rendering the world.
		/// </summary>
		public PerspectiveMode Perspective
		{
			get { return this.perspective; }
			set { this.perspective = value; }
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
		public RenderMatrix RenderMode
		{
			get { return this.renderMode; }
			set { this.renderMode = value; }
		}
		public Rect ViewportRect
		{
			get { return this.viewportRect; }
			set { this.viewportRect = value; }
		}
		public bool DepthWrite
		{
			get { return this.renderMode != RenderMatrix.OrthoScreen; }
		}
		public Point2 TargetSize
		{
			get { return new Point2((int)this.viewportRect.W, (int)this.viewportRect.H); }
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
			}
		}

		
		/// <summary>
		/// Returns the scale factor of objects that are located at the specified (world space) z-Coordinate.
		/// </summary>
		/// <param name="z"></param>
		/// <returns></returns>
		public float GetScaleAtZ(float z)
		{
			if (this.perspective == PerspectiveMode.Parallax)
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
			if (this.perspective == PerspectiveMode.Flat)
			{
				// Scale globally
				scaleTemp = DefaultFocusDist / this.focusDist;
				screenPos.X *= scaleTemp;
				screenPos.Y *= scaleTemp;
			}
			else if (this.perspective == PerspectiveMode.Parallax)
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
		/// Transforms screen space coordinates to world space coordinates.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <returns></returns>
		public Vector3 GetSpaceCoord(Vector2 screenPos)
		{
			return this.GetSpaceCoord(new Vector3(screenPos));
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
			if (this.perspective == PerspectiveMode.Flat)
			{
				// Scale globally
				scaleTemp = this.focusDist / DefaultFocusDist;
				spacePos.X *= scaleTemp;
				spacePos.Y *= scaleTemp;
			}
			else if (this.perspective == PerspectiveMode.Parallax)
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
		/// <summary>
		/// Transforms world space coordinates to screen space coordinates.
		/// </summary>
		/// <param name="spacePos"></param>
		/// <returns></returns>
		public Vector3 GetScreenCoord(Vector2 spacePos)
		{
			return this.GetScreenCoord(new Vector3(spacePos));
		}

		public void PreprocessCoords(ref Vector3 pos, ref float scale)
		{
			if (this.renderMode == RenderMatrix.OrthoScreen) return;
			
			// Make coordinates relative to the Camera
			pos.X -= this.refPos.X;
			pos.Y -= this.refPos.Y;
			pos.Z -= this.refPos.Z;

			// Apply active perspective effect
			float scaleTemp;
			if (this.perspective == PerspectiveMode.Flat)
			{
				// Scale globally
				scaleTemp = this.focusDist / DefaultFocusDist;
				pos.X *= scaleTemp;
				pos.Y *= scaleTemp;
				scale *= scaleTemp;
			}
			else if (this.perspective == PerspectiveMode.Parallax)
			{
				// Scale distance-based
				scaleTemp = this.focusDist / Math.Max(pos.Z, this.nearZ);
				pos.X *= scaleTemp;
				pos.Y *= scaleTemp;
				scale *= scaleTemp;
			}
		}
		public bool IsCoordInView(Vector3 c, float boundRad)
		{
			if (this.renderMode == RenderMatrix.OrthoScreen)
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

		public void AddVertices<T>(ContentRef<Material> material, VertexMode vertexMode, params T[] vertices) where T : struct, IVertexData
		{
			this.AddVertices<T>(material.IsAvailable ? material.Res.InfoDirect : Material.Checkerboard.Res.InfoDirect, vertexMode, vertices, vertices.Length);
		}
		public void AddVertices<T>(BatchInfo material, VertexMode vertexMode, params T[] vertices) where T : struct, IVertexData
		{
			this.AddVertices<T>(material, vertexMode, vertices, vertices.Length);
		}
		public void AddVertices<T>(ContentRef<Material> material, VertexMode vertexMode, T[] vertexBuffer, int vertexCount) where T : struct, IVertexData
		{
			this.AddVertices<T>(material.IsAvailable ? material.Res.InfoDirect : Material.Checkerboard.Res.InfoDirect, vertexMode, vertexBuffer, vertexCount);
		}
		public void AddVertices<T>(BatchInfo material, VertexMode vertexMode, T[] vertexBuffer, int vertexCount) where T : struct, IVertexData
		{
			if (vertexCount == 0) return;
			if (vertexBuffer == null || vertexBuffer.Length == 0) return;
			if (vertexCount > vertexBuffer.Length) vertexCount = vertexBuffer.Length;
			if (material == null) material = Material.Checkerboard.Res.InfoDirect;

			if (this.pickingIndex != 0)
			{
				ColorRgba clr = new ColorRgba((this.pickingIndex << 8) | 0xFF);
				for (int i = 0; i < vertexCount; ++i)
					vertexBuffer[i].Color = clr;

				material = new BatchInfo(material);
				material.Technique = DrawTechnique.Picking;
				if (material.Textures == null) material.MainTexture = Texture.White;
			}
			else if (material.Technique == null || !material.Technique.IsAvailable)
			{
				material = new BatchInfo(material);
				material.Technique = DrawTechnique.Solid;
			}
			
			// When rendering without depth writing, use z sorting everywhere - there's no real depth buffering!
			bool zSort = !this.DepthWrite || material.Technique.Res.NeedsZSort;
			List<IDrawBatch> buffer = zSort ? this.drawBufferZSort : this.drawBuffer;
			float zSortIndex = zSort ? DrawBatch<T>.CalcZSortIndex(vertexBuffer, vertexCount) : 0.0f;

			// Determine if we can append the incoming vertices into the previous batch
			IDrawBatch prevBatch = buffer.Count > 0 ? buffer[buffer.Count - 1] : null;
			if (prevBatch != null &&
				// ToDo: Move into CanAppendJIT on next major version step:
				// Make sure to not generate batches that will be in the Large Object Heap (>= 85k bytes)
				// because this will trigger lots and lots of Gen2 collections over time.
				vertexCount + prevBatch.VertexCount < 1024 &&
				// Check if the batches do match enough for being merged
				prevBatch.CanAppendJIT<T>(	
					zSort ? 1.0f : 0.0f, // Obsolete as of 2016-06-17, can be replcaed with zSort bool.
					zSortIndex, 
					material, 
					vertexMode))
			{
				prevBatch.AppendJIT(vertexBuffer, vertexCount);
			}
			else
			{
				buffer.Add(new DrawBatch<T>(material, vertexMode, vertexBuffer, vertexCount, zSortIndex));
			}
			++this.numRawBatches;
		}
		
		public void UpdateMatrices()
		{
			Vector2 refSize = this.TargetSize;
			this.GenerateModelView(out this.matModelView);
			this.GenerateProjection(new Rect(refSize), out this.matProjection);
			this.matFinal = this.matModelView * this.matProjection;
		}
		public void PrepareForDrawcalls()
		{
			// Recalculate matrices according to current mode
			this.UpdateMatrices();
		}
		public void Render(ClearFlag clearFlags, ColorRgba clearColor, float clearDepth)
		{
			if (DualityApp.GraphicsBackend == null) return;

			// Process drawcalls
			this.OptimizeBatches();
			RenderOptions options = new RenderOptions
			{
				ClearFlags = clearFlags,
				ClearColor = clearColor,
				ClearDepth = clearDepth,
				Viewport = this.viewportRect,
				RenderMode = this.renderMode,
				ModelViewMatrix = this.matModelView,
				ProjectionMatrix = this.matProjection,
				Target = this.renderTarget.IsAvailable ? this.renderTarget.Res.Native : null
			};
			RenderStats stats = new RenderStats();
			DualityApp.GraphicsBackend.BeginRendering(this, options, stats);

			{
				if (this.pickingIndex == 0) Profile.TimeProcessDrawcalls.BeginMeasure();

				// Z-Independent: Sorted as needed by batch optimizer
				DualityApp.GraphicsBackend.Render(this.drawBuffer);

				// Z-Sorted: Back to Front
				DualityApp.GraphicsBackend.Render(this.drawBufferZSort);

				if (this.pickingIndex == 0) Profile.TimeProcessDrawcalls.EndMeasure();
			}
			Profile.StatNumDrawcalls.Add(stats.DrawCalls);

			DualityApp.GraphicsBackend.EndRendering();
			this.drawBuffer.Clear();
			this.drawBufferZSort.Clear();
		}


		private void GenerateModelView(out Matrix4 mvMat)
		{
			mvMat = Matrix4.Identity;
			if (this.renderMode == RenderMatrix.OrthoScreen) return;

			// Translate objects contrary to the camera
			// Removed: Do this in software now for custom perspective / parallax support
			// modelViewMat *= Matrix4.CreateTranslation(-this.GameObj.Transform.Pos);

			// Rotate them according to the camera angle
			mvMat *= Matrix4.CreateRotationZ(-this.refAngle);
		}
		private void GenerateProjection(Rect orthoAbs, out Matrix4 projMat)
		{
			if (this.renderMode == RenderMatrix.OrthoScreen)
			{
				Matrix4.CreateOrthographicOffCenter(
					orthoAbs.X,
					orthoAbs.X + orthoAbs.W, 
					orthoAbs.Y + orthoAbs.H, 
					orthoAbs.Y, 
					this.nearZ, 
					this.farZ,
					out projMat);
				// Flip Z direction from "out of the screen" to "into the screen".
				projMat.M33 = -projMat.M33;
			}
			else
			{
				Matrix4.CreateOrthographicOffCenter(
					orthoAbs.X - orthoAbs.W * 0.5f, 
					orthoAbs.X + orthoAbs.W * 0.5f, 
					orthoAbs.Y + orthoAbs.H * 0.5f, 
					orthoAbs.Y - orthoAbs.H * 0.5f, 
					this.nearZ, 
					this.farZ,
					out projMat);
				// Flip Z direction from "out of the screen" to "into the screen".
				projMat.M33 = -projMat.M33;
			}
		}

		private int DrawBatchComparer(IDrawBatch first, IDrawBatch second)
		{
			return first.SortIndex - second.SortIndex;
		}
		private int DrawBatchComparerZSort(IDrawBatch first, IDrawBatch second)
		{
			if (second.ZSortIndex < first.ZSortIndex) return -1;
			if (second.ZSortIndex > first.ZSortIndex) return 1;
			if (second.ZSortIndex == first.ZSortIndex) return 0;
			if (float.IsNaN(second.ZSortIndex))
				return (float.IsNaN(first.ZSortIndex) ? 0 : -1);
			else
				return 1;
		}
		private void OptimizeBatches()
		{
			int batchCountBefore = this.drawBuffer.Count + this.drawBufferZSort.Count;
			if (this.pickingIndex == 0) Profile.TimeOptimizeDrawcalls.BeginMeasure();

			// Non-ZSorted
			if (this.drawBuffer.Count > 1)
			{
				this.drawBuffer.StableSort(this.DrawBatchComparer);
				this.drawBuffer = this.OptimizeBatches(this.drawBuffer);
			}

			// Z-Sorted
			if (this.drawBufferZSort.Count > 1)
			{
				// Stable sort assures maintaining draw order for batches of equal ZOrderIndex
				this.drawBufferZSort.StableSort(this.DrawBatchComparerZSort);
				this.drawBufferZSort = this.OptimizeBatches(this.drawBufferZSort);
			}

			if (this.pickingIndex == 0) Profile.TimeOptimizeDrawcalls.EndMeasure();
			int batchCountAfter = this.drawBuffer.Count + this.drawBufferZSort.Count;

			Profile.StatNumRawBatches.Add(this.numRawBatches);
			Profile.StatNumMergedBatches.Add(batchCountBefore);
			Profile.StatNumOptimizedBatches.Add(batchCountAfter);
			this.numRawBatches = 0;
		}
		private List<IDrawBatch> OptimizeBatches(List<IDrawBatch> sortedBuffer)
		{
			List<IDrawBatch> optimized = new List<IDrawBatch>(sortedBuffer.Count);
			IDrawBatch current = sortedBuffer[0];
			IDrawBatch next;
			optimized.Add(current);
			for (int i = 1; i < sortedBuffer.Count; i++)
			{
				next = sortedBuffer[i];

				if (current.CanAppend(next))
				{
					current.Append(next);
				}
				else
				{
					current = next;
					optimized.Add(current);
				}
			}

			return optimized;
		}

		public static void RenderVoid(Rect viewportRect)
		{
			if (DualityApp.GraphicsBackend == null) return;

			RenderOptions options = new RenderOptions
			{
				ClearFlags = ClearFlag.All,
				ClearColor = ColorRgba.TransparentBlack,
				ClearDepth = 1.0f,
				Viewport = viewportRect,
				RenderMode = RenderMatrix.OrthoScreen
			};
			DualityApp.GraphicsBackend.BeginRendering(null, options);
			DualityApp.GraphicsBackend.EndRendering();
		}
	}
}
