using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.Base.Properties;

namespace Duality.Editor.Plugins.Base.EditorActions.FirstSession
{
	public class ViewGettingStartedTutorial : EditorAction<object>
	{
		public override Image Icon
		{
			get { return EditorBaseResCache.IconTutorial; }
		}
		public override string Name
		{
			get { return EditorBaseRes.ActionName_ViewGettingStartedTutorial; }
		}
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_ViewGettingStartedTutorial; }
		}

		public override void Perform(IEnumerable<object> objEnum)
		{
			System.Diagnostics.Process.Start("http://dualitytutorial.adamslair.net");
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextFirstSession;
		}
	}
}
