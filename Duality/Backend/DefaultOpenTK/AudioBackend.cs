using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

		public int MaxSourceCount
		{
			get { return this.availSources; }
		}
		public int AvailableSources
		{
			get { return this.sourcePool.Count; }
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

		internal void FreeSourceHandle(int handle)
		{
			this.sourcePool.Push(handle);
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

			activeInstance = this;
		}
		void IDualityBackend.Shutdown()
		{
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
