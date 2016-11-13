using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Cloning.HelperObjects
{
	internal class InterfaceFieldTestObject
	{
		public int IntValue;
		public IList<int> InterfaceValue;
	}
}
