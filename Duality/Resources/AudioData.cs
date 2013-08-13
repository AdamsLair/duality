using System;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality.OggVorbis;
using OpenTK.Audio.OpenAL;
using Duality.EditorHints;

namespace Duality.Resources
{
	/// <summary>
	/// Stores compressed audio data (Ogg Vorbis) in system memory as well as a reference to the
	/// OpenAL buffer containing actual PCM data, once set up. The OpenAL buffer is set up lazy
	/// i.e. as soon as demanded by accessing the AlBuffer property or calling SetupAlBuffer.
	/// </summary>
	/// <seealso cref="Duality.Resources.Sound"/>
	[Serializable]
	[ExplicitResourceReference()]
	public class AudioData : Resource
	{
		/// <summary>
		/// An AudioData resources file extension.
		/// </summary>
		public new const string FileExt = ".AudioData" + Resource.FileExt;
		
		/// <summary>
		/// [GET] A simple beep AudioData.
		/// </summary>
		public static ContentRef<AudioData> Beep		{ get; private set; }
		/// <summary>
		/// [GET] A drone loop AudioData. This is stereo data.
		/// </summary>
		public static ContentRef<AudioData> DroneLoop	{ get; private set; }
		/// <summary>
		/// [GET] A logo jingle AudioData. This is stereo data.
		/// </summary>
		public static ContentRef<AudioData> LogoJingle	{ get; private set; }

		internal static void InitDefaultContent()
		{
			const string VirtualContentPath		= ContentProvider.VirtualContentPath + "AudioData:";
			const string ContentPath_Beep		= VirtualContentPath + "Beep";
			const string ContentPath_DroneLoop	= VirtualContentPath + "DroneLoop";
			const string ContentPath_LogoJingle	= VirtualContentPath + "LogoJingle";

			ContentProvider.RegisterContent(ContentPath_Beep, new AudioData(DefaultRes.Beep));
			ContentProvider.RegisterContent(ContentPath_DroneLoop, new AudioData(DefaultRes.DroneLoop));
			ContentProvider.RegisterContent(ContentPath_LogoJingle, new AudioData(DefaultRes.LogoJingle));

			Beep		= ContentProvider.RequestContent<AudioData>(ContentPath_Beep);
			DroneLoop	= ContentProvider.RequestContent<AudioData>(ContentPath_DroneLoop);
			LogoJingle	= ContentProvider.RequestContent<AudioData>(ContentPath_LogoJingle);
		}
		
		
		/// <summary>
		/// A dummy OpenAL buffer handle, indicating that the buffer in question is not available.
		/// </summary>
		public const int AlBuffer_NotAvailable = 0;
		/// <summary>
		/// A dummy OpenAL buffer handle, indicating that the buffer in question is inactive due to streaming.
		/// </summary>
		public const int AlBuffer_StreamMe = -1;


		private	byte[]	data			= null;
		private	bool	forceStream		= false;
		[NonSerialized]	private	int	alBuffer	= AlBuffer_NotAvailable;

		/// <summary>
		/// [GET / SET] A data chunk representing <see cref="Duality.OggVorbis.OV">Ogg Vorbis</see> compressed
		/// audio data.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public byte[] OggVorbisData
		{
			get { return this.data; }
			set { this.data = value; this.DisposeAlBuffer(); }
		}
		/// <summary>
		/// [GET / SET] If set to true, when playing a <see cref="Duality.Resources.Sound"/> that refers to this
		/// AudioData, it is forced to be played streamed. Normally, streaming kicks in automatically when playing
		/// very large sound files, such as music or large environmental ambience.
		/// </summary>
		public bool ForceStream
		{
			get { return this.forceStream; }
			set { this.forceStream = value; this.DisposeAlBuffer(); }
		}
		/// <summary>
		/// [GET] Returns whether this AudioData will be played streamed.
		/// </summary>
		public bool IsStreamed
		{
			get { return this.forceStream || (this.data != null && this.data.Length > 1024 * 100); }
		}
		/// <summary>
		/// [GET] The OpenAL buffer handle of this AudioData.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		internal int AlBuffer
		{
			get 
			{ 
				if (this.alBuffer == AlBuffer_NotAvailable) this.SetupAlBuffer();
				return this.alBuffer;
			}
		}

