using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK;
using AudioContext = OpenTK.Audio.AudioContext;
using OpenTK.Audio.OpenAL;

using Duality.Resources;
using Duality.Components;
using Duality.Profiling;

namespace Duality
{
	/// <summary>
	/// Provides functionality to play and manage sound in Duality.
	/// </summary>
	public sealed class SoundDevice : IDisposable
	{
		private	bool					disposed		= false;
		private	AudioContext			context			= null;
		private	GameObject				soundListener	= null;
		private	Stack<int>				alSourcePool	= new Stack<int>();
		private	List<SoundInstance>		sounds			= new List<SoundInstance>();
		private	SoundBudgetQueue		budgetMusic		= new SoundBudgetQueue();
		private	SoundBudgetQueue		budgetAmbient	= new SoundBudgetQueue();
		private	Dictionary<string,int>	resPlaying		= new Dictionary<string,int>();
		private	int						maxAlSources	= 0;
		private	int						numPlaying2D	= 0;
		private	int						numPlaying3D	= 0;
		private	bool					mute			= false;


		/// <summary>
		/// [GET] A queue of currently playing ambient pads.
		/// </summary>
		public SoundBudgetQueue Ambient
		{
			get { return this.budgetAmbient; }
		}
		/// <summary>
		/// [GET] A queue of currently playing music pads.
		/// </summary>
		public SoundBudgetQueue Music
		{
			get { return this.budgetMusic; }
		}
		/// <summary>
		/// [GET] Returns whether the SoundDevice is available. If false, no audio output can be generated.
		/// </summary>
		public bool IsAvailable
		{
			get { return this.context != null; }
		}

		/// <summary>
		/// [GET / SET] The current listener object. This is automatically set to an available
		/// <see cref="Duality.Components.SoundListener"/>.
		/// </summary>
		public GameObject Listener
		{
			get { return this.soundListener; }
			set { this.soundListener = value; }
		}
		/// <summary>
		/// [GET] The current listeners position.
		/// </summary>
		public Vector3 ListenerPos
		{
			get { return (this.soundListener != null && this.soundListener.Transform != null) ? this.soundListener.Transform.Pos : Vector3.Zero; }
		}
		/// <summary>
		/// [GET] The current listeners velocity.
		/// </summary>
		public Vector3 ListenerVel
		{
			get { return (this.soundListener != null && this.soundListener.Transform != null) ? this.soundListener.Transform.Vel : Vector3.Zero; }
		}
		/// <summary>
		/// [GET] The current listeners rotation / angle in radians.
		/// </summary>
		public float ListenerAngle
		{
			get { return (this.soundListener != null && this.soundListener.Transform != null) ? this.soundListener.Transform.Angle : 0.0f; }
		}
		
		/// <summary>
		/// [GET / SET] Whether all Duality audio is currently muted completely.
		/// </summary>
		public bool Mute
		{
			get { return this.mute; }
			set { this.mute = value; }
		}
		/// <summary>
		/// [GET] Returns a <see cref="Duality.Resources.Sound">Sounds</see> default minimum distance.
		/// </summary>
		public float DefaultMinDist
		{
			get { return 350.0f; }
		}
		/// <summary>
		/// [GET] Returns a <see cref="Duality.Resources.Sound">Sounds</see> default maximum distance.
		/// </summary>
		public float DefaultMaxDist
		{
			get { return 3500.0f; }
		}
		/// <summary>
		/// [GET] Returns the maximum number of available OpenAL sound sources.
		/// </summary>
		public int MaxOpenALSources
		{
			get { return this.maxAlSources; }
		}
		/// <summary>
		/// [GET] Returns the number of currently playing 2d sounds.
		/// </summary>
		public int NumPlaying2D
		{
			get { return this.numPlaying2D; }
		}
		/// <summary>
		/// [GET] Returns the number of currently playing 3d sounds.
		/// </summary>
		public int NumPlaying3D
		{
			get { return this.numPlaying3D; }
		}
		/// <summary>
		/// [GET] Returns the number of currently available OpenAL sound sources.
		/// </summary>
		public int NumAvailable
		{
			get { return this.alSourcePool.Count; }
		}
		/// <summary>
		/// [GET] Enumerates all currently playing SoundInstances.
		/// </summary>
		public IEnumerable<SoundInstance> Playing
		{
			get { return this.sounds; }
		}

