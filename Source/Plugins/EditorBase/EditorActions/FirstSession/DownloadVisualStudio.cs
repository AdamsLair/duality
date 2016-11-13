using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.Base.Properties;

namespace Duality.Editor.Plugins.Base.EditorActions.FirstSession
{
	public class DownloadVisualStudio : EditorAction<object>
	{
		public override Image Icon
		{
			get { return EditorBaseResCache.IconDownloadCodeIDE; }
		}
		public override string Name
		{
			get { return EditorBaseRes.ActionName_DownloadVisualStudio; }
		}
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_DownloadVisualStudio; }
		}

		public override void Perform(IEnumerable<object> objEnum)
		{
			System.Diagnostics.Process.Start("https://www.visualstudio.com/");
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextFirstSession;
		}
	}
}
