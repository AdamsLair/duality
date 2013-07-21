using System;
using System.IO;

namespace Duality.OggVorbis
{
	public struct PcmData
	{
		public	byte[]	data;
		public	int		channelCount;
		public	int		sampleRate;
	}

	public static class OV
	{
		public static PcmData LoadFromFile(string filename)
		{
			MemoryStream dataStream = null;
			PcmData result;
			IntPtr ovStream;

			BeginStreamFromFile(filename, out ovStream);
			while (StreamChunk(ovStream, ref dataStream, out result.channelCount, out result.sampleRate));
			EndStream(ref ovStream);

			// Return data
			result.data = (dataStream != null) ? dataStream.ToArray() : null;
			return result;
		}
		public static PcmData LoadFromMemory(byte[] memory, int maxPcmByteCount = 0)
		{
			MemoryStream dataStream = null;
			PcmData result;
			IntPtr ovStream;

			BeginStreamFromMemory(memory, out ovStream);
			while (StreamChunk(ovStream, ref dataStream, out result.channelCount, out result.sampleRate))
			{
				if (maxPcmByteCount != 0 && dataStream.Length > maxPcmByteCount) break;
			}
			EndStream(ref ovStream);

			// Return data
			result.data = (dataStream != null) ? dataStream.ToArray() : null;
			return result;
		}

		public static void BeginStreamFromFile(string filename, out IntPtr vFPtr)
		{
			vFPtr = IntPtr.Zero;

			unsafe
			{
				void* vorbisFile = null;
				NativeMethods.ErrorCode err = (NativeMethods.ErrorCode)NativeMethods.init_for_ogg_decode(filename, &vorbisFile);
				if (err != NativeMethods.ErrorCode.None)
				{
					throw new ApplicationException(
						String.Format("An OggVorbis error occurred: {0}", err));
				}
				vFPtr = (IntPtr)vorbisFile;
			}
		}
		public static void BeginStreamFromMemory(byte[] memory, out IntPtr vFPtr)
		{
			vFPtr = IntPtr.Zero;

			unsafe
			{
				void* vorbisFile = null;
				NativeMethods.ErrorCode err = (NativeMethods.ErrorCode)NativeMethods.memory_stream_for_ogg_decode(memory, memory.Length, &vorbisFile);
				if (err != NativeMethods.ErrorCode.None)
				{
					throw new ApplicationException(
						String.Format("An OggVorbis error occurred: {0}", err));
				}
				vFPtr = (IntPtr)vorbisFile;
			}
		}
		public static void EndStream(ref IntPtr vFPtr)
		{
			if (vFPtr != IntPtr.Zero)
			{
				unsafe { NativeMethods.final_ogg_cleanup((void*)vFPtr); }
				vFPtr = IntPtr.Zero;
			}
		}
		/// <summary>
		/// Streams a pcm chunk from an opened ogg vorbis file to the specified OpenAL buffer.
		/// </summary>
		/// <param name="alBufferId">OpenAL buffer id to stream to</param>
		/// <param name="vFPtr">Ogg Vorbis file handle</param>
		/// <returns>Returns false, if EOF is reached.</returns>
		public static bool StreamChunk(IntPtr vFPtr, ref MemoryStream pcmOutputStream, out int channelCount, out int sampleRate)
		{
			unsafe
			{
				void* vorbisFile = (void*)vFPtr;
				byte[] pcmBuffer = new byte[1024 * 32]; // was 1024 * 16
				int chnCount = 0;
				int smpRate = 0;
				int errHoleCount = 0;
				int errBadLinkCount = 0;
				int totalPcmSize = 0;
				bool eof = false;

				while(totalPcmSize < pcmBuffer.Length)
				{
					int pcmBytes;
					fixed(byte* buf = &pcmBuffer[0])
					{
						pcmBytes = NativeMethods.ogg_decode_one_vorbis_packet(
							vorbisFile, 
							buf + totalPcmSize, 
							pcmBuffer.Length - totalPcmSize,
							16,
							&chnCount, &smpRate,
							&errHoleCount, &errBadLinkCount);
					}

					if (pcmBytes > 0)
						totalPcmSize += pcmBytes;
					else
					{
						eof = true;
						break;
					}
				}

				if (totalPcmSize > 0)
				{
					channelCount = chnCount;
					sampleRate = smpRate;
					if (pcmOutputStream == null) pcmOutputStream = new MemoryStream(pcmBuffer.Length);
					pcmOutputStream.Write(pcmBuffer, 0, totalPcmSize);
					return !eof;
				}
				else
				{
					channelCount = 0;
					sampleRate = 0;
					return false;
				}
			}
		}
		public static bool StreamChunk(IntPtr vFPtr, out PcmData result)
		{
			MemoryStream dataStream = null;
			bool notEof = StreamChunk(vFPtr, ref dataStream, out result.channelCount, out result.sampleRate);
			result.data = dataStream != null ? dataStream.ToArray() : new byte[0];
			return notEof;
		}
	}
}
