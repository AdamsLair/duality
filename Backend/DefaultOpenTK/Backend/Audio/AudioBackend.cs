using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

using Duality.Audio;

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class AudioBackend : IAudioBackend
	{
		private static AudioBackend activeInstance = null;
		public static AudioBackend ActiveInstance
		{
			get { return activeInstance; }
		}


		private	AudioContext	context			= null;
		private	Stack<int>		sourcePool		= new Stack<int>();
		private	int				availSources	= 0;

		private	Thread					streamWorker			= null;
		private List<NativeAudioSource>	streamWorkerQueue		= null;
		private AutoResetEvent			streamWorkerQueueEvent	= null;
		private bool					streamWorkerEnd			= false;


		int IAudioBackend.AvailableSources
		{
			get { return this.sourcePool.Count; }
		}
		int IAudioBackend.MaxSourceCount
		{
			get { return this.availSources; }
		}
		string IDualityBackend.Id
		{
			get { return "DefaultOpenTKAudioBackend"; }
		}
		string IDualityBackend.Name
		{
			get { return "OpenAL (OpenTK)"; }
		}
		int IDualityBackend.Priority
		{
			get { return 0; }
		}


		bool IDualityBackend.CheckAvailable()
		{
			return true;
		}
		void IDualityBackend.Init()
		{
			AudioLibraryLoader.LoadAudioLibrary();
			
			Log.Core.Write("Available devices:" + Environment.NewLine + "{0}", 
				AudioContext.AvailableDevices.ToString(d => d == AudioContext.DefaultDevice ? d + " (Default)" : d, "," + Environment.NewLine));

			// Create OpenAL audio context
			this.context = new AudioContext();
			Log.Core.Write("Current device: {0}", this.context.CurrentDevice);

			// Generate OpenAL source pool
			for (int i = 0; i < 256; i++)
			{
				int newSrc = AL.GenSource();
				if (!Backend.DefaultOpenTK.AudioBackend.CheckOpenALErrors(true))
					this.sourcePool.Push(newSrc);
				else
					break;
			}
			this.availSources = this.sourcePool.Count;
			Log.Core.Write("{0} sources available", this.sourcePool.Count);
			
			// Set up the streaming thread
			this.streamWorkerEnd = false;
			this.streamWorkerQueue = new List<NativeAudioSource>();
			this.streamWorkerQueueEvent = new AutoResetEvent(false);
			this.streamWorker = new Thread(ThreadStreamFunc);
			this.streamWorker.IsBackground = true;
			this.streamWorker.Start();

			activeInstance = this;
		}
		void IDualityBackend.Shutdown()
		{
			// Shut down the streaming thread
			if (this.streamWorker != null)
			{
				this.streamWorkerEnd = true;
				if (!this.streamWorker.Join(1000))
				{
					this.streamWorker.Abort();
				}
				this.streamWorkerQueueEvent.Dispose();
				this.streamWorkerEnd = false;
				this.streamWorkerQueueEvent = null;
				this.streamWorkerQueue = null;
				this.streamWorker = null;
			}

			if (activeInstance == this)
				activeInstance = null;

			// Clear OpenAL source pool
			foreach (int alSource in this.sourcePool)
			{
				AL.DeleteSource(alSource);
			}

			// Shut down OpenAL context
			if (this.context != null)
			{
				this.context.Dispose();
				this.context = null;
			}

			AudioLibraryLoader.UnloadAudioLibrary();
		}

		void IAudioBackend.UpdateWorldSettings(float speedOfSound, float dopplerFactor)
		{
			AL.DistanceModel(ALDistanceModel.LinearDistanceClamped);
			AL.DopplerFactor(dopplerFactor);
			AL.SpeedOfSound(speedOfSound);
		}
		void IAudioBackend.UpdateListener(Vector3 position, Vector3 velocity, float angle, bool mute)
		{
			float[] orientation = new float[6];
			orientation[0] = 0.0f;	// forward vector x value
			orientation[1] = 0.0f;	// forward vector y value
			orientation[2] = -1.0f;	// forward vector z value
			orientation[5] = 0.0f;	// up vector z value
			AL.Listener(ALListener3f.Position, position.X, -position.Y, -position.Z);
			AL.Listener(ALListener3f.Velocity, velocity.X, -velocity.Y, -velocity.Z);
			orientation[3] = MathF.Sin(angle);	// up vector x value
			orientation[4] = MathF.Cos(angle);	// up vector y value
			AL.Listener(ALListenerfv.Orientation, ref orientation);
			AL.Listener(ALListenerf.Gain, mute ? 0.0f : 1.0f);
		}

		INativeAudioBuffer IAudioBackend.CreateBuffer()
		{
			return new NativeAudioBuffer();
		}
		INativeAudioSource IAudioBackend.CreateSource()
		{
			if (this.sourcePool.Count == 0)
				return null;
			else
				return new NativeAudioSource(this.sourcePool.Pop());
		}
		
		internal void FreeSourceHandle(int handle)
		{
			this.sourcePool.Push(handle);
		}
		internal void EnqueueForStreaming(NativeAudioSource source)
		{
			lock (this.streamWorkerQueue)
			{
				if (this.streamWorkerQueue.Contains(source)) return;
				this.streamWorkerQueue.Add(source);
			}
			this.streamWorkerQueueEvent.Set();
		}

		private void ThreadStreamFunc()
		{
			int queueIndex = 0;
			Stopwatch watch = new Stopwatch();
			watch.Restart();
			while (!this.streamWorkerEnd)
			{
				// Determine which audio source to update
				NativeAudioSource source;
				lock (this.streamWorkerQueue)
				{
					if (this.streamWorkerQueue.Count > 0)
					{
						int count = this.streamWorkerQueue.Count;
						queueIndex = (queueIndex + count) % count;
						source = this.streamWorkerQueue[queueIndex];
						queueIndex = (queueIndex + count - 1) % count;
					}
					else
					{
						source = null;
						queueIndex = 0;
					}
				}

				// If there is no audio source available, wait for a signal of one being added.
				if (source == null)
				{
					// Timeout of 100 ms to check regularly for requesting the thread to end.
					streamWorkerQueueEvent.WaitOne(100);
					continue;
				}

				// Perform the necessary streaming operations on the audio source, and remove it when requested
				if (!source.PerformStreaming())
				{
					lock (this.streamWorkerQueue)
					{
						this.streamWorkerQueue.Remove(source);
					}
				}

				// After each roundtrip, sleep a little, don't keep the processor busy for no reason
				if (queueIndex == 0)
				{
					watch.Stop();
					int roundtripTime = (int)watch.ElapsedMilliseconds;
					if (roundtripTime <= 1)
					{
						streamWorkerQueueEvent.WaitOne(16);
					}
					watch.Restart();
				}
			}
		}

		public static bool CheckOpenALErrors(bool silent = false)
		{
			ALError error;
			bool found = false;
			while ((error = AL.GetError()) != ALError.NoError)
			{
				if (!silent)
				{
					Log.Core.WriteError(
						"Internal OpenAL error, code {0} at {1}", 
						error,
						Log.CurrentMethod(1));
				}
				found = true;
				if (!silent && System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
			}
			return found;
		}
	}
}