		public SoundDevice()
		{
			Log.Core.Write("Initializing OpenAL...");
			Log.Core.PushIndent();

			try
			{
				AudioLibraryLoader.LoadAudioLibrary();

				Log.Core.Write("Available devices:" + Environment.NewLine + "{0}", 
					AudioContext.AvailableDevices.ToString(d => d == AudioContext.DefaultDevice ? d + " (Default)" : d, "," + Environment.NewLine));

				// Create OpenAL audio context
				this.context = new AudioContext();
				Log.Core.Write("Current device: {0}", this.context.CurrentDevice);

				// Generate OpenAL source pool
				while (true)
				{
					int newSrc = AL.GenSource();
					if (!DualityApp.CheckOpenALErrors(true))
						this.alSourcePool.Push(newSrc);
					else
						break;
				}
				this.maxAlSources = this.alSourcePool.Count;
				Log.Core.Write("{0} sources available", this.alSourcePool.Count);
			}
			catch (Exception e)
			{
				Log.Core.WriteError("An error occured while initializing OpenAL: {0}", Log.Exception(e));
			}

			Log.Core.PopIndent();

			DualityApp.AppDataChanged += this.DualityApp_AppDataChanged;
		}
		~SoundDevice()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool manually)
		{
			if (!this.disposed)
			{
				this.disposed = true;
				DualityApp.AppDataChanged -= this.DualityApp_AppDataChanged;
				
				foreach (SoundInstance inst in this.sounds) inst.Dispose();
				this.sounds.Clear();

				ContentProvider.RemoveAllContent<Sound>();

				try
				{
					if (this.context != null)
					{
						this.context.Dispose();
						this.context = null;
					}

					AudioLibraryLoader.UnloadAudioLibrary();
				}
				catch (Exception e)
				{
					Log.Core.WriteError("An error occured while shutting down OpenAL: {0}", Log.Exception(e));
				}
			}
		}

		/// <summary>
		/// Determines the number of playing instances of a specific <see cref="Duality.Resources.Sound"/>.
		/// </summary>
		/// <param name="snd">The Sound of which to determine the number of playing instances.</param>
		/// <returns>The number of the specified Sounds playing instances.</returns>
		public int GetNumPlaying(ContentRef<Sound> snd)
		{
			int curNumSoundRes;
			if (!snd.IsAvailable || snd.IsRuntimeResource || !this.resPlaying.TryGetValue(snd.Path, out curNumSoundRes))
				return 0;
			else
				return curNumSoundRes;
		}
		/// <summary>
		/// Requests an OpenAL source handle.
		/// </summary>
		/// <returns>An OpenAL source handle. <see cref="SoundInstance.AlSource_NotAvailable"/> if no source is currently available.</returns>
		public int RequestAlSource()
		{
			if (this.alSourcePool.Count == 0) return SoundInstance.AlSource_NotAvailable;
			return this.alSourcePool.Pop();
		}
		/// <summary>
		/// Registers a <see cref="Duality.Resources.Sound">Sounds</see> playing instance.
		/// </summary>
		/// <param name="snd">The Sound that is playing.</param>
		/// <param name="is3D">Whether the instance is 3d or not.</param>
		public void RegisterPlaying(ContentRef<Sound> snd, bool is3D)
		{
			if (is3D)	this.numPlaying3D++;
			else		this.numPlaying2D++;

			if (snd.IsAvailable && !snd.IsRuntimeResource)
			{
				if (!this.resPlaying.ContainsKey(snd.Path))
					this.resPlaying.Add(snd.Path, 1);
				else
					this.resPlaying[snd.Path]++;
			}
		}
		/// <summary>
		/// Frees a previously requested OpenAL source.
		/// </summary>
		/// <param name="alSource">The OpenAL handle of the source to free.</param>
		public void FreeAlSource(int alSource)
		{
			this.alSourcePool.Push(alSource);
		}
		/// <summary>
		/// Unregisters a <see cref="Duality.Resources.Sound">Sounds</see> playing instance.
		/// </summary>
		/// <param name="snd">The Sound that was playing.</param>
		/// <param name="is3D">Whether the instance is 3d or not.</param>
		public void UnregisterPlaying(ContentRef<Sound> snd, bool is3D)
		{
			if (is3D)	this.numPlaying3D--;
			else		this.numPlaying2D--;

			if (snd.IsAvailable && !snd.IsRuntimeResource)
				this.resPlaying[snd.Path]--;
		}
		
