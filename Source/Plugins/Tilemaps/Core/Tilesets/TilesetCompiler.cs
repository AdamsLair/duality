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
	/// The <see cref="TilesetCompiler"/> encapsulates the process of transforming raw source data
	/// of a <see cref="Tileset"/> into an optimized and enriched target dataset, which will then
	/// be provided by the <see cref="Tileset"/> for rendering and collision detection.
	/// </summary>
	public class TilesetCompiler
	{
		private struct LayerGeometry
		{
			public int SourceTileCount;
			public Point2 SourceTileAdvance;
			public int SourceTilesPerRow;
			public int SourceTilesPerColumn;

			public int TargetTileCount;
			public Point2 TargetTileAdvance;
			public Point2 TargetTextureSize;

			/// <summary>
			/// Determines where on the source image / pixel data the specified tile is located.
			/// </summary>
			/// <param name="tileIndex"></param>
			/// <returns></returns>
			public Point2 GetSourceTilePos(int tileIndex)
			{
				return new Point2(
					this.SourceTileAdvance.X * (tileIndex % this.SourceTilesPerRow),
					this.SourceTileAdvance.Y * (tileIndex / this.SourceTilesPerRow));
			}
		}
		private struct LayerPixelData
		{
			public PixelData PixelData;
			public List<Rect> Atlas;
		}
		private struct AutoTileData
		{
			public int BaseTile;
			public int[] StateToTile;
			public bool[] IsStateAvailable;
			public RawList<TilesetAutoTileItem> TileInfo;
		}
		private struct GeneratedQuadTile
		{
			public int SourceBaseIndex;
			public int SourceTopLeftIndex;
			public int SourceTopRightIndex;
			public int SourceBottomRightIndex;
			public int SourceBottomLeftIndex;

			public int TargetIndex;
		}

		private static readonly TilesetAutoTileFallbackMap AutoTileFallbackMap = new TilesetAutoTileFallbackMap();
		private static readonly TilesetRenderInput DefaultRenderInput = new TilesetRenderInput();


		private RawList<GeneratedQuadTile> generateTileSchedule = new RawList<GeneratedQuadTile>();
		private RawList<AutoTileData> autoTiles = new RawList<AutoTileData>();
		private RawList<TileInfo> tiles = new RawList<TileInfo>();
		private List<Texture> renderData = new List<Texture>();
		private int inputTileCount = 0;
		private int outputTileCount = 0;

		private RawListPool<TilesetAutoTileItem> autoTileItemPool = new RawListPool<TilesetAutoTileItem>();
		private bool[] autoTileStateBuffer = new bool[(int)TileConnection.All + 1];


		/// <summary>
		/// Compiles a <see cref="Tileset"/> using its specified source data, in order to
		/// generate optimized target data for rendering and collision detection.
		/// </summary>
		public TilesetCompilerOutput Compile(TilesetCompilerInput input)
		{
			// Clear private working data before beginning a new operation
			this.ClearWorkingData();

			// Initialize private working data with the required space for our new input
			this.InitWorkingData(input);

			// Gather data on AutoTiles that are partially transformed from the input tileset,
			// and partially generated from existing tiles as part of the compilation.
			this.GatherAutoTileData(input.AutoTileConfig);

			// Generate metadata for scheduled tiles
			this.GenerateTileMetadata();

			// Draw output pixel data
			for (int renderInputIndex = 0; renderInputIndex < input.RenderConfig.Count; renderInputIndex++)
			{
				TilesetRenderInput renderInput = input.RenderConfig[renderInputIndex] ?? DefaultRenderInput;
				PixelData sourceLayerData = (renderInput.SourceData.Res ?? Pixmap.Checkerboard.Res).MainLayer;

				// Determine overall geometry values for this layer, such as tile bounds and texture sizes
				LayerGeometry layerGeometry = this.CalculateLayerGeometry(renderInput, sourceLayerData);

				// Generate pixel data and atlas values for this layer's texture
				LayerPixelData targetLayerData = this.ComposeLayerPixelData(
					renderInput, 
					sourceLayerData, 
					layerGeometry);

				// Create the texture to be used for this rendering input
				using (Pixmap targetPixmap = new Pixmap(targetLayerData.PixelData))
				{
					targetPixmap.Atlas = targetLayerData.Atlas;
					Texture targetTexture = new Texture(
						targetPixmap, TextureSizeMode.Enlarge, 
						renderInput.TargetMagFilter, renderInput.TargetMinFilter, 
						TextureWrapMode.Clamp, TextureWrapMode.Clamp, 
						renderInput.TargetFormat);

					this.renderData.Add(targetTexture);
				}
			}

			// Retrieve texture atlas data for quick lookup during rendering
			if (this.renderData.Count > 0)
			{
				for (int i = 0; i < this.tiles.Count; i++)
				{
					this.renderData[0].LookupAtlas(i, out this.tiles.Data[i].TexCoord0);
				}
			}

			// Prepare output, keeping existing structures to avoid allocations
			TilesetCompilerOutput output = input.ExistingOutput;
			output.TileCount = this.outputTileCount;

			output.TileData = output.TileData ?? new RawList<TileInfo>(input.TileInput.Count);
			output.TileData.Clear();
			this.tiles.CopyTo(output.TileData, 0, this.tiles.Count);

			output.RenderData = output.RenderData ?? new List<Texture>(this.renderData);
			output.RenderData.Clear();
			output.RenderData.AddRange(this.renderData);

			output.AutoTileData = output.AutoTileData ?? new List<TilesetAutoTileInfo>();
			output.AutoTileData.Clear();
			this.TransformAutoTileData(output.AutoTileData);

			output.EmptyTileIndex = this.GetEmptyTileIndex();

			this.ClearWorkingData();
			return output;
		}
		
		/// <summary>
		/// Clears all local state of the <see cref="TilesetCompiler"/> related to in-progress compile operations.
		/// </summary>
		private void ClearWorkingData()
		{
			this.generateTileSchedule.Clear();
			this.autoTiles.Clear();
			this.tiles.Clear();
			this.renderData.Clear();
			this.inputTileCount = 0;
			this.outputTileCount = 0;

			this.autoTileItemPool.Reset();
			Array.Clear(this.autoTileStateBuffer, 0, this.autoTileStateBuffer.Length);
		}
		/// <summary>
		/// Initializes local working data for processing the specified <see cref="TilesetCompilerInput"/>.
		/// </summary>
		/// <param name="input"></param>
		private void InitWorkingData(TilesetCompilerInput input)
		{
			// Determine how many source tiles we have. This will be the smallest overlap
			// of available rendering input layers
			this.inputTileCount = int.MaxValue;
			for (int renderInputIndex = 0; renderInputIndex < input.RenderConfig.Count; renderInputIndex++)
			{
				TilesetRenderInput renderInput = input.RenderConfig[renderInputIndex] ?? DefaultRenderInput;
				PixelData sourceLayerData = (renderInput.SourceData.Res ?? Pixmap.Checkerboard.Res).MainLayer;
				LayerGeometry layerGeometry = this.CalculateLayerGeometry(renderInput, sourceLayerData);
				this.inputTileCount = Math.Min(this.inputTileCount, layerGeometry.SourceTileCount);
			}
			if (input.RenderConfig.Count == 0) this.inputTileCount = 0;

			// Initialize intermediate tile data to be big enough for the source data
			this.outputTileCount = this.inputTileCount;
			this.tiles.Count = this.inputTileCount;
			TileInfo[] data = this.tiles.Data;
			for (int i = 0; i < this.inputTileCount; i++)
			{
				data[i].IsVisuallyEmpty = true;
			}

			// Copy input data to our intermediate data structures
			TileInput[] inputData = input.TileInput.Data;
			for (int i = MathF.Min(this.inputTileCount, input.TileInput.Count); i >= 0; i--)
			{
				data[i].DepthOffset = inputData[i].DepthOffset;
				data[i].IsVertical = inputData[i].IsVertical;
				data[i].Collision = inputData[i].Collision;
			}
		}

		/// <summary>
		/// Schedules a new tile to be generated by the <see cref="TilesetCompiler"/> from four sub-tiles, 
		/// using a base tile as a reference for metadata. Returns the tile index of the scheduled new tile.
		/// </summary>
		/// <param name="baseTileIndex"></param>
		/// <param name="topLeftIndex"></param>
		/// <param name="topRightIndex"></param>
		/// <param name="bottomRightIndex"></param>
		/// <param name="bottomLeftIndex"></param>
		private int ScheduleGenerateTile(int baseTileIndex, int topLeftIndex, int topRightIndex, int bottomRightIndex, int bottomLeftIndex)
		{
			GeneratedQuadTile generatedTile = new GeneratedQuadTile
			{
				SourceBaseIndex = baseTileIndex,
				TargetIndex = this.outputTileCount,
				SourceTopLeftIndex = topLeftIndex,
				SourceTopRightIndex = topRightIndex,
				SourceBottomRightIndex = bottomRightIndex,
				SourceBottomLeftIndex = bottomLeftIndex
			};
			this.outputTileCount++;
			this.generateTileSchedule.Add(generatedTile);

			return generatedTile.TargetIndex;
		}
		/// <summary>
		/// Generates the required metadata for all tiles scheduled to be generated. See <see cref="ScheduleGenerateTile"/>.
		/// </summary>
		private void GenerateTileMetadata()
		{
			// Generate additional target tiles as scheduled
			foreach (GeneratedQuadTile tile in this.generateTileSchedule)
			{
				// Initialize the new tile with all properties from its base tile
				this.tiles.Count = MathF.Max(this.tiles.Count, tile.TargetIndex + 1);
				this.tiles[tile.TargetIndex] = this.tiles[tile.SourceBaseIndex];

				// Update additional AutoTile metadata, so if AutoTile A connects to AutoTile B,
				// all of its generated tile variants will also connect to AutoTile B.
				foreach (AutoTileData autoTile in this.autoTiles)
				{
					RawList<TilesetAutoTileItem> autoTileInfo = autoTile.TileInfo;

					// Skip tiles that were generated by the AutoTile itself, so we don't override
					// anything that was specified explicitly in prior AutoTile processing.
					if (autoTileInfo.Count > tile.TargetIndex &&
						autoTileInfo[tile.TargetIndex].IsAutoTile) continue;

					autoTileInfo.Count = MathF.Max(autoTileInfo.Count, tile.TargetIndex + 1);
					autoTileInfo[tile.TargetIndex] = autoTileInfo[tile.SourceBaseIndex];
				}
			}
		}

		/// <summary>
		/// Gathers and processes AutoTile input data into an easily modifyable intermediate format,
		/// while also collecting information on generated tiles and connectivity state mappings.
		/// </summary>
		/// <param name="autoTileConfig"></param>
		private void GatherAutoTileData(IReadOnlyList<TilesetAutoTileInput> autoTileConfig)
		{
			for (int autoTileIndex = 0; autoTileIndex < autoTileConfig.Count; autoTileIndex++)
			{
				TilesetAutoTileInput autoTileInput = autoTileConfig[autoTileIndex];
				AutoTileData autoTile = new AutoTileData
				{
					BaseTile = MathF.Clamp(autoTileInput.BaseTileIndex, 0, this.inputTileCount - 1),
					TileInfo = this.autoTileItemPool.Rent(this.inputTileCount),
					StateToTile = new int[(int)TileConnection.All + 1],
					IsStateAvailable = new bool[(int)TileConnection.All + 1]
				};
				autoTile.TileInfo.Count = this.inputTileCount;

				// Initialize the tile mapping for all potential connection states with the base tile
				for (int conIndex = 0; conIndex < autoTile.StateToTile.Length; conIndex++)
				{
					autoTile.StateToTile[conIndex] = autoTile.BaseTile;
				}

				// Use the directly applicable tile mapping as-is
				int autoTileSourceTileCount = MathF.Min(autoTileInput.TileInput.Count, this.inputTileCount);
				for (int tileIndex = autoTileSourceTileCount - 1; tileIndex >= 0; tileIndex--)
				{
					TilesetAutoTileItem tileInput = autoTileInput.TileInput[tileIndex];
					autoTile.TileInfo[tileIndex] = tileInput;

					if (tileInput.IsAutoTile)
					{
						autoTile.IsStateAvailable[(int)tileInput.Neighbours] = true;
						autoTile.StateToTile[(int)tileInput.Neighbours] = tileIndex;
						autoTile.TileInfo.Data[tileIndex].ConnectsToAutoTile = true;

						// Apply base tile information to the main tile dataset
						this.tiles.Data[tileIndex].AutoTileLayer = autoTileIndex + 1;
					}
				}

				// Attempt to construct missing tiles of the minimum required base set from existing tiles
				// by using their sub-tile quadrants individually. Use a buffer for availability checks, so
				// we don't base generated tiles on previously generated tiles.
				autoTile.IsStateAvailable.CopyTo(this.autoTileStateBuffer, 0);
				for (int i = 0; i < AutoTileFallbackMap.BaseConnectivityTiles.Count; i++)
				{
					TileConnection connectivity = AutoTileFallbackMap.BaseConnectivityTiles[i];
					if (this.autoTileStateBuffer[(int)connectivity]) continue;

					TileConnection topLeft = FindGeneratedAutoTileBase(TileQuadrant.TopLeft, connectivity, this.autoTileStateBuffer);
					TileConnection topRight = FindGeneratedAutoTileBase(TileQuadrant.TopRight, connectivity, this.autoTileStateBuffer);
					TileConnection bottomRight = FindGeneratedAutoTileBase(TileQuadrant.BottomRight, connectivity, this.autoTileStateBuffer);
					TileConnection bottomLeft = FindGeneratedAutoTileBase(TileQuadrant.BottomLeft, connectivity, this.autoTileStateBuffer);

					// Skip cases where we can't construct a full tile
					if (topLeft == TileConnection.None) continue;
					if (topRight == TileConnection.None) continue;
					if (bottomRight == TileConnection.None) continue;
					if (bottomLeft == TileConnection.None) continue;

					int generatedIndex = this.ScheduleGenerateTile(
						autoTile.BaseTile,
						autoTile.StateToTile[(int)topLeft],
						autoTile.StateToTile[(int)topRight],
						autoTile.StateToTile[(int)bottomRight],
						autoTile.StateToTile[(int)bottomLeft]);

					autoTile.IsStateAvailable[(int)connectivity] = true;
					autoTile.StateToTile[(int)connectivity] = generatedIndex;
					autoTile.TileInfo.Count = MathF.Max(autoTile.TileInfo.Count, generatedIndex + 1);
					autoTile.TileInfo.Data[generatedIndex] = autoTileInput.TileInput[autoTile.BaseTile];
					autoTile.TileInfo[generatedIndex] = new TilesetAutoTileItem
					{
						IsAutoTile = true,
						ConnectsToAutoTile = true,
						Neighbours = connectivity
					};
				}

				// Fill up unavailable state mappings with the closest available match
				for (int stateIndex = 0; stateIndex < autoTile.IsStateAvailable.Length; stateIndex++)
				{
					if (autoTile.IsStateAvailable[stateIndex]) continue;

					IReadOnlyList<TileConnection> fallbacks = AutoTileFallbackMap.GetFallback((TileConnection)stateIndex);
					for (int i = 0; i < fallbacks.Count; i++)
					{
						int fallbackStateIndex = (int)fallbacks[i];
						if (autoTile.IsStateAvailable[fallbackStateIndex])
						{
							autoTile.StateToTile[stateIndex] = autoTile.StateToTile[fallbackStateIndex];
							break;
						}
					}
				}

				// Add the gathered info to our local working data
				this.autoTiles.Add(autoTile);
			}
		}
		/// <summary>
		/// Transforms the intermediate AutoTile data into an output format that is optimized for 
		/// efficient reading and updating operations.
		/// 
		/// Requires the total number of output tiles, including generated ones, to be known.
		/// </summary>
		private void TransformAutoTileData(List<TilesetAutoTileInfo> outputData)
		{
			for (int autoTileIndex = 0; autoTileIndex < this.autoTiles.Count; autoTileIndex++)
			{
				AutoTileData data = this.autoTiles[autoTileIndex];

				TilesetAutoTileItem[] tileInfo = new TilesetAutoTileItem[this.outputTileCount];
				data.TileInfo.CopyTo(tileInfo, 0);

				outputData.Add(new TilesetAutoTileInfo(
					data.BaseTile,
					data.StateToTile,
					tileInfo));
			}
		}
		/// <summary>
		/// Determines the overall geometry of a single <see cref="Tileset"/> visual layer. This involves
		/// tile boundaries in source and target data, as well as texture sizes and similar.
		/// </summary>
		/// <param name="renderInput"></param>
		/// <param name="layerData"></param>
		private LayerGeometry CalculateLayerGeometry(TilesetRenderInput renderInput, PixelData layerData)
		{
			LayerGeometry geometry;

			// What's the space requirement for each tile?
			geometry.SourceTileAdvance = renderInput.SourceTileAdvance;
			geometry.TargetTileAdvance = renderInput.TargetTileAdvance;

			// How many tiles will we have?
			Point2 tileCount = renderInput.GetSourceTileCount(layerData.Width, layerData.Height);
			geometry.SourceTilesPerRow = tileCount.X;
			geometry.SourceTilesPerColumn = tileCount.Y;
			geometry.SourceTileCount = geometry.SourceTilesPerRow * geometry.SourceTilesPerColumn;
			geometry.TargetTileCount = geometry.SourceTileCount + this.generateTileSchedule.Count;

			// What's the optimal texture size to include them all?
			int minTilesPerLine = MathF.Max(1, (int)MathF.Sqrt(geometry.TargetTileCount));
			geometry.TargetTextureSize.X = MathF.NextPowerOfTwo(geometry.TargetTileAdvance.X * minTilesPerLine);

			int actualTilesPerLine = geometry.TargetTextureSize.X / geometry.TargetTileAdvance.X;
			int requiredLineCount = 1 + (geometry.TargetTileCount / actualTilesPerLine);
			geometry.TargetTextureSize.Y = MathF.NextPowerOfTwo(geometry.TargetTileAdvance.Y * requiredLineCount);

			return geometry;
		}
		/// <summary>
		/// Composes pixel and atlas data for a single <see cref="Tileset"/> visual layer.
		/// </summary>
		private LayerPixelData ComposeLayerPixelData(TilesetRenderInput renderInput, PixelData sourceData, LayerGeometry geometry)
		{
			// Create a buffer for writing target pixel data
			LayerPixelData target;
			target.PixelData = new PixelData(geometry.TargetTextureSize.X, geometry.TargetTextureSize.Y);
			target.Atlas = new List<Rect>();

			// Iterate over source tiles and copy each tile from source to target
			Point2 targetTilePos = new Point2(0, 0);
			for (int tileIndex = 0; tileIndex < geometry.SourceTileCount; tileIndex++)
			{
				// Determine source and target positions on pixel data / buffer
				Point2 sourceTilePos = geometry.GetSourceTilePos(tileIndex);
				Point2 targetContentPos = new Point2(
					targetTilePos.X + renderInput.TargetTileMargin, 
					targetTilePos.Y + renderInput.TargetTileMargin);

				// Draw the source tile onto the target buffer, including its spacing / border
				sourceData.DrawOnto(target.PixelData,
					BlendMode.Solid, 
					targetContentPos.X, 
					targetContentPos.Y, 
					renderInput.SourceTileSize.X, 
					renderInput.SourceTileSize.Y, 
					sourceTilePos.X, 
					sourceTilePos.Y);

				// Fill up the target spacing area with similar pixels
				if (renderInput.TargetTileMargin > 0)
				{
					FillTileSpacing(target.PixelData, renderInput.TargetTileMargin, targetContentPos, renderInput.SourceTileSize);
				}

				// Update whether the tile is considered visually empty
				if (this.tiles.Data[tileIndex].IsVisuallyEmpty)
				{
					bool isLayerVisuallyEmpty = IsCompletelyTransparent(
						sourceData, 
						sourceTilePos,
						renderInput.SourceTileSize);
					if (!isLayerVisuallyEmpty)
						this.tiles.Data[tileIndex].IsVisuallyEmpty = false;
				}

				// Add an entry to the generated atlas
				Rect atlasRect = new Rect(
					targetTilePos.X + renderInput.TargetTileMargin, 
					targetTilePos.Y + renderInput.TargetTileMargin, 
					geometry.TargetTileAdvance.X - renderInput.TargetTileMargin * 2, 
					geometry.TargetTileAdvance.Y - renderInput.TargetTileMargin * 2);
				target.Atlas.Add(atlasRect);

				// Advance the target tile position
				targetTilePos.X += geometry.TargetTileAdvance.X;
				if (targetTilePos.X + geometry.TargetTileAdvance.X > target.PixelData.Width)
				{
					targetTilePos.X = 0;
					targetTilePos.Y += geometry.TargetTileAdvance.Y;
				}
			}

			// Generate additional target tiles as scheduled
			foreach (GeneratedQuadTile tile in this.generateTileSchedule)
			{
				// Determine source and target positions on pixel data / buffer
				Point2 sourceTilePosTopLeft = geometry.GetSourceTilePos(tile.SourceTopLeftIndex);
				Point2 sourceTilePosTopRight = geometry.GetSourceTilePos(tile.SourceTopRightIndex);
				Point2 sourceTilePosBottomRight = geometry.GetSourceTilePos(tile.SourceBottomRightIndex);
				Point2 sourceTilePosBottomLeft = geometry.GetSourceTilePos(tile.SourceBottomLeftIndex);
				Point2 targetContentPos = new Point2(
					targetTilePos.X + renderInput.TargetTileMargin,
					targetTilePos.Y + renderInput.TargetTileMargin);

				// Draw the source tile onto the target buffer, including its spacing / border
				Point2 quadSize = renderInput.SourceTileSize / 2;
				sourceData.DrawOnto(target.PixelData, BlendMode.Solid,
					targetContentPos.X,
					targetContentPos.Y,
					quadSize.X,
					quadSize.Y,
					sourceTilePosTopLeft.X,
					sourceTilePosTopLeft.Y);
				sourceData.DrawOnto(target.PixelData, BlendMode.Solid,
					targetContentPos.X + quadSize.X,
					targetContentPos.Y,
					quadSize.X,
					quadSize.Y,
					sourceTilePosTopRight.X + quadSize.X,
					sourceTilePosTopRight.Y);
				sourceData.DrawOnto(target.PixelData, BlendMode.Solid,
					targetContentPos.X + quadSize.X,
					targetContentPos.Y + quadSize.Y,
					quadSize.X,
					quadSize.Y,
					sourceTilePosBottomRight.X + quadSize.X,
					sourceTilePosBottomRight.Y + quadSize.Y);
				sourceData.DrawOnto(target.PixelData, BlendMode.Solid,
					targetContentPos.X,
					targetContentPos.Y + quadSize.Y,
					quadSize.X,
					quadSize.Y,
					sourceTilePosBottomLeft.X,
					sourceTilePosBottomLeft.Y + quadSize.Y);

				// Fill up the target spacing area with similar pixels
				if (renderInput.TargetTileMargin > 0)
				{
					FillTileSpacing(target.PixelData, renderInput.TargetTileMargin, targetContentPos, renderInput.SourceTileSize);
				}

				// Add an entry to the generated atlas
				Rect atlasRect = new Rect(
					targetTilePos.X + renderInput.TargetTileMargin,
					targetTilePos.Y + renderInput.TargetTileMargin,
					geometry.TargetTileAdvance.X - renderInput.TargetTileMargin * 2,
					geometry.TargetTileAdvance.Y - renderInput.TargetTileMargin * 2);
				target.Atlas.Add(atlasRect);

				// Advance the target tile position
				targetTilePos.X += geometry.TargetTileAdvance.X;
				if (targetTilePos.X + geometry.TargetTileAdvance.X > target.PixelData.Width)
				{
					targetTilePos.X = 0;
					targetTilePos.Y += geometry.TargetTileAdvance.Y;
				}
			}

			// Update which tiles are considered visually empty
			for (int tileIndex = 0; tileIndex < this.outputTileCount; tileIndex++)
			{
				// Determine target positions on pixel data / buffer
				Rect targetRect = target.Atlas[tileIndex];

				// Update whether the tile is considered visually empty
				if (this.tiles.Data[tileIndex].IsVisuallyEmpty)
				{
					bool isLayerVisuallyEmpty = IsCompletelyTransparent(
						target.PixelData,
						(Point2)targetRect.Pos,
						(Point2)targetRect.Size);
					if (!isLayerVisuallyEmpty)
						this.tiles.Data[tileIndex].IsVisuallyEmpty = false;
				}
			}

			return target;
		}

		private int GetEmptyTileIndex()
		{
			return this.tiles.Data.IndexOfFirst(tileInfo => tileInfo.IsVisuallyEmpty);
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

		/// <summary>
		/// Finds an existing base tile where a certain sub-tile matches with the specified connectivity state.
		/// Returns <see cref="TileConnection.None"/> if no match was found in the existing set.
		/// </summary>
		/// <param name="quadrant"></param>
		/// <param name="targetConnectivity"></param>
		/// <param name="isStateAvailable"></param>
		private static TileConnection FindGeneratedAutoTileBase(TileQuadrant quadrant, TileConnection targetConnectivity, bool[] isStateAvailable)
		{
			TileConnection mask = AutoTileFallbackMap.GetSubTileMask(quadrant);
			TileConnection targetBits = targetConnectivity & mask;

			IReadOnlyList<TileConnection> baseTiles = AutoTileFallbackMap.BaseConnectivityTiles;
			TileConnection bestMatch = TileConnection.None;
			int bestMatchNeighbourCount = 0;
			for (int i = 0; i < baseTiles.Count; i++)
			{
				// Skip tiles that are not available in the tileset
				TileConnection connectivity = baseTiles[i];
				if (!isStateAvailable[(int)connectivity]) continue;

				// Skip tiles that do not match in the required bits
				TileConnection bits = connectivity & mask;
				if (bits != targetBits) continue;

				// Special case: Skip the entirely unconnected tile, since this one is often
				// different from any partially connected tiles. Note if this should change: 
				// It's also currently conflicting with the "no match found" return value.
				if (connectivity == TileConnection.None) continue;

				// Prefer the most connected match we can find, since less connected tiles
				// tend to be more specialized in their visual appearance. Only consider
				// connectivity that we also find in the target tile to avoid under-specializing.
				TileConnection sharedConnectivity = connectivity & targetConnectivity;
				int neighbourCount = GetConnectedNeighbours(sharedConnectivity);
				if (neighbourCount > bestMatchNeighbourCount)
				{
					bestMatchNeighbourCount = neighbourCount;
					bestMatch = connectivity;
				}
			}

			return bestMatch;
		}

		/// <summary>
		/// Counts the number of connected neighbours in the specified connectivity state.
		/// </summary>
		/// <param name="connectivity"></param>
		private static int GetConnectedNeighbours(TileConnection connectivity)
		{
			// See here: https://stackoverflow.com/questions/12171584/what-is-the-fastest-way-to-count-set-bits-in-uint32
			int count = 0;
			int bits = (int)connectivity;
			while (bits != 0)
			{
				count++;
				bits &= bits - 1;
			}
			return count;
		}
	}
}
