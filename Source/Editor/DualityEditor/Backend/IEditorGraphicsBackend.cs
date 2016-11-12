using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Backend;

namespace Duality.Editor.Backend
{
	public interface IEditorGraphicsBackend : IDualityBackend
	{
		INativeEditorGraphicsContext CreateContext();
	}
}
