using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Duality.Drawing;

namespace Duality.Backend
{
	public interface INativeGraphicsBuffer : IDisposable
	{
		GraphicsBufferType BufferType { get; }

		void LoadData(IntPtr data, IntPtr size);
	}

	public static class ExtMethodsINativeVertexBuffer
	{
		public static void LoadData<T>(this INativeGraphicsBuffer buffer, T[] data, int count) where T : struct
		{
			int itemSize = Marshal.SizeOf(typeof(T));
			using (PinnedArrayHandle pinned = new PinnedArrayHandle(data))
			{
				buffer.LoadData(
					pinned.Address,
					(IntPtr)(count * itemSize));
			}
		}
	}
}
