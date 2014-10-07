using System;
using System.Collections.Generic;
using System.Linq;

namespace DualStickSpaceShooter
{
	public interface ICmpMessageListener
	{
		void OnMessage(GameMessage msg);
	}
}
