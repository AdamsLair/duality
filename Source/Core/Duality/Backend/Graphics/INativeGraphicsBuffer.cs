using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Duality.Drawing;

namespace Duality.Backend
{
	/// <summary>
	/// Represents a GPU buffer for storing data such as vertices or indices.
	/// </summary>
	public interface INativeGraphicsBuffer : IDisposable
	{
		/// <summary>
		/// [GET] The kind of data that is stored in this buffer.
		/// </summary>
		GraphicsBufferType BufferType { get; }

		/// <summary>
		/// Uploads the specified data block into this buffer.
		/// </summary>
		/// <param name="data">A pointer to the beginning of the data block to upload.</param>
		/// <param name="size">The size of the data block to upload, in bytes.</param>
		void LoadData(IntPtr data, int size);
	}

	public static class ExtMethodsINativeVertexBuffer
	{
		/// <summary>
		/// Uploads the specified data block into this buffer.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="buffer"></param>
		/// <param name="data">An array containing the data that will be uploaded.</param>
		/// <param name="count">The number of elements from the array that should be uploaded.</param>
		public static void LoadData<T>(this INativeGraphicsBuffer buffer, T[] data, int count) where T : struct
		{
			int itemSize = Marshal.SizeOf(typeof(T));
			using (PinnedArrayHandle pinned = new PinnedArrayHandle(data))
			{
				buffer.LoadData(
					pinned.Address,
					count * itemSize);
			}
		}
	}
}
