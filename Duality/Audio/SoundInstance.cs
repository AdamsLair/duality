using System;
using System.Threading;

using OpenTK;
using OpenTK.Audio.OpenAL;

using Duality.Resources;
using Duality.OggVorbis;

namespace Duality
{
	/// <summary>
	/// Describes the type of a sound. This is used for determining which specific
	/// volume settings affect each sound.
	/// </summary>
	public enum SoundType
	{
		/// <summary>
		/// A sound effect taking place in the game world.
		/// </summary>
		EffectWorld,
		/// <summary>
		/// A User Interface sound effect.
		/// </summary>
		EffectUI,
		/// <summary>
		/// A sound that is considered being game music.
		/// </summary>
		Music,
		/// <summary>
		/// A sound that is considered being spoken language.
		/// </summary>
		Speech
	}

	/// <summary>
	/// An instance of a <see cref="Duality.Resources.Sound"/>.
	/// </summary>
	public sealed class SoundInstance : IDisposable
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

		public const int AlSource_NotYetAssigned = -1;
		public const int AlSource_NotAvailable = 0;
		public const int PriorityStealThreshold = 15;
		public const int PriorityStealLoopedThreshold = 30;


		private	ContentRef<Sound>		sound		= null;
		private	ContentRef<AudioData>	audioData	= null;
		private	bool			disposed		= false;
		private	int				alSource		= AlSource_NotYetAssigned;
		private	GameObject		attachedTo		= null;
		private	Vector3			pos				= Vector3.Zero;
		private	Vector3			vel				= Vector3.Zero;
		private	float			vol				= 1.0f;
		private	float			pitch			= 1.0f;
		private	bool			is3D			= false;
		private	bool			looped			= false;
		private	bool			paused			= false;
		private	bool			registered		= false;
		private	int				curPriority		= 0;
		private	DirtyFlag		dirtyState		= DirtyFlag.All;
		private	float			playTime		= 0.0f;

		// Fading
		private	float			curFade			= 1.0f;
		private	float			fadeTarget		= 1.0f;
		private	float			fadeTimeSec		= 1.0f;
		private	float			pauseFade		= 1.0f;
		private	float			fadeWaitEnd		= 0.0f;

		// Streaming
		private	bool				strStreamed		= false;
		private	VorbisStreamHandle	strOvStr		= null;
		private	int[]				strAlBuffers	= null;
		private	Thread				strWorker		= null;
		private	StopRequest			strStopReq		= StopRequest.None;
		private	object				strLock			= new object();
		

		/// <summary>
		/// [GET] Whether the SoundInstance has been disposed. Disposed objects are not to be
		/// used anymore and should be treated as null or similar.
		/// </summary>
		public bool Disposed
		{
			get { return this.disposed; }
		}
		/// <summary>
		/// [GET] A reference to the <see cref="Duality.Resources.Sound"/> that is being played by
		/// this SoundInstance.
		/// </summary>
		public ContentRef<Sound> Sound
		{
			get { return this.sound; }
		}
		/// <summary>
		/// [GET] A reference to the <see cref="Duality.Resources.AudioData"/> that is being played by
		/// this SoundInstance.
		/// </summary>
		public ContentRef<AudioData> AudioData
		{
			get { return this.audioData; }
		}
		/// <summary>
		/// [GET] The <see cref="GameObject"/> that this SoundInstance is attached to.
		/// </summary>
		public GameObject AttachedTo
		{
			get { return this.attachedTo; }
		}
		/// <summary>
		/// [GET] Whether the sound is played 3d, "in space" or not.
		/// </summary>
		public bool Is3D
		{
			get { return this.is3D; }
		}
		/// <summary>
		/// [GET] The SoundInstances priority.
		/// </summary>
		public int Priority
		{
			get { return this.curPriority; }
		}
		/// <summary>
		/// [GET] When fading in or out, this value represents the current fading state.
		/// </summary>
		public float CurrentFade
		{
			get { return this.curFade; }
		}
		/// <summary>
		/// [GET] The target value for the current fade. Usually 0.0f or 1.0f for fadint out / in.
		/// </summary>
		public float FadeTarget
		{
			get { return this.fadeTarget; }
		}
		/// <summary>
		/// [GET] The time in seconds that this SoundInstance has been playing its sound.
		/// This value is affected by the sounds <see cref="Pitch"/>.
		/// </summary>
		public float PlayTime
		{
			get { return this.playTime; }
		}

