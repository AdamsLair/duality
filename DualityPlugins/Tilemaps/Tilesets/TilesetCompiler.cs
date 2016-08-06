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
			public Point2 SourceTileBounds;
			public int SourceTilesPerRow;
			public int SourceTilesPerColumn;

			public int TargetTileCount;
			public Point2 TargetTileBounds;
			public Point2 TargetTextureSize;
		}
		private struct LayerPixelData
		{
			public PixelData PixelData;
			public List<Rect> Atlas;
		}

		private static readonly TilesetRenderInput DefaultRenderInput = new TilesetRenderInput();

		/// <summary>
		/// Compiles a <see cref="Tileset"/> using its specified source data, in order to
		/// generate optimized target data for rendering and collision detection.
		/// </summary>
		public TilesetCompilerOutput Compile(TilesetCompilerInput input)
		{
			TilesetCompilerOutput output = input.ExistingOutput;
			output.TileData = output.TileData ?? new RawList<TileInfo>(input.TileInput.Count);
			output.RenderData = output.RenderData ?? new List<Texture>();
			output.AutoTileData = output.AutoTileData ?? new List<TilesetAutoTileInfo>();

			// Clear existing data, but keep the sufficiently big data structures
			output.TileData.Clear();
			output.RenderData.Clear();
			output.AutoTileData.Clear();

			// Transform AutoTile data
			for (int autoTileIndex = 0; autoTileIndex < input.AutoTileConfig.Count; autoTileIndex++)
			{
				TilesetAutoTileInput autoTileInput = input.AutoTileConfig[autoTileIndex];
				int[] tileMapping = new int[(int)TileConnection.All + 1];
				
				// Initialize the tile mapping for all potential connection states with the base tile
				for (int conIndex = 0; conIndex < tileMapping.Length; conIndex++)
				{
					tileMapping[conIndex] = autoTileInput.BaseTileIndex;
				}

				// Use the directly applicable tile mapping as-is
				for (int mapIndex = autoTileInput.ConnectivityMap.Count - 1; mapIndex >= 0; mapIndex--)
				{
					TilesetAutoTileItem mapping = autoTileInput.ConnectivityMap[mapIndex];
					tileMapping[(int)mapping.Neighbours] = mapping.TileIndex;
				}

				// Add the complete AutoTile info / mapping to the result data
				TilesetAutoTileInfo autoTileInfo = new TilesetAutoTileInfo(
					autoTileInput.BaseTileIndex, 
					tileMapping);
				output.AutoTileData.Add(autoTileInfo);
			}

			// Generate output pixel data
			int minSourceTileCount = int.MaxValue;
			for (int renderInputIndex = 0; renderInputIndex < input.RenderConfig.Count; renderInputIndex++)
			{
				TilesetRenderInput renderInput = input.RenderConfig[renderInputIndex] ?? DefaultRenderInput;
				PixelData sourceLayerData = (renderInput.SourceData.Res ?? Pixmap.Checkerboard.Res).MainLayer;

				// Determine overal geometry values for this layer, such as tile bounds and texture sizes
				LayerGeometry layerGeometry = this.CalculateLayerGeometry(renderInput, sourceLayerData);
				minSourceTileCount = Math.Min(minSourceTileCount, layerGeometry.SourceTileCount);

				// Generate pixel data and atlas values for this layer's texture
				LayerPixelData targetLayerData = this.GenerateLayerPixelData(
					renderInput, 
					sourceLayerData, 
					layerGeometry, 
					output.TileData);

				// Create the texture to be used for this rendering input
				using (Pixmap targetPixmap = new Pixmap(targetLayerData.PixelData))
				{
					targetPixmap.Atlas = targetLayerData.Atlas;
					Texture targetTexture = new Texture(
						targetPixmap, TextureSizeMode.Enlarge, 
						renderInput.TargetMagFilter, renderInput.TargetMinFilter, 
						TextureWrapMode.Clamp, TextureWrapMode.Clamp, 
						renderInput.TargetFormat);

					output.RenderData.Add(targetTexture);
				}
			}

			// Generate additional per-tile data
			this.TransformTileData(input.TileInput, output.TileData, output.RenderData);

			// Apply global tileset stats
			output.TileCount = (input.RenderConfig.Count > 0) ? minSourceTileCount : 0;

			return output;
		}

		/// <summary>
		/// Determines the overall geometry of a single <see cref="Tileset"/> visual layer. This involves
		/// tile boundaries in source and target data, as well as texture sizes and similar.
		/// </summary>
		/// <param name="renderInput"></param>
		/// <param name="layerData"></param>
		/// <returns></returns>
		private LayerGeometry CalculateLayerGeometry(TilesetRenderInput renderInput, PixelData layerData)
		{
			LayerGeometry geometry;

			// What's the space requirement for each tile?
			geometry.SourceTileBounds = new Point2(
				renderInput.SourceTileSize.X + renderInput.SourceTileSpacing * 2, 
				renderInput.SourceTileSize.Y + renderInput.SourceTileSpacing * 2);
			geometry.TargetTileBounds = new Point2(
				renderInput.SourceTileSize.X + renderInput.TargetTileSpacing * 2, 
				renderInput.SourceTileSize.Y + renderInput.TargetTileSpacing * 2);

			// How many tiles will we have?
			geometry.SourceTilesPerRow = layerData.Width / geometry.SourceTileBounds.X;
			geometry.SourceTilesPerColumn = layerData.Height / geometry.SourceTileBounds.Y;
			geometry.SourceTileCount = geometry.SourceTilesPerRow * geometry.SourceTilesPerColumn;
			geometry.TargetTileCount = geometry.SourceTileCount; // ToDo: Account for expanded AutoTiles

			// What's the optimal texture size to include them all?
			int minTilesPerLine = MathF.Max(1, (int)MathF.Sqrt(geometry.TargetTileCount));
			geometry.TargetTextureSize.X = MathF.NextPowerOfTwo(geometry.TargetTileBounds.X * minTilesPerLine);

			int actualTilesPerLine = geometry.TargetTextureSize.X / geometry.TargetTileBounds.X;
			int requiredLineCount = 1 + (geometry.TargetTileCount / actualTilesPerLine);
			geometry.TargetTextureSize.Y = MathF.NextPowerOfTwo(geometry.TargetTileBounds.Y * requiredLineCount);

			return geometry;
		}
		/// <summary>
		/// Generates pixel and atlas data for a single <see cref="Tileset"/> visual layer.
		/// </summary>
		/// <param name="renderInput"></param>
		/// <param name="sourceData"></param>
		/// <param name="geometry"></param>
		/// <param name="tileData"></param>
		/// <returns></returns>
		private LayerPixelData GenerateLayerPixelData(TilesetRenderInput renderInput, PixelData sourceData, LayerGeometry geometry, RawList<TileInfo> tileData)
		{
			// Create a buffer for writing target pixel data
			LayerPixelData target;
			target.PixelData = new PixelData(geometry.TargetTextureSize.X, geometry.TargetTextureSize.Y);
			target.Atlas = new List<Rect>();

			// Iterate over tiles and move each tile from source to target
			Point2 targetTilePos = new Point2(0, 0);
			for (int tileIndex = 0; tileIndex < geometry.SourceTileCount; tileIndex++)
			{
				// Initialize a new tile info when necessary
				if (tileIndex >= tileData.Count)
				{
					tileData.Count++;
					tileData.Data[tileIndex].IsVisuallyEmpty = true;
				}

				// Determine where on the source buffer the tile is located
				Point2 sourceTilePos = new Point2(
					geometry.SourceTileBounds.X * (tileIndex % geometry.SourceTilesPerRow),
					geometry.SourceTileBounds.Y * (tileIndex / geometry.SourceTilesPerRow));

				// Draw the source tile onto the target buffer, including its spacing / border
				Point2 targetContentPos = new Point2(
					targetTilePos.X + renderInput.TargetTileSpacing, 
					targetTilePos.Y + renderInput.TargetTileSpacing);
				sourceData.DrawOnto(target.PixelData, 
					BlendMode.Solid, 
					targetContentPos.X, 
					targetContentPos.Y, 
					renderInput.SourceTileSize.X, 
					renderInput.SourceTileSize.Y, 
					sourceTilePos.X + renderInput.SourceTileSpacing, 
					sourceTilePos.Y + renderInput.SourceTileSpacing);

				// Fill up the target spacing area with similar pixels
				if (renderInput.TargetTileSpacing > 0)
				{
					FillTileSpacing(target.PixelData, renderInput.TargetTileSpacing, targetContentPos, renderInput.SourceTileSize);
				}

				// Update whether the tile is considered visually empty
				if (tileData.Data[tileIndex].IsVisuallyEmpty)
				{
					bool isLayerVisuallyEmpty = IsCompletelyTransparent(
						sourceData, 
						new Point2(
							sourceTilePos.X + renderInput.SourceTileSpacing,
							sourceTilePos.Y + renderInput.SourceTileSpacing),
						renderInput.SourceTileSize);
					if (!isLayerVisuallyEmpty)
						tileData.Data[tileIndex].IsVisuallyEmpty = false;
				}

				// Add an entry to the generated atlas
				Rect atlasRect = new Rect(
					targetTilePos.X + renderInput.TargetTileSpacing, 
					targetTilePos.Y + renderInput.TargetTileSpacing, 
					geometry.TargetTileBounds.X - renderInput.TargetTileSpacing * 2, 
					geometry.TargetTileBounds.Y - renderInput.TargetTileSpacing * 2);
				target.Atlas.Add(atlasRect);

				// Advance the target tile position
				targetTilePos.X += geometry.TargetTileBounds.X;
				if (targetTilePos.X + geometry.TargetTileBounds.X > target.PixelData.Width)
				{
					targetTilePos.X = 0;
					targetTilePos.Y += geometry.TargetTileBounds.Y;
				}
			}

			return target;
		}
		/// <summary>
		/// Transforms regular <see cref="TileInput"/> data into compiled <see cref="TileInfo"/> output data
		/// and enhances it with performance-relevant additional information in the process.
		/// </summary>
		/// <param name="tileInput"></param>
		/// <param name="tileData"></param>
		/// <param name="renderData"></param>
		private void TransformTileData(RawList<TileInput> tileInput, RawList<TileInfo> tileData, List<Texture> renderData)
		{
			// Copy input data
			for (int i = 0; i < tileData.Count; i++)
			{
				if (i >= tileInput.Count) break;
				tileData.Data[i].DepthOffset = tileInput.Data[i].DepthOffset;
				tileData.Data[i].IsVertical  = tileInput.Data[i].IsVertical;
				tileData.Data[i].Collision   = tileInput.Data[i].Collision;
			}

			// Retrieve texture atlas data for quick lookup during rendering
			if (renderData.Count > 0)
			{
				for (int i = 0; i < tileData.Count; i++)
				{
					renderData[0].LookupAtlas(i, out tileData.Data[i].TexCoord0);
				}
			}
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
