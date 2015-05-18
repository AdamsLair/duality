using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Duality.Drawing;
using Duality.Backend;

namespace Duality.Editor.Backend
{
	public interface INativeRenderableSite : IDisposable
	{
		Control Control { get; }

		void MakeCurrent();
		void SwapBuffers();
	}
}
