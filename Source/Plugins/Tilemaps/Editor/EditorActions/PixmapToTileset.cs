using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality.Resources;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps.EditorActions
{
	/// <summary>
	/// Creates a new Tileset Resource based on the Pixmap.
	/// </summary>
	public class PixmapToTileset : EditorSingleAction<Pixmap>
	{
		public override string Name
		{
			get { return TilemapsRes.ActionName_CreateTileset; }
		}
		public override Image Icon
		{
			get { return typeof(Tileset).GetEditorImage(); }
		}

		public override void Perform(Pixmap obj)
		{
			string targetPath = PathHelper.GetFreePath(obj.FullName, Resource.GetFileExtByType<Tileset>());
			Tileset targetRes = new Tileset();
			targetRes.RenderConfig.Add(new TilesetRenderInput
			{
				SourceData = obj
			});
			targetRes.Compile();
			targetRes.Save(targetPath);
		}
	}
}
