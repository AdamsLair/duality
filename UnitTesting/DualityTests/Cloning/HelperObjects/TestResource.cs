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

	internal class TestResource : Resource, IEquatable<TestResource>
	{
		public string TestProperty { get; set; }
		public ContentRef<Resource> TestContentRef { get; set; }
		public ReferencedObject TestReference { get; set; }
		public List<ReferencedObject> TestReferenceList { get; set; }
		
		public TestResource(Random rnd)
		{
			this.TestProperty = "TestStringA" + rnd.NextByte();
			this.TestReference = new ReferencedObject { TestProperty = "TestStringB" + rnd.NextByte() };
			this.TestReferenceList = new List<ReferencedObject>
			{
				new ReferencedObject { TestProperty = "TestStringC" + rnd.NextByte() },
				new ReferencedObject { TestProperty = "TestStringD" + rnd.NextByte() }
			};
		}

		public override bool Equals(object obj)
		{
			if (obj is TestResource)
				return this.Equals((TestResource)obj);
			else
				return base.Equals(obj);
		}
		public bool Equals(TestResource other)
		{
			return 
				this.TestProperty == other.TestProperty &&
				this.TestContentRef == other.TestContentRef &&
				this.TestReference == other.TestReference &&
				this.TestReferenceList.SequenceEqual(other.TestReferenceList);
		}
	}
}
