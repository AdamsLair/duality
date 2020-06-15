using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality.Resources;
using Duality.Properties;
using Duality.Editor.Plugins.Base.Properties;

namespace Duality.Editor.Plugins.Base.EditorActions.FirstSession
{
	/// <summary>
	/// Need help? Ask a fellow user!
	/// </summary>
	public class VisitCommunity : EditorAction<object>
	{
		public override Image Icon
		{
			get { return EditorBaseResCache.IconCommunity; }
		}
		public override string Name
		{
			get { return EditorBaseRes.ActionName_VisitCommunity; }
		}

		public override void Perform(IEnumerable<object> objEnum)
		{
			System.Diagnostics.Process.Start("https://chat.duality2d.net");
		}
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextFirstSession;
		}
	}
}
