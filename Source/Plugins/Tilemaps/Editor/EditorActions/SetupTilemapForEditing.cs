using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps.EditorActions
{
	public class SetupTilemapForEditing : EditorSingleAction<Tilemap>
	{
		public override bool CanPerformOn(Tilemap obj)
		{
			return base.CanPerformOn(obj) && obj.Size == Point2.Zero;
		}
		public override void Perform(Tilemap tilemap)
		{
			if (tilemap.Size != Point2.Zero) return;

			// Set up the tilemap using default settings.
			TilemapsSetupUtility.SetupTilemap(tilemap, null);
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextSetupObjectForEditing;
		}
	}
}
