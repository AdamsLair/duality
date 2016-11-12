using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Resources;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;


namespace Duality.Editor.Plugins.Tilemaps.EditorActions
{
	/// <summary>
	/// Opens a setup dialog that allows to configure the selected tilemaps.
	/// </summary>
	public class OpenTilemapSetupDialog : EditorAction<Tilemap>
	{
		public override string Name
		{
			get { return TilemapsRes.ActionName_SetupTilemap; }
		}
		public override string Description
		{
			get { return TilemapsRes.ActionDesc_SetupTilemap; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconSetup; }
		}
		public override int Priority
		{
			get { return base.Priority + 1; }
		}

		public override void Perform(IEnumerable<Tilemap> objEnum)
		{
			// Don't show this dialog as a modal dialog, because we need the
			// user to be able to perform dragdrop operations to assign the Tileset.
			TilemapSetupDialog setupDialog = new TilemapSetupDialog();
			setupDialog.Tileset = 
				TilemapsEditorSelectionParser.QuerySelectedTileset().Res ?? 
				TilemapsEditorSelectionParser.GetTilesetsInScene(Scene.Current).FirstOrDefault().Res;
			setupDialog.ShowCentered(DualityEditorApp.MainForm);
		}
		public override bool MatchesContext(string context)
		{
			return context == TilemapsEditorPlugin.ActionTilemapEditor;
		}
		public override bool CanPerformOn(IEnumerable<Tilemap> objEnum)
		{
			return true;
		}
	}
}
