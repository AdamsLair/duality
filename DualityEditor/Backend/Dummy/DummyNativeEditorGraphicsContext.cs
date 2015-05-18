using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Backend;

namespace Duality.Editor.Backend.Dummy
{
	internal class DummyNativeEditorGraphicsContext : INativeEditorGraphicsContext
	{
		INativeRenderableSite INativeEditorGraphicsContext.CreateRenderableSite()
		{
			return new DummyNativeRenderableSite();
		}
		void INativeEditorGraphicsContext.PerformBufferSwap() { }
		void IDisposable.Dispose() { }
	}
}
