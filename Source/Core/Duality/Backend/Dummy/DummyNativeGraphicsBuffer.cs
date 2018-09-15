using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend.Dummy
{
	internal class DummyNativeGraphicsBuffer : INativeGraphicsBuffer
	{
		private GraphicsBufferType type;

		GraphicsBufferType INativeGraphicsBuffer.BufferType
		{
			get { return this.type; }
		}
		int INativeGraphicsBuffer.Length
		{
			get { return 0; }
		}

		public DummyNativeGraphicsBuffer(GraphicsBufferType type)
		{
			this.type = type;
		}

		void INativeGraphicsBuffer.SetupEmpty(int size) { }
		void INativeGraphicsBuffer.LoadData(IntPtr data, int size) { }
		void INativeGraphicsBuffer.LoadSubData(IntPtr offset, IntPtr data, int size) { }
		void IDisposable.Dispose() { }
	}
}
