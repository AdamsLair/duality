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
	/// Create a GameObject by instantiating the Prefab.
	/// </summary>
	public class InstantiatePrefab : EditorSingleAction<Prefab>
	{
		public override void Perform(Prefab prefab)
		{
			try
			{
				CreateGameObjectAction undoRedoAction = new CreateGameObjectAction(null, prefab.Instantiate());
				UndoRedoManager.Do(undoRedoAction);
				DualityEditorApp.Select(this, new ObjectSelection(undoRedoAction.Result));
			}
			catch (Exception exception)
			{
				Logs.Editor.WriteError("An error occurred instanciating Prefab {1}: {0}", 
					LogFormat.Exception(exception),
					prefab != null ? prefab.Path : "null");
			}
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
}
