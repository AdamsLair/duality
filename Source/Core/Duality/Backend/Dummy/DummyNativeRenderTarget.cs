using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend.Dummy
{
	internal class DummyNativeRenderTarget : INativeRenderTarget
	{
		void INativeRenderTarget.Setup(IReadOnlyList<INativeTexture> targets, AAQuality multisample) { }
		void INativeRenderTarget.GetData<T>(T[] buffer, ColorDataLayout dataLayout, ColorDataElementType dataElementType, int targetIndex, int x, int y, int width, int height)
		{
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = default(T);
			}
		}
		void IDisposable.Dispose() { }
	}
}
