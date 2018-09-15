using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Duality.Audio;

namespace Duality.Backend
{
	public interface INativeAudioBuffer : IDisposable
	{
		void LoadData(
			int sampleRate,
			IntPtr data,
			int size,
			AudioDataLayout dataLayout,
			AudioDataElementType dataElementType);
	}

	public static class ExtMethodsINativeAudioBuffer
	{
		public static void LoadData<T>(
			this INativeAudioBuffer buffer,
			int sampleRate,
			T[] data,
			int dataLength,
			AudioDataLayout dataLayout,
			AudioDataElementType dataElementType) where T : struct
		{
			if (dataLength < 0) throw new ArgumentOutOfRangeException("dataLength", "Data length cannot be negative");
			if (dataLength > data.Length) throw new ArgumentOutOfRangeException("dataLength", "Data length cannot exceed the specified arrays length.");

			int itemSize = Marshal.SizeOf(typeof(T));
			using (PinnedArrayHandle pinned = new PinnedArrayHandle(data))
			{
				buffer.LoadData(
					sampleRate,
					pinned.Address,
					itemSize * dataLength,
					dataLayout,
					dataElementType);
			}
		}
	}
}
