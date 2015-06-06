using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.EditorActions
{
	public class PixmapToTexture : EditorSingleAction<Pixmap>
	{
		public override string Name
		{
			get { return EditorBaseRes.ActionName_CreateTexture; }
		}
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_CreateTexture; }
		}
		public override Image Icon
		{
			get { return typeof(Texture).GetEditorImage(); }
		}

		public override void Perform(Pixmap obj)
		{
			Texture.CreateFromPixmap(obj);
		}
	}
}
