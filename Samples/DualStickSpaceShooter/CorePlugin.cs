using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;

namespace DualStickSpaceShooter
{
    public class DualStickSpaceShooterCorePlugin : CorePlugin
	{
		protected override void OnGameStarting()
		{
			base.OnGameStarting();

			// Load all available content so we don't need on-demand loading at runtime.
			// It's probably not a good idea for content-rich games, consider having a per-level
			// loading screen instead, or something similar.
				Logs.Game.Write("Loading game content...");
				Logs.Game.PushIndent();
				{
					List<ContentRef<Resource>> availableContent = ContentProvider.GetAvailableContent<Resource>();
					foreach (ContentRef<Resource> resourceReference in availableContent)
					{
						resourceReference.EnsureLoaded();
					}
				}
				Logs.Game.PopIndent();
			}
		}
}
