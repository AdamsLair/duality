using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Audio;

namespace Duality.Backend.Dummy
{
	public class DummyNativeAudioSource : INativeAudioSource
	{
		private bool initial = true;
		private bool finished = false;
		private bool disposed = false;

		bool INativeAudioSource.IsInitial
		{
			get { return this.initial; }
		}
		bool INativeAudioSource.IsFinished
		{
			get { return this.finished; }
		}

		void INativeAudioSource.Play(INativeAudioBuffer buffer)
		{
			this.initial = false;
		}
		void INativeAudioSource.Play(IAudioStreamProvider streamingProvider)
		{
			this.initial = false;
		}
		void INativeAudioSource.Stop()
		{
			this.finished = true;
		}
		void INativeAudioSource.Reset()
		{
			this.initial = true;
		}
		void INativeAudioSource.ApplyState(ref AudioSourceState state)
		{
			this.finished = true;
		}
		void IDisposable.Dispose()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.finished = true;
				DummyAudioBackend.ActiveInstance.NotifySourceRelease();
			}
		}
	}
}
