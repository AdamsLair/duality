using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Duality.Drawing
{
	public interface IImageCodec
	{
		string CodecId { get; }
		int Priority { get; }

		bool CanReadFormat(string formatId);
		PixelData Read(Stream stream);

		bool CanWriteFormat(string formatId);
		void Write(Stream stream, PixelData data, string formatId);
	}
}
