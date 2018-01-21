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

		public DummyNativeGraphicsBuffer(GraphicsBufferType type)
		{
			this.type = type;
		}

		void INativeGraphicsBuffer.LoadData(IntPtr data, IntPtr size) { }
		void IDisposable.Dispose() { }
	}
}
