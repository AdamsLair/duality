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
	public class OpenScene : EditorSingleAction<Scene>
	{
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_OpenScene; }
		}

		public override void Perform(Scene scene)
		{
			string lastPath = Scene.CurrentPath;
			try
			{
				Scene.SwitchTo(scene);
			}
			catch (Exception exception)
			{
				Logs.Editor.WriteError("An error occurred while switching from Scene {1} to Scene {2}: {0}", 
					LogFormat.Exception(exception),
					lastPath,
					scene != null ? scene.Path : "null");
			}
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
}
