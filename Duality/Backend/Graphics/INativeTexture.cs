using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend
{
	public interface INativeTexture : IDisposable
	{
		void SetupEmpty(
			TexturePixelFormat format, 
			int width, int height, 
			TextureMinFilter minFilter, 
			TextureMagFilter magFilter, 
			TextureWrapMode wrapX, 
			TextureWrapMode wrapY, 
			int anisoLevel, 
			bool mipmaps);
		void LoadData(
			TexturePixelFormat format, 
			int width, int height, 
			ColorRgba[] data);
		void GetData<T>(T[] target) where T : struct;
	}
}
