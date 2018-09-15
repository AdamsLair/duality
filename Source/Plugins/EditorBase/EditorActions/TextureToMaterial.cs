using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

using Duality.IO;
using Duality.Drawing;
using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.EditorActions
{
	/// <summary>
	/// Creates a new Material Resource based on the Texture.
	/// </summary>
	public class TextureToMaterial : EditorSingleAction<Texture>
	{
		public override string Name
		{
			get { return EditorBaseRes.ActionName_CreateMaterial; }
		}
		public override Image Icon
		{
			get { return typeof(Material).GetEditorImage(); }
		}

		public override void Perform(Texture obj)
		{
			string resPath = PathHelper.GetFreePath(obj.FullName, Resource.GetFileExtByType<Material>());
			Material res = new Material(DrawTechnique.Mask, obj);
			res.Save(resPath);
		}
	}
}
