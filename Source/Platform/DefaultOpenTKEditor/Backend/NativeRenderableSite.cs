using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Duality.Drawing;
using Duality.Backend;

using OpenTK;
using OpenTK.Graphics;

namespace Duality.Editor.Backend.DefaultOpenTK
{
	public class NativeRenderableSite : INativeRenderableSite
	{
		private NativeEditorGraphicsContext context;
		private GLControl control;

		Control INativeRenderableSite.Control
		{
			get { return this.control; }
		}

		public NativeRenderableSite(NativeEditorGraphicsContext context)
		{
			this.context = context;
			this.control = new GLControl(this.context.MainGraphicsMode);

			//
			// Since the GLControl will create its own context and make it the
			// current one as soon as its window handle is available, we'll
			// need to register for this happening and counter-act by making
			// the editor's main context current again. 
			//
			// Otherwise, any graphics resource or operation created or performed 
			// will be part of a different context and be invalid / produce 
			// invalid handles.
			//
			this.control.HandleCreated += this.control_HandleCreated;
		}

		void INativeRenderableSite.MakeCurrent()
		{
			this.context.GLContext.MakeCurrent(this.control.WindowInfo);
			DualityApp.GraphicsBackend.ExternalBackbufferSize = new Point2(
				this.control.ClientSize.Width, 
				this.control.ClientSize.Height);
		}
		void INativeRenderableSite.SwapBuffers()
		{
			this.context.ScheduleSwap(this.control);
		}
		void IDisposable.Dispose()
		{
			if (this.control != null)
			{
				this.control.Dispose();
				this.control = null;
			}
		}

		private void control_HandleCreated(object sender, EventArgs e)
		{
			this.control.HandleCreated -= this.control_HandleCreated;
			this.context.GLContext.MakeCurrent(this.control.WindowInfo);
		}
	}
}
