using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

using Duality.Resources;
using Duality.Backend;

namespace Duality.Audio
{
	/// <summary>
	/// An instance of a <see cref="Duality.Resources.Sound"/>.
	/// </summary>
	[DontSerialize]
	public sealed class SoundInstance : IDisposable, IAudioStreamProvider
	{
		public const int PriorityStealThreshold       = 15;
		public const int PriorityStealLoopedThreshold = 30;


		private	ContentRef<Sound>     sound     = null;
		private	ContentRef<AudioData> audioData = null;
		private	INativeAudioSource    native    = null;

		private	bool       disposed       = false;
		private	bool       notYetAssigned = true;
		private	GameObject attachedTo     = null;
		private	Vector3    pos            = Vector3.Zero;
		private	Vector3    vel            = Vector3.Zero;
		private	float      vol            = 1.0f;
		private	float      pitch          = 1.0f;
		private	float      lowpass        = 1.0f;
		private	float      panning        = 0.0f;
		private	bool       is3D           = false;
		private	bool       looped         = false;
		private	bool       paused         = false;
		private	bool       registered     = false;
		private	int        curPriority    = 0;
		private	float      playTime       = 0.0f;

		// Fading
		private	float curFade     = 1.0f;
		private	float fadeTarget  = 1.0f;
		private	float fadeTimeSec = 1.0f;
		private	float pauseFade   = 1.0f;
		private	float fadeWaitEnd = 0.0f;

		// Streaming
		private	VorbisStreamHandle strOvStr = null;
		

