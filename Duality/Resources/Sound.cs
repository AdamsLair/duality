using System;
using Duality.EditorHints;

namespace Duality.Resources
{
	/// <summary>
	/// A Sound is parameterized <see cref="Duality.Resources.AudioData"/>. Note that a Sound
	/// Resource does not contain any kind of audio data by itsself.
	/// </summary>
	/// <example>
	/// While there may be one AudioData Resource sounding like an Explosion effect, there could 
	/// be many Sounds referring to it, like a very loud or very quiet Explosion or one that is 
	/// noticable on higher distance than others.
	/// </example>
	/// <seealso cref="Duality.Resources.AudioData"/>
	[Serializable]
	[ExplicitResourceReference(typeof(AudioData))]
	public class Sound : Resource
	{
		/// <summary>
		/// A Sound resources file extension.
		/// </summary>
		public new const string FileExt = ".Sound" + Resource.FileExt;
		
		/// <summary>
		/// [GET] A simple beep Sound.
		/// </summary>
		public static ContentRef<Sound> Beep		{ get; private set; }
		/// <summary>
		/// [GET] A drone loop Sound. Since this is a stereo Sound, it will always be played 2D.
		/// </summary>
		public static ContentRef<Sound> DroneLoop	{ get; private set; }
		/// <summary>
		/// [GET] A logo jingle Sound. Since this is a stereo Sound, it will always be played 2D.
		/// </summary>
		public static ContentRef<Sound> LogoJingle	{ get; private set; }

		internal static void InitDefaultContent()
		{
			const string VirtualContentPath		= ContentProvider.VirtualContentPath + "Sound:";
			const string ContentPath_Beep		= VirtualContentPath + "Beep";
			const string ContentPath_DroneLoop	= VirtualContentPath + "DroneLoop";
			const string ContentPath_LogoJingle	= VirtualContentPath + "LogoJingle";

			ContentProvider.AddContent(ContentPath_Beep, new Sound(AudioData.Beep));
			ContentProvider.AddContent(ContentPath_DroneLoop, new Sound(AudioData.DroneLoop));
			ContentProvider.AddContent(ContentPath_LogoJingle, new Sound(AudioData.LogoJingle));

			Beep		= ContentProvider.RequestContent<Sound>(ContentPath_Beep);
			DroneLoop	= ContentProvider.RequestContent<Sound>(ContentPath_DroneLoop);
			LogoJingle	= ContentProvider.RequestContent<Sound>(ContentPath_LogoJingle);
		}

		/// <summary>
		/// Creates a new Sound Resource based on the specified AudioData, saves it and returns a reference to it.
		/// </summary>
		/// <param name="baseRes"></param>
		/// <returns></returns>
		public static ContentRef<Sound> CreateFromAudioData(ContentRef<AudioData> baseRes)
		{
			string resPath = PathHelper.GetFreePath(baseRes.FullName, FileExt);
			Sound res = new Sound(baseRes);
			res.Save(resPath);
			return res;
		}


		private	int			maxInstances	= 5;
		private	float		minDistFactor	= 1.0f;
		private	float		maxDistFactor	= 1.0f;
		private	float		volFactor		= 1.0f;
		private	float		pitchFactor		= 1.0f;
		private	float		fadeOutAt		= 0.0f;
		private	float		fadeOutTime		= 0.0f;
		private	SoundType	type			= SoundType.EffectWorld;
		private	ContentRef<AudioData>	audioData	= ContentRef<AudioData>.Null;

