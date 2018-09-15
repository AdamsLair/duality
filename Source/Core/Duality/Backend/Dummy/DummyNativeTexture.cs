using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend.Dummy
{
	internal class DummyNativeTexture : INativeTexture
	{
		void INativeTexture.SetupEmpty(TexturePixelFormat format, int width, int height, TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapX, TextureWrapMode wrapY, int anisoLevel, bool mipmaps) { }
		void INativeTexture.LoadData(TexturePixelFormat format, int width, int height, IntPtr data, ColorDataLayout dataLayout, ColorDataElementType dataElementType) { }
		void INativeTexture.GetData(IntPtr target, ColorDataLayout dataLayout, ColorDataElementType dataElementType) { }
		void IDisposable.Dispose() { }
	}
}
