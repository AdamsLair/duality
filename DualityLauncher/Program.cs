using System;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;

using Duality;
using Duality.Resources;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Platform.Windows;

namespace Duality.Launcher
{
	public class DualityLauncher : GameWindow
	{
		private	static bool	isDebugging			= false;
		private	static bool	isProfiling			= false;
		private	static bool	isRunFromEditor		= false;
		private Stopwatch	frameLimiterWatch	= new Stopwatch();

		public DualityLauncher(int w, int h, GraphicsMode mode, string title, GameWindowFlags flags) : base(w, h, mode, title, flags) {}

		protected override void OnResize(EventArgs e)
		{
			DualityApp.TargetResolution = new Vector2(ClientSize.Width, ClientSize.Height);
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
				if (this.frameLimiterWatch.IsRunning && DualityApp.UserData.RefreshMode == RefreshMode.ManualSync)
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
			DualityApp.Update();
		}
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return;

			DualityApp.Render(new Rect(this.ClientSize.Width, this.ClientSize.Height));
			Profile.TimeRender.BeginMeasure();
			Profile.TimeSwapBuffers.BeginMeasure();
			this.SwapBuffers();
			Profile.TimeSwapBuffers.EndMeasure();
			Profile.TimeRender.EndMeasure();
		}
		
		private void OnUserDataChanged(object sender, EventArgs eventArgs)
		{
			switch (DualityApp.UserData.GfxMode)
			{
				case ScreenMode.Window:
				case ScreenMode.FixedWindow:
					this.WindowState = WindowState.Normal;
					this.WindowBorder = WindowBorder.Fixed;
					this.ClientSize = new Size(DualityApp.UserData.GfxWidth, DualityApp.UserData.GfxHeight);
					DisplayDevice.Default.RestoreResolution();
					break;

				case ScreenMode.FullWindow:
					this.ClientSize = new Size(DisplayDevice.Default.Width, DisplayDevice.Default.Height);
					this.WindowState = WindowState.Fullscreen;
					this.WindowBorder = WindowBorder.Hidden;
					DisplayDevice.Default.RestoreResolution();
					break;

				case ScreenMode.Native:
				case ScreenMode.Fullscreen:
					this.WindowState = WindowState.Fullscreen;
					this.WindowBorder = WindowBorder.Hidden;
					this.ClientSize = new Size(DualityApp.UserData.GfxWidth, DualityApp.UserData.GfxHeight);
					DisplayDevice.Default.ChangeResolution(DualityApp.UserData.GfxWidth, DualityApp.UserData.GfxHeight, DisplayDevice.Default.BitsPerPixel, DisplayDevice.Default.RefreshRate);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			DualityApp.TargetResolution = new Vector2(ClientSize.Width, ClientSize.Height);
			DualityApp.TargetMode = Context.GraphicsMode;
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
			
			int windowWidth = DualityApp.UserData.GfxWidth;
			int windowHeight = DualityApp.UserData.GfxHeight;
			bool isFullscreen = (DualityApp.UserData.GfxMode == ScreenMode.Fullscreen || DualityApp.UserData.GfxMode == ScreenMode.Native) && !isDebugging;
			if (DualityApp.UserData.GfxMode == ScreenMode.Native && !isDebugging)
			{
				windowWidth = DisplayDevice.Default.Width;
				windowHeight = DisplayDevice.Default.Height;
			}

			using (DualityLauncher launcherWindow = new DualityLauncher(
				windowWidth, 
				windowHeight, 
				DualityApp.DefaultMode, 
				DualityApp.AppData.AppName,
				isFullscreen ? GameWindowFlags.Fullscreen : GameWindowFlags.Default))
			{
				DualityApp.UserDataChanged += launcherWindow.OnUserDataChanged;

				// Retrieve icon from executable file and set it as window icon
				string executablePath = System.IO.Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().Location);
				if (System.IO.File.Exists(executablePath))
				{
					launcherWindow.Icon = System.Drawing.Icon.ExtractAssociatedIcon(executablePath);
				}

				// Go into native fullscreen mode
				if (DualityApp.UserData.GfxMode == ScreenMode.FullWindow && !isDebugging)
					launcherWindow.WindowState = WindowState.Fullscreen;

				if (DualityApp.UserData.GfxMode == ScreenMode.FixedWindow)
					launcherWindow.WindowBorder = WindowBorder.Fixed;
				else if (DualityApp.UserData.GfxMode == ScreenMode.Window)
					launcherWindow.WindowBorder = WindowBorder.Resizable;

				// Specify additional window settings and initialize default content
				launcherWindow.MakeCurrent();
				launcherWindow.CursorVisible = isDebugging || DualityApp.UserData.SystemCursorVisible;
				launcherWindow.VSync = (isProfiling || isDebugging || DualityApp.UserData.RefreshMode != RefreshMode.VSync) ? VSyncMode.Off : VSyncMode.On;
				DualityApp.TargetResolution = new Vector2(launcherWindow.ClientSize.Width, launcherWindow.ClientSize.Height);
				DualityApp.TargetMode = launcherWindow.Context.GraphicsMode;
				DualityApp.InitGraphics();

				// Input setup
				DualityApp.Mouse.Source = new GameWindowMouseInputSource(launcherWindow.Mouse, launcherWindow.SetMouseDeviceX, launcherWindow.SetMouseDeviceY);
				DualityApp.Keyboard.Source = new GameWindowKeyboardInputSource(launcherWindow.Keyboard);

				// Debug Logs
				Log.Core.Write("Graphics window initialized: {0}Mode: {1}{0}VSync: {2}{0}SwapInterval: {3}{0}Flags: {4}{0}", 
					Environment.NewLine,
					launcherWindow.Context.GraphicsMode,
					launcherWindow.VSync,
					launcherWindow.Context.SwapInterval,
					new[] { isDebugging ? "Debugging" : null, isProfiling ? "Profiling" : null, isRunFromEditor ? "RunFromEditor" : null }.NotNull().ToString(", "));

				// Load the starting Scene
				Scene.SwitchTo(DualityApp.AppData.StartScene);

				// Run the DualityApp
				launcherWindow.Run();

				// Shut down the DualityApp
				DualityApp.Terminate();
				DisplayDevice.Default.RestoreResolution();
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
