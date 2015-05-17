using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend.Dummy
{
	internal class DummyNativeTexture : INativeTexture
	{
		void INativeTexture.SetupEmpty(TexturePixelFormat format, int width, int height, TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapX, TextureWrapMode wrapY, int anisoLevel, bool mipmaps) { }
		void INativeTexture.LoadData<T>(TexturePixelFormat format, int width, int height, T[] data, ColorDataLayout dataLayout, ColorDataElementType dataElementType) { }
		void INativeTexture.GetData<T>(T[] target, ColorDataLayout dataLayout, ColorDataElementType dataElementType)
		{
			for (int i = 0; i < target.Length; i++)
			{
				target[i] = default(T);
			}
		}
		void IDisposable.Dispose() { }
	}
}
