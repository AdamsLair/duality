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
		private static readonly BatchInfo          DefaultBaseMaterial = new BatchInfo(DrawTechnique.Mask, ColorRgba.White);

		private List<TilesetRenderInput>   renderConfig   = new List<TilesetRenderInput>();
		private List<TilesetAutoTileInput> autoTileConfig = new List<TilesetAutoTileInput>();
		private BatchInfo                  baseMaterial   = new BatchInfo(DefaultBaseMaterial);
		private Vector2                    tileSize       = DefaultTileSize;
		private RawList<TileInput>         tileInput      = new RawList<TileInput>();

		[DontSerialize] private RawList<TileInfo>         tileData       = new RawList<TileInfo>();
		[DontSerialize] private List<TilesetAutoTileInfo> autoTileData   = new List<TilesetAutoTileInfo>();
		[DontSerialize] private List<Texture>             renderData     = new List<Texture>();
		[DontSerialize] private Material                  renderMaterial = null;
		[DontSerialize] private bool                      compiled       = false;
		[DontSerialize] private int                       compileHash    = 0;
		[DontSerialize] private int                       tileCount      = 0;

		
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
		/// [GET] The different auto-tile definitions of this <see cref="Tileset"/>, each specified by
		/// a single <see cref="TilesetAutoTileInput"/>. This data is user-defined and pre-compilation.
		/// To access post-compile data, see the <see cref="AutoTileData"/> property.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IList<TilesetAutoTileInput> AutoTileConfig
		{
			get { return this.autoTileConfig; }
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
		/// [GET] Provides information about each compiled auto-tile in the <see cref="Tileset"/>.
		/// This information is generated during the compilation process of a <see cref="Tileset"/>.
		/// To change any of the settings, instead check out the <see cref="AutoTileConfig"/> property.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IReadOnlyList<TilesetAutoTileInfo> AutoTileData
		{
			get { return this.autoTileData; }
		}
		/// <summary>
		/// [GET] Provides information about each compiled tile in the <see cref="Tileset"/>.
		/// This information is generated during the compilation process of a <see cref="Tileset"/>.
		/// 
		/// Do not modify. The only reason this is not read-only is to provide raw / optimized 
		/// array access in bottleneck paths such as <see cref="Tilemap"/> rendering.
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

			// Compile new tileset data using input configs, while 
			// potentially re-using existing data structures.
			TilesetCompiler compiler = new TilesetCompiler();
			TilesetCompilerOutput data = compiler.Compile(new TilesetCompilerInput
			{
				TileInput = this.tileInput,
				RenderConfig = this.renderConfig,
				AutoTileConfig = this.autoTileConfig,
				ExistingOutput = new TilesetCompilerOutput
				{
					RenderData = this.renderData,
					TileData = this.tileData,
					AutoTileData = this.autoTileData
				}
			});
			
			// Apply compiled data to the internal tileset data
			this.renderData = data.RenderData;
			this.tileData = data.TileData;
			this.autoTileData = data.AutoTileData;
			this.tileCount = data.TileCount;
			this.GenerateRenderMaterial();

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
			this.autoTileData.Clear();
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
		public int GetCompileHashCode()
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

			foreach (TilesetAutoTileInput autoTile in this.autoTileConfig)
			{
				MathF.CombineHashCode(ref hash, autoTile.BaseTileIndex);
				MathF.CombineHashCode(ref hash, autoTile.GenerateMissingTiles ? 1 : 0);
				MathF.CombineHashCode(ref hash, autoTile.ConnectivityMap.Count);
				for (int i = 0; i < autoTile.ConnectivityMap.Count; i++)
				{
					MathF.CombineHashCode(ref hash, (int)autoTile.ConnectivityMap[i].Neighbours);
					MathF.CombineHashCode(ref hash, autoTile.ConnectivityMap[i].TileIndex);
				}
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
	}
}