		/// <summary>
		/// [GET / SET] The sounds local volume factor.
		/// </summary>
		public float Volume
		{
			get { return this.vol; }
			set { this.vol = value; this.dirtyState |= DirtyFlag.Vol; }
		}
		/// <summary>
		/// [GET / SET] The sounds local pitch factor.
		/// </summary>
		public float Pitch
		{
			get { return this.pitch; }
			set { this.pitch = value; this.dirtyState |= DirtyFlag.Pitch; }
		}
		/// <summary>
		/// [GET / SET] Whether the sound is played in a loop.
		/// </summary>
		public bool Looped
		{
			get { return this.looped; }
			set { this.looped = value; this.dirtyState |= DirtyFlag.Loop; }
		}
		/// <summary>
		/// [GET / SET] Whether the sound is currently paused.
		/// </summary>
		public bool Paused
		{
			get { return this.paused; }
			set { this.paused = value; this.dirtyState |= DirtyFlag.Paused; }
		}
		/// <summary>
		/// [GET / SET] The sounds position in space. If it is <see cref="AttachedTo">attached</see> to a GameObject,
		/// this value is considered relative to it.
		/// </summary>
		public Vector3 Pos
		{
			get { return this.pos; }
			set { this.pos = value; }
		}
		/// <summary>
		/// [GET / SET] The sounds velocity. If it is <see cref="AttachedTo">attached</see> to a GameObject,
		/// this value is considered relative to it.
		/// </summary>
		public Vector3 Vel
		{
			get { return this.vel; }
			set { this.vel = value; }
		}