		/// <summary>
		/// [GET / SET] The <see cref="Duality.Resources.AudioData"/> that is parameterized by this Sound.
		/// </summary>
		public ContentRef<AudioData> Data
		{
			get { return this.audioData; }
			set { this.LoadData(value); }
		}
		/// <summary>
		/// [GET / SET] The category to which this Sound belongs.
		/// </summary>
		public SoundType Type
		{
			get { return this.type; }
			set { this.type = value; }
		}
		/// <summary>
		/// [GET / SET] Maximum number of <see cref="Duality.SoundInstance">SoundInstances</see> of this Sound that can
		/// play simultaneously. If exceeded, any new instances of it are discarded.
		/// </summary>
		public int MaxInstances
		{
			get { return this.maxInstances; }
			set { this.maxInstances = value; }
		}
		/// <summary>
		/// [GET / SET] A volume factor that is applied when playing this sound.
		/// </summary>
		public float VolumeFactor
		{
			get { return this.volFactor; }
			set { this.volFactor = value; }
		}
		/// <summary>
		/// [GET / SET] A pitch factor that is applied when playing this sound.
		/// </summary>
		public float PitchFactor
		{
			get { return this.pitchFactor; }
			set { this.pitchFactor = value; }
		}
		/// <summary>
		/// [GET / SET] Play time in seconds at which <see cref="Duality.SoundInstance">SoundInstances</see> of this Sound
		/// automatically fade out.
		/// </summary>
		public float FadeOutAt
		{
			get { return this.fadeOutAt; }
			set { this.fadeOutAt = value; }
		}
		/// <summary>
		/// [GET / SET] If <see cref="FadeOutAt"/> has been triggered, this is the fade out time in seconds that is used.
		/// </summary>
		public float FadeOutTime
		{
			get { return this.fadeOutTime; }
			set { this.fadeOutTime = value; }
		}
		/// <summary>
		/// [GET / SET] The distance at which the sound is played at full volume. Getting nearer to the source won't increase the volume anymore.
		/// Note that the value is a factor to <see cref="SoundDevice.DefaultMinDist"/>.
		/// </summary>
		/// <seealso cref="MinDist"/>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public float MinDistFactor
		{
			get { return this.minDistFactor; }
			set { this.minDistFactor = value; }
		}
		/// <summary>
		/// [GET / SET] The distance at which the sound is played at zero volume.
		/// Note that the value is a factor to <see cref="SoundDevice.DefaultMaxDist"/>.
		/// </summary>
		/// <seealso cref="MaxDist"/>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public float MaxDistFactor
		{
			get { return this.maxDistFactor; }
			set { this.maxDistFactor = value; }
		}
		/// <summary>
		/// [GET / SET] The distance at which the sound is played at full volume. Getting nearer to the source won't increase the volume anymore.
		/// </summary>
		/// <seealso cref="MinDistFactor"/>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public float MinDist
		{
			get { return DualityApp.Sound.DefaultMinDist * this.minDistFactor; }
			set { this.minDistFactor = value / DualityApp.Sound.DefaultMinDist; }
		}
		/// <summary>
		/// [GET / SET] The distance at which the sound is played at zero volume.
		/// </summary>
		/// <seealso cref="MaxDistFactor"/>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public float MaxDist
		{
			get { return DualityApp.Sound.DefaultMaxDist * this.maxDistFactor; }
			set { this.maxDistFactor = value / DualityApp.Sound.DefaultMaxDist; }
		}
		/// <summary>
		/// [GET] Returns whether the Sound will be streamed when playing.
		/// </summary>
		public bool IsStreamed
		{
			get { return this.audioData.IsAvailable && this.audioData.Res.IsStreamed; }
		}
		/// <summary>
		/// The OpenAL buffer handle to the audio data that is used by this Sound.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		internal int AlBuffer
		{
			get { return this.audioData.IsAvailable ? this.audioData.Res.AlBuffer : AudioData.AlBuffer_NotAvailable; }
		}

		/// <summary>
		/// Creates a new, empty sound. Since it does not refer to any <see cref="Duality.Resources.AudioData"/> yet,
		/// it can't be played.
		/// </summary>
		public Sound() : this(AudioData.Beep) {}
		/// <summary>
		/// Creates a new Sound referring to an existing <see cref="Duality.Resources.AudioData"/>.
		/// </summary>
		/// <param name="baseData"></param>
		public Sound(ContentRef<AudioData> baseData)
		{
			this.LoadData(baseData);
		}

		/// <summary>
		/// Assigns new <see cref="Duality.Resources.AudioData"/> to this Sound.
		/// </summary>
		/// <param name="data"></param>
		public void LoadData(ContentRef<AudioData> data)
		{
			this.audioData = data;
			if (this.audioData.IsAvailable) this.audioData.Res.SetupAlBuffer();
		}

		protected override void OnCopyTo(Resource r, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(r, provider);
			Sound c = r as Sound;
			c.maxInstances = this.maxInstances;
			c.minDistFactor = this.minDistFactor;
			c.maxDistFactor = this.maxDistFactor;
			c.volFactor = this.volFactor;
			c.pitchFactor = this.pitchFactor;
			c.fadeOutAt = this.fadeOutAt;
			c.fadeOutTime = this.fadeOutTime;
			c.LoadData(this.audioData);
		}
	}
}
