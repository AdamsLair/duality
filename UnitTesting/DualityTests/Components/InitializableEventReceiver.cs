using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Components;

using OpenTK;

using NUnit.Framework;

namespace DualityTests.Components
{
	[Serializable]
	public class InitializableEventReceiver : Component, ICmpInitializable
	{
		[Flags]
		public enum EventFlag
		{
			None					= 0x00,

			Activate				= 0x01,
			AddToGameObject			= 0x02,
			Loaded					= 0x04,
			Saved					= 0x08,
			Deactivate				= 0x10,
			RemovingFromGameObject	= 0x20,
			Saving					= 0x40
		}

		private EventFlag receivedEvents = EventFlag.None;
		public EventFlag ReceivedEvents
		{
			get { return this.receivedEvents; }
		}

		public void Reset()
		{
			this.receivedEvents = EventFlag.None;
		}
		public bool HasReceived(EventFlag eventFlag)
		{
			return this.receivedEvents.HasFlag(eventFlag);
		}

		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			switch (context)
			{
				case InitContext.Activate:			this.receivedEvents |= EventFlag.Activate;			break;
				case InitContext.AddToGameObject:	this.receivedEvents |= EventFlag.AddToGameObject;	break;
				case InitContext.Loaded:			this.receivedEvents |= EventFlag.Loaded;			break;
				case InitContext.Saved:				this.receivedEvents |= EventFlag.Saved;				break;
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			switch (context)
			{
				case ShutdownContext.Deactivate:				this.receivedEvents |= EventFlag.Deactivate;				break;
				case ShutdownContext.RemovingFromGameObject:	this.receivedEvents |= EventFlag.RemovingFromGameObject;	break;
				case ShutdownContext.Saving:					this.receivedEvents |= EventFlag.Saving;					break;
			}
		}
	}
}
