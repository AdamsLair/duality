using System;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality.Editor;
using Duality.Cloning;
using Duality.Properties;
using Duality.Audio;
using Duality.Backend;


namespace Duality.Resources
{
	/// <summary>
	/// Stores compressed audio data (Ogg Vorbis) in system memory as well as a reference to the
	/// OpenAL buffer containing actual PCM data, once set up. The OpenAL buffer is set up lazy
	/// i.e. as soon as demanded by accessing the AlBuffer property or calling SetupAlBuffer.
	/// </summary>
	/// <seealso cref="Duality.Resources.Sound"/>
	[ExplicitResourceReference()]
	[EditorHintCategory(CoreResNames.CategorySound)]
	[EditorHintImage(CoreResNames.ImageAudioData)]
	public class AudioData : Resource
	{
		/// <summary>
		/// [GET] A simple beep AudioData.
		/// </summary>
		public static ContentRef<AudioData> Beep		{ get; private set; }

		internal static void InitDefaultContent()
		{
			InitDefaultContent<AudioData>(".ogg", stream => new AudioData(stream));
		}


		private	byte[]	data			= null;
		private	bool	forceStream		= false;
		[DontSerialize] private	INativeAudioBuffer native = null;

		/// <summary>
		/// [GET] The backends native audio buffer representation. Don't use this unless you know exactly what you're doing.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public INativeAudioBuffer Native
		{
			get
			{
				if (this.native == null) this.SetupNativeBuffer();
				return this.native;
			}
		}
		/// <summary>
		/// [GET / SET] A data chunk representing Ogg Vorbis compressed
		/// audio data.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public byte[] OggVorbisData
		{
			get { return this.data; }
			set
			{
				if (this.data != value)
				{
					this.data = value;
					this.DisposeNativeBuffer();
					this.SetupNativeBuffer();
				}
			}
		}
		/// <summary>
		/// [GET / SET] If set to true, when playing a <see cref="Duality.Resources.Sound"/> that refers to this
		/// AudioData, it is forced to be played streamed. Normally, streaming kicks in automatically when playing
		/// very large sound files, such as music or large environmental ambience.
		/// </summary>
		public bool ForceStream
		{
			get { return this.forceStream; }
			set { this.forceStream = value; this.DisposeNativeBuffer(); }
		}
		/// <summary>
		/// [GET] Returns whether this AudioData will be played streamed.
		/// </summary>
		public bool IsStreamed
		{
			get { return this.forceStream || (this.data != null && this.data.Length > 1024 * 100); }
		}

		/// <summary>
		/// Creates a new, empty AudioData without any data.
		/// </summary>
		public AudioData() { }
		/// <summary>
		/// Creates a new AudioData based on an Ogg Vorbis memory chunk.
		/// </summary>
		/// <param name="oggVorbisData">An Ogg Vorbis memory chunk</param>
		public AudioData(byte[] oggVorbisData)
		{
			this.data = oggVorbisData;
			this.SetupNativeBuffer();
		}
		/// <summary>
		/// Creates a new AudioData based on a <see cref="System.IO.Stream"/> containing Ogg Vorbis data.
		/// </summary>
		/// <param name="oggVorbisDataStream">A <see cref="System.IO.Stream"/> containing Ogg Vorbis data</param>
		public AudioData(Stream oggVorbisDataStream)
		{
			this.data = new byte[oggVorbisDataStream.Length];
			oggVorbisDataStream.Read(this.data, 0, (int)oggVorbisDataStream.Length);
			this.SetupNativeBuffer();
		}
		/// <summary>
		/// Creates a new AudioData base on an Ogg Vorbis file.
		/// </summary>
		/// <param name="filepath">Path to the Ogg Vorbis file.</param>
		public AudioData(string filepath)
		{
			this.LoadOggVorbisData(filepath);
		}

		/// <summary>
		/// Saves the audio data as Ogg Vorbis file.
		/// </summary>
		/// <param name="oggVorbisPath">The path of the file to which the audio data is written.</param>
		public void SaveOggVorbisData(string oggVorbisPath = null)
		{
			if (oggVorbisPath == null) oggVorbisPath = this.sourcePath;

			// We're saving this data for the first time
			if (!this.IsDefaultContent && this.sourcePath == null) this.sourcePath = oggVorbisPath;

			if (this.data != null)
				File.WriteAllBytes(oggVorbisPath, this.data);
			else
				File.WriteAllBytes(oggVorbisPath, Beep.Res.OggVorbisData);
		}
		/// <summary>
		/// Loads new audio data from an Ogg Vorbis file.
		/// </summary>
		/// <param name="oggVorbisPath">The path of the file from which the audio data is read.</param>
		public void LoadOggVorbisData(string oggVorbisPath = null)
		{
			if (oggVorbisPath == null) oggVorbisPath = this.sourcePath;

			this.sourcePath = oggVorbisPath;

			if (String.IsNullOrEmpty(this.sourcePath) || !File.Exists(this.sourcePath))
				this.data = null;
			else
				this.data = File.ReadAllBytes(this.sourcePath);

			this.DisposeNativeBuffer();
			this.SetupNativeBuffer();
		}
		
		/// <summary>
		/// Disposes the AudioDatas native buffer.
		/// </summary>
		/// <seealso cref="SetupNativeBuffer"/>
		private void DisposeNativeBuffer()
		{
			if (this.native != null)
			{
				this.native.Dispose();
				this.native = null;
			}
		}
		/// <summary>
		/// Sets up a new native buffer for this AudioData. This will result in decompressing
		/// the Ogg Vorbis data and uploading it to OpenAL, unless the AudioData is streamed.
		/// </summary>
		private void SetupNativeBuffer()
		{
			// No AudioData available
			if (this.data == null || this.data.Length == 0)
			{
				this.DisposeNativeBuffer();
				return;
			}

			// Streamed Audio
			if (this.IsStreamed)
			{
				this.DisposeNativeBuffer();
				this.native = null;
			}
			// Non-Streamed Audio
			else
			{
				if (this.native == null)
				{
					this.native = DualityApp.AudioBackend.CreateBuffer();

					PcmData pcm = OggVorbis.LoadFromMemory(this.data);
					this.native.LoadData(
						pcm.SampleRate,
						pcm.Data,
						pcm.DataLength,
						pcm.ChannelCount == 1 ? AudioDataLayout.Mono : AudioDataLayout.LeftRight,
						AudioDataElementType.Short);
				}
				else
				{
					// Buffer already there? Do nothing.
				}
			}
		}
		
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);

			// Dispose unmanages Resources
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
				this.DisposeNativeBuffer();

			// Get rid of the big data blob, so the GC can collect it.
			this.data = null;
		}
		protected override void OnLoaded()
		{
			base.OnLoaded();
			this.SetupNativeBuffer();
		}
	}
}
