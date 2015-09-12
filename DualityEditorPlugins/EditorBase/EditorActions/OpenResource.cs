using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.AssetManagement;

namespace Duality.Editor.Plugins.Base.EditorActions
{
	public class OpenResource : EditorSingleAction<Resource>
	{
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_OpenResourceExternal; }
		}

		public override void Perform(Resource obj)
		{
			string[] exportedPaths = AssetManager.ExportAssets(obj);
			
			// If there is only a single source path, open the file right away
			if (exportedPaths.Length == 1)
			{
				System.Diagnostics.Process.Start(exportedPaths[0]);
			}
			// If there are multiple source paths, just open the base directory
			else
			{
				string mutualBaseDir = PathHelper.GetMutualBaseDirectory(exportedPaths);
				System.Diagnostics.Process.Start(mutualBaseDir);
			}
		}
		public override bool CanPerformOn(Resource obj)
		{
			if (!base.CanPerformOn(obj)) return false;
			
			string[] exportedPaths = AssetManager.SimulateExportAssets(obj);
			return exportedPaths.Length > 0;
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}
	}
}
