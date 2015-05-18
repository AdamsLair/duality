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
		}
		void INativeRenderableSite.MakeCurrent()
		{
			this.context.GLContext.MakeCurrent(this.control.WindowInfo);
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
	}
}
