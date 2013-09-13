using System;
using System.IO;
using System.Collections.Generic;

namespace Duality.OggVorbis
{
	public struct PcmData
	{
		public const int SizeOfDataElement = sizeof(short);

		public	short[]	data;
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
		private const int DefaultBufferSize = 1024 * 8;

		public static PcmData LoadFromFile(string filename)
		{
			PcmData pcm;
			VorbisStreamHandle handle;

			BeginStreamFromFile(filename, out handle);
			ReadAll(handle, out pcm);
			EndStream(ref handle);

			return pcm;
		}
		public static PcmData LoadFromMemory(byte[] memory)
		{
			PcmData pcm;
			VorbisStreamHandle handle;

			BeginStreamFromMemory(memory, out handle);
			ReadAll(handle, out pcm);
			EndStream(ref handle);

			return pcm;
		}
		public static PcmData LoadChunkFromMemory(byte[] memory, uint sampleCount)
		{
			PcmData pcm;
			VorbisStreamHandle handle;

			BeginStreamFromMemory(memory, out handle);
			StreamChunk(handle, out pcm, sampleCount);
			EndStream(ref handle);

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
		public static bool StreamChunk(VorbisStreamHandle handle, out PcmData pcm, uint bufferSize = DefaultBufferSize)
		{
			unsafe
			{
				void* vorbisFile = (void*)handle.VorbisInstance;
				int chnCount = 0;
				int smpRate = 0;
				int errHoleCount = 0;
				int errBadLinkCount = 0;
				bool eof = false;

				pcm.data = new short[bufferSize];
				pcm.dataLength = 0;
				while(pcm.dataLength < pcm.data.Length)
				{
					int pcmBytes;
					fixed (short* bufferPtr = &pcm.data[0])
					{
						pcmBytes = NativeMethods.ogg_decode_one_vorbis_packet(
							vorbisFile, 
							bufferPtr + pcm.dataLength, 
							(pcm.data.Length - pcm.dataLength) * PcmData.SizeOfDataElement,
							16,
							&chnCount, &smpRate,
							&errHoleCount, &errBadLinkCount);
					}

					if (pcmBytes > 0)
					{
						pcm.dataLength += pcmBytes / PcmData.SizeOfDataElement;
					}
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
		public static bool ReadAll(VorbisStreamHandle handle, out PcmData pcm)
		{
			pcm.channelCount = 0;
			pcm.sampleRate = 0;
			pcm.dataLength = 0;

			List<short[]> allBuffers = new List<short[]>();
			bool eof = false;
			int totalSamplesRead = 0;
			while (!eof)
			{
				short[] buffer = new short[DefaultBufferSize];
				int bufferSamplesRead = 0;
				unsafe
				{
					void* vorbisFile = (void*)handle.VorbisInstance;
					int chnCount = 0;
					int smpRate = 0;
					int errHoleCount = 0;
					int errBadLinkCount = 0;

					bufferSamplesRead = 0;
					while(bufferSamplesRead < buffer.Length)
					{
						int pcmBytes;
						fixed (short* bufferPtr = &buffer[0])
						{
							pcmBytes = NativeMethods.ogg_decode_one_vorbis_packet(
								vorbisFile, 
								bufferPtr + bufferSamplesRead, 
								(buffer.Length - bufferSamplesRead) * PcmData.SizeOfDataElement,
								16,
								&chnCount, &smpRate,
								&errHoleCount, &errBadLinkCount);
						}

						if (pcmBytes > 0)
						{
							bufferSamplesRead += pcmBytes / PcmData.SizeOfDataElement;
						}
						else
						{
							eof = true;
							break;
						}
					}

					if (bufferSamplesRead > 0)
					{
						pcm.channelCount = chnCount;
						pcm.sampleRate = smpRate;
					}
				}
				allBuffers.Add(buffer);
				totalSamplesRead += bufferSamplesRead;
			}
				
			if (totalSamplesRead > 0)
			{
				pcm.dataLength = totalSamplesRead;
				pcm.data = new short[totalSamplesRead];
				int offset = 0;
				for (int i = 0; i < allBuffers.Count; i++)
				{
					int len = Math.Min(pcm.data.Length - offset, allBuffers[i].Length);
					Array.Copy(allBuffers[i], 0, pcm.data, offset, len);
					offset += len;
				}
				return !eof;
			}
			else
			{
				pcm.data = new short[0];
				return false;
			}
		}
	}
}
