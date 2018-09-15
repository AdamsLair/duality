using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Audio;

namespace Duality.Backend.Dummy
{
	public class DummyAudioBackend : IAudioBackend
	{
		private static DummyAudioBackend activeInstance = null;
		internal static DummyAudioBackend ActiveInstance
		{
			get { return activeInstance; }
		}

		private const int DummySourceCount = 64;
		private int sourceCounter = DummySourceCount;

		int IAudioBackend.AvailableSources
		{
			get { return this.sourceCounter; }
		}
		int IAudioBackend.MaxSourceCount
		{
			get { return DummySourceCount; }
		}
		string IDualityBackend.Id
		{
			get { return "DummyAudioBackend"; }
		}
		string IDualityBackend.Name
		{
			get { return "No Audio"; }
		}
		int IDualityBackend.Priority
		{
			get { return int.MinValue; }
		}

		bool IDualityBackend.CheckAvailable()
		{
			return true;
		}
		void IDualityBackend.Init()
		{
			Logs.Core.WriteWarning("DummyAudioBackend initialized. This is unusual and may cause problems when someone tries to play audio.");
			activeInstance = this;
		}
		void IDualityBackend.Shutdown()
		{
			if (activeInstance == this)
				activeInstance = null;
		}

		void IAudioBackend.UpdateWorldSettings(float speedOfSound, float dopplerFactor) { }
		void IAudioBackend.UpdateListener(Vector3 position, Vector3 velocity, float angle, bool mute) { }

		INativeAudioBuffer IAudioBackend.CreateBuffer()
		{
			return new DummyNativeAudioBuffer();
		}
		INativeAudioSource IAudioBackend.CreateSource()
		{
			this.sourceCounter--;
			return new DummyNativeAudioSource();
		}

		internal void NotifySourceRelease()
		{
			this.sourceCounter++;
		}
	}
}
