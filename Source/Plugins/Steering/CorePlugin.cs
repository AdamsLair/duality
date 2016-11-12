
using Duality;
using Duality.Resources;

namespace Duality.Plugins.Steering
{
	public class SteeringCorePlugin : CorePlugin
	{
		protected override void OnBeforeUpdate()
		{
			var agents = Scene.Current.FindComponents<Agent>();
			foreach (var agent in agents)
				agent.Update();
		}
	}
}
