using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Duality.Drawing;
using Duality.Backend;
using Duality.Backend.DefaultOpenTK;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;

namespace Duality.Editor.Backend.DefaultOpenTK
{
	public class NativeEditorGraphicsContext : INativeEditorGraphicsContext
	{
		private AAQuality antialiasingQuality;
		private GLControl mainContextControl;
		private HashSet<IWindowInfo> swapSchedule = new HashSet<IWindowInfo>();
		private List<GraphicsMode> availableGraphicsModes = null;
		
		public AAQuality AntialiasingQuality
		{
			get { return this.antialiasingQuality; }
		}
		public GraphicsMode MainGraphicsMode
		{
			get { return this.mainContextControl.GraphicsMode; }
		}
		public IGraphicsContext GLContext
		{
			get { return this.mainContextControl.Context; }
		}
		private IEnumerable<GraphicsMode> AvailableGraphicsModes
		{
			get
			{
				if (this.availableGraphicsModes == null)
				{
					int[] aaLevels = new int[] { 0, 2, 4, 6, 8, 16 };
					HashSet<GraphicsMode> modeSet = new HashSet<GraphicsMode>(new GraphicsModeComparer());
					foreach (int samplecount in aaLevels)
					{
						GraphicsMode mode = new GraphicsMode(32, 24, 0, samplecount, new ColorFormat(0), 2, false);
						if (!modeSet.Contains(mode)) modeSet.Add(mode);
					}

					this.availableGraphicsModes = new List<GraphicsMode>();
					this.availableGraphicsModes.AddRange(modeSet);
				}
				return this.availableGraphicsModes;
			}
		}

		public NativeEditorGraphicsContext(AAQuality antialiasingQuality)
		{
			this.antialiasingQuality = antialiasingQuality;

			GraphicsMode defaultGraphicsMode = this.GetGraphicsMode(this.antialiasingQuality);
			this.mainContextControl = new GLControl(
				defaultGraphicsMode, 
				GraphicsBackend.MinOpenGLVersion.Major, 
				GraphicsBackend.MinOpenGLVersion.Minor, 
				GraphicsContextFlags.ForwardCompatible);
			this.mainContextControl.VSync = false;
			this.mainContextControl.MakeCurrent();

			// Log some general info on the graphics context we've set up
			GraphicsBackend.LogOpenGLContextSpecs(this.mainContextControl.Context);

			// Determine OpenGL capabilities and log them
			GraphicsBackend.LogOpenGLSpecs();
		}

		public void ScheduleSwap(GLControl control)
		{
			this.swapSchedule.Add(control.WindowInfo);
		}

		INativeRenderableSite INativeEditorGraphicsContext.CreateRenderableSite()
		{
			return new NativeRenderableSite(this);
		}
		void INativeEditorGraphicsContext.PerformBufferSwap()
		{
			// Perform a buffer swap
			if (this.swapSchedule.Count > 0)
			{
				Profile.TimeRender.BeginMeasure();
				Profile.TimeSwapBuffers.BeginMeasure();
				foreach (IWindowInfo window in this.swapSchedule)
				{
					// Wrap actual buffer swapping in a try-catch block, since
					// it is possible that the window has been disposed, but we have
					// no way of checking its disposal state... so let's try it the hard way.
					try
					{
						this.mainContextControl.Context.MakeCurrent(window);
						this.mainContextControl.SwapBuffers();
					}
					catch (Exception) {}
				}
				Profile.TimeSwapBuffers.EndMeasure();
				Profile.TimeRender.EndMeasure();
				this.swapSchedule.Clear();
			}
		}
		void IDisposable.Dispose()
		{
			if (this.mainContextControl != null)
			{
				this.mainContextControl.Dispose();
				this.mainContextControl = null;
			}
		}

		private GraphicsMode GetGraphicsMode(AAQuality antialiasingQuality)
		{
			IEnumerable<GraphicsMode> modes = this.AvailableGraphicsModes;
			int highestAALevel = MathF.RoundToInt(MathF.Log(MathF.Max(modes.Max(m => m.Samples), 1.0f), 2.0f));
			int targetAALevel = highestAALevel;
			switch (antialiasingQuality)
			{
				case AAQuality.High:   targetAALevel = highestAALevel;     break;
				case AAQuality.Medium: targetAALevel = highestAALevel / 2; break;
				case AAQuality.Low:    targetAALevel = highestAALevel / 4; break;
				case AAQuality.Off:    targetAALevel = 0;                  break;
			}
			int targetSampleCount = MathF.RoundToInt(MathF.Pow(2.0f, targetAALevel));
			return modes.LastOrDefault(m => m.Samples <= targetSampleCount) ?? modes.Last();
		}
	}
}
