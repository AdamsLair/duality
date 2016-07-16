using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using Duality.Editor;

namespace Duality.Editor.Plugins.Tilemaps.Properties
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	public static class TilemapsResCache
	{
		public static readonly Bitmap IconTileSelect               = TilemapsRes.IconTileSelect;
		public static readonly Bitmap IconTileBrush                = TilemapsRes.IconTileBrush;
		public static readonly Bitmap IconTileFill                 = TilemapsRes.IconTileFill;
		public static readonly Bitmap IconTileOval                 = TilemapsRes.IconTileOval;
		public static readonly Bitmap IconTileRect                 = TilemapsRes.IconTileRect;
		public static readonly Bitmap IconPick                     = TilemapsRes.IconPick;
		public static readonly Bitmap IconTilePalette              = TilemapsRes.IconTilePalette;
		public static readonly Bitmap IconTilesetEditor            = TilemapsRes.IconTilesetEditor;
		public static readonly Bitmap IconTilesetSingleVisualLayer = TilemapsRes.IconTilesetSingleVisualLayer;
		public static readonly Bitmap IconResize                   = TilemapsRes.IconResize;
		public static readonly Bitmap IconSetup                    = TilemapsRes.IconSetup;
		public static readonly Bitmap IconTilesetCollisionInfo     = TilemapsRes.IconTilesetCollisionInfo;
		public static readonly Bitmap IconTilesetDepthInfo         = TilemapsRes.IconTilesetDepthInfo;
		public static readonly Bitmap IconTilesetDepthLayer        = TilemapsRes.IconTilesetDepthLayer;
		public static readonly Bitmap IconTilesetDepthVertical     = TilemapsRes.IconTilesetDepthVertical;
		public static readonly Bitmap IconTilesetDepthFlatTile     = TilemapsRes.IconTilesetDepthFlatTile;
		public static readonly Bitmap IconTilesetDepthVerticalTile = TilemapsRes.IconTilesetDepthVerticalTile;
		public static readonly Bitmap IconTilesetVisualLayers      = TilemapsRes.IconTilesetVisualLayers;
		public static readonly Bitmap IconTilesetCollisionLayer    = TilemapsRes.IconTilesetCollisionLayer;
		public static readonly Bitmap TilesetCollisionBit          = TilemapsRes.TilesetCollisionBit;
		public static readonly Bitmap TilesetCollisionDiagUp       = TilemapsRes.TilesetCollisionDiagUp;
		public static readonly Bitmap TilesetCollisionDiagDown     = TilemapsRes.TilesetCollisionDiagDown;
		public static readonly Bitmap TilesetCollisionHorizontal   = TilemapsRes.TilesetCollisionHorizontal;
		public static readonly Bitmap TilesetCollisionVertical     = TilemapsRes.TilesetCollisionVertical;
		public static readonly Cursor CursorTileSelect             = CursorHelper.ArrowAction;
		public static readonly Cursor CursorTileBrush              = CursorHelper.CreateCursor(TilemapsRes.CursorTileBrush, 1, 13);
		public static readonly Cursor CursorTileRect               = CursorHelper.CreateCursor(TilemapsRes.CursorTileRect, 0, 0);
		public static readonly Cursor CursorTileOval               = CursorHelper.CreateCursor(TilemapsRes.CursorTileOval, 0, 0);
		public static readonly Cursor CursorTileFill               = CursorHelper.CreateCursor(TilemapsRes.CursorTileFill, 13, 12);
		public static readonly Cursor CursorPick                   = CursorHelper.CreateCursor(TilemapsRes.CursorPick, 1, 16);
	}
}
