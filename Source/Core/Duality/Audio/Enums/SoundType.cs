using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

using Duality.Resources;

namespace Duality.Audio
{
	/// <summary>
	/// Describes the type of a sound. This is used for determining which specific
	/// volume settings affect each sound.
	/// </summary>
	public enum SoundType
	{
		/// <summary>
		/// A sound effect taking place in the game world.
		/// </summary>
		World,
		/// <summary>
		/// A User Interface sound effect.
		/// </summary>
		UserInterface,
		/// <summary>
		/// A sound that is considered being game music.
		/// </summary>
		Music,
		/// <summary>
		/// A sound that is considered being spoken language.
		/// </summary>
		Speech
	}
}
