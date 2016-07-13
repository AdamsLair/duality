using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Editor.Plugins.Tilemaps.Properties;


namespace Duality.Editor.Plugins.Tilemaps.CamViewStates
{
	/// <summary>
	/// Selects the topmost tilemap that is hovered by the mouse cursor for editing.
	/// </summary>
	public class SelectTilemapTool : TilemapTool
	{
		public override string Name
		{
			get { return TilemapsRes.ItemName_TileSelect; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTileSelect; }
		}
		public override Cursor ActionCursor
		{
			get { return TilemapsResCache.CursorTileSelect; }
		}
		public override Keys ShortcutKey
		{
			get { return Keys.Q; }
		}
		public override bool PickPreferSelectedLayer
		{
			get { return false; }
		}
		public override int SortOrder
		{
			get { return -1000; }
		}

		public override void UpdatePreview()
		{
			ITilemapToolEnvironment env = this.Environment;

			env.ActiveAreaOutlines.Clear();
			env.ActiveOrigin = env.HoveredTile;
			env.ActiveArea.ResizeClear(1, 1);
			env.ActiveArea[0, 0] = true;
			env.SubmitActiveAreaChanges(true);
		}
	}
}
