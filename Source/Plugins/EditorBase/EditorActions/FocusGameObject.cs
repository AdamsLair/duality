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
	/// <summary>
	/// Focus Camera on GameObject.
	/// </summary>
	public class FocusGameObject : EditorSingleAction<GameObject>
	{
		public override bool CanPerformOn(GameObject obj)
		{
			return base.CanPerformOn(obj) && obj.Transform != null;
		}
		public override void Perform(GameObject obj)
		{
			if (obj.Transform == null) return;
			DualityEditorApp.Highlight(this, new ObjectSelection(obj), HighlightMode.Spatial);
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
}
