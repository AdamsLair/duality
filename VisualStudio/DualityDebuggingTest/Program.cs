using System;
using System.Collections.Generic;

using Duality;
using Duality.Backend;
using Duality.Resources;

namespace Duality.VisualStudio
{
	public static class DualityDebuggingTester 
	{
		[STAThread]
		public static void Main()
		{
			DualityApp.Init(
				DualityApp.ExecutionEnvironment.Launcher, 
				DualityApp.ExecutionContext.Game, 
				null,
				null);
			
			WindowOptions options = new WindowOptions
			{
				Width = 800,
				Height = 600
			};
			using (INativeWindow launcherWindow = DualityApp.OpenWindow(options))
			{
				// Run tests
				BitmapDebuggerVisualizer.TestShow(Pixmap.DualityIcon.Res);
				BitmapDebuggerVisualizer.TestShow(Pixmap.DualityIcon.Res.MainLayer);
				BitmapDebuggerVisualizer.TestShow(Texture.DualityIcon.Res);
				BitmapDebuggerVisualizer.TestShow(Font.GenericMonospace10.Res.Material.MainTexture.Res);
			}
			DualityApp.Terminate();
		}
	}
}
