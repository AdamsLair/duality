using System;
using System.IO;
using System.Collections.Generic;

namespace Duality.Backend
{
	public struct AudioSourceState
	{
		public static readonly AudioSourceState Default = new AudioSourceState
		{
			Position = Vector3.Zero,
			Velocity = Vector3.Zero,
			Volume = 1.0f,
			Pitch = 1.0f,
			MinDistance = 0.0f,
			MaxDistance = 10000.0f,
			Looped = false,
			Paused = false,
			RelativeToListener = false
		};

		public Vector3 Position;
		public Vector3 Velocity;
		public float Volume;
		public float Pitch;
		public float MinDistance;
		public float MaxDistance;
		public bool Looped;
		public bool Paused;
		public bool RelativeToListener;
	}
}
