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
		public static readonly Bitmap IconTileSelect   = TilemapsRes.IconTileSelect;
		public static readonly Bitmap IconTileBrush    = TilemapsRes.IconTileBrush;
		public static readonly Bitmap IconTileFill     = TilemapsRes.IconTileFill;
		public static readonly Bitmap IconTileOval     = TilemapsRes.IconTileOval;
		public static readonly Bitmap IconTileRect     = TilemapsRes.IconTileRect;
		public static readonly Bitmap IconPick         = TilemapsRes.IconPick;
		public static readonly Cursor CursorTileSelect = CursorHelper.ArrowAction;
		public static readonly Cursor CursorTileBrush  = CursorHelper.CreateCursor(TilemapsRes.CursorTileBrush, 1, 13);
		public static readonly Cursor CursorTileRect   = CursorHelper.CreateCursor(TilemapsRes.CursorTileRect, 0, 0);
		public static readonly Cursor CursorTileOval   = CursorHelper.CreateCursor(TilemapsRes.CursorTileOval, 0, 0);
		public static readonly Cursor CursorTileFill   = CursorHelper.CreateCursor(TilemapsRes.CursorTileFill, 13, 12);
		public static readonly Cursor CursorPick       = CursorHelper.CreateCursor(TilemapsRes.CursorPick, 1, 16);
	}
}
