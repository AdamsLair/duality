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

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class NativeWindow : INativeWindow
	{
		private class InternalWindow : GameWindow
		{
			private NativeWindow parent;

			public InternalWindow(NativeWindow parent, int w, int h, GraphicsMode mode, string title, GameWindowFlags flags) : base(w, h, mode, title, flags)
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

		private InternalWindow	internalWindow;
		private RefreshMode		refreshMode;
		private Stopwatch		frameLimiterWatch = new Stopwatch();

		public NativeWindow(GraphicsMode mode, WindowOptions options)
		{
			if (options.ScreenMode == ScreenMode.Native)
			{
				options.Width = DisplayDevice.Default.Width;
				options.Height = DisplayDevice.Default.Height;
			}

			this.refreshMode = options.RefreshMode;
			this.internalWindow = new InternalWindow(
				this,
				options.Width,
				options.Height,
				mode,
				options.Title,
				GameWindowFlags.Default);
			this.internalWindow.MakeCurrent();
			this.internalWindow.CursorVisible = options.SystemCursorVisible;
			this.internalWindow.VSync = (options.RefreshMode != RefreshMode.VSync) ? VSyncMode.Off : VSyncMode.On;

			Log.Core.Write("Window Specification: {0}Mode: {1}{0}VSync: {2}{0}SwapInterval: {3}{0}", 
				Environment.NewLine,
				this.internalWindow.Context.GraphicsMode,
				this.internalWindow.VSync,
				this.internalWindow.Context.SwapInterval);

			// Retrieve icon from executable file and set it as window icon
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null)
			{
				string executablePath = Path.GetFullPath(entryAssembly.Location);
				if (File.Exists(executablePath))
				{
					this.internalWindow.Icon = Icon.ExtractAssociatedIcon(executablePath);
				}
			}

			if (options.ScreenMode == ScreenMode.FullWindow)
				this.internalWindow.WindowState = WindowState.Fullscreen;
			else if (options.ScreenMode == ScreenMode.FixedWindow)
				this.internalWindow.WindowBorder = WindowBorder.Fixed;
			else if (options.ScreenMode == ScreenMode.Window)
				this.internalWindow.WindowBorder = WindowBorder.Resizable;

			DualityApp.TargetResolution = new Vector2(this.internalWindow.ClientSize.Width, this.internalWindow.ClientSize.Height);

			DualityApp.UserDataChanged += this.OnUserDataChanged;
			
			// Determine OpenGL capabilities and log them
			GraphicsBackend.LogOpenGLSpecs();
		}
		void INativeWindow.Run()
		{
			this.internalWindow.Run();
		}
		void IDisposable.Dispose()
		{
			DualityApp.UserDataChanged -= this.OnUserDataChanged;
			if (this.internalWindow != null)
			{
				DisplayDevice.Default.RestoreResolution();
				this.internalWindow.Dispose();
				this.internalWindow = null;
			}
		}
		
		private void OnUserDataChanged(object sender, EventArgs e)
		{
			switch (DualityApp.UserData.GfxMode)
			{
				case ScreenMode.Window:
				case ScreenMode.FixedWindow:
					this.internalWindow.WindowState = WindowState.Normal;
					this.internalWindow.WindowBorder = WindowBorder.Fixed;
					this.internalWindow.ClientSize = new Size(DualityApp.UserData.GfxWidth, DualityApp.UserData.GfxHeight);
					DisplayDevice.Default.RestoreResolution();
					break;

				case ScreenMode.FullWindow:
					this.internalWindow.ClientSize = new Size(DisplayDevice.Default.Width, DisplayDevice.Default.Height);
					this.internalWindow.WindowState = WindowState.Fullscreen;
					this.internalWindow.WindowBorder = WindowBorder.Hidden;
					DisplayDevice.Default.RestoreResolution();
					break;

				case ScreenMode.Native:
				case ScreenMode.Fullscreen:
					this.internalWindow.WindowState = WindowState.Fullscreen;
					this.internalWindow.WindowBorder = WindowBorder.Hidden;
					this.internalWindow.ClientSize = new Size(DualityApp.UserData.GfxWidth, DualityApp.UserData.GfxHeight);
					DisplayDevice.Default.ChangeResolution(DualityApp.UserData.GfxWidth, DualityApp.UserData.GfxHeight, DisplayDevice.Default.BitsPerPixel, DisplayDevice.Default.RefreshRate);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			DualityApp.TargetResolution = new Vector2(this.internalWindow.ClientSize.Width, this.internalWindow.ClientSize.Height);
		}
		private void OnResize(EventArgs e)
		{
			DualityApp.TargetResolution = new Vector2(
				this.internalWindow.ClientSize.Width, 
				this.internalWindow.ClientSize.Height);
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
				while (this.frameLimiterWatch.Elapsed.TotalMilliseconds < Time.MsPFMult)
				{
					// Enough leftover time? Risk a millisecond sleep.
					if (this.frameLimiterWatch.Elapsed.TotalMilliseconds < Time.MsPFMult * 0.75f)
						System.Threading.Thread.Sleep(1);
				}
			}
			this.frameLimiterWatch.Restart();
			DualityApp.Update();
		}
		private void OnRenderFrame(FrameEventArgs e)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return;

			DualityApp.Render(new Rect(this.internalWindow.ClientSize.Width, this.internalWindow.ClientSize.Height));
			Profile.TimeRender.BeginMeasure();
			Profile.TimeSwapBuffers.BeginMeasure();
			this.internalWindow.SwapBuffers();
			Profile.TimeSwapBuffers.EndMeasure();
			Profile.TimeRender.EndMeasure();
		}
	}
}
