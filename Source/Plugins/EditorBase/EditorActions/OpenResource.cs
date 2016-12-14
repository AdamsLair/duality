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
	/// <summary>
	/// Open Resource in external editor.
	/// </summary>
	public class OpenResource : EditorSingleAction<Resource>
	{
		public override void Perform(Resource obj)
		{
			// If we still have the original source files around, prefer them.
			string[] importedPaths = AssetManager.GetAssetSourceFiles(obj);
			if (importedPaths.Length > 0 && importedPaths.All(p => File.Exists(p)))
			{
				this.OpenPaths(importedPaths);
				return;
			}

			// Otherwise, export the Resource and open the result.
			string[] exportedPaths = AssetManager.ExportAssets(obj);
			this.OpenPaths(exportedPaths);
		}
		public override bool CanPerformOn(Resource obj)
		{
			if (!base.CanPerformOn(obj)) return false;

			// If we still have the original source files around, prefer them.
			string[] importedPaths = AssetManager.GetAssetSourceFiles(obj);
			if (importedPaths.Length > 0 && importedPaths.All(p => File.Exists(p)))
				return true;
			
			// Otherwise, see if we can export the resource for opening it.
			string[] exportedPaths = AssetManager.SimulateExportAssets(obj);
			return exportedPaths.Length > 0;
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextOpenRes;
		}

		private void OpenPaths(string[] paths)
		{
			// If there is only a single source path, open the file right away
			if (paths.Length == 1)
			{
				System.Diagnostics.Process.Start(paths[0]);
			}
			// If there are multiple source paths, just open the base directory
			else
			{
				string mutualBaseDir = PathHelper.GetMutualBaseDirectory(paths);
				System.Diagnostics.Process.Start(mutualBaseDir);
			}
		}
	}
}
