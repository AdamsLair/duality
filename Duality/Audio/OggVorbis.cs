using System;
using System.IO;
using System.Collections.Generic;

using NVorbis;
using NVorbis.Ogg;

namespace Duality.Audio
{
	public static class OggVorbis
	{
		private const int DefaultBufferSize = 1024 * 16;
		private static object readMutex = new object();

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
			pcm.DataLength = 0;
			pcm.ChannelCount = handle.VorbisInstance.Channels;
			pcm.SampleRate = handle.VorbisInstance.SampleRate;

			bool eof = false;
			float[] buffer = new float[bufferSize];
			while (pcm.DataLength < buffer.Length)
			{
				int samplesRead;
				lock (readMutex)
				{
					samplesRead = handle.VorbisInstance.ReadSamples(buffer, pcm.DataLength, buffer.Length - pcm.DataLength);
				}
				if (samplesRead > 0)
				{
					pcm.DataLength += samplesRead;
				}
				else
				{
					eof = true;
					break;
				}
			}

			pcm.Data = new short[pcm.DataLength];
			CastBuffer(buffer, pcm.Data, 0, pcm.DataLength);

			return pcm.DataLength > 0 && !eof;
		}
		public static bool ReadAll(VorbisStreamHandle handle, out PcmData pcm)
		{
			pcm.ChannelCount = handle.VorbisInstance.Channels;
			pcm.SampleRate = handle.VorbisInstance.SampleRate;
			pcm.DataLength = 0;

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
						samplesRead = handle.VorbisInstance.ReadSamples(buffer, pcm.DataLength, buffer.Length - pcm.DataLength);
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
				pcm.DataLength = totalSamplesRead;
				pcm.Data = new short[totalSamplesRead];
				int offset = 0;
				for (int i = 0; i < allBuffers.Count; i++)
				{
					int len = Math.Min(pcm.Data.Length - offset, allBuffers[i].Length);
					CastBuffer(allBuffers[i], pcm.Data, offset, len);
					offset += len;
				}
				return !eof;
			}
			else
			{
				pcm.Data = new short[0];
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
