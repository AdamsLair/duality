using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	/// <summary>
	/// Specifies intervals and modes to refresh the screen and update the game.
	/// </summary>
	public enum RefreshMode
	{
		/// <summary>
		/// Refreshes occur as fast as possible with no wait inbetween.
		/// </summary>
		NoSync,
		/// <summary>
		/// Refreshes target 60 FPS and will use wait for each frame to use its entire available time.
		/// Doesn't use hardware / driver VSync, but prevents 100% CPU usage.
		/// </summary>
		ManualSync,
		/// <summary>
		/// Refreshes wait for the hardware / driver VSync.
		/// </summary>
		VSync,
		/// <summary>
		/// Refreshes wait for the hardware / driver VSync as long as the target framerate is reached. When
		/// falling below, VSync will be temporarily suspended.
		/// </summary>
		AdaptiveVSync
	}
}
