using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Resources;

namespace Duality.Backend
{
	public interface INativeWindow : IDisposable
	{
		void Run();
		void SetCursor(ContentRef<Pixmap> cursor, Point2 hotspot);
	}
}
