using System;
using System.IO;
using System.Collections.Generic;

namespace Duality.Backend
{
	public interface IAudioStreamProvider
	{
		void OpenStream();
		bool ReadStream(INativeAudioBuffer targetBuffer);
		void CloseStream();
	}
}
