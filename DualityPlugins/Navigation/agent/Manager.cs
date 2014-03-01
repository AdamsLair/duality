using Duality;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Duality.Plugins.Navigation
{
	[Serializable]
	public class AgentManager : Component, ICmpUpdatable
	{
		public IEnumerable<Agent> FindNeighborAgents(Agent referenceAgent)
		{
			var result = new List<Agent>();
			var allAgents = GameObj.ParentScene.FindComponents<Agent>();
			foreach(var agent in allAgents)
			{
				if (agent != referenceAgent)
					result.Add(agent);
			}
			return result;
		}

		public void OnUpdate()
		{
			foreach (var agent in GameObj.ParentScene.FindComponents<Agent>())
			{
				agent.OnUpdate();
			}

			float factor = 1f;
			foreach (var agent in GameObj.ParentScene.FindComponents<Agent>())
			{
				agent.GameObj.RigidBody.LinearVelocity = (1f - factor) * agent.GameObj.RigidBody.LinearVelocity + factor * agent.BestVel;
			}
		}
	}
}
