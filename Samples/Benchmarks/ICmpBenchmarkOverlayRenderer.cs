using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Drawing;
using Duality.Resources;

namespace Duality.Samples.Benchmarks
{
	/// <summary>
	/// Represents a <see cref="Components"/> that renders a part of a rendering benchmarks
	/// diagnostic overlay. Called by <see cref="BenchmarkRenderSetup"/>.
	/// </summary>
	public interface ICmpBenchmarkOverlayRenderer
	{
		void DrawOverlay(Canvas canvas);
	}
}