using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Resources;
using Duality.Components;
using Duality.Serialization;

using Duality.Tests.Components;
using Duality.Tests.Components.ExecutionOrderTest;

using NUnit.Framework;


namespace Duality.Tests.Components.ExecutionOrderTest
{
	[Flags]
	public enum EventType
	{
		None	   = 0x00,

		Update	 = 0x01,
		Activate   = 0x02,
		Deactivate = 0x04,
		Loaded	 = 0x08,
		Saved	  = 0x10,
		Saving	 = 0x20,

		All = Update | Activate | Deactivate | Loaded | Saved | Saving
	}
}