		/// <summary>
		/// Creates a new, empty AudioData without any data.
		/// </summary>
		public AudioData() : this(Beep.Res.OggVorbisData.Clone() as byte[]) {}
		/// <summary>
		/// Creates a new AudioData based on an <see cref="Duality.OggVorbis.OV">Ogg Vorbis</see> memory chunk.
		/// </summary>
		/// <param name="oggVorbisData">An <see cref="Duality.OggVorbis.OV">Ogg Vorbis</see> memory chunk</param>
		public AudioData(byte[] oggVorbisData)
		{
			this.data = oggVorbisData;
		}
		/// <summary>
		/// Creates a new AudioData based on a <see cref="System.IO.Stream"/> containing <see cref="Duality.OggVorbis.OV">Ogg Vorbis</see> data.
		/// </summary>
		/// <param name="oggVorbisDataStream">A <see cref="System.IO.Stream"/> containing <see cref="Duality.OggVorbis.OV">Ogg Vorbis</see> data</param>
		public AudioData(Stream oggVorbisDataStream)
		{
			this.data = new byte[oggVorbisDataStream.Length];
			oggVorbisDataStream.Read(this.data, 0, (int)oggVorbisDataStream.Length);
		}
		/// <summary>
		/// Creates a new AudioData base on an <see cref="Duality.OggVorbis.OV">Ogg Vorbis</see> file.
		/// </summary>
		/// <param name="filepath">Path to the <see cref="Duality.OggVorbis.OV">Ogg Vorbis</see> file.</param>
		public AudioData(string filepath)
		{
			this.LoadOggVorbisData(filepath);
		}

		/// <summary>
		/// Saves the audio data as <see cref="Duality.OggVorbis.OV">Ogg Vorbis</see> file.
		/// </summary>
		/// <param name="oggVorbisPath">The path of the file to which the audio data is written.</param>
		public void SaveOggVorbisData(string oggVorbisPath = null)
		{
			if (oggVorbisPath == null) oggVorbisPath = this.sourcePath;

			// We're saving this data for the first time
			if (!this.path.Contains(':') && this.sourcePath == null) this.sourcePath = oggVorbisPath;

			if (this.data != null)
				File.WriteAllBytes(oggVorbisPath, this.data);
			else
				File.WriteAllBytes(oggVorbisPath, Beep.Res.OggVorbisData);
		}
		/// <summary>
		/// Loads new audio data from an <see cref="Duality.OggVorbis.OV">Ogg Vorbis</see> file.
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

			this.DisposeAlBuffer();
		}
		
		/// <summary>
		/// Disposes the AudioDatas OpenAL buffer.
		/// </summary>
		/// <seealso cref="SetupAlBuffer"/>
		public void DisposeAlBuffer()
		{
			if (this.alBuffer > AlBuffer_NotAvailable) AL.DeleteBuffer(this.alBuffer);
			this.alBuffer = AlBuffer_NotAvailable;
			return;
		}
		/// <summary>
		/// Sets up a new OpenAL buffer for this AudioData. This will result in decompressing
		/// the <see cref="Duality.OggVorbis.OV">Ogg Vorbis</see> data and uploading it to OpenAL,
		/// unless the AudioData is streamed.
		/// </summary>
		public void SetupAlBuffer()
		{
			// No AudioData available
			if (this.data.Length == 0 || this.data == null)
			{
				this.DisposeAlBuffer();
				return;
			}

			// Streamed Audio
			if (this.IsStreamed)
			{
				this.DisposeAlBuffer();
				this.alBuffer = AlBuffer_StreamMe;
			}
			// Non-Streamed Audio
			else
			{
				if (this.alBuffer <= AlBuffer_NotAvailable && DualityApp.Sound.IsAvailable)
				{
					this.alBuffer = AL.GenBuffer();
					PcmData pcm = OV.LoadFromMemory(this.data);
					AL.BufferData(
						this.alBuffer,
						pcm.channelCount == 1 ? ALFormat.Mono16 : ALFormat.Stereo16,
						pcm.data.ToArray(), 
						(int)pcm.data.Length, 
						pcm.sampleRate);
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
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
				this.DisposeAlBuffer();
		}

		protected override void OnCopyTo(Resource r, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(r, provider);
			AudioData c = r as AudioData;
			c.data	= (byte[])this.data.Clone();
		}
	}
}
