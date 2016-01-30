using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;


namespace Duality.Editor.Plugins.Tilemaps.EditorActions
{
	/// <summary>
	/// Opens a setup dialog that allows to configure the selected tilemaps.
	/// </summary>
	public class OpenTilemapResizeDialog : EditorAction<Tilemap>
	{
		public override string Name
		{
			get { return TilemapsRes.ActionName_ResizeTilemap; }
		}
		public override string Description
		{
			get { return TilemapsRes.ActionDesc_ResizeTilemap; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconResize; }
		}

		public override void Perform(IEnumerable<Tilemap> objEnum)
		{
			TilemapSetupDialog resizeDialog = new TilemapSetupDialog();
			resizeDialog.Tilemaps = objEnum;
			resizeDialog.ShowDialog();
		}
		public override bool MatchesContext(string context)
		{
			return context == TilemapsEditorPlugin.ActionTilemapEditor;
		}
		public override bool CanPerformOn(IEnumerable<Tilemap> objEnum)
		{
			return base.CanPerformOn(objEnum) && objEnum.Any();
		}
	}
}
