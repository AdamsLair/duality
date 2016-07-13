using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Cloning;
using Duality.Resources;
using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// <see cref="Tileset"/> Resources define the tiles that are used by <see cref="Tilemap"/>, <see cref="TilemapRenderer"/> and <see cref="TilemapCollider"/> Components
	/// to update and display tilemaps, as well as define interactions between them and the world.
	/// </summary>
	[ExplicitResourceReference(typeof(Pixmap))]
	[EditorHintCategory(TilemapsResNames.CategoryTilemaps)]
	[EditorHintImage(TilemapsResNames.ImageTileset)]
	public class Tileset : Resource
	{
		public  static readonly Point2             DefaultTileSize     = new Point2(32, 32);
		private static readonly TilesetRenderInput DefaultRenderInput  = new TilesetRenderInput();
		private static readonly BatchInfo          DefaultBaseMaterial = new BatchInfo(DrawTechnique.Mask, ColorRgba.White);

		[CloneBehavior(typeof(TilesetRenderInput), CloneBehavior.ChildObject)]
		private List<TilesetRenderInput> renderConfig = new List<TilesetRenderInput>();
		[CloneBehavior(CloneBehavior.ChildObject)]
		private BatchInfo                baseMaterial = new BatchInfo(DefaultBaseMaterial);
		private Vector2                  tileSize     = DefaultTileSize;
		private RawList<TileInput>       tileInput    = new RawList<TileInput>();

		[DontSerialize] private RawList<TileInfo> tileData       = new RawList<TileInfo>();
		[DontSerialize] private List<Texture>     renderData     = new List<Texture>();
		[DontSerialize] private Material          renderMaterial = null;
		[DontSerialize] private bool              compiled       = false;
		[DontSerialize] private int               compileHash    = 0;
		[DontSerialize] private int               tileCount      = 0;

		
		/// <summary>
		/// [GET] A configuration template that is used for generating the output <see cref="RenderMaterial"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		public BatchInfo BaseMaterial
		{
			get { return this.baseMaterial; }
			private set
			{
				// This private setter is here for editor support
				this.baseMaterial = value ?? new BatchInfo(DefaultBaseMaterial);
				if (this.compiled)
				{
					this.DiscardRenderMaterial();
					this.GenerateRenderMaterial();
				}
			}
		}
		/// <summary>
		/// [GET] The different layers of <see cref="TilesetRenderInput"/>, which compose the look of all the tiles
		/// that are defined in this <see cref="Tileset"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IList<TilesetRenderInput> RenderConfig
		{
			get { return this.renderConfig; }
		}
		/// <summary>
		/// [GET / SET] The desired size of a tile in world space. How exactly this value is accounted for depends
		/// on the Components that evaluate it for rendering, collision detection, etc.
		/// </summary>
		[EditorHintDecimalPlaces(1)]
		[EditorHintIncrement(1)]
		public Vector2 TileSize
		{
			get { return this.tileSize; }
			set { this.tileSize = value; }
		}
		/// <summary>
		/// [GET / SET] Provides information about each tile in the <see cref="Tileset"/>.
		/// This information is transformed in the compilation process of a <see cref="Tileset"/>
		/// in order to form the <see cref="TileData"/> that will be used for determining
		/// tile behaviour.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public RawList<TileInput> TileInput
		{
			get { return this.tileInput; }
			set { this.tileInput = value ?? new RawList<TileInput>(); }
		}
		/// <summary>
		/// [GET] The number of tiles in this <see cref="Tileset"/>. Calculated during compilation.
		/// </summary>
		public int TileCount
		{
			get { return this.tileCount; }
		}
		/// <summary>
		/// [GET] The <see cref="Material"/> that has been compiled from the specified <see cref="RenderConfig"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Material RenderMaterial
		{
			get { return this.renderMaterial; }
		}
		/// <summary>
		/// [GET] Provides information about each compiled tile in the <see cref="Tileset"/>.
		/// This information is generated during the compilation process of a <see cref="Tileset"/>.
		/// Do not modify unless you know exactly what you're doing.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public RawList<TileInfo> TileData
		{
			get { return this.tileData; }
		}
		/// <summary>
		/// [GET] Whether this <see cref="Tileset"/> has been compiled yet or not.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool Compiled
		{
			get { return this.compiled; }
		}
		/// <summary>
		/// [GET] Determines whether the <see cref="Tileset"/> has changed since the last
		/// time it was compiled. Always true, if the <see cref="Tileset"/> has never been
		/// compiled before.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool HasChangedSinceCompile
		{
			get 
			{ 
				return 
					!this.compiled || 
					this.GetCompileHashCode() != this.compileHash;
			}
		}
		

		/// <summary>
		/// Looks up the vertex UV rect for the specified rendering input / data and tile.
		/// </summary>
		/// <param name="renderDataIndex"></param>
		/// <param name="tileIndex"></param>
		/// <param name="uv"></param>
		public void LookupTileAtlas(int renderDataIndex, int tileIndex, out Rect uv)
		{
			Texture texture = this.renderData[renderDataIndex];
			texture.LookupAtlas(tileIndex, out uv);
		}
		/// <summary>
		/// Looks up the source pixel coordinates for the specified input layer and tile.
		/// </summary>
		/// <param name="renderConfigIndex"></param>
		/// <param name="tileIndex"></param>
		/// <param name="pos"></param>
		/// <param name="size"></param>
		public void LookupTileSourceRect(int renderConfigIndex, int tileIndex, out Point2 pos, out Point2 size)
		{
			TilesetRenderInput input = this.renderConfig[renderConfigIndex];
			int rowWidth = input.SourceData.Res.Width;

			size = new Point2(
				input.SourceTileSize.X + input.SourceTileSpacing * 2,
				input.SourceTileSize.Y + input.SourceTileSpacing * 2);

			int totalXOffset = tileIndex * size.X;
			pos = new Point2(
				totalXOffset % rowWidth,
				(totalXOffset / rowWidth) * size.Y);
		}

		/// <summary>
		/// Compiles the <see cref="Tileset"/> using the specified source data, in order to
		/// generate optimized target data for rendering and collision detection.
		/// </summary>
		public void Compile()
		{
			// Clear previous data
			this.DiscardCompiledData();

			// Generate output pixel data
			int minSourceTileCount = int.MaxValue;
			for (int renderInputIndex = 0; renderInputIndex < this.renderConfig.Count; renderInputIndex++)
			{
				TilesetRenderInput input = this.renderConfig[renderInputIndex] ?? DefaultRenderInput;
				PixelData sourceData = (input.SourceData.Res ?? Pixmap.Checkerboard.Res).MainLayer;

				// What's the space requirement for each tile?
				Point2 sourceTileBounds = new Point2(
					input.SourceTileSize.X + input.SourceTileSpacing * 2, 
					input.SourceTileSize.Y + input.SourceTileSpacing * 2);
				Point2 targetTileBounds = new Point2(
					input.SourceTileSize.X + input.TargetTileSpacing * 2, 
					input.SourceTileSize.Y + input.TargetTileSpacing * 2);

				// How many tiles will we have?
				int sourceHorizontalCount = sourceData.Width / sourceTileBounds.X;
				int sourceVerticalCount = sourceData.Height / sourceTileBounds.Y;
				int sourceTileCount = sourceHorizontalCount * sourceVerticalCount;
				int targetTileCount = sourceTileCount; // ToDo: Account for expanded AutoTiles

				minSourceTileCount = Math.Min(minSourceTileCount, sourceTileCount);

				// What's the optimal texture size to include them all?
				int targetTextureWidth;
				int targetTextureHeight;
				{
					int minTilesPerLine = MathF.Max(1, (int)MathF.Sqrt(targetTileCount));
					targetTextureWidth = MathF.NextPowerOfTwo(targetTileBounds.X * minTilesPerLine);

					int actualTilesPerLine = targetTextureWidth / targetTileBounds.X;
					int requiredLineCount = 1 + (targetTileCount / actualTilesPerLine);
					targetTextureHeight = MathF.NextPowerOfTwo(targetTileBounds.Y * requiredLineCount);
				}

				// Create a buffer for writing target pixel data
				PixelData targetData = new PixelData(targetTextureWidth, targetTextureHeight);

				// Iterate over tiles and move each tile from source to target
				List<Rect> tileAtlas = new List<Rect>();
				Point2 targetTilePos = new Point2(0, 0);
				for (int tileIndex = 0; tileIndex < sourceTileCount; tileIndex++)
				{
					// Initialize a new tile info when necessary
					if (tileIndex >= this.tileData.Count)
					{
						this.tileData.Count++;
						this.tileData.Data[tileIndex].IsVisuallyEmpty = true;
					}

					// Determine where on the source buffer the tile is located
					Point2 sourceTilePos = new Point2(
						sourceTileBounds.X * (tileIndex % sourceHorizontalCount),
						sourceTileBounds.Y * (tileIndex / sourceHorizontalCount));

					// Draw the source tile onto the target buffer, including its spacing / border
					Point2 targetContentPos = new Point2(
						targetTilePos.X + input.TargetTileSpacing, 
						targetTilePos.Y + input.TargetTileSpacing);
					sourceData.DrawOnto(targetData, 
						BlendMode.Solid, 
						targetContentPos.X, 
						targetContentPos.Y, 
						input.SourceTileSize.X, 
						input.SourceTileSize.Y, 
						sourceTilePos.X + input.SourceTileSpacing, 
						sourceTilePos.Y + input.SourceTileSpacing);

					// Fill up the target spacing area with similar pixels
					if (input.TargetTileSpacing > 0)
					{
						FillTileSpacing(targetData, input.TargetTileSpacing, targetContentPos, input.SourceTileSize);
					}

					// Update whether the tile is considered visually empty
					if (this.tileData.Data[tileIndex].IsVisuallyEmpty)
					{
						bool isLayerVisuallyEmpty = IsCompletelyTransparent(
							sourceData, 
							new Point2(
								sourceTilePos.X + input.SourceTileSpacing,
								sourceTilePos.Y + input.SourceTileSpacing),
							input.SourceTileSize);
						if (!isLayerVisuallyEmpty)
							this.tileData.Data[tileIndex].IsVisuallyEmpty = false;
					}

					// Add an entry to the generated atlas
					Rect atlasRect = new Rect(
						targetTilePos.X + input.TargetTileSpacing, 
						targetTilePos.Y + input.TargetTileSpacing, 
						targetTileBounds.X - input.TargetTileSpacing * 2, 
						targetTileBounds.Y - input.TargetTileSpacing * 2);
					tileAtlas.Add(atlasRect);

					// Advance the target tile position
					targetTilePos.X += targetTileBounds.X;
					if (targetTilePos.X + targetTileBounds.X > targetData.Width)
					{
						targetTilePos.X = 0;
						targetTilePos.Y += targetTileBounds.Y;
					}
				}

				// Create the texture to be used for this rendering input
				using (Pixmap targetPixmap = new Pixmap(targetData))
				{
					targetPixmap.Atlas = tileAtlas;
					Texture targetTexture = new Texture(
						targetPixmap, TextureSizeMode.Enlarge, 
						input.TargetMagFilter, input.TargetMinFilter, 
						TextureWrapMode.Clamp, TextureWrapMode.Clamp, 
						input.TargetFormat);

					this.renderData.Add(targetTexture);
				}
			}

			// Generate additional per-tile data
			{
				// Copy input data
				for (int i = 0; i < this.tileData.Count; i++)
				{
					if (i >= this.tileInput.Count) break;
					this.tileData.Data[i].DepthOffset = this.tileInput.Data[i].DepthOffset;
					this.tileData.Data[i].IsVertical = this.tileInput.Data[i].IsVertical;
					this.tileData.Data[i].Collision = this.tileInput.Data[i].Collision;
				}

				// Retrieve texture atlas data for quick lookup during rendering
				if (this.renderData.Count > 0)
				{
					for (int i = 0; i < this.tileData.Count; i++)
					{
						this.renderData[0].LookupAtlas(i, out this.tileData.Data[i].TexCoord0);
					}
				}
			}

			// Generate an output material from the generated textures
			this.GenerateRenderMaterial();

			// Apply global tileset stats
			this.tileCount = (this.renderConfig.Count > 0) ? minSourceTileCount : 0;

			this.compiled = true;
			this.compileHash = this.GetCompileHashCode();
		}
		/// <summary>
		/// Generates the <see cref="RenderMaterial"/> from the currently available
		/// <see cref="BaseMaterial"/> and <see cref="renderData"/>.
		/// </summary>
		private void GenerateRenderMaterial()
		{
			this.renderMaterial = new Material(this.baseMaterial);
			for (int i = 0; i < this.renderConfig.Count; i++)
			{
				this.renderMaterial.SetTexture(this.renderConfig[i].Id, this.renderData[i] ?? Texture.Checkerboard);
			}
		}
		/// <summary>
		/// Discards all <see cref="Tileset"/> data that was acquired during <see cref="Compile"/>.
		/// </summary>
		private void DiscardCompiledData()
		{
			this.compiled = false;
			this.DiscardRenderMaterial();
			foreach (Texture tex in this.renderData)
			{
				tex.Dispose();
			}
			this.renderData.Clear();
			this.tileData.Clear();
		}
		/// <summary>
		/// Discards a previously generated <see cref="RenderMaterial"/>. Does not discard any
		/// other generated <see cref="Tileset"/> data.
		/// </summary>
		private void DiscardRenderMaterial()
		{
			if (this.renderMaterial != null)
			{
				this.renderMaterial.Dispose();
				this.renderMaterial = null;
			}
		}
		/// <summary>
		/// Determines the <see cref="Compile"/>-relevant hash code of the specified <see cref="Tileset"/>.
		/// This value is used internally to determine whether a <see cref="Tileset"/> needs to be recompiled.
		/// </summary>
		/// <param name="tileset"></param>
		/// <returns></returns>
		public virtual int GetCompileHashCode()
		{
			int hash = 17;

			if (this.baseMaterial != null)
				MathF.CombineHashCode(ref hash, this.baseMaterial.GetHashCode());
			MathF.CombineHashCode(ref hash, this.tileSize.GetHashCode());

			foreach (TilesetRenderInput input in this.renderConfig)
			{
				MathF.CombineHashCode(ref hash, input.Id.GetHashCode());
				MathF.CombineHashCode(ref hash, input.Name.GetHashCode());
				MathF.CombineHashCode(ref hash, input.SourceData.GetHashCode());
				MathF.CombineHashCode(ref hash, input.SourceTileSize.GetHashCode());
				MathF.CombineHashCode(ref hash, input.SourceTileSpacing.GetHashCode());
				MathF.CombineHashCode(ref hash, input.TargetFormat.GetHashCode());
				MathF.CombineHashCode(ref hash, input.TargetMagFilter.GetHashCode());
				MathF.CombineHashCode(ref hash, input.TargetMinFilter.GetHashCode());
				MathF.CombineHashCode(ref hash, input.TargetTileSpacing.GetHashCode());
			}

			TileInput[] data = this.tileInput.Data;
			int count = this.tileInput.Count;
			int defaultTileHash = default(TileInput).GetHashCode();
			for (int i = 0; i < count; i++)
			{
				int tileHash = data[i].GetHashCode();

				// Exclude hashes from default inputs so they're considered equal 
				// to not being defined in the input data array.
				if (tileHash == defaultTileHash) continue;

				// Due to the above rule, we'll have to include each tiles index in the
				// has, so leading defaults will still affect following non-defaults
				MathF.CombineHashCode(ref hash, i);
				MathF.CombineHashCode(ref hash, tileHash);
			}

			return hash;
		}

		protected override void OnLoaded()
		{
			this.Compile();
			base.OnLoaded();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.DiscardCompiledData();
		}

		/// <summary>
		/// Fills up the specified spacing around a tile's pixel data with colors that are
		/// similar to the existing edge colors in order to prevent filtering artifacts
		/// when rendering them as a texture atlas.
		/// </summary>
		/// <param name="targetData"></param>
		/// <param name="targetTileSpacing"></param>
		/// <param name="targetContentPos"></param>
		/// <param name="targetTileSize"></param>
		private static void FillTileSpacing(PixelData targetData, int targetTileSpacing, Point2 targetContentPos, Point2 targetTileSize)
		{
			ColorRgba[] rawData = targetData.Data;
			int width = targetData.Width;
			int baseIndex;
			int offsetIndex;

			// Top
			for (int offset = 1; offset <= targetTileSpacing; offset++)
			{
				baseIndex = targetContentPos.Y * width + targetContentPos.X;
				offsetIndex = (targetContentPos.Y - offset) * width + targetContentPos.X;
				for (int i = 0; i < targetTileSize.X; i++)
				{
					rawData[offsetIndex + i] = rawData[baseIndex + i];
				}
			}

			// Bottom
			for (int offset = 1; offset <= targetTileSpacing; offset++)
			{
				baseIndex = (targetContentPos.Y + targetTileSize.Y - 1) * width + targetContentPos.X;
				offsetIndex = (targetContentPos.Y + targetTileSize.Y - 1 + offset) * width + targetContentPos.X;
				for (int i = 0; i < targetTileSize.X; i++)
				{
					rawData[offsetIndex + i] = rawData[baseIndex + i];
				}
			}

			// Left
			for (int offset = 1; offset <= targetTileSpacing; offset++)
			{
				baseIndex = targetContentPos.Y * width + targetContentPos.X;
				offsetIndex = targetContentPos.Y * width + targetContentPos.X - offset;
				for (int i = 0; i < targetTileSize.X; i++)
				{
					rawData[offsetIndex + i * width] = rawData[baseIndex + i * width];
				}
			}

			// Right
			for (int offset = 1; offset <= targetTileSpacing; offset++)
			{
				baseIndex = targetContentPos.Y * width + targetContentPos.X + targetTileSize.X - 1;
				offsetIndex = targetContentPos.Y * width + targetContentPos.X + targetTileSize.X - 1 + offset;
				for (int i = 0; i < targetTileSize.X; i++)
				{
					rawData[offsetIndex + i * width] = rawData[baseIndex + i * width];
				}
			}

			// Top Left Corner
			baseIndex = targetContentPos.Y * width + targetContentPos.X;
			for (int offsetY = 1; offsetY <= targetTileSpacing; offsetY++)
			{
				for (int offsetX = 1; offsetX <= targetTileSpacing; offsetX++)
				{
					offsetIndex = baseIndex - offsetX - offsetY * width;
					rawData[offsetIndex] = rawData[baseIndex];
				}
			}

			// Top Right Corner
			baseIndex = targetContentPos.Y * width + targetContentPos.X + targetTileSize.X - 1;
			for (int offsetY = 1; offsetY <= targetTileSpacing; offsetY++)
			{
				for (int offsetX = 1; offsetX <= targetTileSpacing; offsetX++)
				{
					offsetIndex = baseIndex + offsetX - offsetY * width;
					rawData[offsetIndex] = rawData[baseIndex];
				}
			}

			// Bottom Left Corner
			baseIndex = (targetContentPos.Y + targetTileSize.Y - 1) * width + targetContentPos.X;
			for (int offsetY = 1; offsetY <= targetTileSpacing; offsetY++)
			{
				for (int offsetX = 1; offsetX <= targetTileSpacing; offsetX++)
				{
					offsetIndex = baseIndex - offsetX + offsetY * width;
					rawData[offsetIndex] = rawData[baseIndex];
				}
			}

			// Bottom Right Corner
			baseIndex = (targetContentPos.Y + targetTileSize.Y - 1) * width + targetContentPos.X + targetTileSize.X - 1;
			for (int offsetY = 1; offsetY <= targetTileSpacing; offsetY++)
			{
				for (int offsetX = 1; offsetX <= targetTileSpacing; offsetX++)
				{
					offsetIndex = baseIndex + offsetX + offsetY * width;
					rawData[offsetIndex] = rawData[baseIndex];
				}
			}
		}
		/// <summary>
		/// Determines whether the specified pixel data block is completely transparent in
		/// the specified area.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="pos"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		private static bool IsCompletelyTransparent(PixelData data, Point2 pos, Point2 size)
		{
			for (int y = pos.Y; y < pos.Y + size.Y; y++)
			{
				for (int x = pos.X; x < pos.X + size.X; x++)
				{
					if (data[x, y].A > 0)
						return false;
				}
			}
			return true;
		}
	}
}
