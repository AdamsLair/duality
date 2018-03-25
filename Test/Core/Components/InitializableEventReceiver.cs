using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Components;

using NUnit.Framework;

namespace Duality.Tests.Components
{
	public class InitializableEventReceiver : Component, ICmpInitializable, ICmpSerializeListener, ICmpAttachmentListener
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

		void ICmpInitializable.OnActivate()
		{
			this.receivedEvents |= EventFlag.Activate;
		}
		void ICmpInitializable.OnDeactivate()
		{
			this.receivedEvents |= EventFlag.Deactivate;
		}
		void ICmpSerializeListener.OnLoaded()
		{
			this.receivedEvents |= EventFlag.Loaded;
		}
		void ICmpSerializeListener.OnSaved()
		{
			this.receivedEvents |= EventFlag.Saved;
		}
		void ICmpSerializeListener.OnSaving()
		{
			this.receivedEvents |= EventFlag.Saving;
		}
		void ICmpAttachmentListener.OnAddToGameObject()
		{
			this.receivedEvents |= EventFlag.AddToGameObject;
		}
		void ICmpAttachmentListener.OnRemoveFromGameObject()
		{
			this.receivedEvents |= EventFlag.RemovingFromGameObject;
		}
	}
}
