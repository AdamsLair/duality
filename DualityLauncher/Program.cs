using System;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;

using Duality;
using Duality.Resources;
using Duality.Profiling;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform.Windows;

namespace DualityLauncher
{
	public class DualityLauncher : GameWindow
	{
		private	static bool	isDebugging			= false;
		private	static bool	isProfiling			= false;
		private	static bool	isRunFromEditor		= false;
		private Stopwatch		frameLimiterWatch	= new Stopwatch();
		private	WinMMJoystick	mainJoystickDriver	= null;

		public DualityLauncher(int w, int h, GraphicsMode mode, string title, GameWindowFlags flags)
			: base(w, h, mode, title, flags)
		{
		}

		protected override void OnResize(EventArgs e)
		{
			DualityApp.TargetResolution = new Vector2(Width, Height);
		}
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated)
			{
				this.Close();
				return;
			}
			
			if (!isDebugging && !isProfiling) // Don't limit frame rate when debugging or profiling.
			{
				//// Assure we'll at least wait 16 ms until updating again.
				//if (this.frameLimiterWatch.IsRunning)
				//{
				//    while (this.frameLimiterWatch.Elapsed.TotalSeconds < 0.016d) 
				//    {
				//        // Go to sleep if we'd have to wait too long
				//        if (this.frameLimiterWatch.Elapsed.TotalSeconds < 0.01d)
				//            System.Threading.Thread.Sleep(1);
				//    }
				//}

				// Give the processor a rest if we have the time, don't use 100% CPU even without VSync
				if (this.frameLimiterWatch.IsRunning && this.VSync == VSyncMode.Off)
				{
					while (this.frameLimiterWatch.Elapsed.TotalMilliseconds < Time.MsPFMult)
					{
						// Enough leftover time? Risk a millisecond sleep.
						if (this.frameLimiterWatch.Elapsed.TotalMilliseconds < Time.MsPFMult * 0.75f)
							System.Threading.Thread.Sleep(1);
					}
				}
				this.frameLimiterWatch.Restart();
			}
			this.mainJoystickDriver.Poll();
			DualityApp.Update();
		}
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return;

			DualityApp.Render();
			Profile.TimeRender.BeginMeasure();
			Profile.TimeSwapBuffers.BeginMeasure();
			this.SwapBuffers();
			Profile.TimeSwapBuffers.EndMeasure();
			Profile.TimeRender.EndMeasure();
		}

		private void SetMouseDeviceX(int x)
		{
			if (!this.Focused) return;
			Point curPos;
			NativeMethods.GetCursorPos(out curPos);
			Point targetPos = this.PointToScreen(new Point(x, this.PointToClient(curPos).Y));
			NativeMethods.SetCursorPos(targetPos.X, targetPos.Y);
			return;
		}
		private void SetMouseDeviceY(int y)
		{
			if (!this.Focused) return;
			Point curPos;
			NativeMethods.GetCursorPos(out curPos);
			Point targetPos = this.PointToScreen(new Point(this.PointToClient(curPos).X, y));
			NativeMethods.SetCursorPos(targetPos.X, targetPos.Y);
			return;
		}

		[STAThread]
		public static void Main(string[] args)
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
			System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

			isDebugging = System.Diagnostics.Debugger.IsAttached || args.Contains(DualityApp.CmdArgDebug);
			isRunFromEditor = args.Contains(DualityApp.CmdArgEditor);
			isProfiling = args.Contains(DualityApp.CmdArgProfiling);
			if (isDebugging || isRunFromEditor) ShowConsole();

			DualityApp.Init(DualityApp.ExecutionEnvironment.Launcher, DualityApp.ExecutionContext.Game, args);

			using (DualityLauncher launcherWindow = new DualityLauncher(
				DualityApp.UserData.GfxWidth, 
				DualityApp.UserData.GfxHeight, 
				DualityApp.DefaultMode, 
				DualityApp.AppData.AppName,
				(DualityApp.UserData.GfxMode == ScreenMode.Fullscreen && !isDebugging) ? GameWindowFlags.Fullscreen : GameWindowFlags.Default))
			{
				// Retrieve icon from executable file and set it as window icon
				string executablePath = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
				launcherWindow.Icon = System.Drawing.Icon.ExtractAssociatedIcon(executablePath);

				// Go into native fullscreen mode
				if (DualityApp.UserData.GfxMode == ScreenMode.Native && !isDebugging)
					launcherWindow.WindowState = WindowState.Fullscreen;

				if (DualityApp.UserData.GfxMode == ScreenMode.FixedWindow)
					launcherWindow.WindowBorder = WindowBorder.Fixed;
				else if (DualityApp.UserData.GfxMode == ScreenMode.Window)
					launcherWindow.WindowBorder = WindowBorder.Resizable;

				// Initialize default content
				launcherWindow.MakeCurrent();
				DualityApp.TargetResolution = new Vector2(launcherWindow.Width, launcherWindow.Height);
				DualityApp.TargetMode = launcherWindow.Context.GraphicsMode;
				ContentProvider.InitDefaultContent();

				// Input setup
				DualityApp.Mouse.Source = new OpenTKMouseInputSource(launcherWindow.Mouse, launcherWindow.SetMouseDeviceX, launcherWindow.SetMouseDeviceY);
				DualityApp.Keyboard.Source = new OpenTKKeyboardInputSource(launcherWindow.Keyboard);
				{
					// Initialize Joystick manually, since launcherWindow.Joysticks doesn't work for some reason
					launcherWindow.mainJoystickDriver = new OpenTK.Platform.Windows.WinMMJoystick();
					if (launcherWindow.mainJoystickDriver != null && launcherWindow.mainJoystickDriver.Joysticks != null)
						DualityApp.Joysticks.AddSource(launcherWindow.mainJoystickDriver.Joysticks.Select(j => new OpenTKJoystickInputSource(j)));
					//DualityApp.Joysticks.AddSource(launcherWindow.Joysticks.Select(j => new OpenTKJoystickInputSource(j)));
				}

				// Load the starting Scene
				Scene.Current = DualityApp.AppData.StartScene.Res;

				// Run the DualityApp
				launcherWindow.CursorVisible = isDebugging || DualityApp.UserData.SystemCursorVisible;
				launcherWindow.VSync = (isProfiling || isDebugging) ? VSyncMode.Off : VSyncMode.On; // Don't limit frame rate when debugging.
				launcherWindow.Run();

				// Shut down the DualityApp
				DualityApp.Terminate();
			}
		}

		private static bool hasConsole = false;
		public static void ShowConsole()
		{
			if (hasConsole) return;
			NativeMethods.AllocConsole();
			hasConsole = true;
		}
	}
}
