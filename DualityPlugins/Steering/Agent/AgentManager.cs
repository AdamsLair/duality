using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Duality;
using Duality.Resources;

namespace Duality.Plugins.Steering
{
	internal class AgentManager
	{
		private static AgentManager instance = null;
		public static AgentManager Instance
		{
			get
			{
				if (instance == null) instance = new AgentManager();
				return instance;
			}
		}

		public IEnumerable<Agent> FindNeighborAgents(Agent referenceAgent)
		{
			// ToDo: Performance Optimization when necessary.
			return Scene.Current.FindComponents<Agent>().Where(a => a != referenceAgent);
		}
	}
}
