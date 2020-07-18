﻿using System;
using System.Collections.Generic;

using Duality.Resources;
using Duality.Components;

namespace Duality.Audio
{
	/// <summary>
	/// Provides functionality to play and manage sound in Duality.
	/// </summary>
	[DontSerialize]
	public sealed class SoundDevice : IDisposable
	{
		private	bool					disposed		= false;
		private SoundListener			soundListener	= null;
		private	List<SoundInstance>		sounds			= new List<SoundInstance>();
		private	Dictionary<string,int>	resPlaying		= new Dictionary<string,int>();
		private	int						numPlaying2D	= 0;
		private	int						numPlaying3D	= 0;
		private	bool					mute			= false;

		/// <summary>
		/// [GET / SET] The current listener object. This is automatically set to an available
		/// <see cref="SoundListener"/>.
		/// </summary>
		public SoundListener Listener
		{
			get { return this.soundListener; }
			set { this.soundListener = value; }
		}
		/// <summary>
		/// [GET] The current listeners position.
		/// </summary>
		public Vector3 ListenerPos
		{
			get { return (this.soundListener != null) ? this.soundListener.Position : Vector3.Zero; }
		}
		/// <summary>
		/// [GET] The current listeners velocity.
		/// </summary>
		public Vector3 ListenerVel
		{
			get { return (this.soundListener != null) ? this.soundListener.Velocity : Vector3.Zero; }
		}
		/// <summary>
		/// [GET] The current listeners rotation / angle in radians.
		/// </summary>
		public float ListenerAngle
		{
			get { return (this.soundListener != null) ? this.soundListener.Angle : 0.0f; }
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
			get { return DualityApp.AudioBackend.MaxSourceCount; }
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
			get { return DualityApp.AudioBackend.AvailableSources; }
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
			DualityApp.AppData.Applying += this.OnAppDataApplying;
			this.UpdateWorldSettings();
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
				DualityApp.AppData.Applying -= this.OnAppDataApplying;

				// Clear all playing sounds
				foreach (SoundInstance inst in this.sounds) inst.Dispose();
				this.sounds.Clear();
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
		/// Registers a <see cref="Duality.Resources.Sound">Sounds</see> playing instance.
		/// </summary>
		/// <param name="snd">The Sound that is playing.</param>
		/// <param name="is3D">Whether the instance is 3d or not.</param>
		internal void RegisterPlaying(ContentRef<Sound> snd, bool is3D)
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
		/// Unregisters a <see cref="Duality.Resources.Sound">Sounds</see> playing instance.
		/// </summary>
		/// <param name="snd">The Sound that was playing.</param>
		/// <param name="is3D">Whether the instance is 3d or not.</param>
		internal void UnregisterPlaying(ContentRef<Sound> snd, bool is3D)
		{
			if (is3D)	this.numPlaying3D--;
			else		this.numPlaying2D--;

			if (snd.IsAvailable && !snd.IsRuntimeResource)
				this.resPlaying[snd.Path]--;
		}
		
		/// <summary>
		/// Updates the SoundDevice.
		/// </summary>
		internal void Update()
		{
			Profile.TimeUpdateAudio.BeginMeasure();

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
			if (this.soundListener != null && (this.soundListener.Disposed || !this.soundListener.Active)) this.soundListener = null;

			// If no listener is defined, search one
			if (this.soundListener == null)
			{
				this.soundListener = Scene.Current.FindComponent<SoundListener>();
			}

			DualityApp.AudioBackend.UpdateListener(
				this.ListenerPos * AudioUnit.LengthToPhysical,
				this.ListenerVel * AudioUnit.VelocityToPhysical,
				this.ListenerAngle * AudioUnit.AngleToPhysical,
				this.mute);
		}
		private void UpdateWorldSettings()
		{
			DualityApp.AudioBackend.UpdateWorldSettings(
				DualityApp.AppData.Instance.SpeedOfSound, // Already in meters per second / audio units
				DualityApp.AppData.Instance.SoundDopplerFactor);
		}
		
		/// <summary>
		/// Plays a sound in 2D.
		/// </summary>
		/// <param name="snd">The <see cref="Sound"/> to play.</param>
		/// <returns>A new <see cref="SoundInstance"/> representing the playing sound.</returns>
		public SoundInstance PlaySound(ContentRef<Sound> snd)
		{
			SoundInstance inst = new SoundInstance(snd);
			this.sounds.Add(inst);
			return inst;
		}
		/// <summary>
		/// Plays a sound in 3D, so it will be adjusted depending on its spatial relation to the <see cref="SoundListener"/>.
		/// </summary>
		/// <param name="snd">The <see cref="Sound"/> to play.</param>
		/// <param name="pos">The position of the sound in space.</param>
		/// <returns>A new <see cref="SoundInstance"/> representing the playing sound.</returns>
		public SoundInstance PlaySound3D(ContentRef<Sound> snd, Vector3 pos)
		{
			SoundInstance inst = new SoundInstance(snd, pos);
			this.sounds.Add(inst);
			return inst;
		}
		/// <summary>
		/// Plays a sound in 3D, so it will be adjusted depending on its spatial relation to the <see cref="SoundListener"/>.
		/// </summary>
		/// <param name="snd">The <see cref="Sound"/> to play.</param>
		/// <param name="attachTo">The GameObject to which the sound will be attached.</param>
		/// <param name="trackVelocity">Whether the attached objects movement will affect the sounds playback.</param>
		/// <returns>A new <see cref="SoundInstance"/> representing the playing sound.</returns>
		public SoundInstance PlaySound3D(ContentRef<Sound> snd, GameObject attachTo, bool trackVelocity)
		{
			SoundInstance inst = new SoundInstance(snd, attachTo, trackVelocity);
			this.sounds.Add(inst);
			return inst;
		}
		/// <summary>
		/// Plays a sound in 3D, so it will be adjusted depending on its spatial relation to the <see cref="SoundListener"/>.
		/// </summary>
		/// <param name="snd">The <see cref="Sound"/> to play.</param>
		/// <param name="attachTo">The GameObject to which the sound will be attached.</param>
		/// <param name="relativePos">The position of the sound relative to the GameObject.</param>
		/// <param name="trackVelocity">Whether the attached objects movement will affect the sounds playback.</param>
		/// <returns>A new <see cref="SoundInstance"/> representing the playing sound.</returns>
		public SoundInstance PlaySound3D(ContentRef<Sound> snd, GameObject attachTo, Vector3 relativePos, bool trackVelocity)
		{
			SoundInstance inst = new SoundInstance(snd, attachTo, trackVelocity);
			inst.Pos = relativePos;
			this.sounds.Add(inst);
			return inst;
		}
		/// <summary>
		/// Stops all currently playing sounds.
		/// </summary>
		public void StopAll()
		{
			for (int i = this.sounds.Count - 1; i >= 0; i--)
			{
				this.sounds[i].Stop();
			}
		}
		
		private void OnAppDataApplying(object sender, EventArgs e)
		{
			this.UpdateWorldSettings();
		}
	}
}
