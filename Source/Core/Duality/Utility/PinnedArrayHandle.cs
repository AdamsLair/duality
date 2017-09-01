using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Duality
{
	/// <summary>
	/// Allows to temporarily pin a managed <see cref="Array"/> in memory and access its
	/// data directly using an <see cref="IntPtr"/>. Every <see cref="PinnedArrayHandle"/>
	/// that is created needs to be disposed as well, or the pinned <see cref="Array"/>
	/// cannot be garbage collected. To ensure safe usage, wrap the handle in a using block.
	/// </summary>
	public struct PinnedArrayHandle : IDisposable
	{
		private GCHandle handle;
		private IntPtr dataPtr;

		/// <summary>
		/// [GET] The address at which the array data block is available.
		/// </summary>
		public IntPtr Address
		{
			get { return this.dataPtr; }
		}

		/// <summary>
		/// Pins the specified <see cref="Array"/> in memory to allow direct data
		/// access to its elements using an <see cref="IntPtr"/>. Make sure to <see cref="Dispose"/>
		/// the handle after usage, ideally by wrapping it in a using block.
		/// </summary>
		/// <param name="managedArray"></param>
		public PinnedArrayHandle(Array managedArray)
		{
			this.handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
			this.dataPtr = Marshal.UnsafeAddrOfPinnedArrayElement(managedArray, 0);
		}
		/// <summary>
		/// Allows the <see cref="Array"/> to be moved or collected by the GC again.
		/// </summary>
		public void Dispose()
		{
			this.dataPtr = IntPtr.Zero;
			if (this.handle.IsAllocated)
				this.handle.Free();
		}
	}
}
