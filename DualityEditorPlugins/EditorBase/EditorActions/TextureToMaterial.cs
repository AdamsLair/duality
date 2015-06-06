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
	public class TextureToMaterial : EditorSingleAction<Texture>
	{
		public override string Name
		{
			get { return EditorBaseRes.ActionName_CreateMaterial; }
		}
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_CreateMaterial; }
		}
		public override Image Icon
		{
			get { return typeof(Material).GetEditorImage(); }
		}

		public override void Perform(Texture obj)
		{
			Material.CreateFromTexture(obj);
		}
	}
}
