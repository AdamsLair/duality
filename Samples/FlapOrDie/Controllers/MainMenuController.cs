using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Input;
using Duality.Components.Physics;
using Duality.Components;
using Duality.Resources;

namespace FlapOrDie.Controllers
{
	[RequiredComponent(typeof(Camera))]
	public class MainMenuController : Component, ICmpUpdatable
	{
		public ContentRef<Scene> GameScene { get; set; }

		void ICmpUpdatable.OnUpdate()
		{
			if (DualityApp.Keyboard.KeyHit(Key.Escape))
			{
				DualityApp.Terminate();
			}
			if (DualityApp.Keyboard.KeyHit(Key.Space))
			{
				//preloading materials and sounds
				foreach (ContentRef<Material> m in ContentProvider.GetAvailableContent<Material>())
				{
					m.EnsureLoaded();
				}
				foreach (ContentRef<Sound> s in ContentProvider.GetAvailableContent<Sound>())
				{
					s.EnsureLoaded();
				}

				GameScene.Res.FindComponent<GameController>().Reset();

				Scene.SwitchTo(GameScene);
			}
		}
	}
}