		~SoundInstance()
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
				this.OnDisposed(manually);
			}
		}
		private void OnDisposed(bool manually)
		{
			if (manually)
			{
				if (this.strWorker != null && this.strWorker.IsAlive)
				{
					lock (this.strLock)
					{
						OV.EndStream(ref this.strOvStr);
					}
				}
				this.strWorker = null;

				this.attachedTo = null;
				this.curPriority = -1;

				lock (this.strLock)
				{
					if (this.alSource > AlSource_NotAvailable)
					{
						this.CleanupAlSource();
						DualityApp.Sound.FreeAlSource(this.alSource);
						this.alSource = AlSource_NotAvailable;
					}
					if (this.strAlBuffers != null)
					{
						for (int i = 0; i < this.strAlBuffers.Length; i++)
						{
							if (!AL.IsBuffer(this.strAlBuffers[i])) continue;
							AL.DeleteBuffer(this.strAlBuffers[i]);
						}
						this.strAlBuffers = null;
					}
					this.UnregisterPlaying();
				}
			}
		}

		
		internal SoundInstance(ContentRef<Sound> sound, GameObject attachObj)
		{
			this.attachedTo = attachObj;
			this.is3D = true;
			this.sound = sound;
			this.audioData = this.sound.IsAvailable ? this.sound.Res.FetchData() : null;
		}
		internal SoundInstance(ContentRef<Sound> sound, Vector3 pos)
		{
			this.pos = pos;
			this.is3D = true;
			this.sound = sound;
			this.audioData = this.sound.IsAvailable ? this.sound.Res.FetchData() : null;
		}
		internal SoundInstance(ContentRef<Sound> sound)
		{
			this.sound = sound;
			this.is3D = false;
			this.audioData = this.sound.IsAvailable ? this.sound.Res.FetchData() : null;
		}

		/// <summary>
		/// Stops the sound immediately.
		/// </summary>
		public void Stop()
		{
			if (this.alSource > AlSource_NotAvailable) AL.SourceStop(this.alSource);
			this.strStopReq = StopRequest.Immediately;
			// The next update will handle everything else
		}
		/// <summary>
		/// Fades the sound to a specific target value.
		/// </summary>
		/// <param name="target">The target value to fade to.</param>
		/// <param name="timeSeconds">The time in seconds the fading will take.</param>
		public void FadeTo(float target, float timeSeconds)
		{
			this.fadeTarget = target;
			this.fadeTimeSec = timeSeconds;
		}
		/// <summary>
		/// Resets the sounds current fade value to zero and starts to fade it in.
		/// </summary>
		/// <param name="timeSeconds">The time in seconds the fading will take.</param>
		public void BeginFadeIn(float timeSeconds)
		{
			this.curFade = 0.0f;
			this.FadeTo(1.0f, timeSeconds);
		}
		/// <summary>
		/// Fades the sound in from its current fade value. Note that SoundInstances are
		/// initialized with a fade value of 1.0f because they aren't faded in generally. 
		/// To achieve a regular "fade in" effect, you should use <see cref="BeginFadeIn(float)"/>.
		/// </summary>
		/// <param name="timeSeconds">The time in seconds the fading will take.</param>
		public void FadeIn(float timeSeconds)
		{
			this.FadeTo(1.0f, timeSeconds);
		}
		/// <summary>
		/// Fades out the sound.
		/// </summary>
		/// <param name="timeSeconds">The time in seconds the fading will take.</param>
		public void FadeOut(float timeSeconds)
		{
			this.FadeTo(0.0f, timeSeconds);
		}
		/// <summary>
		/// Halts the current fading, keepinf the current fade value as fade target.
		/// </summary>
		public void StopFade()
		{
			this.fadeTarget = this.curFade;
		}

		private void CleanupAlSource()
		{
			if (this.alSource <= AlSource_NotAvailable) return;
			lock (this.strLock)
			{
				// Do not reuse before-streamed sources OpenAL doesn't seem to like that.
				if (this.strStreamed)
				{
					AL.DeleteSource(this.alSource);
					this.alSource = AL.GenSource();
				}
				// Reuse other OpenAL sources
				else
				{
					//int num = 0;
					AL.SourceStop(this.alSource);
					AL.Source(this.alSource, ALSourcei.Buffer, 0);
					/*Al.alGetSourcei(this.alSource, Al.AL_BUFFERS_PROCESSED, out num);
					if (num > 0)
					{
						unsafe
						{
							int[] buffers = new int[num];
							fixed (int* result = &buffers[0])
							{
								Al.alSourceUnqueueBuffers(this.alSource, num, result);
							}
						}
					}*/

					AL.SourceRewind(this.alSource);
				}

				this.dirtyState = DirtyFlag.All;
			}
		}
		private bool GrabAlSource()
		{
			// Retrieve maximum number of sources by kind (2D / 3D)
			int maxNum = DualityApp.Sound.MaxOpenALSources * 3 / 4;
			if (!this.is3D) maxNum = DualityApp.Sound.MaxOpenALSources - maxNum;
			// Determine how many sources of this kind (2D / 3D) are playing
			int curNum = this.is3D ? DualityApp.Sound.NumPlaying3D : DualityApp.Sound.NumPlaying2D;
			// Determine how many sources using this sound resource are playing
			int curNumSoundRes = DualityApp.Sound.GetNumPlaying(this.sound);

			if (DualityApp.Sound.NumAvailable > 0 &&
				curNum < maxNum &&
				curNumSoundRes < this.sound.Res.MaxInstances)
			{
				this.alSource = DualityApp.Sound.RequestAlSource();
			}
			else
			{
				bool searchSimilar = curNumSoundRes >= this.sound.Res.MaxInstances;
				lock (this.strLock)
				{
					this.alSource = AlSource_NotAvailable;
					this.curPriority = this.PreCalcPriority();

					foreach (SoundInstance inst in DualityApp.Sound.Playing)
					{
						if (inst.alSource <= AlSource_NotAvailable) continue;
						if (!searchSimilar && this.is3D != inst.is3D) continue;
						if (searchSimilar && this.sound.Res != inst.sound.Res) continue;
						
						float ownPrioMult = 1.0f;
						if (searchSimilar && !inst.Looped) ownPrioMult *= MathF.Sqrt(inst.playTime + 1.0f);
							
						if (this.curPriority * ownPrioMult > inst.curPriority + 
							(inst.Looped ? PriorityStealLoopedThreshold : PriorityStealThreshold))
						{
							lock (inst.strLock)
							{
								this.alSource = inst.alSource;
								this.CleanupAlSource();
								inst.alSource = AlSource_NotAvailable;
							}
							break;
						}
						// List sorted by priority - if first fails, all will. Exception: Searching
						// similar sounds where play times are taken into account
						if (!searchSimilar)
							break;
					}
				}
			}

			return (this.alSource != AlSource_NotAvailable);
		}
		private int PreCalcPriority()
		{
			// Don't take fade into account: If a yet-to-fade-in sound wants to grab
			// the source of a already-playing sound, it should get its chance.
			float volTemp = this.GetTypeVolFactor() * this.sound.Res.VolumeFactor * this.vol;
			float priorityTemp = 1000.0f;
			priorityTemp *= volTemp;

			if (this.is3D)
			{
				float minDistTemp = this.sound.Res.MinDist;
				float maxDistTemp = this.sound.Res.MaxDist;
				Vector3 listenerPos = DualityApp.Sound.ListenerPos;
				Vector3 posTemp;
				if (this.attachedTo != null)	posTemp = this.attachedTo.Transform.Pos + this.pos;
				else							posTemp = this.pos;
				float dist = MathF.Sqrt(
					(posTemp.X - listenerPos.X) * (posTemp.X - listenerPos.X) +
					(posTemp.Y - listenerPos.Y) * (posTemp.Y - listenerPos.Y) +
					(posTemp.Z - listenerPos.Z) * (posTemp.Z - listenerPos.Z) * 0.25f);
				priorityTemp *= Math.Max(0.0f, 1.0f - (dist - minDistTemp) / (maxDistTemp - minDistTemp));
			}

			int numPlaying = DualityApp.Sound.GetNumPlaying(this.sound);
			return (int)Math.Round(priorityTemp / Math.Sqrt(numPlaying));
		}
		private float GetTypeVolFactor()
		{
			float optVolFactor;
			switch (this.sound.IsAvailable ? this.sound.Res.Type : SoundType.EffectWorld)
			{
				case SoundType.EffectUI:
					optVolFactor = DualityApp.UserData.SfxEffectVol;
					break;
				case SoundType.EffectWorld:
					optVolFactor = DualityApp.UserData.SfxEffectVol;
					break;
				case SoundType.Speech:
					optVolFactor = DualityApp.UserData.SfxSpeechVol;
					break;
				case SoundType.Music:
					optVolFactor = DualityApp.UserData.SfxMusicVol;
					break;
				default:
					optVolFactor = 1.0f;
					break;
			}
			return optVolFactor * DualityApp.UserData.SfxMasterVol * 0.5f;
		}
		private void RegisterPlaying()
		{
			if (this.registered) return;
			DualityApp.Sound.RegisterPlaying(this.sound, this.is3D);
			this.registered = true;
		}
		private void UnregisterPlaying()
		{
			if (!this.registered) return;
			DualityApp.Sound.UnregisterPlaying(this.sound, this.is3D);
			this.registered = false;
		}

		/// <summary>
		/// Updates the SoundInstance
		/// </summary>
		public void Update()
		{
			if (!DualityApp.Sound.IsAvailable) return;
			lock (this.strLock)
			{
				// Check existence of attachTo object
				if (this.attachedTo != null && this.attachedTo.Disposed) this.attachedTo = null;

				// Retrieve sound resource values
				Sound soundRes = this.sound.Res;
				AudioData audioDataRes = this.audioData.Res;
				if (soundRes == null || audioDataRes == null)
				{
					this.Dispose();
					return;
				}
				float optVolFactor = this.GetTypeVolFactor();
				float minDistTemp = soundRes.MinDist;
				float maxDistTemp = soundRes.MaxDist;
				float volTemp = optVolFactor * soundRes.VolumeFactor * this.vol * this.curFade * this.pauseFade;
				float pitchTemp = soundRes.PitchFactor * this.pitch;
				float priorityTemp = 1000.0f;
				priorityTemp *= volTemp;

				// Calculate 3D source values, distance and priority
				Vector3 posAbs = this.pos;
				Vector3 velAbs = this.vel;
				if (this.is3D)
				{
					Components.Transform attachTransform = this.attachedTo != null ? this.attachedTo.Transform : null;

					// Attach to object
					if (this.attachedTo != null && this.attachedTo != DualityApp.Sound.Listener)
					{
						MathF.TransformCoord(ref posAbs.X, ref posAbs.Y, attachTransform.Angle);
						MathF.TransformCoord(ref velAbs.X, ref velAbs.Y, attachTransform.Angle);
						posAbs += attachTransform.Pos;
						velAbs += attachTransform.Vel;
					}

					// Distance check
					Vector3 listenerPos = DualityApp.Sound.ListenerPos;
					float dist;
					if (this.attachedTo != DualityApp.Sound.Listener)
						dist = MathF.Sqrt(
							(posAbs.X - listenerPos.X) * (posAbs.X - listenerPos.X) +
							(posAbs.Y - listenerPos.Y) * (posAbs.Y - listenerPos.Y) +
							(posAbs.Z - listenerPos.Z) * (posAbs.Z - listenerPos.Z) * 0.25f);
					else
						dist = MathF.Sqrt(
							posAbs.X * posAbs.X +
							posAbs.Y * posAbs.Y +
							posAbs.Z * posAbs.Z * 0.25f);
					if (dist > maxDistTemp)
					{
						this.Dispose();
						return;
					}
					else
						priorityTemp *= Math.Max(0.0f, 1.0f - (dist - minDistTemp) / (maxDistTemp - minDistTemp));
				}

				// Grab an OpenAL source, if not yet assigned
				if (this.alSource == AlSource_NotYetAssigned)
				{
					if (this.GrabAlSource())
					{
						this.RegisterPlaying();
					}
					else
					{
						this.Dispose();
						return;
					}
				}

				// Determine source state, if available
				ALSourceState stateTemp = ALSourceState.Stopped;
				if (this.alSource > AlSource_NotAvailable) stateTemp = AL.GetSourceState(this.alSource);

				// If the source is stopped / finished, dispose and return
				if (stateTemp == ALSourceState.Stopped)
				{
					if (!audioDataRes.IsStreamed || this.strStopReq != StopRequest.None)
					{
						this.Dispose();
						return;
					}
				}
				else if (stateTemp == ALSourceState.Initial && this.strStopReq == StopRequest.Immediately)
				{
					this.Dispose();
					return;
				} 

				// Fading in and out
				bool fadeOut = this.fadeTarget <= 0.0f;
				if (!this.paused)
				{
					if (this.fadeTarget != this.curFade)
					{
						float fadeTemp = Time.TimeMult * Time.SPFMult / Math.Max(0.05f, this.fadeTimeSec);

						if (this.fadeTarget > this.curFade)
							this.curFade += fadeTemp;
						else
							this.curFade -= fadeTemp;

						if (Math.Abs(this.curFade - this.fadeTarget) < fadeTemp * 2.0f)
							this.curFade = this.fadeTarget;

						this.dirtyState |= DirtyFlag.Vol;
					}
				}

				// Special paused-fading
				if (this.paused && this.pauseFade > 0.0f)
				{
					this.pauseFade = MathF.Max(0.0f, this.pauseFade - Time.TimeMult * Time.SPFMult * 5.0f);
					this.dirtyState |= DirtyFlag.Paused | DirtyFlag.Vol;
				}
				else if (!this.paused && this.pauseFade < 1.0f)
				{
					this.pauseFade = MathF.Min(1.0f, this.pauseFade + Time.TimeMult * Time.SPFMult * 5.0f);
					this.dirtyState |= DirtyFlag.Paused | DirtyFlag.Vol;
				}

				// SlowMotion
				//if (this.type == SoundType.EffectWorld)
				//{
				//    pitchTemp *= (float)Math.Max(0.5d, SteApp.Current.SlowMotion);
				//    volTemp *= 2.0f * (float)Math.Min(0.5d, SteApp.Current.SlowMotion);
						
				//    // Hack: Pitch always dirty
				//    this.dirtyState |= DirtyFlag.Pitch;
				//}
				//else if (this.type == SoundType.Speech)
				//{
				//    volTemp *= (float)Math.Sqrt(SteApp.Current.SlowMotion);
				//}

				// Hack: Volume always dirty - just to be sure
				this.dirtyState |= DirtyFlag.Vol;

				if (this.is3D)
				{
					// Hack: Relative always dirty to support switching listeners without establishing a notifier-event
					this.dirtyState |= DirtyFlag.Relative;
					if (this.attachedTo != null) this.dirtyState |= DirtyFlag.AttachedTo;

					if ((this.dirtyState & DirtyFlag.Relative) != DirtyFlag.None)
						AL.Source(this.alSource, ALSourceb.SourceRelative, this.attachedTo == DualityApp.Sound.Listener);
					if ((this.dirtyState & DirtyFlag.Pos) != DirtyFlag.None)
						AL.Source(this.alSource, ALSource3f.Position, posAbs.X, -posAbs.Y, -posAbs.Z * 0.5f);
					if ((this.dirtyState & DirtyFlag.Vel) != DirtyFlag.None)
						AL.Source(this.alSource, ALSource3f.Velocity, velAbs.X, -velAbs.Y, -velAbs.Z);
				}
				else
				{
					if ((this.dirtyState & DirtyFlag.Relative) != DirtyFlag.None)
						AL.Source(this.alSource, ALSourceb.SourceRelative, true);
					if ((this.dirtyState & DirtyFlag.Pos) != DirtyFlag.None)
						AL.Source(this.alSource, ALSource3f.Position, 0.0f, 0.0f, 0.0f);
					if ((this.dirtyState & DirtyFlag.Vel) != DirtyFlag.None)
						AL.Source(this.alSource, ALSource3f.Velocity, 0.0f, 0.0f, 0.0f);
				}
				if ((this.dirtyState & DirtyFlag.MaxDist) != DirtyFlag.None)
					AL.Source(this.alSource, ALSourcef.MaxDistance, maxDistTemp);
				if ((this.dirtyState & DirtyFlag.RefDist) != DirtyFlag.None)
					AL.Source(this.alSource, ALSourcef.ReferenceDistance, minDistTemp);
				if ((this.dirtyState & DirtyFlag.Loop) != DirtyFlag.None)
					AL.Source(this.alSource, ALSourceb.Looping, (this.looped && !audioDataRes.IsStreamed));
				if ((this.dirtyState & DirtyFlag.Vol) != DirtyFlag.None)
					AL.Source(this.alSource, ALSourcef.Gain, volTemp);
				if ((this.dirtyState & DirtyFlag.Pitch) != DirtyFlag.None)
					AL.Source(this.alSource, ALSourcef.Pitch, pitchTemp);
				if ((this.dirtyState & DirtyFlag.Paused) != DirtyFlag.None)
				{
					if (this.paused && this.pauseFade == 0.0f && stateTemp == ALSourceState.Playing)
						AL.SourcePause(this.alSource);
					else if ((!this.paused || this.pauseFade > 0.0f) && stateTemp == ALSourceState.Paused)
						AL.SourcePlay(this.alSource);
				}
				this.dirtyState = DirtyFlag.None;

				// Update play time
				if (!this.paused)
				{
					this.playTime += MathF.Max(0.5f, pitchTemp) * Time.TimeMult * Time.SPFMult;
					if (this.sound.Res.FadeOutAt > 0.0f && this.playTime >= this.sound.Res.FadeOutAt)
						this.FadeOut(this.sound.Res.FadeOutTime);
				}

				// Finish priority calculation
				this.curPriority = (int)Math.Round(priorityTemp / Math.Sqrt(DualityApp.Sound.GetNumPlaying(this.sound))); 

				// Initially play the source
				if (stateTemp == ALSourceState.Initial && !this.paused)
				{
					if (audioDataRes.IsStreamed)
					{
						this.strStreamed = true;
						this.strWorker = new Thread(ThreadStreamFunc);
						this.strWorker.Start(this);
					}
					else
					{
						AL.SourceQueueBuffer(this.alSource, audioDataRes.AlBuffer);
						AL.SourcePlay(this.alSource);
					} 
				}
				
				// Remove faded out sources
				if (fadeOut && volTemp <= 0.0f)
				{
					this.fadeWaitEnd += Time.TimeMult * Time.MsPFMult;
					// After fading out entirely, wait 50 ms before actually stopping the source to prevent unpleasant audio tick / glitch noises
					if (this.fadeWaitEnd > 50.0f)
					{
						this.Dispose();
						return;
					}
				}
				else
					this.fadeWaitEnd = 0.0f;

			}
		}


		private static void ThreadStreamFunc(object param)
		{
			SoundInstance sndInst = (SoundInstance)param;
			while (true)
			{
				lock (sndInst.strLock)
				{
					if (sndInst.Disposed) return;
					if (!DualityApp.Sound.IsAvailable) return;

					ALSourceState stateTemp = ALSourceState.Stopped;
					if (sndInst.alSource > AlSource_NotAvailable) stateTemp = AL.GetSourceState(sndInst.alSource);

					if (stateTemp == ALSourceState.Stopped && sndInst.strStopReq != StopRequest.None)
					{
						// Stopped due to regular EOF. If strStopReq is NOT set,
						// the source stopped playing because it reached the end of the buffer
						// but in fact only because we were too slow inserting new data.
						return;
					}
					else if (sndInst.strStopReq == StopRequest.Immediately)
					{
						// Stopped intentionally due to Stop()
						if (sndInst.alSource > AlSource_NotAvailable) AL.SourceStop(sndInst.alSource);
						return;
					}

					Sound soundRes = sndInst.sound.Res;
					AudioData audioDataRes = sndInst.audioData.Res;
					if (soundRes == null || audioDataRes == null)
					{
						sndInst.Dispose();
						return;
					}
					if (stateTemp == ALSourceState.Initial)
					{
						// Generate streaming buffers
						sndInst.strAlBuffers = new int[3];
						for (int i = 0; i < sndInst.strAlBuffers.Length; ++i)
						{
							AL.GenBuffers(1, out sndInst.strAlBuffers[i]);
						}

						// Begin streaming
						OV.BeginStreamFromMemory(audioDataRes.OggVorbisData, out sndInst.strOvStr);

						// Initially, completely fill all buffers
						for (int i = 0; i < sndInst.strAlBuffers.Length; ++i)
						{
							PcmData pcm;
							bool eof = !OV.StreamChunk(sndInst.strOvStr, out pcm);
							if (pcm.dataLength > 0)
							{
								AL.BufferData(
									sndInst.strAlBuffers[i], 
									pcm.channelCount == 1 ? ALFormat.Mono16 : ALFormat.Stereo16,
									pcm.data, 
									pcm.dataLength * PcmData.SizeOfDataElement, 
									pcm.sampleRate);
								AL.SourceQueueBuffer(sndInst.alSource, sndInst.strAlBuffers[i]);
								if (eof) break;
							}
							else break;
						}

						// Initially play source
						AL.SourcePlay(sndInst.alSource);
						stateTemp = AL.GetSourceState(sndInst.alSource);
					}
					else
					{
						int num;
						AL.GetSource(sndInst.alSource, ALGetSourcei.BuffersProcessed, out num);
						while (num > 0)
						{
							num--;

							int unqueued;
							unqueued = AL.SourceUnqueueBuffer(sndInst.alSource);

							if (OV.IsStreamValid(sndInst.strOvStr))
							{
								PcmData pcm;
								bool eof = !OV.StreamChunk(sndInst.strOvStr, out pcm);
								if (eof)
								{
									OV.EndStream(ref sndInst.strOvStr);
									if (sndInst.looped)
									{
										OV.BeginStreamFromMemory(audioDataRes.OggVorbisData, out sndInst.strOvStr);
										if (pcm.dataLength == 0)
										{
											eof = !OV.StreamChunk(sndInst.strOvStr, out pcm);
										}
									}
								}
								if (pcm.dataLength > 0)
								{
									AL.BufferData(
										unqueued, 
										pcm.channelCount == 1 ? ALFormat.Mono16 : ALFormat.Stereo16,
										pcm.data, 
										pcm.dataLength * PcmData.SizeOfDataElement, 
										pcm.sampleRate);
									AL.SourceQueueBuffer(sndInst.alSource, unqueued);
								}
								if (pcm.dataLength == 0 || eof)
								{
									sndInst.strStopReq = StopRequest.EndOfStream;
									break;
								}
							}
						}
					}

					if (stateTemp == ALSourceState.Stopped && sndInst.strStopReq == StopRequest.None)
					{
						// If the source stopped unintentionally, restart it. (See above)
						AL.SourcePlay(sndInst.alSource);
					}
				}
				Thread.Sleep(16);
			}
		}
	}
}
