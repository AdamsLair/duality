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
	public class ComponentRequirementMapTest
	{
		[Test] public void NoRequirements()
		{
			ComponentRequirementMap map = new ComponentRequirementMap();

			CollectionAssert.AreEquivalent(new Type[0], map.GetRequirements(typeof(TestComponentA3)));
			CollectionAssert.AreEqual(new Type[0], map.GetRequirementsToCreate(new GameObject(), typeof(TestComponentA3)));

			Assert.IsFalse(map.IsRequired(typeof(TestComponentA3), typeof(TestComponentA3)));
			Assert.IsFalse(map.IsRequired(typeof(TestComponentA3), typeof(TestComponentA2)));
			Assert.IsFalse(map.IsRequired(typeof(TestComponentA3), typeof(TestComponentA1)));

			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentA3)));
			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentA3), new[] { typeof(TestComponentA2) }));
			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentA3), new[] { typeof(TestComponentA1) }));
			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentA3), new[] { typeof(TestComponentA2), typeof(TestComponentA1) }));
			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentA3), new[] { typeof(TestComponentA1), typeof(TestComponentA2) }));
		}
		[Test] public void DirectRequirements()
		{
			ComponentRequirementMap map = new ComponentRequirementMap();

			CollectionAssert.AreEquivalent(new[] { typeof(TestComponentA3) }, map.GetRequirements(typeof(TestComponentA2)));
			CollectionAssert.AreEqual(new[] { typeof(TestComponentA3) }, map.GetRequirementsToCreate(new GameObject(), typeof(TestComponentA2)));

			Assert.IsTrue(map.IsRequired(typeof(TestComponentA2), typeof(TestComponentA3)));
			Assert.IsFalse(map.IsRequired(typeof(TestComponentA2), typeof(TestComponentA2)));
			Assert.IsFalse(map.IsRequired(typeof(TestComponentA2), typeof(TestComponentA1)));

			Assert.IsFalse(map.IsRequirementMet(new GameObject(), typeof(TestComponentA2)));
			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentA2), new[] { typeof(TestComponentA3) }));
			Assert.IsFalse(map.IsRequirementMet(new GameObject(), typeof(TestComponentA2), new[] { typeof(TestComponentA1) }));
			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentA2), new[] { typeof(TestComponentA3), typeof(TestComponentA1) }));
			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentA2), new[] { typeof(TestComponentA1), typeof(TestComponentA3) }));
		}
		[Test] public void TransitiveRequirements()
		{
			ComponentRequirementMap map = new ComponentRequirementMap();

			CollectionAssert.AreEquivalent(new[] { typeof(TestComponentA3), typeof(TestComponentA2) }, map.GetRequirements(typeof(TestComponentA1)));
			CollectionAssert.AreEqual(new[] { typeof(TestComponentA3), typeof(TestComponentA2) }, map.GetRequirementsToCreate(new GameObject(), typeof(TestComponentA1)));

			Assert.IsTrue(map.IsRequired(typeof(TestComponentA1), typeof(TestComponentA3)));
			Assert.IsTrue(map.IsRequired(typeof(TestComponentA1), typeof(TestComponentA2)));
			Assert.IsFalse(map.IsRequired(typeof(TestComponentA1), typeof(TestComponentA1)));

			Assert.IsFalse(map.IsRequirementMet(new GameObject(), typeof(TestComponentA1)));
			Assert.IsFalse(map.IsRequirementMet(new GameObject(), typeof(TestComponentA1), new[] { typeof(TestComponentA3) }));
			Assert.IsFalse(map.IsRequirementMet(new GameObject(), typeof(TestComponentA1), new[] { typeof(TestComponentA2) }));
			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentA1), new[] { typeof(TestComponentA3), typeof(TestComponentA2) }));
			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentA1), new[] { typeof(TestComponentA2), typeof(TestComponentA3) }));
		}
		[Test] public void SelfRequirements()
		{
			ComponentRequirementMap map = new ComponentRequirementMap();

			CollectionAssert.AreEquivalent(new Type[0], map.GetRequirements(typeof(TestComponentB1)));
			CollectionAssert.AreEqual(new Type[0], map.GetRequirementsToCreate(new GameObject(), typeof(TestComponentB1)));

			Assert.IsFalse(map.IsRequired(typeof(TestComponentB1), typeof(TestComponentB1)));

			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentB1)));
		}
		[Test] public void RedundantRequirements()
		{
			ComponentRequirementMap map = new ComponentRequirementMap();

			CollectionAssert.AreEquivalent(new[] { typeof(TestComponentB1) }, map.GetRequirements(typeof(TestComponentB2)));
			CollectionAssert.AreEqual(new[] { typeof(TestComponentB1) }, map.GetRequirementsToCreate(new GameObject(), typeof(TestComponentB2)));

			Assert.IsTrue(map.IsRequired(typeof(TestComponentB2), typeof(TestComponentB1)));
			Assert.IsFalse(map.IsRequired(typeof(TestComponentB2), typeof(TestComponentB2)));

			Assert.IsFalse(map.IsRequirementMet(new GameObject(), typeof(TestComponentB2)));
			Assert.IsTrue(map.IsRequirementMet(new GameObject(), typeof(TestComponentB2), new[] { typeof(TestComponentB1) }));
		}
		[Test] public void CyclicRequirements()
		{
			ComponentRequirementMap map = new ComponentRequirementMap();

			// In a cyclic dependency situation, behavior is undefined and there is no "right" solution.
			// For this test, just expect the system to not crash. Check some select results that we
			// can safely expect.

			map.GetRequirements(typeof(TestComponentC1));
			map.GetRequirements(typeof(TestComponentC2));
			map.GetRequirements(typeof(TestComponentC3));

			map.GetRequirementsToCreate(new GameObject(), typeof(TestComponentC1));
			map.GetRequirementsToCreate(new GameObject(), typeof(TestComponentC2));
			map.GetRequirementsToCreate(new GameObject(), typeof(TestComponentC3));

			Assert.IsFalse(map.IsRequired(typeof(TestComponentC1), typeof(TestComponentC1)));
			Assert.IsFalse(map.IsRequired(typeof(TestComponentC2), typeof(TestComponentC2)));
			Assert.IsFalse(map.IsRequired(typeof(TestComponentC3), typeof(TestComponentC3)));

			map.IsRequired(typeof(TestComponentC1), typeof(TestComponentC2));
			map.IsRequired(typeof(TestComponentC1), typeof(TestComponentC3));
			map.IsRequired(typeof(TestComponentC2), typeof(TestComponentC1));
			map.IsRequired(typeof(TestComponentC2), typeof(TestComponentC3));
			map.IsRequired(typeof(TestComponentC3), typeof(TestComponentC1));
			map.IsRequired(typeof(TestComponentC3), typeof(TestComponentC2));

			map.IsRequirementMet(new GameObject(), typeof(TestComponentC1));
			map.IsRequirementMet(new GameObject(), typeof(TestComponentC2));
			map.IsRequirementMet(new GameObject(), typeof(TestComponentC3));
			map.IsRequirementMet(new GameObject(), typeof(TestComponentC1), new[] { typeof(TestComponentC2) });
			map.IsRequirementMet(new GameObject(), typeof(TestComponentC2), new[] { typeof(TestComponentC3) });
			map.IsRequirementMet(new GameObject(), typeof(TestComponentC3), new[] { typeof(TestComponentC1) });
			map.IsRequirementMet(new GameObject(), typeof(TestComponentC1), new[] { typeof(TestComponentC2), typeof(TestComponentC3) });
			map.IsRequirementMet(new GameObject(), typeof(TestComponentC2), new[] { typeof(TestComponentC3), typeof(TestComponentC1) });
			map.IsRequirementMet(new GameObject(), typeof(TestComponentC3), new[] { typeof(TestComponentC1), typeof(TestComponentC2) });
		}
		[Test] public void AbstractRequirements()
		{
			ComponentRequirementMap map = new ComponentRequirementMap();

			// Interface requirements are stated as-is without further exploration
			CollectionAssert.AreEquivalent(new[] { typeof(ITestComponentDX) }, map.GetRequirements(typeof(TestComponentD1)));
			CollectionAssert.AreEquivalent(new[] { typeof(ITestComponentDY) }, map.GetRequirements(typeof(TestComponentD2)));
			CollectionAssert.AreEquivalent(new[] { typeof(ITestComponentDY), typeof(TestComponentD6) }, map.GetRequirements(typeof(TestComponentD3)));
			CollectionAssert.AreEquivalent(new[] { typeof(TestComponentD6) }, map.GetRequirements(typeof(TestComponentD4)));
			CollectionAssert.AreEquivalent(new Type[0], map.GetRequirements(typeof(TestComponentD5)));
			CollectionAssert.AreEquivalent(new Type[0], map.GetRequirements(typeof(TestComponentD6)));

			// Creation requirements are conditional depending on what's already there.
			// Most of these checks are of a validation nature where we don't expect anything 
			// to be created, given that a specific set of Components is already existing.
			AssertCreationChain(map, typeof(TestComponentD1),
				null,
				new [] { typeof(TestComponentD6), typeof(TestComponentD4), typeof(TestComponentD2) });
			AssertCreationChain(map, typeof(TestComponentD1),
				new [] { typeof(TestComponentD6), typeof(TestComponentD4), typeof(TestComponentD2) },
				null);
			AssertCreationChain(map, typeof(TestComponentD1),
				new [] { typeof(TestComponentD5), typeof(TestComponentD2) },
				null);
			AssertCreationChain(map, typeof(TestComponentD1),
				new [] { typeof(TestComponentD6), typeof(TestComponentD5), typeof(TestComponentD3) },
				null);

			AssertCreationChain(map, typeof(TestComponentD2),
				null,
				new [] { typeof(TestComponentD6), typeof(TestComponentD4) });
			AssertCreationChain(map, typeof(TestComponentD2),
				new [] { typeof(TestComponentD6), typeof(TestComponentD4) },
				null);
			AssertCreationChain(map, typeof(TestComponentD2),
				new [] { typeof(TestComponentD5) },
				null);

			AssertCreationChainUnsorted(map, typeof(TestComponentD3),
				null,
				new [] { typeof(TestComponentD6), typeof(TestComponentD5) });
			AssertCreationChain(map, typeof(TestComponentD3),
				new [] { typeof(TestComponentD6), typeof(TestComponentD5) },
				null);
			AssertCreationChain(map, typeof(TestComponentD3),
				new [] { typeof(TestComponentD6), typeof(TestComponentD4) },
				null);

			AssertCreationChain(map, typeof(TestComponentD4),
				null,
				new [] { typeof(TestComponentD6) });
			AssertCreationChain(map, typeof(TestComponentD5));
			AssertCreationChain(map, typeof(TestComponentD6));

			// Check partial dependency creation chains where only one part of the requirement list
			// isn't satisfied yet.
			AssertCreationChain(map, typeof(TestComponentD3),
				new [] { typeof(TestComponentD5) },
				new [] { typeof(TestComponentD6) });
			AssertCreationChain(map, typeof(TestComponentD3),
				new [] { typeof(TestComponentD6) },
				new [] { typeof(TestComponentD5) });
		}

		private static void AssertCreationChain(ComponentRequirementMap map, Type newComponent, Type[] existingComponents = null, Type[] expectedChain = null)
		{
			CollectionAssert.AreEqual(
				expectedChain ?? new Type[0], 
				map.GetRequirementsToCreate(
					CreateGameObject(existingComponents ?? new Type[0]), 
					newComponent));
		}
		private static void AssertCreationChainUnsorted(ComponentRequirementMap map, Type newComponent, Type[] existingComponents = null, Type[] expectedChain = null)
		{
			CollectionAssert.AreEquivalent(
				expectedChain ?? new Type[0], 
				map.GetRequirementsToCreate(
					CreateGameObject(existingComponents ?? new Type[0]), 
					newComponent));
		}
		private static GameObject CreateGameObject(Type[] components)
		{
			GameObject obj = new GameObject();
			foreach (Type type in components)
			{
				obj.AddComponent(type);
			}
			return obj;
		}
	}

	namespace RequirementTest
	{
		[RequiredComponent(typeof(TestComponentA2))]
		public class TestComponentA1 : Component { }
		[RequiredComponent(typeof(TestComponentA3))]
		public class TestComponentA2 : Component { }
		public class TestComponentA3 : Component { }

		[RequiredComponent(typeof(TestComponentB1))]
		public class TestComponentB1 : Component { }
		[RequiredComponent(typeof(TestComponentB1))]
		[RequiredComponent(typeof(TestComponentB1))]
		[RequiredComponent(typeof(TestComponentB2))]
		[RequiredComponent(typeof(TestComponentB2))]
		public class TestComponentB2 : Component { }

		[RequiredComponent(typeof(TestComponentC2))]
		public class TestComponentC1 : Component { }
		[RequiredComponent(typeof(TestComponentC3))]
		public class TestComponentC2 : Component { }
		[RequiredComponent(typeof(TestComponentC1))]
		public class TestComponentC3 : Component { }

		
		public interface ITestComponentDX { }
		public interface ITestComponentDY { }

		[RequiredComponent(typeof(ITestComponentDX), typeof(TestComponentD2))]
		public class TestComponentD1 : Component { }

		[RequiredComponent(typeof(ITestComponentDY), typeof(TestComponentD4))]
		public class TestComponentD2 : Component, ITestComponentDX { }
		[RequiredComponent(typeof(ITestComponentDY), typeof(TestComponentD5))]
		[RequiredComponent(typeof(TestComponentD6))]
		public class TestComponentD3 : Component, ITestComponentDX { }

		[RequiredComponent(typeof(TestComponentD6))]
		public class TestComponentD4 : Component, ITestComponentDY { }
		public class TestComponentD5 : Component, ITestComponentDY { }
		public class TestComponentD6 : Component { }
	}
}
