using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality.Resources;
using Duality.Properties;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps.EditorActions
{
	public class OpenTileset : EditorSingleAction<Tileset>
	{
		public override string Name
		{
			get { return TilemapsRes.ActionName_OpenTileset; }
		}
		public override string Description
		{
			get { return TilemapsRes.ActionDesc_OpenTileset; }
		}

		public override void Perform(Tileset obj)
		{
			DualityEditorApp.Select(this, new ObjectSelection(obj));
			TilemapsEditorPlugin.Instance.RequestTilesetEditor();
		}
		public override bool CanPerformOn(Tileset obj)
		{
			return obj != null && base.CanPerformOn(obj);
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
}
