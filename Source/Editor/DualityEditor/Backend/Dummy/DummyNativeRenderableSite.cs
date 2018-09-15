using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using Duality.Drawing;
using Duality.Backend;

namespace Duality.Editor.Backend.Dummy
{
	internal class DummyNativeRenderableSite : INativeRenderableSite
	{
		private Control control;

		public DummyNativeRenderableSite()
		{
			Panel dummyControl = new Panel();
			dummyControl.Name = "DummyRenderableControl";
			dummyControl.BackColor = Color.CornflowerBlue;
			dummyControl.BorderStyle = BorderStyle.None;
			this.control = dummyControl;
		}

		AAQuality INativeRenderableSite.AntialiasingQuality
		{
			get { return AAQuality.Off; }
		}
		Control INativeRenderableSite.Control
		{
			get { return this.control; }
		}
		void INativeRenderableSite.MakeCurrent() { }
		void INativeRenderableSite.SwapBuffers() { }
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
