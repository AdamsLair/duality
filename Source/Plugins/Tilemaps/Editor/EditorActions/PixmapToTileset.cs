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
	/// Context menu action that will create a new <see cref="Tileset"/> 
	/// based on the selected <see cref="Pixmap"/> resource.
	/// </summary>
	public class PixmapToTileset : EditorSingleAction<Pixmap>
	{
		public override string Name
		{
			get { return TilemapsRes.ActionName_CreateTileset; }
		}
		public override string Description
		{
			get { return TilemapsRes.ActionDesc_CreateTileset; }
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
