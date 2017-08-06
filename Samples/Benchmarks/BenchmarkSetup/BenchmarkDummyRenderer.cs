using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using Duality.Drawing;
using Duality.Components;
using Duality.Components.Renderers;

namespace Duality.Samples.Benchmarks
{
	[EditorHintCategory("Benchmarks")]
    public class BenchmarkDummyRenderer : Renderer
	{
		public override float BoundRadius
		{
			get { return 128.0f; }
		}

		public override void Draw(IDrawDevice device) { }
	}
}
