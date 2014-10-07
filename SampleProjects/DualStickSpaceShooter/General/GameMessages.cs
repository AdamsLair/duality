using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace DualStickSpaceShooter
{
	public static class ExtMethodsComponent
	{
		public static void SendMessage(this Component source, GameMessage message)
		{
			GameMessage.Send(source, source.GameObj, message);
		}
		public static void SendMessage(this Component source, GameObject target, GameMessage message)
		{
			GameMessage.Send(source, target, message);
		}
	}

	public abstract class GameMessage
	{
		private	Component sender;
		public Component Sender
		{
			get { return this.sender; }
			private set { this.sender = value; }
		}

		public static void Send(Component source, GameObject target, GameMessage message)
		{
			// If the target is null or inactive, that's okay. It's just that nobody will receive anything.
			if (target == null) return;
			if (!target.Active) return;

			// Send the message to all the interested Components from the target object.
			message.Sender = source;
			target.IterateComponents<ICmpMessageListener>(
				l => l.OnMessage(message),
				l => (l as Component).Active);
		}
	}

	public class ShipDeathMessage : GameMessage {}
}
