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

using NUnit.Framework;

namespace Duality.Tests.Cloning.HelperObjects
{
	#pragma warning disable 659  // GetHashCode not implemented

	internal class TestStructInvestigate : IEquatable<TestStructInvestigate>
	{
		public struct InvestigateStruct
		{
			public int SomeInt;
			public OwnedObject OwnedObject;
		}

		public InvestigateStruct Data { get; set; }

		private TestStructInvestigate() { }
		public TestStructInvestigate(Random rnd)
		{
			this.Data = new InvestigateStruct { SomeInt = rnd.Next(), OwnedObject = new OwnedObject { TestData = rnd.Next() } };
		}

		public override bool Equals(object obj)
		{
			if (obj is TestStructInvestigate)
				return this.Equals((TestStructInvestigate)obj);
			else
				return base.Equals(obj);
		}
		public bool Equals(TestStructInvestigate other)
		{
			return 
				this.Data.SomeInt == other.Data.SomeInt &&
				!object.ReferenceEquals(this.Data.OwnedObject, other.Data.OwnedObject) &&
				this.Data.OwnedObject.TestData == other.Data.OwnedObject.TestData;
		}
	}
}
