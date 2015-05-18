using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend
{
	public interface INativeWindow : IDisposable
	{
		void Run();
	}
}
