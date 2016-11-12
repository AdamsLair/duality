using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Duality.Drawing;
using Duality.Backend;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;

namespace Duality.Editor.Backend.DefaultOpenTK
{
	public class NativeEditorGraphicsContext : INativeEditorGraphicsContext
	{
		private GLControl mainContextControl;
		private HashSet<IWindowInfo> swapSchedule = new HashSet<IWindowInfo>();

		public GraphicsMode MainGraphicsMode
		{
			get { return this.mainContextControl.GraphicsMode; }
		}
		public IGraphicsContext GLContext
		{
			get { return this.mainContextControl.Context; }
		}

		public NativeEditorGraphicsContext()
		{
			GraphicsMode defaultGraphicsMode = this.GetDefaultGraphicsMode();
			this.mainContextControl = new GLControl(defaultGraphicsMode);
			this.mainContextControl.VSync = false;
			this.mainContextControl.MakeCurrent();
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
						mainContextControl.Context.MakeCurrent(window);
						mainContextControl.SwapBuffers();
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

		private GraphicsMode GetDefaultGraphicsMode()
		{
			int[] aaLevels = new int[] { 0, 2, 4, 6, 8, 16 };
			HashSet<GraphicsMode> availGraphicsModes = new HashSet<GraphicsMode>(new GraphicsModeComparer());
			foreach (int samplecount in aaLevels)
			{
				GraphicsMode mode = new GraphicsMode(32, 24, 0, samplecount, new OpenTK.Graphics.ColorFormat(0), 2, false);
				if (!availGraphicsModes.Contains(mode)) availGraphicsModes.Add(mode);
			}
			int highestAALevel = MathF.RoundToInt(MathF.Log(MathF.Max(availGraphicsModes.Max(m => m.Samples), 1.0f), 2.0f));
			int targetAALevel = highestAALevel;
			if (DualityApp.AppData.MultisampleBackBuffer)
			{
				switch (DualityApp.UserData.AntialiasingQuality)
				{
					case AAQuality.High:	targetAALevel = highestAALevel;		break;
					case AAQuality.Medium:	targetAALevel = highestAALevel / 2; break;
					case AAQuality.Low:		targetAALevel = highestAALevel / 4; break;
					case AAQuality.Off:		targetAALevel = 0;					break;
				}
			}
			else
			{
				targetAALevel = 0;
			}
			int targetSampleCount = MathF.RoundToInt(MathF.Pow(2.0f, targetAALevel));
			return availGraphicsModes.LastOrDefault(m => m.Samples <= targetSampleCount) ?? availGraphicsModes.Last();
		}
	}
}
