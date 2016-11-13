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

	internal struct TestData : IEquatable<TestData>
	{
		public int IntField;
		public float FloatField;

		public TestData(Random rnd)
		{
			this.IntField	= rnd.Next();
			this.FloatField	= rnd.NextFloat();
		}

		public override bool Equals(object obj)
		{
			if (obj is TestData)
				return this.Equals((TestData)obj);
			else
				return base.Equals(obj);
		}
		public bool Equals(TestData other)
		{
			return 
				other.IntField == this.IntField &&
				other.FloatField == this.FloatField;
		}
	}
}
