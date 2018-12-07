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
		/// Uploads the specified pixel data in RGBA format to video memory. A call to <see cref="SetupEmpty"/>
		/// is to be considered required for this.
		/// </summary>
		/// <param name="format">The textures internal format.</param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="data">The block of pixel data to transfer.</param>
		/// <param name="dataLayout">The color layout of the specified data block.</param>
		/// <param name="dataElementType">The color element type of the specified data block.</param>
		void LoadData(
			TexturePixelFormat format, 
			int width, int height, 
			IntPtr data,
			ColorDataLayout dataLayout,
			ColorDataElementType dataElementType);

		/// <summary>
		/// Retrieves the textures pixel data from video memory in the Rgba8 format.
		/// 
		/// Note that generic, array-based variants of this method are available via extension method
		/// when using the Duality.Backend namespace.
		/// </summary>
		/// <param name="target">The buffer to store pixel values into.</param>
		/// <param name="dataLayout">The desired color layout of the specified buffer.</param>
		/// <param name="dataElementType">The desired color element type of the specified buffer.</param>
		void GetData(
			IntPtr target,
			ColorDataLayout dataLayout,
			ColorDataElementType dataElementType);
	}

	public static class ExtMethodsINativeTexture
	{
		/// <summary>
		/// Uploads the specified pixel data in RGBA format to video memory. A call to <see cref="INativeTexture.SetupEmpty"/>
		/// is to be considered required for this.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="texture"></param>
		/// <param name="format">The textures internal format.</param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="data">The block of pixel data to transfer.</param>
		/// <param name="dataLayout">The color layout of the specified data block.</param>
		/// <param name="dataElementType">The color element type of the specified data block.</param>
		public static void LoadData<T>(
			this INativeTexture texture,
			TexturePixelFormat format,
			int width, int height,
			T[] data,
			ColorDataLayout dataLayout,
			ColorDataElementType dataElementType) where T : struct
		{
			using (PinnedArrayHandle pinned = new PinnedArrayHandle(data))
			{
				texture.LoadData(
					format, 
					width, 
					height, 
					pinned.Address, 
					dataLayout, 
					dataElementType);
			}
		}
		/// <summary>
		/// Retrieves the textures pixel data from video memory in the Rgba8 format.
		/// As a storage array type, either byte or <see cref="ColorRgba"/> is recommended.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="target">The buffer to store pixel values into.</param>
		/// <param name="dataLayout">The desired color layout of the specified buffer.</param>
		/// <param name="dataElementType">The desired color element type of the specified buffer.</param>
		public static void GetData<T>(
			this INativeTexture texture,
			T[] target,
			ColorDataLayout dataLayout,
			ColorDataElementType dataElementType)
		{
			using (PinnedArrayHandle pinned = new PinnedArrayHandle(target))
			{
				texture.GetData(
					pinned.Address,
					dataLayout,
					dataElementType);
			}
		}
	}
}
