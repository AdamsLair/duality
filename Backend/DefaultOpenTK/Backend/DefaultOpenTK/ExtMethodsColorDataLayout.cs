using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

using OpenTK.Graphics.OpenGL;

namespace Duality.Backend.DefaultOpenTK
{
	public static class ExtMethodColorDataLayout
	{
		public static PixelFormat ToOpenTK(this ColorDataLayout layout)
		{
			switch (layout)
			{
				default:
				case ColorDataLayout.Rgba: return PixelFormat.Rgba;
				case ColorDataLayout.Bgra: return PixelFormat.Bgra;
			}
		}
	}
}
