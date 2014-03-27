using System;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality.Editor;
using Duality.Properties;

using OpenTK.Audio.OpenAL;


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
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategorySound)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageAudioData)]
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

		internal static void InitDefaultContent()
		{
			const string VirtualContentPath		= ContentProvider.VirtualContentPath + "AudioData:";
			const string ContentPath_Beep		= VirtualContentPath + "Beep";

			ContentProvider.AddContent(ContentPath_Beep, new AudioData(DefaultContent.Beep));

			Beep = ContentProvider.RequestContent<AudioData>(ContentPath_Beep);
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
		/// [GET / SET] A data chunk representing Ogg Vorbis compressed
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
		/// Creates a new AudioData based on an Ogg Vorbis memory chunk.
		/// </summary>
		/// <param name="oggVorbisData">An Ogg Vorbis memory chunk</param>
		public AudioData(byte[] oggVorbisData)
		{
			this.data = oggVorbisData;
		}
		/// <summary>
		/// Creates a new AudioData based on a <see cref="System.IO.Stream"/> containing Ogg Vorbis data.
		/// </summary>
		/// <param name="oggVorbisDataStream">A <see cref="System.IO.Stream"/> containing Ogg Vorbis data</param>
		public AudioData(Stream oggVorbisDataStream)
		{
			this.data = new byte[oggVorbisDataStream.Length];
			oggVorbisDataStream.Read(this.data, 0, (int)oggVorbisDataStream.Length);
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
		/// the Ogg Vorbis data and uploading it to OpenAL, unless the AudioData is streamed.
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
					PcmData pcm = OggVorbis.LoadFromMemory(this.data);
					AL.BufferData(
						this.alBuffer,
						pcm.channelCount == 1 ? ALFormat.Mono16 : ALFormat.Stereo16,
						pcm.data.ToArray(), 
						pcm.dataLength * PcmData.SizeOfDataElement, 
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

			// Dispose unmanages Resources
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
				this.DisposeAlBuffer();

			// Get rid of the big data blob, so the GC can collect it.
			this.data = null;
		}

		protected override void OnCopyTo(Resource r, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(r, provider);
			AudioData c = r as AudioData;
			c.data	= (byte[])this.data.Clone();
		}
	}
}
