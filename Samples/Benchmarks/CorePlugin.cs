using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;

namespace Duality.Samples.Benchmarks
{
	public class BenchmarksSampleCorePlugin : CorePlugin
	{
		private BenchmarkController controller = null;

		protected override void OnAfterUpdate()
		{
			base.OnAfterUpdate();
			if (this.controller == null)
			{
				this.controller = new BenchmarkController();
				this.controller.PrepareBenchmarks();
			}
			this.controller.Update();
		}
		protected override void OnExecContextChanged(DualityApp.ExecutionContext previousContext)
		{
			base.OnExecContextChanged(previousContext);
			// When entering or existing sandbox mode in the editor, reset the global controllers
			this.controller = null;
		}
	}
}