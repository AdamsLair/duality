using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Reflection;

using Duality.Drawing;

using OpenTK;
using OpenTK.Graphics;
using Duality.Resources;

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class NativeWindow : INativeWindow
	{
		private class InternalWindow : GameWindow
		{
			private NativeWindow parent;

			public InternalWindow(NativeWindow parent, int w, int h, GraphicsMode mode, string title, GameWindowFlags flags)
				: base(w, h, mode, title, flags, 
					  DisplayDevice.Default, 
					  GraphicsBackend.MinOpenGLVersion.Major, 
					  GraphicsBackend.MinOpenGLVersion.Minor, 
					  GraphicsContextFlags.ForwardCompatible)
			{
				this.parent = parent;
			}

			protected override void OnResize(EventArgs e)
			{
				base.OnResize(e);
				this.parent.OnResize(e);
			}
			protected override void OnUpdateFrame(FrameEventArgs e)
			{
				base.OnUpdateFrame(e);
				this.parent.OnUpdateFrame(e);
			}
			protected override void OnRenderFrame(FrameEventArgs e)
			{
				base.OnRenderFrame(e);
				this.parent.OnRenderFrame(e);
			}
		}

		private InternalWindow internalWindow;
		private RefreshMode refreshMode;
		private Stopwatch frameLimiterWatch = new Stopwatch();

		public int Width
		{
			get { return this.internalWindow.ClientSize.Width; }
		}
		public int Height
		{
			get { return this.internalWindow.ClientSize.Height; }
		}
		public Point2 Size
		{
			get { return new Point2(this.Width, this.Height); }
		}
		public bool IsMultisampled
		{
			get { return this.internalWindow.Context.GraphicsMode.Samples > 0; }
		}

		public NativeWindow(GraphicsMode mode, WindowOptions options)
		{
			if (options.ScreenMode == ScreenMode.Fullscreen || options.ScreenMode == ScreenMode.FullWindow)
			{
				if (DisplayDevice.Default != null)
				{
					options.Size = new Point2(
						DisplayDevice.Default.Width, 
						DisplayDevice.Default.Height);
				}
			}

			GameWindowFlags windowFlags = GameWindowFlags.Default;
			if (options.ScreenMode == ScreenMode.FixedWindow)
				windowFlags = GameWindowFlags.FixedWindow;
			else if (options.ScreenMode == ScreenMode.Fullscreen)
				windowFlags = GameWindowFlags.Fullscreen;

			VSyncMode vsyncMode;
			switch (options.RefreshMode)
			{
				default:
				case RefreshMode.NoSync:
				case RefreshMode.ManualSync:
					vsyncMode = VSyncMode.Off;
					break;
				case RefreshMode.VSync:
					vsyncMode = VSyncMode.On;
					break;
				case RefreshMode.AdaptiveVSync:
					vsyncMode = VSyncMode.Adaptive;
					break;
			}

			this.refreshMode = options.RefreshMode;
			this.internalWindow = new InternalWindow(
				this,
				options.Size.X,
				options.Size.Y,
				mode,
				options.Title,
				windowFlags);
			this.internalWindow.MakeCurrent();
			this.internalWindow.CursorVisible = true;
			if (!options.SystemCursorVisible)
				this.internalWindow.Cursor = MouseCursor.Empty;
			this.internalWindow.VSync = vsyncMode;

			// Log some general info on the graphics context we've set up
			GraphicsBackend.LogOpenGLContextSpecs(this.internalWindow.Context);

			// Retrieve icon from executable file and set it as window icon
			string executablePath = null;
			try
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null)
				{
					executablePath = Path.GetFullPath(entryAssembly.Location);
					if (File.Exists(executablePath))
					{
						this.internalWindow.Icon = Icon.ExtractAssociatedIcon(executablePath);
					}
				}
			}
			// As described in issue 301 (https://github.com/AdamsLair/duality/issues/301), the
			// icon extraction can fail with an exception under certain circumstances. Don't fail
			// just because of an icon. Log the error and continue.
			catch (Exception e)
			{
				Logs.Core.WriteWarning(
					"There was an exception while trying to extract the " +
					"window icon from the game's main executable '{0}'. This is " +
					"uncritical, but still means something went wrong: {1}",
					executablePath,
					LogFormat.Exception(e));
			}

			if (options.ScreenMode == ScreenMode.FullWindow)
				this.internalWindow.WindowState = WindowState.Fullscreen;

			DualityApp.WindowSize = new Point2(this.internalWindow.ClientSize.Width, this.internalWindow.ClientSize.Height);

			// Register events and input
			this.HookIntoDuality();

			// Let's see what rendering features we have available
			GraphicsBackend.ActiveInstance.QueryOpenGLCapabilities();
		}
		void INativeWindow.Run()
		{
			this.internalWindow.Run();
		}
		void IDisposable.Dispose()
		{
			this.UnhookFromDuality();
			if (this.internalWindow != null)
			{
				this.internalWindow.Dispose();
				this.internalWindow = null;
			}
		}

		internal void HookIntoDuality()
		{
			DualityApp.Mouse.Source = new GameWindowMouseInputSource(this.internalWindow);
			DualityApp.Keyboard.Source = new GameWindowKeyboardInputSource(this.internalWindow);
			DualityApp.UserData.Changed += this.OnUserDataChanged;
		}
		internal void UnhookFromDuality()
		{
			if (DualityApp.Mouse.Source is GameWindowMouseInputSource)
				DualityApp.Mouse.Source = null;
			if (DualityApp.Keyboard.Source is GameWindowKeyboardInputSource)
				DualityApp.Keyboard.Source = null;
			DualityApp.UserData.Changed -= this.OnUserDataChanged;
		}

		private void OnUserDataChanged(object sender, EventArgs e)
		{
			// Early-out, if no display is connected / available anyway
			if (DisplayDevice.Default == null) return;

			// Determine the target state for our window
			MouseCursor targetCursor = DualityApp.UserData.Instance.SystemCursorVisible ? MouseCursor.Default : MouseCursor.Empty;
			WindowState targetWindowState = this.internalWindow.WindowState;
			WindowBorder targetWindowBorder = this.internalWindow.WindowBorder;
			Size targetSize = this.internalWindow.ClientSize;
			switch (DualityApp.UserData.Instance.WindowMode)
			{
				case ScreenMode.Window:
					targetWindowState = WindowState.Normal;
					targetWindowBorder = WindowBorder.Resizable;
					targetSize = new Size(DualityApp.UserData.Instance.WindowSize.X, DualityApp.UserData.Instance.WindowSize.Y);
					break;

				case ScreenMode.FixedWindow:
					targetWindowState = WindowState.Normal;
					targetWindowBorder = WindowBorder.Fixed;
					targetSize = new Size(DualityApp.UserData.Instance.WindowSize.X, DualityApp.UserData.Instance.WindowSize.Y);
					break;

				case ScreenMode.FullWindow:
				case ScreenMode.Fullscreen:
					targetWindowState = WindowState.Fullscreen;
					targetWindowBorder = WindowBorder.Hidden;
					targetSize = new Size(DisplayDevice.Default.Width, DisplayDevice.Default.Height);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			// Apply the target state to the game window wherever values changed
			if (this.internalWindow.WindowState != targetWindowState)
				this.internalWindow.WindowState = targetWindowState;
			if (this.internalWindow.WindowBorder != targetWindowBorder)
				this.internalWindow.WindowBorder = targetWindowBorder;
			if (this.internalWindow.ClientSize != targetSize)
				this.internalWindow.ClientSize = targetSize;
			if (this.internalWindow.Cursor != targetCursor)
				this.internalWindow.Cursor = targetCursor;

			DualityApp.WindowSize = new Point2(this.internalWindow.ClientSize.Width, this.internalWindow.ClientSize.Height);
		}
		private void OnResize(EventArgs e)
		{
			DualityApp.WindowSize = this.Size;
			DrawDevice.RenderVoid(new Rect(this.Size));
		}
		private void OnUpdateFrame(FrameEventArgs e)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated)
			{
				this.internalWindow.Close();
				return;
			}

			// Give the processor a rest if we have the time, don't use 100% CPU even without VSync
			if (this.frameLimiterWatch.IsRunning && this.refreshMode == RefreshMode.ManualSync)
			{
				while (this.frameLimiterWatch.Elapsed.TotalMilliseconds < Time.MillisecondsPerFrame)
				{
					// Enough leftover time? Risk a short sleep, don't burn CPU waiting.
					if (this.frameLimiterWatch.Elapsed.TotalMilliseconds < Time.MillisecondsPerFrame * 0.75f)
						System.Threading.Thread.Sleep(0);
				}
			}
			this.frameLimiterWatch.Restart();
			DualityApp.Update();
		}
		private void OnRenderFrame(FrameEventArgs e)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return;
			
			Vector2 imageSize;
			Rect viewportRect;
			DualityApp.CalculateGameViewport(this.Size, out viewportRect, out imageSize);

			DualityApp.Render(null, viewportRect, imageSize);
			Profile.TimeRender.BeginMeasure();
			Profile.TimeSwapBuffers.BeginMeasure();
			this.internalWindow.SwapBuffers();
			Profile.TimeSwapBuffers.EndMeasure();
			Profile.TimeRender.EndMeasure();
		}

		public void SetCursor(ContentRef<Pixmap> cursor, Point2 hotspot)
		{
			if (this.internalWindow != null)
			{
				if (cursor.IsExplicitNull)
				{
					this.internalWindow.Cursor = MouseCursor.Default;
				}
				else
				{
					Pixmap pixmap = cursor.Res;
					byte[] data = new byte[pixmap.MainLayer.Data.Length * 4];

					for (int i = 0, j = 0; i < pixmap.MainLayer.Data.Length; i++, j += 4)
					{
						ColorRgba px = pixmap.MainLayer.Data[i];

						data[j] = (byte)(px.B * px.A / 255);
						data[j + 1] = (byte)(px.G * px.A / 255);
						data[j + 2] = (byte)(px.R * px.A / 255);
						data[j + 3] = px.A;
					}

					this.internalWindow.Cursor = new MouseCursor(hotspot.X, hotspot.Y, pixmap.Width, pixmap.Height, data);
				}
			}
		}
	}
}
