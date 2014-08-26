using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;

using Duality;
using Duality.Cloning;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Renderers;

using OpenTK;
using NUnit.Framework;

namespace Duality.Tests.Cloning.HelperObjects
{
	#pragma warning disable 659  // GetHashCode not implemented

	internal class TestComponent : Component, IEquatable<TestComponent>
	{
		public string TestProperty { get; set; }
		public ReferencedObject TestReference { get; set; }
		public List<ReferencedObject> TestReferenceList { get; set; }
		public GameObject GameObjectReference { get; set; }
		public Component ComponentReference { get; set; }
		
		public TestComponent()
		{
			this.TestProperty = "TestStringA" + CloneProviderTest.SharedRandom.NextByte();
			this.TestReference = new ReferencedObject { TestProperty = "TestStringB" + CloneProviderTest.SharedRandom.NextByte() };
			this.TestReferenceList = new List<ReferencedObject>
			{
				new ReferencedObject { TestProperty = "TestStringC" + CloneProviderTest.SharedRandom.NextByte() },
				new ReferencedObject { TestProperty = "TestStringD" + CloneProviderTest.SharedRandom.NextByte() }
			};
		}

		public override bool Equals(object obj)
		{
			if (obj is TestComponent)
				return this.Equals((TestComponent)obj);
			else
				return base.Equals(obj);
		}
		public bool Equals(TestComponent other)
		{
			return 
				this.TestProperty == other.TestProperty &&
				this.TestReference == other.TestReference &&
				this.TestReferenceList.SequenceEqual(other.TestReferenceList);
		}
	}
}
