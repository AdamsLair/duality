using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Backend;

namespace Duality.Editor.Backend.Dummy
{
	internal class DummyNativeEditorGraphicsContext : INativeEditorGraphicsContext
	{
		private INativeWindow dummyWindow = null;

		public DummyNativeEditorGraphicsContext()
		{
			// Since the graphics backend probably needs some kind of graphics context, create an invisible dummy window.
			this.dummyWindow = DualityApp.GraphicsBackend.CreateWindow(new WindowOptions
			{
				Size = new Point2(640, 480),
				ScreenMode = ScreenMode.Window,
				RefreshMode = RefreshMode.NoSync
			});
		}
		INativeRenderableSite INativeEditorGraphicsContext.CreateRenderableSite()
		{
			return new DummyNativeRenderableSite();
		}
		void INativeEditorGraphicsContext.PerformBufferSwap() { }
		void IDisposable.Dispose()
		{
			if (this.dummyWindow != null)
			{
				this.dummyWindow.Dispose();
				this.dummyWindow = null;
			}
		}
	}
}
