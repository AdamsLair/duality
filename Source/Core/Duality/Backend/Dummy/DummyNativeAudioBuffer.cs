using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Audio;

namespace Duality.Backend.Dummy
{
	public class DummyNativeAudioBuffer : INativeAudioBuffer
	{
		void INativeAudioBuffer.LoadData<T>(int sampleRate, T[] data, int dataLength, AudioDataLayout dataLayout, AudioDataElementType dataElementType) { }
		void IDisposable.Dispose() { }
	}
}
