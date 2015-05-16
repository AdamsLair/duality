using System;
using System.Reflection;
using System.Linq;

using Duality;
using Duality.Resources;

namespace Duality.Backend.DefaultOpenTK
{
    public class DefaultOpenTKBackendPlugin : CorePlugin
	{
		protected override void InitPlugin()
		{
			base.InitPlugin();

			// ToDo: Move mouse and keyboard input here as soon as the horrible hack is no longer necessary

			GlobalGamepadInputSource.UpdateAvailableDecives(DualityApp.Gamepads);
			GlobalJoystickInputSource.UpdateAvailableDecives(DualityApp.Joysticks);
		}
	}
}
