using System;
using System.Collections.Generic;
using System.Linq;

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
		/// <summary>
		/// Creates a new Sound Resource based on the specified AudioData, saves it and returns a reference to it.
		/// </summary>
		/// <param name="baseRes"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static ContentRef<Sound> CreateFromAudioData(IEnumerable<ContentRef<AudioData>> baseRes, string name = null)
		{
			if (!baseRes.Any()) return null;

			string basePath = baseRes.FirstOrDefault().FullName;
			if (name != null) basePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(basePath), name);

			string resPath = PathHelper.GetFreePath(basePath, FileExt);
			Sound res = new Sound(baseRes);
			res.Save(resPath);
			return res;
		}
		/// <summary>
		/// Creates a set of new Sound Resources based on the specified AudioData, saves it and returns references to it.
		/// The incoming AudioData is automatically grouped to the least number of Sounds, according to naming and path conventions.
		/// </summary>
		/// <param name="baseRes"></param>
		/// <returns></returns>
		public static List<ContentRef<Sound>> CreateMultipleFromAudioData(IEnumerable<ContentRef<AudioData>> baseRes)
		{
			char[] trimEndChars = new []{'0','1','2','3','4','5','6','7','8','9','_','-','.','#','~'};
			List<ContentRef<AudioData>> sourceData = baseRes.Reverse().ToList();
			List<ContentRef<Sound>> result = new List<ContentRef<Sound>>();

			// Split data into singular data and grouped data
			while (sourceData.Count > 0)
			{
				ContentRef<AudioData> data = sourceData[sourceData.Count - 1];
				string mutualName = data.Name.TrimEnd(trimEndChars);
				string mutualDir = System.IO.Path.GetDirectoryName(data.Path);

				// Group similar AudioData
				List<ContentRef<AudioData>> localGroup = new List<ContentRef<AudioData>>();
				for (int i = sourceData.Count - 1; i >= 0; i--)
				{
					if (System.IO.Path.GetDirectoryName(sourceData[i].Path) != mutualDir) continue;
					if (!sourceData[i].Name.StartsWith(mutualName)) continue;
					localGroup.Add(sourceData[i]);
					sourceData.RemoveAt(i);
				}
				result.Add(Sound.CreateFromAudioData(localGroup, localGroup.Count > 1 ? mutualName : null));
			}

			return result;
		}


		private	int			maxInstances	= 5;
		private	float		minDistFactor	= 1.0f;
		private	float		maxDistFactor	= 1.0f;
		private	float		volFactor		= 1.0f;
		private	float		pitchFactor		= 1.0f;
		private	float		fadeOutAt		= 0.0f;
		private	float		fadeOutTime		= 0.0f;
		private	SoundType	type			= SoundType.EffectWorld;
		private	List<ContentRef<AudioData>>	audioData	= null;

		/// <summary>
		/// [GET / SET] A collection of <see cref="DataEntry">parameterized data entries</see> that refers to the source <see cref="Duality.Resources.AudioData"/> that is used by this Sound.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback | MemberFlags.AffectsOthers)]
		public List<ContentRef<AudioData>> Data
		{
			get { return this.audioData; }
			set
			{ 
				this.audioData = value;
				this.PreloadData();
			}
		}
		/// <summary>
		/// [GET / SET] The main source <see cref="Duality.Resources.AudioData"/> that is used by this Sound.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public ContentRef<AudioData> MainData
		{
			get { return this.audioData != null && this.audioData.Count > 0 ? this.audioData[0] : null; }
			set
			{
				if (this.audioData == null)
					this.audioData = new List<ContentRef<AudioData>>();

				if (this.audioData.Count == 0)
					this.audioData.Add(value);
				else 
					this.audioData[0] = value;
			}
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
			this.audioData = new List<ContentRef<AudioData>>{ baseData };
			this.PreloadData();
		}
		/// <summary>
		/// Creates a new Sound referring to an existing set of <see cref="Duality.Resources.AudioData"/>.
		/// </summary>
		/// <param name="baseData"></param>
		public Sound(IEnumerable<ContentRef<AudioData>> baseData)
		{
			this.audioData = baseData.ToList();
			this.PreloadData();
		}

		/// <summary>
		/// Upon playing the Sound, this method is called once to determine which of the referenced
		/// <see cref="Duality.Resources.AudioData"/> objects is to be played.
		/// </summary>
		/// <returns></returns>
		public ContentRef<AudioData> FetchData()
		{
			return MathF.Rnd.OneOf(this.audioData);
		}

		/// <summary>
		/// Assigns new <see cref="Duality.Resources.AudioData"/> to this Sound.
		/// </summary>
		/// <param name="data"></param>
		private void PreloadData()
		{
			if (this.audioData == null) return;
			for (int i = 0; i < this.audioData.Count; i++)
			{
				if (this.audioData[i].IsAvailable)
					this.audioData[i].Res.SetupAlBuffer();
			}
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
			c.audioData = this.audioData != null ? this.audioData.ToList() : null;
			c.PreloadData();
		}
		protected override void OnLoaded()
		{
			base.OnLoaded();
			this.PreloadData();
		}
	}
}
