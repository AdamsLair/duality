using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Backend
{
	public interface INativeAudioSource : IDisposable
	{
		/// <summary>
		/// [GET] Whether the audio source is in its initial state, i.e. ready to be used by a new sound instance.
		/// </summary>
		bool IsInitial { get; }
		/// <summary>
		/// [GET] Whether the audio source is completely finished with playing audio.
		/// </summary>
		bool IsFinished { get; }

		/// <summary>
		/// Plays the audio source using a single, static buffer.
		/// </summary>
		void Play(INativeAudioBuffer buffer);
		/// <summary>
		/// Plays the audio source using a streaming input.
		/// </summary>
		void Play(IAudioStreamProvider streamingProvider);
		/// <summary>
		/// Stops the audio source, if it was playing.
		/// </summary>
		void Stop();
		/// <summary>
		/// Resets the entire state of the source, so it can be reused as if it was just created.
		/// </summary>
		void Reset();
		/// <summary>
		/// Updates the audio sources settings according to the specified data struct. It may modify
		/// the struct in order to reflect the actual settings, as supported by the backend.
		/// </summary>
		/// <param name="state"></param>
		void ApplyState(ref AudioSourceState state);
	}
}
