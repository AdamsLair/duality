using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Resources;
using Duality.Editor;
using Duality.Drawing;

namespace Duality.Samples.Benchmarks
{
	/// <summary>
	/// Scenes tagged by containing a <see cref="Component"/> of this type will
	/// install a <see cref="BenchmarkController"/> while active, replacing regular
	/// rendering with a specialized <see cref="BenchmarkRenderSetup"/> for tracking
	/// performance in different environments.
	/// </summary>
	[EditorHintCategory("Benchmarks")]
	public class BenchmarkSceneTag : Component, ICmpUpdatable, ICmpInitializable
	{
		void ICmpUpdatable.OnUpdate()
		{
			BenchmarkController.Instance.Update();
		}
		void ICmpInitializable.OnActivate()
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				BenchmarkController.Instance.EnterBenchmarkMode();
			}
		}
		void ICmpInitializable.OnDeactivate()
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				BenchmarkController.Instance.LeaveBenchmarkMode();
			}
		}
	}
}
