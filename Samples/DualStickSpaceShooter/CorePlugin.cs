using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;

namespace DualStickSpaceShooter
{
    public class DualStickSpaceShooterCorePlugin : CorePlugin
	{
		private bool contentLoaded = false;
		protected override void OnBeforeUpdate()
		{
			base.OnBeforeUpdate();

			// Load all available content so we don't need on-demand loading during runtime.
			// It's probably not a good idea for content-rich games, consider having a per-level
			// loading screen instead, or something similar.
			if (!this.contentLoaded && DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				Log.Game.Write("Loading game content...");
				Log.Game.PushIndent();
				{
					List<ContentRef<Resource>> availableContent = ContentProvider.GetAvailableContent<Resource>();
					foreach (ContentRef<Resource> resourceReference in availableContent)
					{
						resourceReference.MakeAvailable();
					}
				}
				Log.Game.PopIndent();
				this.contentLoaded = true;
			}
		}
	}
}
