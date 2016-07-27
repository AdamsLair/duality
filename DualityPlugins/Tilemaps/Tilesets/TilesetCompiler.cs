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

			// Generate output pixel data
			int minSourceTileCount = int.MaxValue;
			for (int renderInputIndex = 0; renderInputIndex < input.RenderConfig.Count; renderInputIndex++)
			{
				TilesetRenderInput renderInput = input.RenderConfig[renderInputIndex] ?? DefaultRenderInput;
				PixelData renderSourceData = (renderInput.SourceData.Res ?? Pixmap.Checkerboard.Res).MainLayer;

				// What's the space requirement for each tile?
				Point2 sourceTileBounds = new Point2(
					renderInput.SourceTileSize.X + renderInput.SourceTileSpacing * 2, 
					renderInput.SourceTileSize.Y + renderInput.SourceTileSpacing * 2);
				Point2 targetTileBounds = new Point2(
					renderInput.SourceTileSize.X + renderInput.TargetTileSpacing * 2, 
					renderInput.SourceTileSize.Y + renderInput.TargetTileSpacing * 2);

				// How many tiles will we have?
				int sourceHorizontalCount = renderSourceData.Width / sourceTileBounds.X;
				int sourceVerticalCount = renderSourceData.Height / sourceTileBounds.Y;
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
				PixelData renderTargetData = new PixelData(targetTextureWidth, targetTextureHeight);

				// Iterate over tiles and move each tile from source to target
				List<Rect> tileAtlas = new List<Rect>();
				Point2 targetTilePos = new Point2(0, 0);
				for (int tileIndex = 0; tileIndex < sourceTileCount; tileIndex++)
				{
					// Initialize a new tile info when necessary
					if (tileIndex >= output.TileData.Count)
					{
						output.TileData.Count++;
						output.TileData.Data[tileIndex].IsVisuallyEmpty = true;
					}

					// Determine where on the source buffer the tile is located
					Point2 sourceTilePos = new Point2(
						sourceTileBounds.X * (tileIndex % sourceHorizontalCount),
						sourceTileBounds.Y * (tileIndex / sourceHorizontalCount));

					// Draw the source tile onto the target buffer, including its spacing / border
					Point2 targetContentPos = new Point2(
						targetTilePos.X + renderInput.TargetTileSpacing, 
						targetTilePos.Y + renderInput.TargetTileSpacing);
					renderSourceData.DrawOnto(renderTargetData, 
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
						FillTileSpacing(renderTargetData, renderInput.TargetTileSpacing, targetContentPos, renderInput.SourceTileSize);
					}

					// Update whether the tile is considered visually empty
					if (output.TileData.Data[tileIndex].IsVisuallyEmpty)
					{
						bool isLayerVisuallyEmpty = IsCompletelyTransparent(
							renderSourceData, 
							new Point2(
								sourceTilePos.X + renderInput.SourceTileSpacing,
								sourceTilePos.Y + renderInput.SourceTileSpacing),
							renderInput.SourceTileSize);
						if (!isLayerVisuallyEmpty)
							output.TileData.Data[tileIndex].IsVisuallyEmpty = false;
					}

					// Add an entry to the generated atlas
					Rect atlasRect = new Rect(
						targetTilePos.X + renderInput.TargetTileSpacing, 
						targetTilePos.Y + renderInput.TargetTileSpacing, 
						targetTileBounds.X - renderInput.TargetTileSpacing * 2, 
						targetTileBounds.Y - renderInput.TargetTileSpacing * 2);
					tileAtlas.Add(atlasRect);

					// Advance the target tile position
					targetTilePos.X += targetTileBounds.X;
					if (targetTilePos.X + targetTileBounds.X > renderTargetData.Width)
					{
						targetTilePos.X = 0;
						targetTilePos.Y += targetTileBounds.Y;
					}
				}

				// Create the texture to be used for this rendering input
				using (Pixmap targetPixmap = new Pixmap(renderTargetData))
				{
					targetPixmap.Atlas = tileAtlas;
					Texture targetTexture = new Texture(
						targetPixmap, TextureSizeMode.Enlarge, 
						renderInput.TargetMagFilter, renderInput.TargetMinFilter, 
						TextureWrapMode.Clamp, TextureWrapMode.Clamp, 
						renderInput.TargetFormat);

					output.RenderData.Add(targetTexture);
				}
			}

			// Generate additional per-tile data
			{
				// Copy input data
				for (int i = 0; i < output.TileData.Count; i++)
				{
					if (i >= input.TileInput.Count) break;
					output.TileData.Data[i].DepthOffset = input.TileInput.Data[i].DepthOffset;
					output.TileData.Data[i].IsVertical  = input.TileInput.Data[i].IsVertical;
					output.TileData.Data[i].Collision   = input.TileInput.Data[i].Collision;
				}

				// Retrieve texture atlas data for quick lookup during rendering
				if (output.RenderData.Count > 0)
				{
					for (int i = 0; i < output.TileData.Count; i++)
					{
						output.RenderData[0].LookupAtlas(i, out output.TileData.Data[i].TexCoord0);
					}
				}
			}

			// Apply global tileset stats
			output.TileCount = (input.RenderConfig.Count > 0) ? minSourceTileCount : 0;

			return output;
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
