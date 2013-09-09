using System;
using System.IO;

namespace Duality.OggVorbis
{
	public struct PcmData
	{
		public	byte[]	data;
		public	int		dataLength;
		public	int		channelCount;
		public	int		sampleRate;
	}

	public class VorbisStreamHandle : IDisposable
	{
		private bool disposed;
		private IntPtr ovStream;

		public bool Disposed
		{
			get { return this.disposed; }
		}
		internal IntPtr VorbisInstance
		{
			get { return this.ovStream; }
		}

		internal VorbisStreamHandle(byte[] memory)
		{
			this.ovStream = IntPtr.Zero;
			unsafe
			{
				void* vorbisFile = null;
				NativeMethods.ErrorCode err = (NativeMethods.ErrorCode)NativeMethods.memory_stream_for_ogg_decode(memory, memory.Length, &vorbisFile);
				if (err != NativeMethods.ErrorCode.None)
				{
					throw new ApplicationException(
						String.Format("An OggVorbis error occurred: {0}", err));
				}
				this.ovStream = (IntPtr)vorbisFile;
			}
		}
		internal VorbisStreamHandle(string fileName)
		{
			this.ovStream = IntPtr.Zero;
			unsafe
			{
				void* vorbisFile = null;
				NativeMethods.ErrorCode err = (NativeMethods.ErrorCode)NativeMethods.init_for_ogg_decode(fileName, &vorbisFile);
				if (err != NativeMethods.ErrorCode.None)
				{
					throw new ApplicationException(
						String.Format("An OggVorbis error occurred: {0}", err));
				}
				this.ovStream = (IntPtr)vorbisFile;
			}
		}

		~VorbisStreamHandle()
		{
			this.Dispose(false);
		}
		private void Dispose(bool manually)
		{
			if (this.disposed) return;
			
			if (this.ovStream != IntPtr.Zero)
			{
				unsafe { NativeMethods.final_ogg_cleanup((void*)this.ovStream); }
				this.ovStream = IntPtr.Zero;
			}

			this.disposed = true;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

	public static class OV
	{
		public static PcmData LoadFromFile(string filename)
		{
			MemoryStream dataStream = new MemoryStream();
			PcmData pcm;
			VorbisStreamHandle handle;

			BeginStreamFromFile(filename, out handle);
			while (StreamChunk(handle, ref dataStream, out pcm)) {}
			EndStream(ref handle);

			// Return data
			pcm.data = (dataStream != null) ? dataStream.ToArray() : null;
			return pcm;
		}
		public static PcmData LoadFromMemory(byte[] memory, int maxPcmByteCount = 0)
		{
			MemoryStream dataStream = new MemoryStream();
			PcmData pcm;
			VorbisStreamHandle handle;

			BeginStreamFromMemory(memory, out handle);
			while (StreamChunk(handle, ref dataStream, out pcm)) {}
			EndStream(ref handle);

			// Return data
			pcm.data = (dataStream != null) ? dataStream.ToArray() : null;
			return pcm;
		}

		public static void BeginStreamFromFile(string filename, out VorbisStreamHandle handle)
		{
			handle = new VorbisStreamHandle(filename);
		}
		public static void BeginStreamFromMemory(byte[] memory, out VorbisStreamHandle handle)
		{
			handle = new VorbisStreamHandle(memory);
		}
		public static bool IsStreamValid(VorbisStreamHandle handle)
		{
			return handle != null && !handle.Disposed;
		}
		public static void EndStream(ref VorbisStreamHandle handle)
		{
			if (handle != null)
			{
				handle.Dispose();
				handle = null;
			}
		}
		/// <summary>
		/// Streams a pcm chunk from an opened ogg vorbis file to the specified OpenAL buffer.
		/// </summary>
		/// <param name="alBufferId">OpenAL buffer id to stream to</param>
		/// <param name="vFPtr">Ogg Vorbis file handle</param>
		/// <returns>Returns false, if EOF is reached.</returns>
		public static bool StreamChunk(VorbisStreamHandle handle, ref MemoryStream pcmOutputStream, out PcmData pcm)
		{
			unsafe
			{
				void* vorbisFile = (void*)handle.VorbisInstance;
				int chnCount = 0;
				int smpRate = 0;
				int errHoleCount = 0;
				int errBadLinkCount = 0;
				bool eof = false;

				pcm.data = new byte[1024 * 32]; // was 1024 * 16
				pcm.dataLength = 0;
				while(pcm.dataLength < pcm.data.Length)
				{
					int pcmBytes;
					fixed (byte* buf = &pcm.data[0])
					{
						pcmBytes = NativeMethods.ogg_decode_one_vorbis_packet(
							vorbisFile, 
							buf + pcm.dataLength, 
							pcm.data.Length - pcm.dataLength,
							16,
							&chnCount, &smpRate,
							&errHoleCount, &errBadLinkCount);
					}

					if (pcmBytes > 0)
						pcm.dataLength += pcmBytes;
					else
					{
						eof = true;
						break;
					}
				}

				if (pcm.dataLength > 0)
				{
					pcm.channelCount = chnCount;
					pcm.sampleRate = smpRate;
					if (pcmOutputStream == null) pcmOutputStream = new MemoryStream(pcm.data.Length);
					pcmOutputStream.Write(pcm.data, 0, pcm.dataLength);
					return !eof;
				}
				else
				{
					pcm.channelCount = 0;
					pcm.sampleRate = 0;
					return false;
				}
			}
		}
		public static bool StreamChunk(VorbisStreamHandle handle, out PcmData result)
		{
			MemoryStream dataStream = null;
			bool notEof = StreamChunk(handle, ref dataStream, out result);
			result.data = dataStream != null ? dataStream.ToArray() : null;
			return notEof;
		}
	}
}
