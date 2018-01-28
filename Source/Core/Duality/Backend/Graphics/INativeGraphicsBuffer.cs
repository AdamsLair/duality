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
		/// [GET] The length of the buffers available storage space, in bytes.
		/// </summary>
		int Length { get; }

		/// <summary>
		/// Sets up an empty storage with the specified size.
		/// </summary>
		/// <param name="size"></param>
		void SetupEmpty(int size);
		/// <summary>
		/// Uploads the specified data block into this buffer, replacing all previous contents.
		/// If not done before, this method will automatically allocate the required storage space.
		/// </summary>
		/// <param name="data">A pointer to the beginning of the data block to upload.</param>
		/// <param name="size">The size of the data block to upload, in bytes.</param>
		void LoadData(IntPtr data, int size);
		/// <summary>
		/// Uploads the specified data block into a subsection of this buffer, keeping all other content.
		/// </summary>
		/// <param name="offset">A memory offset from the beginning of the existing buffer.</param>
		/// <param name="data">A pointer to the beginning of the data block to upload.</param>
		/// <param name="size">The size of the data block to upload, in bytes.</param>
		void LoadSubData(IntPtr offset, IntPtr data, int size);
	}

	public static class ExtMethodsINativeVertexBuffer
	{
		/// <summary>
		/// Sets up an empty storage with the specified size, in number of array elements of the
		/// generic type of this method.
		/// </summary>
		/// <param name="count"></param>
		public static void SetupEmpty<T>(this INativeGraphicsBuffer buffer, int count) where T : struct
		{
			int itemSize = Marshal.SizeOf(typeof(T));
			buffer.SetupEmpty(count * itemSize);
		}
		/// <summary>
		/// Uploads the specified data block into this buffer, replacing all previous contents.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="buffer"></param>
		/// <param name="data">An array containing the data that will be uploaded.</param>
		/// <param name="index">The array starting index from which to start uploading data.</param>
		/// <param name="count">The number of elements from the array that should be uploaded.</param>
		public static void LoadData<T>(this INativeGraphicsBuffer buffer, T[] data, int index, int count) where T : struct
		{
			if (index < 0) throw new ArgumentOutOfRangeException("index", "Index cannot be negative");
			if (count < 0) throw new ArgumentOutOfRangeException("count", "Count cannot be negative");
			if (index + count > data.Length) throw new ArgumentOutOfRangeException("count", "Index + Count cannot exceed the specified arrays length.");

			int itemSize = Marshal.SizeOf(typeof(T));
			using (PinnedArrayHandle pinned = new PinnedArrayHandle(data))
			{
				IntPtr dataAddress = IntPtr.Add(pinned.Address, index * itemSize);
				buffer.LoadData(
					dataAddress,
					count * itemSize);
			}
		}
		/// <summary>
		/// Uploads the specified data block into a subsection of this buffer, keeping all other content.
		/// </summary>
		/// <param name="bufferIndex">The offset from the beginning of the existing buffer, as number of elements.</param>
		/// <param name="data">An array containing the data that will be uploaded.</param>
		/// <param name="index">The array starting index from which to start uploading data.</param>
		/// <param name="count">The number of elements from the array that should be uploaded.</param>
		public static void LoadSubData<T>(this INativeGraphicsBuffer buffer, int bufferIndex, T[] data, int index, int count) where T : struct
		{
			if (bufferIndex < 0) throw new ArgumentOutOfRangeException("bufferIndex", "Buffer index cannot be negative");
			if (index < 0) throw new ArgumentOutOfRangeException("index", "Index cannot be negative");
			if (count < 0) throw new ArgumentOutOfRangeException("count", "Count cannot be negative");
			if (index + count > data.Length) throw new ArgumentOutOfRangeException("count", "Index + Count cannot exceed the specified arrays length.");

			int itemSize = Marshal.SizeOf(typeof(T));
			using (PinnedArrayHandle pinned = new PinnedArrayHandle(data))
			{
				IntPtr dataAddress = IntPtr.Add(pinned.Address, index * itemSize);
				IntPtr bufferAddressOffset = IntPtr.Add(IntPtr.Zero, bufferIndex * itemSize);
				buffer.LoadSubData(
					bufferAddressOffset,
					dataAddress,
					count * itemSize);
			}
		}
	}
}
