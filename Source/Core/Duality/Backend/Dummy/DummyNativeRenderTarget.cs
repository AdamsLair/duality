using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend.Dummy
{
	internal class DummyNativeRenderTarget : INativeRenderTarget
	{
		void INativeRenderTarget.Setup(IReadOnlyList<INativeTexture> targets, AAQuality multisample, bool depthBuffer) { }
		void INativeRenderTarget.GetData(IntPtr buffer, ColorDataLayout dataLayout, ColorDataElementType dataElementType, int targetIndex, int x, int y, int width, int height) { }
		void IDisposable.Dispose() { }
	}
}
