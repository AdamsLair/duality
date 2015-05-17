using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

using OpenTK.Graphics.OpenGL;

namespace Duality.Backend.DefaultOpenTK
{
	public static class ExtMethodColorDataElementType
	{
		public static PixelType ToOpenTK(this ColorDataElementType type)
		{
			switch (type)
			{
				default:
				case ColorDataElementType.Byte: return PixelType.UnsignedByte;
				case ColorDataElementType.Float: return PixelType.Float;
			}
		}
	}
}
