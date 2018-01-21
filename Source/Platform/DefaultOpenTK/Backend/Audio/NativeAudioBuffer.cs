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
		void INativeAudioBuffer.LoadData(int sampleRate, IntPtr data, IntPtr size, AudioDataLayout dataLayout, AudioDataElementType dataElementType)
		{
			ALFormat format = ALFormat.Mono16;
			if (dataLayout == AudioDataLayout.Mono)
			{
				if (dataElementType == AudioDataElementType.Byte)
					format = ALFormat.Mono8;
				else if (dataElementType == AudioDataElementType.Short)
					format = ALFormat.Mono16;
			}
			else if (dataLayout == AudioDataLayout.LeftRight)
			{
				if (dataElementType == AudioDataElementType.Byte)
					format = ALFormat.Stereo8;
				else if (dataElementType == AudioDataElementType.Short)
					format = ALFormat.Stereo16;
			}

			AL.BufferData(
				this.handle,
				format,
				data, 
				(int)size, 
				sampleRate);
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
