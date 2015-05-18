using System;
using System.Reflection;
using System.Linq;
using System.Threading;

using Duality;
using Duality.Resources;

namespace Duality.Backend.DefaultOpenTK
{
    public class DefaultOpenTKBackendPlugin : CorePlugin
	{
		private static Thread mainThread;

		protected override void InitPlugin()
		{
			base.InitPlugin();

			mainThread = Thread.CurrentThread;

			GlobalGamepadInputSource.UpdateAvailableDecives(DualityApp.Gamepads);
			GlobalJoystickInputSource.UpdateAvailableDecives(DualityApp.Joysticks);
		}
		
		/// <summary>
		/// Guards the calling method agains being called from a thread that is not the main thread.
		/// Use this only at critical code segments that are likely to be called from somewhere else than the main thread
		/// but aren't allowed to.
		/// </summary>
		/// <param name="backend"></param>
		/// <param name="silent"></param>
		/// <returns>True if everyhing is allright. False if the guarded state has been violated.</returns>
		[System.Diagnostics.DebuggerStepThrough]
		internal static bool GuardSingleThreadState(bool silent = false)
		{
			if (Thread.CurrentThread != mainThread)
			{
				if (!silent)
				{
					Log.Core.WriteError(
						"Method {0} isn't allowed to be called from a Thread that is not the main Thread.", 
						Log.CurrentMethod(1));
				}
				return false;
			}
			return true;
		}
	}
}
