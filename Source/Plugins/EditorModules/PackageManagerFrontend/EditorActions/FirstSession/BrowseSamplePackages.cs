using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.PackageManagerFrontend.Properties;

namespace Duality.Editor.Plugins.PackageManagerFrontend.EditorActions.FirstSession
{
	/// <summary>
	/// Download example projects to see Duality in action.
	/// </summary>
	public class BrowseSamplePackages : EditorAction<object>
	{
		public override Image Icon
		{
			get { return PackageManagerFrontendResCache.IconSampleBig; }
		}
		public override string Name
		{
			get { return PackageManagerFrontendRes.ActionName_BrowseSamplePackages; }
		}

		public override void Perform(IEnumerable<object> objEnum)
		{
			PackageManagerFrontendPlugin plugin = DualityEditorApp.GetPlugin<PackageManagerFrontendPlugin>();
			plugin.ShowSamplePackageBrowser();
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextFirstSession;
		}
	}
}
