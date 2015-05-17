using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using OpenTK.Audio.OpenAL;

using Duality.Audio;

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class NativeAudioBuffer : INativeAudioBuffer
	{
		private int handle;
		public int Handle
		{
			get { return this.handle; }
		}

		public NativeAudioBuffer()
		{
			this.handle = AL.GenBuffer();
		}
		void INativeAudioBuffer.LoadData<T>(int sampleRate, T[] data, int dataLength, AudioDataLayout dataLayout, AudioDataElementType dataElementType)
		{
			throw new NotImplementedException();
		}
		void IDisposable.Dispose()
		{
			if (this.handle != 0)
			{
				AL.DeleteBuffer(this.handle);
				this.handle = 0;
			}
		}
	}
}
