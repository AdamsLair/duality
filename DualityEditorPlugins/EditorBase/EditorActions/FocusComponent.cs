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
	public class FocusComponent : EditorSingleAction<Component>
	{
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_FocusGameObject; }
		}

		public override bool CanPerformOn(Component obj)
		{
			return base.CanPerformOn(obj) && obj.GameObj != null && obj.GameObj.Transform != null;
		}
		public override void Perform(Component obj)
		{
			if (obj.GameObj == null) return;
			if (obj.GameObj.Transform == null) return;
			DualityEditorApp.Highlight(this, new ObjectSelection(obj), HighlightMode.Spatial);
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
}
