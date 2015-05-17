using System;
using System.IO;
using System.Collections.Generic;

namespace Duality.Audio
{
	public struct PcmData
	{
		public const int SizeOfDataElement = sizeof(short);

		public	short[]	data;
		public	int		dataLength;
		public	int		channelCount;
		public	int		sampleRate;
	}
}
