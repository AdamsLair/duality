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
		None = 0x0,

		Update = 0x1,
		Activate = 0x2,
		Deactivate = 0x4,

		All = Update | Activate | Deactivate
	}
}