		/// <summary>
		/// Updates the SoundDevice.
		/// </summary>
		public void Update()
		{
			if (this.context == null) return;
			Profile.TimeUpdateAudio.BeginMeasure();

			this.budgetAmbient.Update();
			this.budgetMusic.Update();

			this.UpdateListener();

			for (int i = this.sounds.Count - 1; i >= 0; i--)
			{
				this.sounds[i].Update();
				if (this.sounds[i].Disposed) this.sounds.RemoveAt(i);
			}
			this.sounds.Sort((obj1, obj2) => obj2.Priority - obj1.Priority);

			Profile.TimeUpdateAudio.EndMeasure();
			Profile.StatNumPlaying2D.Add(this.numPlaying2D);
			Profile.StatNumPlaying3D.Add(this.numPlaying3D);
		}
		private void UpdateListener()
		{
			if (this.context == null) return;
			if (this.soundListener != null && (this.soundListener.Disposed || !this.soundListener.Active)) this.soundListener = null;

			// If no listener is defined, search one
			if (this.soundListener == null)
			{
				this.soundListener = Scene.Current.FindGameObject<SoundListener>();
			}

			float[] orientation = new float[6];
			orientation[0] = 0.0f;	// forward vector x value
			orientation[1] = 0.0f;	// forward vector y value
			orientation[2] = -1.0f;	// forward vector z value
			orientation[5] = 0.0f;	// up vector z value
			Vector3 listenerPos = this.ListenerPos;
			Vector3 listenerVel = this.ListenerVel;
			float listenerAngle = this.ListenerAngle;
			AL.Listener(ALListener3f.Position, listenerPos.X, -listenerPos.Y, -listenerPos.Z);
			AL.Listener(ALListener3f.Velocity, listenerVel.X, -listenerVel.Y, -listenerVel.Z);
			orientation[3] = MathF.Sin(listenerAngle);	// up vector x value
			orientation[4] = MathF.Cos(listenerAngle);	// up vector y value
			AL.Listener(ALListenerfv.Orientation, ref orientation);
			AL.Listener(ALListenerf.Gain, this.mute ? 0.0f : 1.0f);
		}
		
		/// <summary>
		/// Plays a sound.
		/// </summary>
		/// <param name="snd">The Sound to play.</param>
		/// <returns>A new SoundInstance representing the currentply playing sound.</returns>
		public SoundInstance PlaySound(ContentRef<Sound> snd)
		{
			SoundInstance inst = new SoundInstance(snd);
			this.sounds.Add(inst);
			return inst;
		}
		/// <summary>
		/// Plays a sound 3d "in space".
		/// </summary>
		/// <param name="snd">The Sound to play.</param>
		/// <param name="pos">The position of the sound in space.</param>
		/// <returns>A new SoundInstance representing the currentply playing sound.</returns>
		public SoundInstance PlaySound3D(ContentRef<Sound> snd, Vector3 pos)
		{
			SoundInstance inst = new SoundInstance(snd, pos);
			this.sounds.Add(inst);
			return inst;
		}
		/// <summary>
		/// Plays a sound 3d "in space".
		/// </summary>
		/// <param name="snd">The Sound to play.</param>
		/// <param name="attachTo">The GameObject to which the sound will be attached.</param>
		/// <returns>A new SoundInstance representing the currentply playing sound.</returns>
		public SoundInstance PlaySound3D(ContentRef<Sound> snd, GameObject attachTo)
		{
			SoundInstance inst = new SoundInstance(snd, attachTo);
			this.sounds.Add(inst);
			return inst;
		}
		/// <summary>
		/// Plays a sound 3d "in space".
		/// </summary>
		/// <param name="snd">The Sound to play.</param>
		/// <param name="attachTo">The GameObject to which the sound will be attached.</param>
		/// <param name="relativePos">The position of the sound relative to the GameObject.</param>
		/// <returns>A new SoundInstance representing the currentply playing sound.</returns>
		public SoundInstance PlaySound3D(ContentRef<Sound> snd, GameObject attachTo, Vector3 relativePos)
		{
			SoundInstance inst = new SoundInstance(snd, attachTo);
			inst.Pos = relativePos;
			this.sounds.Add(inst);
			return inst;
		}
		
		private void DualityApp_AppDataChanged(object sender, EventArgs e)
		{
			if (this.context == null) return;
			AL.DistanceModel(ALDistanceModel.LinearDistanceClamped);
			AL.DopplerFactor(DualityApp.AppData.SoundDopplerFactor);
			AL.SpeedOfSound(DualityApp.AppData.SpeedOfSound);
		}
	}
}
