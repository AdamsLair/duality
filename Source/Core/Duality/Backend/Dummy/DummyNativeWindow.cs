using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend.Dummy
{
	internal class DummyNativeWindow : INativeWindow
	{
		void IDisposable.Dispose() { }
		void INativeWindow.Run()
		{
			while (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
			{
				DualityApp.Update();
				DualityApp.Render(new Rect(DualityApp.UserData.WindowWidth, DualityApp.UserData.WindowHeight));
			}
		}
	}
}
