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
using Duality.Tests.Components.RequirementTest;

using NUnit.Framework;


namespace Duality.Tests.Components
{
	[TestFixture]
	public class ComponentExecutionOrderTest
	{
		[Test] public void BasicFunctionality()
		{
			ComponentExecutionOrder order = new ComponentExecutionOrder();

			int sortIndexA1 = order.GetSortIndex(typeof(TestComponentA1));
			int sortIndexA2 = order.GetSortIndex(typeof(TestComponentA2));
			int sortIndexA3 = order.GetSortIndex(typeof(TestComponentA3));

			// Expect no particular order, but consistent results
			Assert.AreEqual(sortIndexA1, order.GetSortIndex(typeof(TestComponentA1)));
			Assert.AreEqual(sortIndexA2, order.GetSortIndex(typeof(TestComponentA2)));
			Assert.AreEqual(sortIndexA3, order.GetSortIndex(typeof(TestComponentA3)));

			// Expect that clearing the type cache does not change any results
			order.ClearTypeCache();
			Assert.AreEqual(sortIndexA1, order.GetSortIndex(typeof(TestComponentA1)));
			Assert.AreEqual(sortIndexA2, order.GetSortIndex(typeof(TestComponentA2)));
			Assert.AreEqual(sortIndexA3, order.GetSortIndex(typeof(TestComponentA3)));
		}
	}

	namespace ExecutionOrderTest
	{
		public class TestComponentA1 : Component { }
		public class TestComponentA2 : Component { }
		public class TestComponentA3 : Component { }
	}
}
