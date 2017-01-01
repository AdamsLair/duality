﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

using Duality.IO;
using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.EditorActions
{
	/// <summary>
	/// Creates a new Texture Resource based on the Pixmap.
	/// </summary>
	public class PixmapToTexture : EditorSingleAction<Pixmap>
	{
		public override string Name
		{
			get { return EditorBaseRes.ActionName_CreateTexture; }
		}
		public override Image Icon
		{
			get { return typeof(Texture).GetEditorImage(); }
		}

		public override void Perform(Pixmap obj)
		{
			string texPath = PathHelper.GetFreePath(obj.FullName, Resource.GetFileExtByType<Texture>());
			Texture tex = new Texture(obj);
			tex.Save(texPath);
		}
	}
}
