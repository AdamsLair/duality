using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend
{
	public interface IGraphicsBackend : IDualityBackend
	{
		void BeginRendering(IDrawDevice device, RenderOptions options, RenderStats stats = null);
		void Render(IReadOnlyList<IDrawBatch> batches);
		void EndRendering();
	}
}
