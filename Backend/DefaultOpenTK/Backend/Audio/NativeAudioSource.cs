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
		private enum StopRequest
		{
			None,
			EndOfStream,
			Immediately
		}

		private int                  handle         = 0;
		private int                  filterHandle   = 0;
		private	bool                 isInitial      = true;
		private	bool                 isStreamed     = false;
		private	bool                 isFirstUpdate  = true;
		private	AudioSourceState     lastState      = AudioSourceState.Default;
		private IAudioStreamProvider streamProvider = null;

		private object               strLock        = new object();
		private	StopRequest          strStopReq     = StopRequest.None;
		private	INativeAudioBuffer[] strAlBuffers   = null;


		public int Handle
		{
			get { return this.handle; }
		}
		bool INativeAudioSource.IsInitial
		{
			get { return this.isInitial; }
		}
		bool INativeAudioSource.IsFinished
		{
			get
			{
				if (this.isInitial) return false;
				lock (this.strLock)
				{
					ALSourceState stateTemp = AL.GetSourceState(this.handle);

					// Stopped and either not streamed or requesting to end.
					if (stateTemp == ALSourceState.Stopped && (!this.isStreamed || this.strStopReq != StopRequest.None))
						return true;
					// Not even started playing, but requested to end anyway.
					else if (stateTemp == ALSourceState.Initial && this.strStopReq == StopRequest.Immediately)
						return true;
					// Not finished yet.
					else
						return false;
				}
			}
		}

		public NativeAudioSource(int handle)
		{
			this.handle = handle;
		}

		void INativeAudioSource.Play(INativeAudioBuffer buffer)
		{
			lock (this.strLock)
			{
				if (!this.isInitial) throw new InvalidOperationException("Native audio source already in use. To re-use an audio source, reset it first.");
				this.isInitial = false;

				AL.SourceQueueBuffer(this.handle, (buffer as NativeAudioBuffer).Handle);
				AL.SourcePlay(handle);
			}
		}
		void INativeAudioSource.Play(IAudioStreamProvider streamingProvider)
		{
			lock (this.strLock)
			{
				if (!this.isInitial) throw new InvalidOperationException("Native audio source already in use. To re-use an audio source, reset it first.");
				this.isInitial = false;

				this.isStreamed = true;
				this.strStopReq = StopRequest.None;
				this.streamProvider = streamingProvider;
				AudioBackend.ActiveInstance.EnqueueForStreaming(this);
			}
		}
		void INativeAudioSource.Stop()
		{
			lock (this.strLock)
			{
				AL.SourceStop(this.handle);
				this.strStopReq = StopRequest.Immediately;
			}
		}
		void INativeAudioSource.Reset()
		{
			this.ResetLocalState();
			this.ResetSourceState();
			this.isInitial = true;
			this.isFirstUpdate = true;
		}
		void INativeAudioSource.ApplyState(ref AudioSourceState state)
		{
			lock (this.strLock)
			{
				ALSourceState nativeState = AL.GetSourceState(this.handle);
				bool looped = state.Looped && !this.isStreamed;

				if (this.isFirstUpdate || this.lastState.RelativeToListener != state.RelativeToListener)
					AL.Source(handle, ALSourceb.SourceRelative, state.RelativeToListener);
				if (this.isFirstUpdate || this.lastState.Position != state.Position)
					AL.Source(handle, ALSource3f.Position, state.Position.X, -state.Position.Y, -state.Position.Z * 0.5f);
				if (this.isFirstUpdate || this.lastState.Velocity != state.Velocity)
					AL.Source(handle, ALSource3f.Velocity, state.Velocity.X, -state.Velocity.Y, -state.Velocity.Z);
				if (this.isFirstUpdate || this.lastState.MaxDistance != state.MaxDistance)
					AL.Source(handle, ALSourcef.MaxDistance, state.MaxDistance);
				if (this.isFirstUpdate || this.lastState.MinDistance != state.MinDistance)
					AL.Source(handle, ALSourcef.ReferenceDistance, state.MinDistance);
				if (this.isFirstUpdate || this.lastState.Looped != looped)
					AL.Source(handle, ALSourceb.Looping, looped);
				if (this.isFirstUpdate || this.lastState.Volume != state.Volume)
					AL.Source(handle, ALSourcef.Gain, state.Volume);
				if (this.isFirstUpdate || this.lastState.Pitch != state.Pitch)
					AL.Source(handle, ALSourcef.Pitch, state.Pitch);

				// Update lowpass settings requires Effects extension
				if (this.isFirstUpdate || this.lastState.Lowpass != state.Lowpass)
				{
					var fx = AudioBackend.ActiveInstance.EffectsExtension;
					if (fx != null)
					{
						// If there is no filter, create one when required.
						if (this.filterHandle == 0 && state.Lowpass < 1.0f)
						{
							this.filterHandle = fx.GenFilter();
							fx.Filter(this.filterHandle, EfxFilteri.FilterType, (int)EfxFilterType.Lowpass);
							fx.Filter(this.filterHandle, EfxFilterf.LowpassGain, 1);
						}
						// If there is a filter, keep it up-to-date
						if (this.filterHandle != 0)
						{
							fx.Filter(this.filterHandle, EfxFilterf.LowpassGainHF, MathF.Clamp(state.Lowpass, 0.0f, 1.0f));
							fx.BindFilterToSource(this.handle, this.filterHandle);
						}
					}
				}

				if (state.Paused && nativeState == ALSourceState.Playing)
					AL.SourcePause(handle);
				else if (!state.Paused && nativeState == ALSourceState.Paused)
					AL.SourcePlay(handle);

				this.lastState = state;
				this.lastState.Looped = looped;
				this.isFirstUpdate = false;
			}
		}
		void IDisposable.Dispose()
		{
			lock (this.strLock)
			{
				if (this.isStreamed)
				{
					this.streamProvider.CloseStream();
					this.strStopReq = StopRequest.Immediately;
				}

				this.ResetSourceState();

				if (this.handle != 0)
				{
					if (AudioBackend.ActiveInstance != null)
						AudioBackend.ActiveInstance.FreeSourceHandle(this.handle);
					this.handle = 0;
				}

				this.ReleaseStreamBuffers();
			}
		}

		private void ReleaseStreamBuffers()
		{
			lock (this.strLock)
			{
				if (this.strAlBuffers != null)
				{
					for (int i = 0; i < this.strAlBuffers.Length; i++)
					{
						if (this.strAlBuffers[i] != null)
						{
							this.strAlBuffers[i].Dispose();
							this.strAlBuffers[i] = null;
						}
					}
					this.strAlBuffers = null;
				}
			}
		}
		private void ResetLocalState()
		{
			lock (this.strLock)
			{
				this.strStopReq = StopRequest.Immediately;
				this.isStreamed = false;
				this.streamProvider = null;

				this.ReleaseStreamBuffers();
			}
		}
		private void ResetSourceState()
		{
			lock (this.strLock)
			{
				// Release filters, if present
				if (this.filterHandle != 0)
				{
					var fx = AudioBackend.ActiveInstance.EffectsExtension;
					if (fx != null)
					{
						if (this.handle != 0)
						{
							fx.BindFilterToSource(this.handle, 0);
						}
						fx.DeleteFilter(this.filterHandle);
						this.filterHandle = 0;
					}
				}

				// Reset the internal OpenAL source, if present
				if (this.handle != 0)
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
				}
			}
		}

		internal bool PerformStreaming()
		{
			lock (this.strLock)
			{
				if (this.handle == 0) return false;

				ALSourceState stateTemp = AL.GetSourceState(this.handle);
				if (stateTemp == ALSourceState.Stopped && this.strStopReq != StopRequest.None)
				{
					// Stopped due to regular EOF. If strStopReq is NOT set,
					// the source stopped playing because it reached the end of the buffer
					// but in fact only because we were too slow inserting new data.
					return false;
				}
				else if (this.strStopReq == StopRequest.Immediately)
				{
					// Stopped intentionally due to Stop()
					AL.SourceStop(handle);
					return false;
				}

				if (stateTemp == ALSourceState.Initial)
				{
					// Initialize streaming
					PerformStreamingBegin();

					// Initially play source
					AL.SourcePlay(handle);
					stateTemp = AL.GetSourceState(handle);
				}
				else
				{
					// Stream new data
					PerformStreamingUpdate();

					// If the source stopped unintentionally, restart it. (See above)
					if (stateTemp == ALSourceState.Stopped && this.strStopReq == StopRequest.None)
					{
						AL.SourcePlay(handle);
					}
				}
			}

			return true;
		}
		private void PerformStreamingBegin()
		{
			// Generate streaming buffers
			this.strAlBuffers = new INativeAudioBuffer[3];
			for (int i = 0; i < this.strAlBuffers.Length; ++i)
			{
				this.strAlBuffers[i] = DualityApp.AudioBackend.CreateBuffer();
			}

			// Begin streaming
			this.streamProvider.OpenStream();

			// Initially, completely fill all buffers
			for (int i = 0; i < this.strAlBuffers.Length; ++i)
			{
				bool eof = !this.streamProvider.ReadStream(this.strAlBuffers[i]);
				if (!eof)
				{
					AL.SourceQueueBuffer(this.handle, (this.strAlBuffers[i] as NativeAudioBuffer).Handle);
				}
				else
				{
					break;
				}
			}
		}
		private void PerformStreamingUpdate()
		{
			int num;
			AL.GetSource(this.handle, ALGetSourcei.BuffersProcessed, out num);
			while (num > 0)
			{
				num--;

				int unqueuedBufferHandle = AL.SourceUnqueueBuffer(this.handle);
				INativeAudioBuffer unqueuedBuffer = null;
				for (int i = 0; i < this.strAlBuffers.Length; i++)
				{
					NativeAudioBuffer buffer = this.strAlBuffers[i] as NativeAudioBuffer;
					if (buffer.Handle == unqueuedBufferHandle)
					{
						unqueuedBuffer = buffer;
						break;
					}
				}

				bool eof = !this.streamProvider.ReadStream(unqueuedBuffer);
				if (!eof)
				{
					AL.SourceQueueBuffer(this.handle, unqueuedBufferHandle);
				}
				else
				{
					this.strStopReq = StopRequest.EndOfStream;
				}
			}
		}
	}
}
