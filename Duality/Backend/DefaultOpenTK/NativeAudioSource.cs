using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using OpenTK.Audio.OpenAL;

using Duality.Audio;

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class NativeAudioSource : INativeAudioSource
	{
		[FlagsAttribute]
		private enum DirtyFlag : uint
		{
			None		= 0x00000000,

			Pos			= 0x00000001,
			Vel			= 0x00000002,
			Pitch		= 0x00000004,
			Loop		= 0x00000008,
			MaxDist		= 0x00000010,
			RefDist		= 0x00000020,
			Relative	= 0x00000040,
			Vol			= 0x00000080,
			Paused		= 0x00000100,

			AttachedTo	= Pos | Vel,

			All			= 0xFFFFFFFF
		}
		private enum StopRequest
		{
			None,
			EndOfStream,
			Immediately
		}

		private int			handle		= 0;
		private	bool		isStreamed	= false;
		private	DirtyFlag	dirtyState	= DirtyFlag.All;

		private	object		strLock		= new object();
		private	StopRequest	strStopReq	= StopRequest.None;

		public int Handle
		{
			get { return this.handle; }
		}

		public NativeAudioSource(int handle)
		{
			this.handle = handle;
		}
		void INativeAudioSource.Stop()
		{
			AL.SourceStop(this.handle);
			this.strStopReq = StopRequest.Immediately;
			// The next update will handle everything else
		}
		void INativeAudioSource.Reset()
		{
			this.ResetSourceState();
		}
		void IDisposable.Dispose()
		{
			if (this.handle != 0)
			{
				this.ResetSourceState();
				if (AudioBackend.ActiveInstance != null)
					AudioBackend.ActiveInstance.FreeSourceHandle(this.handle);
				this.handle = 0;
			}
		}

		private void ResetSourceState()
		{
			lock (this.strLock)
			{
				// Do not reuse before-streamed sources, since OpenAL doesn't seem to like that.
				if (this.isStreamed)
				{
					AL.DeleteSource(this.handle);
					this.handle = AL.GenSource();
				}
				// Reuse other OpenAL sources
				else
				{
					AL.SourceStop(this.handle);
					AL.Source(this.handle, ALSourcei.Buffer, 0);
					AL.SourceRewind(this.handle);
				}

				this.dirtyState = DirtyFlag.All;
			}
		}
	}
}
