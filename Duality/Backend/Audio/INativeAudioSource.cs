using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Backend
{
	public interface INativeAudioSource : IDisposable
	{
		/// <summary>
		/// Stops the audio source, if it was playing.
		/// </summary>
		void Stop();
		/// <summary>
		/// Resets the entire state of the source, so it can be reused as if it was just created.
		/// </summary>
		void Reset();
	}
}
