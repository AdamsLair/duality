using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend
{
	public interface INativeTexture : IDisposable
	{
		/// <summary>
		/// Initializes the texture without data and configures it.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="minFilter"></param>
		/// <param name="magFilter"></param>
		/// <param name="wrapX"></param>
		/// <param name="wrapY"></param>
		/// <param name="anisoLevel"></param>
		/// <param name="mipmaps"></param>
		void SetupEmpty(
			TexturePixelFormat format, 
			int width, int height, 
			TextureMinFilter minFilter, 
			TextureMagFilter magFilter, 
			TextureWrapMode wrapX, 
			TextureWrapMode wrapY, 
			int anisoLevel, 
			bool mipmaps);

		/// <summary>
		/// Uploads the specified pixel data to video memory. A call to <see cref="SetupEmpty"/>
		/// is to be considered required for this.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="data"></param>
		void LoadData(
			TexturePixelFormat format, 
			int width, int height, 
			ColorRgba[] data);

		/// <summary>
		/// Retrieves the textures pixel data from video memory in the Rgba8 format.
		/// As a storage array type, either byte or <see cref="ColorRgba"/> is recommended.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="target"></param>
		void GetData<T>(T[] target) where T : struct;
	}
}