		/// <summary>
		/// [GET] The currently used native audio source, as provided by the Duality backend. Don't use this unless you know exactly what you're doing.
		/// </summary>
		public INativeAudioSource Native
		{
			get { return this.native; }
		}
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
			set { this.vol = value; }
		}
		/// <summary>
		/// [GET / SET] The sounds local pitch factor.
		/// </summary>
		public float Pitch
		{
			get { return this.pitch; }
			set { this.pitch = value; }
		}
		/// <summary>
		/// [GET / SET] The sounds local lowpass value. Lower values cut off more frequencies.
		/// </summary>
		public float Lowpass
		{
			get { return this.lowpass; }
			set { this.lowpass = value; }
		}
		/// <summary>
		/// [GET / SET] The sounds local stereo panning, ranging from -1.0f (left) to 1.0f (right).
		/// Only available for 2D sounds.
		/// </summary>
		public float Panning
		{
			get { return this.panning; }
			set { this.panning = value; }
		}
		/// <summary>
		/// [GET / SET] Whether the sound is played in a loop.
		/// </summary>
		public bool Looped
		{
			get { return this.looped; }
			set { this.looped = value; }
		}
		/// <summary>
		/// [GET / SET] Whether the sound is currently paused.
		/// </summary>
		public bool Paused
		{
			get { return this.paused; }
			set { this.paused = value; }
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
				this.attachedTo = null;
				this.curPriority = -1;

				if (this.native != null)
				{
					this.native.Dispose();
					this.native = null;
				}
				this.UnregisterPlaying();
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
			if (this.native != null)
				this.native.Stop();
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

		private bool GrabNativeSource()
		{
			if (this.native != null) return true;

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
				this.native = DualityApp.AudioBackend.CreateSource();
			}
			else
			{
				bool searchSimilar = curNumSoundRes >= this.sound.Res.MaxInstances;
				this.curPriority = this.PreCalcPriority();

				foreach (SoundInstance inst in DualityApp.Sound.Playing)
				{
					if (inst.native == null) continue;
					if (!searchSimilar && this.is3D != inst.is3D) continue;
					if (searchSimilar && this.sound.Res != inst.sound.Res) continue;
						
					float ownPrioMult = 1.0f;
					if (searchSimilar && !inst.Looped) ownPrioMult *= MathF.Sqrt(inst.playTime + 1.0f);
							
					if (this.curPriority * ownPrioMult > inst.curPriority + 
						(inst.Looped ? PriorityStealLoopedThreshold : PriorityStealThreshold))
					{
						this.native = inst.native;
						this.native.Reset();
						inst.native = null;
						break;
					}
					// List sorted by priority - if first fails, all will. Exception: Searching
					// similar sounds where play times are taken into account
					if (!searchSimilar)
						break;
				}
			}

			this.notYetAssigned = false;
			return this.native != null;
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
			switch (this.sound.IsAvailable ? this.sound.Res.Type : SoundType.World)
			{
				case SoundType.UserInterface:
					optVolFactor = DualityApp.UserData.SoundEffectVol;
					break;
				case SoundType.World:
					optVolFactor = DualityApp.UserData.SoundEffectVol;
					break;
				case SoundType.Speech:
					optVolFactor = DualityApp.UserData.SoundSpeechVol;
					break;
				case SoundType.Music:
					optVolFactor = DualityApp.UserData.SoundMusicVol;
					break;
				default:
					optVolFactor = 1.0f;
					break;
			}
			return optVolFactor * DualityApp.UserData.SoundMasterVol * 0.5f;
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

			// Set up local variables for state calculation
			Vector3 listenerPos = DualityApp.Sound.ListenerPos;
			bool attachedToListener = this.attachedTo != null && ((this.attachedTo == DualityApp.Sound.Listener) || this.attachedTo.IsChildOf(DualityApp.Sound.Listener));
			float optVolFactor = this.GetTypeVolFactor();
			float priorityTemp = 1000.0f;
			AudioSourceState nativeState = AudioSourceState.Default;
			nativeState.MinDistance = soundRes.MinDist;
			nativeState.MaxDistance = soundRes.MaxDist;
			nativeState.Volume = optVolFactor * soundRes.VolumeFactor * this.vol * this.curFade * this.pauseFade;
			nativeState.Pitch = soundRes.PitchFactor * this.pitch;
			nativeState.Lowpass = soundRes.LowpassFactor * this.lowpass;
			priorityTemp *= nativeState.Volume;

			// Calculate 3D source values, distance and priority
			nativeState.Position = this.pos;
			nativeState.Velocity = this.vel;
			if (this.is3D)
			{
				Components.Transform attachTransform = this.attachedTo != null ? this.attachedTo.Transform : null;

				// Attach to object
				if (this.attachedTo != null)
				{
					MathF.TransformCoord(ref nativeState.Position.X, ref nativeState.Position.Y, attachTransform.Angle);
					MathF.TransformCoord(ref nativeState.Velocity.X, ref nativeState.Velocity.Y, attachTransform.Angle);
					nativeState.Position += attachTransform.Pos;
					nativeState.Velocity += attachTransform.Vel;
				}

				// Distance check
				float dist = MathF.Sqrt(
					(nativeState.Position.X - listenerPos.X) * (nativeState.Position.X - listenerPos.X) +
					(nativeState.Position.Y - listenerPos.Y) * (nativeState.Position.Y - listenerPos.Y) +
					(nativeState.Position.Z - listenerPos.Z) * (nativeState.Position.Z - listenerPos.Z) * 0.25f);
				if (dist > nativeState.MaxDistance)
				{
					this.Dispose();
					return;
				}
				else
					priorityTemp *= Math.Max(0.0f, 1.0f - (dist - nativeState.MinDistance) / (nativeState.MaxDistance - nativeState.MinDistance));
			}

			if (this.notYetAssigned)
			{
				// Grab a native audio source
				if (this.GrabNativeSource())
				{
					this.RegisterPlaying();
				}
				// If there is none available, just stop right there.
				else
				{
					this.Dispose();
					return;
				}
			}

			// If the source is stopped / finished, dispose and return
			if (this.native == null || this.native.IsFinished)
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
					float fadeTemp = Time.TimeMult * Time.SecondsPerFrame / Math.Max(0.05f, this.fadeTimeSec);

					if (this.fadeTarget > this.curFade)
						this.curFade += fadeTemp;
					else
						this.curFade -= fadeTemp;

					if (Math.Abs(this.curFade - this.fadeTarget) < fadeTemp * 2.0f)
						this.curFade = this.fadeTarget;
				}
			}

			// Special paused-fading
			if (this.paused && this.pauseFade > 0.0f)
			{
				this.pauseFade = MathF.Max(0.0f, this.pauseFade - Time.TimeMult * Time.SecondsPerFrame * 5.0f);
			}
			else if (!this.paused && this.pauseFade < 1.0f)
			{
				this.pauseFade = MathF.Min(1.0f, this.pauseFade + Time.TimeMult * Time.SecondsPerFrame * 5.0f);
			}

			// Apply the sounds state to its internal native audio source
			if (this.native != null)
			{
				if (this.is3D)
				{
					nativeState.RelativeToListener = attachedToListener;
					if (attachedToListener)
						nativeState.Position -= listenerPos;
				}
				else
				{
					nativeState.RelativeToListener = true;
					nativeState.Position = new Vector3(this.panning, 0.0f, 0.0f);
					nativeState.Velocity = Vector3.Zero;
				}
				nativeState.Looped = this.looped;
				nativeState.Paused = this.paused && this.pauseFade == 0.0f;

				this.native.ApplyState(ref nativeState);
			}

			// Update play time
			if (!this.paused)
			{
				this.playTime += MathF.Max(0.5f, nativeState.Pitch) * Time.TimeMult * Time.SecondsPerFrame;
				if (this.sound.Res.FadeOutAt > 0.0f && this.playTime >= this.sound.Res.FadeOutAt)
					this.FadeOut(this.sound.Res.FadeOutTime);
			}

			// Finish priority calculation
			this.curPriority = (int)Math.Round(priorityTemp / Math.Sqrt(DualityApp.Sound.GetNumPlaying(this.sound))); 

			// Initially play the source
			if (this.native.IsInitial && !this.paused)
			{
				if (audioDataRes.IsStreamed)
				{
					this.native.Play(this);
				}
				else if (audioDataRes.Native != null)
				{
					this.native.Play(audioDataRes.Native);
				} 
			}
				
			// Remove faded out sources
			if (fadeOut && nativeState.Volume <= 0.0f)
			{
				this.fadeWaitEnd += Time.TimeMult * Time.MillisecondsPerFrame;
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

		void IAudioStreamProvider.OpenStream()
		{
			AudioData audioDataRes = this.audioData.Res;
			OggVorbis.BeginStreamFromMemory(audioDataRes.OggVorbisData, out this.strOvStr);
		}
		bool IAudioStreamProvider.ReadStream(INativeAudioBuffer targetBuffer)
		{
			if (!OggVorbis.IsStreamValid(this.strOvStr))
				return false;

			AudioData audioDataRes = this.audioData.Res;
			PcmData pcm;
			bool eof = !OggVorbis.StreamChunk(this.strOvStr, out pcm);
			if (eof)
			{
				OggVorbis.EndStream(ref this.strOvStr);
				if (this.looped)
				{
					OggVorbis.BeginStreamFromMemory(audioDataRes.OggVorbisData, out this.strOvStr);
					if (pcm.DataLength == 0)
						eof = !OggVorbis.StreamChunk(this.strOvStr, out pcm);
					else
						eof = false;
				}
			}

			if (pcm.DataLength > 0)
			{
				targetBuffer.LoadData(
					pcm.SampleRate,
					pcm.Data,
					pcm.DataLength,
					pcm.ChannelCount == 1 ? AudioDataLayout.Mono : AudioDataLayout.LeftRight,
					AudioDataElementType.Short);
			}

			return pcm.DataLength != 0 && !eof;
		}
		void IAudioStreamProvider.CloseStream()
		{
			OggVorbis.EndStream(ref this.strOvStr);
		}
	}
}
