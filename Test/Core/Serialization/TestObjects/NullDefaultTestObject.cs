using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Duality.Serialization;

namespace Duality.Tests.Serialization
{
	public class NullDefaultTestObject : IEquatable<NullDefaultTestObject>
	{
		public int TestField;
		public TestObject ReferenceTypeField;
		public TestData ValueTypeField;

		public static bool operator ==(NullDefaultTestObject first, NullDefaultTestObject second)
		{
			return first.Equals(second);
		}
		public static bool operator !=(NullDefaultTestObject first, NullDefaultTestObject second)
		{
			return !first.Equals(second);
		}
		public override bool Equals(object obj)
		{
			if (obj is NullDefaultTestObject)
				return this.Equals((NullDefaultTestObject)obj);
			else
				return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public bool Equals(NullDefaultTestObject other)
		{
			return 
				other.TestField == this.TestField &&
				object.Equals(other.ReferenceTypeField, this.ReferenceTypeField) &&
				object.Equals(other.ValueTypeField, this.ValueTypeField);
		}
	}
}
