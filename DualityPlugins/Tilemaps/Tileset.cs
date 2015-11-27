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
		public  static readonly Vector2            DefaultTileSize    = new Vector2(32, 32);
		private static readonly TilesetRenderInput DefaultRenderInput = new TilesetRenderInput();

		private Vector2                  tileSize     = DefaultTileSize;
		private List<TilesetRenderInput> renderConfig = new List<TilesetRenderInput>();
		private BatchInfo                baseMaterial = new BatchInfo(DrawTechnique.Mask, ColorRgba.White);

		[DontSerialize] private List<Texture> renderData     = new List<Texture>();
		[DontSerialize] private Material      renderMaterial = null;
		[DontSerialize] private bool          compiled       = false;

		
		/// <summary>
		/// [GET] A configuration template that is used for generating the output <see cref="RenderMaterial"/>.
		/// </summary>
		public BatchInfo BaseMaterial
		{
			get { return this.baseMaterial; }
		}
		/// <summary>
		/// The different layers of <see cref="TilesetRenderInput"/>, which compose the look of all the tiles
		/// that are defined in this <see cref="Tileset"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IList<TilesetRenderInput> RenderConfig
		{
			get { return this.renderConfig; }
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
		/// [GET / SET] The desired size of a tile in world space. How exactly this value is accounted for depends
		/// on the Components that evaluate it for rendering, collision detection, etc.
		/// </summary>
		public Vector2 TileSize
		{
			get { return this.tileSize; }
			set { this.tileSize = value; }
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
		/// Compiles the <see cref="Tileset"/> using the specified source data, in order to
		/// generate optimized target data for rendering and collision detection.
		/// </summary>
		public void Compile()
		{
			// Clear previous data
			this.DiscardCompiledData();

			// ToDo: Support for a distinct target tile spacing (to prevent filter artifacts)
			// ToDo: Prepare information on AutoTile expansion
			// ToDo: Mapping between conceptual tiles and actual tiles (with AutoTiles in mind)
			// ToDo: Collision info per tile
			// ToDo: Height info per tile
			// ToDo: Flat / Upright info per tile
			// ToDo: Additional data / tags per tile

			// Generate output pixel data
			for (int renderInputIndex = 0; renderInputIndex < this.renderConfig.Count; renderInputIndex++)
			{
				TilesetRenderInput input = this.renderConfig[renderInputIndex] ?? DefaultRenderInput;
				PixelData sourceData = (input.SourceData.Res ?? Pixmap.Checkerboard.Res).MainLayer;

				// What's the space requirement for each tile?
				Point2 sourceTileBounds = new Point2(
					input.SourceTileSize.X + input.SourceTileSpacing * 2, 
					input.SourceTileSize.Y + input.SourceTileSpacing * 2);
				Point2 targetTileBounds = sourceTileBounds; // ToDo: Account for different (smaller or larger) target spacing

				// How many tiles will we have?
				int sourceHorizontalCount = sourceData.Width / sourceTileBounds.X;
				int sourceVerticalCount = sourceData.Height / sourceTileBounds.Y;
				int sourceTileCount = sourceHorizontalCount * sourceVerticalCount;
				int targetTileCount = sourceTileCount; // ToDo: Account for expanded AutoTiles

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
					// Determine where on the source buffer the tile is located
					Point2 sourceTilePos = new Point2(
						sourceTileBounds.X * (tileIndex % sourceHorizontalCount),
						sourceTileBounds.Y * (tileIndex / sourceHorizontalCount));

					// ToDo: Expand AutoTiles

					// Draw the source tile onto the target buffer, including its spacing / border
					sourceData.DrawOnto(targetData, 
						BlendMode.Solid, 
						targetTilePos.X, targetTilePos.Y, 
						sourceTileBounds.X, sourceTileBounds.Y, 
						sourceTilePos.X, sourceTilePos.Y);

					// ToDo: If target spacing is bigger than source spacing, fill the empty edges and corners with matching colors

					// Add an entry to the generated atlas
					tileAtlas.Add(new Rect(
						targetTilePos.X + input.SourceTileSpacing, 
						targetTilePos.Y + input.SourceTileSpacing, 
						targetTileBounds.X - input.SourceTileSpacing * 2, 
						targetTileBounds.Y - input.SourceTileSpacing * 2));

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

			// Generate an output material from the generated textures
			this.renderMaterial = new Material(this.baseMaterial);
			for (int i = 0; i < this.renderConfig.Count; i++)
			{
				this.renderMaterial.SetTexture(this.renderConfig[i].Id, this.renderData[i] ?? Texture.Checkerboard);
			}

			this.compiled = true;
		}
		private void DiscardCompiledData()
		{
			this.compiled = false;
			if (this.renderMaterial != null)
			{
				this.renderMaterial.Dispose();
				this.renderMaterial = null;
			}
			foreach (Texture tex in this.renderData)
			{
				tex.Dispose();
			}
			this.renderData.Clear();
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
		protected override void OnCopyDataTo(object target, ICloneOperation operation)
		{
			base.OnCopyDataTo(target, operation);
			Tileset targetTileset = target as Tileset;
			if (this.compiled) targetTileset.Compile();
		}
	}
}
