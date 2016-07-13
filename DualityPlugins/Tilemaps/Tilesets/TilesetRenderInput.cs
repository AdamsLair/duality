using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Describes a single rendering input in a <see cref="Tileset"/> definition. Each tile's visual representation is 
	/// composed of all rendering inputs that are defined in a <see cref="Tileset"/> - essentially, they are mapped to 
	/// the different texture inputs used when rendering each tile.
	/// </summary>
	public class TilesetRenderInput
	{
		public static readonly string MainTexName    = "Main Texture";
		public static readonly string MainTexId      = "mainTex";
		public static readonly string CustomTexName  = "Custom Texture";
		public static readonly string CustomTexId    = "customTex";

		private static readonly string DefaultName   = MainTexName;
		private static readonly string DefaultId     = MainTexId;

		private string             name              = DefaultName;
		private string             id                = DefaultId;
		private ContentRef<Pixmap> sourceData        = null;
		private Point2             sourceTileSize    = new Point2(32, 32);
		private int                sourceTileSpacing = 0;
		private int                targetTileSpacing = 1;
		private TextureMagFilter   targetMagFilter   = TextureMagFilter.Linear;
		private TextureMinFilter   targetMinFilter   = TextureMinFilter.LinearMipmapLinear;
		private TexturePixelFormat targetFormat      = TexturePixelFormat.Rgba;

		/// <summary>
		/// [GET / SET] The human-friendly name of this rendering input.
		/// </summary>
		public string Name
		{
			get { return this.name; }
			set { this.name = value ?? DefaultName; }
		}
		/// <summary>
		/// [GET / SET] The id of this rendering input, which can later be used for mapping it to <see cref="Material"/> texture slots, etc.
		/// </summary>
		public string Id
		{
			get { return this.id; }
			set { this.id = value ?? DefaultId; }
		}
		/// <summary>
		/// [GET / SET] The source pixel data from which this rendering input will be created. This may end up to be equal to the actually used
		/// pixel data, but it doesn't necessarily have to be, as the <see cref="Tileset"/> is allowed to make internal modifications and optimizations
		/// while preparing the data for rendering.
		/// </summary>
		public ContentRef<Pixmap> SourceData
		{
			get { return this.sourceData; }
			set { this.sourceData = value; }
		}
		/// <summary>
		/// [GET / SET] The width and height (in pixels) of each tile in the specified source data. This is usually equal in all
		/// rendering inputs of a <see cref="Tileset"/>. Whether or not this tile size is used in the generated target data is up to the
		/// <see cref="Tileset"/> implementation.
		/// </summary>
		public Point2 SourceTileSize
		{
			get { return this.sourceTileSize; }
			set { this.sourceTileSize = value; }
		}
		/// <summary>
		/// [GET / SET] The spacing (in pixels) around each tile in the source data. A spacing of one means that there is a 1-pixel-wide space on
		/// each side of every tile, meaning that the actual space between two tiles will be two pixels.
		/// </summary>
		public int SourceTileSpacing
		{
			get { return this.sourceTileSpacing; }
			set { this.sourceTileSpacing = value; }
		}
		/// <summary>
		/// [GET / SET] The spacing (in pixels) around each tile in the target data. A spacing of one means that there is a 1-pixel-wide space on
		/// each side of every tile, meaning that the actual space between two tiles will be two pixels. The spacing is also applied on edges of the
		/// provided pixel data, meaning that the spacing will also generate an offset for top-left and bottom-right tiles. 
		/// Whether or not this tile spacing is used in the generated target data is up to the <see cref="Tileset"/> implementation.
		/// </summary>
		public int TargetTileSpacing
		{
			get { return this.targetTileSpacing; }
			set { this.targetTileSpacing = value; }
		}
		/// <summary>
		/// [GET / SET] The target data's magnification (zooming in) filtering algorithm.
		/// </summary>
		public TextureMagFilter TargetMagFilter
		{
			get { return this.targetMagFilter; }
			set { this.targetMagFilter = value; }
		}
		/// <summary>
		/// [GET / SET] The target data's minification (zooming out) filtering algorithm.
		/// </summary>
		public TextureMinFilter TargetMinFilter
		{
			get { return this.targetMinFilter; }
			set { this.targetMinFilter = value; }
		}
		/// <summary>
		/// [GET / SET] The internal texture format of the generated target data.
		/// </summary>
		public TexturePixelFormat TargetFormat
		{
			get { return this.targetFormat; }
			set { this.targetFormat = value; }
		}
	}
}
