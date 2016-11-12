﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Backend
{
	public interface IAudioBackend : IDualityBackend
	{
		int AvailableSources { get; }
		int MaxSourceCount { get; }

		void UpdateWorldSettings(float speedOfSound, float dopplerFactor);
		void UpdateListener(Vector3 position, Vector3 velocity, float angle, bool mute);

		INativeAudioBuffer CreateBuffer();
		INativeAudioSource CreateSource();
	}
}
