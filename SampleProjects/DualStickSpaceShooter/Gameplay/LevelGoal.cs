using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	[Serializable]
	[RequiredComponent(typeof(Trigger))]
	public class LevelGoal : Component, ICmpMessageListener
	{
		void ICmpMessageListener.OnMessage(GameMessage msg)
		{
			TriggerEnteredMessage entered = msg as TriggerEnteredMessage;
			if (entered != null)
			{
				Ship ship = entered.GameObj.GetComponent<Ship>();
				if (ship != null && ship.Owner != null)
				{
					ship.Owner.NotifyGoalReached();
				}
			}
		}
	}
}
