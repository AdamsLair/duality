using System;
using System.IO;
using System.Collections.Generic;

using NVorbis;
using NVorbis.Ogg;

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
		private VorbisReader ovStream;

		public bool Disposed
		{
			get { return this.disposed; }
		}
		internal VorbisReader VorbisInstance
		{
			get { return this.ovStream; }
		}
		
		internal VorbisStreamHandle(byte[] memory)
		{
			this.ovStream = new VorbisReader(new MemoryStream(memory), true);
		}
		internal VorbisStreamHandle(string fileName)
		{
			this.ovStream = new VorbisReader(fileName);
		}

		~VorbisStreamHandle()
		{
			this.Dispose(false);
		}
		private void Dispose(bool manually)
		{
			if (this.disposed) return;
			
			if (this.ovStream != null)
			{
				this.ovStream.Dispose();
				this.ovStream = null;
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
		private const int DefaultBufferSize = 1024 * 16;
		private static object readMutex = new object();

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
			pcm.dataLength = 0;
			pcm.channelCount = handle.VorbisInstance.Channels;
			pcm.sampleRate = handle.VorbisInstance.SampleRate;

			bool eof = false;
			float[] buffer = new float[bufferSize];
			while (pcm.dataLength < buffer.Length)
			{
				int samplesRead;
				lock (readMutex)
				{
					samplesRead = handle.VorbisInstance.ReadSamples(buffer, pcm.dataLength, buffer.Length - pcm.dataLength);
				}
				if (samplesRead > 0)
				{
					pcm.dataLength += samplesRead;
				}
				else
				{
					eof = true;
					break;
				}
			}

			pcm.data = new short[pcm.dataLength];
			CastBuffer(buffer, pcm.data, 0, pcm.dataLength);

			return pcm.dataLength > 0 && !eof;
		}
		public static bool ReadAll(VorbisStreamHandle handle, out PcmData pcm)
		{
			pcm.channelCount = handle.VorbisInstance.Channels;
			pcm.sampleRate = handle.VorbisInstance.SampleRate;
			pcm.dataLength = 0;

			List<float[]> allBuffers = new List<float[]>();
			bool eof = false;
			int totalSamplesRead = 0;
			while (!eof)
			{
				float[] buffer = new float[DefaultBufferSize];
				int bufferSamplesRead = 0;
				while(bufferSamplesRead < buffer.Length)
				{
					int samplesRead;
					lock (readMutex)
					{
						samplesRead = handle.VorbisInstance.ReadSamples(buffer, pcm.dataLength, buffer.Length - pcm.dataLength);
					}
					if (samplesRead > 0)
					{
						bufferSamplesRead += samplesRead;
					}
					else
					{
						eof = true;
						break;
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
					CastBuffer(allBuffers[i], pcm.data, offset, len);
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
        private static void CastBuffer(float[] source, short[] target, int targetOffset, int length)
        {
            for (int i = 0; i < length; i++)
            {
                var temp = (int)(32767f * source[i]);
                if (temp > short.MaxValue) temp = short.MaxValue;
                else if (temp < short.MinValue) temp = short.MinValue;
                target[targetOffset + i] = (short)temp;
            }
        }
	}
}
