using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Duality.Drawing
{
	public struct VertexArrayLock : IDisposable
	{
		private GCHandle handle;
		private IntPtr dataPtr;

		public IntPtr Address
		{
			get { return this.dataPtr; }
		}

		public VertexArrayLock(Array managedArray)
		{
			this.handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
			this.dataPtr = Marshal.UnsafeAddrOfPinnedArrayElement(managedArray, 0);
		}
		public void Dispose()
		{
			this.dataPtr = IntPtr.Zero;
			if (this.handle.IsAllocated)
				this.handle.Free();
		}
	}
}
